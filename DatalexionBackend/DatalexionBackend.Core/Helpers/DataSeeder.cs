using Microsoft.Extensions.Logging;

namespace DatalexionBackend.Core.Helpers;
public class DataSeeder
{
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(ILogger<DataSeeder> logger)
    {
        _logger = logger;
    }

}
