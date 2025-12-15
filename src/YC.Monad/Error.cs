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
    /// <value>A string representing the error code. Empty string if no code is specified.</value>
    public string Code { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the human-readable error message describing what went wrong.
    /// </summary>
    /// <value>A string containing the error message. Empty string if no message is specified.</value>
    public string Message { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the HTTP status code associated with this error (if applicable).
    /// </summary>
    /// <value>An integer representing the HTTP status code. 0 if no status code is specified.</value>
    public int Status { get; private set; } = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with the specified code and message.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    protected internal Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with the specified code, message, and status code.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="status">The HTTP status code.</param>
    protected internal Error(string code, string message, int status)
    {
        Code = code;
        Message = message;
        Status = status;
    }
    
    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the specified code and message.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <returns>A new <see cref="Error"/> instance.</returns>
    public static Error Create(string code,string message) => new(code, message);
    
    /// <summary>
    /// Creates a new <see cref="Error"/> instance with the specified code, message, and status code.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="status">The HTTP status code.</param>
    /// <returns>A new <see cref="Error"/> instance.</returns>
    public static Error Create(string code,string message, int status) => new(code, message, status);
    
    /// <summary>
    /// Implicitly converts a string message to an <see cref="Error"/> instance.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>A new <see cref="Error"/> instance with an empty code and the specified message.</returns>
    public static implicit operator Error (string message) => new(string.Empty, message);
}