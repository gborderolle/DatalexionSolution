using Microsoft.Extensions.Logging;

namespace DatalexionBackend.Core.Helpers;
public class SeedVotes
{
    private readonly ILogger<SeedVotes> _logger;

    public SeedVotes(ILogger<SeedVotes> logger)
    {
        _logger = logger;
    }

}
