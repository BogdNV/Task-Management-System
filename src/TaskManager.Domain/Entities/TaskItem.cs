using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

public class TaskItem
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Status Status { get; private set; } = Status.New;
    public Priority Priority { get; private set; } = Priority.Medium;
    public DateTime DueDate { get; private set; }
    public int ProjectId { get; private set; }

    public TaskItem(string title, string description, int projectId, DateTime dueDate)
    {
        ValidateTitle(title);
        ValidateDueDate(dueDate);
        Title = title;
        Description = description;
        DueDate = dueDate;
        ProjectId = projectId;
    }

    public void ChangeStatus(Status newStatus)
    {
        if (Status == Status.Done && newStatus != Status.Done)
            throw new InvalidOperationException("Нельзя изменить статус выполненой задачи");
        Status = newStatus;
    }

    public void AssignToProject(Project project)
    {
        project.AddTask(this);
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
            throw new ArgumentNullException(nameof(title), "Название не может быть пустым");
    }

    private static void ValidateDueDate(DateTime dueDate)
    {
        if (dueDate < DateTime.UtcNow)
            throw new ArgumentException("Дедлайн не может быть в прошлом", nameof(dueDate));
    }
}
