using DatalexionBackend.Core.Helpers;
using DatalexionBackend.Infrastructure.Services;
using DatalexionBackend.UI.Middlewares;
using DatalexionBackend.UI.StartupExtensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging - Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration) // Leer la configuración
    .ReadFrom.Services(services); // Hacer disponibles los servicios de la app
});

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
    // var logger = app.Services.GetRequiredService<ILogger<DataSeeder>>();
    await ExcelDataSeeder.LoadDataFromExcel(app.Services, wwwrootPath);
}
bool SeedVotesOnStartup = builder.Configuration.GetValue<bool>("SeedVotesOnStartup");
if (SeedVotesOnStartup)
{
    // var logger = app.Services.GetRequiredService<ILogger<SeedVotes>>();
    ExcelDataSeeder.SeedVotes(app.Services);
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
app.MapHub<NotifyHub>("/notifyHub");

app.Run();