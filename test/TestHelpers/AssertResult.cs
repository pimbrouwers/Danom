namespace Danom.TestHelpers;

using Danom;
using Xunit;

public static class AssertResult
{
    public static void IsOk<T, TError>(Func<T, bool> predicate, Result<T, TError> result) =>
        Assert.True(result.Match(predicate, _ => false));

    public static void IsOk<T, TError>(T value, Result<T, TError> result) =>
        IsOk(t => t is not null && t.Equals(value), result);

    public static void IsOk<T, TError>(Result<T, TError> result) =>
        IsOk(_ => true, result);

    public static void IsError<T, TError>(Result<T, TError> result) =>
        Assert.True(result.IsError);
}
