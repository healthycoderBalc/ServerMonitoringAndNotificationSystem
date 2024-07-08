using MessageProcessingAndAnomalyDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingAndAnomalyDetection.Interfaces
{
    public interface IDatabase
    {
        Task InsertStatisticsAsync(ServerStatistics statistics);
        IEnumerable<ServerStatistics> GetStatistics(string serverIdentifier);
    }
}
