namespace DatalexionBackend.Infrastructure.Services;

public interface ILogService
{
    Task LogAction(string entity, string action, string username, string data, int? clientId);
}
