namespace Danom.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class ResultTaskTests {
    [Fact]
    public async Task Match_TaskDelegates() {
        var ok = await Task.FromResult(Result<int, string>.Ok(1))
            .MatchAsync(x => Task.FromResult(x), _ => Task.FromResult(-1));
        Assert.Equal(1, ok);

        var err = await Task.FromResult(Result<int, string>.Error("e"))
            .MatchAsync(_ => Task.FromResult(-1), _ => Task.FromResult(1));
        Assert.Equal(1, err);
    }

    [Fact]
    public async Task Bind_TaskBind() {
        AssertResult.IsOk(2, await Task.FromResult(Result<int, string>.Ok(1))
            .BindAsync(x => Result<int, string>.OkAsync(Task.FromResult(x + 1))));
        AssertResult.IsError(await Task.FromResult(Result<int, string>.Error("e"))
            .BindAsync(x => Result<int, string>.OkAsync(Task.FromResult(x + 1))));
    }

    [Fact]
    public async Task Map_TaskMap() {
        AssertResult.IsOk(2, await Task.FromResult(Result<int, string>.Ok(1))
            .MapAsync(x => Task.FromResult(x + 1)));
        AssertResult.IsError(await Task.FromResult(Result<int, string>.Error("e"))
            .MapAsync(x => Task.FromResult(x + 1)));
    }

    [Fact]
    public async Task MapError_TaskMapError() {
        var ok = await Task.FromResult(Result<int, string>.Ok(1))
            .MapErrorAsync(e => Task.FromResult(e.ToUpperInvariant()));
        AssertResult.IsOk(ok);

        var err = await Task.FromResult(Result<int, string>.Error("bad"))
            .MapErrorAsync(e => Task.FromResult(e.ToUpperInvariant()));
        AssertResult.IsError(err);
        err.Match(_ => Assert.Fail("Expected error"), e => Assert.Equal("BAD", e));
    }

    [Fact]
    public async Task DefaultValue_TaskDefault() {
        Assert.Equal(1, await Task.FromResult(Result<int, string>.Error("e"))
            .DefaultValueAsync(Task.FromResult(1)));
        Assert.Equal(2, await Task.FromResult(Result<int, string>.Ok(2))
            .DefaultValueAsync(Task.FromResult(1)));
    }

    [Fact]
    public async Task DefaultWith_TaskFactory() {
        Assert.Equal(1, await Task.FromResult(Result<int, string>.Error("e"))
            .DefaultWithAsync(() => Task.FromResult(1)));
        Assert.Equal(2, await Task.FromResult(Result<int, string>.Ok(2))
            .DefaultWithAsync(() => Task.FromResult(1)));
    }
}

public sealed class ResultValueTaskTests {
    [Fact]
    public async Task Match_ValueTask_Source_SyncDelegates() {
        var ok = await new ValueTask<Result<int, string>>(Result<int, string>.Ok(1))
            .MatchAsync(x => x, _ => -1);
        Assert.Equal(1, ok);

        var err = await new ValueTask<Result<int, string>>(Result<int, string>.Error("e"))
            .MatchAsync(_ => -1, _ => 1);
        Assert.Equal(1, err);
    }

    [Fact]
    public async Task Match_ValueTask_Source_TaskDelegates() {
        var ok = await new ValueTask<Result<int, string>>(Result<int, string>.Ok(1))
            .MatchAsync(x => Task.FromResult(x), _ => Task.FromResult(-1));
        Assert.Equal(1, ok);

        var err = await new ValueTask<Result<int, string>>(Result<int, string>.Error("e"))
            .MatchAsync(_ => Task.FromResult(-1), _ => Task.FromResult(1));
        Assert.Equal(1, err);
    }

    [Fact]
    public async Task Match_ValueTask_Source_ValueTaskDelegates() {
        var ok = await new ValueTask<Result<int, string>>(Result<int, string>.Ok(1))
            .MatchAsync(x => new ValueTask<int>(x), _ => new ValueTask<int>(-1));
        Assert.Equal(1, ok);

        var err = await new ValueTask<Result<int, string>>(Result<int, string>.Error("e"))
            .MatchAsync(_ => new ValueTask<int>(-1), _ => new ValueTask<int>(1));
        Assert.Equal(1, err);
    }

    [Fact]
    public async Task Bind_ValueTask_Source_SyncBind() {
        AssertResult.IsOk(2, await new ValueTask<Result<int, string>>(Result<int, string>.Ok(1))
            .BindAsync(x => Result<int, string>.Ok(x + 1)));
        AssertResult.IsError(await new ValueTask<Result<int, string>>(Result<int, string>.Error("e"))
            .BindAsync(x => Result<int, string>.Ok(x + 1)));
    }

    [Fact]
    public async Task Bind_ValueTask_Source_TaskBind() {
        AssertResult.IsOk(2, await new ValueTask<Result<int, string>>(Result<int, string>.Ok(1))
            .BindAsync(x => Result<int, string>.OkAsync(Task.FromResult(x + 1))));
        AssertResult.IsError(await new ValueTask<Result<int, string>>(Result<int, string>.Error("e"))
            .BindAsync(x => Result<int, string>.OkAsync(Task.FromResult(x + 1))));
    }

    [Fact]
    public async Task Bind_Result_Source_ValueTaskBind() {
        var ok = Result<int, string>.Ok(1);
        var err = Result<int, string>.Error("e");

        AssertResult.IsOk(2, await ok.BindAsync(x => new ValueTask<Result<int, string>>(Result<int, string>.Ok(x + 1))));
        AssertResult.IsError(await err.BindAsync(x => new ValueTask<Result<int, string>>(Result<int, string>.Ok(x + 1))));
    }

    [Fact]
    public async Task Map_ValueTask_Source_SyncMap() {
        AssertResult.IsOk(2, await new ValueTask<Result<int, string>>(Result<int, string>.Ok(1))
            .MapAsync(x => x + 1));
        AssertResult.IsError(await new ValueTask<Result<int, string>>(Result<int, string>.Error("e"))
            .MapAsync(x => x + 1));
    }

    [Fact]
    public async Task Map_ValueTask_Source_TaskMap() {
        AssertResult.IsOk(2, await new ValueTask<Result<int, string>>(Result<int, string>.Ok(1))
            .MapAsync(x => Task.FromResult(x + 1)));
        AssertResult.IsError(await new ValueTask<Result<int, string>>(Result<int, string>.Error("e"))
            .MapAsync(x => Task.FromResult(x + 1)));
    }

    [Fact]
    public async Task Map_Result_Source_ValueTaskMap() {
        var ok = Result<int, string>.Ok(1);
        var err = Result<int, string>.Error("e");

        AssertResult.IsOk(2, await ok.MapAsync(x => new ValueTask<int>(x + 1)));
        AssertResult.IsError(await err.MapAsync(x => new ValueTask<int>(x + 1)));
    }

    [Fact]
    public async Task MapError_ValueTask_Source_SyncMapError() {
        var ok = await new ValueTask<Result<int, string>>(Result<int, string>.Ok(1))
            .MapErrorAsync(e => e.ToUpperInvariant());
        AssertResult.IsOk(ok);

        var err = await new ValueTask<Result<int, string>>(Result<int, string>.Error("bad"))
            .MapErrorAsync(e => e.ToUpperInvariant());
        AssertResult.IsError(err);
        err.Match(_ => Assert.Fail("Expected error"), e => Assert.Equal("BAD", e));
    }

    [Fact]
    public async Task MapError_ValueTask_Source_TaskMapError() {
        var ok = await new ValueTask<Result<int, string>>(Result<int, string>.Ok(1))
            .MapErrorAsync(e => Task.FromResult(e.ToUpperInvariant()));
        AssertResult.IsOk(ok);

        var err = await new ValueTask<Result<int, string>>(Result<int, string>.Error("bad"))
            .MapErrorAsync(e => Task.FromResult(e.ToUpperInvariant()));
        AssertResult.IsError(err);
        err.Match(_ => Assert.Fail("Expected error"), e => Assert.Equal("BAD", e));
    }

    [Fact]
    public async Task MapError_Result_Source_ValueTaskMapError() {
        var ok = Result<int, string>.Ok(1);
        var err = Result<int, string>.Error("bad");

        AssertResult.IsOk(await ok.MapErrorAsync(e => new ValueTask<string>(e.ToUpperInvariant())));
        var mappedErr = await err.MapErrorAsync(e => new ValueTask<string>(e.ToUpperInvariant()));
        AssertResult.IsError(mappedErr);
        mappedErr.Match(_ => Assert.Fail("Expected error"), e => Assert.Equal("BAD", e));
    }

    [Fact]
    public async Task DefaultValue_ValueTask_Source_SyncDefault() {
        Assert.Equal(1, await new ValueTask<Result<int, string>>(Result<int, string>.Error("e"))
            .DefaultValueAsync(1));
        Assert.Equal(2, await new ValueTask<Result<int, string>>(Result<int, string>.Ok(2))
            .DefaultValueAsync(1));
    }

    [Fact]
    public async Task DefaultValue_ValueTask_Source_TaskDefault() {
        Assert.Equal(1, await new ValueTask<Result<int, string>>(Result<int, string>.Error("e"))
            .DefaultValueAsync(Task.FromResult(1)));
        Assert.Equal(2, await new ValueTask<Result<int, string>>(Result<int, string>.Ok(2))
            .DefaultValueAsync(Task.FromResult(1)));
    }

    [Fact]
    public async Task DefaultValue_ValueTask_Source_ValueTaskDefault() {
        Assert.Equal(1, await new ValueTask<Result<int, string>>(Result<int, string>.Error("e"))
            .DefaultValueAsync(new ValueTask<int>(1)));
        Assert.Equal(2, await new ValueTask<Result<int, string>>(Result<int, string>.Ok(2))
            .DefaultValueAsync(new ValueTask<int>(1)));
    }

    [Fact]
    public async Task DefaultWith_ValueTask_Source_TaskFactory() {
        Assert.Equal(1, await new ValueTask<Result<int, string>>(Result<int, string>.Error("e"))
            .DefaultWithAsync(() => Task.FromResult(1)));
        Assert.Equal(2, await new ValueTask<Result<int, string>>(Result<int, string>.Ok(2))
            .DefaultWithAsync(() => Task.FromResult(1)));
    }

    [Fact]
    public async Task DefaultWith_ValueTask_Source_SyncFactory() {
        Assert.Equal(1, await new ValueTask<Result<int, string>>(Result<int, string>.Error("e"))
            .DefaultWithAsync(() => 1));
        Assert.Equal(2, await new ValueTask<Result<int, string>>(Result<int, string>.Ok(2))
            .DefaultWithAsync(() => 1));
    }

    [Fact]
    public async Task DefaultWith_Result_Source_ValueTaskFactory() {
        var ok = Result<int, string>.Ok(2);
        var err = Result<int, string>.Error("e");

        Assert.Equal(1, await err.DefaultWithAsync(() => new ValueTask<int>(1)));
        Assert.Equal(2, await ok.DefaultWithAsync(() => new ValueTask<int>(1)));
    }
}
