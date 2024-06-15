using Microsoft.AspNetCore.Authorization;

namespace edgeiot.Server;

public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
    {
        if (!context.User.HasClaim(x => x.Type == "scope" && x.Issuer == requirement.Issuer))
        {
            return Task.CompletedTask;
        }

        var scopes = context.User.FindFirst(x => x.Type == "scope" && x.Issuer == requirement.Issuer)?.Value.Split(' ');

        if (scopes is null)
        {
            return Task.CompletedTask;
        }

        if (scopes.Any(x => x == requirement.Scope))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

public class HasPermissionsHandler : AuthorizationHandler<HasPermissionsRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionsRequirement requirement)
    {
        if (!context.User.HasClaim(x => x.Type == "permissions" && x.Issuer == requirement.Issuer))
        {
            return Task.CompletedTask;
        }

        var permissions = context.User.FindFirst(x => x.Type == "permissions" && x.Issuer == requirement.Issuer)?.Value.Split(' ');

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