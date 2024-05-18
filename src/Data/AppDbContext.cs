using Microsoft.EntityFrameworkCore;

namespace Data;

public class AppDbContext : DbContext
{
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Telemetry> Telemetries { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Tenant>()
            .HasMany(x => x.Devices);
        modelBuilder
            .Entity<Device>()
            .HasOne(x => x.Tenant);
        modelBuilder
            .Entity<Telemetry>()
            .HasOne(x => x.Tenant)
            .WithMany(x => x.Telemetries);
        modelBuilder
            .Entity<Telemetry>()
            .HasOne(x => x.Device)
            .WithMany(x => x.Telemetries);
    }
}