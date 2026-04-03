using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Mappers;
using TaskManager.Application.Projects.Commands.CreateProject;
using TaskManager.Application.Projects.Commands.UpdateProject;
using TaskManager.Application.Projects.DTO;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Mappers;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectRepository _repository;
    public ProjectsController(IProjectRepository repository)
    {
        _repository = repository;

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAll(CancellationToken ct)
    {
        var projects = await _repository.GetAllAsync();
        var dtos = projects.Select(p => p.ToDto());
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectDto>> GetById(int id, CancellationToken ct)
    {
        if (id <= 0) return BadRequest("Неверный ID проекта");
        var result = await _repository.GetByIdAsync(id);

        return result is null ? NotFound() : Ok(result.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectCommand dto, CancellationToken ct)
    {

        var project = await _repository.AddAsync(dto.ToDomain());
        var resultDto = project.ToDto();

        return CreatedAtAction(nameof(GetById), new { id = project.Id }, resultDto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProjectCommand dto, CancellationToken ct)
    {
        if (id <= 0) return BadRequest("Неверный ID проекта");

        var project = await _repository.GetByIdAsync(id);
        if (project is null) return NotFound();

        // Вызов доменных методов обновления
        project.UpdateName(dto.Name);
        project.UpdateDescription(dto.Description);

        await _repository.UpdateAsync(project);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, PatchProjectCommand patch, CancellationToken ct)
    {
        if (id <= 0) return BadRequest("Неверный ID проекта");

        var project = await _repository.GetByIdAsync(id);
        if (project is null) return NotFound();

        if (string.IsNullOrEmpty(patch.Name)
            && string.IsNullOrEmpty(patch.Description))
            return NoContent();

        if (!string.IsNullOrEmpty(patch.Name)) project.UpdateName(patch.Name);
        if (!string.IsNullOrEmpty(patch.Description)) project.UpdateDescription(patch.Description);

        await _repository.UpdateAsync(project);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        if (id <= 0) return BadRequest("Неверный ID проекта");

        await _repository.DeleteAsync(id);

        return NoContent();
    }
}
