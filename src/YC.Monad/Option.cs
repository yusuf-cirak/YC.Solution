namespace YC.Monad;

/// <summary>
/// Represents an optional value that may or may not contain a value of type T.
/// This is a monad that helps handle null values and missing data in a functional way.
/// </summary>
/// <typeparam name="T">The type of the value that may be contained in the Option.</typeparam>
public record Option<T>
{
    /// <summary>
    /// Gets a value indicating whether this Option contains a value.
    /// </summary>
    /// <value>true if this Option contains a value; otherwise, false.</value>
    public bool HasValue { get; }
    
    /// <summary>
    /// The underlying value stored in this Option.
    /// </summary>
    private readonly T _content;

    /// <summary>
    /// Initializes a new instance of the <see cref="Option{T}"/> class with a value.
    /// </summary>
    /// <param name="content">The value to store in this Option.</param>
    private Option(T content)
    {
        _content = content;
        HasValue = true;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Option{T}"/> class representing None.
    /// </summary>
    internal Option()
    {
        _content = default!;
        HasValue = false;
    }

    /// <summary>
    /// Creates an Option from a nullable value.
    /// </summary>
    /// <param name="value">The nullable value to create an Option from.</param>
    /// <returns>Some(value) if value is not null; otherwise, None.</returns>
    public static Option<T> Create(T? value) => value is not null ? Some(value) : None();

    /// <summary>
    /// Creates an Option containing the specified value.
    /// </summary>
    /// <param name="value">The value to wrap in an Option.</param>
    /// <returns>An Option containing the specified value.</returns>
    public static Option<T> Some(T value) => new(value);

    /// <summary>
    /// Gets an Option representing no value.
    /// </summary>
    /// <returns>An Option representing no value.</returns>
    public static Option<T> None() => OptionCache<T>.None;

    /// <summary>
    /// Attempts to get the value contained in this Option.
    /// </summary>
    /// <param name="value">When this method returns, contains the value stored in this Option if it has one, or the default value of T if it doesn't.</param>
    /// <returns>true if this Option has a value; otherwise, false.</returns>
    public bool TryGetValue(out T value)
    {
        value = _content;
        return HasValue;
    }
    
    /// <summary>
    /// Gets the value contained in this Option, or the default value of T if this Option has no value.
    /// </summary>
    /// <returns>The value if this Option has one; otherwise, the default value of T.</returns>
    public T GetValueOrDefault() => _content;

    /// <summary>
    /// Gets the value contained in this Option, or throws an exception if this Option has no value.
    /// </summary>
    /// <returns>The value contained in this Option.</returns>
    /// <exception cref="NoneException">Thrown when this Option has no value.</exception>
    public T GetValueOrFail() => HasValue ? _content : throw new NoneException($"Expected value of type {typeof(T).Name} but got None");

    /// <summary>
    /// Matches this Option to one of two functions based on whether it has a value.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of the match operation.</typeparam>
    /// <param name="none">The function to execute if this Option has no value.</param>
    /// <param name="some">The function to execute if this Option has a value.</param>
    /// <returns>The result of executing either the none or some function.</returns>
    public TResult Match<TResult>(Func<TResult> none, Func<T, TResult> some) => HasValue ? some(_content) : none();

    /// <summary>
    /// Maps the value contained in this Option using the specified mapping function.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of the mapping operation.</typeparam>
    /// <param name="map">The function to map the value with.</param>
    /// <returns>An Option containing the mapped value if this Option has a value; otherwise, None.</returns>
    public Option<TResult> Map<TResult>(Func<T, TResult> map)
        => HasValue ? Option<TResult>.Some(map(_content)) : Option<TResult>.None();

    /// <summary>
    /// Binds this Option to another Option using the specified binding function.
    /// </summary>
    /// <typeparam name="TResult">The type of the result of the binding operation.</typeparam>
    /// <param name="bind">The function to bind the value with.</param>
    /// <returns>The result of the binding function if this Option has a value; otherwise, None.</returns>
    public Option<TResult> Bind<TResult>(Func<T, Option<TResult>> bind)
        => HasValue ? bind(_content) : Option<TResult>.None();

    /// <summary>
    /// Implicitly converts a nullable value to an Option.
    /// </summary>
    /// <param name="value">The nullable value to convert.</param>
    /// <returns>Some(value) if value is not null; otherwise, None.</returns>
    public static implicit operator Option<T>(T? value) => value is not null ? Some(value) : None();
}

/// <summary>
/// Provides a cache of None options to avoid unnecessary object creation.
/// </summary>
/// <typeparam name="T">The type of the value that would be contained in the Option.</typeparam>
internal static class OptionCache<T>
{
    /// <summary>
    /// Gets a cached None option of type T.
    /// </summary>
    public static readonly Option<T> None = new();
}

/// <summary>
/// Provides extension methods for working with Option types.
/// </summary>
public static class OptionExtensions
{
    /// <summary>
    /// Converts a nullable value to an Option.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The nullable value to convert.</param>
    /// <returns>Some(value) if value is not null; otherwise, None.</returns>
    public static Option<T> ToOption<T>(this T? value) => Option<T>.Create(value);

    /// <summary>
    /// Returns the first element of a sequence that satisfies a condition as an Option, or None if no such element exists.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence to search.</param>
    /// <param name="predicate">The condition to test elements against.</param>
    /// <returns>Some(element) for the first element that satisfies the condition; otherwise, None.</returns>
    public static Option<T> FirstOrNone<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        => source
            .Where(predicate)
            .Select(Option<T>.Some)
            .DefaultIfEmpty(Option<T>.None())
            .First();

    /// <summary>
    /// Returns the single element of a sequence that satisfies a condition as an Option, or None if no such element exists or if more than one such element exists.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence to search.</param>
    /// <param name="predicate">The condition to test elements against.</param>
    /// <returns>Some(element) if exactly one element satisfies the condition; otherwise, None.</returns>
    public static Option<T> SingleOrNone<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        => source
            .Where(predicate)
            .Select(Option<T>.Some)
            .DefaultIfEmpty(Option<T>.None())
            .Single();

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

/// <summary>
/// Exception thrown when attempting to access the value of a None Option.
/// </summary>
public sealed class NoneException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NoneException"/> class with the specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public NoneException(string message) : base(message) { }
}