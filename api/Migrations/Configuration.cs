using Microsoft.Extensions.Configuration;

namespace Migrations;

public static class Configuration
{
    public static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile("appsettings.Development.json", true)
            .AddEnvironmentVariables() // We use environment variables in release pipeline, not config files
            .Build();
    }
}