using Newtonsoft.Json;
using RabbitMQ.Client;
using ServerMonitoringAndNotificationSystem.Interfaces;
using ServerMonitoringAndNotificationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServerMonitoringAndNotificationSystem.Services.RabbitMq
{
    public class RabbitMqMessageQueue : IMessageQueue
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqMessageQueue(IRabbitMqConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection() ?? throw new ArgumentNullException(nameof(connectionFactory));
            _channel = connectionFactory.CreateChannel(_connection) ?? throw new ArgumentNullException(nameof(connectionFactory));
        }


        public void Publish(string topic, ServerStatistics statistics)
        {
            _channel.QueueDeclare(
                queue: topic,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(statistics));

            _channel.BasicPublish(
                exchange: "",
                routingKey: topic,
                basicProperties: null,
                body: body
                );
        }
    }
}
