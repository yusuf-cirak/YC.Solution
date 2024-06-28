namespace YC.Result;

/// <summary>
/// Represents the result of an operation, indicating success or failure.
/// </summary>
public readonly record struct Result
{
    /// <summary>
    /// Gets the error associated with a failed result.
    /// </summary>
    public Error Error { get; } = ErrorsCache.None;

    /// <summary>
    /// Gets a value indicating whether the result is a success.
    /// </summary>
    public bool IsSuccess { get; } = false;

    /// <summary>
    /// Gets a value indicating whether the result is a failure.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> record struct with the specified error.
    /// </summary>
    /// <param name="error">The error associated with the result.</param>
    private Result(Error error)
    {
        Error = error;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> struct with the specified success flag.
    /// </summary>
    /// <param name="isSuccess">A value indicating whether the result is a success.</param>
    internal Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    /// <returns>A cached success <see cref="Result"/>.</returns>
    public static Result Success() => ResultsCache.Success;

    /// <returns>A cached failure <see cref="Result"/>.</returns>
    public static Result Failure() => ResultsCache.Failure;

    /// <summary>
    /// Creates a new failure result with the specified error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A new failure <see cref="Result"/> with the specified error.</returns>
    public static Result Failure(Error error) => new(error);

    /// <summary>
    /// Implicitly converts an <see cref="Error"/> to a failure <see cref="Result"/>.
    /// </summary>
    /// <param name="error">The error to convert to a result.</param>
    public static implicit operator Result(Error error) => Failure(error);

    /// <summary>
    /// Implicitly converts a boolean value to a success or failure <see cref="Result"/>.
    /// </summary>
    /// <param name="isSuccess">The boolean value indicating success or failure.</param>
    public static implicit operator Result(bool isSuccess) => isSuccess ? ResultsCache.Success : ResultsCache.Failure;

    /// <summary>
    /// Matches the result to a success or failure function.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the match functions.</typeparam>
    /// <param name="success">The function to execute if the result is a success.</param>
    /// <param name="failure">The function to execute if the result is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    public TResult Match<TResult>(Func<TResult> success, Func<Error, TResult> failure)
        => this.IsSuccess ? success() : failure(this.Error);

    /// <summary>
    /// Executes one of the provided actions based on the success or failure state of the current result.
    /// </summary>
    /// <param name="success">The action to execute if the result is successful. This parameter is optional and defaults to null.</param>
    /// <param name="failure">The action to execute if the result is a failure, with the error as a parameter. This parameter is optional and defaults to null.</param>
# nullable enable
    public void Match(Action? success = null, Action<Error>? failure = null)
    {
        if (this.IsSuccess)
        {
            success?.Invoke();
        }
        else
        {
            failure?.Invoke(this.Error);
        }
    }
}

/// <summary>
/// Represents the result of an operation, indicating success or failure, and holds a value if successful.
/// </summary>
/// <typeparam name="TValue">The type of the value held by the result if successful.</typeparam>
public readonly record struct Result<TValue>
{
    /// <summary>
    /// Gets the value of the result.
    /// </summary>
    public TValue Value { get; }

    /// <summary>
    /// Gets the error associated with a failed result.
    /// </summary>
    public Error Error { get; } = ErrorsCache.None;

    /// <summary>
    /// Gets a value indicating whether the result is a success.
    /// </summary>
    public bool IsSuccess { get; } = false;

    /// <summary>
    /// Gets a value indicating whether the result is a failure.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> struct with the specified value.
    /// </summary>
    /// <param name="value">The value of the successful result.</param>
    private Result(TValue value)
    {
        Value = value;
        IsSuccess = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValue}"/> struct with the specified error.
    /// </summary>
    /// <param name="error">The error associated with the result.</param>
    private Result(Error error)
    {
        Value = default!;
        Error = error;
    }

    /// <summary>
    /// Creates a success result with the specified value.
    /// </summary>
    /// <param name="value">The value of the successful result.</param>
    /// <returns>A success <see cref="Result{TValue}"/>.</returns>
    public static Result<TValue> Success(TValue value) => new(value);

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>A failure <see cref="Result{TValue}"/>.</returns>
    public static Result<TValue> Failure(Error error) => new(error);

    /// <summary>
    /// Implicitly converts a value to a success result.
    /// </summary>
    /// <param name="value">The value to convert to a result.</param>
    public static implicit operator Result<TValue>(TValue value) => Success(value);

    /// <summary>
    /// Implicitly converts an error to a failure result.
    /// </summary>
    /// <param name="error">The error to convert to a result.</param>
    public static implicit operator Result<TValue>(Error error) => Failure(error);

    /// <summary>
    /// Matches the result to a success or failure function.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the match functions.</typeparam>
    /// <param name="success">The function to execute if the result is a success.</param>
    /// <param name="failure">The function to execute if the result is a failure.</param>
    /// <returns>The result of the executed function.</returns>
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<Error, TResult> failure)
        => this.IsSuccess ? success(this.Value) : failure(this.Error);

    /// <summary>
    /// Executes one of the provided actions based on the success or failure state of the current result.
    /// </summary>
    /// <param name="success">The action to execute if the result is successful, with the value as a parameter. This parameter is optional and defaults to null.</param>
    /// <param name="failure">The action to execute if the result is a failure, with the error as a parameter. This parameter is optional and defaults to null.</param>
# nullable enable
    public void Match(Action<TValue>? success = null, Action<Error>? failure = null)
    {
        if (this.IsSuccess)
        {
            success?.Invoke(this.Value);
        }
        else
        {
            failure?.Invoke(this.Error);
        }
    }
}