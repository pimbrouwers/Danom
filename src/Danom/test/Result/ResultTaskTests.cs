namespace Danom.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class ResultTaskTests
{
    [Fact]
    public async Task Match()
    {
        var resultOk = await Task.FromResult(Result<int, string>.Ok(1)).MatchAsync(x => x, _ => -1);
        Assert.Equal(1, resultOk);

        var resultError = await Task.FromResult(Result<int, string>.Error("Error")).MatchAsync(_ => -1, _ => 1);
        Assert.Equal(1, resultError);
    }

    [Fact]
    public async Task Bind()
    {
        AssertResult.IsOk(2, await Task.FromResult(Result<int, string>.Ok(1)).BindAsync(x => Result<int, string>.Ok(x + 1)));
        AssertResult.IsOk(2, await Task.FromResult(Result<int, string>.Ok(1)).BindAsync(x => Result<int, string>.OkAsync(Task.FromResult(x + 1))));
        AssertResult.IsError(await Task.FromResult(Result<int, string>.Error("Error")).BindAsync(x => Result<int, string>.Ok(x + 1)));
        AssertResult.IsError(await Task.FromResult(Result<int, string>.Error("Error")).BindAsync(x => Result<int, string>.OkAsync(Task.FromResult(x + 1))));
    }

    [Fact]
    public async Task Map()
    {
        AssertResult.IsOk(2, await Task.FromResult(Result<int, string>.Ok(1)).MapAsync(x => x + 1));
        AssertResult.IsOk(2, await Task.FromResult(Result<int, string>.Ok(1)).MapAsync(x => Task.FromResult(x + 1)));
        AssertResult.IsError(await Task.FromResult(Result<int, string>.Error("Error")).MapAsync(x => x + 1));
        AssertResult.IsError(await Task.FromResult(Result<int, string>.Error("Error")).MapAsync(x => Task.FromResult(x + 1)));
    }

    [Fact]
    public async Task DefaultValue()
    {
        Assert.Equal(1, await Task.FromResult(Result<int, string>.Error("Error")).DefaultValueAsync(1));
        Assert.Equal(1, await Task.FromResult(Result<int, string>.Error("Error")).DefaultValueAsync(Task.FromResult(1)));
        Assert.Equal(2, await Task.FromResult(Result<int, string>.Ok(2)).DefaultValueAsync(1));
        Assert.Equal(2, await Task.FromResult(Result<int, string>.Ok(2)).DefaultValueAsync(Task.FromResult(1)));
    }

    [Fact]
    public async Task DefaultWith()
    {
        Assert.Equal(1, await Task.FromResult(Result<int, string>.Error("Error")).DefaultWithAsync(() => 1));
        Assert.Equal(1, await Task.FromResult(Result<int, string>.Error("Error")).DefaultWithAsync(() => Task.FromResult(1)));
        Assert.Equal(2, await Task.FromResult(Result<int, string>.Ok(2)).DefaultWithAsync(() => 1));
        Assert.Equal(2, await Task.FromResult(Result<int, string>.Ok(2)).DefaultWithAsync(() => Task.FromResult(1)));
    }
}
