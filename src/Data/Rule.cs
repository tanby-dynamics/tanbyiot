using System.ComponentModel.DataAnnotations;

namespace Data;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class Rule
{
    public Guid Id { get; init; }
    [MaxLength(128)] public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? DeletedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public ICollection<RuleCondition> Conditions { get; init; } = default!;
    public ICollection<RuleAction> Actions { get; init; } = default!;
}