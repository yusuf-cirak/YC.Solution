namespace YC.Monad;

/// <summary>
/// Represents an error with a code, message, and optional status code.
/// This type is used by the Result monad to encapsulate error information.
/// </summary>
public record Error
{
    /// <summary>
    /// Gets the error code that uniquely identifies the type of error.
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the human-readable error message describing what went wrong.
    /// </summary>
    public string Message { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the HTTP status code associated with this error (if applicable).
    /// </summary>
    public int Status { get; private set; } = 0;


    protected internal Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    protected internal Error(string code, string message, int status)
    {
        Code = code;
        Message = message;
        Status = status;
    }
    
    
    
    public static Error Create(string code,string message) => new(code, message);
    
    public static Error Create(string code,string message, int status) => new(code, message, status);
    
    public static implicit operator Error (string message) => new(string.Empty, message);
}



/// <summary>
/// Provides a cache of commonly used Error instances to avoid unnecessary object creation.
/// </summary>
public static class ErrorCache
{
    public static readonly Error None = new(string.Empty, string.Empty);
    
    public static readonly Error Unauthorized = new("Error.Unauthorized", "Unauthorized",401);
    
    public static readonly Error BadRequest = new("Error.BadRequest", "Bad Request",400);
    
    public static readonly Error NotFound = new("Error.NotFound", "Not Found",404);
    
    public static readonly Error Forbidden = new("Error.Forbidden", "Forbidden",403);
}