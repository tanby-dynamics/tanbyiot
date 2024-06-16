using Microsoft.AspNetCore.Authorization;

namespace tanbyiot.Server;

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