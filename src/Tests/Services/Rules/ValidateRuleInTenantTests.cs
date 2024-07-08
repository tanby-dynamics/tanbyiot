using FluentAssertions;
using Microsoft.Extensions.Internal;
using NSubstitute;
using Services.Rules;

namespace Tests.Services.Rules;

public class ValidateRuleInTenantTests
{
    [Fact]
    public async Task WhenRuleExistsInTenant_ItReturnsTrue()
    {
        var dbContext = Helper.GetDbContext();
        var clock = Substitute.For<ISystemClock>();
        var user = await Helper.CreateNewUser(dbContext, clock);
        var tenant = await Helper.CreateNewTenant(dbContext, user, "Tenant");
        var rule = await Helper.CreateNewRule(dbContext, clock, tenant);

        var sut = new ValidateRuleInTenant(dbContext);

        var result = await sut.ExecuteAsync(tenant.Id, rule.Id, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task WhenRuleInDifferentTenant_ItReturnsFalse()
    {
        var dbContext = Helper.GetDbContext();
        var clock = Substitute.For<ISystemClock>();
        var user = await Helper.CreateNewUser(dbContext, clock);
        var tenant1 = await Helper.CreateNewTenant(dbContext, user, "tenant 1");
        var rule = await Helper.CreateNewRule(dbContext, clock, tenant1);
        var tenant2 = await Helper.CreateNewTenant(dbContext, user, "tenant 2");

        var sut = new ValidateRuleInTenant(dbContext);

        var result = await sut.ExecuteAsync(tenant2.Id, rule.Id, CancellationToken.None);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task WhenRuleDoesNotExist_ItReturnsFalse()
    {
        var dbContext = Helper.GetDbContext();
        var clock = Substitute.For<ISystemClock>();
        var user = await Helper.CreateNewUser(dbContext, clock);
        var tenant = await Helper.CreateNewTenant(dbContext, user, "tenant");
        await Helper.CreateNewRule(dbContext, clock, tenant);

        var sut = new ValidateRuleInTenant(dbContext);

        var result = await sut.ExecuteAsync(tenant.Id, Guid.NewGuid(), CancellationToken.None);

        result.Should().BeFalse();
    }
}