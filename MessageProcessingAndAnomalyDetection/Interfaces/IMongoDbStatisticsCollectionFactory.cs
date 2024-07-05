﻿using MessageProcessingAndAnomalyDetection.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageProcessingAndAnomalyDetection.Interfaces
{
    public interface IMongoDbStatisticsCollectionFactory
    {
        IMongoCollection<ServerStatistics> CreateStatisticsCollection();
    }
}