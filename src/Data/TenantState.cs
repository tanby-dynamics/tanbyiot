using System.ComponentModel.DataAnnotations;

namespace Data;

public class TenantState
{
    public Guid Id { get; init; }
    public Guid TenantId { get; init; }
    public virtual Tenant Tenant { get; init; } = default!;
    [MaxLength(128)] public string Key { get; set; } = string.Empty;
    [MaxLength(4000)] public string Value { get; set; } = string.Empty;
    public DateTimeOffset SetAt { get; set; }
}