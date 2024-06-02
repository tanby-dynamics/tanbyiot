using Microsoft.AspNetCore.Mvc;
using Services.Instructions;

namespace edgeiot.Server.Features.Instructions;

[ApiController]
[Route("/api/instructions")]
public class InstructionsController(IPollForInstructions pollForInstructions) : ControllerBase
{
    [HttpPost("poll")]
    [ProducesResponseType<IEnumerable<PollForInstructionsResponseDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> PollForInstructions(PollForInstructionsRequestDto request,
        CancellationToken cancellationToken)
    {
        var response = await pollForInstructions.ExecuteAsync(request.DeviceId, cancellationToken);

        return Ok(response);
    }
}