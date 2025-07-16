namespace Danom.Validation.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class ValidatorTests
{
    [Fact]
    public void ReturnsOk_WhenValidationSucceeds()
    {
        Assert.True(ValidationOption<TestInput>.From<TestInputValidator>(TestInput.ValidInput).IsSome);
    }

    [Fact]
    public void ReturnsError_WhenValidationFails()
    {
        Assert.True(ValidationOption<TestInput>.From<TestInputValidator>(TestInput.InvalidInput).IsNone);
    }

    [Fact]
    public void VerifyResultErrors()
    {
        var result = ValidationResult<TestInput>.From<TestInputValidator>(TestInput.InvalidInput);
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
        Rule("Id", x => x.Id, Check.IsValid(new TestInputIdValidator()));
        Rule("Label", x => x.Label, [Check.String.IsNotEmpty, Check.String.IsLengthGreaterThan(3)]);
        Rule("IntValue", x => x.IntValue, [Check.IsGreaterThan(0), Check.IsPositive<int>()]);
        Rule("IntValue", x => x.IntValue, x => field => x == -1 ? Result.Error($"This is not an acceptable response for '{field}'") : Result.Ok());
        Rule(x => x.OptionalIntValue, Check.Optional(Check.IsGreaterThanOrEqualTo(1)));
        Rule(x => x.StringOption, Check.Required(Check.String.IsLengthBetween(3, 100)));
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
        Rule(x => x.Id, Check.Guid.IsNotEmpty);
    }
}

public sealed class ReadmeExampleTest
{
    [Fact]
    public void DoTheTest()
    {
        var validator = new AttendeeValidator();

        ValidationResult<Attendee>
            .From<AttendeeValidator>(new(
                Name: "John Doe",
                Age: 30,
                Email: Option<string>.Some("john@doe.com"),
                AlternateEmail: Option<string>.None()))
            .Match(
                x => Assert.Equal("John Doe", x.Name),
                e => Assert.Fail("Input is valid, but validation failed"));

        ValidationOption<Attendee>
            .From<AttendeeValidator>(new(
                Name: "John Doe",
                Age: 30,
                Email: Option<string>.Some("john@doe.com"),
                AlternateEmail: Option<string>.None()))
            .Match(
                some: x => Console.WriteLine("Input is valid: {0}", x),
                none: () => Console.WriteLine("Input is invalid"));

        ValidationResult<Attendee>
            .From<AttendeeValidator>(new(
                Name: "",
                Age: -1,
                Email: Option<string>.NoneValue,
                AlternateEmail: Option<string>.Some("invalid_email")))
            .Match(
                x => Assert.Fail("Input is invalid, but validation succeeded"),
                e => Assert.True(e.Any()));
    }

    public record Attendee(
        string Name,
        int Age,
        Option<string> Email,
        Option<string> AlternateEmail);

    public sealed class AttendeeValidator : BaseValidator<Attendee>
    {
        public AttendeeValidator()
        {
            Rule("Name", x => x.Name, Check.String.IsNotEmpty);
            Rule("Age", x => x.Age, Check.IsGreaterThan(0));
            Rule("Email", x => x.Email, Check.Required(Check.String.IsEmailAddress));
            Rule("AlternateEmail", x => x.AlternateEmail, Check.Optional(Check.String.IsEmailAddress));
        }
    }

}
