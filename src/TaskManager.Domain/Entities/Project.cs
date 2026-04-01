namespace TaskManager.Domain.Entities;

public class Project
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public int OwnerId { get; private set; }

    private readonly List<TaskItem> _tasks = new();
    public IReadOnlyCollection<TaskItem> Tasks => _tasks;

    public Project(string name, string description, int ownerId)
    {
        ValidateName(name);
        Name = name;
        Description = description;
        OwnerId = ownerId;
        CreatedAt = DateTime.UtcNow;

    }

    public void AddTask(TaskItem task)
    {
        if (task is null)
            throw new ArgumentNullException(nameof(task), "Задача не может быть null");
        _tasks.Add(task);
    }

    public void UpdateName(string newName)
    {
        ValidateName(newName);
        Name = newName;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Название не должно быть пустым", nameof(name));
        if (name.Length < 5)
            throw new ArgumentException("Название должно быть длиной мнимум 5 символов", nameof(name));
    }
}
