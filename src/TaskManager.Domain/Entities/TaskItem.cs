using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

public class TaskItem
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Status Status { get; private set; } = Status.New;
    public Priority Priority { get; private set; } = Priority.Medium;
    public DateTime? DueDate { get; private set; }
    public int ProjectId { get; private set; }

    public TaskItem(string title, string description, int projectId, DateTime? dueDate)
    {
        ValidateTitle(title);
        ValidateDueDate(dueDate);
        ValidateDescription(description);
        Title = title;
        Description = description;
        DueDate = dueDate;
        ProjectId = projectId;
    }

    public TaskItem(int id, string title, string description, Status status, Priority priority, int projectId, DateTime? dueDate)
    {
        ValidateTitle(title);
        ValidateDueDate(dueDate);
        ValidateDescription(description);
        Id = id;
        Title = title;
        Description = description;
        Status = status;
        Priority = priority;
        DueDate = dueDate;
        ProjectId = projectId;
    }

    public void UpdateDueDate(DateTime newDate)
    {
        ValidateDueDate(newDate);
        DueDate = newDate;
    }

    public void UpdatePriority(Priority newPriority)
    {
        Priority = newPriority;
    }

    public void ChangeStatus(Status newStatus)
    {
        if (Status == Status.Done && newStatus != Status.Done)
            throw new InvalidOperationException("Нельзя изменить статус выполненой задачи");
        Status = newStatus;
    }

    public void AssignToProject(Project project)
    {
        if (project.Id <= 0)
            throw new ArgumentException("ID проекта должен быть положительным", nameof(project.Id));

        project.AddTask(this);
        ProjectId = project.Id;
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
            throw new ArgumentException("Название не может быть пустым", nameof(title));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название состоит из пробелов", nameof(title));
    }

    private static void ValidateDueDate(DateTime? dueDate)
    {
        if (dueDate.HasValue && dueDate.Value < DateTime.UtcNow)
            throw new ArgumentException("Дедлайн не может быть в прошлом", nameof(dueDate));
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Описание задачи не может быть пустым", nameof(description));
    }
}
