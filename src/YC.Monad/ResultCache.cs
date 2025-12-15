namespace YC.Monad;

/// <summary>
/// Provides cached instances of commonly used Result objects.
/// </summary>
public static class ResultCache
{
    /// <summary>
    /// A cached successful result.
    /// </summary>
    internal static readonly Result Success = new(true);

    /// <summary>
    /// A cached failed result.
    /// </summary>
    internal static readonly Result Failure = new(false);

    /// <summary>
    /// A cached unauthorized error result.
    /// </summary>
    public static readonly Result Unauthorized = Result.Failure(ErrorCache.Unauthorized);

    /// <summary>
    /// A cached bad request error result.
    /// </summary>
    public static readonly Result BadRequest = Result.Failure(ErrorCache.BadRequest);

    /// <summary>
    /// A cached not found error result.
    /// </summary>
    public static readonly Result NotFound = Result.Failure(ErrorCache.NotFound);

    /// <summary>
    /// A cached forbidden error result.
    /// </summary>
    public static readonly Result Forbidden = Result.Failure(ErrorCache.Forbidden);
}