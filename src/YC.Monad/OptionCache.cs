namespace YC.Monad;

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