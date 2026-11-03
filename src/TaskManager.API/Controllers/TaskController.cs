using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Abstractions.Inrefaces;
using TaskManager.API.Models.Request;
using TaskManager.API.Models.Response;
using TaskManager.Application.Tasks.Commands;
using TaskManager.Application.Tasks.Queries;
using TaskManager.Domain.Enums;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IGetUserIdService _idService;

    public TaskController(IMediator mediator, IMapper mapper, IGetUserIdService idService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _idService = idService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<TaskResponse>>> Create(CreateTaskRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<CreateTaskCommand>(request);
        command.UserId = _idService.Id;

        var commandResult = await _mediator.Send(command, ct);

        return new ApiResponse<TaskResponse>
        {
            Data = _mapper.Map<TaskResponse>(commandResult),
            Message = "Success"
        };
    }

    [HttpPatch("{id:int}")]
    public async Task<ApiResponse<TaskResponse>> Update([FromRoute] int id, UpdateTaskRequest request, CancellationToken ct)
    {
        var command = _mapper.Map<UpdateTaskCommand>(request);
        command.Id = id;
        command.UserId = _idService.Id;
        var result = await _mediator.Send(command, ct);
        return new ApiResponse<TaskResponse>
        {
            Data = _mapper.Map<TaskResponse>(result),
            Message = "Succes"
        };
    }

    [HttpGet]
    public async Task<ApiResponse<GetTaskQueryResult>> GetTasks(
        [FromQuery] Status? status,
        [FromQuery] Priority? priority,
        CancellationToken ct)
    {
        var query = new GetTaskQuery
        {
            UserId = _idService.Id,
            Status = status,
            Priority = priority
        };

        var result = await _mediator.Send(query, ct);
        return new ApiResponse<GetTaskQueryResult>
        {
            Data = result,
            Message = "Success"
        };
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<DeleteTaskResponse>>> Delete([FromRoute] int id, CancellationToken ct)
    {
        var command = new DeleteTaskCommand
        {
            Id = id,
            UserId = _idService.Id
        };

        var commandResult = await _mediator.Send(command, ct);

        return new ApiResponse<DeleteTaskResponse>
        {
            Data = _mapper.Map<DeleteTaskResponse>(commandResult),
            Message = "Success"
        };
    }

    [HttpDelete("completed")]
    public async Task<ApiResponse<ClearCompletedTasksCommandResult>> ClearCompletedTasks(CancellationToken ct)
    {
        var command = new ClearCompletedTasksCommand { UserId = _idService.Id };
        var result = await _mediator.Send(command, ct);

        return new ApiResponse<ClearCompletedTasksCommandResult>
        {
            Data = result,
            Message = "Completed tasks cleared successfully"
        };
    }
}
