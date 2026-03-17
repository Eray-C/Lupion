using Empty_ERP_Template.Business.Services;
using Microsoft.AspNetCore.Http;

namespace Empty_ERP_Template.Business.Helpers;

public class TenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CompanyService _companyService;

    public string? Tenant { get; private set; }
    public string? ConnectionString { get; private set; }

    public TenantContext(IHttpContextAccessor httpContextAccessor, CompanyService companyService)
    {
        _httpContextAccessor = httpContextAccessor;
        _companyService = companyService;

        InitializeTenant();
    }

    /// <summary>
    /// HTTP Context'teki kullanýcýdan CompanyCode alýr ve
    /// buna göre tenant (þirket) bilgisini set eder.
    /// </summary>
    private void InitializeTenant()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var user = httpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
            return;

        // Kullanýcýnýn claim'lerinden "CompanyCode" deðerini al
        var companyCode = user.FindFirst("CompanyCode")?.Value;
        if (string.IsNullOrEmpty(companyCode))
            return;

        // Þirket bilgisini ManagementDB'den çek
        var company = _companyService.GetCompanyByCode(companyCode);
        if (company == null)
            return;

        Tenant = companyCode;
        ConnectionString = company.ConnectionString;
    }
}
