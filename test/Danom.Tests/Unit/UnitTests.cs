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

    [Fact]
    public void GetHashCodeShouldBeZero()
    {
        Assert.Equal(0, Unit.Value.GetHashCode());
    }

    [Fact]
    public void EqualityOperatorShouldWork()
    {
#pragma warning disable CS1718 // Comparison made to same variable
        Assert.True(Unit.Value == Unit.Value);
#pragma warning restore CS1718 // Comparison made to same variable
    }

    [Fact]
    public void InequalityOperatorShouldWork()
    {
#pragma warning disable CS1718 // Comparison made to same variable
        Assert.False(Unit.Value != Unit.Value);
#pragma warning restore CS1718 // Comparison made to same variable
    }
}