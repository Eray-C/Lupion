using AutoMapper;
using Empty_ERP_Template.Business.DTOs.TaskManager;
using Empty_ERP_Template.Business.Requests.TaskManager;
using Empty_ERP_Template.Data.Entities.TaskManagerEntities;

namespace Empty_ERP_Template.Business.MapProfiles.TaskManager;

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
