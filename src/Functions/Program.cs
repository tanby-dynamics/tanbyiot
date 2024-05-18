using Data;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Services;

var loggerConfiguration = new LoggerConfiguration()
        .Enrich.WithProperty("Application", "Functions")
#if DEBUG
        .WriteTo.Console()
        .WriteTo.Seq("http://localhost:5341")
#endif
    ;

Log.Logger = loggerConfiguration.CreateLogger();

var host = new HostBuilder()
    .ConfigureAppConfiguration(configurationBuilder => configurationBuilder
        .AddJsonFile("appsettings.json")
#if DEBUG
        .AddJsonFile("appsettings.Development.json")
#endif
        .AddEnvironmentVariables()
    )
    .ConfigureFunctionsWorkerDefaults()
    
    .UseSerilog()
    .ConfigureServices((context, services) =>
    {
        ConfigureData.Configure(services, context.Configuration);
        ConfigureServices.Configure(services, context.Configuration);
    })
    .Build();

host.Run();