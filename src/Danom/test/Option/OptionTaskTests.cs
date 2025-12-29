namespace Danom.Tests;

using Danom.TestHelpers;
using Xunit;

#pragma warning disable CA1849
public sealed class OptionTaskTests {
    [Fact]
    public async Task Match_TaskDelegates() {
        var some = await Task.FromResult(Option<int>.Some(1))
            .MatchAsync(x => Task.FromResult(x + 1), () => Task.FromResult(-1));
        Assert.Equal(2, some);

        var none = await Task.FromResult(Option<int>.NoneValue)
            .MatchAsync(x => Task.FromResult(x + 1), () => Task.FromResult(-1));
        Assert.Equal(-1, none);
    }

    [Fact]
    public async Task Match_SyncDelegates() {
        var some = await Task.FromResult(Option<int>.Some(1))
            .MatchAsync(x => x + 1, () => -1);
        Assert.Equal(2, some);

        var none = await Task.FromResult(Option<int>.NoneValue)
            .MatchAsync(x => x + 1, () => -1);
        Assert.Equal(-1, none);
    }

    [Fact]
    public async Task Bind_TaskBind() {
        AssertOption.IsSome(2, await Task.FromResult(Option<int>.Some(1))
            .BindAsync(x => Task.FromResult(Option<int>.Some(x + 1))));
        AssertOption.IsNone(await Task.FromResult(Option<int>.NoneValue)
            .BindAsync(x => Task.FromResult(Option<int>.Some(x + 1))));
    }

    [Fact]
    public async Task Bind_SyncBind() {
        AssertOption.IsSome(2, await Task.FromResult(Option<int>.Some(1))
            .BindAsync(x => Option<int>.Some(x + 1)));
        AssertOption.IsNone(await Task.FromResult(Option<int>.NoneValue)
            .BindAsync(x => Option<int>.Some(x + 1)));
    }

    [Fact]
    public async Task Bind_OnOption_DelegatesToTaskOverload() {
        AssertOption.IsSome(3, await Option<int>.Some(2)
            .BindAsync(x => Task.FromResult(Option<int>.Some(x + 1))));
        AssertOption.IsNone(await Option<int>.NoneValue
            .BindAsync(x => Task.FromResult(Option<int>.Some(x + 1))));
    }

    [Fact]
    public async Task Map_TaskMap() {
        AssertOption.IsSome(2, await Task.FromResult(Option<int>.Some(1))
            .MapAsync(x => Task.FromResult(x + 1)));
        AssertOption.IsNone(await Task.FromResult(Option<int>.NoneValue)
            .MapAsync(x => Task.FromResult(x + 1)));
    }

    [Fact]
    public async Task Map_SyncMap() {
        AssertOption.IsSome(2, await Task.FromResult(Option<int>.Some(1))
            .MapAsync(x => x + 1));
        AssertOption.IsNone(await Task.FromResult(Option<int>.NoneValue)
            .MapAsync(x => x + 1));
    }

    [Fact]
    public async Task Map_OnOption_DelegatesToTaskOverload() {
        AssertOption.IsSome(4, await Option<int>.Some(2)
            .MapAsync(x => Task.FromResult(x * 2)));
        AssertOption.IsNone(await Option<int>.NoneValue
            .MapAsync(x => Task.FromResult(x * 2)));
    }

    [Fact]
    public async Task DefaultValue_TaskDefaultValue_Sync() {
        Assert.Equal(5, await Task.FromResult(Option<int>.NoneValue)
            .DefaultValueAsync(5));
        Assert.Equal(7, await Task.FromResult(Option<int>.Some(7))
            .DefaultValueAsync(5));
    }

    [Fact]
    public async Task DefaultValue_TaskDefaultValue_Task() {
        Assert.Equal(5, await Task.FromResult(Option<int>.NoneValue)
            .DefaultValueAsync(Task.FromResult(5)));
        Assert.Equal(7, await Task.FromResult(Option<int>.Some(7))
            .DefaultValueAsync(Task.FromResult(5)));
    }

    [Fact]
    public async Task DefaultValue_OnOption_TaskDefault() {
        Assert.Equal(3, await Option<int>.NoneValue
            .DefaultValueAsync(Task.FromResult(3)));
        Assert.Equal(9, await Option<int>.Some(9)
            .DefaultValueAsync(Task.FromResult(3)));
    }

    [Fact]
    public async Task DefaultWith_TaskFactory_Task() {
        var called = false;
        Func<Task<int>> factory = async () => { called = true; return await Task.FromResult(5); };

        Assert.Equal(5, await Task.FromResult(Option<int>.NoneValue)
            .DefaultWithAsync(factory));
        Assert.True(called);

        called = false;
        Assert.Equal(3, await Task.FromResult(Option<int>.Some(3))
            .DefaultWithAsync(factory));
        Assert.False(called);
    }

    [Fact]
    public async Task DefaultWith_TaskFactory_Sync() {
        var called = false;
        Func<int> factory = () => { called = true; return 5; };

        Assert.Equal(5, await Task.FromResult(Option<int>.NoneValue)
            .DefaultWithAsync(factory));
        Assert.True(called);

        called = false;
        Assert.Equal(3, await Task.FromResult(Option<int>.Some(3))
            .DefaultWithAsync(factory));
        Assert.False(called);
    }

    [Fact]
    public async Task DefaultWith_OnOption_TaskFactory() {
        var called = false;
        Func<Task<int>> factory = () => { called = true; return Task.FromResult(6); };

        Assert.Equal(6, await Option<int>.NoneValue.DefaultWithAsync(factory));
        Assert.True(called);

        called = false;
        Assert.Equal(2, await Option<int>.Some(2).DefaultWithAsync(factory));
        Assert.False(called);
    }

    [Fact]
    public async Task OrElse_TaskIfNone() {
        AssertOption.IsSome(5, await Task.FromResult(Option<int>.NoneValue)
            .OrElseAsync(Task.FromResult(Option<int>.Some(5))));
        AssertOption.IsSome(3, await Task.FromResult(Option<int>.Some(3))
            .OrElseAsync(Task.FromResult(Option<int>.Some(5))));
    }

    [Fact]
    public async Task OrElse_SyncIfNone() {
        AssertOption.IsSome(5, await Task.FromResult(Option<int>.NoneValue)
            .OrElseAsync(Option<int>.Some(5)));
        AssertOption.IsSome(3, await Task.FromResult(Option<int>.Some(3))
            .OrElseAsync(Option<int>.Some(5)));
    }

    [Fact]
    public async Task OrElse_OnOption_TaskIfNone() {
        AssertOption.IsSome(8, await Option<int>.NoneValue
            .OrElseAsync(Task.FromResult(Option<int>.Some(8))));
        AssertOption.IsSome(4, await Option<int>.Some(4)
            .OrElseAsync(Task.FromResult(Option<int>.Some(8))));
    }

    [Fact]
    public async Task OrElseWith_TaskIfNoneWith_TaskFactory() {
        AssertOption.IsSome(5, await Task.FromResult(Option<int>.NoneValue)
            .OrElseWithAsync(() => Task.FromResult(Option<int>.Some(5))));
        AssertOption.IsSome(3, await Task.FromResult(Option<int>.Some(3))
            .OrElseWithAsync(() => Task.FromResult(Option<int>.Some(5))));
    }

    [Fact]
    public async Task OrElseWith_TaskIfNoneWith_SyncFactory() {
        AssertOption.IsSome(5, await Task.FromResult(Option<int>.NoneValue)
            .OrElseWithAsync(() => Option<int>.Some(5)));
        AssertOption.IsSome(3, await Task.FromResult(Option<int>.Some(3))
            .OrElseWithAsync(() => Option<int>.Some(5)));
    }

    [Fact]
    public async Task OrElseWith_OnOption_TaskFactory() {
        AssertOption.IsSome(7, await Option<int>.NoneValue
            .OrElseWithAsync(() => Task.FromResult(Option<int>.Some(7))));
        AssertOption.IsSome(2, await Option<int>.Some(2)
            .OrElseWithAsync(() => Task.FromResult(Option<int>.Some(7))));
    }

    [Fact]
    public async Task ToOptionAsync_FromTask_Value() {
        var some = await Task.FromResult(42).ToOptionAsync();
        AssertOption.IsSome(42, some);
    }

    [Fact]
    public async Task ToOptionAsync_FromTask_NullableValue() {
        var some = await Task.FromResult<int?>(42).ToOptionAsync();
        AssertOption.IsSome(42, some);

        var none = await Task.FromResult<int?>(null).ToOptionAsync();
        AssertOption.IsNone(none);
    }

    [Fact]
    public async Task ToOptionAsync_FromTask_String() {
        var some = await Task.FromResult<string?>("abc").ToOptionAsync();
        AssertOption.IsSome("abc", some);

        var none = await Task.FromResult<string?>(null).ToOptionAsync();
        AssertOption.IsNone(none);
    }
}

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
