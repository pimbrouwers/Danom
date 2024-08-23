namespace Danom.Tests;

using Xunit;

public sealed class UnitTests
{
    [Fact]
    public void EqualityShouldWork()
    {
        Assert.Equal(Unit.Value, Unit.Value);
    }

    [Fact]
    public void ToStringShouldProduceBrackets()
    {
        Assert.Equal("()", Unit.Value.ToString());
    }
}
