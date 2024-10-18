using FluentAssertions;
using Microsoft.Extensions.Internal;
using NSubstitute;
using Services.Rules;

namespace Tests.Services.Rules;

public class AddRuleTests
{
    [Fact]
    public async Task WhenAddingNewRule_ItIsAsExpected()
    {
        var dbContext = Helper.GetDbContext();
        var clock = Substitute.For<ISystemClock>();
        clock.UtcNow.Returns(DateTimeOffset.Parse("2024-07-08"));

        var sut = new AddRule(dbContext, clock);

        var result = await sut.ExecuteAsync("Test rule", CancellationToken.None);

        result.Name.Should().Be("Test rule");
        result.CreatedAt.Should().Be(DateTimeOffset.Parse("2024-07-08"));
        result.Enabled.Should().BeTrue();
    }
}