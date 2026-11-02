using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Exceptions;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Entities;

namespace TaskManager.Application.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, CreateTaskCommandResult>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateTaskCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreateTaskCommandResult> Handle(CreateTaskCommand command, CancellationToken ct)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == command.ProjectId);
        if (project is null) throw new NotFoundException("Проект не найден");
        if (project.UserId != command.UserId) throw new ForbiddenException("нет прав");

        var task = _mapper.Map<TaskItemEntity>(command);

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync(ct);

        return _mapper.Map<CreateTaskCommandResult>(task);
    }
}
