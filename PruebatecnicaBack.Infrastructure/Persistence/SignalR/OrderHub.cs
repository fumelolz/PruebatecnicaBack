
using Microsoft.AspNetCore.SignalR;

namespace PruebatecnicaBack.Infrastructure.Persistence.SignalR
{
    public class OrderHub : Hub
    {
        public async Task SendNextOrderNumber(int orderNumber)
        {
            await Clients.All.SendAsync("ReceiveNextOrderNumber", orderNumber);
        }
    }
}
