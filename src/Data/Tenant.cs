using System.ComponentModel.DataAnnotations;

namespace Data;

public class Tenant
{
    public Guid Id { get; init; }
    [MaxLength(128)] public string Name { get; init; } = string.Empty;
    // ReSharper disable once EntityFramework.ModelValidation.CircularDependency
    public ICollection<Device> Devices { get; init; } = default!;
    // ReSharper disable once EntityFramework.ModelValidation.CircularDependency
    public ICollection<Telemetry> Telemetries { get; init; } = default!;
    // ReSharper disable once EntityFramework.ModelValidation.CircularDependency
    public ICollection<Rule> Rules { get; init; } = default!;
    public ICollection<User> Users { get; init; } = default!;
    public SubscriptionLevel SubscriptionLevel { get; init; }
    public ICollection<TenantState> States { get; init; } = default!;
}