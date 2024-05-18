using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Services.Devices;
using Services.Telemetries;
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
        services.AddScoped<IAddTelemetry, AddTelemetry>();
        services.AddScoped<IValidateDevice, ValidateDevice>();
        services.AddScoped<ISystemClock, SystemClock>();
        services.AddScoped<IConnectDevice, ConnectDevice>();
    }
}