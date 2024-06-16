using Microsoft.AspNetCore.Authorization;

namespace tanbyiot.Server;

public class HasPermissionsRequirement(string permission, string issuer) : IAuthorizationRequirement
{
    public string Issuer { get; } = issuer ?? throw new ArgumentNullException(nameof(issuer));
    public string Permission { get; } = permission ?? throw new ArgumentNullException(nameof(permission));
}