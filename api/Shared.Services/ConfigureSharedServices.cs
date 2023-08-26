using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services.Options;
using Shared.Services.Queueing;

namespace Shared.Services;

public static class ConfigureSharedServices
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AzureStorageOptions>()
            .Bind(configuration.GetSection(AzureStorageOptions.Key))
            .ValidateDataAnnotations();
        services.AddScoped<IQueueManager, QueueManager>();
    }
}