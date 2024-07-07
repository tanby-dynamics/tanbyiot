namespace Services.TenantStates;

// ReSharper disable once ClassNeverInstantiated.Global
public class UpdateTenantStateArgs
{
    public string Key { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}