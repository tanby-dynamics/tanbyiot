using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Serilog;
using Services.Queueing;

namespace Services.Telemetries;

public interface IAddTelemetry
{
    Task ExecuteAsync(AddTelemetryRequestDto requestDto, CancellationToken cancellationToken);
}

public class AddTelemetry(AppDbContext dbContext, ISystemClock clock,
    IQueueManager queueManager) : IAddTelemetry
{
    public async Task ExecuteAsync(AddTelemetryRequestDto requestDto, CancellationToken cancellationToken)
    {
        var now = clock.UtcNow;
        
        var entry = await dbContext.Telemetries.AddAsync(new Telemetry
        {
            TenantId = requestDto.TenantId,
            DeviceId = requestDto.DeviceId,
            Type = requestDto.Type,
            Value = requestDto.Value,
            Payload = requestDto.Payload,
            ReceivedAt = now
        }, cancellationToken);

        var device = await dbContext.Devices.SingleAsync(x => x.Id == requestDto.DeviceId, cancellationToken);
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
            requestDto.Type);
    }
}