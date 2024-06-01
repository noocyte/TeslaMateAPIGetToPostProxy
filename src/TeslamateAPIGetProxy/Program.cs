using TeslamateAPIGetProxy;
using TeslamateAPIGetProxy.Commands;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient(Constants.ClientName, (provider, httpClient) =>
{
    var config = provider.GetRequiredService<IConfiguration>();

    var apiHost = config.GetValue<string>("API_HOST");
    var apiHostPort = config.GetValue<string>("API_HOST_PORT");

    httpClient.BaseAddress = new Uri($"http://{apiHost}:{apiHostPort}");
});

var app = builder.Build();

app.MapGet("/test", TestCommand.ExecuteCommand);
app.MapGet("/charge", ChargeCommand.ExecuteCommand);

app.Run();
