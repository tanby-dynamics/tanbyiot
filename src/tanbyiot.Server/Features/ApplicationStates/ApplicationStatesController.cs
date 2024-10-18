using Microsoft.AspNetCore.Mvc;
using Services.ApplicationStates;

namespace tanbyiot.Server.Features.ApplicationStates;

[ApiController]
[Route("/api/states")]
public class ApplicationStatesController(
    IGetApplicationStates getApplicationStates,
    IUpdateApplicationState updateApplicationState,
    IDeleteApplicationState deleteApplicationState,
    IAddApplicationState addApplicationState) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<ApplicationStateDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllStates(CancellationToken cancellationToken)
    {
        var states = await getApplicationStates.ExecuteAsync(cancellationToken);

        return Ok(states);
    }

    [HttpPut("{applicationStateId:guid}")]
    [ProducesResponseType<ApplicationStateDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateApplicationState(Guid applicationStateId, UpdateApplicationStateArgs args,
        CancellationToken cancellationToken)
    {
        var state = await updateApplicationState.ExecuteAsync(applicationStateId, args, cancellationToken);

        return Ok(state);
    }

    [HttpDelete("{applicationStateId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteApplicationState(Guid applicationStateId,
        CancellationToken cancellationToken)
    {
        await deleteApplicationState.ExecuteAsync(applicationStateId, cancellationToken);

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType<ApplicationStateDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddApplicationState(AddApplicationStateArgs args,
        CancellationToken cancellationToken)
    {
        var state = await addApplicationState.ExecuteAsync(args, cancellationToken);

        return Ok(state);
    }
}