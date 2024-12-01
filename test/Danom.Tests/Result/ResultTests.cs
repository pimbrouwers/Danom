namespace Danom.Tests;

using Xunit;
using Danom.TestHelpers;

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
    public async Task OkAsyncFromValue()
    {
        var result = await Result<int, string>.OkAsync(1);
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
    public async Task ErrorAsync()
    {
        var result = await Result<int, string>.ErrorAsync("Error");
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
        Assert.Equal("Error(Error)", result.ToString());
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
        Assert.Equal(Result<int,string>.Error("Error"), Result<int,string>.Error("Error"));
        Assert.Equal(Result<int,string>.Ok(1), Result<int,string>.Ok(1));
        Assert.NotEqual(Result<int,string>.Ok(1), Result<int,string>.Ok(2));
        Assert.NotEqual(Result<int,string>.Ok(1), Result<int,string>.Error("Error"));
    }

    [Fact]
    public void EqualityOperator()
    {
        Assert.True(Result<int,string>.Error("Error") == Result<int,string>.Error("Error"));
        Assert.True(Result<int,string>.Ok(1) == Result<int,string>.Ok(1));
        Assert.False(Result<int,string>.Ok(1) == Result<int,string>.Ok(2));
        Assert.False(Result<int,string>.Ok(1) == Result<int,string>.Error("Error"));
    }

    [Fact]
    public void InequalityOperator()
    {
        Assert.False(Result<int,string>.Error("Error") != Result<int,string>.Error("Error"));
        Assert.False(Result<int,string>.Ok(1) != Result<int,string>.Ok(1));
        Assert.True(Result<int,string>.Ok(1) != Result<int,string>.Ok(2));
        Assert.True(Result<int,string>.Ok(1) != Result<int,string>.Error("Error"));
    }
}
