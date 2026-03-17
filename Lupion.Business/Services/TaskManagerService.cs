using AutoMapper;
using Lupion.Business.DTOs.Shared;
using Lupion.Business.DTOs.TaskManager;
using Lupion.Business.Exceptions;
using Lupion.Business.Requests.TaskManager;
using Lupion.Data;
using Lupion.Data.Entities.TaskManagerEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Lupion.Business.Services;

public class TaskManagerService(DBContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    : BaseService(httpContextAccessor)
{
    public async Task<IEnumerable<TaskDTO>> GetTasksAsync(
        bool archiveMode = false,
        IEnumerable<int>? doneStatusIds = null,
        string? search = null,
        IEnumerable<int>? statusIds = null,
        IEnumerable<int>? priorityIds = null,
        IEnumerable<GeneralTypeDTO>? statuses = null,
        IEnumerable<GeneralTypeDTO>? priorities = null)
    {
        var statusList = doneStatusIds?.ToList();
        var query = context.Tasks.AsNoTracking().Where(t => !t.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(t =>
                EF.Functions.Like(t.Description ?? string.Empty, $"%{term}%") ||
                EF.Functions.Like(t.TaskNumber ?? string.Empty, $"%{term}%"));
        }

        if (statusIds?.Any() == true)
        {
            var statusFilter = statusIds.ToList();
            query = query.Where(t => statusFilter.Contains(t.StatusId));
        }

        if (priorityIds?.Any() == true)
        {
            var priorityFilter = priorityIds.ToList();
            query = query.Where(t => t.PriorityId.HasValue && priorityFilter.Contains(t.PriorityId.Value));
        }

        if (archiveMode)
            query = query.Where(t => t.IsArchived);
        else
            query = query.Where(t => !t.IsArchived);

        var entities = await query.OrderByDescending(t => t.TaskDate).ToListAsync();
        var (statusLookup, priorityLookup) = await LoadLookupsAsync(statuses, priorities);

        var mappedTasks = mapper.Map<List<TaskDTO>>(entities);

        foreach (var task in mappedTasks)
        {
            if (statusLookup.TryGetValue(task.StatusId, out var statusName))
            {
                task.StatusName = statusName;
            }

            if (task.PriorityId.HasValue &&
                priorityLookup.TryGetValue(task.PriorityId.Value, out var priorityName))
            {
                task.PriorityName = priorityName;
            }
        }

        return mappedTasks;

    }

    public async Task<TaskDTO> GetTaskAsync(int id)
    {
        var entity = await context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted)
                     ?? throw new RecordNotFoundException();

        return mapper.Map<TaskDTO>(entity);
    }

    public async Task<int> CreateAsync(TaskRequest request)
    {
        var entity = mapper.Map<TaskItem>(request);
        entity.TaskNumber = string.IsNullOrWhiteSpace(request.TaskNumber)
            ? await GenerateTaskNumberAsync()
            : request.TaskNumber;
        entity.CreatedBy = CurrentUser.Id;
        entity.CreatedDate = DateTime.UtcNow;

        await context.Tasks.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<string> GetNextTaskNumberAsync() => await GenerateTaskNumberAsync();

    public async Task UpdateAsync(int id, TaskRequest request)
    {
        var entity = await context.Tasks.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted)
                     ?? throw new RecordNotFoundException();

        mapper.Map(request, entity);
        entity.UpdatedBy = CurrentUser.Id;
        entity.UpdatedDate = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(TaskHistoryRequest request)
    {
        var entity = await context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId && !t.IsDeleted)
                     ?? throw new RecordNotFoundException();

        if (entity.StatusId == request.NewStatusId)
        {
            return;
        }

        var history = mapper.Map<TaskHistory>(request);
        history.TaskId = entity.Id;
        history.OldStatusId = entity.StatusId;
        history.ChangedBy = CurrentUser.Id;
        history.ChangedDate = DateTime.UtcNow;

        entity.StatusId = request.NewStatusId;
        entity.UpdatedBy = CurrentUser.Id;
        entity.UpdatedDate = DateTime.UtcNow;

        await context.TaskHistories.AddAsync(history);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await context.Tasks.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted)
                     ?? throw new RecordNotFoundException();

        entity.IsDeleted = true;
        entity.UpdatedBy = CurrentUser.Id;
        entity.UpdatedDate = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task ArchiveAsync(int id)
    {
        var entity = await context.Tasks.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted)
                     ?? throw new RecordNotFoundException();

        entity.IsArchived = true;
        entity.UpdatedBy = CurrentUser.Id;
        entity.UpdatedDate = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task UnarchiveAsync(int id)
    {
        var entity = await context.Tasks.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted)
                     ?? throw new RecordNotFoundException();

        entity.IsArchived = false;
        entity.UpdatedBy = CurrentUser.Id;
        entity.UpdatedDate = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    private async Task<string> GenerateTaskNumberAsync()
    {
        var year = DateTime.UtcNow.Year % 100;
        var nextSequence = await context.Tasks
            .CountAsync(t => !t.IsDeleted && t.TaskDate.Year == DateTime.UtcNow.Year) + 1;

        return $"IS-{nextSequence:D4}-{year:D2}";
    }


    private async Task<(Dictionary<int, string> Statuses, Dictionary<int, string> Priorities)> LoadLookupsAsync(
        IEnumerable<GeneralTypeDTO>? statuses,
        IEnumerable<GeneralTypeDTO>? priorities)
    {
        var statusTask = statuses is not null
            ? Task.FromResult(statuses.ToDictionary(s => s.Id, s => s.Name ?? string.Empty))
            : context.GeneralTypes
                .Where(gt => gt.Category == "TaskType")
                .ToDictionaryAsync(gt => gt.Id, gt => gt.Name ?? string.Empty);

        var priorityTask = priorities is not null
            ? Task.FromResult(priorities.ToDictionary(p => p.Id, p => p.Name ?? string.Empty))
            : context.GeneralTypes
                .Where(gt => gt.Category == "GeneralPriorityType")
                .ToDictionaryAsync(gt => gt.Id, gt => gt.Name ?? string.Empty);

        await Task.WhenAll(statusTask, priorityTask);

        return (await statusTask, await priorityTask);
    }

    //private async Task<(Dictionary<int, string> Statuses, Dictionary<int, string> Priorities)> LoadLookupsAsync(
    //    IEnumerable<GeneralTypeDTO>? statuses,
    //    IEnumerable<GeneralTypeDTO>? priorities)
    //{
    //    var statusTask = statuses is not null
    //        ? Task.FromResult(statuses.ToDictionary(s => s.Id, s => s.Name ?? string.Empty))
    //        : context.GeneralTypes
    //            .Where(gt => gt.Category == "TaskType")
    //            .ToDictionaryAsync(gt => gt.Id, gt => gt.Name ?? string.Empty);

    //    var priorityTask = priorities is not null
    //        ? Task.FromResult(priorities.ToDictionary(p => p.Id, p => p.Name ?? string.Empty))
    //        : context.GeneralTypes
    //            .Where(gt => gt.Category == "GeneralPriorityType")
    //            .ToDictionaryAsync(gt => gt.Id, gt => gt.Name ?? string.Empty);

    //    await Task.WhenAll(statusTask, priorityTask);

    //    return (await statusTask, await priorityTask);
    //}
}
