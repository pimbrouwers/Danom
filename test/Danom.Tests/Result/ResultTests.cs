namespace Danom.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class ResultTests
{
    [Fact]
    public void Ok()
    {
        var result = Result<int, string>.Ok(1);
        AssertResult.IsOk(result);
        Assert.False(result.IsError);
        Assert.Equal("Ok(1)", result.ToString());
    }

    [Fact]
    public async Task OkAsyncFromTask()
    {
        var result = await Result<int, string>.OkAsync(Task<int>.Factory.StartNew(() => 1));
        AssertResult.IsOk(result);
        Assert.False(result.IsError);
        Assert.Equal("Ok(1)", result.ToString());
    }

    [Fact]
    public void Error()
    {
        var result = Result<int, string>.Error("Error");
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
        Assert.Equal("Error(Error)", result.ToString());
    }

    [Fact]
    public void Match()
    {
        Assert.Equal(1,
            Result<int, string>.Ok(1)
                .Match(x => x, _ => -1));

        Assert.Equal(1,
            Result<int, string>.Error("Error")
                .Match(_ => -1, _ => 1));
    }

    [Fact]
    public void MatchAction()
    {
        var ok = false;
        var error = false;

        Result<int, string>.Ok(1)
            .Match(ok: _ => ok = true, error: _ => error = true);

        Assert.True(ok);
        Assert.False(error);

        ok = false;
        error = false;

        Result<int, string>.Error("Error")
            .Match(ok: _ => ok = true, error: _ => error = true);

        Assert.False(ok);
        Assert.True(error);
    }

    [Fact]
    public void Bind()
    {
        AssertResult.IsOk(2, Result<int, string>.Ok(1).Bind(x => Result<int, string>.Ok(x + 1)));
        AssertResult.IsError(Result<int, string>.Error("Error").Bind(x => Result<int, string>.Ok(x + 1)));
    }

    [Fact]
    public void Map()
    {
        AssertResult.IsOk(2, Result<int, string>.Ok(1).Map(x => x + 1));
        AssertResult.IsError(Result<int, string>.Error("Error").Map(x => x + 1));
    }

    [Fact]
    public void DefaultValue()
    {
        Assert.Equal(1, Result<int, string>.Error("Error").DefaultValue(1));
        Assert.Equal(2, Result<int, string>.Ok(2).DefaultValue(1));
    }

    [Fact]
    public void DefaultWith()
    {
        Assert.Equal(1, Result<int, string>.Error("Error").DefaultWith(() => 1));
        Assert.Equal(2, Result<int, string>.Ok(2).DefaultWith(() => 1));
    }

    [Fact]
    public void Equality()
    {
        Assert.Equal(Result<int, string>.Error("Error"), Result<int, string>.Error("Error"));
        Assert.Equal(Result<int, string>.Ok(1), Result<int, string>.Ok(1));
        Assert.NotEqual(Result<int, string>.Ok(1), Result<int, string>.Ok(2));
        Assert.NotEqual(Result<int, string>.Ok(1), Result<int, string>.Error("Error"));
    }

    [Fact]
    public void EqualityOperator()
    {
        Assert.True(Result<int, string>.Error("Error") == Result<int, string>.Error("Error"));
        Assert.True(Result<int, string>.Ok(1) == Result<int, string>.Ok(1));
        Assert.False(Result<int, string>.Ok(1) == Result<int, string>.Ok(2));
        Assert.False(Result<int, string>.Ok(1) == Result<int, string>.Error("Error"));
    }

    [Fact]
    public void InequalityOperator()
    {
        Assert.False(Result<int, string>.Error("Error") != Result<int, string>.Error("Error"));
        Assert.False(Result<int, string>.Ok(1) != Result<int, string>.Ok(1));
        Assert.True(Result<int, string>.Ok(1) != Result<int, string>.Ok(2));
        Assert.True(Result<int, string>.Ok(1) != Result<int, string>.Error("Error"));
    }

    [Fact]
    public void TryGet()
    {
        var result = Result<int, string>.Ok(1);
        if (result.TryGet(out var value))
        {
            Assert.Equal(1, value);
        }
        else
        {
            Assert.Fail("Expected Ok result but got Error.");
        }
    }

    [Fact]
    public void TryGetError()
    {
        var resultE = Result<int, string>.Error("Error");
        if (resultE.TryGetError(out var errorE))
        {
            Assert.Equal("Error", errorE);
        }
        else
        {
            Assert.Fail("Expected Error result but got Ok.");
        }
    }
}


public sealed class ResultTTests
{
    [Fact]
    public void Ok()
    {
        var result = Result<int>.Ok(1);
        AssertResult.IsOk(result);
        Assert.False(result.IsError);
        Assert.Equal("Ok(1)", result.ToString());
    }

    [Fact]
    public async Task OkAsyncFromValue()
    {
        var result = await Task.FromResult(Result<int>.Ok(1));
        AssertResult.IsOk(result);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task OkAsyncFromTask()
    {
        var result = await Result<int>.OkAsync(Task<int>.Factory.StartNew(() => 1));
        AssertResult.IsOk(result);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task ErrorAsync()
    {
        var result = await Task.FromResult(Result<int>.Error("Error"));
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    [Fact]
    public void Error()
    {
        var result = Result<int>.Error(new ResultErrors("Error"));
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    [Fact]
    public void ErrorFromString()
    {
        var result = Result<int>.Error("Error");
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    [Fact]
    public void ErrorFromKeyAndValue()
    {
        var result = Result<int>.Error("Error", "Error message");
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    [Fact]
    public void ErrorFromKeyAndParams()
    {
        var result = Result<int>.Error("Error", "Error message 1", "Error message 2");
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    [Fact]
    public void Match()
    {
        Assert.Equal(1,
            Result<int>.Ok(1)
                .Match(x => x, _ => -1));

        Assert.Equal(1,
            Result<int>.Error("Error")
                .Match(_ => -1, _ => 1));
    }

    [Fact]
    public void Bind()
    {
        AssertResult.IsOk(2, Result<int>.Ok(1).Bind(x => Result<int>.Ok(x + 1)));
        AssertResult.IsError(Result<int>.Error("Error").Bind(x => Result<int>.Ok(x + 1)));
    }

    [Fact]
    public void Map()
    {
        AssertResult.IsOk(2, Result<int>.Ok(1).Map(x => x + 1));
        AssertResult.IsError(Result<int>.Error("Error").Map(x => x + 1));
    }

    [Fact]
    public void DefaultValue()
    {
        Assert.Equal(1, Result<int>.Error("Error").DefaultValue(1));
        Assert.Equal(2, Result<int>.Ok(2).DefaultValue(1));
    }

    [Fact]
    public void DefaultWith()
    {
        Assert.Equal(1, Result<int>.Error("Error").DefaultWith(() => 1));
        Assert.Equal(2, Result<int>.Ok(2).DefaultWith(() => 1));
    }
}

public sealed class ResultTAsyncTests
{
    [Fact]
    public async Task Match()
    {
        var resultOk = await Task.FromResult(Result<int>.Ok(1)).MatchAsync(x => x, _ => -1);
        Assert.Equal(1, resultOk);

        var resultError = await Task.FromResult(Result<int>.Error("Error")).MatchAsync(_ => -1, _ => 1);
        Assert.Equal(1, resultError);
    }

    [Fact]
    public async Task Bind()
    {
        AssertResult.IsOk(2, await Task.FromResult(Result<int>.Ok(1)).BindAsync(x => Result<int>.Ok(x + 1)));
        AssertResult.IsOk(2, await Task.FromResult(Result<int>.Ok(1)).BindAsync(x => Result<int>.OkAsync(Task.FromResult(x + 1))));
        AssertResult.IsError(await Task.FromResult(Result<int>.Error("Error")).BindAsync(x => Result<int>.Ok(x + 1)));
        AssertResult.IsError(await Task.FromResult(Result<int>.Error("Error")).BindAsync(x => Result<int>.OkAsync(Task.FromResult(x + 1))));
    }

    [Fact]
    public async Task Map()
    {
        AssertResult.IsOk(2, await Task.FromResult(Result<int>.Ok(1)).MapAsync(x => x + 1));
        AssertResult.IsOk(2, await Task.FromResult(Result<int>.Ok(1)).MapAsync(x => Task.FromResult(x + 1)));
        AssertResult.IsError(await Task.FromResult(Result<int>.Error("Error")).MapAsync(x => x + 1));
        AssertResult.IsError(await Task.FromResult(Result<int>.Error("Error")).MapAsync(x => Task.FromResult(x + 1)));
    }

    [Fact]
    public async Task DefaultValue()
    {
        Assert.Equal(1, await Task.FromResult(Result<int>.Error("Error")).DefaultValueAsync(1));
        Assert.Equal(1, await Task.FromResult(Result<int>.Error("Error")).DefaultValueAsync(Task.FromResult(1)));
        Assert.Equal(2, await Task.FromResult(Result<int>.Ok(2)).DefaultValueAsync(1));
        Assert.Equal(2, await Task.FromResult(Result<int>.Ok(2)).DefaultValueAsync(Task.FromResult(1)));
    }

    [Fact]
    public async Task DefaultWith()
    {
        Assert.Equal(1, await Task.FromResult(Result<int>.Error("Error")).DefaultWithAsync(() => 1));
        Assert.Equal(1, await Task.FromResult(Result<int>.Error("Error")).DefaultWithAsync(() => Task.FromResult(1)));
        Assert.Equal(2, await Task.FromResult(Result<int>.Ok(2)).DefaultWithAsync(() => 1));
        Assert.Equal(2, await Task.FromResult(Result<int>.Ok(2)).DefaultWithAsync(() => Task.FromResult(1)));
    }


}
