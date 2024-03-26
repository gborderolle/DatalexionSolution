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
    [Route("api/participants")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Analyst")]
    public class ParticipantsController : CustomBaseController<Participant>
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;

        public ParticipantsController(ILogger<ParticipantsController> logger, IMapper mapper, IParticipantRepository participantRepository, ContextDB dbContext, ILogService logService)
        : base(mapper, logger, participantRepository)
        {
            _response = new();
            _participantRepository = participantRepository;
            _dbContext = dbContext;
            _logService = logService;
        }

        #region Endpoints genéricos

        [HttpGet("GetParticipant")]
        public async Task<ActionResult<APIResponse>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Participant>>
            {
                    new IncludePropertyConfiguration<Participant>
                    {
                        IncludeExpression = b => b.Slate
                    }
                };
            return await Get<Participant, ParticipantDTO>(paginationDTO: paginationDTO, includes: includes);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> All()
        {
            var participants = await _participantRepository.GetAll();
            _response.Result = _mapper.Map<List<ParticipantDTO>>(participants);
            _response.StatusCode = HttpStatusCode.OK;
            return _response;
        }

        [HttpGet("{id:int}")] // url completa: https://localhost:7003/api/Participants/1
        public async Task<ActionResult<APIResponse>> Get([FromRoute] int id)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Participant>>
            {
                    new IncludePropertyConfiguration<Participant>
                    {
                        IncludeExpression = b => b.Slate
                    }
                };
            return await GetById<Participant, ParticipantDTO>(id, includes: includes);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Participant>(id);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromBody] ParticipantCreateDTO participantCreateDTO)
        {
            return await Put<ParticipantCreateDTO, ParticipantDTO, Participant>(id, participantCreateDTO);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromBody] JsonPatchDocument<ParticipantPatchDTO> patchDto)
        {
            return await Patch<Participant, ParticipantPatchDTO>(id, patchDto);
        }

        #endregion

        #region Endpoints específicos

        [HttpPost(Name = "CreateParticipant")]
        public async Task<ActionResult<APIResponse>> Post([FromBody] ParticipantCreateDTO participantCreateDto)
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
                if (await _participantRepository.Get(v => v.Name.ToLower() == participantCreateDto.Name.ToLower()) != null)
                {
                    _logger.LogError($"El nombre {participantCreateDto.Name} ya existe en el sistema");
                    _response.ErrorMessages = new List<string> { $"El nombre {participantCreateDto.Name} ya existe en el sistema." };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    ModelState.AddModelError("NameAlreadyExists", $"El nombre {participantCreateDto.Name} ya existe en el sistema.");
                    return BadRequest(ModelState);
                }

                participantCreateDto.Name = Utils.ToCamelCase(participantCreateDto.Name);
                participantCreateDto.Comments = Utils.ToCamelCase(participantCreateDto.Comments);

                Participant participant = _mapper.Map<Participant>(participantCreateDto);
                participant.Creation = DateTime.Now;
                participant.Update = DateTime.Now;

                await _participantRepository.Create(participant);

                _logger.LogInformation($"Se creó correctamente el participant Id:{participant.Id}.");
                await _logService.LogAction("Participant", "Create", $"Id:{participant.Id}, Nombre: {participant.Name}.", User.Identity.Name, null);

                _response.Result = _mapper.Map<ParticipantDTO>(participant);
                _response.StatusCode = HttpStatusCode.Created;

                // CreatedAtRoute -> Nombre de la ruta (del método): GetCountryDSById
                // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/13816172#notes
                return CreatedAtAction(nameof(Get), new { id = participant.Id }, _response);
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

