namespace YC.Monad;

/// <summary>
/// Represents a discriminated union of a success state or a failure state with an error.
/// This monad is useful for error handling and expressing business logic outcomes.
/// </summary>
/// <remarks>
/// <see cref="Result"/> and <see cref="Result{TValue}"/> are independent value types (they do not
/// inherit from one another — structs cannot). <c>default(Result)</c> has <see cref="IsSuccess"/> equal
/// to <see langword="false"/>, so an uninitialized <see cref="Result"/> reads as a failure rather than
/// silently as a success.
/// </remarks>
public readonly record struct Result
{
    /// <summary>
    /// Gets the error associated with this result if it represents a failure.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Gets a value indicating whether this result represents a successful operation.
    /// </summary>
    /// <value>true if this result represents success; otherwise, false.</value>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether this result represents a failed operation.
    /// </summary>
    /// <value>true if this result represents failure; otherwise, false.</value>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> struct with the specified success state.
    /// </summary>
    /// <param name="isSuccess">A value indicating whether this result represents success.</param>
    internal Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> struct with the specified error.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    internal Result(Error error)
    {
        Error = error;
        IsSuccess = false;
    }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <returns>A successful result.</returns>
    public static Result Success() => new(true);

    /// <summary>
    /// Creates a successful result containing the specified value.
    /// </summary>
    /// <param name="value">The success value.</param>
    /// <returns>A successful result containing the specified value.</returns>
    public static Result<TValue> Success<TValue>(TValue value) => new(value);

    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    /// <returns>A failed result containing the specified error.</returns>
    public static Result Failure(Error error) => new(error);

    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    /// <returns>A failed result containing the specified error.</returns>
    public static Result<TValue> Failure<TValue>(Error error) => new(error);

    /// <summary>
    /// Creates a failed result with no specific error.
    /// </summary>
    /// <returns>A failed result.</returns>
    public static Result Failure() => new(false);

    /// <summary>
    /// Implicitly converts an error to a failed result.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <returns>A failed result containing the specified error.</returns>
    public static implicit operator Result(Error error) => Failure(error);

    /// <summary>
    /// Matches the result to one of two functions based on whether it represents success or failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of the match operation.</typeparam>
    /// <param name="success">The function to execute if this result represents success.</param>
    /// <param name="failure">The function to execute if this result represents failure.</param>
    /// <returns>The result of executing either the success or failure function.</returns>
    public TResult Match<TResult>(Func<TResult> success, Func<Error, TResult> failure)
        => IsSuccess ? success() : failure(Error);

    /// <summary>
    /// Converts this untyped result to a <see cref="Result{TValue}"/>, using the default value of
    /// <typeparamref name="TValue"/> for the success case (this result carries no value to preserve).
    /// </summary>
    /// <typeparam name="TValue">The value type of the target result.</typeparam>
    /// <returns>A typed result with the same success state and error (if any) as this result.</returns>
    public Result<TValue> ToTypedResult<TValue>()
        => IsSuccess ? Result<TValue>.Success(default!) : Result<TValue>.Failure(Error);
}

/// <summary>
/// Represents a discriminated union of a success state with a value of type TValue,
/// or a failure state with an error. This monad combines error handling with type-safe value wrapping.
/// </summary>
/// <typeparam name="TValue">The type of the value in case of success.</typeparam>
public readonly record struct Result<TValue>
{
    /// <summary>
    /// Gets the value associated with this result if it represents success.
    /// </summary>
    /// <value>The value if this result represents success.</value>
    public TValue Value { get; }

    /// <summary>
    /// Gets the error associated with this result if it represents a failure.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Gets a value indicating whether this result represents a successful operation.
    /// </summary>
    /// <value>true if this result represents success; otherwise, false.</value>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether this result represents a failed operation.
    /// </summary>
    /// <value>true if this result represents failure; otherwise, false.</value>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> struct with a success value.
    /// </summary>
    /// <param name="value">The success value.</param>
    internal Result(TValue value)
    {
        Value = value;
        IsSuccess = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> struct with an error.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    internal Result(Error error)
    {
        Error = error;
        IsSuccess = false;
    }

    /// <summary>
    /// Creates a successful result containing the specified value.
    /// </summary>
    /// <param name="value">The success value.</param>
    /// <returns>A successful result containing the specified value.</returns>
    public static Result<TValue> Success(TValue value) => new(value);

    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    /// <returns>A failed result containing the specified error.</returns>
    public static Result<TValue> Failure(Error error) => new(error);

    /// <summary>
    /// Implicitly converts a value to a successful result containing that value.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>A successful result containing the specified value.</returns>
    public static implicit operator Result<TValue>(TValue value) => Success(value);

    /// <summary>
    /// Implicitly converts an error to a failed result.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    /// <returns>A failed result containing the specified error.</returns>
    public static implicit operator Result<TValue>(Error error) => Failure(error);

    /// <summary>
    /// Creates a typed result from an untyped result.
    /// </summary>
    /// <param name="result">The untyped result to convert.</param>
    /// <returns>A typed result with the same success state and error (if any) as the input result.</returns>
    public static Result<TValue> From(Result result) => result.ToTypedResult<TValue>();

    /// <summary>
    /// Matches the result to one of two functions based on whether it represents success or failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of the match operation.</typeparam>
    /// <param name="success">The function to execute if this result represents success.</param>
    /// <param name="failure">The function to execute if this result represents failure.</param>
    /// <returns>The result of executing either the success or failure function.</returns>
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<Error, TResult> failure)
        => IsSuccess ? success(Value) : failure(Error);
}
