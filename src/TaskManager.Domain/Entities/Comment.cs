namespace TaskManager.Domain.Entities;

public class Comment
{
    public int Id { get; private set; }
    public string Text { get; private set; }
    public int AuthorId { get; private set; }
    public int TaskId { get; private set; }
    public DateTime CreatedAt { get; }

    private Comment(string text, int authorId, int taskId)
    {
        ValidateText(text);
        Text = text;
        AuthorId = authorId;
        TaskId = taskId;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateText(string newText)
    {
        ValidateText(newText);
        if ((DateTime.UtcNow - CreatedAt).TotalHours > 24)
            throw new InvalidOperationException("Комментарий можно редактировать только в течение 24 часов после создания.");
        Text = newText;
    }

    public static Comment Create(string text, int authorId, int taskId)
    {
        return new Comment(text, authorId, taskId);
    }

    private static void ValidateText(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException("Текст не должен быть пустым", nameof(text));
        if (text.Length > 1000)
            throw new ArgumentException("Длина текста должна быть от 1 до 1000 символов", nameof(text));
    }
}
