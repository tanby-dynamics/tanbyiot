namespace Data;

public class Rule
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public DateTimeOffset CreatedAt { get; init; }
    public Guid TenantId { get; init; }
    public virtual Tenant Tenant { get; init; }
}