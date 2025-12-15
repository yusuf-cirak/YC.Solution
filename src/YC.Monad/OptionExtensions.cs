namespace YC.Monad;

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
}