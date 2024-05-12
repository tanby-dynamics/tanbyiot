using Microsoft.EntityFrameworkCore;

namespace Data;

public interface IAppDbContext
{
    DbSet<Tenant> Tenants { get; set; }
}

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Tenant> Tenants { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }
}