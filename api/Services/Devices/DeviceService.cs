namespace Services;

public interface IDeviceService
{
    Task<string> GetNewApiKey(string tenant, CancellationToken cancellationToken);
}

public class DeviceService : IDeviceService
{
    public Task<string> GetNewApiKey(string tenant, CancellationToken cancellationToken)
    {
        return Task.FromResult("ABCDEF");
    }
}