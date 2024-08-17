using Microsoft.AspNetCore.SignalR;

namespace ChatOperaMini;

public class ChatHub : Hub
{
    public async Task SendMessage(string sender, string message, string messageId, string sendDate)
    {
        await Clients.All.SendAsync("ReceiveMessage", sender, message, messageId, sendDate);
    }
}