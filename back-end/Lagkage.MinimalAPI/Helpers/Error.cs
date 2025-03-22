namespace Lagkage.MinimalAPI.Features;

public enum ErrorType
{
    ValidationError,
    Conflict,
    NotFound,
}

public class Error
{
    public ErrorType ErrorType { get; }
    public string Message { get; }

    public Error(ErrorType errorType, string message)
    {
        ErrorType = errorType;
        Message = message;
    }
}