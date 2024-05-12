using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data;

public static class ConfigureData
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection")),
            ServiceLifetime.Transient);
    }
}