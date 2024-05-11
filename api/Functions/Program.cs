using Data;
using Functions.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Shared.Services;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true)
#if DEBUG
    .AddJsonFile("appsettings.development.json", true)
#endif
    .AddEnvironmentVariables()
    .Build();

var loggerConfiguration = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .Enrich.WithProperty("Application", "Functions")
#if DEBUG
        .WriteTo.Console()
        .WriteTo.Seq("http://localhost:5341")
#endif
    ;

Log.Logger = loggerConfiguration.CreateLogger();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .UseSerilog()
    .ConfigureServices(services =>
    {
        ConfigureFunctionsServices.Configure(services, configuration);
        ConfigureData.Configure(services, configuration);
        ConfigureSharedServices.Configure(services, configuration);
    })
    .Build();

host.Run();
