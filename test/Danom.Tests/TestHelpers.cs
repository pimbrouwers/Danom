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
