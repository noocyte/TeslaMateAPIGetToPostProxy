using TeslaMateAPIGetToPostProxy.Web;
using TeslaMateAPIGetToPostProxy.Web.Commands;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient(Constants.ClientName, httpClient =>
{
    var apiHost = Environment.GetEnvironmentVariable("API_HOST");
    var apiHostPort = Environment.GetEnvironmentVariable("API_HOST_PORT");

    httpClient.BaseAddress = new Uri($"http://{apiHost}:{apiHostPort}");
});

var app = builder.Build();

app.MapGet("/test", TestCommand.ExecuteCommand);
app.MapGet("/charge", ChargeCommand.ExecuteCommand);

app.Run("http://*:3000");
