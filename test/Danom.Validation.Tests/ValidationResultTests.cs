namespace Danom.Validation.Tests;

using Danom.TestHelpers;
using FluentValidation;
using Xunit;

public sealed class ValidationResultTests
{
    [Fact]
    public void ReturnsOkResult_WhenValidationSucceeds()
    {
        var input = new TestInput { Value = 1 };
        var result = ValidationResult<TestInput>.From<TestInputValidator>(input);

        AssertResult.IsOk(result);
        Assert.False(result.IsError);
    }

    [Fact]
    public void ReturnsErrorResult_WhenValidationFails()
    {
        var input = new TestInput { Value = 0 };
        var result = ValidationResult<TestInput>.From<TestInputValidator>(input);

        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    public sealed class TestInput
    {
        public int Value { get; set; }

        public override string ToString() => Value.ToString();
    }

    public sealed class TestInputValidator : AbstractValidator<TestInput>
    {
        public TestInputValidator()
        {
            RuleFor(x => x.Value).GreaterThan(0);
        }
    }
}
