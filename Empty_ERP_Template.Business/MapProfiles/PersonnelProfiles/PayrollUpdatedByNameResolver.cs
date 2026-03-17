using AutoMapper;
using Empty_ERP_Template.Business.DTOs.Personnel;
using Empty_ERP_Template.Data.Entities.PersonnelEntities;

namespace Empty_ERP_Template.Business.MapProfiles.PersonnelProfiles;

public class PayrollUpdatedByNameResolver : IValueResolver<PaidPayroll, PayrollPeriodListDTO, string?>
{
    public string? Resolve(PaidPayroll source, PayrollPeriodListDTO destination, string? destMember, ResolutionContext context)
    {
        var userId = source.UpdatedBy ?? source.CreatedBy;
        if (!userId.HasValue) return null;
        if (context.Items.TryGetValue("UserNames", out var obj) && obj is Dictionary<int, string> userNames
            && userNames.TryGetValue(userId.Value, out var name))
            return name;
        return null;
    }
}
