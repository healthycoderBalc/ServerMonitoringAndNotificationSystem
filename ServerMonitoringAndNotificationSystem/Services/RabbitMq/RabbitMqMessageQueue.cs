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
        private readonly string _exchangeName = "server_statistics";

        public RabbitMqMessageQueue(IRabbitMqConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection() ?? throw new ArgumentNullException(nameof(connectionFactory));
            _channel = connectionFactory.CreateChannel(_connection) ?? throw new ArgumentNullException(nameof(connectionFactory));

            _channel.ExchangeDeclare(
                exchange: _exchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null
            );
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

            _channel.QueueBind(
                queue: topic,
                exchange: _exchangeName,
                routingKey: topic
            );

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(statistics));

            _channel.BasicPublish(
                exchange: _exchangeName,
                routingKey: topic,
                basicProperties: null,
                body: body
                );
        }
    }
}
