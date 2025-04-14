namespace Danom.Tests;

using System.Globalization;
using Xunit;
using Danom.TestHelpers;

public sealed class OptionTests
{
    [Fact]
    public void ToStringOverloadsShouldWork()
    {
        Assert.Equal("1", Option<int>.Some(1).ToString(""));
        Assert.Equal("Here", Option<int>.NoneValue.ToString("Here"));
        Assert.Equal("12345.68", Option<double>.Some(12345.6789).ToString("", "F2"));
        Assert.Equal("1967-01-01", Option<DateTime>.Some(new DateTime(1967, 1, 1)).ToString("", "yyyy-MM-dd"));

        var culture = new CultureInfo("en-US");
        Assert.Equal("1967-01-01", Option<DateTime>.Some(new DateTime(1967, 1, 1)).ToString("", "yyyy-MM-dd", culture));
    }

    [Fact]
    public void SomeShouldWork()
    {
        var option = Option<int>.Some(1);
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
        Assert.Equal("Some(1)", option.ToString());
    }

    [Fact]
    public async Task SomeAsyncFromTaskShouldWork()
    {
        var option = await Option<int>.SomeAsync(Task<int>.Factory.StartNew(() => 1));
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);

        var option2 = await Option.SomeAsync(Task<int>.Factory.StartNew(() => 1));
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
    }

    [Fact]
    public void NoneShouldWork()
    {
        AssertOption.IsNone(Option<int>.NoneValue);
        var option = Option<int>.NoneValue;
        AssertOption.IsNone(option);
        Assert.False(option.IsSome);
        Assert.Equal("None", option.ToString());

        var option2 = Option<Unit>.NoneValue;
        Assert.Equal(Option<Unit>.NoneValue, option2);
    }

    [Fact]
    public void NullShouldProduceNone()
    {
        var option = Option<object>.Some(default!);
        AssertOption.IsNone(option);
        Assert.False(option.IsSome);
    }

    [Fact]
    public void MatchShouldWork()
    {
        Assert.Equal(1,
            Option<int>.Some(1)
                .Match(x => x, () => -1));

        Assert.Equal(1,
            Option<int>.NoneValue
                .Match(_ => -1, () => 1));
    }

    [Fact]
    public void MatchActionShouldWork()
    {
        Option<int>.Some(1).Match(
            some: x => Assert.Equal(1, x),
            none: () => Assert.True(false));

        Option<int>.NoneValue.Match(
            some: x => Assert.True(false),
            none: () => Assert.True(true));
    }

    [Fact]
    public void IterActionShouldWork()
    {
        var result = 0;
        Option<int>.Some(1).Iter(
            x => result += x);
        Assert.Equal(1, result);

        result = 0;
        Option<int>.NoneValue.Iter(
            x => result += x);
        Assert.Equal(0, result);
    }

    [Fact]
    public void BindShouldWork()
    {
        AssertOption.IsSome(2, Option<int>.Some(1).Bind(x => Option<int>.Some(x + 1)));
        AssertOption.IsNone(Option<int>.NoneValue.Bind(x => Option<int>.Some(x + 1)));
    }

    [Fact]
    public void MapShouldWork()
    {
        AssertOption.IsSome(2, Option<int>.Some(1).Map(x => x + 1));
        AssertOption.IsNone(Option<int>.NoneValue.Map(x => x + 1));
    }

    [Fact]
    public void DefaultValueShouldWork()
    {
        Assert.Equal(1, Option<int>.NoneValue.DefaultValue(1));
        Assert.Equal(2, Option<int>.Some(2).DefaultValue(1));
    }

    [Fact]
    public void DefaultWithShouldWork()
    {
        Assert.Equal(1, Option<int>.NoneValue.DefaultWith(() => 1));
        Assert.Equal(2, Option<int>.Some(2).DefaultWith(() => 1));
    }

    [Fact]
    public void OrElseShouldWork()
    {
        AssertOption.IsSome(1, Option<int>.NoneValue.OrElse(Option<int>.Some(1)));
        AssertOption.IsSome(2, Option<int>.Some(2).OrElse(Option<int>.Some(1)));
    }

    [Fact]
    public void OrElseWithShouldWork()
    {
        AssertOption.IsSome(1, Option<int>.NoneValue.OrElseWith(() => Option<int>.Some(1)));
        AssertOption.IsSome(2, Option<int>.Some(2).OrElseWith(() => Option<int>.Some(1)));
    }

    [Fact]
    public void TryGetShouldWork()
    {
        if (Option<int>.Some(1).TryGet(out var x))
        {
            Assert.Equal(1, x);
        }
        else
        {
            Assert.Fail("Failed to TryGet a valid Option");
        }
    }

    [Fact]
    public void EqualityShouldWork()
    {
        Assert.Equal(Option<int>.NoneValue, Option<int>.NoneValue);
        Assert.Equal(Option<int>.Some(1), Option<int>.Some(1));
        Assert.NotEqual(Option<int>.Some(1), Option<int>.Some(2));
        Assert.NotEqual(Option<int>.Some(1), Option<int>.NoneValue);
    }

    [Fact]
    public void GetHashCodeShouldBeZero()
    {
        Assert.Equal(0, Option<int>.NoneValue.GetHashCode());
    }

    [Fact]
    public void EqualityOperatorShouldWork()
    {
        Assert.True(Option<int>.NoneValue == Option<int>.NoneValue);
        Assert.True(Option<int>.Some(1) == Option<int>.Some(1));
        Assert.False(Option<int>.Some(1) == Option<int>.Some(2));
        Assert.False(Option<int>.Some(1) == Option<int>.NoneValue);
    }

    [Fact]
    public void InequalityOperatorShouldWork()
    {
        Assert.False(Option<int>.NoneValue != Option<int>.NoneValue);
        Assert.False(Option<int>.Some(1) != Option<int>.Some(1));
        Assert.True(Option<int>.Some(1) != Option<int>.Some(2));
        Assert.True(Option<int>.Some(1) != Option<int>.NoneValue);
    }
}
