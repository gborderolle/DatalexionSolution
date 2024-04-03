using Microsoft.AspNetCore.SignalR;

namespace DatalexionBackend.Core.Helpers
{
    public class NotifyHub : Hub
    {
        public async Task AskServer(string message)
        {
            string tempString;
            if (message == "hey")
            {
                tempString = "Hello, how can I help you?";
            }
            else if (message == "How are you?")
            {
                tempString = "I'm fine, thank you!";
            }
            else if (message == "What's your name?")
            {
                tempString = "I'm Jarvis, at your service!";
            }
            else
            {
                tempString = "I'm sorry, I don't understand that command.";
            }

            await Clients.Clients(this.Context.ConnectionId).SendAsync("askServerResponse", tempString);
        }

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
