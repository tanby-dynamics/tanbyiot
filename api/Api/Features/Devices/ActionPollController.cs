using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Devices;

[ApiController]
[Route("/api/action-poll")]
public class ActionPollController : ControllerBase
{
	public ActionPollController()
	{
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionPollResponseDto))]
	public Task<IActionResult> Post(
		[FromBody] ActionPollRequestDto request,
		CancellationToken cancellationToken)
	{
		var response = new ActionPollResponseDto
		{
			Actions = new[] {
				new ActionPollResponseDto.ActionDto()
			}
		};

		return Task.FromResult<IActionResult>(Ok(response));
	}
}

public class ActionPollRequestDto
{
	public string Token { get; set; } = string.Empty;
}

public class ActionPollResponseDto
{
	public IEnumerable<ActionDto> Actions { get; set; } = Enumerable.Empty<ActionDto>();
	
	public class ActionDto
	{ }
}