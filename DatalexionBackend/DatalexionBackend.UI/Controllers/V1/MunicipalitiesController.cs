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
using DatalexionBackend.Core.Enums;
using System.Net;

namespace DatalexionBackend.UI.Controllers.V1
{
    [ApiController]
    [HasHeader("x-version", "1")]
    [Route("api/municipalities")]
    public class MunicipalitiesController : CustomBaseController<Municipality>
    {
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;
        private readonly IMessage<Municipality> _message;

        public MunicipalitiesController(ILogger<MunicipalitiesController> logger, IMapper mapper, IMunicipalityRepository municipalityRepository, ContextDB dbContext, ILogService logService, IMessage<Municipality> message)
        : base(mapper, logger, municipalityRepository)
        {
            _response = new();
            _municipalityRepository = municipalityRepository;
            _dbContext = dbContext;
            _logService = logService;
            _message = message;
        }

        #region Endpoints genéricos

        [HttpGet("GetMunicipality")]
        public async Task<ActionResult<APIResponse>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Municipality>>
            {
                new IncludePropertyConfiguration<Municipality>
                {
                    IncludeExpression = b => b.ListCircuits
                },
                new IncludePropertyConfiguration<Municipality>
                {
                    IncludeExpression = b => b.Delegado
                },
                new IncludePropertyConfiguration<Municipality>
                {
                    IncludeExpression = b => b.Province
                },
                new IncludePropertyConfiguration<Municipality>
                {
                    IncludeExpression = b => b.Delegado
                },
            };
            return await Get<Municipality, MunicipalityDTO>(paginationDTO: paginationDTO, includes: includes);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> All()
        {
            var municipalitys = await _municipalityRepository.GetAll();
            _response.Result = _mapper.Map<List<MunicipalityDTO>>(municipalitys);
            _response.StatusCode = HttpStatusCode.OK;
            return _response;
        }

        [HttpGet("{id:int}")] // url completa: https://localhost:7003/api/Municipalities/1
        public async Task<ActionResult<APIResponse>> Get([FromRoute] int id)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Municipality>>
            {
                new IncludePropertyConfiguration<Municipality>
                {
                    IncludeExpression = b => b.ListCircuits
                },
                new IncludePropertyConfiguration<Municipality>
                {
                    IncludeExpression = b => b.Delegado
                },
                new IncludePropertyConfiguration<Municipality>
                {
                    IncludeExpression = b => b.Province
                },
                new IncludePropertyConfiguration<Municipality>
                {
                    IncludeExpression = b => b.Delegado
                },
            };
            return await GetById<Municipality, MunicipalityDTO>(id, includes: includes);
        }

        [Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Municipality>(id);
        }

        [Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromBody] MunicipalityCreateDTO dto)
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

                // 1..n
                var includes = new List<IncludePropertyConfiguration<Municipality>>
            {
                 new IncludePropertyConfiguration<Municipality>
                    {
                        IncludeExpression = b => b.Province
                    },
                };

                var municipality = await _municipalityRepository.Get(v => v.Id == id, includes: includes);
                if (municipality == null)
                {
                    _logger.LogError(_message.NotFound(id));
                    _response.ErrorMessages = new() { _message.NotFound(id) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                municipality.Province = await _dbContext.Province.FindAsync(dto.ProvinceId);
                if (municipality.Province == null)
                {
                    _logger.LogError(((ProvinceMessage)_message).NotFound(dto.ProvinceId));
                    _response.ErrorMessages = new() { ((ProvinceMessage)_message).NotFound(dto.ProvinceId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // No usar AutoMapper para mapear todo el objeto, sino actualizar campo por campo
                municipality.Name = Utils.ToCamelCase(dto.Name);
                municipality.Comments = Utils.ToCamelCase(dto.Comments);
                municipality.Update = DateTime.Now;

                municipality.ProvinceId = dto.ProvinceId;
                municipality.Province = await _dbContext.Province.FindAsync(dto.ProvinceId);

                var updatedMunicipality = await _municipalityRepository.Update(municipality);

                _logger.LogInformation(_message.ActionLog(id, municipality.Name));
                await _logService.LogAction("Municipality", "Update", _message.ActionLog(id, municipality.Name), User.Identity.Name, null);

                _response.Result = _mapper.Map<MunicipalityDTO>(updatedMunicipality);
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

        [Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromBody] JsonPatchDocument<MunicipalityPatchDTO> dto)
        {
            return await Patch<Municipality, MunicipalityPatchDTO>(id, dto);
        }

        #endregion

        #region Endpoints específicos

        [Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPost(Name = "CreateMunicipality")]
        public async Task<ActionResult<APIResponse>> Post([FromBody] MunicipalityCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"Ocurrió un error en el servidor.");
                    _response.ErrorMessages = new() { $"Ocurrió un error en el servidor." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }
                if (await _municipalityRepository.Get(v => v.Name.ToLower() == dto.Name.ToLower()) != null)
                {
                    _logger.LogError($"El nombre {dto.Name} ya existe en el sistema");
                    _response.ErrorMessages = new() { $"El nombre {dto.Name} ya existe en el sistema." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("NameAlreadyExists", $"El nombre {dto.Name} ya existe en el sistema.");
                    return BadRequest(ModelState);
                }

                dto.Name = Utils.ToCamelCase(dto.Name);
                dto.Comments = Utils.ToCamelCase(dto.Comments);

                Municipality municipality = _mapper.Map<Municipality>(dto);
                municipality.Creation = DateTime.Now;
                municipality.Update = DateTime.Now;

                municipality.ListCircuits = _mapper.Map<List<Circuit>>(dto.ListCircuits);

                await _municipalityRepository.Create(municipality);

                _logger.LogInformation(_message.Created(municipality.Id, municipality.Name));
                await _logService.LogAction("Municipality", "Create", $"Id:{municipality.Id}, Nombre: {municipality.Name}.", User.Identity.Name, null);

                _response.Result = _mapper.Map<MunicipalityDTO>(municipality);
                _response.StatusCode = HttpStatusCode.Created;

                // CreatedAtRoute -> Nombre de la ruta (del método): GetCountryDSById
                // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/13816172#notes
                return CreatedAtAction(nameof(Get), new { id = municipality.Id }, _response);
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

        #endregion

        #region Private methods

        #endregion

    }
}

