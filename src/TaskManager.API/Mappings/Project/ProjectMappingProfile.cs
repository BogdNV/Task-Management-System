using AutoMapper;
using TaskManager.API.Models.Request;
using TaskManager.Application.Projects.Commands.CreateProject;

namespace TaskManager.API.Mappings.Project;

public partial class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<CreateProjectRequest, CreateProjectCommand>();
    }
}
