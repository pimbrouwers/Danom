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
        AssertResult.IsError(validator.Validate(TestInput.InvalidInput));
    }
}

public sealed class TestInput
{
    public TestInputId Id { get; init; } = new(Guid.Empty);
    public string Label { get; init; } = string.Empty;
    public int IntValue { get; init; }
    public Option<int> OptionalIntValue { get; init; } = Option<int>.None();
    public Option<string> StringOption { get; init; } = Option<string>.None();

    public override string ToString() => IntValue.ToString();

    public static TestInput ValidInput => new()
    {
        Id = new(Guid.NewGuid()),
        IntValue = 50,
        OptionalIntValue = Option.Some(10),
        StringOption = Option.Some("Valid")
    };

    public static TestInput InvalidInput => new();
}

public sealed class TestInputValidator : BaseValidator<TestInput>
{
    public TestInputValidator()
    {
        Rule(x => x.Id.IsValid(new TestInputIdValidator()));
        Rule("Label", x => x.Label, [ x => x.IsNotEmpty("Label"), x => x.IsLengthGreaterThan(3, "Label") ]);
        Rule(x => x.IntValue.IsGreaterThan(0, field: "Value"));
        Rule(x => x.IntValue.IsLessThan(100, field: "Value"));
        Rule(x => x.IntValue == 99 ? Result.Error("sddsa") : Result.Ok());
        Rule(x => x.OptionalIntValue.Optional(x => x.IsGreaterThanOrEqualTo(1)));
        Rule(x => x.StringOption.Required(x => x.IsLengthBetween(3, 100), field: "OptionString"));
    }
}

public readonly record struct TestInputId(Guid Id);

public sealed class TestInputIdValidator : BaseValidator<TestInputId>
{
    public TestInputIdValidator()
    {
        Rule(x => x.Id.IsNotEmpty("TestInputId"));
    }
}
