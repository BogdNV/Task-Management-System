using MediatR;
using TaskManager.Application.Projects.DTO;

namespace TaskManager.Application.Projects.Queries.GetProjectById;

public record GetProjectByIdQuery(int Id) : IRequest<ProjectDto>;
