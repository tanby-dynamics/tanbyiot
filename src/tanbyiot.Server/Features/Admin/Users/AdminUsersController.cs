using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Users;

namespace tanbyiot.Server.Features.Admin.Users;

[ApiController]
[Route("/api/admin/users")]
[Authorize("admin:all")]
public class AdminUsersController(IGetUsers getUsers) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<UserDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
    {
        var result = await getUsers.ExecuteAsync(cancellationToken);

        return Ok(result);
    }
}