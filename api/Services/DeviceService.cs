namespace Services;

public interface IDeviceService
{
    Task<string> GetNewApiKey(string tenant, CancellationToken cancellationToken = default(CancellationToken));
}

public class DeviceService : IDeviceService
{
    public async Task<string> GetNewApiKey(string tenant, CancellationToken cancellationToken = default(CancellationToken))
    {
        return "ABCDEF";
    }
}