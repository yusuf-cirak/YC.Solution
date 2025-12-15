using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace YC.Monad.EntityFrameworkCore;

/// <summary>
/// Provides extension methods for converting Entity Framework Core async query results into Option types.
/// </summary>
public static class OptionAsyncExtensions
{
    /// <summary>
    /// Asynchronously returns the first element of a sequence wrapped in an Option, or None if the sequence contains no elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">An IQueryable&lt;T&gt; to return the first element of.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains Some(T) with the first element in source,
    /// or None if source is empty.
    /// </returns>
    public static async Task<Option<T>> FirstOrNoneAsync<T>(this IQueryable<T> source,
        CancellationToken cancellationToken = default)
        => await source.FirstOrDefaultAsync(cancellationToken) is { } value
            ? Option<T>.Some(value)
            : Option<T>.None();
    
    /// <summary>
    /// Asynchronously returns the first element of a sequence that satisfies a specified condition wrapped in an Option,
    /// or None if no such element is found.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">An IQueryable&lt;T&gt; to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains Some(T) with the first element that passes
    /// the test in the specified predicate function, or None if no such element is found.
    /// </returns>
    public static async Task<Option<T>> FirstOrNoneAsync<T>(this IQueryable<T> source,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await source.FirstOrDefaultAsync(predicate, cancellationToken) is { } value
            ? Option<T>.Some(value)
            : Option<T>.None();
    
    /// <summary>
    /// Asynchronously returns the only element of a sequence wrapped in an Option, or None if the sequence is empty.
    /// This method throws an exception if there is more than one element in the sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">An IQueryable&lt;T&gt; to return the single element of.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains Some(T) with the single element of the input sequence,
    /// or None if the sequence contains no elements.
    /// </returns>
    /// <exception cref="InvalidOperationException">The input sequence contains more than one element.</exception>
    public static async Task<Option<T>> SingleOrNoneAsync<T>(this IQueryable<T> source,
        CancellationToken cancellationToken = default)
        => await source.SingleOrDefaultAsync(cancellationToken) is { } value
            ? Option<T>.Some(value)
            : Option<T>.None();
    
    /// <summary>
    /// Asynchronously returns the only element of a sequence that satisfies a specified condition wrapped in an Option,
    /// or None if no such element exists. This method throws an exception if more than one element satisfies the condition.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">An IQueryable&lt;T&gt; to return a single element from.</param>
    /// <param name="predicate">A function to test an element for a condition.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains Some(T) with the single element that satisfies
    /// the condition in predicate, or None if no such element is found.
    /// </returns>
    /// <exception cref="InvalidOperationException">More than one element satisfies the condition in predicate.</exception>
    public static async Task<Option<T>> SingleOrNoneAsync<T>(this IQueryable<T> source,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await source.SingleOrDefaultAsync(predicate, cancellationToken) is { } value
            ? Option<T>.Some(value)
            : Option<T>.None();
    
    /// <summary>
    /// Asynchronously returns the last element of a sequence wrapped in an Option, or None if the sequence contains no elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">An IQueryable&lt;T&gt; to return the last element of.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains Some(T) with the last element in source,
    /// or None if source is empty.
    /// </returns>
    public static async Task<Option<T>> LastOrNoneAsync<T>(this IQueryable<T> source,
        CancellationToken cancellationToken = default)
        => await source.LastOrDefaultAsync(cancellationToken) is { } value
            ? Option<T>.Some(value)
            : Option<T>.None();
    
    /// <summary>
    /// Asynchronously returns the last element of a sequence that satisfies a specified condition wrapped in an Option,
    /// or None if no such element is found.
    /// </summary>
    /// <typeparam name="T">The type of the elements of source.</typeparam>
    /// <param name="source">An IQueryable&lt;T&gt; to return an element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains Some(T) with the last element that passes
    /// the test in the specified predicate function, or None if no such element is found.
    /// </returns>
    public static async Task<Option<T>> LastOrNoneAsync<T>(this IQueryable<T> source,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await source.LastOrDefaultAsync(predicate, cancellationToken) is { } value
            ? Option<T>.Some(value)
            : Option<T>.None();
    
    /// <summary>
    /// Asynchronously finds an entity with the given primary key values wrapped in an Option.
    /// If no entity is found, then None is returned.
    /// </summary>
    /// <typeparam name="T">The type of entity to find.</typeparam>
    /// <param name="source">A DbSet&lt;T&gt; to find an entity in.</param>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <returns>
    /// A task that represents the asynchronous find operation. The task result contains Some(T) with the entity found,
    /// or None if no entity with the given primary key values exists.
    /// </returns>
    public static async ValueTask<Option<T>> FindOrNoneAsync<T>(this DbSet<T> source,
        params object[] keyValues) where T : class
        => await source.FindAsync(keyValues) is { } value
            ? Option<T>.Some(value)
            : Option<T>.None();
    
    /// <summary>
    /// Asynchronously finds an entity with the given primary key values wrapped in an Option.
    /// If no entity is found, then None is returned.
    /// </summary>
    /// <typeparam name="T">The type of entity to find.</typeparam>
    /// <param name="source">A DbSet&lt;T&gt; to find an entity in.</param>
    /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous find operation. The task result contains Some(T) with the entity found,
    /// or None if no entity with the given primary key values exists.
    /// </returns>
    public static async ValueTask<Option<T>> FindOrNoneAsync<T>(this DbSet<T> source,
        object[] keyValues,
        CancellationToken cancellationToken) where T : class
        => await source.FindAsync(keyValues, cancellationToken) is { } value
            ? Option<T>.Some(value)
            : Option<T>.None();
}