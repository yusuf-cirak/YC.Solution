namespace YC.Monad;

/// <summary>
/// Provides LINQ support for Result and Result&lt;T&gt; types.
/// </summary>
public static class ResultLinq
{
    /// <summary>
    /// Projects the value of a successful Result into a new form.
    /// </summary>
    /// <typeparam name="T">The type of the value in the source Result.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by the mapping function.</typeparam>
    /// <param name="result">The Result to map.</param>
    /// <param name="map">A transform function to apply to the value if it exists.</param>
    /// <returns>A Result whose value is the result of invoking the transform function on the source Result's value if it's successful, or a failed Result with the same error.</returns>
    public static Result<TResult> Select<T, TResult>(this Result<T> result, Func<T, TResult> map)
        => result.IsSuccess ? Result<TResult>.Success(map(result.Value)) : Result<TResult>.Failure(result.Error);

    /// <summary>
    /// Projects the value of a successful Result to a Result and flattens the resulting sequences into one sequence.
    /// </summary>
    /// <typeparam name="T">The type of the value in the source Result.</typeparam>
    /// <typeparam name="TSecond">The type of the value returned by the binding function.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by the result selector function.</typeparam>
    /// <param name="result">The Result to bind and map.</param>
    /// <param name="bind">A transform function to apply to the value if it exists, returning a Result.</param>
    /// <param name="map">A transform function to apply to the source value and binding result.</param>
    /// <returns>A Result whose value is the result of invoking the transform function on the source Result's value and binding result if they're both successful, or a failed Result with the first error encountered.</returns>
    public static Result<TResult> SelectMany<T, TSecond, TResult>(this Result<T> result, Func<T, Result<TSecond>> bind,
        Func<T, TSecond, TResult> map)
    {
        if (!result.IsSuccess)
            return Result<TResult>.Failure(result.Error);

        var bound = bind(result.Value);
        if (!bound.IsSuccess)
            return Result<TResult>.Failure(bound.Error);

        return Result<TResult>.Success(map(result.Value, bound.Value));
    }

    /// <summary>
    /// Filters a Result based on a predicate. If the predicate fails, returns a failed Result.
    /// </summary>
    /// <typeparam name="T">The type of the value in the Result.</typeparam>
    /// <param name="result">The Result to filter.</param>
    /// <param name="predicate">A function to test the value against.</param>
    /// <param name="error">The error to return if the predicate fails.</param>
    /// <returns>The Result if it's successful and the value satisfies the predicate; otherwise, a failed Result with the specified error or original error if already failed.</returns>
    public static Result<T> Where<T>(this Result<T> result, Func<T, bool> predicate, Error error)
    {
        if (!result.IsSuccess)
            return result;
        
        return predicate(result.Value) ? result : Result<T>.Failure(error);
    }

    /// <summary>
    /// Filters a Result based on a predicate. If the predicate fails, returns a failed Result with a default error.
    /// </summary>
    /// <typeparam name="T">The type of the value in the Result.</typeparam>
    /// <param name="result">The Result to filter.</param>
    /// <param name="predicate">A function to test the value against.</param>
    /// <returns>The Result if it's successful and the value satisfies the predicate; otherwise, a failed Result.</returns>
    public static Result<T> Where<T>(this Result<T> result, Func<T, bool> predicate)
        => Where(result, predicate, Error.Create("PREDICATE_FAILED", "The predicate condition was not satisfied"));
}