using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Serilog;
using Services.Queueing;

namespace Services.Telemetries;

public interface IAddTelemetry
{
    Task ExecuteAsync(AddTelemetryArgsDto args, CancellationToken cancellationToken);
}

public class AddTelemetry(AppDbContext dbContext, ISystemClock clock,
    IQueueManager queueManager) : IAddTelemetry
{
    public async Task ExecuteAsync(AddTelemetryArgsDto args, CancellationToken cancellationToken)
    {
        var now = clock.UtcNow;
        
        var entry = await dbContext.Telemetries.AddAsync(new Telemetry
        {
            TenantId = args.TenantId,
            DeviceId = args.DeviceId,
            Type = args.Type,
            Value = args.Value,
            Payload = args.Payload,
            ReceivedAt = now
        }, cancellationToken);

        var device = await dbContext.Devices.SingleAsync(x => x.Id == args.DeviceId, cancellationToken);
        device.LastConnected = now;
        
        await dbContext.SaveChangesAsync(cancellationToken);
        await queueManager.SendMessageAsync(Queues.ProcessTelemetry, new ProcessTelemetryMessage
        {
            Id = entry.Entity.Id
        }, cancellationToken);
        
        Log.Information(
            "Added telemetry {TelemetryId} {DeviceId} {Type}", 
            entry.Entity.Id, 
            device.Id, 
            args.Type);
    }
}