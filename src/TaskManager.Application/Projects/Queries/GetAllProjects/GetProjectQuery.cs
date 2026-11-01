using MediatR;

namespace TaskManager.Application.Projects.Queries.GetAllProjects;

public record GetAllProjectsQuery(int Limit, int Offset, Guid? UserId) : IRequest<GetProjectQueryResult>;
