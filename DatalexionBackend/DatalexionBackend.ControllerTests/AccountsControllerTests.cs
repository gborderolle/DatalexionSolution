using System.Net;
using AutoMapper;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.IdentityEntities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Core.DTO;
using DatalexionBackend.EmailService;
using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Infrastructure.Services;
using DatalexionBackend.UI.Controllers.V1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Wangkanai.Detection.Services;
using System.Collections;
using Microsoft.Extensions.Configuration;

namespace DatalexionBackend.ControllerTests;

public class AccountsControllerTests
{
    private readonly Mock<ILogger<AccountsController>> _loggerMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IConfiguration> _configurationMock = new();
    private readonly Mock<IEmailSender> _emailSenderMock = new();
    private readonly Mock<IDetectionService> _detectionServiceMock = new();
    private readonly Mock<UserManager<DatalexionUser>> _userManagerMock;
    private readonly Mock<SignInManager<DatalexionUser>> _signInManagerMock;
    private readonly Mock<RoleManager<DatalexionRole>> _roleManagerMock;
    private readonly Mock<ILogService> _logServiceMock = new();
    private readonly Mock<ContextDB> _dbContextMock = new();
    private readonly Mock<IWebHostEnvironment> _environmentMock = new();
    private readonly Mock<IClientRepository> _clientRepositoryMock = new();
    private readonly Mock<IDatalexionUserRepository> _datalexionUserRepositoryMock = new();

    public AccountsControllerTests()
    {
        var userStoreMock = new Mock<IUserStore<DatalexionUser>>();
        _userManagerMock = new Mock<UserManager<DatalexionUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<DatalexionUser>>();
        _signInManagerMock = new Mock<SignInManager<DatalexionUser>>(_userManagerMock.Object, contextAccessorMock.Object, userPrincipalFactoryMock.Object, null, null, null, null);
        var roleStoreMock = new Mock<IRoleStore<DatalexionRole>>();
        _roleManagerMock = new Mock<RoleManager<DatalexionRole>>(roleStoreMock.Object, null, null, null, null);
    }

    [Fact]
    public async Task GetUsers_ReturnsOkResult()
    {
        // Arrange
        var emailConfig = new EmailConfiguration();
        var controller = new AccountsController(
            _loggerMock.Object,
            _mapperMock.Object,
            _configurationMock.Object,
            _emailSenderMock.Object,
            emailConfig,
            _detectionServiceMock.Object,
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _roleManagerMock.Object,
            _logServiceMock.Object,
            _dbContextMock.Object,
            _environmentMock.Object,
            _clientRepositoryMock.Object,
            _datalexionUserRepositoryMock.Object
        );

        _dbContextMock.Setup(x => x.DatalexionUser).Returns(DbContextMock.GetQueryableMockDbSet(new List<DatalexionUser>()));
        _mapperMock.Setup(x => x.Map<List<UserDTO>>(It.IsAny<List<DatalexionUser>>())).Returns(new List<UserDTO>());

        // Act
        var result = await controller.GetUsers(new PaginationDTO());

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetRoles_ReturnsOkResultWithRoles()
    {
        // Arrange
        var roles = new List<DatalexionRole>
    {
        new DatalexionRole { Name = "Admin" },
        new DatalexionRole { Name = "User" }
    };
        _roleManagerMock.Setup(x => x.Roles).Returns(roles.AsQueryable());

        var emailConfig = new EmailConfiguration();
        var controller = new AccountsController(
            _loggerMock.Object,
            _mapperMock.Object,
            _configurationMock.Object,
            _emailSenderMock.Object,
            emailConfig,
            _detectionServiceMock.Object,
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _roleManagerMock.Object,
            _logServiceMock.Object,
            _dbContextMock.Object,
            _environmentMock.Object,
            _clientRepositoryMock.Object,
            _datalexionUserRepositoryMock.Object
        );

        // Act
        var result = await controller.GetRoles();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<APIResponse>(okResult.Value);
        Assert.NotEmpty((IEnumerable)returnValue.Result);
    }

    [Fact]
    public async Task CreateUser_ReturnsOkResult()
    {
        // Arrange
        // Nota: Esto es un ejemplo genérico; ajusta los parámetros y los tipos según tu implementación específica.
        // _clientRepositoryMock.Setup(repo => repo.Get(It.IsAny<Expression<Func<Client, bool>>>())).ReturnsAsync(new Client());
        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<DatalexionUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _roleManagerMock.Setup(rm => rm.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new DatalexionRole());
        _userManagerMock.Setup(um => um.AddToRoleAsync(It.IsAny<DatalexionUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        var emailConfig = new EmailConfiguration();
        var controller = new AccountsController(
            _loggerMock.Object,
            _mapperMock.Object,
            _configurationMock.Object,
            _emailSenderMock.Object,
            emailConfig,
            _detectionServiceMock.Object,
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _roleManagerMock.Object,
            _logServiceMock.Object,
            _dbContextMock.Object,
            _environmentMock.Object,
            _clientRepositoryMock.Object,
            _datalexionUserRepositoryMock.Object
        );

        // Act
        var result = await controller.CreateUser(new DatalexionUserCreateDTO());

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateUser_UserExists_ReturnsOkResult()
    {
        // Arrange
        var userId = "testUserId";
        var existingUser = new DatalexionUser { Id = userId, UserName = "existingUser" };

        _userManagerMock.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(existingUser);
        _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<DatalexionUser>())).ReturnsAsync(IdentityResult.Success);

        var emailConfig = new EmailConfiguration();
        var controller = new AccountsController(
            _loggerMock.Object,
            _mapperMock.Object,
            _configurationMock.Object,
            _emailSenderMock.Object,
            emailConfig,
            _detectionServiceMock.Object,
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _roleManagerMock.Object,
            _logServiceMock.Object,
            _dbContextMock.Object,
            _environmentMock.Object,
            _clientRepositoryMock.Object,
            _datalexionUserRepositoryMock.Object
        );

        // Act
        var result = await controller.UpdateUser(userId, new DatalexionUserPatchDTO());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<APIResponse>(okResult.Value);
        Assert.Equal(HttpStatusCode.OK, returnValue.StatusCode);
    }

    [Fact]
    public async Task LoginGeneral_ValidCredentials_ReturnsOkResult()
    {
        // Arrange
        var user = new DatalexionUser { UserName = "validUser", Email = "valid@example.com" };
        var loginDto = new DatalexionUserLoginDTO { Username = user.UserName, Password = "Password123!" };

        _signInManagerMock.Setup(x => x.PasswordSignInAsync(loginDto.Username, loginDto.Password, false, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
        _userManagerMock.Setup(x => x.FindByNameAsync(user.UserName)).ReturnsAsync(user);
        _userManagerMock.Setup(x => x.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin" });

        var emailConfig = new EmailConfiguration();
        var controller = new AccountsController(
            _loggerMock.Object,
            _mapperMock.Object,
            _configurationMock.Object,
            _emailSenderMock.Object,
            emailConfig,
            _detectionServiceMock.Object,
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _roleManagerMock.Object,
            _logServiceMock.Object,
            _dbContextMock.Object,
            _environmentMock.Object,
            _clientRepositoryMock.Object,
            _datalexionUserRepositoryMock.Object
        );

        // Act
        var result = await controller.LoginGeneral(loginDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<APIResponse>(okResult.Value);
        Assert.Equal(HttpStatusCode.OK, returnValue.StatusCode);
    }



    // ----------------------------------

    private static class DbContextMock
    {
        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            return dbSet.Object;
        }
    }
}
