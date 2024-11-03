using Data;
using FluentAssertions;
using Services.Processing.Conditions;

namespace Tests.Services.Processing.Conditions;

public class CheckConditionForApplicationStateValueTests
{
    [Fact]
    public void GivenParsePayload_WhenPayloadPathNull_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            ApplicationStateMatchingType = ApplicationStateMatchingType.ParsePayload,
            ApplicationStateMatchingPayloadPath = null
        };
        var state = new ApplicationState();
        
        var result = CheckConditionForApplicationStateValue.Execute(condition, state);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsTrue_GivenMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            ApplicationStateMatchingType = ApplicationStateMatchingType.ParsePayload,
            ApplicationStateMatchingPayloadPath = "$.humidity.value",
            ApplicationStateComparisonOperationType = ComparisonOperationType.IsTrue
        };
        var state = new ApplicationState
        {
            Value = @"{""humidity"":{""value"":""true""}}"
        };
        
        var result = CheckConditionForApplicationStateValue.Execute(condition, state);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsTrue_GivenNoMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            ApplicationStateMatchingType = ApplicationStateMatchingType.ParsePayload,
            ApplicationStateMatchingPayloadPath = "$.humidity.value",
            ApplicationStateComparisonOperationType = ComparisonOperationType.IsTrue
        };
        var state = new ApplicationState
        {
            Value = @"{""humidity"":{""value"":""false""}}"
        };
        
        var result = CheckConditionForApplicationStateValue.Execute(condition, state);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsFalse_GivenMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            ApplicationStateMatchingType = ApplicationStateMatchingType.ParsePayload,
            ApplicationStateMatchingPayloadPath = "$.humidity.value",
            ApplicationStateComparisonOperationType = ComparisonOperationType.IsFalse
        };
        var state = new ApplicationState
        {
            Value = @"{""humidity"":{""value"":""false""}}"
        };
        
        var result = CheckConditionForApplicationStateValue.Execute(condition, state);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsFalse_GivenNoMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            ApplicationStateMatchingType = ApplicationStateMatchingType.ParsePayload,
            ApplicationStateMatchingPayloadPath = "$.humidity.value",
            ApplicationStateComparisonOperationType = ComparisonOperationType.IsFalse
        };
        var state = new ApplicationState
        {
            Value = @"{""humidity"":{""value"":""true""}}"
        };
        
        var result = CheckConditionForApplicationStateValue.Execute(condition, state);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsEquals_GivenMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            ApplicationStateMatchingType = ApplicationStateMatchingType.ParsePayload,
            ApplicationStateMatchingPayloadPath = "$.humidity.value",
            ApplicationStateComparisonOperationType = ComparisonOperationType.Equals,
            ApplicationStateMatchingValue = "123.45"
        };
        var state = new ApplicationState
        {
            Value = @"{""humidity"":{""value"":123.45}}"
        };
        
        var result = CheckConditionForApplicationStateValue.Execute(condition, state);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsEquals_GivenNoMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            ApplicationStateMatchingType = ApplicationStateMatchingType.ParsePayload,
            ApplicationStateMatchingPayloadPath = "$.humidity.value",
            ApplicationStateComparisonOperationType = ComparisonOperationType.Equals,
            ApplicationStateMatchingValue = "123.456"
        };
        var state = new ApplicationState
        {
            Value = @"{""humidity"":{""value"":123.45}}"
        };
        
        var result = CheckConditionForApplicationStateValue.Execute(condition, state);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsNotEquals_GivenMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            ApplicationStateMatchingType = ApplicationStateMatchingType.ParsePayload,
            ApplicationStateMatchingPayloadPath = "$.humidity.value",
            ApplicationStateComparisonOperationType = ComparisonOperationType.NotEquals,
            ApplicationStateMatchingValue = "123.456"
        };
        var state = new ApplicationState
        {
            Value = @"{""humidity"":{""value"":123.45}}"
        };
        
        var result = CheckConditionForApplicationStateValue.Execute(condition, state);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsNotEquals_GivenNoMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            ApplicationStateMatchingType = ApplicationStateMatchingType.ParsePayload,
            ApplicationStateMatchingPayloadPath = "$.humidity.value",
            ApplicationStateComparisonOperationType = ComparisonOperationType.NotEquals,
            ApplicationStateMatchingValue = "123.456"
        };
        var state = new ApplicationState
        {
            Value = @"{""humidity"":{""value"":123.456}}"
        };
        
        var result = CheckConditionForApplicationStateValue.Execute(condition, state);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsGreaterThan_GivenMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            ApplicationStateMatchingType = ApplicationStateMatchingType.ParsePayload,
            ApplicationStateMatchingPayloadPath = "$.humidity.value",
            ApplicationStateComparisonOperationType = ComparisonOperationType.GreaterThan,
            ApplicationStateMatchingValue = "11"
        };
        var state = new ApplicationState
        {
            Value = @"{""humidity"":{""value"":12}}"
        };
        
        var result = CheckConditionForApplicationStateValue.Execute(condition, state);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsGreaterThan_GivenNoMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            ApplicationStateMatchingType = ApplicationStateMatchingType.ParsePayload,
            ApplicationStateMatchingPayloadPath = "$.humidity.value",
            ApplicationStateComparisonOperationType = ComparisonOperationType.GreaterThan,
            ApplicationStateMatchingValue = "12"
        };
        var state = new ApplicationState
        {
            Value = @"{""humidity"":{""value"":11}}"
        };
        
        var result = CheckConditionForApplicationStateValue.Execute(condition, state);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsLessThan_GivenMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            ApplicationStateMatchingType = ApplicationStateMatchingType.ParsePayload,
            ApplicationStateMatchingPayloadPath = "$.humidity.value",
            ApplicationStateComparisonOperationType = ComparisonOperationType.LessThan,
            ApplicationStateMatchingValue = "11"
        };
        var state = new ApplicationState
        {
            Value = @"{""humidity"":{""value"":10}}"
        };
        
        var result = CheckConditionForApplicationStateValue.Execute(condition, state);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenParsePayloadAndOperationIsLessThan_GivenNoMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            ApplicationStateMatchingType = ApplicationStateMatchingType.ParsePayload,
            ApplicationStateMatchingPayloadPath = "$.humidity.value",
            ApplicationStateComparisonOperationType = ComparisonOperationType.LessThan,
            ApplicationStateMatchingValue = "10"
        };
        var state = new ApplicationState
        {
            Value = @"{""humidity"":{""value"":11}}"
        };
        
        var result = CheckConditionForApplicationStateValue.Execute(condition, state);

        result.Should().BeFalse();
    }
}