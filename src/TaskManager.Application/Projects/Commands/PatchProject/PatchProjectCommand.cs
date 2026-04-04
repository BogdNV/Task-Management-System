using MediatR;

namespace TaskManager.Application.Projects.Commands.PatchProject;

public record PatchProjectCommand(int Id, string? Name, string? Description) : IRequest;
