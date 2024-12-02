namespace BlogSimplesEF.Web.Hub
{
    using Microsoft.AspNetCore.SignalR;

    public class PostsHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
