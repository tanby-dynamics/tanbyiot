﻿using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;
using Serilog;
using Services.Processing;
using Services.Queueing;

namespace Functions;

// ReSharper disable once ClassNeverInstantiated.Global
public class ProcessTelemetryFunction(IProcessTelemetry processTelemetry)
{
    [Function(nameof(ProcessTelemetryFunction))]
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