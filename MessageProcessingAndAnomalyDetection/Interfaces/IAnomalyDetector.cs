using MessageProcessingAndAnomalyDetection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingAndAnomalyDetection.Interfaces
{
    public interface IAnomalyDetector
    {
        void DetectAnomalies(ServerStatistics currentStatistics, ServerStatistics previousStatistics);
    }
}
