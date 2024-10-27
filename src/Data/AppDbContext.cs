using Microsoft.EntityFrameworkCore;

namespace Data;

public class AppDbContext : DbContext
{
    public DbSet<Device> Devices { get; set; }
    public DbSet<Telemetry> Telemetries { get; set; }
    public DbSet<Instruction> Instructions { get; set; }
    public DbSet<Rule> Rules { get; set; }
    public DbSet<RuleCondition> RuleConditions { get; set; }
    public DbSet<RuleAction> RuleActions { get; set; }
    public DbSet<ApplicationState> ApplicationStates { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<Device>().HasMany(x => x.Instructions).WithOne(x => x.Device).IsRequired(false);
        modelBuilder.Entity<Device>().HasMany(x => x.Telemetries).WithOne(x => x.Device).IsRequired(false);
        
        modelBuilder.Entity<Rule>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<Rule>().HasMany(x => x.Conditions).WithOne(x => x.Rule);
        modelBuilder.Entity<Rule>().HasMany(x => x.Actions).WithOne(x => x.Rule);
        
        modelBuilder.Entity<RuleCondition>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<RuleCondition>().Property(x => x.Type).HasConversion<string>();
        modelBuilder.Entity<RuleCondition>().Property(x => x.ApplicationStateComparisonOperationType).HasConversion<string>();
        modelBuilder.Entity<RuleCondition>().Property(x => x.DeviceMatchingType).HasConversion<string>();
        modelBuilder.Entity<RuleCondition>().Property(x => x.TelemetryTypeMatchingType).HasConversion<string>();
        modelBuilder.Entity<RuleCondition>().Property(x => x.TelemetryValueMatchingType).HasConversion<string>();
        modelBuilder.Entity<RuleCondition>().Property(x => x.TelemetryValueMatchingComparisonOperationType).HasConversion<string>();
        
        modelBuilder.Entity<RuleAction>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<RuleAction>().Property(x => x.Type).HasConversion<string>();
        modelBuilder.Entity<RuleAction>().Property(x => x.SendInstructionTargetDeviceType).HasConversion<string>();

        modelBuilder.Entity<ApplicationState>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<ApplicationState>().HasIndex(x => x.Key);
    }
}