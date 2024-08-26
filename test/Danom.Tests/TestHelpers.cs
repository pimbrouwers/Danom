namespace Danom.Tests;

using Xunit;

public static class AssertOption
{
    public static void IsSome<T>(Func<T, bool> predicate, Option<T> option) =>
        Assert.True(option.Match(predicate, () => false));

    public static void IsSome<T>(T value, Option<T> option) =>
        IsSome(t => t is not null && t.Equals(value), option);

    public static void IsSome<T>(Option<T> option) =>
        IsSome(_ => true, option);

    public static void IsNone<T>(Option<T> option) =>
        Assert.True(option.IsNone);
}

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
