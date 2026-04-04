using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Projects.Commands.CreateProject;
using TaskManager.Application.Projects.Commands.DeleteProject;
using TaskManager.Application.Projects.Commands.PatchProject;
using TaskManager.Application.Projects.Commands.UpdateProject;
using TaskManager.Application.Projects.DTO;
using TaskManager.Application.Projects.Queries.GetAllProjects;
using TaskManager.Application.Projects.Queries.GetProjectById;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll(CancellationToken ct)
    {
        var results = await _mediator.Send(new GetAllProjectsQuery(), ct);
        return Ok(results);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectDto>> GetById(int id, CancellationToken ct)
    {
        if (id <= 0) return BadRequest("Неверный ID проекта");
        var result = await _mediator.Send(new GetProjectByIdQuery(id), ct);

        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectCommand command, CancellationToken ct)
    {

        var result = await _mediator.Send(command, ct);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProjectCommand command, CancellationToken ct)
    {
        if (id <= 0) return BadRequest("Неверный ID проекта");
        if (id != command.Id) return BadRequest("ID не совпадает");

        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchProjectCommand command, CancellationToken ct)
    {
        if (id <= 0) return BadRequest("Неверный ID проекта");
        if (id != command.Id) return BadRequest("ID не совпадает");

        await _mediator.Send(command, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        if (id <= 0) return BadRequest("Неверный ID проекта");

        await _mediator.Send(new DeleteProjectCommand(id), ct);

        return NoContent();
    }
}
