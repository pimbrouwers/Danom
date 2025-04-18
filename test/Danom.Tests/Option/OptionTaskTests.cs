namespace Danom.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class OptionAsyncTests
{
    [Fact]
    public async Task Match()
    {
        var optionSome = await Option<int>.SomeAsync(1).MatchAsync(x => x, () => -1);
        Assert.Equal(1, optionSome);

        var optionNone = await Task.FromResult(Option<int>.NoneValue).MatchAsync(_ => -1, () => 1);
        Assert.Equal(1, optionNone);
    }

    [Fact]
    public async Task Bind()
    {
        AssertOption.IsSome(2, await Option<int>.SomeAsync(1).BindAsync(x => Option<int>.Some(x + 1)));
        AssertOption.IsSome(2, await Option<int>.SomeAsync(1).BindAsync(x => Option<int>.SomeAsync(x + 1)));
        AssertOption.IsNone(await Task.FromResult(Option<int>.NoneValue).BindAsync(x => Option<int>.Some(x + 1)));
        AssertOption.IsNone(await Task.FromResult(Option<int>.NoneValue).BindAsync(x => Option<int>.SomeAsync(x + 1)));
    }

    [Fact]
    public async Task Map()
    {
        AssertOption.IsSome(2, await Option<int>.SomeAsync(1).MapAsync(x => x + 1));
        AssertOption.IsSome(2, await Option<int>.SomeAsync(1).MapAsync(x => Task.FromResult(x + 1)));
        AssertOption.IsNone(await Task.FromResult(Option<int>.NoneValue).MapAsync(x => x + 1));
        AssertOption.IsNone(await Task.FromResult(Option<int>.NoneValue).MapAsync(x => Task.FromResult(x + 1)));
    }

    [Fact]
    public async Task DefaultValue()
    {
        Assert.Equal(1, await Task.FromResult(Option<int>.NoneValue).DefaultValueAsync(1));
        Assert.Equal(1, await Task.FromResult(Option<int>.NoneValue).DefaultValueAsync(Task.FromResult(1)));
        Assert.Equal(2, await Option<int>.SomeAsync(2).DefaultValueAsync(1));
        Assert.Equal(2, await Option<int>.SomeAsync(2).DefaultValueAsync(Task.FromResult(1)));
    }

    [Fact]
    public async Task DefaultWith()
    {
        Assert.Equal(1, await Task.FromResult(Option<int>.NoneValue).DefaultWithAsync(() => 1));
        Assert.Equal(1, await Task.FromResult(Option<int>.NoneValue).DefaultWithAsync(() => Task.FromResult(1)));
        Assert.Equal(2, await Option<int>.SomeAsync(2).DefaultWithAsync(() => 1));
        Assert.Equal(2, await Option<int>.SomeAsync(2).DefaultWithAsync(() => Task.FromResult(1)));
    }

    [Fact]
    public async Task OrElse()
    {
        AssertOption.IsSome(1, await Task.FromResult(Option<int>.NoneValue).OrElseAsync(Option<int>.Some(1)));
        AssertOption.IsSome(1, await Task.FromResult(Option<int>.NoneValue).OrElseAsync(Option<int>.SomeAsync(1)));
        AssertOption.IsSome(2, await Option<int>.SomeAsync(2).OrElseAsync(Option<int>.Some(1)));
        AssertOption.IsSome(2, await Option<int>.SomeAsync(2).OrElseAsync(Option<int>.SomeAsync(1)));
    }

    [Fact]
    public async Task OrElseWith()
    {
        AssertOption.IsSome(1, await Task.FromResult(Option<int>.NoneValue).OrElseWithAsync(() => Option<int>.Some(1)));
        AssertOption.IsSome(1, await Task.FromResult(Option<int>.NoneValue).OrElseWithAsync(() => Option<int>.SomeAsync(1)));
        AssertOption.IsSome(2, await Option<int>.SomeAsync(2).OrElseWithAsync(() => Option<int>.Some(1)));
        AssertOption.IsSome(2, await Option<int>.SomeAsync(2).OrElseWithAsync(() => Option<int>.SomeAsync(1)));
    }
}
