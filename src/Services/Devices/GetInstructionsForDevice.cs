using Data;
using Microsoft.EntityFrameworkCore;

namespace Services.Devices;

public interface IGetInstructionsForDevice
{
    Task<IEnumerable<GetInstructionsForDeviceResponseDto>> ExecuteAsync(Guid deviceId, int take, CancellationToken cancellationToken);
}

public class GetInstructionsForDevice(AppDbContext dbContext) : IGetInstructionsForDevice
{
    public async Task<IEnumerable<GetInstructionsForDeviceResponseDto>> ExecuteAsync(Guid deviceId, int take, CancellationToken cancellationToken)
    {
        var results = await dbContext.Instructions
            .Where(x => x.DeviceId == deviceId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(take)
            .Select(x => new GetInstructionsForDeviceResponseDto
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