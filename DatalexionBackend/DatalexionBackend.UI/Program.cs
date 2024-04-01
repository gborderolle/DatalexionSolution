using DatalexionBackend.Core.Helpers;
using DatalexionBackend.Infrastructure.Services;
using DatalexionBackend.UI.Middlewares;
using DatalexionBackend.UI.StartupExtensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Logging - Serilog

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING_DATALEXION") ?? "Server=localhost;Database=datalexiondb;User Id=datalexion_admin;Password=datalexion_admin1234;Encrypt=False;TrustServerCertificate=True"; // Usa una conexión de respaldo si no se encuentra la variable de entorno

    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Hour, fileSizeLimitBytes: 1048576, rollOnFileSizeLimit: true)
        .WriteTo.MSSqlServer(
            connectionString: connectionString,
            sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
            {
                TableName = "Serilog",
                AutoCreateSqlTable = true
            });
});

#endregion

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

// Crear un ámbito manualmente
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Registrar el inicio de la aplicación
    var loggerProgram = services.GetRequiredService<ILogger<Program>>();
    var loggerService = services.GetRequiredService<ILogService>();
    loggerProgram.LogInformation("La aplicación DatalexionBackend ha iniciado.");
    await loggerService.LogAction("Program", "Inicio", "System", "La aplicación DatalexionBackend ha iniciado.", null);
}

var webHostEnvironment = app.Services.GetService<IWebHostEnvironment>();
var wwwrootPath = webHostEnvironment?.WebRootPath ?? "";

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

bool SeedCircuitsAndMunicipalitiesFromExcelOnStartup = builder.Configuration.GetValue<bool>("SeedCircuitsAndMunicipalitiesFromExcelOnStartup");
if (SeedCircuitsAndMunicipalitiesFromExcelOnStartup)
{
    var logger = app.Services.GetRequiredService<ILogger<DataSeeder>>();
    await ExcelDataSeeder.LoadDataFromExcel(app.Services, wwwrootPath, logger);
}
bool SeedVotesOnStartup = builder.Configuration.GetValue<bool>("SeedVotesOnStartup");
if (SeedVotesOnStartup)
{
    var logger = app.Services.GetRequiredService<ILogger<SeedVotes>>();
    ExcelDataSeeder.SeedVotes(app.Services, logger);
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Datalexion 3.0");
});

app.UseHsts();
app.UseHttpsRedirection(); //n1
app.UseSerilogRequestLogging();
app.UseHttpLogging();
app.UseStaticFiles();
app.UseCors(); //n2
app.UseRouting(); //n3
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();