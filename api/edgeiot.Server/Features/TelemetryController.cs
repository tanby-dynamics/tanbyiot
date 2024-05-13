using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace edgeiot.Server.Features;

[ApiController]
[Route("/api/telemetry")]
public class TelemetryController : ControllerBase
{
    public TelemetryController()
    {
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> PostTelemetry(
        [FromBody] PostTelemetryRequest request,
        CancellationToken cancellationToken)
    {
        var token = request.Token;
        var payload = request.Payload.ToString();

        // TODO store telemetry
        // TODO put telemetry processing on the queue

        return Task.FromResult<IActionResult>(Ok());
    }
}

public class PostTelemetryRequest
{
    public string Token { get; set; } = string.Empty;
    public JsonElement Payload { get; set; }
}