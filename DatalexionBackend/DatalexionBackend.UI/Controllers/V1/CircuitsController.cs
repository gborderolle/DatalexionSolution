﻿using DatalexionBackend.Infrastructure.DbContext;
using AutoMapper;
using DatalexionBackend.Core.DTO;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Infrastructure.Services;
using DatalexionBackend.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.SignalR;

namespace DatalexionBackend.UI.Controllers.V1
{
    [ApiController]
    [HasHeader("x-version", "1")]
    [Route("api/circuits")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Analyst")]
    public class CircuitsController : CustomBaseController<Circuit>
    {
        private readonly ICircuitRepository _circuitRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;
        private readonly IFileStorage _fileStorage;
        private readonly IHubContext<NotifyHub> _hubContext;

        public CircuitsController(ILogger<CircuitsController> logger, IMapper mapper, ICircuitRepository circuitRepository, IPhotoRepository photoRepository, IFileStorage fileStorage, ContextDB dbContext, ILogService logService, IHubContext<NotifyHub> hubContext)
            : base(mapper, logger, circuitRepository)
        {
            _circuitRepository = circuitRepository;
            _photoRepository = photoRepository;
            _fileStorage = fileStorage;
            _dbContext = dbContext;
            _logService = logService;
            _hubContext = hubContext;
        }

        #region Endpoints genéricos

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

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> All()
        {
            var circuits = await _circuitRepository.GetAll();
            _response.Result = _mapper.Map<List<CircuitDTO>>(circuits);
            _response.StatusCode = HttpStatusCode.OK;
            return _response;
        }

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

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Circuit>(id);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromBody] CircuitCreateDTO circuitCreateDTO)
        {
            try
            {
                if (circuitCreateDTO == null || id <= 0)
                {
                    _logger.LogError(Messages.Generic.NotValid);
                    _response.ErrorMessages = new List<string> { Messages.Generic.NotValid };
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
                    _response.ErrorMessages = new List<string> { $"No se encontró el circuito con ID {id}." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var client = await _dbContext.Client.FirstOrDefaultAsync();
                if (client == null)
                {
                    _logger.LogError($"No se encontró el cliente.");
                    _response.ErrorMessages = new List<string> { $"No se encontró el cliente." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var party = await _dbContext.Party.FindAsync(client?.PartyId);
                if (party == null)
                {
                    _logger.LogError($"No se encontró el partido con ID {id}.");
                    _response.ErrorMessages = new List<string> { $"No se encontró el partido con ID {id}." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // Paso 1) Actualizar CircuitSlates
                if (circuitCreateDTO.ListCircuitSlates != null && circuitCreateDTO.ListCircuitSlates.Any())
                {
                    var existingSlates = _circuitDB.ListCircuitSlates.ToList(); // Hacer una copia de la lista para evitar modificar la colección durante la iteración.
                    foreach (var circuitSlateDTO in circuitCreateDTO.ListCircuitSlates)
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
                    _circuitDB.ListCircuitSlates = _circuitDB.ListCircuitSlates.Where(cs => circuitCreateDTO.ListCircuitSlates.Any(dto => dto.SlateId == cs.SlateId)).ToList();
                }

                // Paso 2) Actualizar CircuitParties
                if (circuitCreateDTO.ListCircuitParties != null && circuitCreateDTO.ListCircuitParties.Any())
                {
                    var existingCircuitParties = _circuitDB.ListCircuitParties.ToList(); // Hacer una copia de la lista para evitar modificar la colección durante la iteración.
                    foreach (var circuitPartyDTO in circuitCreateDTO.ListCircuitParties)
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
                    _circuitDB.ListCircuitParties = _circuitDB.ListCircuitParties.Where(cs => circuitCreateDTO.ListCircuitParties.Any(dto => dto.PartyId == cs.PartyId)).ToList();
                }

                // Paso 3) Actualizar Extra votes y Step actual
                if (circuitCreateDTO != null)
                {
                    _circuitDB.BlankVotes = circuitCreateDTO.BlankVotes;
                    _circuitDB.NullVotes = circuitCreateDTO.NullVotes;
                    _circuitDB.ObservedVotes = circuitCreateDTO.ObservedVotes;
                    _circuitDB.RecurredVotes = circuitCreateDTO.RecurredVotes;

                    _circuitDB.Step1completed = circuitCreateDTO.Step1completed;
                    _circuitDB.Step2completed = circuitCreateDTO.Step2completed;
                    _circuitDB.Step3completed = circuitCreateDTO.Step3completed;
                }

                // Fotos
                if (circuitCreateDTO.ListPhotos != null && circuitCreateDTO.ListPhotos.Count > 0)
                {
                    // En el método de creación o actualización de un circuito:
                    await HandlePhotoUpload(circuitCreateDTO.ListPhotos, _circuitDB);
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
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPut("{id:int}/update")]
        public async Task<ActionResult<APIResponse>> UpdateCircuit(int id, [FromBody] CircuitCreateDTO circuitCreateDTO)
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

                var circuit = await _circuitRepository.Get(v => v.Id == id);
                if (circuit == null)
                {
                    _logger.LogError(string.Format(Messages.Circuit.NotFound, id), id);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Circuit.NotFound, id) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // No usar AutoMapper para mapear todo el objeto, sino actualizar campo por campo
                circuit.Name = Utils.ToCamelCase(circuitCreateDTO.Name);
                circuit.Comments = Utils.ToCamelCase(circuitCreateDTO.Comments);
                circuit.Update = DateTime.Now;

                var updateCircuit = await _circuitRepository.Update(circuit);

                _logger.LogInformation(string.Format(Messages.Circuit.ActionLog, id, circuit.Name), id);
                await _logService.LogAction("Circuit", "Update", string.Format(Messages.Circuit.ActionLog, id, circuit.Name), User.Identity.Name, null);

                _response.Result = _mapper.Map<ProvinceDTO>(updateCircuit);
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
        [Consumes("multipart/form-data")] // Indicar que el método aceptará multipart/form-data
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromForm] CircuitPatchDTO circuitPatchDTO, [FromForm] List<IFormFile> photos)
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

                var _circuitDB = await _dbContext.Circuit.Include(c => c.ListPhotos).FirstOrDefaultAsync(c => c.Id == id);
                if (_circuitDB == null)
                {
                    _logger.LogError($"No se encontró el circuito con ID {id}.");
                    _response.ErrorMessages = new List<string> { $"No se encontró el circuito con ID {id}." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // Actualizar votos
                _circuitDB.BlankVotes = circuitPatchDTO.BlankVotes;
                _circuitDB.NullVotes = circuitPatchDTO.NullVotes;
                _circuitDB.ObservedVotes = circuitPatchDTO.ObservedVotes;
                _circuitDB.RecurredVotes = circuitPatchDTO.RecurredVotes;
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
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return BadRequest(_response);
            }
        }

        #endregion

        #region Endpoints específicos

        [HttpPost(Name = "CreateCircuit")]
        public async Task<ActionResult<APIResponse>> Post([FromBody] CircuitCreateDTO circuitCreateDto)
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
                if (await _circuitRepository.Get(v => v.Name.ToLower() == circuitCreateDto.Name.ToLower()) != null)
                {
                    _logger.LogError($"El nombre {circuitCreateDto.Name} ya existe en el sistema");
                    _response.ErrorMessages = new List<string> { $"El nombre {circuitCreateDto.Name} ya existe en el sistema." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("NameAlreadyExists", $"El nombre {circuitCreateDto.Name} ya existe en el sistema.");
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

                _logger.LogInformation($"Se creó correctamente el circuit Id:{circuit.Id}.");
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
                _response.ErrorMessages = new List<string> { ex.ToString() };
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
