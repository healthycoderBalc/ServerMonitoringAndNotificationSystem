using SignalRServer.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSignalR();
var configuration = builder.Configuration;
var signalRUrl = configuration["SIGNALR_URL"];
var uri = new Uri(signalRUrl);
var alertHubPath = uri.AbsolutePath;


var app = builder.Build();
app.MapHub<AlertHub>(alertHubPath);
Console.WriteLine("relative path: " + alertHubPath);

await app.RunAsync();
