namespace Data;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Device> Devices { get; set; }
    public ICollection<Telemetry> Telemetries { get; set; }
}