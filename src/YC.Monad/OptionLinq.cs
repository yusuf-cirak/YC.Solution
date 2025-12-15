namespace YC.Monad;

public static class OptionLinq
{
    /// <summary>
    /// Projects each element of a sequence into a new form.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source Option.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by the mapping function.</typeparam>
    /// <param name="option">The Option to map.</param>
    /// <param name="map">A transform function to apply to the value if it exists.</param>
    /// <returns>An Option whose value is the result of invoking the transform function on the source Option's value if it has one.</returns>
    public static Option<TResult> Select<T, TResult>(this Option<T> option, Func<T, TResult> map)
        => option.Map(map);

    /// <summary>
    /// Projects each element of a sequence to an Option and flattens the resulting sequences into one sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source Option.</typeparam>
    /// <typeparam name="TSecond">The type of the value returned by the binding function.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by the result selector function.</typeparam>
    /// <param name="option">The Option to bind and map.</param>
    /// <param name="bind">A transform function to apply to the value if it exists, returning an Option.</param>
    /// <param name="map">A transform function to apply to the source value and binding result.</param>
    /// <returns>An Option whose value is the result of invoking the transform function on the source Option's value and binding result if they exist.</returns>
    public static Option<TResult> SelectMany<T, TSecond, TResult>(this Option<T> option, Func<T, Option<TSecond>> bind,
        Func<T, TSecond, TResult> map)
        => option.Bind(original => bind(original).Map(result => map(original, result)));

    /// <summary>
    /// Filters an Option based on a predicate.
    /// </summary>
    /// <typeparam name="T">The type of the value in the Option.</typeparam>
    /// <param name="option">The Option to filter.</param>
    /// <param name="predicate">A function to test the value against.</param>
    /// <returns>The Option if it has a value that satisfies the predicate; otherwise, None.</returns>
    public static Option<T> Where<T>(this Option<T> option, Func<T, bool> predicate)
        => option.Bind(value => predicate(value) ? option : Option<T>.None());
}