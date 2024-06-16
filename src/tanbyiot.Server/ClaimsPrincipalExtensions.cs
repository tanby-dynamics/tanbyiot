using System.Security.Claims;

namespace tanbyiot.Server;

public static class ClaimsPrincipalExtensions
{
    public static string[]? GetPermissions(this ClaimsPrincipal claimsPrincipal, string issuer)
    {
        return !claimsPrincipal.HasClaim(x => x.Type == "permissions" && x.Issuer == issuer) 
            ? null 
            : claimsPrincipal.FindFirst(x => x.Type == "permissions" && x.Issuer == issuer)?.Value.Split(' ');
    }
}