using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Context;
using TaskManager.Infrastructure.Entities;
using TaskManager.Infrastructure.Mappers;

namespace TaskManager.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Project project)
    {

        await _context.Projects.AddAsync(project.ToEntity());
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await _context.Projects
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        var entities = await _context.Projects
                            .AsNoTracking()
                            .ToListAsync();
        return entities.Select(e => e.ToDomain());
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        var entity = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);

        return entity?.ToDomain();
    }

    public async Task UpdateAsync(Project project)
    {
        await _context.Projects
                    .Where(p => p.Id == project.Id)
                    .ExecuteUpdateAsync(s => s
                            .SetProperty(x => x.Name, project.Name)
                            .SetProperty(x => x.Description, project.Description));
    }
}
