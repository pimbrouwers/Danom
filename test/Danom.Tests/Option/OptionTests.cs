namespace Danom.Tests;

using System.Globalization;
using Xunit;
using Danom.TestHelpers;

public sealed class OptionTests
{
    [Fact]
    public void Some()
    {
        var option = Option<int>.Some(1);
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
        Assert.Equal("Some(1)", option.ToString());
    }

    [Fact]
    public async Task SomeAsyncFromValue()
    {
        var option = await Option<int>.SomeAsync(1);
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
        Assert.Equal("Some(1)", option.ToString());
    }

    [Fact]
    public async Task SomeAsyncFromTask()
    {
        var option = await Option<int>.SomeAsync(Task<int>.Factory.StartNew(() => 1));
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
    }

    [Fact]
    public async Task SomeInferred()
    {
        var option = Option.Some(1);
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
        Assert.Equal("Some(1)", option.ToString());

        var optionTask = await Option.SomeAsync(Task<int>.Factory.StartNew(() => 1));
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
    }

    [Fact]
    public void None()
    {
        AssertOption.IsNone(Option<int>.NoneValue);
        var option = Option<int>.None();
        AssertOption.IsNone(option);
        Assert.False(option.IsSome);
        Assert.Equal("None", option.ToString());
    }

    [Fact]
    public void NullShouldProduceNone()
    {
        var option = Option<object>.Some(default!);
        AssertOption.IsNone(option);
        Assert.False(option.IsSome);
    }

    [Fact]
    public void Match()
    {
        Assert.Equal(1,
            Option<int>.Some(1)
                .Match(x => x, () => -1));

        Assert.Equal(1,
            Option<int>.None()
                .Match(_ => -1, () => 1));
    }

    [Fact]
    public void MatchAction()
    {
        var some = Option<int>.Some(1);
        var none = Option<int>.None();

        some.Match(
            some: x => Assert.Equal(1, x),
            none: () => Assert.True(false));

        none.Match(
            some: x => Assert.True(false),
            none: () => Assert.True(true));
    }

    [Fact]
    public void Bind()
    {
        AssertOption.IsSome(2, Option<int>.Some(1).Bind(x => Option<int>.Some(x + 1)));
        AssertOption.IsNone(Option<int>.None().Bind(x => Option<int>.Some(x + 1)));
    }

    [Fact]
    public void Map()
    {
        AssertOption.IsSome(2, Option<int>.Some(1).Map(x => x + 1));
        AssertOption.IsNone(Option<int>.None().Map(x => x + 1));
    }

    [Fact]
    public void DefaultValue()
    {
        Assert.Equal(1, Option<int>.None().DefaultValue(1));
        Assert.Equal(2, Option<int>.Some(2).DefaultValue(1));
    }

    [Fact]
    public void DefaultWith()
    {
        Assert.Equal(1, Option<int>.None().DefaultWith(() => 1));
        Assert.Equal(2, Option<int>.Some(2).DefaultWith(() => 1));
    }

    [Fact]
    public void OrElse()
    {
        AssertOption.IsSome(1, Option<int>.None().OrElse(Option<int>.Some(1)));
        AssertOption.IsSome(2, Option<int>.Some(2).OrElse(Option<int>.Some(1)));
    }

    [Fact]
    public void OrElseWith()
    {
        AssertOption.IsSome(1, Option<int>.None().OrElseWith(() => Option<int>.Some(1)));
        AssertOption.IsSome(2, Option<int>.Some(2).OrElseWith(() => Option<int>.Some(1)));
    }

    [Fact]
    public void TryGet()
    {
        if (Option<int>.Some(1).TryGet(out var x))
        {
            Assert.Equal(1, x);
        }
        else
        {
            Assert.Fail("Failed to TryGet Some(x)");
        }

        if (Option<int>.None().TryGet(out var y))
        {
            Assert.Fail("Should not succeed, value is None");
        }
    }

    [Fact]
    public void Equality()
    {
        Assert.Equal(Option<int>.None(), Option<int>.None());
        Assert.Equal(Option<int>.Some(1), Option<int>.Some(1));
        Assert.NotEqual(Option<int>.Some(1), Option<int>.Some(2));
        Assert.NotEqual(Option<int>.Some(1), Option<int>.None());
    }

    [Fact]
    public void GetHashCodeShouldBeZero()
    {
        Assert.Equal(0, Option<int>.None().GetHashCode());
    }

    [Fact]
    public void EqualityOperator()
    {
        Assert.True(Option<int>.None() == Option<int>.None());
        Assert.True(Option<int>.Some(1) == Option<int>.Some(1));
        Assert.False(Option<int>.Some(1) == Option<int>.Some(2));
        Assert.False(Option<int>.Some(1) == Option<int>.None());
    }

    [Fact]
    public void InequalityOperator()
    {
        Assert.False(Option<int>.None() != Option<int>.None());
        Assert.False(Option<int>.Some(1) != Option<int>.Some(1));
        Assert.True(Option<int>.Some(1) != Option<int>.Some(2));
        Assert.True(Option<int>.Some(1) != Option<int>.None());
    }

    [Fact]
    public void ToStringDefaultOrFormat()
    {
        Assert.Equal("1", Option<int>.Some(1).ToString("0"));
        Assert.Equal("0", Option<int>.None().ToString("0"));
        Assert.NotEqual("0", Option<decimal>.Some(1.9878565765675M).ToString("0", "C2"));
        Assert.Equal("£1.99", Option<decimal>.Some(1.9878565765675M).ToString("0", "C2", CultureInfo.CreateSpecificCulture("en-GB")));
    }

#pragma warning disable CS1718 // Comparison made to same variable
    [Fact]
    public void Comparability()
    {
        var some1 = Option<int>.Some(1);
        var some2 = Option<int>.Some(2);
        var none = Option<int>.None();

        Assert.True(some1 < some2);
        Assert.True(some1 <= some2);
        Assert.True(some2 > some1);
        Assert.True(some2 >= some1);
        Assert.True(some1 <= some1);
        Assert.True(some1 >= some1);

        Assert.True(none < some1);
        Assert.True(none <= some1);
        Assert.True(some1 > none);
        Assert.True(some1 >= none);
        Assert.True(none <= none);
        Assert.True(none >= none);
    }
}
#pragma warning restore CS1718 // Comparison made to same variable
