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
        private readonly IClientRepository _clientRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;
        private readonly IFileStorage _fileStorage;
        // private readonly IHubContext<NotifyHub> _hubContext; // SignalR
        private readonly IMessage<Circuit> _message;

        public CircuitsController(ILogger<CircuitsController> logger, IMapper mapper, ICircuitRepository circuitRepository, IClientRepository clientRepository, IPhotoRepository photoRepository, IFileStorage fileStorage, ContextDB dbContext, ILogService logService, IMessage<Circuit> message)
            : base(mapper, logger, circuitRepository)
        {
            _circuitRepository = circuitRepository;
            _clientRepository = clientRepository;
            _photoRepository = photoRepository;
            _fileStorage = fileStorage;
            _dbContext = dbContext;
            _logService = logService;
            _message = message;
        }

        #region Endpoints genéricos

        /// <summary>
        /// Obtiene una lista paginada de circuitos con detalles de municipio, partes y fotos incluidos.
        /// </summary>
        /// <param name="paginationDTO">Parámetros de paginación.</param>
        /// <returns>Una lista paginada de circuitos.</returns>
        [HttpGet("GetCircuit")]
        [ResponseCache(Duration = 60)]
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
                    // new IncludePropertyConfiguration<Circuit>
                    // {
                    //     IncludeExpression = b => b.ListPhotos
                    // },
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
        /// Filtra los circuitos que incluye únicamente las listas de nuestro cliente
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        // [ResponseCache(Duration = 60)]
        [HttpGet("GetCircuitsByClient")]
        public async Task<ActionResult<APIResponse>> GetCircuitsByClient([FromQuery] int clientId)
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
                    _logger.LogError(((PartyMessage)_message).NotFound(), clientId);
                    _response.ErrorMessages = new() { ((PartyMessage)_message).NotFound() };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

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
                        IncludeExpression = b => b.ListCircuitSlates.Where(cs => cs.Slate.Wing.PartyId == client.PartyId),
                        ThenIncludeExpression = ab => ((CircuitSlate)ab).Slate
                    },
                    new ThenIncludePropertyConfiguration<Circuit>
                    {
                        IncludeExpression = b => b.ListCircuitParties,
                        ThenIncludeExpression = ab => ((CircuitParty)ab).Party
                    },
                };
                return await Get<Circuit, CircuitDTO>(thenIncludes: thenIncludes);
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
        [ResponseCache(Duration = 60)]
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
                    // new IncludePropertyConfiguration<Circuit>
                    // {
                    //     IncludeExpression = b => b.ListPhotos
                    // },
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
        /// Obtiene un circuito específico por su ID de cliente.
        /// </summary>
        /// <param name="id">ID del circuito.</param>
        /// <param name="clientId">ID del cliente.</param>
        /// <returns>Un circuito específico.</returns>
        [ResponseCache(Duration = 60)]
        [HttpGet("GetCircuitByClient/{id:int}")]
        public async Task<ActionResult<APIResponse>> GetCircuitByClient([FromRoute] int id, [FromQuery] int clientId)
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
                    // new IncludePropertyConfiguration<Circuit>
                    // {
                    //     IncludeExpression = b => b.ListPhotos
                    // },
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
                        IncludeExpression = b => b.ListCircuitSlates.Where(cs => cs.Slate.Wing.PartyId == clientId),
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
        [Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Circuit>(id);
        }

        /// <summary>
        /// Actualiza los datos de un circuito existente, incluyendo las partes y votos asociados.
        /// Origen Frontend: FormParty1.js y FormSlate1.js
        /// </summary>
        /// <param name="id">ID del circuito a actualizar.</param>
        /// <param name="clientId">ID del cliente para hallar al Partido<!param>
        /// <param name="delegadoId">ID del delegado que actualiza.</param>
        /// <param name="dto">Datos actualizados del circuito.</param>
        /// <returns>El circuito actualizado.</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromBody] CircuitPutDTO dto)
        {
            try
            {
                if (id <= 0 || dto.ClientId <= 0 || dto.LastUpdateDelegadoId <= 0 || dto == null)
                {
                    _logger.LogError(_message.NotValid());
                    _response.ErrorMessages = new() { _message.NotValid() };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var circuitDB = await _dbContext.Circuit
                    .Include(c => c.ListCircuitSlates)
                    .ThenInclude(c => c.Slate)
                    .Include(c => c.ListCircuitParties)
                    .ThenInclude(c => c.Party)
                    // .Include(c => c.ListPhotos)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (circuitDB == null)
                {
                    _logger.LogError($"No se encontró el circuito con ID {id}.");
                    _response.ErrorMessages = new() { $"No se encontró el circuito con ID {id}." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var clientDB = await _dbContext.Client
                           .Where(c => c.Id == dto.ClientId)
                           .FirstOrDefaultAsync();

                if (clientDB == null)
                {
                    _logger.LogError(((ClientMessage)_message).NotFound(dto.ClientId), dto.ClientId);
                    _response.ErrorMessages = new() { ((ClientMessage)_message).NotFound(dto.ClientId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var delegadoDB = await _dbContext.Delegado
                           .Where(c => c.Id == dto.LastUpdateDelegadoId)
                           .FirstOrDefaultAsync();

                if (delegadoDB == null)
                {
                    _logger.LogError(((DelegadoMessage)_message).NotFound(dto.LastUpdateDelegadoId.Value), dto.LastUpdateDelegadoId.Value);
                    _response.ErrorMessages = new() { ((DelegadoMessage)_message).NotFound(dto.LastUpdateDelegadoId.Value) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // Paso 1) Actualizar CircuitSlates
                if (dto.ListCircuitSlates != null && dto.ListCircuitSlates.Any())
                {
                    var existingSlates = circuitDB.ListCircuitSlates.ToList(); // Hacer una copia de la lista para evitar modificar la colección durante la iteración.
                    foreach (var circuitSlateDTO in dto.ListCircuitSlates)
                    {
                        var circuitSlate = existingSlates.FirstOrDefault(cs => cs.CircuitId == id && cs.SlateId == circuitSlateDTO.SlateId);
                        if (circuitSlate != null)
                        {
                            // La entidad ya existe, actualiza los valores necesarios.
                            circuitSlate.TotalSlateVotes = circuitSlateDTO.TotalSlateVotes ?? 0;
                        }
                        else
                        {
                            // La entidad no existe, crea una nueva y agrégala.
                            var newCircuitSlate = new CircuitSlate
                            {
                                CircuitId = id, // Asegúrate de usar el ID del circuito de la petición para la creación
                                SlateId = circuitSlateDTO.SlateId,
                                TotalSlateVotes = circuitSlateDTO.TotalSlateVotes ?? 0
                            };
                            circuitDB.ListCircuitSlates.Add(newCircuitSlate); // Agrega directamente a la colección del circuito
                        }
                    }
                    // Elimina los slates que no están en el DTO
                    circuitDB.ListCircuitSlates = circuitDB.ListCircuitSlates.Where(cs => dto.ListCircuitSlates.Any(dto => dto.SlateId == cs.SlateId)).ToList();
                }

                // Paso 2) Actualizar CircuitParties
                if (dto.ListCircuitParties != null && dto.ListCircuitParties.Any())
                {
                    // Chequear si está OK !!
                    var existingCircuitParties = circuitDB.ListCircuitParties.Where(x => x.PartyId == clientDB.PartyId).ToList(); // Hacer una copia de la lista para evitar modificar la colección durante la iteración.
                    foreach (var circuitPartyDTO in dto.ListCircuitParties)
                    {
                        // var circuitPartyDB = existingCircuitParties.FirstOrDefault(cs => cs.CircuitId == circuitId && cs.PartyId == circuitPartyDTO.PartyId);
                        var circuitPartyDB = existingCircuitParties.FirstOrDefault(cs => cs.CircuitId == id && cs.PartyId == circuitPartyDTO.PartyId);
                        if (circuitPartyDB != null)
                        {
                            // La entidad ya existe, actualiza los valores necesarios.
                            circuitPartyDB.TotalPartyVotes = circuitPartyDTO.TotalPartyVotes ?? 0;
                        }
                        else
                        {
                            // La entidad no existe, crea una nueva y agrégala.
                            var newCircuitParty = new CircuitParty
                            {
                                CircuitId = id, // Asegúrate de usar el ID del circuito de la petición para la creación
                                PartyId = circuitPartyDTO.PartyId,
                                TotalPartyVotes = circuitPartyDTO.TotalPartyVotes ?? 0
                            };
                            circuitDB.ListCircuitParties.Add(newCircuitParty); // Agrega directamente a la colección del circuito
                        }

                        if (circuitPartyDB != null)
                        {
                            // Paso 3) Actualizar Extra votes y Step actual
                            // Todo

                            circuitPartyDB.BlankVotes = dto.BlankVotes;
                            circuitPartyDB.NullVotes = dto.NullVotes;
                            circuitPartyDB.ObservedVotes = dto.ObservedVotes;
                            circuitPartyDB.RecurredVotes = dto.RecurredVotes;

                            circuitPartyDB.Step1completed = dto.Step1completed;
                            circuitPartyDB.Step2completed = dto.Step2completed;
                            circuitPartyDB.Step3completed = dto.Step3completed;

                            circuitPartyDB.LastUpdateDelegadoId = dto.LastUpdateDelegadoId;

                            // Fotos
                            if (dto.ListPhotos != null && dto.ListPhotos.Count > 0)
                            {
                                // En el método de creación o actualización de un circuito:
                                await HandlePhotoUpload(dto.ListPhotos, circuitPartyDB);
                            }
                        }
                    }
                    // Elimina los Partys que no están en el DTO
                    circuitDB.ListCircuitParties = circuitDB.ListCircuitParties.Where(cs => dto.ListCircuitParties.Any(dto => dto.PartyId == cs.PartyId)).ToList();
                }

                await _dbContext.SaveChangesAsync();
                _response.Result = $"Los votos del circuito con ID {id} han sido actualizados exitosamente.";
                _response.StatusCode = HttpStatusCode.OK;

                // SignalR
                // await _hubContext.Clients.All.SendAsync("ReceiveMessage", "System", $"Se actualizó correctamente el circuito Id:{id}.");

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
        /// Actualiza los datos de un circuito existente, incluyendo las partes y votos asociados.
        /// Origen Frontend: FormParty1.js y FormSlate1.js
        /// </summary>
        /// <param name="id">ID del circuito a actualizar.</param>
        /// <param name="clientId">ID del cliente para hallar al Partido<!param>
        /// <param name="delegadoId">ID del delegado que actualiza.</param>
        /// <param name="dto">Datos actualizados del circuito.</param>
        /// <returns>El circuito actualizado.</returns>
        [HttpPut("CircuitPut/{id:int}")] // url completa: https://localhost:7003/api/Circuits/CircuitPut/1
        public async Task<ActionResult<APIResponse>> CircuitPut(int id, [FromBody] CircuitPutDTO dto)
        {
            try
            {
                if (id <= 0 || dto == null || dto.ClientId <= 0 || dto.LastUpdateDelegadoId <= 0)
                {
                    _logger.LogError(_message.NotValid());
                    _response.ErrorMessages = new() { _message.NotValid() };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var circuitDB = await _dbContext.Circuit
                    .Include(c => c.ListCircuitSlates)
                    .ThenInclude(c => c.Slate)
                    .Include(c => c.ListCircuitParties)
                    .ThenInclude(c => c.Party)
                    // .Include(c => c.ListPhotos)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (circuitDB == null)
                {
                    _logger.LogError($"No se encontró el circuito con ID {id}.");
                    _response.ErrorMessages = new() { $"No se encontró el circuito con ID {id}." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var clientDB = await _dbContext.Client
                           .Where(c => c.Id == dto.ClientId)
                           .FirstOrDefaultAsync();

                if (clientDB == null)
                {
                    _logger.LogError(((ClientMessage)_message).NotFound(dto.ClientId), dto.ClientId);
                    _response.ErrorMessages = new() { ((ClientMessage)_message).NotFound(dto.ClientId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var delegadoDB = await _dbContext.Delegado
                           .Where(c => c.Id == dto.LastUpdateDelegadoId)
                           .FirstOrDefaultAsync();

                if (delegadoDB == null)
                {
                    _logger.LogError(((DelegadoMessage)_message).NotFound(dto.LastUpdateDelegadoId.Value), dto.LastUpdateDelegadoId.Value);
                    _response.ErrorMessages = new() { ((DelegadoMessage)_message).NotFound(dto.LastUpdateDelegadoId.Value) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // Paso 1) Actualizar CircuitSlates
                if (dto.ListCircuitSlates != null && dto.ListCircuitSlates.Any())
                {
                    var existingSlates = circuitDB.ListCircuitSlates.ToList(); // Hacer una copia de la lista para evitar modificar la colección durante la iteración.
                    foreach (var circuitSlateDTO in dto.ListCircuitSlates)
                    {
                        var circuitSlate = existingSlates.FirstOrDefault(cs => cs.CircuitId == id && cs.SlateId == circuitSlateDTO.SlateId);
                        if (circuitSlate != null)
                        {
                            // La entidad ya existe, actualiza los valores necesarios.
                            circuitSlate.TotalSlateVotes = circuitSlateDTO.TotalSlateVotes ?? 0;
                        }
                        else
                        {
                            // La entidad no existe, crea una nueva y agrégala.
                            var newCircuitSlate = new CircuitSlate
                            {
                                CircuitId = id, // Asegúrate de usar el ID del circuito de la petición para la creación
                                SlateId = circuitSlateDTO.SlateId,
                                TotalSlateVotes = circuitSlateDTO.TotalSlateVotes ?? 0
                            };
                            circuitDB.ListCircuitSlates.Add(newCircuitSlate); // Agrega directamente a la colección del circuito
                        }
                    }
                    // Elimina los slates que no están en el DTO
                    circuitDB.ListCircuitSlates = circuitDB.ListCircuitSlates.Where(cs => dto.ListCircuitSlates.Any(dto => dto.SlateId == cs.SlateId)).ToList();
                }

                // Paso 2) Actualizar CircuitParties
                if (dto.ListCircuitParties != null && dto.ListCircuitParties.Any())
                {
                    // Chequear si está OK !!
                    var existingCircuitParties = circuitDB.ListCircuitParties.Where(x => x.PartyId == clientDB.PartyId).ToList(); // Hacer una copia de la lista para evitar modificar la colección durante la iteración.
                    foreach (var circuitPartyDTO in dto.ListCircuitParties)
                    {
                        // var circuitPartyDB = existingCircuitParties.FirstOrDefault(cs => cs.CircuitId == circuitId && cs.PartyId == circuitPartyDTO.PartyId);
                        var circuitPartyDB = existingCircuitParties.FirstOrDefault(cs => cs.CircuitId == id && cs.PartyId == circuitPartyDTO.PartyId);
                        if (circuitPartyDB != null)
                        {
                            // La entidad ya existe, actualiza los valores necesarios.
                            circuitPartyDB.TotalPartyVotes = circuitPartyDTO.TotalPartyVotes ?? 0;
                        }
                        else
                        {
                            // La entidad no existe, crea una nueva y agrégala.
                            var newCircuitParty = new CircuitParty
                            {
                                CircuitId = id, // Asegúrate de usar el ID del circuito de la petición para la creación
                                PartyId = circuitPartyDTO.PartyId,
                                TotalPartyVotes = circuitPartyDTO.TotalPartyVotes ?? 0
                            };
                            circuitDB.ListCircuitParties.Add(newCircuitParty); // Agrega directamente a la colección del circuito
                            circuitPartyDB = newCircuitParty;
                        }

                        if (circuitPartyDB != null)
                        {
                            // Paso 3) Actualizar Extra votes y Step actual
                            circuitPartyDB.BlankVotes = dto.BlankVotes;
                            circuitPartyDB.NullVotes = dto.NullVotes;
                            circuitPartyDB.ObservedVotes = dto.ObservedVotes;
                            circuitPartyDB.RecurredVotes = dto.RecurredVotes;

                            circuitPartyDB.Step1completed = dto.Step1completed;
                            circuitPartyDB.Step2completed = dto.Step2completed;
                            circuitPartyDB.Step3completed = dto.Step3completed;

                            circuitPartyDB.LastUpdateDelegadoId = dto.LastUpdateDelegadoId;

                            // Fotos
                            if (dto.ListPhotos != null && dto.ListPhotos.Count > 0)
                            {
                                // En el método de creación o actualización de un circuito:
                                await HandlePhotoUpload(dto.ListPhotos, circuitPartyDB);
                            }
                        }
                    }
                    // Elimina los Partys que no están en el DTO
                    circuitDB.ListCircuitParties = circuitDB.ListCircuitParties.Where(cs => dto.ListCircuitParties.Any(dto => dto.PartyId == cs.PartyId)).ToList();
                }

                await _dbContext.SaveChangesAsync();
                _response.Result = $"Los votos del circuito con ID {id} han sido actualizados exitosamente.";
                _response.StatusCode = HttpStatusCode.OK;

                // SignalR
                // await _hubContext.Clients.All.SendAsync("ReceiveMessage", "System", $"Se actualizó correctamente el circuito Id:{id}.");

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
        /// Updates an existing circuit, applying the specified changes in the creation DTO.
        /// Origin Frontend: CircuitTable.js
        /// </summary>
        /// <param name="id">ID of the circuit to update.</param>
        /// <param name="dto">Creation DTO with the data to update the circuit.</param>
        /// <returns>Response indicating the result of the update operation.</returns>
        [HttpPut("UpdateStep1/{id:int}")] // url completa: https://localhost:7003/api/Circuits/UpdateStep1/1
        public async Task<ActionResult<APIResponse>> UpdateStep1(int id, [FromBody] CircuitUpdateStep1DTO dto)
        {
            try
            {
                if (id <= 0 || dto == null || dto.ClientId <= 0 || dto.LastUpdateDelegadoId <= 0)
                {
                    _logger.LogError(_message.NotValid());
                    _response.ErrorMessages = new() { _message.NotValid() };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var circuitDB = await _dbContext.Circuit
                    .Include(c => c.ListCircuitSlates)
                    .ThenInclude(c => c.Slate)
                    .Include(c => c.ListCircuitParties)
                    .ThenInclude(c => c.Party)
                    // .Include(c => c.ListPhotos)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (circuitDB == null)
                {
                    _logger.LogError($"No se encontró el circuito con ID {id}.");
                    _response.ErrorMessages = new() { $"No se encontró el circuito con ID {id}." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var delegadoDB = await _dbContext.Delegado
                        .Where(c => c.Id == dto.LastUpdateDelegadoId)
                        .FirstOrDefaultAsync();

                if (delegadoDB == null)
                {
                    _logger.LogError(((DelegadoMessage)_message).NotFound(dto.LastUpdateDelegadoId.Value), dto.LastUpdateDelegadoId.Value);
                    _response.ErrorMessages = new() { ((DelegadoMessage)_message).NotFound(dto.LastUpdateDelegadoId.Value) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var clientDB = await _dbContext.Client
                           .Where(c => c.Id == dto.ClientId)
                           .FirstOrDefaultAsync();
                if (clientDB == null)
                {
                    _logger.LogError(((ClientMessage)_message).NotFound(dto.ClientId), dto.ClientId);
                    _response.ErrorMessages = new() { ((ClientMessage)_message).NotFound(dto.ClientId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                if (circuitDB.ListCircuitParties != null && circuitDB.ListCircuitParties.Any())
                {
                    var circuitPartyDB = circuitDB.ListCircuitParties.Where(x => x.PartyId == clientDB.PartyId && x.CircuitId == dto.Id).FirstOrDefault();
                    if (circuitPartyDB == null)
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
                        var existingSlates = circuitDB.ListCircuitSlates.ToList(); // Hacer una copia de la lista para evitar modificar la colección durante la iteración.
                        int totalPartyVotes1 = 0;
                        foreach (var circuitSlateFromDTO in dto.ListCircuitSlates)
                        {
                            var circuitSlate = existingSlates.FirstOrDefault(cs => cs.CircuitId == id && cs.SlateId == circuitSlateFromDTO.SlateId);
                            if (circuitSlate != null)
                            {
                                // La entidad ya existe, actualiza los valores necesarios.
                                circuitSlate.TotalSlateVotes = circuitSlateFromDTO.TotalSlateVotes ?? 0;
                            }
                            else
                            {
                                // La entidad no existe, crea una nueva y agrégala.
                                var newCircuitSlate = new CircuitSlate
                                {
                                    CircuitId = id, // Asegúrate de usar el ID del circuito de la petición para la creación
                                    SlateId = circuitSlateFromDTO.SlateId,
                                    TotalSlateVotes = circuitSlateFromDTO.TotalSlateVotes ?? 0
                                };
                                circuitDB.ListCircuitSlates.Add(newCircuitSlate); // Agrega directamente a la colección del circuito
                            }
                            totalPartyVotes1 += circuitSlateFromDTO.TotalSlateVotes ?? 0;

                            // Elimina los slates que no están en el DTO
                            circuitDB.ListCircuitSlates = circuitDB.ListCircuitSlates.Where(cs => dto.ListCircuitSlates.Any(dto => dto.SlateId == cs.SlateId)).ToList();
                        }

                        // Paso 2) Actualizar CircuitParty
                        if (circuitPartyDB != null)
                        {
                            circuitPartyDB.Step1completed = true;
                            circuitPartyDB.TotalPartyVotes = totalPartyVotes1;
                            circuitPartyDB.LastUpdateDelegadoId = dto.LastUpdateDelegadoId;
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();
                _response.Result = $"Los votos del circuito con ID {id} han sido actualizados exitosamente.";
                _response.StatusCode = HttpStatusCode.OK;

                // SignalR
                // await _hubContext.Clients.All.SendAsync("ReceiveMessage", "System", $"Se actualizó correctamente el circuito Id:{id}.");

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
        /// Updates an existing circuit, applying the specified changes in the creation DTO.
        /// Origin Frontend: CircuitTable.js
        /// </summary>
        /// <param name="id">ID of the circuit to update.</param>
        /// <param name="dto">Creation DTO with the data to update the circuit.</param>
        /// <returns>Response indicating the result of the update operation.</returns>
        [HttpPut("UpdateStep2/{id:int}")] // url completa: https://localhost:7003/api/Circuits/UpdateStep2/2
        public async Task<ActionResult<APIResponse>> UpdateStep2(int id, [FromBody] CircuitUpdateStep2DTO dto)
        {
            try
            {
                if (id <= 0 || dto == null || dto.ClientId <= 0 || dto.LastUpdateDelegadoId <= 0)
                {
                    _logger.LogError(_message.NotValid());
                    _response.ErrorMessages = new() { _message.NotValid() };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var circuitDB = await _dbContext.Circuit
                    .Include(c => c.ListCircuitSlates)
                    .ThenInclude(c => c.Slate)
                    .Include(c => c.ListCircuitParties)
                    .ThenInclude(c => c.Party)
                    // .Include(c => c.ListPhotos)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (circuitDB == null)
                {
                    _logger.LogError($"No se encontró el circuito con ID {id}.");
                    _response.ErrorMessages = new() { $"No se encontró el circuito con ID {id}." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var delegadoDB = await _dbContext.Delegado
                        .Where(c => c.Id == dto.LastUpdateDelegadoId)
                        .FirstOrDefaultAsync();

                if (delegadoDB == null)
                {
                    _logger.LogError(((DelegadoMessage)_message).NotFound(dto.LastUpdateDelegadoId.Value), dto.LastUpdateDelegadoId.Value);
                    _response.ErrorMessages = new() { ((DelegadoMessage)_message).NotFound(dto.LastUpdateDelegadoId.Value) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var clientDB = await _dbContext.Client
                           .Where(c => c.Id == dto.ClientId)
                           .FirstOrDefaultAsync();
                if (clientDB == null)
                {
                    _logger.LogError(((ClientMessage)_message).NotFound(dto.ClientId), dto.ClientId);
                    _response.ErrorMessages = new() { ((ClientMessage)_message).NotFound(dto.ClientId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                if (circuitDB.ListCircuitParties != null && circuitDB.ListCircuitParties.Any())
                {
                    var circuitPartyDB = circuitDB.ListCircuitParties.Where(x => x.PartyId == clientDB.PartyId && x.CircuitId == dto.Id).FirstOrDefault();
                    if (circuitPartyDB == null)
                    {
                        _logger.LogError($"No se encontró el circuito con ID {id}.");
                        _response.ErrorMessages = new() { $"No se encontró el circuito con ID {id}." };
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        return NotFound(_response);
                    }

                    // Paso 1) Actualizar CircuitParties
                    if (dto.ListCircuitParties != null && dto.ListCircuitParties.Any())
                    {
                        var existingParties = circuitDB.ListCircuitParties.ToList(); // Hacer una copia de la lista para evitar modificar la colección durante la iteración.
                        foreach (var circuitPartyDTO in dto.ListCircuitParties)
                        {
                            var circuitParty = existingParties.FirstOrDefault(cs => cs.CircuitId == id && cs.PartyId == circuitPartyDTO.PartyId);
                            if (circuitParty != null)
                            {
                                // La entidad ya existe, actualiza los valores necesarios.
                                circuitParty.TotalPartyVotes = circuitPartyDTO.TotalPartyVotes ?? 0;
                            }
                            else
                            {
                                // La entidad no existe, crea una nueva y agrégala.
                                var newCircuitParty = new CircuitParty
                                {
                                    CircuitId = id, // Asegúrate de usar el ID del circuito de la petición para la creación
                                    PartyId = circuitPartyDTO.PartyId,
                                    TotalPartyVotes = circuitPartyDTO.TotalPartyVotes ?? 0
                                };
                                circuitDB.ListCircuitParties.Add(newCircuitParty); // Agrega directamente a la colección del circuito
                            }

                            // Elimina los slates que no están en el DTO
                            circuitDB.ListCircuitParties = circuitDB.ListCircuitParties.Where(cs => dto.ListCircuitParties.Any(dto => dto.PartyId == cs.PartyId)).ToList();
                        }

                        // Paso 2) Actualizar CircuitParty
                        if (circuitPartyDB != null)
                        {
                            circuitPartyDB.Step2completed = true;
                            circuitPartyDB.LastUpdateDelegadoId = dto.LastUpdateDelegadoId;
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();
                _response.Result = $"Los votos del circuito con ID {id} han sido actualizados exitosamente.";
                _response.StatusCode = HttpStatusCode.OK;

                // SignalR
                // await _hubContext.Clients.All.SendAsync("ReceiveMessage", "System", $"Se actualizó correctamente el circuito Id:{id}.");

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
        /// Updates an existing circuit, applying the specified changes in the creation DTO.
        /// Origin Frontend: CircuitTable.js
        /// </summary>
        /// <param name="id">ID of the circuit to update.</param>
        /// <param name="dto">Creation DTO with the data to update the circuit.</param>
        /// <returns>Response indicating the result of the update operation.</returns>
        [HttpPut("UpdateStep3/{id:int}")] // url completa: https://localhost:7003/api/Circuits/UpdateStep3/3
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<APIResponse>> UpdateStep3(int id, [FromForm] CircuitUpdateStep3DTO dto, [FromForm] List<IFormFile> photos)
        {
            try
            {
                if (id <= 0 || dto == null || dto.ClientId <= 0 || dto.LastUpdateDelegadoId <= 0)
                {
                    _logger.LogError(_message.NotValid());
                    _response.ErrorMessages = new() { _message.NotValid() };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var circuitDB = await _dbContext.Circuit
                    .Include(c => c.ListCircuitSlates)
                    .ThenInclude(c => c.Slate)
                    .Include(c => c.ListCircuitParties)
                    .ThenInclude(c => c.Party)
                    // .Include(c => c.ListPhotos)
                    .FirstOrDefaultAsync(c => c.Id == id);
                if (circuitDB == null)
                {
                    _logger.LogError($"No se encontró el circuito con ID {id}.");
                    _response.ErrorMessages = new() { $"No se encontró el circuito con ID {id}." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var delegadoDB = await _dbContext.Delegado
                        .Where(c => c.Id == dto.LastUpdateDelegadoId)
                        .FirstOrDefaultAsync();

                if (delegadoDB == null)
                {
                    _logger.LogError(((DelegadoMessage)_message).NotFound(dto.LastUpdateDelegadoId.Value), dto.LastUpdateDelegadoId.Value);
                    _response.ErrorMessages = new() { ((DelegadoMessage)_message).NotFound(dto.LastUpdateDelegadoId.Value) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var clientDB = await _dbContext.Client
                           .Where(c => c.Id == dto.ClientId)
                           .FirstOrDefaultAsync();
                if (clientDB == null)
                {
                    _logger.LogError(((ClientMessage)_message).NotFound(dto.ClientId), dto.ClientId);
                    _response.ErrorMessages = new() { ((ClientMessage)_message).NotFound(dto.ClientId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                if (circuitDB.ListCircuitParties != null && circuitDB.ListCircuitParties.Any())
                {
                    var circuitPartyDB = circuitDB.ListCircuitParties.Where(x => x.PartyId == clientDB.PartyId && x.CircuitId == dto.Id).FirstOrDefault();
                    if (circuitPartyDB == null)
                    {
                        _logger.LogError($"No se encontró el circuito con ID {id}.");
                        _response.ErrorMessages = new() { $"No se encontró el circuito con ID {id}." };
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        return NotFound(_response);
                    }

                    // Paso 1) Actualizar Extra votes
                    if (circuitPartyDB != null)
                    {
                        // Actualizar votos
                        circuitPartyDB.BlankVotes = dto.BlankVotes;
                        circuitPartyDB.NullVotes = dto.NullVotes;
                        circuitPartyDB.ObservedVotes = dto.ObservedVotes;
                        circuitPartyDB.RecurredVotes = dto.RecurredVotes;
                        circuitPartyDB.Step3completed = true;

                        circuitPartyDB.LastUpdateDelegadoId = dto.LastUpdateDelegadoId;
                        circuitPartyDB.ImagesUploadedCount = dto.ImagesUploadedCount;

                        // Manejar carga de fotos
                        if (photos != null && photos.Count > 0)
                        {
                            await HandlePhotoUpload(photos, circuitPartyDB);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();
                _response.Result = $"Los votos del circuito con ID {id} han sido actualizados exitosamente.";
                _response.StatusCode = HttpStatusCode.OK;

                // SignalR
                // await _hubContext.Clients.All.SendAsync("ReceiveMessage", "System", $"Se actualizó correctamente el circuito Id:{id}.");

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
        /// Origen Frontend: CircuitTable.js
        /// </summary>
        /// <param name="id">ID del circuito a actualizar.</param>
        /// <param name="dto">DTO de creación con los datos para actualizar el circuito.</param>
        /// <returns>Respuesta indicando el resultado de la operación de actualización.</returns>
        [HttpPut("{id:int}/update")]
        public async Task<ActionResult<APIResponse>> CircuitUpdate(int id, [FromBody] CircuitCreateDTO dto)
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
        /// Origen Frontend: FormExtras1.js
        /// </summary>
        /// <param name="id">ID del circuito a actualizar.</param>
        /// <param name="clientId">ID del cliente para hallar al Partido<!param>
        /// <param name="delegadoId">ID del delegado que actualiza.</param>
        /// <param name="dto">DTO para aplicar actualizaciones parciales.</param>
        /// <param name="photos">Lista de fotos nuevas a cargar.</param>
        /// <returns>Respuesta indicando el resultado de la operación de actualización parcial.</returns>
        [HttpPatch("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromQuery] int clientId, [FromQuery] int delegadoId, [FromForm] CircuitPartyPatchDTO dto, [FromForm] List<IFormFile> photos)
        {
            try
            {
                if (id <= 0 || clientId <= 0 || delegadoId <= 0 || dto == null)
                {
                    _logger.LogError(_message.NotValid());
                    _response.ErrorMessages = new() { _message.NotValid() };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var circuitDB = await _dbContext.Circuit.FirstOrDefaultAsync(c => c.Id == id);
                if (circuitDB == null)
                {
                    _logger.LogError($"No se encontró el circuito con ID {id}.");
                    _response.ErrorMessages = new() { $"No se encontró el circuito con ID {id}." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var clientDB = await _dbContext.Client
                         .Where(c => c.Id == clientId)
                         .FirstOrDefaultAsync();

                if (clientDB == null)
                {
                    _logger.LogError(((ClientMessage)_message).NotFound(clientId), clientId);
                    _response.ErrorMessages = new() { ((ClientMessage)_message).NotFound(clientId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var circuitPartyDB = _dbContext.CircuitParty.FirstOrDefault(cs => cs.CircuitId == id && cs.PartyId == clientDB.PartyId);
                if (circuitPartyDB != null)
                {
                    // Actualizar votos
                    circuitPartyDB.BlankVotes = dto.BlankVotes;
                    circuitPartyDB.NullVotes = dto.NullVotes;
                    circuitPartyDB.ObservedVotes = dto.ObservedVotes;
                    circuitPartyDB.RecurredVotes = dto.RecurredVotes;
                    circuitPartyDB.Step3completed = true;

                    circuitPartyDB.LastUpdateDelegadoId = delegadoId;

                    // Manejar carga de fotos
                    if (photos != null && photos.Count > 0)
                    {
                        await HandlePhotoUpload(photos, circuitPartyDB);
                    }
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

        /// <summary>
        /// Crea un nuevo circuito en el sistema.
        /// Origen Frontend: FormParty1.js y FormSlate1.js
        /// </summary>
        /// <param name="circuitCreateDto">Datos del nuevo circuito.</param>
        /// <returns>El circuito creado.</returns>
        [Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPost]
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

        #region Endpoints específicos

        #endregion

        #region Private methods

        /// <summary>
        /// Sube una lista de fotos.
        /// </summary>
        /// <param name="photoFiles"></param>
        /// <param name="circuitParty"></param>
        /// <returns></returns>
        private async Task HandlePhotoUpload(List<IFormFile> photoFiles, CircuitParty circuitParty)
        {
            if (photoFiles != null && photoFiles.Count > 0)
            {
                foreach (var photoFile in photoFiles)
                {
                    if (photoFile != null)
                    {
                        string dynamicContainer = $"uploads/parties/party{circuitParty.PartyId}/circuits/circuit{circuitParty.CircuitId}";
                        var newPhoto = new Photo
                        {
                            Circuit = circuitParty
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

