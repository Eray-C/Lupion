using Lupion.Business.DTOs.Shared;
using Lupion.Business.DTOs.TaskManager;

namespace Lupion.MVC.Models;

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
