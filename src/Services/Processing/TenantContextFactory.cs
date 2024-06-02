using Data;

namespace Services.Processing;

public interface ITenantContextFactory
{
    TenantContext CreateNew(Tenant tenant);
}

public class TenantContextFactory : ITenantContextFactory
{
    public TenantContext CreateNew(Tenant tenant)
    {
        return new TenantContext(tenant);
    }
}

public class TenantContext(Tenant tenant)
{
    public Tenant Tenant => tenant;
    public Telemetry? CurrentTelemetry { get; set; }
}