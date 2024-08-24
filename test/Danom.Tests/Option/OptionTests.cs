namespace Danom.Tests;

using Xunit;

public sealed class OptionTests
{
    [Fact]
    public void SomeShouldWork()
    {
        var option = Option<int>.Some(1);
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
        Assert.Equal("Some(1)", option.ToString());
    }

    [Fact]
    public async Task SomeAsyncFromValueShouldWork()
    {
        var option = await Option<int>.SomeAsync(1);
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
        Assert.Equal("Some(1)", option.ToString());
    }

    [Fact]
    public async Task NoneAsyncShouldWork()
    {
        var option = await Option<int>.NoneAsync();
        AssertOption.IsNone(option);
        Assert.False(option.IsSome);
        Assert.Equal("None", option.ToString());
    }

    [Fact]
    public void NoneShouldWork()
    {
        var option = Option<int>.None();
        AssertOption.IsNone(option);
        Assert.False(option.IsSome);
        Assert.Equal("None", option.ToString());
    }

    [Fact]
    public void MatchShouldWork()
    {
        Assert.Equal(1,
            Option<int>.Some(1)
                .Match(x => x, () => -1));

        Assert.Equal(1,
            Option<int>.None()
                .Match(_ => -1, () => 1));
    }

    [Fact]
    public void BindShouldWork()
    {
        AssertOption.IsSome(2, Option<int>.Some(1).Bind(x => Option<int>.Some(x + 1)));
        AssertOption.IsNone(Option<int>.None().Bind(x => Option<int>.Some(x + 1)));
    }

    [Fact]
    public void MapShouldWork()
    {
        AssertOption.IsSome(2, Option<int>.Some(1).Map(x => x + 1));
        AssertOption.IsNone(Option<int>.None().Map(x => x + 1));
    }

    [Fact]
    public void DefaultValueShouldWork()
    {
        Assert.Equal(1, Option<int>.None().DefaultValue(1));
        Assert.Equal(2, Option<int>.Some(2).DefaultValue(1));
    }

    [Fact]
    public void DefaultWithShouldWork()
    {
        Assert.Equal(1, Option<int>.None().DefaultWith(() => 1));
        Assert.Equal(2, Option<int>.Some(2).DefaultWith(() => 1));
    }

    [Fact]
    public void OrElseShouldWork()
    {
        AssertOption.IsSome(1, Option<int>.None().OrElse(Option<int>.Some(1)));
        AssertOption.IsSome(2, Option<int>.Some(2).OrElse(Option<int>.Some(1)));
    }

    [Fact]
    public void OrElseWithShouldWork()
    {
        AssertOption.IsSome(1, Option<int>.None().OrElseWith(() => Option<int>.Some(1)));
        AssertOption.IsSome(2, Option<int>.Some(2).OrElseWith(() => Option<int>.Some(1)));
    }
}
