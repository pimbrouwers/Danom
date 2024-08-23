namespace Danom.Tests;

using Xunit;

public static class AssertOption
{
    public static void IsSome<T>(Func<T, bool> predicate, IOption<T> option) =>
        Assert.True(option.Match(predicate, () => false));

    public static void IsSome<T>(T value, IOption<T> option) =>
        IsSome(t => t is not null && t.Equals(value), option);

    public static void IsSome<T>(IOption<T> option) =>
        IsSome(_ => true, option);

    public static void IsNone<T>(IOption<T> option) =>
        Assert.True(option.IsNone);
}

public static class AssertResult
{
    public static void IsOk<T, TError>(Func<T, bool> predicate, IResult<T, TError> result) =>
        Assert.True(result.Match(predicate, _ => false));

    public static void IsOk<T, TError>(T value, IResult<T, TError> result) =>
        IsOk(t => t is not null && t.Equals(value), result);

    public static void IsOk<T, TError>(IResult<T, TError> result) =>
        IsOk(_ => true, result);

    public static void IsError<T, TError>(IResult<T, TError> result) =>
        Assert.True(result.IsError);
}
