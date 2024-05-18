using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Services.Telemetries;

public interface IAddTelemetry
{
    Task ExecuteAsync(AddTelemetryRequestDto requestDto, CancellationToken cancellationToken);
}

public class AddTelemetry(AppDbContext dbContext, ISystemClock clock) : IAddTelemetry
{
    public async Task ExecuteAsync(AddTelemetryRequestDto requestDto, CancellationToken cancellationToken)
    {
        var now = clock.UtcNow;
        
        await dbContext.Telemetries.AddAsync(new Telemetry
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

        // TODO enqueue to function app for processing
    }
}