using DatalexionBackend.Infrastructure.Services;
using DatalexionBackend.UI.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

var startup = new ConfigureServicesExtensions(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

if (app.Environment.IsDevelopment())
{
    await ExcelDataSeeder.SeedDataAsync(app.Services);
}

app.Run();