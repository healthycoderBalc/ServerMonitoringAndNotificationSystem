using MessageProcessingAndAnomalyDetection.Interfaces;
using MessageProcessingAndAnomalyDetection.Models;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingAndAnomalyDetection.Services.MongoDb
{
    public class MongoDbStatisticsCollectionFactory : IMongoDbStatisticsCollectionFactory
    {
        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly string _collectionName;


        public MongoDbStatisticsCollectionFactory(string connectionString, string databaseName, string collectionName)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            _collectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
        }

        public IMongoCollection<ServerStatistics> CreateStatisticsCollection()
        {
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(_databaseName);
            return database.GetCollection<ServerStatistics>(_collectionName);
        }

    }
}
