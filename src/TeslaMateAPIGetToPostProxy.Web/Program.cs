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
    return Results.Ok(body);
});

app.MapGet("/charge", async (string token, IHttpClientFactory factory, ILogger<Program> logger) =>
{
    var client = factory.CreateClient(clientName);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{token}");

    var isAwake = false;
    var counter = 0;
    do
    {
        counter++;
        var wakeResponse = await client.PostAsJsonAsync("/api/v1/cars/1/wake_up", new { });
        if (!wakeResponse.IsSuccessStatusCode)
        {
            var wakeResponseBody = wakeResponse.Content.ReadAsStringAsync();
            logger.LogWarning("Call to wakeup failed: {status}, message: {message}", wakeResponse.StatusCode, wakeResponseBody);
            continue;
        }

        var currentStatus = await wakeResponse.Content.ReadFromJsonAsync<TeslaAPIResponse>();
        if (currentStatus == null)
        {
            logger.LogError("currentStatus is null");
            continue;
        }

        isAwake = currentStatus.response.state.Equals("online", StringComparison.OrdinalIgnoreCase);

        if (!isAwake)
        {
            logger.LogInformation("Waiting 500ms - counter: {counter}", counter);
            await Task.Delay(500);
        }

    } while (!isAwake && counter <= 10);

    if (!isAwake || counter >= 10)
    {
        logger.LogError("Failed to wakeup car, tried {counter} times", counter);
        return Results.Problem(detail: "Wakeup failed", statusCode: 500);
    }

    var chargeResponse = await client.PostAsJsonAsync("/api/v1/cars/1/command/charge_port_door_open", new { });
    var body = await chargeResponse.Content.ReadAsStringAsync();
    logger.LogInformation("Response from Charge Command: {response}", body);

    return Results.Ok(body);
});

app.Run("http://*:3000");

