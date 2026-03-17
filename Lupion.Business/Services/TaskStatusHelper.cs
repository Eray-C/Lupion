using Empty_ERP_Template.Business.DTOs.Shared;

namespace Empty_ERP_Template.Business.Services;

public static class TaskStatusHelper
{
    public static IReadOnlyCollection<int> ResolveDoneStatusIds(IEnumerable<GeneralTypeDTO> statuses)
    {
        return statuses
            .Where(status => string.Equals(status.Code, "done", StringComparison.OrdinalIgnoreCase)
                             || string.Equals(status.Name, "done", StringComparison.OrdinalIgnoreCase)
                             || string.Equals(status.Name, "tamamlandı", StringComparison.OrdinalIgnoreCase)
                             || string.Equals(status.Name, "bitti", StringComparison.OrdinalIgnoreCase))
            .Select(status => status.Id)
            .ToArray();
    }
}
