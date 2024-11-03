using Data;

namespace Services.Processing.Conditions;

public static class CheckConditionForDevice
{
    public static bool Execute(RuleCondition condition, Telemetry telemetry)
    {
        return condition.DeviceMatchingType switch
        {
            DeviceMatchingType.AllDevices => true,
            DeviceMatchingType.SingleDevice => condition.DeviceMatchingId != null && telemetry.DeviceId == condition.DeviceMatchingId,
            DeviceMatchingType.DeviceGroups => (condition.DeviceMatchingGroups ?? string.Empty)
                .Split(",")
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Any(x => x == telemetry.Device.GroupName),
            _ => false
        };
    }
}