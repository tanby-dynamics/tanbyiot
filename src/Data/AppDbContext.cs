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
            .AddInterceptors(new SoftDeleteInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder
            .Entity<Device>()
            .HasOne(x => x.Tenant)
            .WithMany(x => x.Devices);
        modelBuilder
            .Entity<Telemetry>()
            .HasOne(x => x.Tenant)
            .WithMany(x => x.Telemetries);
        modelBuilder
            .Entity<Telemetry>()
            .HasOne(x => x.Device)
            .WithMany(x => x.Telemetries);
        modelBuilder
            .Entity<Instruction>()
            .HasOne(x => x.Device)
            .WithMany(x => x.Instructions);
        modelBuilder.Entity<Rule>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder
            .Entity<Rule>()
            .HasOne(x => x.Tenant)
            .WithMany(x => x.Rules);
        modelBuilder
            .Entity<Rule>()
            .HasMany(x => x.Conditions)
            .WithOne(x => x.Rule);
        modelBuilder
            .Entity<Rule>()
            .HasMany(x => x.Actions)
            .WithOne(x => x.Rule);
        modelBuilder.Entity<RuleCondition>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder
            .Entity<RuleCondition>()
            .Property(x => x.Type)
            .HasConversion<string>();
        modelBuilder.Entity<RuleAction>().HasQueryFilter(x => x.DeletedAt == null);
        modelBuilder
            .Entity<RuleAction>()
            .Property(x => x.Type)
            .HasConversion<string>();
    }
}