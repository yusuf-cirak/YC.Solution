using System.Collections.Immutable;

namespace YC.Monad;

/// <summary>
/// Represents an error with a code, message, and optional status code.
/// This type is used by the Result monad to encapsulate error information.
/// </summary>
/// <remarks>
/// <see cref="Error"/> is a value type. Constructing it through <see cref="Create(string,string)"/> or the
/// implicit string conversion guarantees <see cref="Code"/>/<see cref="Message"/> default to
/// <see cref="string.Empty"/> as documented below. <c>default(Error)</c> bypasses those initializers, so its
/// <see cref="Code"/> and <see cref="Message"/> are <see langword="null"/> instead — this is the value every
/// successful <see cref="Result"/>'s <see cref="Result.Error"/> holds, since it is never explicitly set. Prefer
/// <see cref="Result.IsSuccess"/>/<see cref="Result.IsFailure"/> over inspecting <see cref="Result.Error"/> to
/// detect failure; unlike the previous reference-type <c>Error</c>, it is never <see langword="null"/> itself, so
/// an <c>Error != null</c> check is always <see langword="true"/> and no longer signals failure.
/// </remarks>
public readonly record struct Error
{
    /// <summary>
    /// Gets the error code that uniquely identifies the type of error.
    /// </summary>
    /// <value>A string representing the error code. Empty string if no code is specified, or null for <c>default(Error)</c>.</value>
    public string Code { get; } = string.Empty;

    /// <summary>
    /// Gets the human-readable error message describing what went wrong.
    /// </summary>
    /// <value>A string containing the error message. Empty string if no message is specified, or null for <c>default(Error)</c>.</value>
    public string Message { get; } = string.Empty;

    /// <summary>
    /// Gets the HTTP status code associated with this error (if applicable).
    /// </summary>
    /// <value>An integer representing the HTTP status code. 0 if no status code is specified.</value>
    public int Status { get; }

    /// <summary>
    /// Backing store for attributes attached via <see cref="WithAttribute"/>/<see cref="WithAttributes"/>.
    /// Uses a persistent (structurally-shared) dictionary so each With* call is O(log n) instead of
    /// copying the whole map. Null for a default-constructed or attribute-less <see cref="Error"/> —
    /// checked lazily so <c>default(Error)</c> never throws when read or extended.
    /// </summary>
    private readonly ImmutableDictionary<string, object>? _attributes;

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with the specified code and message.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    internal Error(string code, string message)
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
    internal Error(string code, string message, int status)
    {
        Code = code;
        Message = message;
        Status = status;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with the specified code, message, status code, and attributes.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="status">The HTTP status code.</param>
    /// <param name="attributes">The attributes to attach to the error.</param>
    private Error(string code, string message, int status, ImmutableDictionary<string, object> attributes)
    {
        Code = code;
        Message = message;
        Status = status;
        _attributes = attributes;
    }

    /// <summary>
    /// Creates a new <see cref="Error"/> with the specified attribute added, leaving this instance unchanged.
    /// </summary>
    /// <param name="key">The attribute key.</param>
    /// <param name="value">The attribute value.</param>
    /// <returns>A new <see cref="Error"/> instance carrying the merged attributes.</returns>
    public Error WithAttribute(string key, object value)
        => new(Code, Message, Status, (_attributes ?? ImmutableDictionary<string, object>.Empty).SetItem(key, value));

    /// <summary>
    /// Creates a new <see cref="Error"/> with the specified attributes added, leaving this instance unchanged.
    /// </summary>
    /// <param name="attributes">The attributes to merge into the new <see cref="Error"/>.</param>
    /// <returns>A new <see cref="Error"/> instance carrying the merged attributes.</returns>
    public Error WithAttributes(params IEnumerable<KeyValuePair<string, object>> attributes)
        => new(Code, Message, Status, (_attributes ?? ImmutableDictionary<string, object>.Empty).SetItems(attributes));

    /// <summary>
    /// Attempts to get an attribute previously attached via <see cref="WithAttribute"/> or <see cref="WithAttributes"/>.
    /// </summary>
    /// <param name="key">The attribute key.</param>
    /// <param name="value">When this method returns, contains the attribute value if found; otherwise, null.</param>
    /// <returns>true if the attribute was found; otherwise, false.</returns>
    public bool TryGetAttribute(string key, out object? value)
    {
        if (_attributes is not null)
            return _attributes.TryGetValue(key, out value);

        value = null;
        return false;
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
