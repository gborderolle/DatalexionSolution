﻿using AutoMapper;
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
    [Route("api/wings")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Analyst")]
    public class WingsController : CustomBaseController<Wing>
    {
        private readonly IWingRepository _wingRepository; // Servicio que contiene la lógica principal de negocio para Wings.
        private readonly IClientRepository _clientRepository;
        private readonly IPartyRepository _partyRepository;
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;
        private readonly IFileStorage _fileStorage;
        private readonly IPhotoRepository _photoRepository; // Servicio que contiene la lógica principal de negocio para Reports.

        public WingsController(ILogger<WingsController> logger, IMapper mapper, IWingRepository wingRepository, ContextDB dbContext, ILogService logService, IFileStorage fileStorage, IPhotoRepository photoRepository, IClientRepository clientRepository, IPartyRepository partyRepository)
        : base(mapper, logger, wingRepository)
        {
            _response = new();
            _wingRepository = wingRepository;
            _dbContext = dbContext;
            _logService = logService;
            _fileStorage = fileStorage;
            _photoRepository = photoRepository;
            _clientRepository = clientRepository;
            _partyRepository = partyRepository;
        }

        #region Endpoints genéricos

        [HttpGet("GetWing")]
        public async Task<ActionResult<APIResponse>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Wing>>
            {
                    new IncludePropertyConfiguration<Wing>
                    {
                        IncludeExpression = b => b.ListSlates
                    },
                     new IncludePropertyConfiguration<Wing>
                    {
                        IncludeExpression = b => b.Photo
                    },
                     new IncludePropertyConfiguration<Wing>
                    {
                        IncludeExpression = b => b.Party
                    }
                };
            return await Get<Wing, WingDTO>(paginationDTO: paginationDTO, includes: includes);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> All()
        {
            var wings = await _wingRepository.GetAll();
            _response.Result = _mapper.Map<List<WingDTO>>(wings);
            _response.StatusCode = HttpStatusCode.OK;
            return _response;
        }

        [HttpGet("{id:int}")] // url completa: https://localhost:7003/api/Wings/1
        public async Task<ActionResult<APIResponse>> Get([FromRoute] int id)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Wing>>
            {
                    new IncludePropertyConfiguration<Wing>
                    {
                        IncludeExpression = b => b.ListSlates
                    },
                     new IncludePropertyConfiguration<Wing>
                    {
                        IncludeExpression = b => b.Photo
                    },
                    new IncludePropertyConfiguration<Wing>
                    {
                        IncludeExpression = b => b.Party
                    }
                };
            return await GetById<Wing, WingDTO>(id, includes: includes);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Wing>(id);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromBody] WingCreateDTO wingCreateDTO)
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
                var includes = new List<IncludePropertyConfiguration<Wing>>
            {
                 new IncludePropertyConfiguration<Wing>
                    {
                        IncludeExpression = b => b.Party
                    },
                };

                var wing = await _wingRepository.Get(v => v.Id == id, includes: includes);
                if (wing == null)
                {
                    _logger.LogError(string.Format(Messages.Wing.NotFound, id), id);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Wing.NotFound, id) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                wing.Party = await _dbContext.Party.FindAsync(wingCreateDTO.PartyId);
                if (wing.Party == null)
                {
                    _logger.LogError(string.Format(Messages.Party.NotFound, wingCreateDTO.PartyId), wingCreateDTO.PartyId);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Party.NotFound, wingCreateDTO.PartyId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // No usar AutoMapper para mapear todo el objeto, sino actualizar campo por campo
                wing.Name = Utils.ToCamelCase(wingCreateDTO.Name);
                wing.Comments = Utils.ToCamelCase(wingCreateDTO.Comments);
                wing.Update = DateTime.Now;

                wing.PartyId = wingCreateDTO.PartyId;
                wing.Party = await _dbContext.Party.FindAsync(wingCreateDTO.PartyId);

                var updatedWing = await _wingRepository.Update(wing);

                _logger.LogInformation(string.Format(Messages.Wing.ActionLog, id, wing.Name, id));
                await _logService.LogAction("Wing", "Update", string.Format(Messages.Wing.ActionLog, id, wing.Name), User.Identity.Name, null);

                _response.Result = _mapper.Map<WingDTO>(updatedWing);
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
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromBody] JsonPatchDocument<WingPatchDTO> patchDto)
        {
            return await Patch<Wing, WingPatchDTO>(id, patchDto);
        }

        #endregion

        #region Endpoints específicos

        [HttpGet("GetWingsByClient")]
        public async Task<ActionResult<APIResponse>> GetWingsByClient([FromQuery] int clientId)
        {
            try
            {
                var includesClient = new List<IncludePropertyConfiguration<Client>>
                {
                    new IncludePropertyConfiguration<Client>
                    {
                        IncludeExpression = b => b.Party
                    },
                };
                var client = await _clientRepository.Get(v => v.Id == clientId, includes: includesClient);
                if (client == null)
                {
                    _logger.LogError(string.Format(Messages.Client.NotFound, clientId), clientId);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Client.NotFound, clientId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                if (client.Party == null)
                {
                    _logger.LogError(string.Format(Messages.Party.NotFound, clientId), clientId);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Party.NotFound, clientId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // 1..n
                var includes = new List<IncludePropertyConfiguration<Wing>>
                {
                    new IncludePropertyConfiguration<Wing>
                    {
                        IncludeExpression = b => b.ListSlates
                    },
                     new IncludePropertyConfiguration<Wing>
                    {
                        IncludeExpression = b => b.Photo
                    },
                    new IncludePropertyConfiguration<Wing>
                    {
                        IncludeExpression = b => b.Party
                    }
                };
                var wings = await _wingRepository.GetAll(w => w.PartyId == client.Party.Id, includes: includes);

                // Mapea los candidatos únicos a DTOs para la respuesta
                _response.Result = _mapper.Map<List<WingDTO>>(wings);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = [ex.ToString()];
                return BadRequest(_response);
            }
        }

        [HttpPost(Name = "CreateWing")]
        public async Task<ActionResult<APIResponse>> Post([FromBody] WingCreateDTO wingCreateDto)
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
                if (await _wingRepository.Get(v => v.Name.ToLower() == wingCreateDto.Name.ToLower()) != null)
                {
                    _logger.LogError($"El nombre {wingCreateDto.Name} ya existe en el sistema");
                    _response.ErrorMessages = new List<string> { $"El nombre {wingCreateDto.Name} ya existe en el sistema." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("NameAlreadyExists", $"El nombre {wingCreateDto.Name} ya existe en el sistema.");
                    return BadRequest(ModelState);
                }

                wingCreateDto.Name = Utils.ToCamelCase(wingCreateDto.Name);
                wingCreateDto.Comments = Utils.ToCamelCase(wingCreateDto.Comments);

                Wing wing = _mapper.Map<Wing>(wingCreateDto);
                wing.Creation = DateTime.Now;
                wing.Update = DateTime.Now;

                wing.ListSlates = _mapper.Map<List<Slate>>(wingCreateDto.ListSlates);

                await _wingRepository.Create(wing);

                // Manejar carga de fotos
                if (wingCreateDto.Photo != null)
                {
                    await HandlePhotoUpload(wingCreateDto.Photo, wing);
                }

                _logger.LogInformation($"Se creó correctamente el wing Id:{wing.Id}.");
                await _logService.LogAction("Wing", "Create", $"Id:{wing.Id}, Nombre: {wing.Name}.", User.Identity.Name, null);

                _response.Result = _mapper.Map<WingDTO>(wing);
                _response.StatusCode = HttpStatusCode.Created;

                // CreatedAtRoute -> Nombre de la ruta (del método): GetCountryDSById
                // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/13816172#notes
                return CreatedAtAction(nameof(Get), new { id = wing.Id }, _response);
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

        /// <summary>
        /// Sube una foto sola.
        /// </summary>
        /// <param name="photoFile"></param>
        /// <param name="wing"></param>
        /// <returns></returns>
        private async Task HandlePhotoUpload(IFormFile photoFile, Wing wing)
        {
            if (photoFile != null)
            {
                string dynamicContainer = $"uploads/wings/wing{wing.Id}";
                var newPhoto = new Photo
                {
                    Wing = wing
                };

                using (var stream = new MemoryStream())
                {
                    await photoFile.CopyToAsync(stream);
                    var content = stream.ToArray();
                    var extension = Path.GetExtension(photoFile.FileName);
                    newPhoto.URL = await _fileStorage.SaveFile(content, extension, dynamicContainer, photoFile.ContentType);
                }
                await _photoRepository.Create(newPhoto);
            }
        }

        #endregion

    }
}

