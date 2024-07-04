using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerMonitoringAndNotificationSystem.Models;

namespace ServerMonitoringAndNotificationSystem.Interfaces
{
    public interface IStatisticsCollector
    {
        ServerStatistics CollectStatistics();
    }
}
