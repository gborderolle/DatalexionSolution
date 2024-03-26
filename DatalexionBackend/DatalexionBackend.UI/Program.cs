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
    await ExcelDataSeeder.LoadDataFromExcel(app.Services, wwwrootPath);
}
bool SeedVotesOnStartup = builder.Configuration.GetValue<bool>("SeedVotesOnStartup");
if (SeedVotesOnStartup)
{
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