using Microsoft.Extensions.DependencyInjection;
using Services;

namespace Api.Services;

public static class ConfigureApiServices
{
    public static void Configure(IServiceCollection services)
    {
        services.AddScoped<IDeviceService, DeviceService>();
    }
}