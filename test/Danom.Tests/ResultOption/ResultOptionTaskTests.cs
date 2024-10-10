namespace Danom.Tests;

using Xunit;
using Danom.TestHelpers;

public sealed class ResultOptionTaskTests
{
    [Fact]
    public async Task MatchShouldWork()
    {
        var resultOk = await ResultOption<int, string>.OkAsync(1).MatchAsync(x => x, () => -1, _ => -1);
        Assert.Equal(1, resultOk);

        var resultError = await ResultOption<int, string>.ErrorAsync("Error").MatchAsync(_ => -1, () => -1, _ => 1);
        Assert.Equal(1, resultError);
    }

    [Fact]
    public async Task BindShouldWork()
    {
        AssertResultOption.IsOk(2, await ResultOption<int, string>.OkAsync(1).BindAsync(x => ResultOption<int, string>.Ok(x + 1)));
        AssertResultOption.IsOk(2, await ResultOption<int, string>.OkAsync(1).BindAsync(x => ResultOption<int, string>.OkAsync(x + 1)));
        AssertResultOption.IsError(await ResultOption<int, string>.ErrorAsync("Error").BindAsync(x => ResultOption<int, string>.Ok(x + 1)));
        AssertResultOption.IsError(await ResultOption<int, string>.ErrorAsync("Error").BindAsync(x => ResultOption<int, string>.OkAsync(x + 1)));
    }

    [Fact]
    public async Task MapShouldWork()
    {
        AssertResultOption.IsOk(2, await ResultOption<int, string>.OkAsync(1).MapAsync(x => x + 1));
        AssertResultOption.IsOk(2, await ResultOption<int, string>.OkAsync(1).MapAsync(x => Task.FromResult(x + 1)));
        AssertResultOption.IsError(await ResultOption<int, string>.ErrorAsync("Error").MapAsync(x => x + 1));
        AssertResultOption.IsError(await ResultOption<int, string>.ErrorAsync("Error").MapAsync(x => Task.FromResult(x + 1)));
    }

    [Fact]
    public async Task DefaultValueShouldWork()
    {
        Assert.Equal(1, await ResultOption<int, string>.ErrorAsync("Error").DefaultValueAsync(1));
        Assert.Equal(1, await ResultOption<int, string>.ErrorAsync("Error").DefaultValueAsync(Task.FromResult(1)));
        Assert.Equal(2, await ResultOption<int, string>.OkAsync(2).DefaultValueAsync(1));
        Assert.Equal(2, await ResultOption<int, string>.OkAsync(2).DefaultValueAsync(Task.FromResult(1)));
    }

    [Fact]
    public async Task DefaultWithShouldWork()
    {
        Assert.Equal(1, await ResultOption<int, string>.ErrorAsync("Error").DefaultWithAsync(() => 1));
        Assert.Equal(1, await ResultOption<int, string>.ErrorAsync("Error").DefaultWithAsync(() => Task.FromResult(1)));
        Assert.Equal(2, await ResultOption<int, string>.OkAsync(2).DefaultWithAsync(() => 1));
        Assert.Equal(2, await ResultOption<int, string>.OkAsync(2).DefaultWithAsync(() => Task.FromResult(1)));
    }
}
