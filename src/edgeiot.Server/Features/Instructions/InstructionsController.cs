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
        // TODO security, make sure tenant is active, make sure device belongs to tenant
        
        var response = await pollForInstructions.ExecuteAsync(request.DeviceId, cancellationToken);

        return Ok(response);
    }
}

public class PollForInstructionsRequestDto
{
    public Guid TenantId { get; set; }
    public Guid DeviceId { get; set; }
}