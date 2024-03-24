using AutoMapper;
using DatalexionBackend.Core.DTO;
using DatalexionBackend.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Wangkanai.Detection.Services;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Helpers;
using DatalexionBackend.EmailService;
using DatalexionBackend.Infrastructure.DbContext;

namespace Datalexion.Controllers.V1
{
    [ApiController]
    [HasHeader("x-version", "1")] // Agregar header: "x-version": "1"
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountsController> _logger;
        private readonly IClientRepository _clientRepository;
        private readonly IDatalexionUserRepository _datalexionUserRepository;
        private readonly IMapper _mapper;
        private readonly EmailConfiguration _emailConfiguration;
        private readonly IEmailSender _emailSender;
        private readonly IDetectionService _detectionService;
        private readonly UserManager<DatalexionUser> _userManager;
        private readonly SignInManager<DatalexionUser> _signInManager;
        private readonly RoleManager<DatalexionRole> _roleManager;
        private readonly ILogService _logService;
        private readonly ContextDB _contextDB;
        private APIResponse _response;
        private readonly IWebHostEnvironment _environment;

        public AccountsController
        (
            ILogger<AccountsController> logger,
            IMapper mapper,
            IConfiguration configuration,
            IEmailSender emailSender,
            EmailConfiguration emailConfiguration,
            IDetectionService detectionService,
            UserManager<DatalexionUser> userManager,
            SignInManager<DatalexionUser> signInManager,
            RoleManager<DatalexionRole> roleManager,
            ILogService logService,
            ContextDB dbContext,
            IWebHostEnvironment environment,
            IClientRepository clientRepository,
            IDatalexionUserRepository datalexionUserRepository
        )
        {
            _response = new();
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
            _emailSender = emailSender;
            _emailConfiguration = emailConfiguration;
            _detectionService = detectionService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logService = logService;
            _contextDB = dbContext;
            _environment = environment;
            _clientRepository = clientRepository;
            _datalexionUserRepository = datalexionUserRepository;
        }

        #region Endpoints genéricos

        [HttpGet("GetUsers")]
        public async Task<ActionResult<APIResponse>> GetUsers([FromQuery] PaginationDTO paginationDTO)
        {
            try
            {
                var queryable = _contextDB.DatalexionUser;
                await HttpContext.InsertParamPaginationHeader(queryable);
                var users = await queryable.OrderBy(x => x.UserName).DoPagination(paginationDTO).ToListAsync();
                _response.Result = users;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }

        [HttpGet("GetRoles")]
        public async Task<ActionResult<APIResponse>> GetRoles()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                _response.Result = roles;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }

        [HttpGet("GetLogs")]
        public async Task<ActionResult<APIResponse>> GetLogs([FromQuery] PaginationDTO paginationDTO)
        {
            try
            {
                var queryable = _contextDB.Log;
                await HttpContext.InsertParamPaginationHeader(queryable);
                var logs = await queryable.OrderByDescending(x => x.Creation).DoPagination(paginationDTO).ToListAsync();
                _response.Result = logs;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }

        [HttpGet("GetUserRole/{id}")]
        public async Task<ActionResult<APIResponse>> GetUserRole(string id)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                var roles = await _userManager.GetRolesAsync(user);
                _response.Result = roles;
                _response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }

        [HttpPost("makeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> MakeAdmin([FromBody] string usuarioId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(usuarioId);
                await _userManager.AddClaimAsync(user, new Claim("role", "admin"));
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }

        [HttpPost("removeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> RemoveAdmin([FromBody] string usuarioId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(usuarioId);
                await _userManager.RemoveClaimAsync(user, new Claim("role", "admin"));
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }

        [HttpPost("register")] //api/accounts/register
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> Register(RegisterModel model)
        {
            try
            {
                var client = await _contextDB.Client.FirstOrDefaultAsync(c => c.Id == 1);
                if (client == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _logger.LogError($"No existe el cliente.");
                    return NotFound(_response);
                }

                var user = new DatalexionUser
                {
                    UserName = model.Username,
                    Name = model.Name,
                    Email = model.Email,
                    ClientId = client.Id
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Registración correcta.");
                    await _logService.LogAction("Account", "Create", $"Username: {model.Username}, Rol: {model.UserRoleId}.", model.Username, client.Id);

                    _response.StatusCode = HttpStatusCode.OK;

                    // Asignar el rol al usuario
                    if (!string.IsNullOrEmpty(model.UserRoleId))
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user, model.UserRoleName);
                        if (!roleResult.Succeeded)
                        {
                            _response.IsSuccess = false;
                            _response.StatusCode = HttpStatusCode.InternalServerError;
                            _logger.LogError($"Error al asignar rol al usuario.");
                        }
                    }

                    var userCredential = new UserCredential
                    {
                        Username = user.UserName,
                        Password = model.Password
                    };
                    _response.Result = await TokenSetup(userCredential);
                }
                else
                {
                    _logger.LogError($"Registración incorrecta.");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }

        [HttpPut("UpdateUser/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> UpdateUser(string id, [FromBody] UpdateUserModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                // Actualiza los campos del usuario
                user.UserName = model.Username; // Si el email es también el nombre de usuario
                user.Email = model.Email;
                user.Name = model.Name;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                    _response.ErrorMessages = updateResult.Errors.Select(e => e.Description).ToList();
                    return BadRequest(_response);
                }

                // Actualiza el rol si es necesario
                if (!string.IsNullOrEmpty(model.UserRoleId))
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    await _userManager.AddToRoleAsync(user, model.UserRoleName);
                }

                _logger.LogInformation("Usuario actualizado.");
                await _logService.LogAction("Account", "Update", $"Username: {model.Username}, Rol: {model.UserRoleId}.", model.Username, null);

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<UserDTO>(user); // Mapea el usuario actualizado a un DTO si es necesario
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }

        [HttpPost("loginDelegados")]
        public async Task<ActionResult<APIResponse>> loginDelegados([FromBody] DelegadoCredential delegadoCredential)
        {
            try
            {
                var delegado = await _contextDB.Delegado
                    .Include(d => d.ListCircuitDelegados) // Primero, incluye la lista de circuitos delegados del delegado
                    .ThenInclude(cd => cd.Circuit) // Luego, para cada circuito delegado, incluye el circuito relacionado
                    .FirstOrDefaultAsync(d => d.CI == delegadoCredential.CI); // Filtra para encontrar el delegado con el CI específico                
                if (delegado != null && delegado.CI == delegadoCredential.CI)
                {
                    _logger.LogInformation("Login correcto.");

                    var municipalities = await _contextDB.Municipality
                        .Include(m => m.ListCircuits) // Incluye la lista de circuitos de cada municipio
                        .ThenInclude(c => c.ListCircuitDelegados) // Luego, para cada circuito, incluye la lista de circuitos delegados
                        .ThenInclude(cd => cd.Delegado) // Luego, para cada circuito delegado, incluye el delegado relacionado
                        .Where(m => m.ListCircuits.Any(c => c.ListCircuitDelegados.Any(cd => cd.DelegadoId == delegado.Id)))
                        .ToListAsync();

                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = new
                    {
                        Token = await TokenSetupDelegado(delegado),
                        UserRoles = "Delegado",
                        Fullname = delegado.Name,
                        UserId = delegado.Id,
                        ListMunicipalities = municipalities,
                        ListCircuitDelegados = delegado.ListCircuitDelegados,
                        ClientId = delegado.ClientId
                    };
                    await SendLoginNotificationDelegado(delegado);
                }
                else
                {
                    _logger.LogError($"Login incorrecto.");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest("Login incorrecto");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }

        [HttpPost("loginGeneral")]
        public async Task<ActionResult<APIResponse>> loginGeneral([FromBody] UserCredential userCredential)
        {
            try
            {
                // lockoutOnFailure: bloquea al usuario si tiene muchos intentos de logueo
                var result = await _signInManager.PasswordSignInAsync(userCredential.Username, userCredential.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Login correcto.");
                    var user = await _userManager.FindByNameAsync(userCredential.Username);
                    var roles = await _userManager.GetRolesAsync(user); // Obtener roles del usuario

                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = new
                    {
                        Token = await TokenSetup(userCredential),
                        UserRoles = roles,
                        Fullname = user.Name,
                        ClientId = user.ClientId
                    };
                    await SendLoginNotificationAdmin(userCredential);
                }
                else
                {
                    _logger.LogError($"Login incorrecto.");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest("Login incorrecto");  // respuesta genérica para no revelar información
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }


        [HttpPost("CreateUserRole")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> CreateUserRole([FromBody] CreateRoleModel model)
        {
            try
            {
                var roleExist = await _roleManager.RoleExistsAsync(model.Name);
                if (roleExist)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ErrorMessages = new List<string> { "El rol ya existe." };
                    return BadRequest(_response);
                }

                var newRole = new DatalexionRole
                {
                    Name = model.Name
                };

                var result = await _roleManager.CreateAsync(newRole);
                if (result.Succeeded)
                {
                    _response.StatusCode = HttpStatusCode.Created;
                    _response.Result = newRole;
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                    _response.ErrorMessages = result.Errors.Select(e => e.Description).ToList();
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.ToString() };
                return StatusCode(500, _response);
            }
            return Ok(_response);
        }

        [HttpPut("UpdateUserRole/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<APIResponse>> UpdateUserRole(string id, [FromBody] UpdateUserRoleModel model)
        {
            try
            {
                var userRole = await _roleManager.FindByIdAsync(id);
                if (userRole == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                userRole.Name = model.Name;
                var updateResult = await _roleManager.UpdateAsync(userRole);
                if (!updateResult.Succeeded)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.InternalServerError;
                    _response.ErrorMessages = updateResult.Errors.Select(e => e.Description).ToList();
                    return BadRequest(_response);
                }

                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<UserDTO>(userRole); // Mapea el usuario actualizado a un DTO si es necesario
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.InternalServerError;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return Ok(_response);
        }

        #endregion

        #region Endpoints específicos

        [HttpGet("GetUsersByClient")]
        public async Task<ActionResult<APIResponse>> GetUsersByClient([FromQuery] int clientId)
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

                var usersList = await _datalexionUserRepository.GetAll(v => v.ClientId == clientId);

                _response.Result = _mapper.Map<List<DatalexionUserDTO>>(usersList);
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

        [HttpGet("GetLogsByClient")]
        public async Task<ActionResult<APIResponse>> GetLogsByClient([FromQuery] int clientId)
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

                //var usersList = await _logger.GetAll(v => v.ClientId == clientId);

                var queryable = _contextDB.Log;
                await HttpContext.InsertParamPaginationHeader(queryable);
                var logs = await queryable.OrderByDescending(x => x.Creation).ToListAsync();

                _response.Result = _mapper.Map<List<DatalexionUserDTO>>(logs);
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

        #endregion Endpoints específicos

        #region Private methods

        private async Task<AuthenticationResponse> TokenSetup(UserCredential userCredential)
        {
            var user = await _userManager.FindByNameAsync(userCredential.Username);
            if (user == null)
            {
                return null;
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.Email)
    };

            // Añadir los claims que ya existen en la base de datos
            claims.AddRange(userClaims);

            // Añadir los roles del usuario como claims
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            var token = new JwtSecurityToken(
                issuer: null, // Opcionalmente añadir el emisor del token
                audience: null, // Opcionalmente añadir la audiencia del token
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        private Task<AuthenticationResponse> TokenSetupDelegado(Delegado user)
        {
            var claims = new List<Claim>
                {
                    new Claim("ci", user.CI),
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return Task.FromResult(new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            });
        }

        #region Email

        private async Task SendLoginNotificationAdmin(UserCredential userCredential)
        {
            // Comprueba si el entorno es de producción
            //if (!_environment.IsProduction())
            //{
            //    return;
            //}

            var user = await _userManager.FindByNameAsync(userCredential.Username);
            if (user == null)
            {
                _logger.LogError("Usuario no encontrado para la notificación de inicio de sesión.");
                return;
            }

            // Comprueba si el usuario tiene el rol de administrador
            var userRoles = await _userManager.GetRolesAsync(user);
            // if (userRoles.Contains("Admin"))
            if (userRoles.Count == 0)
            {
                return;
            }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            string? clientIP = HttpContext.Connection.RemoteIpAddress?.ToString();
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            string? clientIPCity = await GetIpInfo(clientIP);
            bool isMobile = _detectionService.Device.Type == Wangkanai.Detection.Models.Device.Mobile;
            await SendAsyncEmail(userCredential.Username, clientIP, clientIPCity, isMobile, userRoles[0]);
        }

        private async Task SendLoginNotificationDelegado(Delegado delegado)
        {
            // Comprueba si el entorno es de producción
            // if (!_environment.IsProduction())
            // {
            //     return;
            // }

            if (delegado == null)
            {
                _logger.LogError("Delegado no encontrado para la notificación de inicio de sesión.");
                return;
            }

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            string? clientIP = HttpContext.Connection.RemoteIpAddress?.ToString();
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            string? clientIPCity = await GetIpInfo(clientIP);
            bool isMobile = _detectionService.Device.Type == Wangkanai.Detection.Models.Device.Mobile;
            await SendAsyncEmail(delegado.CI, clientIP, clientIPCity, isMobile, "Delegado");
        }

        private static async Task<string?> GetIpInfo(string? Ip_Api_Url)
        {
            string? returnString = string.Empty;
            if (!string.IsNullOrWhiteSpace(Ip_Api_Url) && Ip_Api_Url != "::1")
            {
                using (HttpClient httpClient = new())
                {
                    try
                    {
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage httpResponse = await httpClient.GetAsync("http://ip-api.com/json/" + Ip_Api_Url);
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            var geolocationInfo = await httpResponse.Content.ReadFromJsonAsync<LocationDetails_IpApi>();
                            returnString = geolocationInfo?.city;
                        }
                    }
                    catch (Exception)
                    {
                        //ServiceLog.AddException("Excepcion. Obteniendo info de IP al login. ERROR: " + ex.Message, MethodBase.GetCurrentMethod()?.DeclaringType?.Name, MethodBase.GetCurrentMethod()?.Name, ex.Message);
                    }
                }
            }
            return returnString;
        }

        private async Task SendAsyncEmail(string username, string? clientIP, string? clientIPCity, bool isMobile, string role = "")
        {
            // string emailNotificationDestination = _configuration["NotificationEmail:To"];
            // string emailNotificationSubject = _configuration["NotificationEmail:Subject"];
            string emailNotificationDestination = _emailConfiguration.To;
            string emailNotificationSubject = _emailConfiguration.Subject;

            string emailNotificationBody = GlobalServices.GetEmailNotificationBody(username, clientIP, clientIPCity, isMobile, role);
            var message = new Message(new string[] { emailNotificationDestination }, emailNotificationSubject, emailNotificationBody);
            await _emailSender.SendEmailAsync(message);
        }

        private class LocationDetails_IpApi
        {
            public string? query { get; set; }
            public string? city { get; set; }
            public string? country { get; set; }
            public string? countryCode { get; set; }
            public string? isp { get; set; }
            public double lat { get; set; }
            public double lon { get; set; }
            public string? org { get; set; }
            public string? region { get; set; }
            public string? regionName { get; set; }
            public string? status { get; set; }
            public string? timezone { get; set; }
            public string? zip { get; set; }
        }

        #endregion Email

        public class RegisterModel
        {
            public string Username { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string UserRoleId { get; set; }
            public string UserRoleName { get; set; }
        }

        public class UpdateUserModel
        {
            public string Username { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string UserRoleId { get; set; }
            public string UserRoleName { get; set; }
        }

        public class CreateRoleModel
        {
            public string Name { get; set; }
        }

        public class UpdateUserRoleModel
        {
            public string Name { get; set; }
        }

        public class DelegadoCredential
        {
            [Required]
            public string CI { get; set; }
        }

        #endregion

    }
}