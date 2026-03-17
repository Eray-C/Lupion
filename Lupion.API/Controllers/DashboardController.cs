using Lupion.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lupion.API.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController(DashboardService dashboardService) : BaseController
{
    //RedisCache("DashboardSummary", nameof(Personnel), nameof(Shipment), nameof(Customer))]
    
}
