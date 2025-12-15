namespace YC.Monad;

/// <summary>
/// Represents a discriminated union of a success state or a failure state with an error.
/// This monad is useful for error handling and expressing business logic outcomes.
/// </summary>
public record Result
{
    /// <summary>
    /// Gets the error associated with this result if it represents a failure.
    /// </summary>
    /// <value>The error object if this is a failure, or <see cref="ErrorCache.None"/> if this is a success.</value>
    public Error Error { get; protected init; } = ErrorCache.None;

    /// <summary>
    /// Gets a value indicating whether this result represents a successful operation.
    /// </summary>
    /// <value>true if this result represents success; otherwise, false.</value>
    public bool IsSuccess { get; protected init; }

    /// <summary>
    /// Gets a value indicating whether this result represents a failed operation.
    /// </summary>
    /// <value>true if this result represents failure; otherwise, false.</value>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    protected Result()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class with the specified error.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    private Result(Error error)
    {
        Error = error;
        IsSuccess = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class with the specified success state.
    /// </summary>
    /// <param name="isSuccess">A value indicating whether this result represents success.</param>
    internal Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <returns>A successful result.</returns>
    public static Result Success() => ResultCache.Success;

    /// <summary>
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    /// <returns>A failed result containing the specified error.</returns>
    public static Result Failure(Error error) => new(error);

    /// <summary>
    /// Creates a failed result with no specific error.
    /// </summary>
    /// <returns>A failed result.</returns>
    public static Result Failure() => ResultCache.Failure;

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
    /// Creates a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error to create the result from.</param>
    /// <returns>A failed result containing the specified error.</returns>
    public static Result Create(Error error) => Result.Failure(error);

    /// <summary>
    /// Converts a result to a typed result of the specified type.
    /// </summary>
    /// <typeparam name="TResponse">The type of result to convert to.</typeparam>
    /// <param name="result">The result to convert.</param>
    /// <returns>A typed result with the same success state and error (if any) as the input result.</returns>
    public TResponse ToTypedResult<TResponse>() where TResponse : Result
    {
        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)this;
        }
        
        var genericType = typeof(TResponse).GetGenericArguments()[0];
        var genericResultType = typeof(Result<>).MakeGenericType(genericType);
        return (TResponse)genericResultType.GetMethod(nameof(Result<object>.From))!
            .Invoke(null, new object[]{this})!;
    }
}

/// <summary>
/// Represents a discriminated union of a success state with a value of type TValue,
/// or a failure state with an error. This monad combines error handling with type-safe value wrapping.
/// </summary>
/// <typeparam name="TValue">The type of the value in case of success.</typeparam>
public record Result<TValue> : Result
{
    /// <summary>
    /// Gets the value associated with this result if it represents success.
    /// </summary>
    /// <value>The value if this result represents success.</value>
    public TValue Value { get; } = default!;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> class with a success value.
    /// </summary>
    /// <param name="value">The success value.</param>
    private Result(TValue value)
    {
        Value = value;
        IsSuccess = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> class with an error.
    /// </summary>
    /// <param name="error">The error that caused the failure.</param>
    private Result(Error error) : base()
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
    public new static Result<TValue> Failure(Error error) => new(error);

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
    public static Result<TValue> From(Result result) => result.IsSuccess ? Success(default!) : Failure(result.Error);

    /// <summary>
    /// Matches the result to one of two functions based on whether it represents success or failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of the match operation.</typeparam>
    /// <param name="success">The function to execute if this result represents success.</param>
    /// <param name="failure">The function to execute if this result represents failure.</param>
    /// <returns>The result of executing either the success or failure function.</returns>
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<Error, TResult> failure)
        => this.IsSuccess ? success(this.Value) : failure(this.Error);
}