using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(int id);
    Task<IEnumerable<Project>> GetAllAsync();
    Task AddAsync(Project project);
    Task UpdateAsync(Project project);
    Task DeleteAsync(int id);
}
