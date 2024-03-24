using AutoMapper;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Core.DTO;
using DatalexionBackend.Core.Helpers;
using DatalexionBackend.Infrastructure.DbContext;
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
    public class DelegadosController : CustomBaseController<Delegado> // Notice <Delegate> here
    {
        private readonly IDelegadoRepository _delegadoRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;

        public DelegadosController(ILogger<DelegadosController> logger, IMapper mapper, IDelegadoRepository delegateRepository, ContextDB dbContext, ILogService logService, IClientRepository clientRepository)
        : base(mapper, logger, delegateRepository)
        {
            _response = new();
            _delegadoRepository = delegateRepository;
            _dbContext = dbContext;
            _logService = logService;
            _clientRepository = clientRepository;
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
        public async Task<ActionResult<APIResponse>> Put(int id, [FromBody] DelegadoCreateDTO delegateCreateDTO)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError(Messages.Generic.NotValid);
                    _response.ErrorMessages = new List<string> { Messages.Generic.NotValid };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var delegado = await _delegadoRepository.Get(v => v.Id == id);
                if (delegado == null)
                {
                    _logger.LogError(string.Format(Messages.Delegados.NotFound, id), id);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Delegados.NotFound, id) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // No usar AutoMapper para mapear todo el objeto, sino actualizar campo por campo
                delegado.Name = Utils.ToCamelCase(delegateCreateDTO.Name);
                delegado.Comments = Utils.ToCamelCase(delegateCreateDTO.Comments);
                delegado.Update = DateTime.Now;

                var updatedDelegado = await _delegadoRepository.Update(delegado);

                _logger.LogInformation(string.Format(Messages.Delegados.ActionLog, id, delegado.Name), id);
                await _logService.LogAction("Delegado", "Update", string.Format(Messages.Delegados.ActionLog, id, delegado.Name), User.Identity.Name, null);

                _response.Result = _mapper.Map<DelegadoDTO>(updatedDelegado);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromBody] JsonPatchDocument<DelegadoPatchDTO> patchDto)
        {
            return await Patch<Delegado, DelegadoPatchDTO>(id, patchDto);
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
                    _logger.LogError(string.Format(Messages.Client.NotFound, clientId), clientId);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Client.NotFound, clientId) };
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
                var thenIncludes = new List<ThenIncludePropertyConfiguration<Delegado>>
                {
                    // circuitos
                    new ThenIncludePropertyConfiguration<Delegado>
                    {
                        IncludeExpression = b => b.ListCircuitDelegados,
                        ThenIncludeExpression = ab => ((CircuitDelegado)ab).Delegado
                    },
                };
                var delegadoList = await _delegadoRepository.GetAll(v => v.ClientId == clientId, includes: includes, thenIncludes: thenIncludes);

                _response.Result = _mapper.Map<List<DelegadoDTO>>(delegadoList);
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPost(Name = "CreateDelegado")]
        public async Task<ActionResult<APIResponse>> Post([FromBody] DelegadoCreateDTO delegateCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(Messages.Generic.InternalError);
                    _response.ErrorMessages = new List<string> { Messages.Generic.InternalError };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }
                if (await _delegadoRepository.Get(v => v.Name.ToLower() == delegateCreateDto.Name.ToLower()) != null)
                {
                    _logger.LogError(Messages.Generic.NameAlreadyExists, delegateCreateDto.Name);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Generic.NameAlreadyExists, delegateCreateDto.Name) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("NameAlreadyExists", string.Format(Messages.Generic.NameAlreadyExists, delegateCreateDto.Name));
                    return BadRequest(ModelState);
                }

                var client = _dbContext.Client.FirstOrDefault();
                if (client == null)
                {
                    _logger.LogError(Messages.Client.NotFoundGeneric);
                    _response.ErrorMessages = new List<string> { Messages.Client.NotFoundGeneric };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("NotFound", Messages.Client.NotFoundGeneric);
                    return BadRequest(ModelState);
                }

                delegateCreateDto.Name = Utils.ToCamelCase(delegateCreateDto.Name);
                delegateCreateDto.Comments = Utils.ToCamelCase(delegateCreateDto.Comments);

                Delegado delegado = _mapper.Map<Delegado>(delegateCreateDto);
                delegado.Creation = DateTime.Now;
                delegado.Update = DateTime.Now;

                delegado.ListMunicipalities = _mapper.Map<List<Municipality>>(delegateCreateDto.ListMunicipalities);
                delegado.Client = client;
                delegado.ClientId = client.Id;

                await _delegadoRepository.Create(delegado);

                _logger.LogInformation(string.Format(Messages.Delegados.Created), delegado.Id);
                await _logService.LogAction("Delegado", "Create", string.Format(Messages.Delegados.ActionLog, delegado.Id, delegado.Name), User.Identity.Name, null);

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
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        #endregion

        #region Private methods

        #endregion

    }
}

