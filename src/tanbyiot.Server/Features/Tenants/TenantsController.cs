using Microsoft.AspNetCore.Mvc;
using Serilog;
using Services.Tenants;
using Services.Users;

namespace tanbyiot.Server.Features.Tenants;

[ApiController]
[Route("/api/tenants")]
public class TenantsController(IAddTenantForUser addTenantForUser, IGetUserByExternalId getUserByExternalId) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<TenantDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> AddTenant(AddTenantForUserArgs args, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<TenantsController>();
        var externalId = HttpContext.User.Identity?.Name;
        
        var (userFound, user) = await getUserByExternalId.ExecuteAsync(externalId, cancellationToken);

        if (!userFound)
        {
            log.Error("No user when trying to create tenant, {ExternalId}", externalId);
            return Unauthorized();
        }

        var (created, tenant) = await addTenantForUser.ExecuteAsync(args, user!, cancellationToken);

        if (!created)
        {
            return Unauthorized();
        }

        return Ok(tenant);
    }
}