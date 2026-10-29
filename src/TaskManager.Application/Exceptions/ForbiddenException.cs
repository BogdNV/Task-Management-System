namespace TaskManager.Application.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException() : base() { }
    public ForbiddenException(string message) : base(message) { }
    public ForbiddenException(string message, Exception? inner) : base(message, inner) { }
}
