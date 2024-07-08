using Amazon.Runtime.Internal.Util;
using MessageProcessingAndAnomalyDetection.Interfaces;
using MessageProcessingAndAnomalyDetection.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MessageProcessingAndAnomalyDetection.Services.MongoDb
{
    public class MongoDbService : IDatabase
    {
        private readonly IMongoCollection<ServerStatistics> _statisticsCollection;
        private readonly ILogger<MongoDbService> _logger;

        public MongoDbService(IMongoDbStatisticsCollectionFactory mongoDbFactory, ILogger<MongoDbService> logger)
        {
            _statisticsCollection = mongoDbFactory.CreateStatisticsCollection() ?? throw new ArgumentException(nameof(mongoDbFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task InsertStatisticsAsync(ServerStatistics statistics)
        {
            try
            {
                await _statisticsCollection.InsertOneAsync(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving statistics to MongoDB");
                throw;
            }
        }

        public IEnumerable<ServerStatistics> GetStatistics(string serverIdentifier)
        {
            return _statisticsCollection.Find(stat => stat.ServerIdentifier == serverIdentifier).ToList();
        }

    }
}
