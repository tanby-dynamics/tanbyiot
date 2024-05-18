using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;
using Serilog;
using Services.Queueing;
using Services.Telemetries;

namespace Functions;

// ReSharper disable once ClassNeverInstantiated.Global
public class ProcessTelemetryFunction(IProcessTelemetry processTelemetry)
{
    [Function("ProcessTelemetry")]
    public async Task ProcessTelemetryAsync(
        [QueueTrigger(Queues.ProcessTelemetry)]
        string messageJson,
        CancellationToken cancellationToken)
    {
        var message = JsonConvert.DeserializeObject<ProcessTelemetryMessage>(messageJson);
        if (message == null)
        {
            Log.Error("Cannot deserialize process telemetry message");
            return;
        }
        
        Log.Information("Processing telemetry {TelemetryId}", message.Id);
        await processTelemetry.ExecuteAsync(message.Id, cancellationToken);
    }
}