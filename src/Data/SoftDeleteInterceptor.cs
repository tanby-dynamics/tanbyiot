using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Data;

public class SoftDeleteInterceptor : SaveChangesInterceptor
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
                    State: EntityState.Deleted,
                    Entity: ISoftDelete softDeletable
                })
            {
                continue;
            }

            entry.State = EntityState.Modified;
            softDeletable.DeletedAt = DateTimeOffset.Now;
        }

        return result;
    }
}