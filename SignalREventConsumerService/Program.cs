using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalREventConsumerService.Services;
using System;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var signalRUrl = builder["SignalRConfig:SignalRUrl"];
Console.WriteLine("Connecting to: " + signalRUrl + "...");

var hubConnection = new HubConnectionBuilder()
    .WithUrl(signalRUrl)
    .Build();

var consumerService = new ConsumerService(hubConnection);
consumerService.ConsumeService();

await consumerService.StartAsync();

Console.WriteLine("Connection started. Waiting for messages");
await Task.Delay(-1);