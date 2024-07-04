using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using ServerMonitoringAndNotificationSystem.Configurations;
using ServerMonitoringAndNotificationSystem.Interfaces;
using ServerMonitoringAndNotificationSystem.Services;
using ServerMonitoringAndNotificationSystem.Services.Publisher;
using ServerMonitoringAndNotificationSystem.Services.RabbitMq;
using System.IO;


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IConfiguration>(configuration);
        services.Configure<ServerStatisticsConfig>(configuration.GetSection("ServerStatisticsConfig"));
        services.AddSingleton<IStatisticsCollector, StatisticsCollector>();
        services.AddSingleton<IRabbitMqConnectionFactory>(provider => new RabbitMqConnectionFactory("localhost"));
        services.AddSingleton<IMessageQueue, RabbitMqMessageQueue>();
        services.AddSingleton<StatisticsPublisherService>();
    })
    .Build();

var statisticsService = host.Services.GetRequiredService<StatisticsPublisherService>();
await statisticsService.StartAsync();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();