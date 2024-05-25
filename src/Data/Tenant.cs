namespace Data;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Device> Devices { get; set; }
    public ICollection<Telemetry> Telemetries { get; set; }
    public ICollection<Rule> Rules { get; set; }
}