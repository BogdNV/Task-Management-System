using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Abstractions.Inrefaces;
using TaskManager.API.Models.Request;
using TaskManager.API.Models.Response;
using TaskManager.API.Models.Response.Project;
using TaskManager.Application.Projects.Commands;
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
    [HttpGet("{userId:Guid}")]
    public async Task<ActionResult<ApiResponse<GetPagidatedProjectResponse>>> GetUserProjects(
        [FromRoute] Guid userId,
        CancellationToken ct,
        [FromQuery] int offset = 1,
        [FromQuery] int limit = 5)
    {
        if (offset <= 0 || limit <= 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        var common = new GetAllProjectsQuery(limit, offset, userId);

        var result = await _mediator.Send(common, ct);
        return new ApiResponse<GetPagidatedProjectResponse>
        {
            Data = _mapper.Map<GetPagidatedProjectResponse>(result),
            Message = "Success"
        };
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<GetPagidatedProjectResponse>>> GetPaginatedProjects(
        CancellationToken ct,
        [FromQuery] int offset = 1,
        [FromQuery] int limit = 5)
    {
        if (offset <= 0 || limit <= 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        var results = await _mediator.Send(new GetAllProjectsQuery(limit, offset, null), ct);

        return new ApiResponse<GetPagidatedProjectResponse>
        {
            Data = _mapper.Map<GetPagidatedProjectResponse>(results),
            Message = "Success"
        };
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
    public async Task<ActionResult<ApiResponse<PutProjectResponse>>> Put([FromRoute] int id, UpdateProjectRequest request, CancellationToken ct)
    {
        if (id <= 0) return BadRequest("Неверный ID проекта");
        var userId = _idService.Id;
        request.UserId = userId;
        request.Id = id;
        var result = await _mediator.Send(_mapper.Map<PutProjectCommand>(request), ct);
        return new ApiResponse<PutProjectResponse>
        {
            Data = _mapper.Map<PutProjectResponse>(result),
            Message = "Success"
        };
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<ApiResponse<PatchProjectResponse>>> Patch([FromRoute] int id, UpdateProjectRequest request, CancellationToken ct)
    {
        if (id <= 0) return BadRequest("Неверный ID проекта");
        request.Id = id;
        request.UserId = _idService.Id;

        var result = await _mediator.Send(_mapper.Map<PatchProjectCommand>(request), ct);
        return new ApiResponse<PatchProjectResponse>
        {
            Data = _mapper.Map<PatchProjectResponse>(result),
            Message = "Success"
        };
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken ct)
    {
        if (id <= 0) return BadRequest("Неверный ID проекта");

        var idResult = await _mediator.Send(new DeleteProjectCommand { Id = id, UserId = _idService.Id }, ct);

        return Ok(new ApiResponse<DeleteProjectResponse>
        {
            Data = _mapper.Map<DeleteProjectResponse>(idResult),
            Message = "Success"
        });
    }
}
