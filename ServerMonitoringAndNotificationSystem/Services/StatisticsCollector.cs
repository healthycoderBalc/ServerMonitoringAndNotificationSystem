using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            var availableMemory = GetAvailableMemory(memoryUsage);
            var cpuUsage = GetCurrentCpuUsage();

            return new ServerStatistics
            {
                MemoryUsage = memoryUsage,
                AvailableMemory = availableMemory,
                CpuUsage = cpuUsage,
                Timestamp = DateTime.Now
            };
        }

        private long GetTotalSystemMemory()
        {
            try
            {
                var procMemInfo = File.ReadAllText("/proc/meminfo");

                var match = Regex.Match(procMemInfo, @"MemTotal:\s+(\d+)\skB");
                if (match.Success && long.TryParse(match.Groups[1].Value, out long totalKb))
                {
                    return totalKb / 1024;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading /proc/meminfo: {ex.Message}");
            }

            return 4096;
        }

        private double GetCurrentCpuUsage()
        {
            try
            {
                using var process = Process.GetCurrentProcess();
                var totalProcessorTime = process.TotalProcessorTime;
                var systemUptime = TimeSpan.FromSeconds(Environment.TickCount / 1000.0);
                var cpuUsage = (double)(totalProcessorTime.TotalMilliseconds / systemUptime.TotalMilliseconds / Environment.ProcessorCount) * 100.0f;

                return cpuUsage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting CPU usage: {ex.Message}");
                return 0.0f;
            }
        }

        private double GetAvailableMemory(double memoryUsage)
        {
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    using var performanceCounter = new PerformanceCounter("Memory", "Available MBytes");
                    return performanceCounter.NextValue();
                }
                else
                {
                    var totalMemory = GetTotalSystemMemory();
                    var availableMemory = totalMemory - memoryUsage;
                    return availableMemory;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting available memory: {ex.Message}");
                return 0.0f;
            }
        }

    }
}
