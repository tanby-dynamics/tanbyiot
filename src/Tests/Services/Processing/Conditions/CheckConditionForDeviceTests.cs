using Data;
using FluentAssertions;
using Services.Processing.Conditions;

namespace Tests.Services.Processing.Conditions;

public class CheckConditionForDeviceTests
{
    [Fact]
    public void GivenAllDevices_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            DeviceMatchingType = DeviceMatchingType.AllDevices
        };
        var telemetry = new Telemetry();

        var result = CheckConditionForDevice.Execute(condition, telemetry);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenSingleDevice_WhenNoMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            DeviceMatchingType = DeviceMatchingType.SingleDevice,
            DeviceMatchingId = Guid.NewGuid()
        };
        var telemetry = new Telemetry
        {
            DeviceId = Guid.NewGuid()
        };

        var result = CheckConditionForDevice.Execute(condition, telemetry);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenSingleDevice_WhenMatch_ReturnsTrue()
    {
        var deviceId = Guid.NewGuid();
        var condition = new RuleCondition
        {
            DeviceMatchingType = DeviceMatchingType.SingleDevice,
            DeviceMatchingId = deviceId
        };
        var telemetry = new Telemetry
        {
            DeviceId = deviceId
        };

        var result = CheckConditionForDevice.Execute(condition, telemetry);

        result.Should().BeTrue();
    }

    [Fact]
    public void GivenSingleDevice_WhenMatchingIdNull_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            DeviceMatchingType = DeviceMatchingType.SingleDevice,
            DeviceMatchingId = null
        };
        var telemetry = new Telemetry
        {
            DeviceId = Guid.NewGuid()
        };

        var result = CheckConditionForDevice.Execute(condition, telemetry);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenDeviceGroups_WhenMatchingGroupsNull_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            DeviceMatchingType = DeviceMatchingType.DeviceGroups,
            DeviceMatchingGroups = null
        };
        var telemetry = new Telemetry();

        var result = CheckConditionForDevice.Execute(condition, telemetry);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenDeviceGroups_WhenGroupsDoNotMatch_ReturnsFalse()
    {
        var condition = new RuleCondition
        {
            DeviceMatchingType = DeviceMatchingType.DeviceGroups,
            DeviceMatchingGroups = "group-2, group-3"
        };
        var telemetry = new Telemetry
        {
            Device = new Device()
            {
                GroupName = "group-1"
            }
        };

        var result = CheckConditionForDevice.Execute(condition, telemetry);

        result.Should().BeFalse();
    }

    [Fact]
    public void GivenDeviceGroups_WhenGroupsMatch_ReturnsTrue()
    {
        var condition = new RuleCondition
        {
            DeviceMatchingType = DeviceMatchingType.DeviceGroups,
            DeviceMatchingGroups = "group-2, group-3"
        };
        var telemetry = new Telemetry
        {
            Device = new Device()
            {
                GroupName = "group-3"
            }
        };

        var result = CheckConditionForDevice.Execute(condition, telemetry);

        result.Should().BeTrue();
    }
}