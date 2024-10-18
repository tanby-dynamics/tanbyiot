using Data;
using Serilog;
using Services;
using Services.Queueing;
using StackExchange.Redis;
using tanbyiot.WorkerService;

var loggerConfiguration = new LoggerConfiguration()
        .Enrich.WithProperty("Application", "tanbyiot.WorkerService")
#if DEBUG
        .WriteTo.Console()
        .WriteTo.Seq("http://localhost:5341")
#endif
    ;

Log.Logger = loggerConfiguration.CreateLogger();

Log.Information("Starting tanbyiot.WorkerService");

try
{
    var builder = Host.CreateApplicationBuilder(args);
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(Log.Logger);
    builder.Configuration
        .AddJsonFile("appsettings.json")
#if DEBUG
        .AddJsonFile("appsettings.Development.json")
#endif
        .AddEnvironmentVariables();

    // Configure Redis
    var redisConfiguration = builder.Configuration["Redis:Configuration"] 
                             ?? throw new Exception("Redis:Configuration setting not found");
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfiguration));
    
    // Register application services
    ConfigureData.Configure(builder.Services, builder.Configuration);
    ConfigureServices.Configure(builder.Services, builder.Configuration);

    // Register workers
    // TODO builder.Services.AddHostedService<ScheduledTasksWorker>();
    builder.Services.AddHostedService<ProcessTelemetryWorker>();

    var host = builder.Build();
    host.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "tanbyiot.WorkerService startup failed");
}
finally
{
    Log.CloseAndFlush();
}