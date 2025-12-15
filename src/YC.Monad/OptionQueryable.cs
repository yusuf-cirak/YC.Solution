using System.Linq.Expressions;

namespace YC.Monad;

public static class OptionQueryable
{
    /// <summary>
    /// Returns the first element of a sequence that satisfies a condition as an Option, or None if no such element exists.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence to search.</param>
    /// <param name="predicate">The condition to test elements against.</param>
    /// <returns>Some(element) for the first element that satisfies the condition; otherwise, None.</returns>
    public static Option<T> FirstOrNone<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
        => source
            .Where(predicate)
            .FirstOrDefault() is {} value ? Option<T>.Some(value) : Option<T>.None();
    
    
    /// <summary>
    /// Returns the single element of a sequence that satisfies a condition as an Option, or None if no such element exists or if more than one such element exists.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence to search.</param>
    /// <param name="predicate">The condition to test elements against.</param>
    /// <returns>Some(element) if exactly one element satisfies the condition; otherwise, None.</returns>
    public static Option<T> SingleOrNone<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
        => source
            .Where(predicate)
            .FirstOrDefault() is {} value
            ? Option<T>.Some(value)
            : Option<T>.None();
}