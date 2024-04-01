using DatalexionBackend.Infrastructure.DbContext;
using AutoMapper;
using DatalexionBackend.Core.DTO;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Infrastructure.Services;
using DatalexionBackend.Infrastructure.MessagesService;
using DatalexionBackend.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.SignalR;
using DatalexionBackend.Core.Enums;

namespace DatalexionBackend.UI.Controllers.V1
{
    [ApiController]
    [HasHeader("x-version", "1")]
    [Route("api/circuits")]
    public class CircuitsController : CustomBaseController<Circuit>
    {
        private readonly ICircuitRepository _circuitRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;
        private readonly IFileStorage _fileStorage;
        private readonly IHubContext<NotifyHub> _hubContext;
        private readonly IMessage<Circuit> _message;

        public CircuitsController(ILogger<CircuitsController> logger, IMapper mapper, ICircuitRepository circuitRepository, IPhotoRepository photoRepository, IFileStorage fileStorage, ContextDB dbContext, ILogService logService, IHubContext<NotifyHub> hubContext, IMessage<Circuit> message)
            : base(mapper, logger, circuitRepository)
        {
            _circuitRepository = circuitRepository;
            _photoRepository = photoRepository;
            _fileStorage = fileStorage;
            _dbContext = dbContext;
            _logService = logService;
            _hubContext = hubContext;
            _message = message;
        }

        #region Endpoints genéricos

        /// <summary>
        /// Obtiene una lista paginada de circuitos con detalles de municipio, partes y fotos incluidos.
        /// </summary>
        /// <param name="paginationDTO">Parámetros de paginación.</param>
        /// <returns>Una lista paginada de circuitos.</returns>
        [HttpGet("GetCircuit")]
        public async Task<ActionResult<APIResponse>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Circuit>>
            {
                    new IncludePropertyConfiguration<Circuit>
                    {
                        IncludeExpression = b => b.Municipality
                    },
                    new IncludePropertyConfiguration<Circuit>
                    {
                        IncludeExpression = b => b.ListCircuitParties
                    },
                    new IncludePropertyConfiguration<Circuit>
                    {
                        IncludeExpression = b => b.ListPhotos
                    },
                };
            // n..n
            var thenIncludes = new List<ThenIncludePropertyConfiguration<Circuit>>
            {
                // delegados
                new ThenIncludePropertyConfiguration<Circuit>
                {
                    IncludeExpression = b => b.ListCircuitDelegados,
                    ThenIncludeExpression = ab => ((CircuitDelegado)ab).Delegado
                },
                new ThenIncludePropertyConfiguration<Circuit>
                {
                    IncludeExpression = b => b.ListCircuitSlates,
                    ThenIncludeExpression = ab => ((CircuitSlate)ab).Slate
                },
                new ThenIncludePropertyConfiguration<Circuit>
                {
                    IncludeExpression = b => b.ListCircuitParties,
                    ThenIncludeExpression = ab => ((CircuitParty)ab).Party
                },
            };
            return await Get<Circuit, CircuitDTO>(paginationDTO: paginationDTO, thenIncludes: thenIncludes);
        }

        /// <summary>
        /// Obtiene todos los circuitos sin aplicar paginación, incluyendo sus detalles básicos.
        /// </summary>
        /// <returns>Todos los circuitos.</returns>
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> All()
        {
            var circuits = await _circuitRepository.GetAll();
            _response.Result = _mapper.Map<List<CircuitDTO>>(circuits);
            _response.StatusCode = HttpStatusCode.OK;
            return _response;
        }

        /// <summary>
        /// Obtiene un circuito específico por su ID, incluyendo detalles de municipio, partes y fotos.
        /// </summary>
        /// <param name="id">ID del circuito.</param>
        /// <returns>Un circuito específico.</returns>
        [HttpGet("{id:int}")] // url completa: https://localhost:7003/api/Circuits/1
        public async Task<ActionResult<APIResponse>> Get([FromRoute] int id)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Circuit>>
            {
                    new IncludePropertyConfiguration<Circuit>
                    {
                        IncludeExpression = b => b.Municipality
                    },
                    new IncludePropertyConfiguration<Circuit>
                    {
                        IncludeExpression = b => b.ListCircuitParties
                    },
                    new IncludePropertyConfiguration<Circuit>
                    {
                        IncludeExpression = b => b.ListPhotos
                    },
                };
            var thenIncludes = new List<ThenIncludePropertyConfiguration<Circuit>>
            {
                // delegados
                new ThenIncludePropertyConfiguration<Circuit>
                {
                    IncludeExpression = b => b.ListCircuitDelegados,
                    ThenIncludeExpression = ab => ((CircuitDelegado)ab).Delegado
                },
                new ThenIncludePropertyConfiguration<Circuit>
                {
                    IncludeExpression = b => b.ListCircuitSlates,
                    ThenIncludeExpression = ab => ((CircuitSlate)ab).Slate
                },
                                new ThenIncludePropertyConfiguration<Circuit>
                {
                    IncludeExpression = b => b.ListCircuitParties,
                    ThenIncludeExpression = ab => ((CircuitParty)ab).Party
                },

            };
            return await GetById<Circuit, CircuitDTO>(id, thenIncludes: thenIncludes);
        }

        /// <summary>
        /// Elimina un circuito específico por su ID.
        /// </summary>
        /// <param name="id">ID del circuito a eliminar.</param>
        /// <returns>Resultado de la operación de eliminación.</returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Circuit>(id);
        }

        /// <summary>
        /// Actualiza los datos de un circuito existente, incluyendo las partes y votos asociados.
        /// </summary>
        /// <param name="id">ID del circuito a actualizar.</param>
        /// <param name="dto">Datos actualizados del circuito.</param>
        /// <returns>El circuito actualizado.</returns>
        //[Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromBody] CircuitCreateDTO dto)
        {
            try
            {
                if (dto == null || id <= 0)
                {
                    _logger.LogError(_message.NotValid());
                    _response.ErrorMessages = new() { _message.NotValid() };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var _circuitDB = await _dbContext.Circuit
                    .Include(c => c.ListCircuitSlates)
                    .ThenInclude(c => c.Slate)
                    .Include(c => c.ListCircuitParties)
                    .ThenInclude(c => c.Party)
                    .Include(c => c.ListPhotos)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (_circuitDB == null)
                {
                    _logger.LogError($"No se encontró el circuito con ID {id}.");
                    _response.ErrorMessages = new() { $"No se encontró el circuito con ID {id}." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // Paso 1) Actualizar CircuitSlates
                if (dto.ListCircuitSlates != null && dto.ListCircuitSlates.Any())
                {
                    var existingSlates = _circuitDB.ListCircuitSlates.ToList(); // Hacer una copia de la lista para evitar modificar la colección durante la iteración.
                    foreach (var circuitSlateDTO in dto.ListCircuitSlates)
                    {
                        var circuitSlate = existingSlates.FirstOrDefault(cs => cs.CircuitId == id && cs.SlateId == circuitSlateDTO.SlateId);

                        if (circuitSlate != null)
                        {
                            // La entidad ya existe, actualiza los valores necesarios.
                            circuitSlate.Votes = circuitSlateDTO.Votes ?? 0;
                        }
                        else
                        {
                            // La entidad no existe, crea una nueva y agrégala.
                            var newCircuitSlate = new CircuitSlate
                            {
                                CircuitId = id, // Asegúrate de usar el ID del circuito de la petición para la creación
                                SlateId = circuitSlateDTO.SlateId,
                                Votes = circuitSlateDTO.Votes ?? 0
                            };
                            _circuitDB.ListCircuitSlates.Add(newCircuitSlate); // Agrega directamente a la colección del circuito
                        }
                    }
                    // Elimina los slates que no están en el DTO
                    _circuitDB.ListCircuitSlates = _circuitDB.ListCircuitSlates.Where(cs => dto.ListCircuitSlates.Any(dto => dto.SlateId == cs.SlateId)).ToList();
                }

                // Paso 2) Actualizar CircuitParties
                if (dto.ListCircuitParties != null && dto.ListCircuitParties.Any())
                {
                    var existingCircuitParties = _circuitDB.ListCircuitParties.ToList(); // Hacer una copia de la lista para evitar modificar la colección durante la iteración.
                    foreach (var circuitPartyDTO in dto.ListCircuitParties)
                    {
                        var circuitParty1 = existingCircuitParties.FirstOrDefault(cs => cs.CircuitId == id && cs.PartyId == circuitPartyDTO.PartyId);
                        if (circuitParty1 != null)
                        {
                            // La entidad ya existe, actualiza los valores necesarios.
                            circuitParty1.Votes = circuitPartyDTO.Votes ?? 0;
                        }
                        else
                        {
                            // La entidad no existe, crea una nueva y agrégala.
                            var newCircuitParty = new CircuitParty
                            {
                                CircuitId = id, // Asegúrate de usar el ID del circuito de la petición para la creación
                                PartyId = circuitPartyDTO.PartyId,
                                Votes = circuitPartyDTO.Votes ?? 0
                            };
                            _circuitDB.ListCircuitParties.Add(newCircuitParty); // Agrega directamente a la colección del circuito
                        }
                    }
                    // Elimina los Partys que no están en el DTO
                    _circuitDB.ListCircuitParties = _circuitDB.ListCircuitParties.Where(cs => dto.ListCircuitParties.Any(dto => dto.PartyId == cs.PartyId)).ToList();
                }

                // Paso 3) Actualizar Extra votes y Step actual
                if (dto != null)
                {
                    _circuitDB.BlankVotes = dto.BlankVotes;
                    _circuitDB.NullVotes = dto.NullVotes;
                    _circuitDB.ObservedVotes = dto.ObservedVotes;
                    _circuitDB.RecurredVotes = dto.RecurredVotes;

                    _circuitDB.Step1completed = dto.Step1completed;
                    _circuitDB.Step2completed = dto.Step2completed;
                    _circuitDB.Step3completed = dto.Step3completed;
                }

                // Fotos
                if (dto.ListPhotos != null && dto.ListPhotos.Count > 0)
                {
                    // En el método de creación o actualización de un circuito:
                    await HandlePhotoUpload(dto.ListPhotos, _circuitDB);
                }

                await _dbContext.SaveChangesAsync();
                _response.Result = $"Los votos del circuito con ID {id} han sido actualizados exitosamente.";
                _response.StatusCode = HttpStatusCode.OK;

                // SignalR
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "System", $"Se actualizó correctamente el circuito Id:{id}.");

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

        /// <summary>
        /// Actualiza un circuito existente, aplicando los cambios especificados en el DTO de creación.
        /// </summary>
        /// <param name="id">ID del circuito a actualizar.</param>
        /// <param name="dto">DTO de creación con los datos para actualizar el circuito.</param>
        /// <returns>Respuesta indicando el resultado de la operación de actualización.</returns>
        //[Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPut("{id:int}/update")]
        public async Task<ActionResult<APIResponse>> UpdateCircuit(int id, [FromBody] CircuitCreateDTO dto)
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

                var circuit = await _circuitRepository.Get(v => v.Id == id);
                if (circuit == null)
                {
                    _logger.LogError(_message.NotFound(id));
                    _response.ErrorMessages = new() { _message.NotFound(id) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // No usar AutoMapper para mapear todo el objeto, sino actualizar campo por campo
                circuit.Name = Utils.ToCamelCase(dto.Name);
                circuit.Comments = Utils.ToCamelCase(dto.Comments);
                circuit.Update = DateTime.Now;

                var updateCircuit = await _circuitRepository.Update(circuit);

                _logger.LogInformation(_message.ActionLog(id, circuit.Name));
                await _logService.LogAction("Circuit", "Update", _message.ActionLog(id, circuit.Name), User.Identity.Name, null);

                _response.Result = _mapper.Map<ProvinceDTO>(updateCircuit);
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

        /// <summary>
        /// Aplica actualizaciones parciales a un circuito existente, específicamente dirigido a los votos y la carga de fotos.
        /// </summary>
        /// <param name="id">ID del circuito a actualizar.</param>
        /// <param name="dto">DTO para aplicar actualizaciones parciales.</param>
        /// <param name="photos">Lista de fotos nuevas a cargar.</param>
        /// <returns>Respuesta indicando el resultado de la operación de actualización parcial.</returns>
        //[Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPatch("{id:int}")]
        [Consumes("multipart/form-data")] // Indicar que el método aceptará multipart/form-data
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromForm] CircuitPatchDTO dto, [FromForm] List<IFormFile> photos)
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

                var _circuitDB = await _dbContext.Circuit.Include(c => c.ListPhotos).FirstOrDefaultAsync(c => c.Id == id);
                if (_circuitDB == null)
                {
                    _logger.LogError($"No se encontró el circuito con ID {id}.");
                    _response.ErrorMessages = new() { $"No se encontró el circuito con ID {id}." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // Actualizar votos
                _circuitDB.BlankVotes = dto.BlankVotes;
                _circuitDB.NullVotes = dto.NullVotes;
                _circuitDB.ObservedVotes = dto.ObservedVotes;
                _circuitDB.RecurredVotes = dto.RecurredVotes;
                _circuitDB.Step3completed = true;

                // Manejar carga de fotos
                if (photos != null && photos.Count > 0)
                {
                    await HandlePhotoUpload(photos, _circuitDB);
                }

                await _dbContext.SaveChangesAsync();
                _response.Result = $"Los votos y fotos del circuito con ID {id} han sido actualizados exitosamente.";
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

        #endregion

        #region Endpoints específicos

        /// <summary>
        /// Crea un nuevo circuito en el sistema.
        /// </summary>
        /// <param name="circuitCreateDto">Datos del nuevo circuito.</param>
        /// <returns>El circuito creado.</returns>
        [HttpPost(Name = "CreateCircuit")]
        public async Task<ActionResult<APIResponse>> Post([FromBody] CircuitCreateDTO circuitCreateDto)
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
                if (await _circuitRepository.Get(v => v.Name.ToLower() == circuitCreateDto.Name.ToLower()) != null)
                {
                    _logger.LogError($"El nombre {circuitCreateDto.Name} ya existe en el sistema");
                    _response.ErrorMessages = new() { $"El nombre {circuitCreateDto.Name} ya existe en el sistema." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("NameAlreadyExists", $"El nombre {circuitCreateDto.Name} ya existe en el sistema.");
                    return BadRequest(ModelState);
                }

                circuitCreateDto.Name = Utils.ToCamelCase(circuitCreateDto.Name);
                circuitCreateDto.Comments = Utils.ToCamelCase(circuitCreateDto.Comments);

                Circuit circuit = _mapper.Map<Circuit>(circuitCreateDto);
                circuit.Creation = DateTime.Now;
                circuit.Update = DateTime.Now;

                circuit.ListCircuitDelegados = circuitCreateDto.ListCircuitDelegados
                    .Select(dto => _mapper.Map<CircuitDelegado>(dto))
                    .ToList();
                circuit.ListCircuitParties = _mapper.Map<List<CircuitParty>>(circuitCreateDto.ListCircuitParties);
                circuit.ListCircuitSlates = _mapper.Map<List<CircuitSlate>>(circuitCreateDto.ListCircuitSlates);

                await _circuitRepository.Create(circuit);

                _logger.LogInformation(_message.Created(circuit.Id, circuit.Name));
                await _logService.LogAction("Circuit", "Create", $"Id:{circuit.Id}, Nombre: {circuit.Name}.", User.Identity.Name, null);

                _response.Result = _mapper.Map<CircuitDTO>(circuit);
                _response.StatusCode = HttpStatusCode.Created;

                // CreatedAtRoute -> Nombre de la ruta (del método): GetCountryDSById
                // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/13816172#notes
                return CreatedAtAction(nameof(Get), new { id = circuit.Id }, _response);
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
        /// Sube una lista de fotos.
        /// </summary>
        /// <param name="photoFiles"></param>
        /// <param name="circuit"></param>
        /// <returns></returns>
        private async Task HandlePhotoUpload(List<IFormFile> photoFiles, Circuit circuit)
        {
            if (photoFiles != null && photoFiles.Count > 0)
            {
                foreach (var photoFile in photoFiles)
                {
                    if (photoFile != null)
                    {
                        string dynamicContainer = $"uploads/circuits/circuit{circuit.Id}";
                        var newPhoto = new Photo
                        {
                            Circuit = circuit
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
            }
        }

        #endregion

    }
}

