﻿using DatalexionBackend.Core.ApiBehavior;
using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Domain.IdentityEntities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Core.Enums;
using DatalexionBackend.Core.Filters;
using DatalexionBackend.Core.Helpers;
using DatalexionBackend.Core.Services;
using DatalexionBackend.EmailService;
using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Infrastructure.MessagesService;
using DatalexionBackend.Infrastructure.Repositories;
using DatalexionBackend.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace DatalexionBackend.UI.StartupExtensions;

/// <summary>
/// Extension method to configure services
/// s: https://www.udemy.com/course/asp-net-core-true-ultimate-guide-real-project/learn/lecture/34524780#overview
/// </summary>
public static class ConfigureServicesExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region Swagger

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Datalexion 3.0",
                Version = "v1",
                Description = "Inteligencia Artificial analítica para optimizar y procesar la información electoral en tiempo real.",
                TermsOfService = new Uri("https://datalexion.uy/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Soporte",
                    Email = "hola@datalexion.uy",
                    Url = new Uri("https://datalexion.uy/support"),
                },
                License = new OpenApiLicense
                {
                    Name = "Uso bajo licencia 2024.",
                    Url = new Uri("https://datalexion.uy"),
                }
            });
            // Include XML comments
            var xmlFile = "api.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        #endregion

        // Configuración de servicios
        services.AddControllers(options =>
        {
            // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/13816116#notes
            options.Filters.Add(typeof(ExceptionFilter));
            options.Filters.Add(typeof(BadRequestParse));
            options.Conventions.Add(new SwaggerGroupByVersion());
        })
        .ConfigureApiBehaviorOptions(BehaviorBadRequests.Parse)
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // para arreglar errores de loop de relaciones 1..n y viceversa
        });

        services.AddAutoMapper(typeof(AutoMapperProfiles));

        #region Registro de servicios

        // --------------

        // AddTransient: cambia dentro del contexto
        // AddScoped: se mantiene dentro del contexto (mejor para los servicios)
        // AddSingleton: no cambia nunca

        // Repositorios
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<ICircuitRepository, CircuitRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IDatalexionUserRepository, DatalexionUserRepository>();
        services.AddScoped<IDelegadoRepository, DelegadoRepository>();
        services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        services.AddScoped<IPartyRepository, PartyRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();
        services.AddScoped<IProvinceRepository, ProvinceRepository>();
        services.AddScoped<ISlateRepository, SlateRepository>();
        services.AddScoped<IWingRepository, WingRepository>();

        // Mensajes
        services.AddScoped<IMessage<Candidate>, CandidateMessage>();
        services.AddScoped<IMessage<Circuit>, CircuitMessage>();
        services.AddScoped<IMessage<Client>, ClientMessage>();
        services.AddScoped<IMessage<DatalexionRole>, DatalexionRoleMessage>();
        services.AddScoped<IMessage<DatalexionUser>, DatalexionUserMessage>();
        services.AddScoped<IMessage<Delegado>, DelegadoMessage>();
        services.AddScoped<IMessage<Municipality>, MunicipalityMessage>();
        services.AddScoped<IMessage<Participant>, ParticipantMessage>();
        services.AddScoped<IMessage<Party>, PartyMessage>();
        services.AddScoped<IMessage<Province>, ProvinceMessage>();
        services.AddScoped<IMessage<Slate>, SlateMessage>();
        services.AddScoped<IMessage<Wing>, WingMessage>();

        // Otros servicios
        services.AddScoped<ILogService, LogService>();

        // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/27148834#notes
        services.AddTransient<GenerateLinks>();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();


        // Filtros
        //Ejemplo: services.AddScoped<MovieExistsAttribute>();

        // Servicios extra
        // services.AddSignalR();

        // Manejo de archivos en el servidor 
        services.AddSingleton<IFileStorage, FileStorageLocal>();
        services.AddHttpContextAccessor();

        // services.AddScoped<DataSeeder>();
        // services.AddScoped<SeedVotes>();

        #endregion

        #region Email Configuration

        var emailConfig = configuration.GetSection("NotificationEmail").Get<EmailConfiguration>();
        services.AddSingleton(emailConfig);
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddDetection();

        #endregion

        #region AddDbContext and AddIdentity

        services.AddDbContext<ContextDB>(options =>
        {
            // Obtener el ConnectionString desde una variable de entorno
            var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING_DATALEXION");
            if (string.IsNullOrEmpty(connectionString))
            {
                // Si no se encuentra el ConnectionString en las variables de entorno, intenta obtenerlo de appsettings
                connectionString = configuration.GetConnectionString("ConnectionString_Datalexion"); // Localhost
                // Si aún es nulo o vacío, lanza una excepción
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("No se encontró el ConnectionString. Asegúrese de configurar la variable de entorno 'ConnectionString_Datalexion' o definirla en appsettings.");
                }
            }
            options.UseSqlServer(connectionString)
            .EnableSensitiveDataLogging();
        });

        services.AddIdentity<DatalexionUser, DatalexionRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequiredLength = 6;
        })
            .AddEntityFrameworkStores<ContextDB>()
            .AddDefaultTokenProviders()
            .AddUserStore<UserStore<DatalexionUser, DatalexionRole, ContextDB, string>>()
            .AddRoleStore<RoleStore<DatalexionRole, ContextDB, string>>();

        #endregion

        // --------------

        #region AddAuthentication

        // Intentar obtener la clave JWT desde una variable de entorno
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

        // Si la variable de entorno no está establecida, intenta obtenerla desde appsettings.json
        if (string.IsNullOrEmpty(jwtKey))
        {
            jwtKey = configuration["JWT:key"];
        }

        // Asegúrate de que la clave no sea nula o vacía
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("No se encontró la clave JWT. Asegúrese de configurar la variable de entorno 'JWT_KEY' o definirla en appsettings.");
        }

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,   // false para desarrollo y pruebas
            ValidateAudience = false, // false para desarrollo y pruebas
                                      //ValidateIssuer = true,
                                      //ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        });

        #endregion

        #region AddAuthorization

        // Autorización basada en Claims
        // Agregar los roles del sistema
        // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/27047710#notes
        services.AddAuthorization(options =>
        {
            options.AddPolicy("IsAdmin", policy => policy.RequireRole(UserTypeOptions.Admin.ToString()));
            options.AddPolicy("IsAnalyst", policy => policy.RequireRole(UserTypeOptions.Analyst.ToString()));
        });

        #endregion

        #region AddCors

        // Configuración CORS: para permitir recibir peticiones http desde un origen específico
        // CORS Sólo sirve para aplicaciones web (Angular, React, etc)
        // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/27047732#notes
        // apirequest.io
        services.AddCors(options =>
        {
            var frontendURL = configuration.GetValue<string>("Frontend_URL");
            options.AddDefaultPolicy(builder =>
            {
                //builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                builder.WithExposedHeaders(new string[] { "totalSizeRecords" }); // Permite agregar headers customizados. Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/27148924#notes
            });
        });

        #endregion

        services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties
                | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
        });

        return services;
    }
}
