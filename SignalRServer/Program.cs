using SignalRServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
var configuration = builder.Configuration;
var signalRUrl = configuration.GetSection("SignalRConfig:SignalRUrl").Value;
var uri = new Uri(signalRUrl);
var alertHubPath = uri.AbsolutePath;


var app = builder.Build();
app.MapHub<AlertHub>(alertHubPath);

//app.MapGet("/", () => "Hello World!");

app.Run();
