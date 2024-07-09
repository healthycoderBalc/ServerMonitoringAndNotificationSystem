using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalREventConsumerService.Services;
using System;
using System.Net.Security;

var builder = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

var signalRUrl = builder["SIGNALR_URL"];
Console.WriteLine("Connecting to: " + signalRUrl + "...");

var hubConnection = new HubConnectionBuilder()
    .WithUrl(signalRUrl, conf =>
    {
        conf.HttpMessageHandlerFactory = (x) => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
    })
    .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Debug))
    .Build();

var consumerService = new ConsumerService(hubConnection);
consumerService.ConsumeService();

await consumerService.StartAsync();

Console.WriteLine("Connection started. Waiting for messages");
await Task.Delay(-1);