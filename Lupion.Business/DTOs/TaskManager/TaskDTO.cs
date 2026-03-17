namespace Lupion.Business.DTOs.TaskManager;

public class TaskDTO(
    int? id,
    string? taskNumber,
    string? description,
    DateTime taskDate,
    DateTime? targetDate,
    int statusId,
    int? priorityId,
    string? comment,
    string? statusName,
    string? priorityName)
{
    public TaskDTO() : this(null, null, null, default, null, default, null, null, null, null)
    {
    }

    public int? Id { get; set; } = id;

    public string? TaskNumber { get; set; } = taskNumber;

    public string? Description { get; set; } = description;

    public DateTime TaskDate { get; set; } = taskDate;

    public DateTime? TargetDate { get; set; } = targetDate;

    public int StatusId { get; set; } = statusId;

    public int? PriorityId { get; set; } = priorityId;

    public string? Comment { get; set; } = comment;

    public string? StatusName { get; set; } = statusName;

    public string? PriorityName { get; set; } = priorityName;

    public bool IsArchived { get; set; }
}
