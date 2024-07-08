using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Services.Rules;
using Services.Tenants;
using Services.Users;

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
    
    public static async Task<RuleDto> CreateNewRule(AppDbContext dbContext, ISystemClock clock, TenantDto? tenant)
    {
        var addRule = new AddRule(dbContext, clock);
        var rule = await addRule.ExecuteAsync(tenant!.Id, "Rule", CancellationToken.None);
        return rule;
    }

    public static async Task<TenantDto> CreateNewTenant(AppDbContext dbContext, UserDto user, string name)
    {
        var addTenant = new AddTenantForUser(dbContext);
        var (_, tenant) = await addTenant.ExecuteAsync(
            new AddTenantForUserArgs
            {
                Name = name,
                SubscriptionLevel = SubscriptionLevel.Developer
            }, user, CancellationToken.None);
        return tenant!;
    }

    public static async Task<UserDto> CreateNewUser(AppDbContext dbContext, ISystemClock clock)
    {
        var addUser = new AddUser(dbContext, clock);
        var user = await addUser.ExecuteAsync("external id", "test@tanbyiot.app", CancellationToken.None);
        return user;
    }
}

public class TestAppDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options);