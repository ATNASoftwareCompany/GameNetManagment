using Microsoft.AspNetCore.SignalR;

namespace GameNetManagment.Server.Hubs
{
    public class CafeHub : Hub
    {
        public async Task Announce(string clientName, string message)
        {
            await Clients.Others.SendAsync("ReceiveAnnouncement", clientName, message);
        }

        public override async Task OnConnectedAsync()
        {
            // یک پیام به کلاینتی که وصل شده بفرست
            await Clients.Caller.SendAsync("ReceiveMessage", "System", "Welcome to the Cafe Hub!");
            // به همه کلاینت های دیگر اعلام کن که یک نفر وصل شد
            await Clients.Others.SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} has joined.");
            await base.OnConnectedAsync();
        }


    }
}
