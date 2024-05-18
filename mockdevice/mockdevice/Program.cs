using System.Text;
using System.Text.Json;

const string baseApiUrl = "https://localhost:7061";
const string telemetryUrl = $"{baseApiUrl}/api/telemetry";
const string actionPollUrl = $"{baseApiUrl}/api/action-poll";

Console.WriteLine("edgeiot mock device");
Console.WriteLine("-------------------");
Console.WriteLine();

Console.Write("Enter tenant ID: ");
var tenantId = Guid.Parse(Console.ReadLine()!);

Console.Write("Enter device ID: ");
var deviceId = Guid.Parse(Console.ReadLine()!);

Console.WriteLine();
Console.WriteLine("Entering loop, press control-c to exit");

while (true)
{
    await SendPayload();
    Thread.Sleep(TimeSpan.FromSeconds(2));
    await PollForActions();
}

async Task SendPayload()
{
    Console.WriteLine();
    Console.Write("Enter payload JSON (empty to skip): ");
    var payloadJson = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(payloadJson)) return;

    var payload = JsonSerializer.Serialize(new
    {
        TenantId = tenantId,
        DeviceId = deviceId,
        Payload = payloadJson
    });
    var content = new StringContent(payload, Encoding.UTF8, "application/json");

    Console.Write("Sending...");
    using var client = new HttpClient();
    var response = await client.PostAsync(telemetryUrl, content);

    Console.WriteLine(response.IsSuccessStatusCode ? "Success" : $"Error: {response.StatusCode}");
}

async Task PollForActions()
{
    Console.Write("Polling for actions...");
    var payload = JsonSerializer.Serialize(new
    {
        TenantId = tenantId,
        DeviceId = deviceId
    });
    var content = new StringContent(payload, Encoding.UTF8, "application/json");

    using var client = new HttpClient();
    var response = await client.PostAsync(actionPollUrl, content);
    var responseContent = await response.Content.ReadAsStringAsync();

    Console.WriteLine(response.IsSuccessStatusCode ? "Success, response content:" : $"Error: {response.StatusCode}");
    Console.WriteLine(responseContent);
}