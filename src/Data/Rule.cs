namespace Data;

public class Rule : ISoftDelete
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public DateTimeOffset CreatedAt { get; init; }
    public Guid TenantId { get; init; }
    public DateTimeOffset? DeletedAt { get; set; }
    public virtual Tenant Tenant { get; init; }
    public ICollection<RuleCondition> Conditions { get; init; }
    public ICollection<RuleAction> Actions { get; init; }
}