using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Users;

namespace edgeiot.Server.Features.Users;

[ApiController]
[Route("/api/users")]
[Authorize]
public class UsersController(IAddUser addUser, IGetUserByExternalId getUserByExternalId,
    ISetCurrentTenantForUser setCurrentTenantForUser) : ControllerBase
{
    /// <summary>
    /// Gets the current user. If the current user doesn't exist, create it.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("current-user")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var (found, user) = await getUserByExternalId.ExecuteAsync(HttpContext.User.Identity?.Name, cancellationToken);

        if (found)
        {
            return Ok(user);
        }

        var newUser = await addUser.ExecuteAsync(HttpContext.User.Identity?.Name!, cancellationToken);

        return Ok(newUser);
    }

    /// <summary>
    /// Set the current tenant for the current user.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("set-current-tenant")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetCurrentTenant(SetCurrentTenantRequestDto request, CancellationToken cancellationToken)
    {
        var (found, user) = await getUserByExternalId.ExecuteAsync(HttpContext.User.Identity?.Name, cancellationToken);

        if (!found)
        {
            return Unauthorized();
        }

        if (user!.Tenants.All(x => x.Id != request.TenantId))
        {
            return NotFound();
        }

        await setCurrentTenantForUser.ExecuteAsync(user, request.TenantId, cancellationToken);

        return Ok();
    }
}