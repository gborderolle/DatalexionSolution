using AutoMapper;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Core.DTO;
using DatalexionBackend.Core.Helpers;
using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Infrastructure.MessagesService;
using DatalexionBackend.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DatalexionBackend.UI.Controllers.V1
{
    [ApiController]
    [HasHeader("x-version", "1")]
    [Route("api/delegados")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Analyst")]
    public class DelegadosController : CustomBaseController<Delegado>
    {
        private readonly IDelegadoRepository _delegadoRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;
        private readonly IMessage<Delegado> _message;

        public DelegadosController(ILogger<DelegadosController> logger, IMapper mapper, IDelegadoRepository delegateRepository, ContextDB dbContext, ILogService logService, IClientRepository clientRepository, IMunicipalityRepository municipalityRepository, IMessage<Delegado> message)
        : base(mapper, logger, delegateRepository)
        {
            _response = new();
            _delegadoRepository = delegateRepository;
            _dbContext = dbContext;
            _logService = logService;
            _clientRepository = clientRepository;
            _municipalityRepository = municipalityRepository;
            _message = message;
        }

        #region Endpoints genéricos

        [HttpGet("GetDelegado")]
        public async Task<ActionResult<APIResponse>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Delegado>>
            {
                    new IncludePropertyConfiguration<Delegado>
                    {
                        IncludeExpression = b => b.ListMunicipalities
                    }
                };
            // n..n
            var thenIncludes = new List<ThenIncludePropertyConfiguration<Delegado>>
            {
                // delegados
                new ThenIncludePropertyConfiguration<Delegado>
                {
                    IncludeExpression = b => b.ListCircuitDelegados,
                    ThenIncludeExpression = ab => ((CircuitDelegado)ab).Circuit
                },
            };
            return await Get<Delegado, DelegadoDTO>(paginationDTO: paginationDTO, includes: includes, thenIncludes: thenIncludes);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> All()
        {
            var delegates = await _delegadoRepository.GetAll();
            _response.Result = _mapper.Map<List<DelegadoDTO>>(delegates);
            _response.StatusCode = HttpStatusCode.OK;
            return _response;
        }

        [HttpGet("{id:int}")] // url completa: https://localhost:7003/api/Delegate1s/1
        public async Task<ActionResult<APIResponse>> Get([FromRoute] int id)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Delegado>>
            {
                new IncludePropertyConfiguration<Delegado>
                {
                    IncludeExpression = b => b.ListMunicipalities
                }
            };
            // n..n
            var thenIncludes = new List<ThenIncludePropertyConfiguration<Delegado>>
            {
                // circuitos
                new ThenIncludePropertyConfiguration<Delegado>
                {
                    IncludeExpression = b => b.ListCircuitDelegados,
                    ThenIncludeExpression = ab => ((CircuitDelegado)ab).Delegado
                },
            };
            return await GetById<Delegate, DelegadoDTO>(id, includes: includes, thenIncludes: thenIncludes);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Delegado>(id);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromBody] DelegadoCreateDTO dto)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError(_message.NotValid());
                    _response.ErrorMessages = new() { _message.NotValid() };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var delegado = await _delegadoRepository.Get(v => v.Id == id);
                if (delegado == null)
                {
                    _logger.LogError(_message.NotFound(id));
                    _response.ErrorMessages = new() { _message.NotFound(id) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // No usar AutoMapper para mapear todo el objeto, sino actualizar campo por campo
                delegado.Name = Utils.ToCamelCase(dto.Name);
                delegado.Comments = Utils.ToCamelCase(dto.Comments);
                delegado.Update = DateTime.Now;

                var updatedDelegado = await _delegadoRepository.Update(delegado);

                _logger.LogInformation(_message.ActionLog(id, delegado.Name));
                await _logService.LogAction("Delegado", "Update", _message.ActionLog(id, delegado.Name), User.Identity.Name, null);

                _response.Result = _mapper.Map<DelegadoDTO>(updatedDelegado);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = [ex.ToString()];
            }
            return BadRequest(_response);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromBody] JsonPatchDocument<DelegadoPatchDTO> dto)
        {
            return await Patch<Delegado, DelegadoPatchDTO>(id, dto);
        }

        #endregion

        #region Endpoints específicos

        [HttpGet("GetDelegadosByClient")]
        public async Task<ActionResult<APIResponse>> GetDelegadosByClient([FromQuery] int clientId)
        {
            try
            {
                var client = await _clientRepository.Get(v => v.Id == clientId);
                if (client == null)
                {
                    _logger.LogError(_message.ClientNotFound(clientId), clientId);
                    _response.ErrorMessages = new() { _message.ClientNotFound(clientId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // 1..n
                var includes = new List<IncludePropertyConfiguration<Delegado>>
                {
                        new IncludePropertyConfiguration<Delegado>
                        {
                            IncludeExpression = b => b.ListMunicipalities
                        }
                    };
                // n..n
                //var thenIncludes = new List<ThenIncludePropertyConfiguration<Delegado>>
                //{
                //    // circuitos
                //    new ThenIncludePropertyConfiguration<Delegado>
                //    {
                //        IncludeExpression = b => b.ListCircuitDelegados,
                //        ThenIncludeExpression = ab => ((CircuitDelegado)ab).Delegado
                //    },
                //};
                //var delegadoList = await _delegadoRepository.GetAll(v => v.ClientId == clientId, includes: includes, thenIncludes: thenIncludes);
                var delegadoList = await _delegadoRepository.GetAll(v => v.ClientId == clientId, includes: includes);

                _response.Result = _mapper.Map<List<DelegadoDTO>>(delegadoList);
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = [ex.ToString()];
            }
            return BadRequest(_response);
        }

        [HttpPost(Name = "CreateDelegado")]
        public async Task<ActionResult<APIResponse>> Post([FromBody] DelegadoCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(_message.InternalError());
                    _response.ErrorMessages = new() { _message.InternalError() };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }
                if (await _delegadoRepository.Get(v => v.Name.ToLower() == dto.Name.ToLower()) != null)
                {
                    _logger.LogError(_message.NameAlreadyExists(dto.Name), dto.Name);
                    _response.ErrorMessages = new() { _message.NameAlreadyExists(dto.Name) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("NameAlreadyExists", _message.NameAlreadyExists(dto.Name));
                    return BadRequest(ModelState);
                }

                var client = _dbContext.Client.Where(c => c.Id == dto.ClientId).FirstOrDefault();
                if (client == null)
                {
                    _logger.LogError(_message.ClientNotFound(dto.ClientId));
                    _response.ErrorMessages = new() { _message.ClientNotFound(dto.ClientId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("NotFound", _message.ClientNotFound(dto.ClientId));
                    return BadRequest(ModelState);
                }

                dto.Name = Utils.ToCamelCase(dto.Name);
                dto.Comments = Utils.ToCamelCase(dto.Comments);

                Delegado delegado = _mapper.Map<Delegado>(dto);
                delegado.Creation = DateTime.Now;
                delegado.Update = DateTime.Now;

                //
                delegado.ListMunicipalities = _dbContext.Municipality.Where(m => dto.MunicipalityIds.Contains(m.Id)).ToList();
                delegado.Client = client;
                delegado.ClientId = client.Id;
                //  

                await _delegadoRepository.Create(delegado);

                _logger.LogInformation(_message.Created(delegado.Id, delegado.Name));
                await _logService.LogAction("Delegado", "Create", _message.ActionLog(delegado.Id, delegado.Name), User.Identity.Name, null);

                _response.Result = _mapper.Map<DelegadoDTO>(delegado);
                _response.StatusCode = HttpStatusCode.Created;

                // CreatedAtRoute -> Nombre de la ruta (del método): GetCountryDSById
                // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/13816172#notes
                return CreatedAtAction(nameof(Get), new { id = delegado.Id }, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = [ex.ToString()];
            }
            return _response;
        }

        [HttpGet("IsCIAlreadyRegistered")]
        public async Task<IActionResult> IsCIAlreadyRegistered(string ci)
        {
            var exists = await _delegadoRepository.Exists(d => d.CI == ci);
            return Ok(!exists);
        }

        #endregion

        #region Private methods

        #endregion

    }
}

