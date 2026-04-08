namespace TaskManager.Domain.Entities;

public class Project
{
    public const int MAX_NAME_LENGTH = 100;
    public const int MIN_NAME_LENGTH = 5;
    public const int MAX_DESCRIPTION_LENGTH = 1000;
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
        ValidateDescription(description);
        ValidateOwnerId(ownerId);
        Name = name;
        Description = description;
        OwnerId = ownerId;
        CreatedAt = DateTime.UtcNow;

    }

    public Project(int id, string name, string description, int ownerId, DateTime createdAt)
    {
        ValidateName(name);
        ValidateDescription(description);
        ValidateOwnerId(ownerId);
        Id = id;
        Name = name;
        Description = description;
        OwnerId = ownerId;
        CreatedAt = createdAt;
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

    public void UpdateDescription(string newDescription)
    {
        ValidateDescription(newDescription);
        Description = newDescription;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Название не должно быть пустым", nameof(name));
        if (name.Length < MIN_NAME_LENGTH)
            throw new ArgumentException($"Название должно быть длиной минимум {MIN_NAME_LENGTH} символов", nameof(name));
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
            throw new ArgumentException("Описание не должно быть пустым", nameof(description));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Описание состоит из пробелов", nameof(description));
        if (description.Length > MAX_DESCRIPTION_LENGTH)
            throw new ArgumentException($"Превышен порог в {MAX_DESCRIPTION_LENGTH} символов", nameof(description));
    }

    private static void ValidateOwnerId(int id)
    {
        if (id < 1)
            throw new ArgumentException("Неверный ID владельца", nameof(id));
    }
}
