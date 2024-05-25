using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Services.Devices;
using Services.Instructions;
using Services.Options;
using Services.Telemetries;
using Services.Queueing;
using Services.Rules;

namespace Services;

public static class ConfigureServices
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureStorageOptions>(options => configuration.GetSection("AzureStorage").Bind(options));
        services.AddScoped<IQueueManager, QueueManager>();
        services.AddScoped<IGetAllDevicesForTenant, GetAllDevicesForTenant>();
        services.AddScoped<IAddDevice, AddDevice>();
        services.AddScoped<IAddTelemetry, AddTelemetry>();
        services.AddScoped<IValidateDevice, ValidateDevice>();
        services.AddScoped<ISystemClock, SystemClock>();
        services.AddScoped<IConnectDevice, ConnectDevice>();
        services.AddScoped<IProcessTelemetry, ProcessTelemetry>();
        services.AddScoped<IGetTelemetryForDevice, GetTelemetryForDevice>();
        services.AddScoped<IGetInstructionsForDevice, GetInstructionsForDevice>();
        services.AddScoped<IGetDevice, GetDevice>();
        services.AddScoped<IPollForInstructions, PollForInstructions>();
        services.AddScoped<IGetAllRulesForTenant, GetAllRulesForTenant>();
    }
}