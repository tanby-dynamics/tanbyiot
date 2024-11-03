using Data;
using FluentAssertions;
using Services.Processing.Conditions;

namespace Tests.Services.Processing.Conditions;

public class CheckConditionForTelemetryTypesTests
{
    [Fact]
    public void GivenAllTypes_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            TelemetryTypeMatchingType = TelemetryTypeMatchingType.AllTypes
        };
        var telemetry = new Telemetry();

        var result = CheckConditionForTelemetryTypes.Execute(condition, telemetry);

        result.Should().BeTrue();
    }
    
    [Fact]
    public void GivenSpecifiedTypes_WhenMatchingTypesNull_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            TelemetryTypeMatchingType = TelemetryTypeMatchingType.SpecifiedTypes,
            TelemetryTypeMatchingSpecifiedTypes = null
        };
        var telemetry = new Telemetry();

        var result = CheckConditionForTelemetryTypes.Execute(condition, telemetry);

        result.Should().BeFalse();
    }
    
    [Fact]
    public void GivenSpecifiedTypes_WhenNoMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            TelemetryTypeMatchingType = TelemetryTypeMatchingType.SpecifiedTypes,
            TelemetryTypeMatchingSpecifiedTypes = "humidity, methane"
        };
        var telemetry = new Telemetry
        {
            Type = "temp"
        };

        var result = CheckConditionForTelemetryTypes.Execute(condition, telemetry);

        result.Should().BeFalse();
    }
    
    [Fact]
    public void GivenSpecifiedTypes_WhenMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            TelemetryTypeMatchingType = TelemetryTypeMatchingType.SpecifiedTypes,
            TelemetryTypeMatchingSpecifiedTypes = "humidity, methane"
        };
        var telemetry = new Telemetry
        {
            Type = "methane"
        };

        var result = CheckConditionForTelemetryTypes.Execute(condition, telemetry);

        result.Should().BeTrue();
    }
}