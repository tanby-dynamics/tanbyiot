using Data;
using FluentAssertions;
using Services.Processing.Conditions;

namespace Tests.Services.Processing.Conditions;

public class CheckConditionForTelemetryValueTests
{
    [Fact]
    public void GivenAlwaysMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.AlwaysMatch
        };
        var telemetry = new Telemetry();

        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenUseValue_WhenValueIsNull_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.UseValue
        };
        var telemetry = new Telemetry
        {
            Value = null
        };
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenParsePayload_WhenPayloadPathNull_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.ParsePayload,
            TelemetryValueMatchingPayloadPath = null
        };
        var telemetry = new Telemetry();
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenParsePayload_WhenPayloadNull_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.ParsePayload,
            TelemetryValueMatchingPayloadPath = "$.humidity.value"
        };
        var telemetry = new Telemetry
        {
            Payload = null
        };
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsTrue_GivenMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.ParsePayload,
            TelemetryValueMatchingPayloadPath = "$.humidity.value",
            TelemetryValueMatchingComparisonOperationType = ComparisonOperationType.IsTrue
        };
        var telemetry = new Telemetry
        {
            Payload = @"{""humidity"":{""value"":""true""}}"
        };
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsTrue_GivenNoMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.ParsePayload,
            TelemetryValueMatchingPayloadPath = "$.humidity.value",
            TelemetryValueMatchingComparisonOperationType = ComparisonOperationType.IsTrue
        };
        var telemetry = new Telemetry
        {
            Payload = @"{""humidity"":{""value"":""false""}}"
        };
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsFalse_GivenMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.ParsePayload,
            TelemetryValueMatchingPayloadPath = "$.humidity.value",
            TelemetryValueMatchingComparisonOperationType = ComparisonOperationType.IsFalse
        };
        var telemetry = new Telemetry
        {
            Payload = @"{""humidity"":{""value"":""false""}}"
        };
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsFalse_GivenNoMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.ParsePayload,
            TelemetryValueMatchingPayloadPath = "$.humidity.value",
            TelemetryValueMatchingComparisonOperationType = ComparisonOperationType.IsFalse
        };
        var telemetry = new Telemetry
        {
            Payload = @"{""humidity"":{""value"":""true""}}"
        };
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsEquals_GivenMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.ParsePayload,
            TelemetryValueMatchingPayloadPath = "$.humidity.value",
            TelemetryValueMatchingComparisonOperationType = ComparisonOperationType.Equals,
            TelemetryValueMatchingValue = "123.45"
        };
        var telemetry = new Telemetry
        {
            Payload = @"{""humidity"":{""value"":123.45}}"
        };
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsEquals_GivenNoMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.ParsePayload,
            TelemetryValueMatchingPayloadPath = "$.humidity.value",
            TelemetryValueMatchingComparisonOperationType = ComparisonOperationType.Equals,
            TelemetryValueMatchingValue = "123.456"
        };
        var telemetry = new Telemetry
        {
            Payload = @"{""humidity"":{""value"":123.45}}"
        };
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsNotEquals_GivenMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.ParsePayload,
            TelemetryValueMatchingPayloadPath = "$.humidity.value",
            TelemetryValueMatchingComparisonOperationType = ComparisonOperationType.NotEquals,
            TelemetryValueMatchingValue = "123.456"
        };
        var telemetry = new Telemetry
        {
            Payload = @"{""humidity"":{""value"":123.45}}"
        };
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsNotEquals_GivenNoMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.ParsePayload,
            TelemetryValueMatchingPayloadPath = "$.humidity.value",
            TelemetryValueMatchingComparisonOperationType = ComparisonOperationType.NotEquals,
            TelemetryValueMatchingValue = "123.456"
        };
        var telemetry = new Telemetry
        {
            Payload = @"{""humidity"":{""value"":123.456}}"
        };
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsGreaterThan_GivenMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.ParsePayload,
            TelemetryValueMatchingPayloadPath = "$.humidity.value",
            TelemetryValueMatchingComparisonOperationType = ComparisonOperationType.GreaterThan,
            TelemetryValueMatchingValue = "11"
        };
        var telemetry = new Telemetry
        {
            Payload = @"{""humidity"":{""value"":12}}"
        };
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsGreaterThan_GivenNoMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.ParsePayload,
            TelemetryValueMatchingPayloadPath = "$.humidity.value",
            TelemetryValueMatchingComparisonOperationType = ComparisonOperationType.GreaterThan,
            TelemetryValueMatchingValue = "12"
        };
        var telemetry = new Telemetry
        {
            Payload = @"{""humidity"":{""value"":11}}"
        };
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsLessThan_GivenMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.ParsePayload,
            TelemetryValueMatchingPayloadPath = "$.humidity.value",
            TelemetryValueMatchingComparisonOperationType = ComparisonOperationType.LessThan,
            TelemetryValueMatchingValue = "11"
        };
        var telemetry = new Telemetry
        {
            Payload = @"{""humidity"":{""value"":10}}"
        };
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsLessThan_GivenNoMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            TelemetryValueMatchingType = TelemetryValueMatchingType.ParsePayload,
            TelemetryValueMatchingPayloadPath = "$.humidity.value",
            TelemetryValueMatchingComparisonOperationType = ComparisonOperationType.LessThan,
            TelemetryValueMatchingValue = "10"
        };
        var telemetry = new Telemetry
        {
            Payload = @"{""humidity"":{""value"":11}}"
        };
        
        var result = CheckConditionForTelemetryValue.Execute(condition, telemetry);

        result.Should().BeFalse();
    }
}