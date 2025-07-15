namespace Danom.Validation.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class OptionValidatorTests
{
    // public sealed class Optional
    // {
    //     [Fact]
    //     public void ReturnsOkResult_WhenValidationSucceeds()
    //     {

    //         var input = new TestInput { Value = Option<int>.Some(1) };
    //         var result = ValidationResult<TestInput>.From<TestInputValidator>(input);

    //         AssertResult.IsOk(input, result);
    //         Assert.False(result.IsError);
    //     }

    //     [Fact]
    //     public void ReturnsErrorResult_WhenValidationFails()
    //     {
    //         var input = new TestInput { Value = Option<int>.Some(0) };
    //         var result = ValidationResult<TestInput>.From<TestInputValidator>(input);

    //         AssertResult.IsError(result);
    //         Assert.False(result.IsOk);
    //         Assert.Equal(
    //             $"Error([ Value - 'Value' must be greater than '0'., 'Value' is optional, but invalid. ])",
    //             result.ToString());
    //     }

    //     public sealed class TestInput
    //     {
    //         public Option<int> Value { get; set; }

    //         public override string ToString() => Value.ToString();
    //     }

    //     public sealed class TestInputValidator : AbstractValidator<TestInput>
    //     {
    //         public TestInputValidator()
    //         {
    //             RuleFor(x => x.Value).Optional(x => x.GreaterThan(0));
    //         }
    //     }
    // }

    // public sealed class Required
    // {
    //     public sealed class TestInput
    //     {
    //         public Option<int> Value { get; set; }

    //         public override string ToString() => Value.ToString();
    //     }

    //     public sealed class TestInputValidator : AbstractValidator<TestInput>
    //     {
    //         public TestInputValidator()
    //         {
    //             RuleFor(x => x.Value).Required();
    //         }
    //     }

    //     public sealed class TestInputValidator2 : AbstractValidator<TestInput>
    //     {
    //         public TestInputValidator2()
    //         {
    //             RuleFor(x => x.Value).Required(x => x.GreaterThan(2));
    //         }
    //     }

    //     public sealed class TestId
    //     {
    //         public int Value { get; set; }
    //     }

    //     public sealed class TestIdValidator : AbstractValidator<TestId>
    //     {
    //         public TestIdValidator()
    //         {
    //             RuleFor(x => x.Value).GreaterThan(0).WithMessage("{PropertyName} must be a valid TestId.");
    //         }
    //     }

    //     public sealed class TestIdInput
    //     {
    //         public Option<TestId> TestId { get; set; }
    //     }

    //     public sealed class TestIdInputValidator : AbstractValidator<TestIdInput>
    //     {
    //         public TestIdInputValidator()
    //         {
    //             RuleFor(x => x.TestId).Required(new TestIdValidator());
    //         }
    //     }

    //     [Fact]
    //     public void ReturnsErrorResult_WhenValueIsNone()
    //     {
    //         var input = new TestInput { Value = Option<int>.None() };
    //         var result = ValidationResult<TestInput>.From<TestInputValidator>(input);

    //         AssertResult.IsError(result);
    //         Assert.False(result.IsOk);
    //     }

    //     [Fact]
    //     public void ReturnsOkResult_WhenValueIsSome()
    //     {
    //         var input = new TestInput { Value = Option<int>.Some(1) };
    //         var result = ValidationResult<TestInput>.From<TestInputValidator>(input);

    //         AssertResult.IsOk(input, result);
    //         Assert.False(result.IsError);
    //     }

    //     [Fact]
    //     public void ReturnsOkResult_WhenValueIsSome2()
    //     {
    //         var input = new TestInput { Value = Option<int>.Some(3) };
    //         var result = ValidationResult<TestInput>.From<TestInputValidator2>(input);

    //         AssertResult.IsOk(input, result);
    //         Assert.False(result.IsError);
    //     }

    //     [Fact]
    //     public void ReturnsErrorResult_WhenValueIsNone2()
    //     {
    //         var input = new TestInput { Value = Option<int>.None() };
    //         var result = ValidationResult<TestInput>.From<TestInputValidator2>(input);

    //         AssertResult.IsError(result);
    //         Assert.False(result.IsOk);
    //         Assert.Equal(
    //             $"Error([ Value - 'Value' is required and invalid or missing. ])",
    //             result.ToString());

    //         input = new TestInput { Value = Option<int>.Some(2) };
    //         result = ValidationResult<TestInput>.From<TestInputValidator2>(input);

    //         AssertResult.IsError(result);
    //         Assert.False(result.IsOk);

    //         Assert.Equal(
    //             $"Error([ Value - 'Value' must be greater than '2'., 'Value' is required and invalid or missing. ])",
    //             result.ToString());
    //     }

    //     [Fact]
    //     public void ReturnsOkResult_WhenTestIdIsSome()
    //     {
    //         var input = new TestIdInput { TestId = Option<TestId>.Some(new TestId() { Value = 1 }) };
    //         var result = ValidationResult<TestIdInput>.From<TestIdInputValidator>(input);

    //         AssertResult.IsOk(input, result);
    //         Assert.False(result.IsError);
    //     }

    //     [Fact]
    //     public void ReturnsErrorResult_WhenTestIdIsNone()
    //     {
    //         var input = new TestIdInput { TestId = Option<TestId>.None() };
    //         var result = ValidationResult<TestIdInput>.From<TestIdInputValidator>(input);

    //         AssertResult.IsError(result);
    //         Assert.False(result.IsOk);
    //         Assert.Equal(
    //             $"Error([ TestId - 'Test Id' is required and invalid or missing. ])",
    //             result.ToString());
    //     }
    // }

}
