namespace Danom.Validation.Tests;

using Xunit;
using FluentValidation;
using Danom.TestHelpers;

public sealed class ValidationOptionTests
{
    [Fact]
    public void ReturnsSomeOption_WhenValidationSucceeds()
    {
        var input = new TestInput { Value = 1 };
        var result = ValidationOption<TestInput>.From<TestInputValidator>(input);

        AssertOption.IsNone(result);
        Assert.False(result.IsSome);
    }

    [Fact]
    public void ReturnsNoneOption_WhenValidationFails()
    {
        var input = new TestInput { Value = 0 };
        var result = ValidationOption<TestInput>.From<TestInputValidator>(input);

        AssertOption.IsNone(result);
        Assert.False(result.IsSome);
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
