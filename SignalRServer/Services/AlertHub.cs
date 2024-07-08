using Microsoft.AspNetCore.SignalR;
using SignalRServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServer.Services
{
    public class AlertHub : Hub
    {
        public async Task SendMessage(Alert alert)
        {
            await Clients.All.SendAsync("ReceiveMessage", alert);
        }
    }
}
