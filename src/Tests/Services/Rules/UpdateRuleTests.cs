using FluentAssertions;
using Microsoft.Extensions.Internal;
using NSubstitute;
using Services.Rules;

namespace Tests.Services.Rules;

public class UpdateRuleTests
{
    [Fact]
    public async Task WhenUpdatingRule_FieldsAreUpdated()
    {
        var dbContext = Helper.GetDbContext();
        var clock = Substitute.For<ISystemClock>();
        clock.UtcNow.Returns(DateTimeOffset.Parse("2024-01-01"));
        var addRule = new AddRule(dbContext, clock);
        var rule = await addRule.ExecuteAsync("Rule", CancellationToken.None);
        
        clock.UtcNow.Returns(DateTimeOffset.Parse("2024-01-02"));
        var sut = new UpdateRule(dbContext, clock);

        var updatedRule = await sut.ExecuteAsync(rule.Id, new UpdateRuleArgs
        {
            Name = "Updated rule",
            Enabled = false
        }, CancellationToken.None);

        updatedRule.Name.Should().Be("Updated rule");
        updatedRule.Enabled.Should().BeFalse();
        updatedRule.UpdatedAt.Should().Be(DateTimeOffset.Parse("2024-01-02"));
    }
}