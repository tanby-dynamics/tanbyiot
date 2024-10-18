using Data;
using Serilog;
using Services;
using StackExchange.Redis;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting up API");
var builder = WebApplication.CreateBuilder(args);

// Load configuration
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
#if DEBUG
    .AddJsonFile("appsettings.Development.json")
#endif
    .AddEnvironmentVariables();

// Set up logging
builder.Host.UseSerilog(
    (context, services, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.WithProperty("Application", "tanbyiot.Server")
            .WriteTo.Console()
#if DEBUG
            // TODO make this configurable
            .WriteTo.Seq("http://localhost:5341")
#endif
            ;
    });

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configurePolicy =>
    {
        // TODO make this configurable
        configurePolicy
            .WithOrigins("http://localhost:5173", "https://dev-portal.tanbyiot.app", "https://portal.tanbyiot.app")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Set up the API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register configuration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Configure Redis
var redisConfiguration = builder.Configuration["Redis:Configuration"] 
                         ?? throw new Exception("Redis:Configuration setting not found");
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConfiguration));

// Register application services
ConfigureData.Configure(builder.Services, builder.Configuration);
ConfigureServices.Configure(builder.Services, builder.Configuration);

// Set up the application
var app = builder.Build();

app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();
