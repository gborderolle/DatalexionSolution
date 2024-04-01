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
    //[Authorize(Roles = nameof(UserTypeOptions.Admin) + "," + nameof(UserTypeOptions.Analyst))]
    [ApiController]
    [HasHeader("x-version", "1")]
    [Route("api/slates")]
    public class SlatesController : CustomBaseController<Slate>
    {
        private readonly ISlateRepository _slateRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IPartyRepository _partyRepository;
        private readonly IWingRepository _wingRepository;
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;
        private readonly IFileStorage _fileStorage;
        private readonly IPhotoRepository _photoRepository;
        private readonly IMessage<Slate> _message;

        public SlatesController(ILogger<SlatesController> logger, IMapper mapper, ISlateRepository slateRepository, ContextDB dbContext, ILogService logService, IFileStorage fileStorage, IPhotoRepository photoRepository, IClientRepository clientRepository, IPartyRepository partyRepository, IWingRepository wingRepository, IMessage<Slate> message)
        : base(mapper, logger, slateRepository)
        {
            _response = new();
            _slateRepository = slateRepository;
            _dbContext = dbContext;
            _logService = logService;
            _fileStorage = fileStorage;
            _photoRepository = photoRepository;
            _clientRepository = clientRepository;
            _partyRepository = partyRepository;
            _wingRepository = wingRepository;
            _message = message;
        }

        #region Endpoints genéricos

        [HttpGet("GetSlate")]
        public async Task<ActionResult<APIResponse>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Slate>>
            {
                    new IncludePropertyConfiguration<Slate>
                    {
                        IncludeExpression = b => b.ListParticipants
                    },
                     new IncludePropertyConfiguration<Slate>
                    {
                        IncludeExpression = b => b.Candidate
                    },
                    new IncludePropertyConfiguration<Slate>
                    {
                        IncludeExpression = b => b.Photo
                    },
                    new IncludePropertyConfiguration<Slate>
                    {
                        IncludeExpression = b => b.Wing
                    },
                    new IncludePropertyConfiguration<Slate>
                    {
                        IncludeExpression = b => b.Province
                    },
                };
            // n..n
            var thenIncludes = new List<ThenIncludePropertyConfiguration<Slate>>
            {
                new ThenIncludePropertyConfiguration<Slate>
                {
                    IncludeExpression = b => b.ListCircuitSlates,
                    ThenIncludeExpression = ab => ((CircuitSlate)ab).Circuit
                },
            };
            return await Get<Slate, SlateDTO>(paginationDTO: paginationDTO, includes: includes, thenIncludes: thenIncludes);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> All()
        {
            var slates = await _slateRepository.GetAll();
            _response.Result = _mapper.Map<List<SlateDTO>>(slates);
            _response.StatusCode = HttpStatusCode.OK;
            return _response;
        }

        [HttpGet("{id:int}")] // url completa: https://localhost:7003/api/Slates/1
        public async Task<ActionResult<APIResponse>> Get([FromRoute] int id)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Slate>>
            {
                    new IncludePropertyConfiguration<Slate>
                    {
                        IncludeExpression = b => b.ListParticipants
                    },
                     new IncludePropertyConfiguration<Slate>
                    {
                        IncludeExpression = b => b.Candidate
                    },
                    new IncludePropertyConfiguration<Slate>
                    {
                        IncludeExpression = b => b.Photo
                    },
                    new IncludePropertyConfiguration<Slate>
                    {
                        IncludeExpression = b => b.Wing
                    },
                    new IncludePropertyConfiguration<Slate>
                    {
                        IncludeExpression = b => b.Province
                    },
                };
            // n..n
            var thenIncludes = new List<ThenIncludePropertyConfiguration<Slate>>
            {
                new ThenIncludePropertyConfiguration<Slate>
                {
                    IncludeExpression = b => b.ListCircuitSlates,
                    ThenIncludeExpression = ab => ((CircuitSlate)ab).Circuit
                },
            };
            return await GetById<Slate, SlateDTO>(id, includes: includes, thenIncludes: thenIncludes);
        }

        //[Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Slate>(id);
        }

        //[Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromBody] SlateCreateDTO dto)
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
                var includes = new List<IncludePropertyConfiguration<Slate>>
                    {
                        new IncludePropertyConfiguration<Slate>
                        {
                            IncludeExpression = b => b.ListParticipants
                        },
                        new IncludePropertyConfiguration<Slate>
                        {
                            IncludeExpression = b => b.Candidate
                        },
                        new IncludePropertyConfiguration<Slate>
                        {
                            IncludeExpression = b => b.Photo
                        },
                        new IncludePropertyConfiguration<Slate>
                        {
                            IncludeExpression = b => b.Wing
                        },
                        new IncludePropertyConfiguration<Slate>
                        {
                            IncludeExpression = b => b.Province
                        },
                    };
                // n..n
                var thenIncludes = new List<ThenIncludePropertyConfiguration<Slate>>
                {
                    new ThenIncludePropertyConfiguration<Slate>
                    {
                        IncludeExpression = b => b.ListCircuitSlates,
                        ThenIncludeExpression = ab => ((CircuitSlate)ab).Circuit
                    },
                };
                var slate = await _slateRepository.Get(v => v.Id == id, includes: includes, thenIncludes: thenIncludes);
                if (slate == null)
                {
                    _logger.LogError(_message.NotFound(id));
                    _response.ErrorMessages = new() { _message.NotFound(id) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                slate.Candidate = await _dbContext.Candidate.FindAsync(dto.CandidateId.Value);
                if (slate.Candidate == null)
                {
                    _logger.LogError(((CandidateMessage)_message).NotFound(dto.CandidateId.Value), dto.CandidateId.Value);
                    _response.ErrorMessages = new() { ((CandidateMessage)_message).NotFound(dto.CandidateId.Value) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                slate.Wing = await _dbContext.Wing.FindAsync(dto.WingId);
                if (slate.Wing == null)
                {
                    _logger.LogError(((WingMessage)_message).NotFound(dto.WingId), dto.WingId);
                    _response.ErrorMessages = new() { ((WingMessage)_message).NotFound(dto.WingId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                slate.Province = await _dbContext.Province.FindAsync(dto.ProvinceId);
                if (slate.Province == null)
                {
                    _logger.LogError(((ProvinceMessage)_message).NotFound(dto.ProvinceId), dto.ProvinceId);
                    _response.ErrorMessages = new() { ((ProvinceMessage)_message).NotFound(dto.ProvinceId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // No usar AutoMapper para mapear todo el objeto, sino actualizar campo por campo
                slate.Name = Utils.ToCamelCase(dto.Name);
                slate.Comments = Utils.ToCamelCase(dto.Comments);
                slate.Update = DateTime.Now;

                slate.CandidateId = dto.CandidateId;
                slate.Candidate = await _dbContext.Candidate.FindAsync(dto.CandidateId);

                slate.WingId = dto.WingId;
                slate.Wing = await _dbContext.Wing.FindAsync(dto.WingId);

                slate.ProvinceId = dto.ProvinceId;
                slate.Province = await _dbContext.Province.FindAsync(dto.ProvinceId);

                var updatedSlate = await _slateRepository.Update(slate);

                _logger.LogInformation(_message.ActionLog(id, slate.Name));
                await _logService.LogAction("Slate", "Update", _message.ActionLog(id, slate.Name), User.Identity.Name, null);

                _response.Result = _mapper.Map<SlateDTO>(updatedSlate);
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
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromBody] JsonPatchDocument<SlatePatchDTO> dto)
        {
            return await Patch<Slate, SlatePatchDTO>(id, dto);
        }

        #endregion

        #region Endpoints específicos

        [HttpGet("GetSlatesByClient")]
        public async Task<ActionResult<APIResponse>> GetSlatesByClient([FromQuery] int clientId)
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
                    _logger.LogError(((ClientMessage)_message).NotFound(clientId), clientId);
                    _response.ErrorMessages = new() { ((ClientMessage)_message).NotFound(clientId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                if (client.Party == null)
                {
                    _logger.LogError(((PartyMessage)_message).NotFound(clientId), clientId);
                    _response.ErrorMessages = new() { ((PartyMessage)_message).NotFound(clientId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var wings = await _wingRepository.GetAll(w => w.PartyId == client.Party.Id);

                // Obtiene las listas (slates) asociadas a las alas
                var slates = new List<Slate>();
                foreach (var wing in wings)
                {

                    // 1..n
                    var includes = new List<IncludePropertyConfiguration<Slate>>
                        {
                            new IncludePropertyConfiguration<Slate>
                            {
                                IncludeExpression = b => b.ListParticipants
                            },
                            new IncludePropertyConfiguration<Slate>
                            {
                                IncludeExpression = b => b.Candidate
                            },
                            new IncludePropertyConfiguration<Slate>
                            {
                                IncludeExpression = b => b.Photo
                            },
                            new IncludePropertyConfiguration<Slate>
                            {
                                IncludeExpression = b => b.Wing
                            },
                            new IncludePropertyConfiguration<Slate>
                            {
                                IncludeExpression = b => b.Province
                            },
                        };
                    // n..n
                    var thenIncludes = new List<ThenIncludePropertyConfiguration<Slate>>
                    {
                        new ThenIncludePropertyConfiguration<Slate>
                        {
                            IncludeExpression = b => b.ListCircuitSlates,
                            ThenIncludeExpression = ab => ((CircuitSlate)ab).Circuit
                        },
                    };

                    var wingSlates = await _slateRepository.GetAll(s => s.WingId == wing.Id, includes: includes, thenIncludes: thenIncludes);
                    slates.AddRange(wingSlates);
                }

                // Mapea los candidatos únicos a DTOs para la respuesta
                _response.Result = _mapper.Map<List<SlateDTO>>(slates);
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

        //[Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPost(Name = "CreateSlate")]
        public async Task<ActionResult<APIResponse>> Post([FromBody] SlateCreateDTO dto)
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
                if (await _slateRepository.Get(v => v.Name.ToLower() == dto.Name.ToLower()) != null)
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

                Slate slate = _mapper.Map<Slate>(dto);
                slate.Creation = DateTime.Now;
                slate.Update = DateTime.Now;

                slate.ListCircuitSlates = _mapper.Map<List<CircuitSlate>>(dto.ListCircuitSlates);
                slate.ListParticipants = _mapper.Map<List<Participant>>(dto.ListParticipants);

                await _slateRepository.Create(slate);

                // Manejar carga de fotos
                if (dto.Photo != null)
                {
                    await HandlePhotoUpload(dto.Photo, slate);
                }

                _logger.LogInformation(_message.Created(slate.Id, slate.Name));
                await _logService.LogAction("Slate", "Create", $"Id:{slate.Id}, Nombre: {slate.Name}.", User.Identity.Name, null);

                _response.Result = _mapper.Map<SlateDTO>(slate);
                _response.StatusCode = HttpStatusCode.Created;

                // CreatedAtRoute -> Nombre de la ruta (del método): GetCountryDSById
                // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/13816172#notes
                return CreatedAtAction(nameof(Get), new { id = slate.Id }, _response);
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
        /// <param name="slate"></param>
        /// <returns></returns>
        private async Task HandlePhotoUpload(IFormFile photoFile, Slate slate)
        {
            if (photoFile != null)
            {
                string dynamicContainer = $"uploads/slates/slate{slate.Id}";
                var newPhoto = new Photo
                {
                    Slate = slate
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


