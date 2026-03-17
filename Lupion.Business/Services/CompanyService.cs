using Empty_ERP_Template.Data;
using Empty_ERP_Template.Data.Entities;

namespace Empty_ERP_Template.Business.Services;

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
