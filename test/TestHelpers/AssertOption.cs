namespace Danom.TestHelpers;

using Danom;
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
