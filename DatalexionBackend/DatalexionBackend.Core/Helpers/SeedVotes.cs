using Microsoft.Extensions.Logging;

namespace DatalexionBackend.Core.Helpers;
public class SeedVotes
{
    private readonly ILogger<SeedVotes> _logger;

    public SeedVotes(ILogger<SeedVotes> logger)
    {
        _logger = logger;
    }

    public async Task LoadDataFromExcel(IServiceProvider serviceProvider, string wwwrootPath)
    {
        _logger.LogInformation("Iniciando carga de datos desde Excel...");
    }
}
