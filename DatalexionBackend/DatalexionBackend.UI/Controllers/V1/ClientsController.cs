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
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DatalexionBackend.UI.Controllers.V1
{
    [ApiController]
    [HasHeader("x-version", "1")]
    [Route("api/clients")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Analyst")]
    public class ClientsController : CustomBaseController<Client>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IDatalexionUserRepository _datalexionUserRepository;
        private readonly IDelegadoRepository _delegadoRepository;
        private readonly ContextDB _dbContext;
        private readonly ILogService _logService;

        public ClientsController(ILogger<ClientsController> logger, IMapper mapper, IClientRepository clientRepository, ContextDB dbContext, ILogService logService, IDatalexionUserRepository datalexionUserRepository, IDelegadoRepository delegadoRepository)
        : base(mapper, logger, clientRepository)
        {
            _response = new();
            _clientRepository = clientRepository;
            _dbContext = dbContext;
            _logService = logService;
            _datalexionUserRepository = datalexionUserRepository;
            _delegadoRepository = delegadoRepository;
        }

        #region Endpoints genéricos

        [HttpGet("GetClient")]
        public async Task<ActionResult<APIResponse>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            // 1..n
            var includes = new List<IncludePropertyConfiguration<Client>>
            {
                new IncludePropertyConfiguration<Client>
                {
                    IncludeExpression = b => b.Party
                },
                new IncludePropertyConfiguration<Client>
                {
                    IncludeExpression = b => b.ListUsers
                },
            };
            // 1..n..n
            var thenIncludes = new List<ThenIncludePropertyConfiguration<Client>>
            {
                new ThenIncludePropertyConfiguration<Client>
                {
                    IncludeExpression = b => b.Party,
                    ThenIncludeExpression = ab => ((Party)ab).PhotoLong
                },
                new ThenIncludePropertyConfiguration<Client>
                {
                    IncludeExpression = b => b.Party,
                    ThenIncludeExpression = ab => ((Party)ab).PhotoShort
                },
            };
            return await Get<Client, ClientDTO>(paginationDTO: paginationDTO, includes: includes, thenIncludes: thenIncludes);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> All()
        {
            var clients = await _clientRepository.GetAll();
            _response.Result = _mapper.Map<List<ClientDTO>>(clients);
            _response.StatusCode = HttpStatusCode.OK;
            return _response;
        }

        [HttpGet("{id:int}")] // url completa: https://localhost:7003/api/Clients/1
        public async Task<ActionResult<APIResponse>> Get([FromRoute] int id)
        {
            var includes = new List<IncludePropertyConfiguration<Client>>
            {
                new IncludePropertyConfiguration<Client>
                {
                    IncludeExpression = b => b.Party
                },
                new IncludePropertyConfiguration<Client>
                {
                    IncludeExpression = b => b.ListUsers
                }
            };
            // 1..n..n
            var thenIncludes = new List<ThenIncludePropertyConfiguration<Client>>
            {
                new ThenIncludePropertyConfiguration<Client>
                {
                    IncludeExpression = b => b.Party,
                    ThenIncludeExpression = ab => ((Party)ab).PhotoLong
                },
                new ThenIncludePropertyConfiguration<Client>
                {
                    IncludeExpression = b => b.Party,
                    ThenIncludeExpression = ab => ((Party)ab).PhotoShort
                },
            };
            return await GetById<Client, ClientDTO>(id, includes: includes, thenIncludes: thenIncludes);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIResponse>> Delete([FromRoute] int id)
        {
            return await Delete<Client>(id);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<APIResponse>> Put(int id, [FromBody] ClientCreateDTO clientCreateDTO)
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
                var includes = new List<IncludePropertyConfiguration<Client>>
            {
                 new IncludePropertyConfiguration<Client>
                    {
                        IncludeExpression = b => b.Party
                    },
                };

                var client = await _clientRepository.Get(v => v.Id == id, includes: includes);
                if (client == null)
                {
                    _logger.LogError(string.Format(Messages.Client.NotFound, id), id);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Client.NotFound, id) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                client.Party = await _dbContext.Party.FindAsync(clientCreateDTO.PartyId);
                if (client.Party == null)
                {
                    _logger.LogError(string.Format(Messages.Party.NotFound, clientCreateDTO.PartyId), clientCreateDTO.PartyId);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Party.NotFound, clientCreateDTO.PartyId) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // No usar AutoMapper para mapear todo el objeto, sino actualizar campo por campo
                client.Name = Utils.ToCamelCase(clientCreateDTO.Name);
                client.Comments = Utils.ToCamelCase(clientCreateDTO.Comments);
                client.Update = DateTime.Now;

                client.PartyId = clientCreateDTO.PartyId;
                client.Party = await _dbContext.Party.FindAsync(clientCreateDTO.PartyId);

                var updatedClient = await _clientRepository.Update(client);

                _logger.LogInformation(string.Format(Messages.Client.ActionLog, id, client.Name), id);
                await _logService.LogAction("Client", "Update", string.Format(Messages.Client.ActionLog, id, client.Name), User.Identity.Name, null);

                _response.Result = _mapper.Map<ClientDTO>(updatedClient);
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
        public async Task<ActionResult<APIResponse>> Patch(int id, [FromBody] JsonPatchDocument<ClientPatchDTO> patchDto)
        {
            return await Patch<Client, ClientPatchDTO>(id, patchDto);
        }

        #endregion

        #region Endpoints específicos

        [HttpGet("GetUserClient")]
        public async Task<ActionResult<APIResponse>> GetUserClient([FromQuery] string username)
        {
            try
            {
                int clientId = 0;
                var datalexionUser = await _datalexionUserRepository.Get(v => v.UserName == username);
                if (datalexionUser == null)
                {
                    var delegado = await _delegadoRepository.Get(v => v.CI == username);
                    if (delegado == null)
                    {
                        _logger.LogError(string.Format(Messages.Delegados.NotFoundUsername, username), username);
                        _response.ErrorMessages = new List<string> { string.Format(Messages.Delegados.NotFoundUsername, username) };
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.NotFound;
                        return NotFound(_response);
                    }
                    clientId = delegado.ClientId;
                }
                else
                {
                    clientId = datalexionUser.ClientId;
                }

                var client = await _dbContext.Client
                                           .Where(c => c.Id == clientId)
                                           .Include(c => c.Party)
                                           .ThenInclude(p => p.PhotoLong)
                                           .Include(c => c.Party)
                                           .ThenInclude(p => p.PhotoShort)
                                           .Include(c => c.ListUsers)
                                           .FirstOrDefaultAsync();

                if (client == null)
                {
                    _logger.LogError(string.Format(Messages.Client.NotFoundUsername, username), username);
                    _response.ErrorMessages = new List<string> { string.Format(Messages.Client.NotFoundUsername, username) };
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<ClientDTO>(client);
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

        [HttpPost(Name = "CreateClient")]
        public async Task<ActionResult<APIResponse>> Post([FromBody] ClientCreateDTO clientCreateDto)
        {
            return Ok();
        }

        #endregion

        #region Private methods

        #endregion

    }
}
