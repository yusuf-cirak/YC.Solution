namespace YC.Result;

public readonly record struct Result
{
    public Error Error { get; } = Error.None;

    public bool IsSuccess { get; } = false;

    public bool IsFailure => !IsSuccess;

    internal Result(Error error)
    {
        Error = error;
    }

    internal Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public static readonly Result Success = new(true);

    public static readonly Result Fail = new(false);

    public static Result Failure() => new(false);
    public static Result Failure(Error error) => new(error);

    public static implicit operator Result(Error error) => Failure(error);

    public static implicit operator Result(bool isSuccess) => isSuccess ? Result.Success : Result.Fail;


    public TResult Match<TResult>(Func<TResult> success, Func<Error, TResult> failure)
        => this.IsSuccess ? success() : failure(this.Error);
}

public readonly record struct Result<TValue>
{
    public TValue Value { get; private init; }
    
    public Error Error { get; init; } = Error.None;

    public bool IsSuccess { get; init; } = false;

    public bool IsFailure => !IsSuccess;

    private Result(TValue value)
    {
        Value = value;
        IsSuccess = true;
    }

    private Result(Error error)
    {
        Value = default!;
        Error = error;
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public static Result<TValue> Failure(Error error) => new(error);

    public static implicit operator Result<TValue>(TValue value) => Success(value);

    public static implicit operator Result<TValue>(Error error) => Failure(error);

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<Error, TResult> failure)
        => this.IsSuccess ? success(this.Value) : failure(this.Error);
}