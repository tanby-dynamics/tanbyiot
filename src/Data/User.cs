using System.ComponentModel.DataAnnotations;

namespace Data;

public class User
{
    public Guid Id { get; init; }
    [MaxLength(256)] public string ExternalId { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public ICollection<Tenant>? Tenants { get; init; } = default!;
    public Guid? CurrentTenantId { get; init; }
    public Tenant? CurrentTenant { get; set; }
    [MaxLength(320)] public string Email { get; set; } = string.Empty;
}