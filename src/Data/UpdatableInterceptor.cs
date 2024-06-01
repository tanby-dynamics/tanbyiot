using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Data;

public class UpdatableInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        if (eventData.Context is null)
        {
            return result;
        }

        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            if (entry is not
                {
                    Entity: IUpdatable updatable
                })
            {
                continue;
            }

            updatable.UpdatedAt = DateTimeOffset.Now;
        }

        return result;
    }
}