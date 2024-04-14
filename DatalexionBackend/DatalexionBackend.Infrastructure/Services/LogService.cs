using DatalexionBackend.Core.Domain.Entities;
using DatalexionBackend.Core.Helpers;
using DatalexionBackend.Infrastructure.DbContext;

namespace DatalexionBackend.Infrastructure.Services;

public class LogService : ILogService
{
    private readonly ContextDB _context;
    public LogService(ContextDB context)
    {
        _context = context;
    }

    public async Task LogAction(string entity, string action, string username, string data, int? clientId)
    {
        var log = new Log
        {
            Entity = entity,
            Action = action,
            Username = username,
            Data = data ?? "", // Asegúrate de que no se inserte nulo
            Creation = GlobalServices.GetDatetimeUruguay(),
            ClientId = clientId
        };
        _context.Log.Add(log);
        await _context.SaveChangesAsync();
    }
}