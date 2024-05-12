using Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Services;

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
        ConfigureData.Configure(services, configuration);
        ConfigureServices.Configure(services, configuration);
    })
    .Build();

host.Run();
