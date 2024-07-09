using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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
            options.UserName = configuration["RABBITMQ_USERNAME"];
            options.Password = configuration["RABBITMQ_PASSWORD"];
        });
        services.Configure<ServerStatisticsConfig>(options =>
        {
            options.ServerIdentifier = configuration["SERVER_IDENTIFIER"];
            options.SamplingIntervalSeconds = int.Parse(configuration["SAMPLING_INTERVAL_SECONDS"]);
        });
        services.AddSingleton<IStatisticsCollector, StatisticsCollector>();
        services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>(provider => {
            var config = provider.GetRequiredService<IOptions<RabbitMqConfig>>().Value;
            return new RabbitMqConnectionFactory(config.HostName, config.UserName, config.Password);
            });
        services.AddSingleton<IMessageQueue, RabbitMqMessageQueue>();
        services.AddSingleton<StatisticsPublisherService>();
    })
    .Build();

var statisticsService = host.Services.GetRequiredService<StatisticsPublisherService>();
await statisticsService.StartAsync();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();