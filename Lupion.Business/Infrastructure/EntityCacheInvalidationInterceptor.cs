using Empty_ERP_Template.Business.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Empty_ERP_Template.Business.Infrastructure;

public class EntityCacheInvalidationInterceptor(RedisCacheService cache) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var changedEntities = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted)
            .Select(e => e.Entity.GetType().Name)
            .Distinct()
            .ToList();

        if (changedEntities.Count > 0)
        {
            var tasks = changedEntities
                .Select(entityName => cache.RemoveByEntityAsync(entityName))
                .ToArray();

            await Task.WhenAll(tasks);
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

}
