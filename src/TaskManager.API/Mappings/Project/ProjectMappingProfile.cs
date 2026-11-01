using AutoMapper;
using TaskManager.API.Models.Request;
using TaskManager.API.Models.Response;
using TaskManager.API.Models.Response.Project;
using TaskManager.Application.Projects.Commands;
using TaskManager.Application.Projects.DTO;
using TaskManager.Application.Projects.Queries.GetAllProjects;

namespace TaskManager.API.Mappings.Project;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<CreateProjectRequest, CreateProjectCommand>();
        CreateMap<ProjectDto, ProjectResponse>();

        CreateMap<UpdateProjectRequest, PatchProjectCommand>();
        CreateMap<UpdateProjectRequest, PutProjectCommand>();
        CreateMap<DeleteProjectCommandResult, DeleteProjectResponse>();
        CreateMap<GetProjectQueryResult, GetPagidatedProjectResponse>();
        CreateMap<PutProjectCommandResult, PutProjectResponse>();
        CreateMap<PatchProjectCommandResult, PatchProjectResponse>();
    }
}
