namespace Danom.Validation.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class ValidationOptionTests
{
    [Fact]
    public void ReturnsSomeOption_WhenValidationSucceeds()
    {
        var result = ValidationOption<TestInput>.From<TestInputValidator>(TestInput.ValidInput);
        AssertOption.IsSome(result);
        Assert.False(result.IsNone);
    }

    [Fact]
    public void ReturnsNoneOption_WhenValidationFails()
    {
        var result = ValidationOption<TestInput>.From<TestInputValidator>(TestInput.InvalidInput);
        AssertOption.IsNone(result);
        Assert.False(result.IsSome);
    }
}
