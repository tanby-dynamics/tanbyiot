using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Devices;

public interface IGetInstructionsForDevice
{
    Task<IEnumerable<InstructionDto>> ExecuteAsync(Guid deviceId, int take, CancellationToken cancellationToken);
}

public class GetInstructionsForDevice(AppDbContext dbContext) : IGetInstructionsForDevice
{
    public async Task<IEnumerable<InstructionDto>> ExecuteAsync(Guid deviceId, int take, CancellationToken cancellationToken)
    {
        var results = await dbContext.Instructions
            .Where(x => x.DeviceId == deviceId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(take)
            .Select(x => new InstructionDto
            {
                Id = x.Id,
                DeviceId = x.Id,
                Type = x.Type,
                Value = x.Value,
                Payload = x.Payload,
                CreatedAt = x.CreatedAt,
                SentAt = x.SentAt
            })
            .ToListAsync(cancellationToken);

        return results;
    }
}

public class InstructionDto
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Payload { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? SentAt { get; set; }
}