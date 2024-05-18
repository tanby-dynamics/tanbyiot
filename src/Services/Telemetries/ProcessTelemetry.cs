using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Newtonsoft.Json;

namespace Services.Telemetries;

public interface IProcessTelemetry
{
    Task ExecuteAsync(Guid telemetryId, CancellationToken cancellationToken);
}

public class ProcessTelemetry(AppDbContext dbContext, ISystemClock clock) : IProcessTelemetry
{
    public async Task ExecuteAsync(Guid telemetryId, CancellationToken cancellationToken)
    {
        var telemetry = await dbContext.Telemetries.SingleAsync(x => x.Id == telemetryId, cancellationToken);

        await dbContext.Instructions.AddAsync(new Instruction
        {
            DeviceId = telemetry.DeviceId,
            Type = "set-temperature",
            Value = "25",
            Payload = JsonConvert.SerializeObject(new
            {
                Temperature = 25
            }),
            CreatedAt = clock.UtcNow
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}