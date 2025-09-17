namespace Danom.Validation.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class ValidationTests {
    [Fact]
    public void ReturnsOk_WhenValidationSucceeds() {
        Assert.True(Validate<TestInput>.Using<TestInputValidator>(TestInput.ValidInput).ToOption().IsSome);
    }

    [Fact]
    public void ReturnsError_WhenValidationFails() {
        Assert.True(Validate<TestInput>.Using<TestInputValidator>(TestInput.InvalidInput).ToOption().IsNone);
    }

    [Fact]
    public void VerifyResultErrors() {
        var result = Validate<TestInput>.Using<TestInputValidator>(TestInput.InvalidInput);
        if (result.TryGetError(out var errors)) {
            Assert.NotEmpty(errors);
            Assert.Single(errors["TestInputId"].Errors);

            Assert.Equal(2, errors["Label"].Errors.Count);
            Assert.Contains("'Label'", errors["Label"].Errors[0]);
            Assert.Contains("'Label'", errors["Label"].Errors[1]);

            Assert.Equal(3, errors["IntValue"].Errors.Count);
            Assert.Contains("'IntValue'", errors["IntValue"].Errors[0]);
            Assert.Contains("'IntValue'", errors["IntValue"].Errors[1]);
            Assert.Equal("This is not an acceptable response for 'IntValue'", errors["IntValue"].Errors[2]);

            Assert.Equal(2, errors[""].Errors.Count);
            Assert.Contains("'Value'", errors[""].Errors[0]);
            Assert.Contains("'Value'", errors[""].Errors[1]);

            Assert.Single(errors["Email"].Errors);
            Assert.Contains("'Email'", errors["Email"].Errors[0]);

            Assert.Equal(2, errors["Phones"].Errors.Count);
            Assert.Contains("'Phones'", errors["Phones"].Errors[0]);
            Assert.Contains("'Phones'", errors["Phones"].Errors[1]);
        }
        else {
            Assert.Fail("Expected validation to fail, but it succeeded.");
        }
    }
}

public sealed class TestInput {
    public TestInputId Id { get; init; } = new(Guid.Empty);
    public string Label { get; init; } = string.Empty;
    public int IntValue { get; init; } = -1;
    public Option<int> OptionalIntValue { get; init; } = Option<int>.Some(-1);
    public Option<string> StringOption { get; init; } = Option<string>.NoneValue;
    public string Email { get; init; } = string.Empty;
    public IEnumerable<string> Phones { get; init; } = [];
    public IEnumerable<string> AlternateEmails { get; init; } = [];

    public override string ToString() => IntValue.ToString();

    public static TestInput ValidInput => new() {
        Id = new(Guid.NewGuid()),
        Label = "Valid Label",
        IntValue = 50,
        OptionalIntValue = Option.Some(10),
        StringOption = Option.Some("Valid"),
        Phones = ["+19051234567", "+19057654321"],
        Email = "bob@bob.com",
        AlternateEmails = ["rob@bob.com", "robert@bob.com"]
    };

    public static TestInput InvalidInput => new() {
        Id = new(Guid.Empty),
        Label = string.Empty,
        IntValue = -1,
        OptionalIntValue = Option.Some(-1),
        StringOption = Option<string>.NoneValue,
        Phones = ["a", "b"],
        Email = "a",
        AlternateEmails = ["a", "b"]
    };
}

public sealed class TestInputValidator : BaseValidator<TestInput> {
    public TestInputValidator() {
        // nested validator
        Rule("TestInputId", x => x.Id,
            Check.IsValid(new TestInputIdValidator()));

        // multiple rules with label
        Rule("Label", x => x.Label, [
            Check.String.IsNotEmpty, Check.String.IsLengthGreaterThan(3)]);

        // numeric rules
        Rule("IntValue", x => x.IntValue, [
            Check.IsGreaterThan(0),
            Check.IsPositive<int>()]);

        // inline custom rule
        Rule("IntValue", x => x.IntValue,
            x => field => x == -1 ? Result.Error($"This is not an acceptable response for '{field}'") : Result.Ok());

        // optional, single rule
        Rule(x => x.OptionalIntValue,
            Check.Optional(Check.IsGreaterThanOrEqualTo(1)));

        // optional, multiple rules
        Rule("OptionalIntValue", x => x.OptionalIntValue, Check.Optional([
            Check.IsGreaterThanOrEqualTo(1),
            Check.IsLessThanOrEqualTo(100)]));

        // required, single rule
        Rule(x => x.StringOption,
            Check.Required(Check.String.IsLengthBetween(3, 100)));

        // required, multiple rules
        Rule("StringOption", x => x.StringOption, Check.Required([
            Check.String.IsNotEmpty,
            Check.String.IsLengthBetween(3, 100) ]));

        // collection rules
        Rule("Phones", x => x.Phones, [
            Check.Enumerable.IsNotEmpty<string>(),
            Check.Enumerable.ForEach(Check.String.IsE164) ]);

        // email rule
        Rule("Email", x => x.Email,
            Check.String.IsEmailAddress);

        // collection single rule
        Rule("AlternateEmails", x => x.AlternateEmails,
            Check.Enumerable.ForEach(Check.String.IsEmailAddress));
    }
}

public readonly record struct TestInputId(Guid Id);

public sealed class TestInputIdValidator : BaseValidator<TestInputId> {
    public TestInputIdValidator() {
        Rule("Id", x => x.Id, Check.Guid.IsNotEmpty);
    }
}
