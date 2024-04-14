using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Infrastructure.MessagesService;
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
using DatalexionBackend.Core.Enums;
using DatalexionBackend.Infrastructure.Services;

namespace DatalexionBackend.UI.Controllers.V1
{
    [ApiController]
    [HasHeader("x-version", "1")]
    [Route("api/parties")]
    public class PartiesController : CustomBaseController<Party>
    {
        private readonly IPartyRepository _partyRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;
        private readonly IFileStorage _fileStorage;
        private readonly IPhotoRepository _photoRepository;
        private readonly IMessage<Party> _message;

        public PartiesController(ILogger<PartiesController> logger, IMapper mapper, IPartyRepository partyRepository, ContextDB dbContext, ILogService logService, IFileStorage fileStorage, IPhotoRepository photoRepository, IClientRepository clientRepository, IMessage<Party> message)
        : base(mapper, logger, partyRepository)
        {
            _response = new();
            _partyRepository = partyRepository;
            _dbContext = dbContext;
            _logService = logService;
            _fileStorage = fileStorage;
            _photoRepository = photoRepository;
            _clientRepository = clientRepository;
            _message = message;
        }

        #region Endpoints genéricos

        [HttpGet("GetParty")]
        public async Task<ActionResult<APIResponse>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Party>>
            {
                    new IncludePropertyConfiguration<Party>
                    {
                        IncludeExpression = b => b.ListWings
                    },
                    new IncludePropertyConfiguration<Party>
                    {
                        IncludeExpression = b => b.PhotoLong
                    },
                    new IncludePropertyConfiguration<Party>
                    {
                        IncludeExpression = b => b.PhotoShort
                    },
                };
            // n..n
            var thenIncludes = new List<ThenIncludePropertyConfiguration<Party>>
            {
                // wings->slates
                new ThenIncludePropertyConfiguration<Party>
                {
                    IncludeExpression = b => b.ListWings,
                    ThenIncludeExpression = ab => ((Wing)ab).ListSlates
                },
                new ThenIncludePropertyConfiguration<Party>
                {
                    IncludeExpression = b => b.ListCircuitParties,
                    ThenIncludeExpression = ab => ((CircuitParty)ab).Circuit
                },
            };
            return await Get<Party, PartyDTO>(paginationDTO: paginationDTO, includes: includes, thenIncludes: thenIncludes);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> All()
        {
            var parties = await _partyRepository.GetAll();
            _response.Result = _mapper.Map<List<PartyDTO>>(parties);
            _response.StatusCode = HttpStatusCode.OK;
            return _response;
        }

        [HttpGet("{id:int}")] // url completa: https://localhost:7003/api/Parties/1
        public async Task<ActionResult<APIResponse>> Get([FromRoute] int id)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Party>>
            {
                     new IncludePropertyConfiguration<Party>
                    {
                        IncludeExpression = b => b.ListWings
                    },
                    new IncludePropertyConfiguration<Party>
                    {
                        IncludeExpression = b => b.PhotoLong
                    },
                    new IncludePropertyConfiguration<Party>
                    {
                        IncludeExpression = b => b.PhotoShort
                    },
                };
            // n..n
            var thenIncludes = new List<ThenIncludePropertyConfiguration<Party>>
            {
                // wings->slates
                new ThenIncludePropertyConfiguration<Party>
                {
                    IncludeExpression = b => b.ListWings,
                    ThenIncludeExpression = ab => ((Wing)ab).ListSlates
                },
                new ThenIncludePropertyConfiguration<Party>
                {
                    IncludeExpression = b => b.ListCircuitParties,
                    ThenIncludeExpression = ab => ((CircuitParty)ab).Circuit
                },
            };
            return await GetById<Party, PartyDTO>(id, includes: includes, thenIncludes: thenIncludes);
        }

        [Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Party>(id);
        }

        // [Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromForm] PartyCreateDTO dto, IFormFile fileLong, IFormFile fileShort)
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

                var party = await _partyRepository.Get(v => v.Id == id);
                if (party == null)
                {
                    _logger.LogError(_message.NotFound(id));
                    _response.ErrorMessages = new() { _message.NotFound(id) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // No usar AutoMapper para mapear todo el objeto, sino actualizar campo por campo
                party.Name = Utils.ToCamelCase(dto.Name);
                party.Comments = Utils.ToCamelCase(dto.Comments);
                party.Update = DateTime.Now;
                party.ShortName = dto.ShortName;
                party.Color = dto.Color;

                var updatedParty = await _partyRepository.Update(party);

                _logger.LogInformation(_message.ActionLog(id, party.Name));
                await _logService.LogAction("Party", "Update", _message.ActionLog(id, party.Name), User.Identity.Name, null);

                _response.Result = _mapper.Map<PartyDTO>(updatedParty);
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

        // [Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromBody] JsonPatchDocument<PartyPatchDTO> dto)
        {
            return new APIResponse { StatusCode = HttpStatusCode.NotImplemented };
        }

        [Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<APIResponse>> Post([FromForm] PartyCreateDTO dto, IFormFile fileLong, IFormFile fileShort)
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
                if (await _partyRepository.Get(v => v.Name.ToLower() == dto.Name.ToLower()) != null)
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

                Party party = _mapper.Map<Party>(dto);
                party.Creation = DateTime.Now;
                party.Update = DateTime.Now;

                party.ListCircuitParties = _mapper.Map<List<CircuitParty>>(dto.ListCircuitParties);
                party.ListWings = _mapper.Map<List<Wing>>(dto.ListWings);

                await _partyRepository.Create(party);

                // Para PhotoLong
                if (dto.PhotoLong != null)
                {
                    await HandlePhotoUpload(dto.PhotoLong, party, "partiesLong", (photo, p) => photo.PartyLong = p);
                }
                // Para PhotoShort
                if (dto.PhotoShort != null)
                {
                    await HandlePhotoUpload(dto.PhotoShort, party, "partiesShort", (photo, p) => photo.PartyShort = p);
                }

                _logger.LogInformation(_message.Created(party.Id, party.Name));
                await _logService.LogAction("Party", "Create", $"Id:{party.Id}, Nombre: {party.Name}.", User.Identity.Name, null);

                _response.Result = _mapper.Map<PartyDTO>(party);
                _response.StatusCode = HttpStatusCode.Created;

                // CreatedAtRoute -> Nombre de la ruta (del método): GetCountryDSById
                // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/13816172#notes
                return CreatedAtAction(nameof(Get), new { id = party.Id }, _response);
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

        #region Endpoints específicos

        [HttpGet("GetPartiesByClient")]
        public async Task<ActionResult<APIResponse>> GetPartiesByClient([FromQuery] int clientId)
        {
            try
            {
                var client = await _clientRepository.Get(v => v.Id == clientId);
                if (client == null)
                {
                    _logger.LogError(((ClientMessage)_message).NotFound(clientId), clientId);
                    _response.ErrorMessages = new() { ((ClientMessage)_message).NotFound(clientId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // 1..n
                var includes = new List<IncludePropertyConfiguration<Party>>
                {
                        new IncludePropertyConfiguration<Party>
                        {
                            IncludeExpression = b => b.ListWings
                        },
                        new IncludePropertyConfiguration<Party>
                        {
                            IncludeExpression = b => b.PhotoLong
                        },
                        new IncludePropertyConfiguration<Party>
                        {
                            IncludeExpression = b => b.PhotoShort
                        },
                    };
                // n..n
                var thenIncludes = new List<ThenIncludePropertyConfiguration<Party>>
                {
                    // wings->slates
                    new ThenIncludePropertyConfiguration<Party>
                    {
                        IncludeExpression = b => b.ListWings,
                        ThenIncludeExpression = ab => ((Wing)ab).ListSlates
                    },
                    new ThenIncludePropertyConfiguration<Party>
                    {
                        IncludeExpression = b => b.ListCircuitParties,
                        ThenIncludeExpression = ab => ((CircuitParty)ab).Circuit
                    },
                };
                var partyList = await _partyRepository.GetAll(v => v.Id == client.PartyId, includes: includes, thenIncludes: thenIncludes);

                _response.Result = _mapper.Map<List<PartyDTO>>(partyList);
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

        #endregion

        #region Private methods

        /// <summary>
        /// Sube una foto sola.
        /// Uso de delegados para determinar el tipo dinámicamente.
        /// </summary>
        /// <param name="photoFile"></param>
        /// <param name="party"></param>
        /// <param name="containerName"></param>
        /// <param name="setPartyPhotoAction"></param>
        /// <returns></returns>
        private async Task HandlePhotoUpload(IFormFile photoFile, Party party, string containerName, Action<Photo, Party> setPartyPhotoAction)
        {
            if (photoFile != null)
            {
                string dynamicContainer = $"uploads/{containerName}/party{party.Id}";
                var newPhoto = new Photo();
                setPartyPhotoAction(newPhoto, party); // Establece la propiedad adecuada de la entidad Photo.

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

