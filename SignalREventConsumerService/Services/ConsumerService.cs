using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SignalREventConsumerService.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalREventConsumerService.Services
{
    public class ConsumerService
    {
        private readonly HubConnection _hubConnection;

        public ConsumerService(HubConnection hubConnection)
        {
            _hubConnection = hubConnection ?? throw new ArgumentNullException(nameof(hubConnection));
        }

        public void ConsumeService()
        {
            _hubConnection.On<Alert>("ReceiveMessage", alert =>
            {
                Console.WriteLine($"[{alert.Timestamp}] {alert.ServerIdentifier} - {alert.AlertType}: {alert.Message}");
            });
        }
        public async Task StartAsync()
        {
            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("SignalR connection established.");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to SignalR hub: {ex.Message}");
                Console.WriteLine(ex);
            }
        }
    }
}
