using Data;
using Microsoft.Extensions.Internal;
using Serilog;

namespace Services.Rules;

public interface IAddRule
{
    Task<RuleDto> ExecuteAsync(string name, CancellationToken cancellationToken);
}

public class AddRule(AppDbContext dbContext, ISystemClock clock) : IAddRule
{
    public async Task<RuleDto> ExecuteAsync(string name, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<AddRule>();
        var result = await dbContext.Rules.AddAsync(new Rule
        {
            Name = name,
            CreatedAt = clock.UtcNow,
            Enabled = true
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        log.Information("Added rule {RuleId}", result.Entity.Id);

        return RuleDto.FromEntity(result.Entity);
    }
}