namespace TeslaMateAPIGetToPostProxy.Web.Commands
{
    public class TestCommand
    {
        public static async Task<IResult> ExecuteCommand(IHttpClientFactory factory, ILogger<TestCommand> logger)
        {
            var client = factory.CreateClient(Constants.ClientName);

            var response = await client.GetAsync("/api/v1/cars/1/command");
            var body = await response.Content.ReadAsStringAsync();
            logger.LogInformation(body);
            return Results.Ok(body);
        }
    }
}
