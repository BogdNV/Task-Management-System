using MediatR;
using TaskManager.Application.Projects.DTO;

namespace TaskManager.Application.Projects.Queries.GetAllProjects;

public record GetAllProjectsQuery(int Limit, int Offset) : IRequest<IEnumerable<ProjectDto>>;
