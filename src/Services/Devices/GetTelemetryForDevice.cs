using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Devices;

public interface IGetTelemetryForDevice
{
    Task<IEnumerable<TelemetryDto>> ExecuteAsync(Guid deviceId, int take, CancellationToken cancellationToken);
}

public class GetTelemetryForDevice(AppDbContext dbContext) : IGetTelemetryForDevice
{
    public async Task<IEnumerable<TelemetryDto>> ExecuteAsync(Guid deviceId, int take, CancellationToken cancellationToken)
    {
        var results = await dbContext.Telemetries
            .Where(x => x.DeviceId == deviceId)
            .OrderByDescending(x => x.ReceivedAt)
            .Take(take)
            .Select(x => new TelemetryDto
            {
                Id = x.Id,
                DeviceId = x.Id,
                Type = x.Type,
                Value = x.Value,
                Payload = x.Payload,
                ReceivedAt = x.ReceivedAt,
            })
            .ToListAsync(cancellationToken);

        return results;
    }
}

public class TelemetryDto
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Payload { get; set; }
    public DateTimeOffset ReceivedAt { get; set; }
}