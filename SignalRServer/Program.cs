using SignalRServer.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSignalR();
var signalRUrl = builder.Configuration["SIGNALR_URL"];
var uri = new Uri(signalRUrl);
var alertHubPath = uri.AbsolutePath;


var app = builder.Build();
app.MapHub<AlertHub>(alertHubPath);

await app.RunAsync();
