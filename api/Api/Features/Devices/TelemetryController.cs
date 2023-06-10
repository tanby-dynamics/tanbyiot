using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Devices;

[ApiController]
[Route("/api/telemetry")]
public class TelemetryController : ControllerBase
{
	public TelemetryController()
	{
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<IActionResult> PostTelemetry(
		[FromBody] PostTelemetryRequest request,
		CancellationToken cancellationToken)
	{
		var token = request.Token;
		var payload = request.Payload.ToString();

		// TODO store telemetry
		// TODO put telemetry processing on the queue

		return Ok();
	}
}

public class PostTelemetryRequest
{
	public string Token { get; set; }
	public JsonElement Payload { get; set; }
}