using AutoMapper;
using TaskManager.API.Models.Request;
using TaskManager.API.Models.Response;
using TaskManager.Application.Tasks.Commands;

namespace TaskManager.API.Mappings.Task;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        CreateMap<CreateTaskRequest, CreateTaskCommand>();
        CreateMap<CreateTaskCommandResult, TaskResponse>();

        CreateMap<DeleteTaskCommandResult, DeleteTaskResponse>();

        CreateMap<UpdateTaskRequest, UpdateTaskCommand>();
        CreateMap<UpdateTaskCommandResult, TaskResponse>();
    }
}
