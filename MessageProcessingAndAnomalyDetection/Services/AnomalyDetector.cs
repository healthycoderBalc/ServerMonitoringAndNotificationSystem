using MessageProcessingAndAnomalyDetection.Interfaces;
using MessageProcessingAndAnomalyDetection.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingAndAnomalyDetection.Services
{
    public class AnomalyDetector : IAnomalyDetector
    {
        private readonly double _memoryUsageAnomalyThreshold;
        private readonly double _cpuUsageAnomalyThreshold;
        private readonly double _memoryUsageThreshold;
        private readonly double _cpuUsageThreshold;
        private readonly HubConnection _hubConnection;
        private readonly ILogger<AnomalyDetector> _logger;
        public AnomalyDetector(
            double memoryUsageAnomalyThreshold, 
            double cpuUsageAnomalyThreshold, 
            double memoryUsageThreshold, 
            double cpuUsageThreshold, 
            HubConnection hubConnection,
            ILogger<AnomalyDetector> logger)
        {
            _memoryUsageAnomalyThreshold = memoryUsageAnomalyThreshold;
            _cpuUsageAnomalyThreshold = cpuUsageAnomalyThreshold;
            _memoryUsageThreshold = memoryUsageThreshold;
            _cpuUsageThreshold = cpuUsageThreshold;
            _hubConnection = hubConnection;
            _logger = logger;
        }


        public void DetectAnomalies(ServerStatistics currentStatistics, ServerStatistics previousStatistics)
        {
            _logger.LogInformation("Detecting anomalies for ServerIdentifier: {ServerIdentifier}", currentStatistics.ServerIdentifier);

            bool memoryUsageAnomalyAlert = currentStatistics.MemoryUsage > previousStatistics.MemoryUsage * (1 + _memoryUsageAnomalyThreshold);
            bool cpuUsageAnomalyAlert = currentStatistics.CpuUsage > previousStatistics.CpuUsage * (1 + _cpuUsageAnomalyThreshold);

            if (memoryUsageAnomalyAlert)
            {
                SendAlert(new Alert
                {
                    ServerIdentifier = currentStatistics.ServerIdentifier,
                    AlertType = "MemoryUsageAnomaly",
                    Message = $"Memory usage anomaly detected. Current: {currentStatistics.MemoryUsage}, Previous: {previousStatistics.MemoryUsage}",
                    Timestamp = currentStatistics.Timestamp
                });
            }

            if (cpuUsageAnomalyAlert)
            {
                SendAlert(new Alert
                {
                    ServerIdentifier = currentStatistics.ServerIdentifier,
                    AlertType = "CpuUsageAnomaly",
                    Message = $"CPU usage anomaly detected. Current: {currentStatistics.CpuUsage}, Previous: {previousStatistics.CpuUsage}",
                    Timestamp = currentStatistics.Timestamp
                });
            }

            bool memoryHighUsageAlert = currentStatistics.MemoryUsage / (currentStatistics.MemoryUsage + currentStatistics.AvailableMemory) > _memoryUsageThreshold;
            bool cpuHighUsageAlert = currentStatistics.CpuUsage > _cpuUsageThreshold;

            if (memoryHighUsageAlert)
            {
                SendAlert(new Alert
                {
                    ServerIdentifier = currentStatistics.ServerIdentifier,
                    AlertType = "HighMemoryUsage",
                    Message = $"High memory usage detected. Usage: {currentStatistics.MemoryUsage / (currentStatistics.MemoryUsage + currentStatistics.AvailableMemory) * 100}%",
                    Timestamp = currentStatistics.Timestamp
                });
            }

            if (cpuHighUsageAlert)
            {
                SendAlert(new Alert
                {
                    ServerIdentifier = currentStatistics.ServerIdentifier,
                    AlertType = "HighCpuUsage",
                    Message = $"High CPU usage detected. Usage: {currentStatistics.CpuUsage}%",
                    Timestamp = currentStatistics.Timestamp
                });
            }
        }

        private async void SendAlert(Alert alert)
        {
            try
            {
                _logger.LogInformation("Sending alert: {AlertType} for ServerIdentifier: {ServerIdentifier}", alert.AlertType, alert.ServerIdentifier);
                await _hubConnection.SendAsync("SendAlert", alert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending alert");
            }
        }
    }
}
