using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Serilog;
using Services.Messaging;
using Services.Queueing;

namespace Services.Telemetries;

public interface IAddTelemetry
{
    Task ExecuteAsync(AddTelemetryArgsDto args, CancellationToken cancellationToken);
}

public class AddTelemetry(AppDbContext dbContext, ISystemClock clock,
    IMessageManager messageManager) : IAddTelemetry
{
    public async Task ExecuteAsync(AddTelemetryArgsDto args, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<AddTelemetry>();
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
        await messageManager.PublishAsync(MessageChannels.ProcessTelemetry, new ProcessTelemetryMessage
        {
            Id = entry.Entity.Id
        });
        
        log.Information(
            "Added telemetry {TelemetryId} {DeviceId} {Type}", 
            entry.Entity.Id, 
            device.Id, 
            args.Type);
    }
}