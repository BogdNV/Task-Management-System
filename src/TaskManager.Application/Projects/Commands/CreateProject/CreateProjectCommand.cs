using MediatR;
using TaskManager.Application.Projects.DTO;

namespace TaskManager.Application.Projects.Commands.CreateProject;

public record CreateProjectCommand(
    string Name,

    string Description,

    int OwnerId
) : IRequest<ProjectDto>;
