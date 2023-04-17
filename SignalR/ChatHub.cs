using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using SweperBackend.Controllers;

namespace SignalRChat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public string CHAT_INPUT { get; private set; } = "chatInput";

        public override async Task OnConnectedAsync()
        {
            dynamic zz = JsonConvert.DeserializeObject(Context.User.Claims.Last(x => x.Value.Contains("google.com")).Value);
            var userId = ((dynamic)zz["identities"]["google.com"][0]).Value;

            await Groups.AddToGroupAsync(Context.ConnectionId, userId + CHAT_INPUT);
            await base.OnConnectedAsync();
        }
        public async Task SendChatMessage(string userId, MessageUi message)
        {
            await Clients?.Group(userId + CHAT_INPUT)?.SendAsync("ReceiveMessage", message);
        }

        public async Task TestMethod(string test)
        {
            var z = test;
        }
    }
}