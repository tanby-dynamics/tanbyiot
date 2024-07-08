using Data;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public static class Helper
{
    public static AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var dbContext = new TestAppDbContext(options);

        return dbContext;
    }
}

public class TestAppDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options);