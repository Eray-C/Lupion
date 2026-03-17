using Empty_ERP_Template.Business.DTOs.Shared;
using Empty_ERP_Template.Data;
using Microsoft.AspNetCore.Mvc;

namespace Empty_ERP_Template.MVC.ViewComponents;

public class LocationSelectorViewComponent : ViewComponent
{
    private readonly DBContext _context;

    public LocationSelectorViewComponent(DBContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke(
        string prefix,
        int? cityId = null,
        int? townId = null,
        string cityLabel = "",
        string townLabel = ""
    )
    {
        var cities = _context.GeneralTypes
                .Where(x => x.Category == "City")
                .OrderBy(x => x.Name)
                .Select(x => new GeneralTypeDTO
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

        ViewBag.Prefix = prefix;
        ViewBag.SelectedCity = cityId;
        ViewBag.SelectedTown = townId;
        ViewBag.CityLabel = cityLabel;
        ViewBag.TownLabel = townLabel;

        return View("Default", cities);
    }
}
