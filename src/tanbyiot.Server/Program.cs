using System.Security.Claims;
using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Services;
using StackExchange.Redis;
using tanbyiot.Server;

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

// Add authentication and authorization
var auth0Domain = builder.Configuration["Auth0:Domain"] ?? throw new ArgumentException("Auth0:Domain not set");
var auth0Audience = builder.Configuration["Auth0:Audience"] ?? throw new ArgumentException("Auth0:Audience not set");
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = auth0Domain;
        options.Audience = auth0Audience;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(
        "admin:all",
        policy => policy.Requirements.Add(new HasPermissionsRequirement("admin:all", auth0Domain)));
builder.Services.AddSingleton<IAuthorizationHandler, HasPermissionsHandler>();

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
