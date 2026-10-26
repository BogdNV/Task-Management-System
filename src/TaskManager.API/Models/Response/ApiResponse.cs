namespace TaskManager.API.Models.Response;

public class ApiResponse<T> where T : notnull
{
    public T Data { get; set; } = default!;
    public string Message { get; set; } = string.Empty;
}
