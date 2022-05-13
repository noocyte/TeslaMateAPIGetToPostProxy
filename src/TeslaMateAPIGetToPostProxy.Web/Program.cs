using System.Net.Http.Headers;

var clientName = "TeslaMateAPI";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient(clientName, httpClient =>
{
    var apiHost = Environment.GetEnvironmentVariable("API_HOST");
    var apiHostPort = Environment.GetEnvironmentVariable("API_HOST_PORT");

    httpClient.BaseAddress = new Uri($"http://{apiHost}:{apiHostPort}");
});

var app = builder.Build();

app.MapGet("/test", async (IHttpClientFactory factory) =>
{
    var client = factory.CreateClient(clientName);

    var response = await client.GetAsync("/api/v1/cars/1/command");
    var body = await response.Content.ReadAsStringAsync();
    return body;
});
app.MapGet("/trunk", async (string token, IHttpClientFactory factory) =>
{
    var client = factory.CreateClient(clientName);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{token}");
    var response = await client.PostAsJsonAsync("/api/v1/cars/1/command/actuate_trunk", new { which = "rear" });
    var body = await response.Content.ReadAsStringAsync();
    return body;
});
app.MapGet("/charge", async (string token, IHttpClientFactory factory) =>
{
    var client = factory.CreateClient(clientName);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{token}");
    var response = await client.PostAsJsonAsync("/api/v1/cars/1/command/charge_port_door_open", new { });
    var body = await response.Content.ReadAsStringAsync();
    return body;
});

app.Run("http://*:3000");