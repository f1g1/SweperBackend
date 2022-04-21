using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }
        public async Task SendMessage(string user, string message)
        {
             await Clients?.All?.SendAsync("BroadcastMessage", user, message);
        }
    }
}