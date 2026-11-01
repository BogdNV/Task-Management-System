using AutoMapper;
using TaskManager.Application.Projects.Commands;
using TaskManager.Application.Projects.DTO;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Application.Mappings.Project;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<ProjectEntity, ProjectDto>()
            .ForMember(p =>
                p.Name,
                opt => opt.MapFrom(src => src.User!.Name));

        CreateMap<CreateProjectCommand, ProjectEntity>();
        CreateMap<ProjectEntity, CreateProjectCommandResult>();
        CreateMap<ProjectEntity, PatchProjectCommandResult>();
        CreateMap<ProjectEntity, PutProjectCommandResult>();
    }
}
