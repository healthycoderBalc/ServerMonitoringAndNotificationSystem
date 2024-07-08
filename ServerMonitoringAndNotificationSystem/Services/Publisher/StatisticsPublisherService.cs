using Microsoft.Extensions.Options;
using ServerMonitoringAndNotificationSystem.Configurations;
using ServerMonitoringAndNotificationSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMonitoringAndNotificationSystem.Services.Publisher
{
    public class StatisticsPublisherService
    {
        private readonly IStatisticsCollector _statisticsCollector;
        private readonly IMessageQueue _messageQueue;
        private readonly ServerStatisticsConfig _config;

        public StatisticsPublisherService(IStatisticsCollector statisticsCollector, IMessageQueue messageQueue, IOptions<ServerStatisticsConfig> config)
        {
            _statisticsCollector = statisticsCollector;
            _messageQueue = messageQueue;
            _config = config.Value;
        }

        public async Task StartAsync()
        {
            while (true)
            {
                var statistics = _statisticsCollector.CollectStatistics();
                _messageQueue.Publish($"ServerStatistics.{_config.ServerIdentifier}", statistics);
                await Task.Delay(_config.SamplingIntervalSeconds * 1000);
            }
        }
    }
}
