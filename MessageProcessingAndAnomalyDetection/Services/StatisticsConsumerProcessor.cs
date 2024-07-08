using MessageProcessingAndAnomalyDetection.Interfaces;
using MessageProcessingAndAnomalyDetection.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingAndAnomalyDetection.Services
{
    public class StatisticsConsumerProcessor : IHostedService
    {
        private readonly ISubscribeMessageQueue _messageQueue;
        private readonly IDatabase _database;
        private readonly IAnomalyDetector _anomalyDetector;
        private readonly ILogger<StatisticsConsumerProcessor> _logger;

        public StatisticsConsumerProcessor(
            ISubscribeMessageQueue messageQueue, 
            IDatabase database,
            IAnomalyDetector anomalyDetector,
            ILogger<StatisticsConsumerProcessor> logger)
        {
            _messageQueue = messageQueue ?? throw new ArgumentNullException(nameof(messageQueue));
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _anomalyDetector = anomalyDetector ?? throw new ArgumentNullException(nameof(anomalyDetector));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RabbitMQ Consumer Service started.");
            _messageQueue.Subscribe("ServerStatistics.*", HandleMessage);
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RabbitMQ Consumer Service stopped.");
            return Task.CompletedTask;
        }

        private void HandleMessage(string serverIdentifier, string message)
        {
            _logger.LogInformation("Message received: {Message}", message);
            var statistics = JsonConvert.DeserializeObject<ServerStatistics>(message);
            statistics.ServerIdentifier = serverIdentifier;
            _logger.LogInformation("Message received: {Message}", message);
            var previousStatistics = _database.GetStatistics(statistics.ServerIdentifier).OrderByDescending(stat => stat.Timestamp).FirstOrDefault();
            _database.InsertStatisticsAsync(statistics);

            if (previousStatistics != null)
            {
                _anomalyDetector.DetectAnomalies(statistics, previousStatistics);
            }
        }
    }
}
