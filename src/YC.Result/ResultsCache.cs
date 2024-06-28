namespace YC.Result;

internal static class ResultsCache
{
    internal static Result Success { get; } = new(true);
    internal static Result Failure { get; } = new(false);
}