using MediatR;
using TaskManager.Application.Projects.DTO;

namespace TaskManager.Application.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(int Id, string Name, string Description) : IRequest;