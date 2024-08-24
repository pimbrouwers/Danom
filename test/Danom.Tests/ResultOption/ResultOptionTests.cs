namespace Danom.Tests;

using Xunit;

public sealed class ResultOptionTests
{
    [Fact]
    public void OkShouldWork()
    {
        var result = ResultOption<int, string>.Ok(1);
        AssertResultOption.IsOk(result);
        Assert.False(result.IsNone);
        Assert.False(result.IsError);
        Assert.Equal("Ok(1)", result.ToString());
    }

    [Fact]
    public async Task OkAsyncFromValueShouldWork()
    {
        var result = await ResultOption<int, string>.OkAsync(1);
        AssertResultOption.IsOk(result);
        Assert.False(result.IsNone);
        Assert.False(result.IsError);
        Assert.Equal("Ok(1)", result.ToString());
    }

    [Fact]
    public async Task OkAsyncFromTaskShouldWork()
    {
        var result = await ResultOption<int, string>.OkAsync(Task<int>.Factory.StartNew(() => 1));
        AssertResultOption.IsOk(result);
        Assert.False(result.IsNone);
        Assert.False(result.IsError);
        Assert.Equal("Ok(1)", result.ToString());
    }

    [Fact]
    public void NoneShouldWork()
    {
        var result = ResultOption<int, string>.None();
        AssertResultOption.IsNone(result);
        Assert.False(result.IsOk);
        Assert.False(result.IsError);
        Assert.Equal("None", result.ToString());
    }

    [Fact]
    public async Task NoneAsyncShouldWork()
    {
        var result = await ResultOption<int, string>.NoneAsync();
        AssertResultOption.IsNone(result);
        Assert.False(result.IsOk);
        Assert.False(result.IsError);
        Assert.Equal("None", result.ToString());
    }

    [Fact]
    public void ErrorShouldWork()
    {
        var result = ResultOption<int, string>.Error("Error");
        AssertResultOption.IsError(result);
        Assert.False(result.IsOk);
        Assert.Equal("Error(Error)", result.ToString());
    }

    [Fact]
    public async Task ErrorAsyncShouldWork()
    {
        var result = await ResultOption<int, string>.ErrorAsync("Error");
        AssertResultOption.IsError(result);
        Assert.False(result.IsOk);
        Assert.Equal("Error(Error)", result.ToString());
    }

    [Fact]
    public void MatchShouldWork()
    {
        Assert.Equal(1,
            ResultOption<int, string>.Ok(1)
                .Match(x => x, () => -1, _ => -1));

        Assert.Equal(1,
            ResultOption<int, string>.Error("Error")
                .Match(_ => -1, () => -1, _ => 1));
    }

    [Fact]
    public void BindShouldWork()
    {
        AssertResultOption.IsOk(2, ResultOption<int, string>.Ok(1).Bind(x => ResultOption<int, string>.Ok(x + 1)));
        AssertResultOption.IsError(ResultOption<int, string>.Error("Error").Bind(x => ResultOption<int, string>.Ok(x + 1)));
    }

    [Fact]
    public void MapShouldWork()
    {
        AssertResultOption.IsOk(2, ResultOption<int, string>.Ok(1).Map(x => x + 1));
        AssertResultOption.IsError(ResultOption<int, string>.Error("Error").Map(x => x + 1));
    }

    [Fact]
    public void DefaultValueShouldWork()
    {
        Assert.Equal(1, ResultOption<int, string>.Error("Error").DefaultValue(1));
        Assert.Equal(2, ResultOption<int, string>.Ok(2).DefaultValue(1));
    }

    [Fact]
    public void DefaultWithShouldWork()
    {
        Assert.Equal(1, ResultOption<int, string>.Error("Error").DefaultWith(() => 1));
        Assert.Equal(2, ResultOption<int, string>.Ok(2).DefaultWith(() => 1));
    }
}
