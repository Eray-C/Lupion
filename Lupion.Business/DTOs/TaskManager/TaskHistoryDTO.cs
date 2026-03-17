namespace Lupion.Business.DTOs.TaskManager;

public class TaskHistoryDTO
{
    public int TaskId { get; set; }
    public int? OldStatusId { get; set; }
    public int NewStatusId { get; set; }
    public string? Comment { get; set; }
}
