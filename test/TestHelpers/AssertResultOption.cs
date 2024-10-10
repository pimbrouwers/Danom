namespace Danom.TestHelpers;

using Danom;
using Xunit;

public static class AssertResultOption
{
    public static void IsOk<T, TError>(Func<T, bool> predicate, ResultOption<T, TError> result) =>
        Assert.True(result.Match(predicate, () => false, _ => false));

    public static void IsOk<T, TError>(T value, ResultOption<T, TError> result) =>
        IsOk(t => t is not null && t.Equals(value), result);

    public static void IsOk<T, TError>(ResultOption<T, TError> result) =>
        IsOk(_ => true, result);

    public static void IsNone<T, TError>(ResultOption<T, TError> result) =>
        Assert.True(result.IsNone);

    public static void IsError<T, TError>(ResultOption<T, TError> result) =>
        Assert.True(result.IsError);
}
