using Empty_ERP_Template.Business.DTOs.Shared;
using Empty_ERP_Template.Business.DTOs.TaskManager;

namespace Empty_ERP_Template.MVC.Models;

public record TaskManagerViewModel(
    IEnumerable<TaskDTO> Tasks,
    IEnumerable<GeneralTypeDTO> Statuses,
    IEnumerable<GeneralTypeDTO> Priorities,
    IReadOnlyCollection<int> DoneStatusIds
);

public record TaskFormViewModel(
    TaskDTO? Task,
    IEnumerable<GeneralTypeDTO> Statuses,
    IEnumerable<GeneralTypeDTO> Priorities
);
