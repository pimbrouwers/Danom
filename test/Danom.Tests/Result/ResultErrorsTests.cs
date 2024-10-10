namespace Danom.Tests;

using Xunit;
using Danom.TestHelpers;

public sealed class ResultErrorTests
{
    [Fact]
    public void CanCreateFromString()
    {
        Assert.Equal("Error", new ResultError("Error").ToString());
    }

    [Fact]
    public void CanCreateFromStringWithKey()
    {
        Assert.Equal("Key - Error", new ResultError("Key", "Error").ToString());
    }

    [Fact]
    public void CanCreateFromStrings()
    {
        Assert.Equal("Error1, Error2", new ResultError(["Error1", "Error2"]).ToString());
    }

    [Fact]
    public void CanCreateFromStringsWithKey()
    {
        Assert.Equal("Key - Error1, Error2", new ResultError("Key", ["Error1", "Error2"]).ToString());
    }
}

public sealed class ResultErrorsTests
{
    [Fact]
    public void CanCreateAndEnumerate()
    {
        var resultErrors = new ResultErrors([
            new ResultError("Error1"),
            new ResultError("Error2")]);

        Assert.NotEmpty(resultErrors);
        Assert.Equal(2, resultErrors.Count());
        Assert.Equal($"[{Environment.NewLine}Error1{Environment.NewLine}Error2{Environment.NewLine}]", resultErrors.ToString());
    }

    [Fact]
    public void CanAdd()
    {
        var resultErrors = new ResultErrors();
        resultErrors.Add(new("Error"));
        Assert.Single(resultErrors);
        Assert.Equal($"[{Environment.NewLine}Error{Environment.NewLine}]", resultErrors.ToString());
    }

    [Fact]
    public void CanCreateFromString()
    {
        var resultErrors = new ResultErrors("Error");
        Assert.Single(resultErrors);
        Assert.Equal($"[{Environment.NewLine}Error{Environment.NewLine}]", resultErrors.ToString());
    }
}

public sealed class ResultTTests
{
    [Fact]
    public void OkShouldWork()
    {
        var result = Result<int>.Ok(1);
        AssertResult.IsOk(result);
        Assert.False(result.IsError);
        Assert.Equal("Ok(1)", result.ToString());
    }

    [Fact]
    public async Task OkAsyncFromValueShouldWork()
    {
        var result = await Result<int>.OkAsync(1);
        AssertResult.IsOk(result);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task OkAsyncFromTaskShouldWork()
    {
        var result = await Result<int>.OkAsync(Task<int>.Factory.StartNew(() => 1));
        AssertResult.IsOk(result);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task ErrorAsyncShouldWork()
    {
        var result = await Result<int>.ErrorAsync("Error");
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    [Fact]
    public void ErrorShouldWork()
    {
        var result = Result<int>.Error("Error");
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    [Fact]
    public void MatchShouldWork()
    {
        Assert.Equal(1,
            Result<int>.Ok(1)
                .Match(x => x, _ => -1));

        Assert.Equal(1,
            Result<int>.Error("Error")
                .Match(_ => -1, _ => 1));
    }

    [Fact]
    public void BindShouldWork()
    {
        AssertResult.IsOk(2, Result<int>.Ok(1).Bind(x => Result<int>.Ok(x + 1)));
        AssertResult.IsError(Result<int>.Error("Error").Bind(x => Result<int>.Ok(x + 1)));
    }

    [Fact]
    public void MapShouldWork()
    {
        AssertResult.IsOk(2, Result<int>.Ok(1).Map(x => x + 1));
        AssertResult.IsError(Result<int>.Error("Error").Map(x => x + 1));
    }

    [Fact]
    public void DefaultValueShouldWork()
    {
        Assert.Equal(1, Result<int>.Error("Error").DefaultValue(1));
        Assert.Equal(2, Result<int>.Ok(2).DefaultValue(1));
    }

    [Fact]
    public void DefaultWithShouldWork()
    {
        Assert.Equal(1, Result<int>.Error("Error").DefaultWith(() => 1));
        Assert.Equal(2, Result<int>.Ok(2).DefaultWith(() => 1));
    }
}

public sealed class ResultTAsyncTests
{
    [Fact]
    public async Task MatchShouldWork()
    {
        var resultOk = await Result<int>.OkAsync(1).MatchAsync(x => x, _ => -1);
        Assert.Equal(1, resultOk);

        var resultError = await Result<int>.ErrorAsync("Error").MatchAsync(_ => -1, _ => 1);
        Assert.Equal(1, resultError);
    }

    [Fact]
    public async Task BindShouldWork()
    {
        AssertResult.IsOk(2, await Result<int>.OkAsync(1).BindAsync(x => Result<int>.Ok(x + 1)));
        AssertResult.IsOk(2, await Result<int>.OkAsync(1).BindAsync(x => Result<int>.OkAsync(x + 1)));
        AssertResult.IsError(await Result<int>.ErrorAsync("Error").BindAsync(x => Result<int>.Ok(x + 1)));
        AssertResult.IsError(await Result<int>.ErrorAsync("Error").BindAsync(x => Result<int>.OkAsync(x + 1)));
    }

    [Fact]
    public async Task MapShouldWork()
    {
        AssertResult.IsOk(2, await Result<int>.OkAsync(1).MapAsync(x => x + 1));
        AssertResult.IsOk(2, await Result<int>.OkAsync(1).MapAsync(x => Task.FromResult(x + 1)));
        AssertResult.IsError(await Result<int>.ErrorAsync("Error").MapAsync(x => x + 1));
        AssertResult.IsError(await Result<int>.ErrorAsync("Error").MapAsync(x => Task.FromResult(x + 1)));
    }

    [Fact]
    public async Task DefaultValueShouldWork()
    {
        Assert.Equal(1, await Result<int>.ErrorAsync("Error").DefaultValueAsync(1));
        Assert.Equal(1, await Result<int>.ErrorAsync("Error").DefaultValueAsync(Task.FromResult(1)));
        Assert.Equal(2, await Result<int>.OkAsync(2).DefaultValueAsync(1));
        Assert.Equal(2, await Result<int>.OkAsync(2).DefaultValueAsync(Task.FromResult(1)));
    }

    [Fact]
    public async Task DefaultWithShouldWork()
    {
        Assert.Equal(1, await Result<int>.ErrorAsync("Error").DefaultWithAsync(() => 1));
        Assert.Equal(1, await Result<int>.ErrorAsync("Error").DefaultWithAsync(() => Task.FromResult(1)));
        Assert.Equal(2, await Result<int>.OkAsync(2).DefaultWithAsync(() => 1));
        Assert.Equal(2, await Result<int>.OkAsync(2).DefaultWithAsync(() => Task.FromResult(1)));
    }
}
