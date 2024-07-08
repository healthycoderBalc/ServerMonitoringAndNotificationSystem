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



var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        services.Configure<RabbitMqConfig>(options =>
        {
            options.HostName = configuration["RABBITMQ_HOSTNAME"];
        });

        services.Configure<ServerStatisticsConfig>(options =>
        {
            options.ServerIdentifier = configuration["SERVER_IDENTIFIER"];
            options.SamplingIntervalSeconds = int.Parse(configuration["SAMPLING_INTERVAL_SECONDS"]);
        });
        services.AddSingleton<IStatisticsCollector, StatisticsCollector>();
        services.AddSingleton<IRabbitMqConnectionFactory>(provider =>
            new RabbitMqConnectionFactory(
                configuration["RABBITMQ_HOSTNAME"]
            ));

        services.AddSingleton<IMessageQueue, RabbitMqMessageQueue>();
        services.AddSingleton<StatisticsPublisherService>();
    })
    .Build();

var statisticsService = host.Services.GetRequiredService<StatisticsPublisherService>();
await statisticsService.StartAsync();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();