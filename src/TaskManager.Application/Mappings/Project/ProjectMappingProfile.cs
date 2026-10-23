using AutoMapper;
using TaskManager.Application.Projects.DTO;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Application.Mappings.Project;

public partial class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<ProjectEntity, ProjectDto>();
    }
}
