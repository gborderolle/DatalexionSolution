using Microsoft.AspNetCore.SignalR;

namespace DatalexionBackend.Core.Helpers
{
    public class NotifyHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
