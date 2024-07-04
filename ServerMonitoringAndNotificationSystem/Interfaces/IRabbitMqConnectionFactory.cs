using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAndNotificationSystem.Interfaces
{
    public interface IRabbitMqConnectionFactory
    {
        IConnection CreateConnection();
        IModel CreateChannel(IConnection connection);
    }
}
