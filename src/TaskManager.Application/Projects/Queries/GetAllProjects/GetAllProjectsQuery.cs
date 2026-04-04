using MediatR;
using TaskManager.Application.Projects.DTO;

namespace TaskManager.Application.Projects.Queries.GetAllProjects;

public record GetAllProjectsQuery : IRequest<IEnumerable<ProjectDto>>;
