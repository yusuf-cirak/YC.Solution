namespace YC.Monad;

/// <summary>
/// Provides extension methods for composing <see cref="Result{T}"/> instances
/// in a Railway Oriented Programming (ROP) style.
/// </summary>
/// <remarks>
/// These methods allow chaining operations while automatically propagating failures.
/// This eliminates the need for repetitive error handling and enables a linear workflow.
/// </remarks>
public static class ResultRailway
{
    /// <summary>
    /// Transforms the value of a successful result.
    /// </summary>
    /// <example>
    /// <code>
    /// var result = Result.Success(5)
    ///     .Map(x => x * 2); // Result(10)
    /// </code>
    /// </example>
    public static Result<TOut> Map<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> func)
    {
        return !result.IsSuccess
            ? Result<TOut>.Failure(result.Error)
            : Result.Success(func(result.Value));
    }

    /// <summary>
    /// Asynchronously transforms the value of a successful result.
    /// </summary>
    public static async Task<Result<TOut>> MapAsync<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Task<TOut>> func)
    {
        if (!result.IsSuccess)
            return Result<TOut>.Failure(result.Error);

        var value = await func(result.Value).ConfigureAwait(false);
        return Result.Success(value);
    }

    /// <summary>
    /// Chains another result-returning operation.
    /// </summary>
    /// <remarks>
    /// Equivalent to SelectMany in LINQ. Prevents nested Result instances.
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = GetUser(id)
    ///     .Bind(ValidateUser)
    ///     .Bind(GetOrders);
    /// </code>
    /// </example>
    public static Result<TOut> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Result<TOut>> func)
    {
        return result.IsSuccess
            ? func(result.Value)
            : Result<TOut>.Failure(result.Error);
    }

    /// <summary>
    /// Asynchronously chains another result-returning operation.
    /// </summary>
    public static async Task<Result<TOut>> BindAsync<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Task<Result<TOut>>> func)
    {
        return result.IsSuccess
            ? await func(result.Value).ConfigureAwait(false)
            : Result<TOut>.Failure(result.Error);
    }

    /// <summary>
    /// Executes a side-effect if the result is successful.
    /// </summary>
    public static Result<T> Tap<T>(
        this Result<T> result,
        Action<T> action)
    {
        if (result.IsSuccess)
            action(result.Value);

        return result;
    }

    /// <summary>
    /// Executes a side-effect if the result is a failure.
    /// </summary>
    public static Result<T> TapError<T>(
        this Result<T> result,
        Action<Error> action)
    {
        if (!result.IsSuccess)
            action(result.Error);

        return result;
    }

    /// <summary>
    /// Asynchronously executes a side-effect if the result is successful.
    /// </summary>
    public static async Task<Result<T>> TapAsync<T>(
        this Result<T> result,
        Func<T, Task> action)
    {
        if (result.IsSuccess)
            await action(result.Value).ConfigureAwait(false);

        return result;
    }

    /// <summary>
    /// Asynchronously executes a side-effect if the result is a failure.
    /// </summary>
    public static async Task<Result<T>> TapErrorAsync<T>(
        this Result<T> result,
        Func<Error, Task> action)
    {
        if (!result.IsSuccess)
            await action(result.Error).ConfigureAwait(false);

        return result;
    }

    /// <summary>
    /// Ensures that a condition holds for the value.
    /// </summary>
    /// <remarks>
    /// Converts a successful result into a failure if the predicate is not satisfied.
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Result.Success(10)
    ///     .Ensure(x => x > 0, Error.Validation("Must be positive"));
    /// </code>
    /// </example>
    public static Result<T> Ensure<T>(
        this Result<T> result,
        Func<T, bool> predicate,
        Error error)
    {
        if (!result.IsSuccess)
            return result;

        return predicate(result.Value)
            ? result
            : Result<T>.Failure(error);
    }

    /// <summary>
    /// Executes a function and captures exceptions as a failure result.
    /// </summary>
    public static Result<T> Try<T>(
        Func<T> func,
        Func<Exception, Error>? errorHandler = null)
    {
        try
        {
            return Result.Success(func());
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(errorHandler != null ? errorHandler(ex) : ex.Message);
        }
    }

    /// <summary>
    /// Asynchronously executes a function and captures exceptions as a failure result.
    /// </summary>
    public static async Task<Result<T>> TryAsync<T>(
        Func<Task<T>> func,
        Func<Exception, Error>? errorHandler = null)
    {
        try
        {
            return Result.Success(await func().ConfigureAwait(false));
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(errorHandler != null ? errorHandler(ex) : ex.Message);
        }
    }
}