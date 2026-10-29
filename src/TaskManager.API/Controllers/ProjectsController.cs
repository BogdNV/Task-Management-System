using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Abstractions.Inrefaces;
using TaskManager.API.Models.Request;
using TaskManager.API.Models.Response;
using TaskManager.Application.Projects.Commands;
using TaskManager.Application.Projects.DTO;
using TaskManager.Application.Projects.Queries.GetAllProjects;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IGetUserIdService _idService;
    public ProjectsController(IMediator mediator, IMapper mapper, IGetUserIdService idService)
    {
        _mediator = mediator;
        _mapper = mapper;
        _idService = idService;
    }

    [AllowAnonymous]
    [HttpGet("user/{userId:Guid}")]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetUserProjects(
        [FromRoute] Guid userId,
        CancellationToken ct,
        [FromQuery] int offset = 1,
        [FromQuery] int limit = 5)
    {
        if (offset <= 0 || limit <= 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        var common = new PaginatedProjectCommand
        {
            Offset = offset,
            Limit = limit,
            UserId = userId
        };

        var result = await _mediator.Send(common, ct);
        return Ok(new ApiResponse<IList<ProjectDto>>
        {
            Data = result.Projects,
            Message = "Done"
        });
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetPaginatedProjects(
        CancellationToken ct,
        [FromQuery] int offset = 1,
        [FromQuery] int limit = 5)
    {
        if (offset <= 0 || limit <= 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        var results = await _mediator.Send(new GetAllProjectsQuery(limit, offset), ct);

        return Ok(new ApiResponse<IEnumerable<ProjectDto>>
        {
            Data = results,
            Message = "Done"
        });
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectRequest request, CancellationToken ct)
    {
        Guid userId = _idService.Id;

        var command = _mapper.Map<CreateProjectCommand>(request);
        command.UserId = userId;
        var result = await _mediator.Send(command, ct);

        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put([FromRoute] int id, PutProjectCommand command, CancellationToken ct)
    {
        if (id <= 0) return BadRequest("Неверный ID проекта");
        var userId = _idService.Id;
        command.UserId = userId;
        command.Id = id;
        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch([FromRoute] int id, PatchProjectCommand command, CancellationToken ct)
    {
        if (id <= 0) return BadRequest("Неверный ID проекта");
        command.Id = id;
        command.UserId = _idService.Id;

        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        if (id <= 0) return BadRequest("Неверный ID проекта");

        await _mediator.Send(new DeleteProjectCommand { Id = id, UserId = _idService.Id }, ct);

        return NoContent();
    }
}
