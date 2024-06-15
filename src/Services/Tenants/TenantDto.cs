using Data;

namespace Services.Tenants;

public class TenantDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public SubscriptionLevel SubscriptionLevel { get; init; }

    public static TenantDto? FromEntity(Tenant? tenant)
    {
        if (tenant is null)
        {
            return null;
        }
        
        return new TenantDto
        {
            Id = tenant.Id,
            Name = tenant.Name,
            SubscriptionLevel = tenant.SubscriptionLevel
        };
    }
}