using Lupion.Data;
using Lupion.Data.Entities.SharedEntities;
using Microsoft.EntityFrameworkCore;

namespace Lupion.Business.Services;

public class MenuService(DBContext dbContext, CacheService cacheService)
{
    public async Task<IEnumerable<Menu>> GetMenusAsync()
    {
        return await dbContext.Menus.Where(x => x.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Menu>> GetMenusFromCacheAsync()
    {
        return await cacheService.GetOrAddAsync("Menu", async _ =>
        {
            return await GetMenusAsync();
        });
    }
}
