using AutoMapper;
using Lupion.Business.DTOs.TaskManager;
using Lupion.Business.Requests.TaskManager;
using Lupion.Data.Entities.TaskManagerEntities;

namespace Lupion.Business.MapProfiles.TaskManager;

public class TaskManagerProfile : Profile
{
    public TaskManagerProfile()
    {
        CreateMap<TaskRequest, TaskItem>();
        CreateMap<TaskItem, TaskDTO>();
        CreateMap<TaskHistoryRequest, TaskHistory>();
        CreateMap<TaskHistory, TaskHistoryDTO>();
    }
}
