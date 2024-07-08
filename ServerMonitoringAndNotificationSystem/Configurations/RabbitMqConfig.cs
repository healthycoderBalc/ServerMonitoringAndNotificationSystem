using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAndNotificationSystem.Configurations
{
    public class RabbitMqConfig
    {
        public string HostName { get; set; } = string.Empty;
        public string? UserName {  get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
    }
}
