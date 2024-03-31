using Microsoft.Extensions.Logging;

namespace DatalexionBackend.Core.Helpers;
public class DataSeeder
{
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(ILogger<DataSeeder> logger)
    {
        _logger = logger;
    }

    public async Task LoadDataFromExcel(IServiceProvider serviceProvider, string wwwrootPath)
    {
        _logger.LogInformation("Iniciando carga de datos desde Excel...");
    }
}
