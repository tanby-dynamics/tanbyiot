using Data;

namespace Services.Processing.Conditions;

public class CheckTelemetryCondition : ICheckCondition
{
    public Task<bool> ExecuteAsync(RuleCondition condition, ApplicationContext context, CancellationToken cancellationToken)
    {
        if (context.CurrentTelemetry is null)
        {
            return Task.FromResult(false);
        }

        if (!CheckForDevice(condition, context.CurrentTelemetry))
        {
            return Task.FromResult(false);
        }

        if (!CheckForTelemetryTypes(condition, context.CurrentTelemetry))
        {
            return Task.FromResult(false);
        }
        
        // TODO telemetry value
        // if (!CheckForTelemetryValue(condition, context.CurrentTelemetry))
        //{
        //    return Task.FromResult(false);
        //}

        return Task.FromResult(true);
    }

    private static bool CheckForDevice(RuleCondition condition, Telemetry telemetry)
    {
        return condition.DeviceMatchingType switch
        {
            DeviceMatchingType.AllDevices => true,
            DeviceMatchingType.SingleDevice => telemetry.DeviceId == condition.DeviceMatchingId,
            DeviceMatchingType.DeviceGroups => (condition.DeviceMatchingGroups ?? string.Empty)
                .Split(",")
                .Select(x => x.Trim())
                .Any(x => x == telemetry.Device.GroupName),
            _ => false
        };
    }

    private static bool CheckForTelemetryTypes(RuleCondition condition, Telemetry telemetry)
    {
        return condition.TelemetryTypeMatchingType switch
        {
            TelemetryTypeMatchingType.AllTypes => true,
            TelemetryTypeMatchingType.SpecifiedTypes => (condition.TelemetryTypeMatchingSpecifiedTypes ?? string.Empty)
                .Split(",")
                .Select(x => x.Trim())
                .Any(x => telemetry.Type == x),
            _ => false
        };
    }

    private static bool CheckForTelemetryValue(RuleCondition condition, Telemetry telemetry)
    {
        /*
         * string json = "{ 'name': 'John', 'age': 30, 'address': { 'city': 'New York' } }";
JObject obj = JObject.Parse(json);

// User-specified path
string path = "$.address.city";
JToken token = obj.SelectToken(path);

Console.WriteLine(token);  // Output: New York
         */

        throw new NotImplementedException();
    }
}