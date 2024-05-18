using Data;

namespace Services.Devices;

public class DeviceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public DateTimeOffset? LastConnected { get; set; }

    public static DeviceDto FromEntity(Device device)
    {
        return new DeviceDto
        {
            Id = device.Id,
            Name = device.Name,
            GroupName = device.GroupName,
            LastConnected = device.LastConnected
        };
    }
}