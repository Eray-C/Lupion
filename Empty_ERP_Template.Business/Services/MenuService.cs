using Empty_ERP_Template.Data;
using Empty_ERP_Template.Data.Entities.SharedEntities;
using Microsoft.EntityFrameworkCore;

namespace Empty_ERP_Template.Business.Services;

public class MenuService(DBContext dbContext, CacheService cacheService)
{
    public async Task<IEnumerable<Menu>> GetMenusAsync()
    {
        return await dbContext.Menus.Where(x => x.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Menu>> GetMenusForNavigationAsync()
    {
        return await dbContext.Menus
            .ToListAsync();
    }

    public async Task<IEnumerable<Menu>> GetMenusFromCacheAsync()
    {
        return await cacheService.GetOrAddAsync("Menu", async _ =>
        {
            return await GetMenusForNavigationAsync();
        });
    }
}
