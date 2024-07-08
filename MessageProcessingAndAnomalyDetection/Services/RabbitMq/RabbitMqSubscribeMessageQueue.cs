using MessageProcessingAndAnomalyDetection.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingAndAnomalyDetection.Services.RabbitMq
{
    public class RabbitMqSubscribeMessageQueue : ISubscribeMessageQueue
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqSubscribeMessageQueue> _logger;

        public RabbitMqSubscribeMessageQueue(IRabbitMqConnectionSubscribeFactory connection, ILogger<RabbitMqSubscribeMessageQueue> logger)
        {
            _connection = connection.CreateConnection() ?? throw new ArgumentNullException(nameof(connection));
            _channel = connection.CreateChannel(_connection) ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger;
        }

        public void Subscribe(string topic, Action<string, string> handleMessage)
        {
            _logger.LogInformation("Subscribing to topic: {Topic}", topic);
            _channel.ExchangeDeclare(exchange: "server_statistics", type: ExchangeType.Topic, durable: true);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: "server_statistics", routingKey: topic);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                var serverIdentifier = GetServerIdentifierFromRoutingKey(routingKey);
                _logger.LogInformation("Message received: {Message}", message);
                handleMessage(serverIdentifier, message);
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        private string? GetServerIdentifierFromRoutingKey(string routingKey)
        {
            var parts = routingKey.Split('.');
            if (parts.Length >= 2)
            {
                return parts[1];
            }
            return null;
        }
    }
}
