using Microsoft.AspNetCore.SignalR;

namespace server_try.Hubs
{
public class MyHub : Hub
    {
        public async Task Changed(string input)
        {   
            await Clients.All.SendAsync("ChangeRecieved",input);
        }
    }
}
