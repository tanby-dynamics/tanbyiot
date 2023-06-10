using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

const string baseApiUrl = "https://localhost:7128";

Console.WriteLine("Edge IoT Mock Device");
Console.Write("What is the API key for this device? ");
var apiKey = Console.ReadLine();
string token = string.Empty;

Console.WriteLine("Connecting...");
using (var client = new HttpClient())
{
    var payload = JsonSerializer.Serialize(new { ApiKey = apiKey });
    var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
    var url = $"{baseApiUrl}/api/devices/connect";

    var response = await client.PostAsync(url, postContent);

    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("Success");
        Console.WriteLine("Reading device token...");
        
        var contentJson = await response.Content.ReadAsStringAsync();
        var content = JsonSerializer.Deserialize<ConnectResponse>(contentJson);
        
        if (content is null) 
        {
            Console.WriteLine($"Response is invalid: {contentJson}");
            return;
        }

        token = content.Token;

        Console.WriteLine($"Token: {token}");
    }
    else
    {
        Console.WriteLine($"Error: {response.StatusCode}");
        return;
    }
}

// Ok we've got a working API key, now loop for input
Console.WriteLine("Looping for inputs and polling for actions. Press control-c to exit");
while (true) {
    await SendPayload();
    Console.WriteLine("Waiting...");
    Thread.Sleep(TimeSpan.FromSeconds(2));
    await PollForActions();
}

async Task SendPayload()
{
    using var client = new HttpClient();

    Console.WriteLine();
    
    Console.Write("Enter payload JSON (empty to skip): ");
    var payloadJson = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(payloadJson)) return;

    var payload = JsonSerializer.Serialize(new { Token = token, Payload = payloadJson});
    var content = new StringContent(payload, Encoding.UTF8, "application/json");
    var url = $"{baseApiUrl}/api/telemetry";

    Console.Write("Sending...");
    var response = await client.PostAsync(url, content);
    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("Success");
    }
    else
    {
        Console.WriteLine($"Error: {response.StatusCode}");
    }
}

async Task PollForActions()
{
    Console.Write("Polling for actions...");
    using var client = new HttpClient();
    var payload = JsonSerializer.Serialize(new { Token = token });
    var content= new StringContent(payload, Encoding.UTF8, "application/json");
    var url = $"{baseApiUrl}/api/action-poll";

    var response = await client.PostAsync(url, content);
    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine("Success");
        // write response
    }
    else
    {
        Console.WriteLine($"Error: {response.StatusCode}");
    }
}

public class ConnectResponse  { [JsonPropertyName("token")] public string Token { get;set;} }