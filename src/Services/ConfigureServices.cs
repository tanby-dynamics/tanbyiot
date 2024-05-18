using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Devices;
using Shared.Services.Options;
using Shared.Services.Queueing;

namespace Services;

public static class ConfigureServices
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<AzureStorageOptions>()
            .Configure(o => configuration.GetSection(AzureStorageOptions.Key));
        services.AddScoped<IQueueManager, QueueManager>();
        services.AddScoped<IGetAllDevicesForTenant, GetAllDevicesForTenant>();
        services.AddScoped<IAddDevice, AddDevice>();
    }
}