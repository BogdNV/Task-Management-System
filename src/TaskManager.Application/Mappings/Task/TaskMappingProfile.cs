using AutoMapper;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Application.Tasks.DTO;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Application.Mappings.Task;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        CreateMap<CreateTaskCommand, TaskItemEntity>();
        CreateMap<TaskItemEntity, CreateTaskCommandResult>();
        CreateMap<TaskItemEntity, TaskDto>();
        CreateMap<TaskItemEntity, UpdateTaskCommandResult>();
    }
}
