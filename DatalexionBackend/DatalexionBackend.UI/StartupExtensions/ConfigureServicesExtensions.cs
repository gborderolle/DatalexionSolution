using DatalexionBackend.Core.ApiBehavior;
using DatalexionBackend.Core.Domain.IdentityEntities;
using DatalexionBackend.Core.Domain.RepositoryContracts;
using DatalexionBackend.Core.Filters;
using DatalexionBackend.Core.Helpers;
using DatalexionBackend.Core.Middlewares;
using DatalexionBackend.Core.Services;
using DatalexionBackend.EmailService;
using DatalexionBackend.Infrastructure.DbContext;
using DatalexionBackend.Infrastructure.Repositories;
using DatalexionBackend.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;


[assembly: ApiConventionType(typeof(DefaultApiConventions))] // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/27148912#notes

namespace DatalexionBackend.UI.StartupExtensions;
public class ConfigureServicesExtensions
{
    public IConfiguration Configuration { get; }

    public ConfigureServicesExtensions(IConfiguration configuration)
    {
        // Limpia los mapeos de los tipos de los Claims (del login de usuario)
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/27047628#notes
        Configuration = configuration;
    }

    /// <summary>
    /// Configuración de los Services
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(options =>
        //services.AddControllersWithViews(options =>
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

        //services.AddControllersWithViews(); // testing!! --> borrar

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Datalexion 3.0",
                Version = "v1",
                Description = "Dashboard analítico para optimizar y procesar información en tiempo real. Datalexion es el aliado tecnológico ideal para la gestión electoral eficiente.",
                Contact = new OpenApiContact
                {
                    Email = "gborderolle@gmail.com",
                    Name = "Gonzalo Borderolle",
                    Url = new Uri("https://linkedin.com/in/gborderolle")
                }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{ }
                    }
            });

            var fileXML = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var routeXML = Path.Combine(AppContext.BaseDirectory, fileXML);
            c.IncludeXmlComments(routeXML);
        });

        // Configuración de la base de datos
        services.AddDbContext<ContextDB>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("ConnectionString_Datalexion"))
            .EnableSensitiveDataLogging();
        });

        services.AddAutoMapper(typeof(ConfigureServicesExtensions));

        // Registro de servicios 
        // --------------

        // AddTransient: cambia dentro del contexto
        // AddScoped: se mantiene dentro del contexto (mejor para los servicios)
        // AddSingleton: no cambia nunca

        // Repositorios
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<ICircuitRepository, CircuitRepository>();
        services.AddScoped<IDatalexionUserRepository, DatalexionUserRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IDelegadoRepository, DelegadoRepository>();
        services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();
        services.AddScoped<IPartyRepository, PartyRepository>();
        services.AddScoped<IProvinceRepository, ProvinceRepository>();
        services.AddScoped<ISlateRepository, SlateRepository>();
        services.AddScoped<IWingRepository, WingRepository>();
        services.AddScoped<IPhotoRepository, PhotoRepository>();

        services.AddScoped<ILogService, LogService>();

        // Filtros
        //Ejemplo: services.AddScoped<MovieExistsAttribute>();

        // Servicios extra
        services.AddSignalR();

        // Manejo de archivos en el servidor 
        services.AddSingleton<IFileStorage, FileStorageLocal>();
        services.AddHttpContextAccessor();

        // Email Configuration
        var emailConfig = Configuration.GetSection("NotificationEmail").Get<EmailConfiguration>();
        services.AddSingleton(emailConfig);
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddDetection();

        // --------------

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
                Encoding.UTF8.GetBytes(Configuration["JWT:key"])),
            ClockSkew = TimeSpan.Zero
        });

        // Identity Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/27047608#notes
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
            .AddDefaultTokenProviders();

        // Autorización basada en Claims
        // Agregar los roles del sistema
        // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/27047710#notes
        services.AddAuthorization(options =>
        {
            options.AddPolicy("IsAdmin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("IsAnalyst", policy => policy.RequireRole("Analyst"));
        });

        // Configuración CORS: para permitir recibir peticiones http desde un origen específico
        // CORS Sólo sirve para aplicaciones web (Angular, React, etc)
        // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/27047732#notes
        // apirequest.io
        services.AddCors(options =>
        {
            var frontendURL = Configuration.GetValue<string>("Frontend_URL");
            options.AddDefaultPolicy(builder =>
            {
                //builder.WithOrigins(frontendURL).AllowAnyMethod().AllowAnyHeader();
                builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                builder.WithExposedHeaders(new string[] { "totalSizeRecords" }); // Permite agregar headers customizados. Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/27148924#notes
            });
        });

        // Clase: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/27148834#notes
        services.AddTransient<GenerateLinks>();
        services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { new CultureInfo("es-UY") };
    options.DefaultRequestCulture = new RequestCulture("es-UY");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

    }

    /// <summary>
    /// Configuración del Middleware
    /// Middlewares son los métodos "Use..()"
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Middleware customizado: https://www.udemy.com/course/construyendo-web-apis-restful-con-aspnet-core/learn/lecture/26839760#notes
        app.UseLogResponseHTTP();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Datalexion 3.0");
        });

        app.UseRouting();  // orden n3
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<NotifyHub>("/notifyHub");
        });
    }

}