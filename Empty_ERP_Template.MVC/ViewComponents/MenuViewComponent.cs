using Empty_ERP_Template.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Empty_ERP_Template.MVC.ViewComponents;

public class MenuViewComponent(MenuService service) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var menus = await service.GetMenusFromCacheAsync();

        return View(menus);
    }
}
