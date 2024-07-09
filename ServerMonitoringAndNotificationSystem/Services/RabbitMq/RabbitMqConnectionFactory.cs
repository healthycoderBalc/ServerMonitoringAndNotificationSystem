using RabbitMQ.Client;
using ServerMonitoringAndNotificationSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAndNotificationSystem.Services.RabbitMq
{
    public class RabbitMqConnectionFactory : IRabbitMqConnectionFactory
    {
        private readonly string _hostname;

        public RabbitMqConnectionFactory(string hostname)
        {
            _hostname = hostname ?? throw new ArgumentNullException(nameof(hostname));
        }

        public IConnection CreateConnection()
        {
            var factory = new ConnectionFactory();
            factory.UserName = "agent";
            factory.Password = "agent";
            factory.VirtualHost = "/";
            factory.HostName = "rabbitmq";
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            //{ HostName = _hostname };
            return factory.CreateConnection();
        }

        public IModel CreateChannel(IConnection connection)
        {

            return connection.CreateModel();
        }

    }
}
