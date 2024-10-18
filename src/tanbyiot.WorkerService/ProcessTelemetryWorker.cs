using Newtonsoft.Json;
using Serilog;
using Services.Processing;
using Services.Queueing;
using StackExchange.Redis;

namespace tanbyiot.WorkerService;

public class ProcessTelemetryWorker(
    IConnectionMultiplexer connectionMultiplexer,
    IServiceProvider serviceProvider
) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var log = Log.ForContext<ProcessTelemetryWorker>();
        
        log.Information("Configuring process telemetry worker");
        
        var subscriber = connectionMultiplexer.GetSubscriber();

        subscriber.Subscribe("process-telemetry")
            .OnMessage(async channelMessage =>
            {
                log.Information("Received process telemetry message");
                
                if (!channelMessage.Message.HasValue)
                {
                    log.Error("Message has no value");
                    return;
                }

                var message = JsonConvert.DeserializeObject<ProcessTelemetryMessage>(channelMessage.Message!);
                if (message == null)
                {
                    log.Error("Cannot deserialize process telemetry message");
                    return;
                }
                
                log.Information("Processing telemetry message for {TelemetryId}", message.Id);

                using var scope = serviceProvider.CreateScope();
                var processTelemetry = scope.ServiceProvider.GetRequiredService<IProcessTelemetry>();

                try
                {
                    await processTelemetry.ExecuteAsync(message.Id, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    log.Error(ex, "Error processing telemetry message");
                }
            });

        return Task.CompletedTask;
    }
}