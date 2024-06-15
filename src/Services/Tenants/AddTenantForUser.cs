using Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Services.Users;

namespace Services.Tenants;

public interface IAddTenantForUser
{
    Task<(bool created, TenantDto? tenant)> ExecuteAsync(AddTenantForUserArgs args, UserDto user, CancellationToken cancellationToken);
}

public class AddTenantForUser(AppDbContext dbContext) : IAddTenantForUser
{
    public async Task<(bool created, TenantDto? tenant)> ExecuteAsync(AddTenantForUserArgs args, UserDto user, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<AddTenantForUser>();
        
        // Make sure the user isn't creating multiple developer tenants
        if (args.SubscriptionLevel == SubscriptionLevel.Developer &&
            user.Tenants.Any(x => x.SubscriptionLevel == SubscriptionLevel.Developer))
        {
            log.Warning("User {UserId} trying to create multiple developer tenants",  user.Id);    
            return (false, null);
        }
        
        var result = await dbContext.Tenants.AddAsync(new Tenant
        {
            Name = args.Name,
            SubscriptionLevel = args.SubscriptionLevel
        }, cancellationToken);
        
        var userEntity = await dbContext.Users
            .Include(x => x.Tenants)
            .SingleAsync(x => x.Id == user.Id, cancellationToken);
        userEntity.Tenants.Add(result.Entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        log.Information("Added tenant {TenantId} with name {Name} for user {UserId}", 
            result.Entity.Id, 
            args.Name,
            userEntity.Id);

        var tenant = TenantDto.FromEntity(result.Entity);

        return (true, tenant);
    }
}