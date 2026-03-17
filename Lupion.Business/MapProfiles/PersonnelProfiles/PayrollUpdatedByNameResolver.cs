using AutoMapper;
using Lupion.Business.DTOs.Personnel;
using Lupion.Data.Entities.PersonnelEntities;

namespace Lupion.Business.MapProfiles.PersonnelProfiles;

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
