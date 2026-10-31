using AutoMapper;
using TaskManager.API.Models.Request;
using TaskManager.Application.Projects.Commands;

namespace TaskManager.API.Mappings.Project;

public class CreateProjectMappingProfile : Profile
{
    public CreateProjectMappingProfile()
    {
        CreateMap<CreateProjectRequest, CreateProjectCommand>();
    }
}
