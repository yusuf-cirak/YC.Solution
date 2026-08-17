namespace YC.Monad;

/// <summary>
/// Provides Option-returning search extensions for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class OptionEnumerable
{
    /// <param name="source">The sequence to search.</param>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    extension<T>(IEnumerable<T> source)
    {
        /// <summary>
        /// Returns the first element of a sequence that has an value, or None if no such element exists.
        /// </summary>
        /// <returns>Some(element) for the first element that satisfies the condition; otherwise, None.</returns>
        public Option<T> FirstOrNone()
            => source
                .Select(Option<T>.Some)
                .DefaultIfEmpty(Option<T>.None())
                .First();

        /// <summary>
        /// Returns the first element of a sequence that satisfies a condition as an Option, or None if no such element exists.
        /// </summary>
        /// <param name="predicate">The condition to test elements against.</param>
        /// <returns>Some(element) for the first element that satisfies the condition; otherwise, None.</returns>
        public Option<T> FirstOrNone(Func<T, bool> predicate)
            => source
                .Where(predicate)
                .Select(Option<T>.Some)
                .DefaultIfEmpty(Option<T>.None())
                .First();

        /// <summary>
        /// Returns the single element of a sequence that satisfies a condition as an Option, or None if no such element exists or if more than one such element exists.
        /// </summary>
        /// <param name="predicate">The condition to test elements against.</param>
        /// <returns>Some(element) if exactly one element satisfies the condition; otherwise, None.</returns>
        public Option<T> SingleOrNone(Func<T, bool> predicate)
        {
            var results = source.Where(predicate).Take(2).ToArray();
            return results.Length == 1 ? Option<T>.Some(results[0]) : Option<T>.None();
        }
    }
}
