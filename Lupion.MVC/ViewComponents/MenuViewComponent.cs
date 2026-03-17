using Lupion.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lupion.MVC.ViewComponents;

public class MenuViewComponent(MenuService service) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var menus = await service.GetMenusFromCacheAsync();

        return View(menus);
    }
}
