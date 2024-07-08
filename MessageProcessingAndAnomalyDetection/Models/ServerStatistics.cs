using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingAndAnomalyDetection.Models
{
    public class ServerStatistics
    {
        [BsonIgnoreIfDefault]
        public MongoDB.Bson.ObjectId Id { get; set; }
        public String ServerIdentifier { get; set; } = string.Empty;
        public double MemoryUsage { get; set; } // in MB
        public double AvailableMemory { get; set; } // in MB
        public double CpuUsage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
