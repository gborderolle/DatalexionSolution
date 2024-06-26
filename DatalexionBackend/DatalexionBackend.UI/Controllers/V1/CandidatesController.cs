﻿using AutoMapper;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Core.DTO;
using DatalexionBackend.Core.Helpers;
using DatalexionBackend.Infrastructure.MessagesService;
using DatalexionBackend.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using DatalexionBackend.Core.Enums;
using System.Net;

namespace DatalexionBackend.UI.Controllers.V1
{
    // //[Authorize(Roles = nameof(UserTypeOptions.Admin) + "," + nameof(UserTypeOptions.Analyst))]
    [ApiController]
    [HasHeader("x-version", "1")]
    [Route("api/candidates")]
    public class CandidatesController : CustomBaseController<Candidate>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IWingRepository _wingRepository;
        private readonly ISlateRepository _slateRepository;
        private readonly ILogService _logService;
        private readonly IFileStorage _fileStorage;
        private readonly IPhotoRepository _photoRepository;
        private readonly IMessage<Candidate> _message;

        public CandidatesController(ILogger<CandidatesController> logger, IMapper mapper, ICandidateRepository candidateRepository, ILogService logService, IFileStorage fileStorage, IPhotoRepository photoRepository, IClientRepository clientRepository, IWingRepository wingRepository, ISlateRepository slateRepository, IMessage<Candidate> message)
        : base(mapper, logger, candidateRepository)
        {
            _response = new();
            _candidateRepository = candidateRepository;
            _logService = logService;
            _fileStorage = fileStorage;
            _photoRepository = photoRepository;
            _clientRepository = clientRepository;
            _wingRepository = wingRepository;
            _slateRepository = slateRepository;
            _message = message;
        }

        #region Endpoints genéricos

        /// <summary>
        /// Obtiene la lista de candidatos paginada, incluyendo sus fotos.
        /// </summary>
        /// <param name="paginationDTO">Datos de paginación.</param>
        /// <returns>Lista paginada de candidatos.</returns>
        [HttpGet("GetCandidate")] // url completa: https://localhost:7003/api/Candidates/GetCandidate
        [ResponseCache(Duration = 20)]
        public async Task<ActionResult<APIResponse>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var includes = new List<IncludePropertyConfiguration<Candidate>>
            {
                    new IncludePropertyConfiguration<Candidate>
                    {
                        IncludeExpression = b => b.Photo
                    },
                };
            return await Get<Candidate, CandidateDTO>(paginationDTO: paginationDTO, includes: includes);
        }

        /// <summary>
        /// Obtiene todos los candidatos sin aplicar paginación.
        /// </summary>
        /// <returns>Todos los candidatos.</returns>
        [HttpGet("all")] // url completa: https://localhost:7003/api/Candidates/all
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> All()
        {
            var candidates = await _candidateRepository.GetAll();
            _response.Result = _mapper.Map<List<CandidateDTO>>(candidates);
            _response.StatusCode = HttpStatusCode.OK;
            return _response;
        }

        /// <summary>
        /// Obtiene un candidato específico por su ID, incluyendo su foto.
        /// </summary>
        /// <param name="id">ID del candidato.</param>
        /// <returns>Un candidato específico.</returns>
        [HttpGet("{id:int}")] // url completa: https://localhost:7003/api/Candidates/1
        [ResponseCache(Duration = 20)]
        public async Task<ActionResult<APIResponse>> Get([FromRoute] int id)
        {
            var includes = new List<IncludePropertyConfiguration<Candidate>>
            {
                    new IncludePropertyConfiguration<Candidate>
                    {
                        IncludeExpression = b => b.Photo
                    },
                };
            return await GetById<Candidate, CandidateDTO>(id, includes: includes);
        }

        /// <summary>
        /// Elimina un candidato específico basado en su ID.
        /// </summary>
        /// <param name="id">ID del candidato a eliminar.</param>
        /// <returns>Resultado de la operación de eliminación.</returns>
        //[Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Candidate>(id);
        }

        /// <summary>
        /// Actualiza los datos de un candidato existente, incluyendo la posibilidad de actualizar su foto.
        /// </summary>
        /// <param name="id">ID del candidato a actualizar.</param>
        /// <param name="dto">Datos actualizados del candidato.</param>
        /// <param name="file">Archivo de foto nueva del candidato.</param>
        /// <returns>El candidato actualizado.</returns>
        //[Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPut("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromForm] CandidateCreateDTO dto, IFormFile file)
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

                var candidate = await _candidateRepository.Get(v => v.Id == id);
                if (candidate == null)
                {
                    _logger.LogError(_message.NotFound(id));
                    _response.ErrorMessages = new() { _message.NotFound(id) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // No usar AutoMapper para mapear todo el objeto, sino actualizar campo por campo
                candidate.Name = Utils.ToCamelCase(dto.Name);
                candidate.Comments = Utils.ToCamelCase(dto.Comments);
                candidate.Update = DateTime.Now;

                // Manejar carga de fotos
                if (file != null)
                {
                    await HandlePhotoUpload(file, candidate);
                }

                var updatedCandidate = await _candidateRepository.Update(candidate);

                _logger.LogInformation(_message.ActionLog(id, candidate.Name));
                await _logService.LogAction("Candidate", "Update", _message.ActionLog(id, candidate.Name), User.Identity.Name, null);

                _response.Result = _mapper.Map<CandidateDTO>(updatedCandidate);
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
        /// Aplica cambios parciales a un candidato existente.
        /// </summary>
        /// <param name="id">ID del candidato a modificar.</param>
        /// <param name="dto">Documento JSON con los cambios a aplicar.</param>
        /// <returns>El candidato con los cambios aplicados.</returns>
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromBody] JsonPatchDocument<CandidatePatchDTO> dto)
        {
            return new APIResponse { StatusCode = HttpStatusCode.NotImplemented };
        }

        #endregion

        #region Endpoints específicos

        /// <summary>
        /// Obtiene candidatos asociados a un cliente específico, considerando las relaciones entre clientes, alas, listas y candidatos.
        /// </summary>
        /// <param name="clientId">ID del cliente.</param>
        /// <returns>Lista de candidatos asociados al cliente especificado.</returns>
        [HttpGet("GetCandidatesByClient")] // url completa: https://localhost:7003/api/Candidates/GetCandidatesByClient?clientId=1
        [ResponseCache(Duration = 20)]
        public async Task<ActionResult<APIResponse>> GetCandidatesByClient([FromQuery] int clientId)
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

                var wings = await _wingRepository.GetAll(w => w.PartyId == client.Party.Id);

                // Obtiene las listas (slates) asociadas a las alas
                var slates = new List<Slate>();
                foreach (var wing in wings)
                {
                    var wingSlates = await _slateRepository.GetAll(s => s.WingId == wing.Id);
                    slates.AddRange(wingSlates);
                }

                // Obtiene los candidatos de las listas
                var candidates = new List<Candidate>();
                foreach (var slate in slates)
                {
                    var includes = new List<IncludePropertyConfiguration<Candidate>>
                    {
                        new IncludePropertyConfiguration<Candidate>
                        {
                            IncludeExpression = c => c.Photo
                        },
                    };

                    var slateCandidates = await _candidateRepository.GetAll(c => c.Id == slate.CandidateId, includes: includes);
                    candidates.AddRange(slateCandidates);
                }

                // Elimina candidatos duplicados basándose en el ID del candidato
                var uniqueCandidates = candidates.DistinctBy(c => c.Id).ToList();

                // Mapea los candidatos únicos a DTOs para la respuesta
                _response.Result = _mapper.Map<List<CandidateDTO>>(uniqueCandidates);
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
        /// Crea un nuevo candidato en el sistema.
        /// </summary>
        /// <param name="dto">Datos del nuevo candidato.</param>
        /// <returns>El candidato creado.</returns>
        //[Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPost] // url completa: https://localhost:7003/api/Candidates
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<APIResponse>> Post([FromForm] CandidateCreateDTO dto, IFormFile file)
        {
            return Ok();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sube una foto sola.
        /// </summary>
        /// <param name="photoFile"></param>
        /// <param name="candidate"></param>
        /// <returns></returns>
        private async Task HandlePhotoUpload(IFormFile photoFile, Candidate candidate)
        {
            if (photoFile != null)
            {
                string dynamicContainer = $"uploads/candidates/candidate{candidate.Id}";
                var newPhoto = new Photo
                {
                    Candidate = candidate
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
