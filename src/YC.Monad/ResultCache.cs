namespace YC.Monad;

/// <summary>
/// Provides pre-built <see cref="Result"/> instances for commonly used errors.
/// </summary>
/// <remarks>
/// <see cref="Result"/> is a value type, so these fields exist for convenience/DRY rather than to avoid
/// allocation — constructing a <see cref="Result"/> directly (e.g. <see cref="Result.Success()"/>) is just
/// as cheap.
/// </remarks>
public static class ResultCache
{
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