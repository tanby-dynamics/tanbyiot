using Microsoft.EntityFrameworkCore;

namespace Data;

public interface IAppDbContext
{
    DbSet<Tenant> Tenants { get; set; }
    DbSet<Device> Devices { get; set; }
}

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Device> Devices { get; set; }

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
    }
}