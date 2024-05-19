using System.Text;
using System.Text.Json;

const string baseUrl = "https://localhost:7061";
const string telemetryUrl = $"{baseUrl}/api/telemetry";
const string instructionsPollUrl = $"{baseUrl}/api/instructions/poll";
const string connectUrl = $"{baseUrl}/api/devices/connect";

Console.WriteLine("edgeiot mock device");
Console.WriteLine("-------------------");
Console.WriteLine();

Console.Write("Enter tenant ID: ");
var tenantId = Guid.Parse(Console.ReadLine()!);

Console.Write("Enter device ID: ");
var deviceId = Guid.Parse(Console.ReadLine()!);

await Connect();

Console.WriteLine();
Console.WriteLine("Entering loop, press control-c to exit");

while (true)
{
    await SendPayload();
    Thread.Sleep(TimeSpan.FromSeconds(2));
    await PollForInstructions();
}

async Task Connect()
{
    Console.Write("Connecting...");
    var body = JsonSerializer.Serialize(new
    {
        TenantId = tenantId,
        DeviceId = deviceId
    });
    var content = new StringContent(body, Encoding.UTF8, "application/json");
    using var client = new HttpClient();
    var response = await client.PostAsync(connectUrl, content);
    Console.WriteLine(response.IsSuccessStatusCode ? "Success" : $"Error: {response.StatusCode}");
}

async Task SendPayload()
{
    Console.WriteLine();
    Console.Write("Telemetry type (empty to skip): ");
    var type = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(type)) return;
    Console.Write("Value: ");
    var value = Console.ReadLine();
    Console.Write("Payload: ");
    var payload = Console.ReadLine();
    
    var body = JsonSerializer.Serialize(new
    {
        TenantId = tenantId,
        DeviceId = deviceId,
        Type = type,
        Value = value,
        Payload = payload
    });
    var content = new StringContent(body, Encoding.UTF8, "application/json");

    Console.Write("Sending...");
    using var client = new HttpClient();
    var response = await client.PostAsync(telemetryUrl, content);

    Console.WriteLine(response.IsSuccessStatusCode ? "Success" : $"Error: {response.StatusCode}");
    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }
}

async Task PollForInstructions()
{
    Console.Write("Polling for instructions...");
    var payload = JsonSerializer.Serialize(new
    {
        TenantId = tenantId,
        DeviceId = deviceId
    });
    var content = new StringContent(payload, Encoding.UTF8, "application/json");

    using var client = new HttpClient();
    var response = await client.PostAsync(instructionsPollUrl, content);
    var responseContent = await response.Content.ReadAsStringAsync();

    Console.WriteLine(response.IsSuccessStatusCode ? "Success, response content:" : $"Error: {response.StatusCode}");
    Console.WriteLine(responseContent);
}