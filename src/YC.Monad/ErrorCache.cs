namespace YC.Monad;

/// <summary>
/// Provides a cache of commonly used Error instances to avoid unnecessary object creation.
/// </summary>
public static class ErrorCache
{
    /// <summary>
    /// Represents an empty error with no code or message.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);
    
    /// <summary>
    /// Represents an unauthorized error with HTTP status code 401.
    /// </summary>
    public static readonly Error Unauthorized = new("Error.Unauthorized", "Unauthorized",401);
    
    /// <summary>
    /// Represents a bad request error with HTTP status code 400.
    /// </summary>
    public static readonly Error BadRequest = new("Error.BadRequest", "Bad Request",400);
    
    /// <summary>
    /// Represents a not found error with HTTP status code 404.
    /// </summary>
    public static readonly Error NotFound = new("Error.NotFound", "Not Found",404);
    
    /// <summary>
    /// Represents a forbidden error with HTTP status code 403.
    /// </summary>
    public static readonly Error Forbidden = new("Error.Forbidden", "Forbidden",403);
}