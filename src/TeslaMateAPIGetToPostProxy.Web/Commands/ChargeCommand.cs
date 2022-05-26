using System.Net.Http.Headers;

namespace TeslaMateAPIGetToPostProxy.Web.Commands
{
    public class ChargeCommand
    {
        public static async Task<IResult> ExecuteCommand(string token, IHttpClientFactory factory, ILogger<ChargeCommand> logger)
        {
            var client = factory.CreateClient(Constants.ClientName);
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
        }
    }
}
