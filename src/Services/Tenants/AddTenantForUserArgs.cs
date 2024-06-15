using Data;

namespace Services.Tenants;

public class AddTenantForUserArgs
{
    public string Name { get; init; } = string.Empty;
    public SubscriptionLevel SubscriptionLevel { get; init; }
}