namespace Lupion.Business.Requests.TaskManager;

public class TaskHistoryRequest
{
    public int TaskId { get; set; }
    public int? OldStatusId { get; set; }
    public int NewStatusId { get; set; }
    public string? Comment { get; set; }
}
