using DatalexionBackend.Infrastructure.DbContext;
using AutoMapper;
using DatalexionBackend.Core.DTO;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Datalexion.Controllers.V1
{
    [ApiController]
    [HasHeader("x-version", "1")]
    [Route("api/municipalities")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Analyst")]
    public class MunicipalitiesController : CustomBaseController<Municipality> // Notice <Municipality> here
    {
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;

        public MunicipalitiesController(ILogger<MunicipalitiesController> logger, IMapper mapper, IMunicipalityRepository municipalityRepository, ContextDB dbContext, ILogService logService)
        : base(mapper, logger, municipalityRepository)
        {
            _response = new();
            _municipalityRepository = municipalityRepository;
            _dbContext = dbContext;
            _logService = logService;
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

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Municipality>(id);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromBody] MunicipalityCreateDTO municipalityCreateDTO)
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
                    _logger.LogError(string.Format(Messages.Municipality.NotFound, id), id);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Municipality.NotFound, id) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                municipality.Province = await _dbContext.Province.FindAsync(municipalityCreateDTO.ProvinceId);
                if (municipality.Province == null)
                {
                    _logger.LogError(string.Format(Messages.Province.NotFound, municipalityCreateDTO.ProvinceId), municipalityCreateDTO.ProvinceId);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Province.NotFound, municipalityCreateDTO.ProvinceId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // No usar AutoMapper para mapear todo el objeto, sino actualizar campo por campo
                municipality.Name = Utils.ToCamelCase(municipalityCreateDTO.Name);
                municipality.Comments = Utils.ToCamelCase(municipalityCreateDTO.Comments);
                municipality.Update = DateTime.Now;

                municipality.ProvinceId = municipalityCreateDTO.ProvinceId;
                municipality.Province = await _dbContext.Province.FindAsync(municipalityCreateDTO.ProvinceId);

                var updatedMunicipality = await _municipalityRepository.Update(municipality);

                _logger.LogInformation(string.Format(Messages.Municipality.ActionLog, id, municipality.Name), id);
                await _logService.LogAction("Municipality", "Update", string.Format(Messages.Municipality.ActionLog, id, municipality.Name), User.Identity.Name, null);

                _response.Result = _mapper.Map<MunicipalityDTO>(updatedMunicipality);
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
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromBody] JsonPatchDocument<MunicipalityPatchDTO> patchDto)
        {
            return await Patch<Municipality, MunicipalityPatchDTO>(id, patchDto);
        }

        #endregion

        #region Endpoints específicos

        [HttpPost(Name = "CreateMunicipality")]
        public async Task<ActionResult<APIResponse>> Post([FromBody] MunicipalityCreateDTO municipalityCreateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"Ocurrió un error en el servidor.");
                    _response.ErrorMessages = new List<string> { $"Ocurrió un error en el servidor." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }
                if (await _municipalityRepository.Get(v => v.Name.ToLower() == municipalityCreateDto.Name.ToLower()) != null)
                {
                    _logger.LogError($"El nombre {municipalityCreateDto.Name} ya existe en el sistema");
                    _response.ErrorMessages = new List<string> { $"El nombre {municipalityCreateDto.Name} ya existe en el sistema." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("NameAlreadyExists", $"El nombre {municipalityCreateDto.Name} ya existe en el sistema.");
                    return BadRequest(ModelState);
                }

                var client = _dbContext.Client.FirstOrDefault();
                if (client == null)
                {
                    _logger.LogError($"El cliente no existe en el sistema");
                    _response.ErrorMessages = new List<string> { $"El cliente no existe en el sistema." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("NameAlreadyExists", $"El cliente no existe en el sistema.");
                    return BadRequest(ModelState);
                }

                municipalityCreateDto.Name = Utils.ToCamelCase(municipalityCreateDto.Name);
                municipalityCreateDto.Comments = Utils.ToCamelCase(municipalityCreateDto.Comments);

                Municipality municipality = _mapper.Map<Municipality>(municipalityCreateDto);
                municipality.Creation = DateTime.Now;
                municipality.Update = DateTime.Now;

                municipality.ListCircuits = _mapper.Map<List<Circuit>>(municipalityCreateDto.ListCircuits);

                await _municipalityRepository.Create(municipality);

                _logger.LogInformation($"Se creó correctamente el municipality Id:{municipality.Id}.");
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
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        #endregion

        #region Private methods

        #endregion

    }
}

