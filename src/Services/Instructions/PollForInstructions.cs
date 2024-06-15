using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;

namespace Services.Instructions;

public interface IPollForInstructions
{
    Task<IEnumerable<PollForInstructionsResponseDto>> ExecuteAsync(
        Guid tenantId,
        Guid deviceId, 
        CancellationToken cancellationToken);
}

public class PollForInstructions(AppDbContext dbContext, ISystemClock clock) : IPollForInstructions
{
    public async Task<IEnumerable<PollForInstructionsResponseDto>> ExecuteAsync(
        Guid tenantId,
        Guid deviceId, 
        CancellationToken cancellationToken)
    {
        var instructions = await dbContext.Instructions
            .Where(x => x.Device.TenantId == tenantId && x.Device.Id == deviceId && x.SentAt == null)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        foreach (var instruction in instructions)
        {
            instruction.SentAt = clock.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        var result = instructions.Select(instruction => new PollForInstructionsResponseDto
        {
            Id = instruction.Id,
            Type = instruction.Type,
            Value = instruction.Value,
            Payload = instruction.Payload,
            CreatedAt = instruction.CreatedAt,
        });

        return result;
    }
}