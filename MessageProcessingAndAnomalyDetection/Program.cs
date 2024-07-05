using MessageProcessingAndAnomalyDetection.Configurations;
using MessageProcessingAndAnomalyDetection.Interfaces;
using MessageProcessingAndAnomalyDetection.Services;
using MessageProcessingAndAnomalyDetection.Services.MongoDb;
using MessageProcessingAndAnomalyDetection.Services.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection("RabbitMqConfig"));
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection("MongoDbConfig"));
builder.Services.Configure<AnomalyDetectionConfig>(builder.Configuration.GetSection("AnomalyDetectionConfig"));
builder.Services.Configure<SignalRConfig>(builder.Configuration.GetSection("SignalRConfig"));

builder.Services.AddLogging(configure => configure.AddConsole());
builder.Services.AddSingleton<IRabbitMqConnectionSubscribeFactory, RabbitMqConnectionSubscribeFactory>(sp =>
{
    var config = sp.GetRequiredService<IOptions<RabbitMqConfig>>().Value;
    sp.GetRequiredService<ILogger<Program>>().LogInformation("RabbitMQ Config: HostName={HostName}, UserName={UserName}", config.HostName, config.UserName);
    return new RabbitMqConnectionSubscribeFactory(config.HostName, config.UserName, config.Password);
});
builder.Services.AddSingleton<ISubscribeMessageQueue, RabbitMqSubscribeMessageQueue>();

builder.Services.AddSingleton<IMongoDbStatisticsCollectionFactory, MongoDbStatisticsCollectionFactory>(sp =>
{
    var config = sp.GetRequiredService<IOptions<MongoDbConfig>>().Value;
    sp.GetRequiredService<ILogger<Program>>().LogInformation("MongoDB Config: ConnectionString={ConnectionString}, DatabaseName={DatabaseName}, CollectionName={CollectionName}", config.ConnectionString, config.DatabaseName, config.CollectionName);
    return new MongoDbStatisticsCollectionFactory(config.ConnectionString, config.DatabaseName, config.CollectionName);
});
builder.Services.AddSingleton<IDatabase, MongoDbService>();

builder.Services.AddSingleton<HubConnection>(sp =>
{
    var signalRConfig = sp.GetRequiredService<IOptions<SignalRConfig>>().Value;
    var hubConnection = new HubConnectionBuilder()
        .WithUrl(signalRConfig.SignalRUrl)
        .Build();
    hubConnection.StartAsync().Wait();
    return hubConnection;
});

builder.Services.AddSingleton<IAnomalyDetector, AnomalyDetector>(sp =>
{
    var config = sp.GetRequiredService<IOptions<AnomalyDetectionConfig>>().Value;
    var hubConnection = sp.GetRequiredService<HubConnection>();
    var logger = sp.GetRequiredService<ILogger<AnomalyDetector>>();

    return new AnomalyDetector(config.MemoryUsageAnomalyThresholdPercentage, config.CpuUsageAnomalyThresholdPercentage, config.MemoryUsageThresholdPercentage, config.CpuUsageThresholdPercentage, hubConnection, logger);
});


builder.Services.AddHostedService<StatisticsConsumerProcessor>();

var app = builder.Build();

await app.RunAsync();