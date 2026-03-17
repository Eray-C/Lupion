using Lupion.Business.DTOs.Shared;
using Lupion.Business.Requests.TaskManager;
using Lupion.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lupion.API.Controllers;

[ApiController]
[Route("api/task-manager")]
public class TaskManagerController(TaskManagerService taskManagerService, SharedService sharedService) : BaseController
{
    [HttpGet("tasks")]
    public async Task<IActionResult> GetTasksAsync(
        [FromQuery] bool archive = false,
        [FromQuery] string? search = null,
        [FromQuery] List<int>? statusIds = null,
        [FromQuery] List<int>? priorityIds = null)
    {
        var statuses = await sharedService.QueryTypesAsync("TaskType");
        var priorities = await sharedService.QueryTypesAsync("GeneralPriorityType");
        var doneStatusIds = TaskStatusHelper.ResolveDoneStatusIds(statuses);
        var tasks = await taskManagerService.GetTasksAsync(
            archive,
            doneStatusIds,
            search,
            statusIds,
            priorityIds,
            statuses,
            priorities);
        return Ok(tasks);
    }

    [HttpGet("tasks/{id:int}")]
    public async Task<IActionResult> GetTaskAsync(int id)
    {
        var task = await taskManagerService.GetTaskAsync(id);
        return Ok(task);
    }

    [HttpPost("tasks")]
    public async Task<IActionResult> CreateAsync([FromBody] TaskRequest request)
    {
        var id = await taskManagerService.CreateAsync(request);
        return Ok(id, "GÃ¶rev baÅŸarÄ±yla oluÅŸturuldu.");
    }

    [HttpGet("tasks/next-number")]
    public async Task<IActionResult> GetNextTaskNumberAsync()
    {
        var taskNumber = await taskManagerService.GetNextTaskNumberAsync();
        return Ok(taskNumber);
    }

    [HttpPut("tasks/{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] TaskRequest request)
    {
        await taskManagerService.UpdateAsync(id, request);
        return Ok("GÃ¶rev baÅŸarÄ±yla gÃ¼ncellendi.");
    }

    [HttpPost("tasks/{id:int}/status")]
    public async Task<IActionResult> UpdateStatusAsync(int id, [FromBody] TaskHistoryRequest request)
    {
        if (id != request.TaskId)
        {
            return BadRequest("Ä°stek yolu ile gÃ¶vde task kimlikleri eÅŸleÅŸmiyor.");
        }


        await taskManagerService.UpdateStatusAsync(request);
        return Ok("GÃ¶rev durumu gÃ¼ncellendi.");
    }

    [HttpDelete("tasks/{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await taskManagerService.DeleteAsync(id);
        return Ok("GÃ¶rev baÅŸarÄ±yla silindi.");
    }

    [HttpGet("lookups")]
    public async Task<IActionResult> GetLookupsAsync()
    {
        var statuses = await sharedService.QueryTypesAsync("TaskType");
        var priorities = await sharedService.QueryTypesAsync("GeneralPriorityType");

        var response = new TaskManagerLookupsDto(statuses, priorities);
        return Ok(response);
    }

    [HttpPost("tasks/{id:int}/archive")]
    public async Task<IActionResult> ArchiveTask(int id)
    {
        await taskManagerService.ArchiveAsync(id);
        return Ok();
    }

    [HttpPost("tasks/{id:int}/unarchive")]
    public async Task<IActionResult> UnarchiveTask(int id)
    {
        await taskManagerService.UnarchiveAsync(id);
        return Ok("GÃ¶rev arÅŸivden geri alÄ±ndÄ±.");
    }

}

public record TaskManagerLookupsDto(IEnumerable<GeneralTypeDTO> Statuses, IEnumerable<GeneralTypeDTO> Priorities);
