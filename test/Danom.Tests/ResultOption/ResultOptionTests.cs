namespace Danom.Tests;

using Xunit;
using Danom.TestHelpers;

public sealed class ResultOptionTests
{
    [Fact]
    public void Ok()
    {
        var result = ResultOption<int, string>.Ok(1);
        AssertResultOption.IsOk(result);
        Assert.False(result.IsNone);
        Assert.False(result.IsError);
        Assert.Equal("Ok(1)", result.ToString());
    }

    [Fact]
    public async Task OkAsyncFromValue()
    {
        var result = await ResultOption<int, string>.OkAsync(1);
        AssertResultOption.IsOk(result);
        Assert.False(result.IsNone);
        Assert.False(result.IsError);
        Assert.Equal("Ok(1)", result.ToString());
    }

    [Fact]
    public async Task OkAsyncFromTask()
    {
        var result = await ResultOption<int, string>.OkAsync(Task<int>.Factory.StartNew(() => 1));
        AssertResultOption.IsOk(result);
        Assert.False(result.IsNone);
        Assert.False(result.IsError);
        Assert.Equal("Ok(1)", result.ToString());
    }

    [Fact]
    public void None()
    {
        var result = ResultOption<int, string>.None();
        AssertResultOption.IsNone(result);
        Assert.False(result.IsOk);
        Assert.False(result.IsError);
        Assert.Equal("None", result.ToString());
    }

    [Fact]
    public async Task NoneAsync()
    {
        var result = await ResultOption<int, string>.NoneAsync();
        AssertResultOption.IsNone(result);
        Assert.False(result.IsOk);
        Assert.False(result.IsError);
        Assert.Equal("None", result.ToString());
    }

    [Fact]
    public void Error()
    {
        var result = ResultOption<int, string>.Error("Error");
        AssertResultOption.IsError(result);
        Assert.False(result.IsOk);
        Assert.Equal("Error(Error)", result.ToString());
    }

    [Fact]
    public async Task ErrorAsync()
    {
        var result = await ResultOption<int, string>.ErrorAsync("Error");
        AssertResultOption.IsError(result);
        Assert.False(result.IsOk);
        Assert.Equal("Error(Error)", result.ToString());
    }

    [Fact]
    public void Match()
    {
        Assert.Equal(1,
            ResultOption<int, string>.Ok(1)
                .Match(x => x, () => -1, _ => -1));

        Assert.Equal(1,
            ResultOption<int, string>.Error("Error")
                .Match(_ => -1, () => -1, _ => 1));
    }

    [Fact]
    public void MatchAction()
    {
        var ok = 0;
        var error = 0;
        ResultOption<int, string>.Ok(1).Match(_ => ok++, () => error++, _ => error++);
        Assert.Equal(1, ok);
        Assert.Equal(0, error);

        ok = 0;
        error = 0;
        ResultOption<int, string>.Error("Error").Match(_ => ok++, () => error++, _ => error++);
        Assert.Equal(0, ok);
        Assert.Equal(1, error);
    }

    [Fact]
    public void Bind()
    {
        AssertResultOption.IsOk(2, ResultOption<int, string>.Ok(1).Bind(x => ResultOption<int, string>.Ok(x + 1)));
        AssertResultOption.IsError(ResultOption<int, string>.Error("Error").Bind(x => ResultOption<int, string>.Ok(x + 1)));
    }

    [Fact]
    public void Map()
    {
        AssertResultOption.IsOk(2, ResultOption<int, string>.Ok(1).Map(x => x + 1));
        AssertResultOption.IsError(ResultOption<int, string>.Error("Error").Map(x => x + 1));
    }

    [Fact]
    public void DefaultValue()
    {
        Assert.Equal(1, ResultOption<int, string>.Error("Error").DefaultValue(1));
        Assert.Equal(2, ResultOption<int, string>.Ok(2).DefaultValue(1));
    }

    [Fact]
    public void DefaultWith()
    {
        Assert.Equal(1, ResultOption<int, string>.Error("Error").DefaultWith(() => 1));
        Assert.Equal(2, ResultOption<int, string>.Ok(2).DefaultWith(() => 1));
    }


    [Fact]
    public void Equality()
    {
        Assert.Equal(ResultOption<int, string>.None(), ResultOption<int, string>.None());
        Assert.Equal(ResultOption<int,string>.Error("Error"), ResultOption<int,string>.Error("Error"));
        Assert.Equal(ResultOption<int,string>.Ok(1), ResultOption<int,string>.Ok(1));
        Assert.NotEqual(ResultOption<int,string>.Ok(1), ResultOption<int,string>.Ok(2));
        Assert.NotEqual(ResultOption<int,string>.Ok(1), ResultOption<int,string>.Error("Error"));
    }

    [Fact]
    public void EqualityOperator()
    {
        Assert.True(ResultOption<int, string>.None() == ResultOption<int, string>.None());
        Assert.True(ResultOption<int,string>.Error("Error") == ResultOption<int,string>.Error("Error"));
        Assert.True(ResultOption<int,string>.Ok(1) == ResultOption<int,string>.Ok(1));
        Assert.False(ResultOption<int,string>.Ok(1) == ResultOption<int,string>.Ok(2));
        Assert.False(ResultOption<int,string>.Ok(1) == ResultOption<int,string>.Error("Error"));
    }

    [Fact]
    public void InequalityOperator()
    {
        Assert.False(ResultOption<int, string>.None() != ResultOption<int, string>.None());
        Assert.False(ResultOption<int,string>.Error("Error") != ResultOption<int,string>.Error("Error"));
        Assert.False(ResultOption<int,string>.Ok(1) != ResultOption<int,string>.Ok(1));
        Assert.True(ResultOption<int,string>.Ok(1) != ResultOption<int,string>.Ok(2));
        Assert.True(ResultOption<int,string>.Ok(1) != ResultOption<int,string>.Error("Error"));
    }
}
