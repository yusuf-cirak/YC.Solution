namespace YC.Monad;

/// <summary>
/// Represents a discriminated union of a success state or a failure state with an error.
/// This monad is useful for error handling and expressing business logic outcomes.
/// </summary>
public record Result
{
    public Error Error { get; protected init; } = ErrorCache.None;

    public bool IsSuccess { get; protected init; }

    public bool IsFailure => !IsSuccess;

    protected Result()
    {
    }

    private Result(Error error)
    {
        Error = error;
        IsSuccess = false;
    }

    internal Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public static Result Success() => ResultCache.Success;

    public static Result Failure(Error error) => new(error);

    public static Result Failure() => ResultCache.Failure;

    public static implicit operator Result(Error error) => Failure(error);

    public TResult Match<TResult>(Func<TResult> success, Func<Error, TResult> failure)
        => IsSuccess ? success() : failure(Error);
}

/// <summary>
/// Represents a discriminated union of a success state with a value of type TValue,
/// or a failure state with an error. This monad combines error handling with type-safe value wrapping.
/// </summary>
/// <typeparam name="TValue">The type of the value in case of success.</typeparam>
public record Result<TValue> : Result
{
    public TValue Value { get; } = default!;

    private Result(TValue value)
    {
        Value = value;
        IsSuccess = true;
    }

    private Result(Error error) : base()
    {
        Error = error;
        IsSuccess = false;
    }

    public static Result<TValue> Success(TValue value) => new(value);

    public new static Result<TValue> Failure(Error error) => new(error);

    public static implicit operator Result<TValue>(TValue value) => Success(value);

    public static implicit operator Result<TValue>(Error error) => Failure(error);

    public static Result<TValue> From(Result result) => result.IsSuccess ? Success(default!) : Failure(result.Error);

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<Error, TResult> failure)
        => this.IsSuccess ? success(this.Value) : failure(this.Error);
}

public static class ResultCache
{
    internal static readonly Result Success = new(true);
    internal static readonly Result Failure = new(false);

    public static readonly Result Unauthorized = Result.Failure(ErrorCache.Unauthorized);

    public static readonly Result BadRequest = Result.Failure(ErrorCache.BadRequest);

    public static readonly Result NotFound = Result.Failure(ErrorCache.NotFound);

    public static readonly Result Forbidden = Result.Failure(ErrorCache.Forbidden);
}

public static class ResultExtensions
{
    public static Result Create(Error error) => Result.Failure(error);

    public static Result<TValue> Create<TValue>(TValue value) => Result<TValue>.Success(value);

    public static TResponse ToTypedResult<TResponse>(this Result result) where TResponse : Result
    {
        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)result;
        }

        var genericType = typeof(TResponse).GetGenericArguments()[0];
        var genericResultType = typeof(Result<>).MakeGenericType(genericType);
        return (TResponse)genericResultType.GetMethod(nameof(Result<object>.From))!
            .Invoke(null, new object[]{result})!;
    }
}