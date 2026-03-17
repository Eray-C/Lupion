using Lupion.Data;
using Lupion.Data.Entities;

namespace Lupion.Business.Services;

public class CompanyService(ManagementDBContext context)
{
    private Company? _cachedCompany;

    public Company GetCompany(int id)
    {
        var company = context.Companies.Find(id);

        return company;
    }
    public Company GetCompanyByCode(string code)
    {
        _cachedCompany ??= context.Companies.FirstOrDefault(x => x.Code == code);

        return _cachedCompany;
    }
}
