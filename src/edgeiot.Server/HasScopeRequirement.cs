using Microsoft.AspNetCore.Authorization;

namespace edgeiot.Server;

public class HasScopeRequirement(string scope, string issuer) : IAuthorizationRequirement
{
    public string Issuer { get; } = issuer ?? throw new ArgumentNullException(nameof(issuer));
    public string Scope { get; } = scope ?? throw new ArgumentNullException(nameof(scope));
}

public class HasPermissionsRequirement(string permission, string issuer) : IAuthorizationRequirement
{
    public string Issuer { get; } = issuer ?? throw new ArgumentNullException(nameof(issuer));
    public string Permission { get; } = permission ?? throw new ArgumentNullException(nameof(permission));
}