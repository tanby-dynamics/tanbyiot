using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Services.Users;

namespace tanbyiot.Server.Features.Users;

[ApiController]
[Route("/api/users")]
[Authorize]
public class UsersController(
    IAddUser addUser,
    IGetUserByExternalId getUserByExternalId,
    ISetCurrentTenantForUser setCurrentTenantForUser,
    IConfiguration configuration,
    ISetUserEmail setUserEmail) : ControllerBase
{
    /// <summary>
    ///     Gets the current user. If the current user doesn't exist, create it.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("current-user")]
    [ProducesResponseType<UserDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        // Email address is passed in a header
        var emailHeaders = HttpContext.Request.Headers["Email"];
        if (emailHeaders.Count == 0 || string.IsNullOrEmpty(emailHeaders.First()))
        {
            var log = Log.ForContext<UsersController>();
            log.Error("Trying to create user but no Email header present in request. External ID {ExternalId}",
                HttpContext.User.Identity?.Name!);
            return NotFound();
        }

        var email = emailHeaders.First()!;
        
        var (found, user) = await getUserByExternalId.ExecuteAsync(HttpContext.User.Identity?.Name, cancellationToken);
        if (found)
        {
            if (user!.Email == email) return Ok(user);
            
            // Email address has changed, update our record
            var updatedUser = await setUserEmail.ExecuteAsync(user.Id, email, cancellationToken);
            return Ok(updatedUser);
        }

        var newUser = await addUser.ExecuteAsync(
            HttpContext.User.Identity?.Name!,
            emailHeaders.First()!,
            cancellationToken);

        return Ok(newUser);
    }

    [HttpGet("current-user/permissions")]
    [ProducesResponseType<string[]>(StatusCodes.Status200OK)]
    public IActionResult GetCurrentUserPermissions()
    {
        var issuer = configuration["Auth0:Domain"] ?? throw new ArgumentException("Auth0:Domain not set");
        var permissions = HttpContext.User.GetPermissions(issuer) ?? Array.Empty<string>();

        return Ok(permissions);
    }

    /// <summary>
    ///     Set the current tenant for the current user.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("set-current-tenant")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SetCurrentTenant(SetCurrentTenantRequestDto request,
        CancellationToken cancellationToken)
    {
        var (found, user) = await getUserByExternalId.ExecuteAsync(HttpContext.User.Identity?.Name, cancellationToken);

        if (!found) return NotFound();

        if (user!.Tenants.All(x => x.Id != request.TenantId)) return NotFound();

        await setCurrentTenantForUser.ExecuteAsync(user, request.TenantId, cancellationToken);

        return Ok();
    }
}