using Microsoft.EntityFrameworkCore;

namespace Data;

public class AppDbContext : DbContext
{
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Telemetry> Telemetries { get; set; }
    public DbSet<Instruction> Instructions { get; set; }
    public DbSet<Rule> Rules { get; set; }
    public DbSet<RuleCondition> RuleConditions { get; set; }
    public DbSet<RuleAction> RuleActions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .AddInterceptors(new SoftDeleteInterceptor(), new UpdatableInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>().HasMany(x => x.Devices).WithOne(x => x.Tenant);
        modelBuilder.Entity<Tenant>().HasMany(x => x.Telemetries).WithOne(x => x.Tenant);
        modelBuilder.Entity<Tenant>().HasMany(x => x.Rules).WithOne(x => x.Tenant);
        
        modelBuilder.Entity<Device>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<Device>().HasMany(x => x.Instructions).WithOne(x => x.Device).IsRequired(false);
        modelBuilder.Entity<Device>().HasMany(x => x.Telemetries).WithOne(x => x.Device).IsRequired(false);
        
        modelBuilder.Entity<Rule>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<Rule>().HasMany(x => x.Conditions).WithOne(x => x.Rule);
        modelBuilder.Entity<Rule>().HasMany(x => x.Actions).WithOne(x => x.Rule);
        
        modelBuilder.Entity<RuleCondition>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<RuleCondition>().Property(x => x.Type).HasConversion<string>();
        modelBuilder.Entity<RuleCondition>().Property(x => x.ComparisonOperation).HasConversion<string>();
        modelBuilder.Entity<RuleCondition>().Property(x => x.Conversion).HasConversion<string>();
        
        modelBuilder.Entity<RuleAction>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder.Entity<RuleAction>().Property(x => x.Type).HasConversion<string>();
        modelBuilder.Entity<RuleAction>().Property(x => x.SendInstructionTargetDeviceType).HasConversion<string>();
    }
}