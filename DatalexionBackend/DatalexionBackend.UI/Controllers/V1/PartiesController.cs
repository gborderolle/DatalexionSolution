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
using DatalexionBackend.Infrastructure.Services;

namespace DatalexionBackend.UI.Controllers.V1
{
    [ApiController]
    [HasHeader("x-version", "1")]
    [Route("api/parties")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Analyst")]
    public class PartiesController : CustomBaseController<Party> // Notice <Party> here
    {
        private readonly IPartyRepository _partyRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;
        private readonly IFileStorage _fileStorage;
        private readonly IPhotoRepository _photoRepository;

        public PartiesController(ILogger<PartiesController> logger, IMapper mapper, IPartyRepository partyRepository, ContextDB dbContext, ILogService logService, IFileStorage fileStorage, IPhotoRepository photoRepository, IClientRepository clientRepository)
        : base(mapper, logger, partyRepository)
        {
            _response = new();
            _partyRepository = partyRepository;
            _dbContext = dbContext;
            _logService = logService;
            _fileStorage = fileStorage;
            _photoRepository = photoRepository;
            _clientRepository = clientRepository;
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

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Party>(id);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromBody] PartyCreateDTO partyCreateDTO)
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

                var party = await _partyRepository.Get(v => v.Id == id);
                if (party == null)
                {
                    _logger.LogError(string.Format(Messages.Party.NotFound, id), id);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Party.NotFound, id) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // No usar AutoMapper para mapear todo el objeto, sino actualizar campo por campo
                party.Name = Utils.ToCamelCase(partyCreateDTO.Name);
                party.Comments = Utils.ToCamelCase(partyCreateDTO.Comments);
                party.Update = DateTime.Now;
                party.ShortName = partyCreateDTO.ShortName;
                party.Color = partyCreateDTO.Color;

                var updatedParty = await _partyRepository.Update(party);

                _logger.LogInformation(string.Format(Messages.Party.ActionLog, id, party.Name), id);
                await _logService.LogAction("Party", "Update", string.Format(Messages.Party.ActionLog, id, party.Name), User.Identity.Name, null);

                _response.Result = _mapper.Map<PartyDTO>(updatedParty);
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
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromBody] JsonPatchDocument<PartyPatchDTO> patchDto)
        {
            return await Patch<Party, PartyPatchDTO>(id, patchDto);
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
                    _logger.LogError(string.Format(Messages.Client.NotFound, clientId), clientId);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Client.NotFound, clientId) };
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
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPost(Name = "CreateParty")]
        public async Task<ActionResult<APIResponse>> Post([FromBody] PartyCreateDTO partyCreateDto)
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
                if (await _partyRepository.Get(v => v.Name.ToLower() == partyCreateDto.Name.ToLower()) != null)
                {
                    _logger.LogError($"El nombre {partyCreateDto.Name} ya existe en el sistema");
                    _response.ErrorMessages = new List<string> { $"El nombre {partyCreateDto.Name} ya existe en el sistema." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("NameAlreadyExists", $"El nombre {partyCreateDto.Name} ya existe en el sistema.");
                    return BadRequest(ModelState);
                }

                partyCreateDto.Name = Utils.ToCamelCase(partyCreateDto.Name);
                partyCreateDto.Comments = Utils.ToCamelCase(partyCreateDto.Comments);

                Party party = _mapper.Map<Party>(partyCreateDto);
                party.Creation = DateTime.Now;
                party.Update = DateTime.Now;

                party.ListCircuitParties = _mapper.Map<List<CircuitParty>>(partyCreateDto.ListCircuitParties);
                party.ListWings = _mapper.Map<List<Wing>>(partyCreateDto.ListWings);

                await _partyRepository.Create(party);

                // Para PhotoLong
                if (partyCreateDto.PhotoLong != null)
                {
                    await HandlePhotoUpload(partyCreateDto.PhotoLong, party, "partiesLong", (photo, p) => photo.PartyLong = p);
                }
                // Para PhotoShort
                if (partyCreateDto.PhotoShort != null)
                {
                    await HandlePhotoUpload(partyCreateDto.PhotoShort, party, "partiesShort", (photo, p) => photo.PartyShort = p);
                }

                _logger.LogInformation($"Se creó correctamente el partido Id:{party.Id}.");
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
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
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

