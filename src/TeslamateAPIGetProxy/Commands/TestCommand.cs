namespace TeslamateAPIGetProxy.Commands;

public class TestCommand
{
    public static async Task<IResult> ExecuteCommand(IHttpClientFactory factory, ILogger<TestCommand> logger, IConfiguration configuration)
    {
        var client = factory.CreateClient(Constants.ClientName);
        var carNumber = configuration.GetValue<int>("CAR_NUMBER");

        var response = await client.GetAsync($"/api/v1/cars/{carNumber}/command");
        var body = await response.Content.ReadAsStringAsync();
        logger.LogInformation(body);
        return Results.Ok(body);
    }
}
