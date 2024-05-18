using Microsoft.EntityFrameworkCore;
using Migrations;

var contextFactory = new DbContextFactory();
var context = contextFactory.CreateDbContext(args);

context.Database.SetCommandTimeout(300);

var pendingMigrations = context.Database.GetPendingMigrations().ToList();
var appliedMigrations = context.Database.GetAppliedMigrations().ToList();
if (!pendingMigrations.Any())
{
    Console.WriteLine("Database is up to date");
    Console.WriteLine("Applied migrations:");
    foreach (var migration in appliedMigrations)
    {
        Console.WriteLine(migration);
    }
}
else
{
    Console.WriteLine("Migrating database");
    Console.WriteLine("Pending migrations:");
    foreach (var migration in pendingMigrations)
    {
        Console.WriteLine(migration);
    }
    Console.WriteLine("Applied migrations:");
    foreach (var migration in appliedMigrations)
    {
        Console.WriteLine(migration);
    }
    
    context.Database.Migrate();
    
    Console.WriteLine("Complete");
}



