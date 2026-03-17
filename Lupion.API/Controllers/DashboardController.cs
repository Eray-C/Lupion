using Empty_ERP_Template.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Empty_ERP_Template.API.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController(DashboardService dashboardService) : BaseController
{
    //RedisCache("DashboardSummary", nameof(Personnel), nameof(Shipment), nameof(Customer))]
    
}
