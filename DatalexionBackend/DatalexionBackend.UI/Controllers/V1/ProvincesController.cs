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
    [Route("api/provinces")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Analyst")]
    public class ProvincesController : CustomBaseController<Province>
    {
        private readonly IProvinceRepository _provinceRepository; // Servicio que contiene la lógica principal de negocio para Provinces.
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;

        public ProvincesController(ILogger<ProvincesController> logger, IMapper mapper, IProvinceRepository provinceRepository, ContextDB dbContext, ILogService logService)
        : base(mapper, logger, provinceRepository)
        {
            _response = new();
            _provinceRepository = provinceRepository;
            _dbContext = dbContext;
            _logService = logService;
        }

        #region Endpoints genéricos

        [HttpGet("GetProvince")]
        public async Task<ActionResult<APIResponse>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Province>>
            {
                    new IncludePropertyConfiguration<Province>
                    {
                        IncludeExpression = b => b.ListMunicipalities
                    },
                     new IncludePropertyConfiguration<Province>
                    {
                        IncludeExpression = b => b.ListSlates
                    }
                };
            return await Get<Province, ProvinceDTO>(paginationDTO: paginationDTO, includes: includes);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> All()
        {
            var provinces = await _provinceRepository.GetAll();
            _response.Result = _mapper.Map<List<ProvinceDTO>>(provinces);
            _response.StatusCode = HttpStatusCode.OK;
            return _response;
        }

        [HttpGet("{id:int}")] // url completa: https://localhost:7003/api/Provinces/1
        public async Task<ActionResult<APIResponse>> Get([FromRoute] int id)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Province>>
            {
                    new IncludePropertyConfiguration<Province>
                    {
                        IncludeExpression = b => b.ListMunicipalities
                    },
                     new IncludePropertyConfiguration<Province>
                    {
                        IncludeExpression = b => b.ListSlates
                    }
                };
            return await GetById<Province, ProvinceDTO>(id, includes: includes);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Province>(id);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromBody] ProvinceCreateDTO provinceCreateDTO)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError(Messages.Generic.NotValid);
                    _response.ErrorMessages = new() { Messages.Generic.NotValid };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var province = await _provinceRepository.Get(v => v.Id == id);
                if (province == null)
                {
                    _logger.LogError(string.Format(Messages.Province.NotFound, id));
                    _response.ErrorMessages = new() { string.Format(Messages.Province.NotFound, id) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // No usar AutoMapper para mapear todo el objeto, sino actualizar campo por campo
                province.Name = Utils.ToCamelCase(provinceCreateDTO.Name);
                province.Comments = Utils.ToCamelCase(provinceCreateDTO.Comments);
                province.Update = DateTime.Now;

                var updatedProvince = await _provinceRepository.Update(province);

                _logger.LogInformation(string.Format(Messages.Party.ActionLog, id, province.Name, id));
                await _logService.LogAction("Province", "Update", string.Format(Messages.Province.ActionLog, id, province.Name), User.Identity.Name, null);

                _response.Result = _mapper.Map<ProvinceDTO>(updatedProvince);
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
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromBody] JsonPatchDocument<ProvincePatchDTO> patchDto)
        {
            return await Patch<Province, ProvincePatchDTO>(id, patchDto);
        }

        #endregion

        #region Endpoints específicos

        [HttpPost(Name = "CreateProvince")]
        public async Task<ActionResult<APIResponse>> Post([FromBody] ProvinceCreateDTO provinceCreateDto)
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
                if (await _provinceRepository.Get(v => v.Name.ToLower() == provinceCreateDto.Name.ToLower()) != null)
                {
                    _logger.LogError($"El nombre {provinceCreateDto.Name} ya existe en el sistema");
                    _response.ErrorMessages = new() { $"El nombre {provinceCreateDto.Name} ya existe en el sistema." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("NameAlreadyExists", $"El nombre {provinceCreateDto.Name} ya existe en el sistema.");
                    return BadRequest(ModelState);
                }

                provinceCreateDto.Name = Utils.ToCamelCase(provinceCreateDto.Name);
                provinceCreateDto.Comments = Utils.ToCamelCase(provinceCreateDto.Comments);

                Province province = _mapper.Map<Province>(provinceCreateDto);
                province.Creation = DateTime.Now;
                province.Update = DateTime.Now;

                province.ListMunicipalities = provinceCreateDto.ListMunicipalities;
                province.ListSlates = provinceCreateDto.ListSlates;

                await _provinceRepository.Create(province);

                _logger.LogInformation($"Se creó correctamente el province Id:{province.Id}.");
                await _logService.LogAction("Province", "Create", $"Id:{province.Id}, Nombre: {province.Name}.", User.Identity.Name, null);

                _response.Result = _mapper.Map<ProvinceDTO>(province);
                _response.StatusCode = HttpStatusCode.Created;

                // CreatedAtRoute -> Nombre de la ruta (del método): GetCountryDSById
                // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/13816172#notes
                return CreatedAtAction(nameof(Get), new { id = province.Id }, _response);
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

