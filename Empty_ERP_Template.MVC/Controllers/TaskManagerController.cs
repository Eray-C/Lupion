using Empty_ERP_Template.Business.DTOs.TaskManager;
using Empty_ERP_Template.Business.Services;
using Empty_ERP_Template.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Empty_ERP_Template.MVC.Controllers;

[Route("task-manager")]
public class TaskManagerController(TaskManagerService taskManagerService, SharedService sharedService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var statuses = await sharedService.QueryTypesAsync("TaskType");
        var priorities = await sharedService.QueryTypesAsync("GeneralPriorityType");
        var doneStatusIds = TaskStatusHelper.ResolveDoneStatusIds(statuses);
        var tasks = await taskManagerService.GetTasksAsync(false, doneStatusIds, statuses: statuses, priorities: priorities);

        var model = new TaskManagerViewModel(tasks, statuses, priorities, doneStatusIds);

        return View("Index", model);
    }

    [HttpGet("board")]
    public async Task<IActionResult> Board(
        bool archive = false,
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

        var model = new TaskManagerViewModel(tasks, statuses, priorities, doneStatusIds);

        return PartialView("_Board", model);
    }



    [HttpGet("add-edit")]
    public async Task<IActionResult> frmAddEditTask(int? id)

    {
        var statuses = await sharedService.QueryTypesAsync("TaskType");
        var priorities = await sharedService.QueryTypesAsync("GeneralPriorityType");
        TaskDTO? task = id.HasValue ? await taskManagerService.GetTaskAsync(id.Value) : null;

        var model = new TaskFormViewModel(task, statuses, priorities);
        return View("frmAddEditTask", model);

    }

}
