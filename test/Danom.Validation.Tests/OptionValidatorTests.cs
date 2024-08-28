namespace Danom.Validation.Tests;

using FluentValidation;
using Xunit;

public sealed class OptionValidatorTests
{
    [Fact]
    public void ReturnsOkResult_WhenValidationSucceeds()
    {
        var input = new TestInput { Value = Option<int>.Some(1) };
        var result = ValidationResult<TestInput>.From<TestInputValidator>(input);

        AssertResult.IsOk(input, result);
        Assert.False(result.IsError);
    }

    [Fact]
    public void ReturnsErrorResult_WhenValidationFails()
    {
        var input = new TestInput { Value = Option<int>.Some(0) };
        var result = ValidationResult<TestInput>.From<TestInputValidator>(input);

        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    public sealed class TestInput
    {
        public Option<int> Value { get; set; }

        public override string ToString() => Value.ToString();
    }

    public sealed class TestInputValidator : AbstractValidator<TestInput>
    {
        public TestInputValidator()
        {
            RuleFor(x => x.Value).WhenSome(x => x.GreaterThan(0));
        }
    }


}
