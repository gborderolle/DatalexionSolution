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
using System.Net;
using DatalexionBackend.Core.Enums;

namespace DatalexionBackend.UI.Controllers.V1
{
    //[Authorize(Roles = nameof(UserTypeOptions.Admin) + "," + nameof(UserTypeOptions.Analyst))]
    [ApiController]
    [HasHeader("x-version", "1")]
    [Route("api/participants")]
    public class ParticipantsController : CustomBaseController<Participant>
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;
        private readonly IMessage<Participant> _message;

        public ParticipantsController(ILogger<ParticipantsController> logger, IMapper mapper, IParticipantRepository participantRepository, ContextDB dbContext, ILogService logService, IMessage<Participant> message)
        : base(mapper, logger, participantRepository)
        {
            _response = new();
            _participantRepository = participantRepository;
            _dbContext = dbContext;
            _logService = logService;
            _message = message;
            _message = message;
        }

        #region Endpoints genéricos

        [HttpGet("GetParticipant")]
        [ResponseCache(Duration = 20)]
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
        [ResponseCache(Duration = 20)]
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

        //[Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Participant>(id);
        }

        //[Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromBody] ParticipantCreateDTO dto)
        {
            return await Put<ParticipantCreateDTO, ParticipantDTO, Participant>(id, dto);
        }

        //[Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromBody] JsonPatchDocument<ParticipantPatchDTO> dto)
        {
            return new APIResponse { StatusCode = HttpStatusCode.NotImplemented };
        }

        #endregion

        #region Endpoints específicos

        //[Authorize(Roles = nameof(UserTypeOptions.Admin))]
        [HttpPost]
        public async Task<ActionResult<APIResponse>> Post([FromBody] ParticipantCreateDTO dto)
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
                if (await _participantRepository.Get(v => v.Name.ToLower() == dto.Name.ToLower()) != null)
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

                Participant participant = _mapper.Map<Participant>(dto);
                participant.Creation = DateTime.Now;
                participant.Update = DateTime.Now;

                await _participantRepository.Create(participant);

                _logger.LogInformation(_message.Created(participant.Id, participant.Name));
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

