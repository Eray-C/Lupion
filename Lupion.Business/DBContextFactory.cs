using Lupion.Business.Services;
using Lupion.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lupion.Business;

public class DBContextFactory(IHttpContextAccessor httpContextAccessor, CompanyService companyService)
{
    public DBContext CreateDBContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DBContext>();

        var user = httpContextAccessor.HttpContext.User;

        if (user?.Identity?.IsAuthenticated == true)
        {
            var companyCode = user.FindFirst("CompanyCode").Value;

            var company = companyService.GetCompanyByCode(companyCode) ?? throw new Exception("Tenant bulunamadı");

            optionsBuilder.UseSqlServer(company.ConnectionString).LogTo(Console.WriteLine, LogLevel.Information);
        }

        return new DBContext(optionsBuilder.Options);
    }
}
