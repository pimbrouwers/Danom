namespace Danom.Validation.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class ValidatorTests
{
    [Fact]
    public void ReturnsOk_WhenValidationSucceeds()
    {
        var validator = new TestInputValidator();
        AssertResult.IsOk(validator.Validate(TestInput.ValidInput));
    }

    [Fact]
    public void ReturnsError_WhenValidationFails()
    {
        var validator = new TestInputValidator();
        var result = validator.Validate(TestInput.InvalidInput);
        Assert.True(result.IsError);
    }

    [Fact]
    public void VerifyResultErrors()
    {
        var validator = new TestInputValidator();
        var result = validator.Validate(TestInput.InvalidInput);
        if (result.TryGetError(out var errors))
        {
            Assert.NotEmpty(errors);
            Assert.Single(errors["Id"].Errors);
            // Assert.Equal("Value must not be empty", errors["Id"].Errors[0]);
            Assert.Equal(2, errors["Label"].Errors.Count);
            Assert.Equal("'Label' must not be empty", errors["Label"].Errors[0]);
            Assert.Equal("'Label' must be longer than 3 characters", errors["Label"].Errors[1]);
            Assert.Equal(3, errors["IntValue"].Errors.Count);
            Assert.Equal("'IntValue' must be greater than 0", errors["IntValue"].Errors[0]);
            Assert.Equal("'IntValue' must be positive", errors["IntValue"].Errors[1]);
            Assert.Equal("This is not an acceptable response for 'IntValue'", errors["IntValue"].Errors[2]);
            Assert.Equal(2, errors[""].Errors.Count);
            Assert.Equal("'Value' must be greater than or equal to 1", errors[""].Errors[0]);
            Assert.Equal("'Value' is required", errors[""].Errors[1]);
        }
        else
        {
            Assert.Fail("Expected validation to fail, but it succeeded.");
        }
    }
}

public sealed class TestInputValidator : BaseValidator<TestInput>
{
    public TestInputValidator()
    {
        Rule("Id", x => x.Id.IsValid(new TestInputIdValidator()));
        Rule("Label", x => x.Label, [Check.IsNotEmpty, Check.IsLengthGreaterThan(3)]);
        Rule("IntValue", x => x.IntValue, [Check.IsGreaterThan(0), Check.IsPositive<int>()]);
        Rule("IntValue", x => x.IntValue, x => field => x == -1 ? Result.Error($"This is not an acceptable response for '{field}'") : Result.Ok());
        Rule(x => x.OptionalIntValue.Optional(x => x.IsGreaterThanOrEqualTo(1)));
        Rule(x => x.StringOption.Required(x => x.IsLengthBetween(3, 100)));
    }
}

public sealed class TestInput
{
    public TestInputId Id { get; init; } = new(Guid.Empty);
    public string Label { get; init; } = string.Empty;
    public int IntValue { get; init; } = -1;
    public Option<int> OptionalIntValue { get; init; } = Option<int>.Some(-1);
    public Option<string> StringOption { get; init; } = Option<string>.None();
    public IEnumerable<string> Phones { get; init; } = [];

    public override string ToString() => IntValue.ToString();

    public static TestInput ValidInput => new()
    {
        Id = new(Guid.NewGuid()),
        Label = "Valid Label",
        IntValue = 50,
        OptionalIntValue = Option.Some(10),
        StringOption = Option.Some("Valid"),
        Phones = ["+19051234567", "+19057654321"]
    };

    public static TestInput InvalidInput => new();
}

public readonly record struct TestInputId(Guid Id);

public sealed class TestInputIdValidator : BaseValidator<TestInputId>
{
    public TestInputIdValidator()
    {
        Rule(x => x.Id, Check.IsNotEmptyGuid);
    }
}
