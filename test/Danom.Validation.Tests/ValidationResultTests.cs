namespace Danom.Validation.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class ValidationResultTests
{
    [Fact]
    public void ReturnsOkResult_WhenValidationSucceeds()
    {
        var result = ValidationResult<TestInput>.From<TestInputValidator>(TestInput.ValidInput);
        AssertResult.IsOk(result);
        Assert.False(result.IsError);
    }

    [Fact]
    public void ReturnsErrorResult_WhenValidationFails()
    {
        var result = ValidationResult<TestInput>.From<TestInputValidator>(TestInput.InvalidInput);
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }
}
