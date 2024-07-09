﻿using RabbitMQ.Client;
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
        private readonly string _username;
        private readonly string _password;

        public RabbitMqConnectionFactory(string hostname, string username, string password)
        {
            _hostname = hostname ?? throw new ArgumentNullException(nameof(hostname));
            _username = username ?? throw new ArgumentNullException(nameof(username));
            _password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public IConnection CreateConnection()
        {
            var factory = new ConnectionFactory();
            factory.UserName = _username;
            factory.Password = _password;
            factory.VirtualHost = "/";
            factory.HostName = _hostname;
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            return factory.CreateConnection();
        }

        public IModel CreateChannel(IConnection connection)
        {

            return connection.CreateModel();
        }

    }
}
