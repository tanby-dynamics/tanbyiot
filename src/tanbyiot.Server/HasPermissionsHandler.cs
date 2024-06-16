using Microsoft.AspNetCore.Authorization;

namespace tanbyiot.Server;

public class HasPermissionsHandler : AuthorizationHandler<HasPermissionsRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionsRequirement requirement)
    {
        var permissions = context.User.GetPermissions(requirement.Issuer);

        if (permissions is null)
        {
            return Task.CompletedTask;
        }

        if (permissions.Any(x => x == requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}