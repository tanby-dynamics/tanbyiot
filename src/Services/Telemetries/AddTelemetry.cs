using Data;
using Microsoft.Extensions.Internal;

namespace Services.Telemetries;

public interface IAddTelemetry
{
    Task ExecuteAsync(AddTelemetryDto dto, CancellationToken cancellationToken);
}

public class AddTelemetry(AppDbContext dbContext, ISystemClock clock) : IAddTelemetry
{
    public async Task ExecuteAsync(AddTelemetryDto dto, CancellationToken cancellationToken)
    {
        await dbContext.Telemetries.AddAsync(new Telemetry
        {
            TenantId = dto.TenantId,
            DeviceId = dto.DeviceId,
            Type = dto.Type,
            Value = dto.Value,
            Payload = dto.Payload,
            ReceivedAt = clock.UtcNow
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        // TODO enqueue to function app for processing
    }
}