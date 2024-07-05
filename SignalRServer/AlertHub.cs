using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRServer
{
    internal class AlertHub : Hub
    {
        public async Task SendAlert(string alertType, string message)
        {
            await Clients.All.SendAsync("ReceiveAlert", alertType, message);
        }
    }
}
