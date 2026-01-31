#pragma warning disable CA1849
namespace Danom.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class OptionValueTaskTests {
    [Fact]
    public async Task Match_TaskDelegates() {
        var some = await new ValueTask<Option<int>>(Option<int>.Some(1))
            .MatchAsync(x => Task.FromResult(x + 1), () => Task.FromResult(-1));
        Assert.Equal(2, some);

        var none = await new ValueTask<Option<int>>(Option<int>.NoneValue)
            .MatchAsync(x => Task.FromResult(x + 1), () => Task.FromResult(-1));
        Assert.Equal(-1, none);
    }

    [Fact]
    public async Task Match_SyncDelegates() {
        var some = await new ValueTask<Option<int>>(Option<int>.Some(1))
            .MatchAsync(x => x + 1, () => -1);
        Assert.Equal(2, some);

        var none = await new ValueTask<Option<int>>(Option<int>.NoneValue)
            .MatchAsync(x => x + 1, () => -1);
        Assert.Equal(-1, none);
    }

    [Fact]
    public async Task Bind_TaskBind() {
        AssertOption.IsSome(2, await new ValueTask<Option<int>>(Option<int>.Some(1))
            .BindAsync(x => Task.FromResult(Option<int>.Some(x + 1))));
        AssertOption.IsNone(await new ValueTask<Option<int>>(Option<int>.NoneValue)
            .BindAsync(x => Task.FromResult(Option<int>.Some(x + 1))));
    }

    [Fact]
    public async Task Bind_SyncBind() {
        AssertOption.IsSome(2, await new ValueTask<Option<int>>(Option<int>.Some(1))
            .BindAsync(x => Option<int>.Some(x + 1)));
        AssertOption.IsNone(await new ValueTask<Option<int>>(Option<int>.NoneValue)
            .BindAsync(x => Option<int>.Some(x + 1)));
    }

    [Fact]
    public async Task Map_TaskMap() {
        AssertOption.IsSome(2, await new ValueTask<Option<int>>(Option<int>.Some(1))
            .MapAsync(x => Task.FromResult(x + 1)));
        AssertOption.IsNone(await new ValueTask<Option<int>>(Option<int>.NoneValue)
            .MapAsync(x => Task.FromResult(x + 1)));
    }

    [Fact]
    public async Task Map_SyncMap() {
        AssertOption.IsSome(2, await new ValueTask<Option<int>>(Option<int>.Some(1))
            .MapAsync(x => x + 1));
        AssertOption.IsNone(await new ValueTask<Option<int>>(Option<int>.NoneValue)
            .MapAsync(x => x + 1));
    }

    [Fact]
    public async Task DefaultValue_SyncDefaultValue() {
        Assert.Equal(5, await new ValueTask<Option<int>>(Option<int>.NoneValue)
            .DefaultValueAsync(5));
        Assert.Equal(7, await new ValueTask<Option<int>>(Option<int>.Some(7))
            .DefaultValueAsync(5));
    }

    [Fact]
    public async Task DefaultValue_TaskDefaultValue() {
        Assert.Equal(5, await new ValueTask<Option<int>>(Option<int>.NoneValue)
            .DefaultValueAsync(Task.FromResult(5)));
        Assert.Equal(7, await new ValueTask<Option<int>>(Option<int>.Some(7))
            .DefaultValueAsync(Task.FromResult(5)));
    }

    [Fact]
    public async Task DefaultWith_TaskFactory() {
        var called = false;
        Func<Task<int>> factory = async () => { called = true; return await Task.FromResult(5); };

        Assert.Equal(5, await new ValueTask<Option<int>>(Option<int>.NoneValue)
            .DefaultWithAsync(factory));
        Assert.True(called);

        called = false;
        Assert.Equal(3, await new ValueTask<Option<int>>(Option<int>.Some(3))
            .DefaultWithAsync(factory));
        Assert.False(called);
    }

    [Fact]
    public async Task DefaultWith_SyncFactory() {
        var called = false;
        Func<int> factory = () => { called = true; return 5; };

        Assert.Equal(5, await new ValueTask<Option<int>>(Option<int>.NoneValue)
            .DefaultWithAsync(factory));
        Assert.True(called);

        called = false;
        Assert.Equal(3, await new ValueTask<Option<int>>(Option<int>.Some(3))
            .DefaultWithAsync(factory));
        Assert.False(called);
    }

    [Fact]
    public async Task OrElse_TaskIfNone() {
        AssertOption.IsSome(5, await new ValueTask<Option<int>>(Option<int>.NoneValue)
            .OrElseAsync(Task.FromResult(Option<int>.Some(5))));
        AssertOption.IsSome(3, await new ValueTask<Option<int>>(Option<int>.Some(3))
            .OrElseAsync(Task.FromResult(Option<int>.Some(5))));
    }

    [Fact]
    public async Task OrElse_SyncIfNone() {
        AssertOption.IsSome(5, await new ValueTask<Option<int>>(Option<int>.NoneValue)
            .OrElseAsync(Option<int>.Some(5)));
        AssertOption.IsSome(3, await new ValueTask<Option<int>>(Option<int>.Some(3))
            .OrElseAsync(Option<int>.Some(5)));
    }

    [Fact]
    public async Task OrElseWith_TaskFactory() {
        AssertOption.IsSome(5, await new ValueTask<Option<int>>(Option<int>.NoneValue)
            .OrElseWithAsync(() => Task.FromResult(Option<int>.Some(5))));
        AssertOption.IsSome(3, await new ValueTask<Option<int>>(Option<int>.Some(3))
            .OrElseWithAsync(() => Task.FromResult(Option<int>.Some(5))));
    }

    [Fact]
    public async Task OrElseWith_SyncFactory() {
        AssertOption.IsSome(5, await new ValueTask<Option<int>>(Option<int>.NoneValue)
            .OrElseWithAsync(() => Option<int>.Some(5)));
        AssertOption.IsSome(3, await new ValueTask<Option<int>>(Option<int>.Some(3))
            .OrElseWithAsync(() => Option<int>.Some(5)));
    }

    [Fact]
    public async Task ToOptionAsync_ValueTask_Value() {
        var some = await new ValueTask<int>(42).ToOptionAsync();
        AssertOption.IsSome(42, some);
    }

    [Fact]
    public async Task ToOptionAsync_ValueTask_NullableValue() {
        var some = await new ValueTask<int?>(42).ToOptionAsync();
        AssertOption.IsSome(42, some);

        var none = await new ValueTask<int?>((int?)null).ToOptionAsync();
        AssertOption.IsNone(none);
    }

    [Fact]
    public async Task ToOptionAsync_ValueTask_String() {
        var some = await new ValueTask<string?>("abc").ToOptionAsync();
        AssertOption.IsSome("abc", some);

        var none = await new ValueTask<string?>((string?)null).ToOptionAsync();
        AssertOption.IsNone(none);
    }
}

#pragma warning restore CA1849
