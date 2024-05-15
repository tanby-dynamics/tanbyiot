namespace Data;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Device> Devices { get; set; }
}