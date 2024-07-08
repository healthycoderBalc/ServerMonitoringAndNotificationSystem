using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerMonitoringAndNotificationSystem.Interfaces;
using ServerMonitoringAndNotificationSystem.Models;

namespace ServerMonitoringAndNotificationSystem.Services
{
    public class StatisticsCollector : IStatisticsCollector
    {
        public ServerStatistics CollectStatistics()
        {
            var memoryUsage = Process.GetCurrentProcess().WorkingSet64 / (1024 * 1024);
            var availableMemory = new PerformanceCounter("Memory", "Available MBytes").NextValue();
            var cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total").NextValue();

            return new ServerStatistics
            {
                MemoryUsage = memoryUsage,
                AvailableMemory = availableMemory,
                CpuUsage = cpuUsage,
                Timestamp = DateTime.Now
            };
        }
    }
}
