using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Exceptions;
using TaskManager.Infrastructure.Context;

namespace TaskManager.Application.Tasks.Commands.UpdateTasks;

public class UpdateTaskStatusHandler : IRequestHandler<UpdateTaskCommand, UpdateTaskCommandResult>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateTaskStatusHandler(IMapper mapper, ApplicationDbContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<UpdateTaskCommandResult> Handle(UpdateTaskCommand request, CancellationToken ct)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.Id && t.ProjectEntity!.UserId == request.UserId, ct);

        if (task is null) throw new NotFoundException("Проект не найден");

        task.Description = request.Description ?? task.Description;
        task.Status = request.Status.HasValue ? request.Status.Value : task.Status;
        task.Priority = request.Priority.HasValue ? request.Priority.Value : task.Priority;
        task.Title = request.Title ?? task.Title;
        task.DueDate = request.DueDate ?? task.DueDate;

        await _context.SaveChangesAsync(ct);

        return _mapper.Map<UpdateTaskCommandResult>(task);
    }
}
