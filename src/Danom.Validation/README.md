# Danom.Validation
[![NuGet Version](https://img.shields.io/nuget/v/Danom.Validation.svg)](https://www.nuget.org/packages/Danom.Validation)
[![build](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml/badge.svg)](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml)

Danom.Validation is a library that provides an API for defining validation rules and checking input data against those rules, returning a `Result<T, ResultErrors>` that contains either the validated data or an error message.

## Key Features

- Built-in validation checks for common scenarios (e.g., string length, numeric ranges, email format, etc.)
- Support for optional fields using the `Option<T>` type
- Support for validating collections and nested objects
- Composable validation rules for complex scenarios
- Clear and informative error messages using `ResultErrors`

# Design Goals

- The API is designed to be simple and easy to use, with a minimal learning curve.
- Validation rules can be composed to create more complex validation scenarios.
- New validation checks can be easily added to the library.

## Getting Started

Install the [Danom.Validation](https://www.nuget.org/packages/Danom.Validation/) NuGet package:

```
PM>  Install-Package Danom.Validation
```

Or using the dotnet CLI
```cmd
dotnet add package Danom.Validation
```

## Quick Start

The example below demonstrates _most_ the functionality delivered from this slim API built on top of Danom.

```csharp
using Danom;
using Danom.Validation;

// Attendee record with some optional fields and a collection
public record Attendee(
    string Name,
    int Age,
    string Phone,
    Option<string> Email,
    Option<string> MailingAddress,
    IEnumerable<string> Interests);

// Validator for the Attendee record
public sealed class AttendeeValidator : BaseValidator<Attendee>
{
    public AttendeeValidator()
    {
        Rule("Name", x => x.Name,
            Check.String.IsNotEmpty);

        // multiple checks in sequence
        Rule("Age", x => x.Age, [
            Check.IsGreaterThan(0),
            Check.IsLessThan(120) ];

        // built-in check for E.164 phone number format
        Rule("Phone", x => x.Phone,
            Check.String.IsE164);

        // required optional field (must be Some and valid)
        // using built-in email check
        Required("Email", x => x.Email, Check.String.IsEmailAddress);

        // optional field (can be None or Some and valid)
        // demonstrating use of multiple optional checks
        Optional("Mailing Address", x => x.MailingAddress, [
            Check.String.IsNotEmpty,
            Check.String.IsLengthOrGreaterThan(10) ]);

        // collection check, ensuring each item in the collection
        // is at least 2 characters long
        ForEach("Interests", x => x.Interests, Check.String.IsLengthOrGreaterThan(2));
    }
}

var validator = new AttendeeValidator();

// validate an instance of Attendee
validator.Validate(new(
    Name: "John Doe",
    Age: 30,
    Phone: "+14055551234",
    Email: Option<string>.Some("john@doe.com"),
    MailingAddress: Option.Some("123 Main St, Toronto, Ontario, Canada"),
    Interests: ["C#", "ASP.NET"]))
    .Match(
        ok: x => Console.WriteLine("Input is valid: {0}", x),
        error: e => Console.WriteLine("Input is invalid: {0}", e));

// or, if you don't care about the error message(s) use Result.ToOption() to
// flatten the result to an Option<T>
validator.Validate(new(
    Name: "",
    Age: -1,
    Phone: "123",
    Email: Option<string>.NoneValue,
    MailingAddress: Option<string>.Some("invalid mailing address"),
    Interests: ["a"]))
    .ToOption()
    .Match(
        some: x => Console.WriteLine("Input is valid: {0}", x),
        none: () => Console.WriteLine("Input is invalid"));
```

## A Closer Look

Validation is defined by creating a validator class that specifies rules for each field or property of the type being validated.

```csharp
public sealed record Person(string Name, int Age);

public class PersonValidator : BaseValidator<Person>
{
    public PersonValidator()
    {
        Rule("Name", x => x.Name, Check.String.IsNotEmpty);
        Rule("Age", x => x.Age, Check.IsGreaterThan(0));
    }
}
```

Once created, the validator can be used to validate instances of that type using the `Validate(T input)` method.

```csharp
var input = new Person("John", 30);
var validator = new PersonValidator();
var result = validator.Validate(input);

result.Match(
    ok: x => Console.WriteLine("Valid person: {0}", x),
    error: e => Console.WriteLine("Invalid person: {0}", e));
```

There is also a static `Validate<T>` class that provides a convenient way to validate input using a specified validator type or factory method.

```csharp
var input = new Person("John", 30);
var result = Validate<Person>.Using<PersonValidator>(input);

result.Match(
    ok: x => Console.WriteLine("Valid person: {0}", x),
    error: e => Console.WriteLine("Invalid person: {0}", e));
```

### Adding Rules

Base class for creating custom validators. Inherit and define rules in the constructor. Adding rules usually involves a field name, a selector function, and one or more rules. The field name is ultimately optional, but is used in error messages to identify which field failed validation. It is recommended to provide a field name for better error reporting.

```csharp
// a rule using the built-in email check
Rule("Email", x => x.Email, Check.String.IsEmailAddress);

// a rule checking a sequence of values
Rule("Tags", x => x.Tags, Check.Enumerable.ForEach(Check.String.IsNotEmpty));
// or,
ForEach("Tags", x => x.Tags, Check.String.IsNotEmpty);

// a rule for an optional field
Rule(x => x.OptionalAge, Check.Optional(Check.IsGreaterThan(18)));
// or,
Optional("OptionalAge", x => x.OptionalAge, Check.IsGreaterThan(18));
```

## Built-in Rules

All rules are available via the static `Check` class.

### Equality & Comparison

```csharp
Check.IsEqualTo(value)
Check.IsNotEqualTo(value)
Check.IsBetween(min, max)
Check.IsNotBetween(min, max)
Check.IsGreaterThan(threshold)
Check.IsGreaterThanOrEqualTo(threshold)
Check.IsLessThan(threshold)
Check.IsLessThanOrEqualTo(threshold)
Check.IsPositive<T>()
Check.IsNegative<T>()
Check.IsZero<T>()
```

**Example:**
```csharp
Rule("Score", x => x.Score, Check.IsBetween(0, 100));
```

### Option Rules

For `Option<T>` types.

```csharp
// Must be Some
Check.Required<T>()
// Must be Some and satisfy rule
Check.Required(rule)
// Must be Some and satisfy all rules
Check.Required(rule1, rule2, ...)

// Always valid (Some or None)
Check.Optional<T>()
// If Some, must satisfy rule
Check.Optional(rule)
// If Some, must satisfy all rules
Check.Optional(rule1, rule2, ...)
```

**Example:**
```csharp
Rule(x => x.OptionalAge, Check.Optional(Check.IsGreaterThan(18)));
```

### Nested Validation

```csharp
Check.IsValid(validator)
```

**Example:**
```csharp
Rule("Address", x => x.Address, Check.IsValid(new AddressValidator()));
```

### String Rules

```csharp
Check.String.IsEmpty
Check.String.IsNotEmpty
Check.String.IsStartingWith(prefix)
Check.String.IsEndingWith(suffix)
Check.String.IsContaining(substring)
Check.String.IsLength(length)
Check.String.IsLengthBetween(min, max)
Check.String.IsLengthGreaterThan(min)
Check.String.IsLengthOrGreaterThan(min)
Check.String.IsLengthLessThan(max)
Check.String.IsLengthOrLessThan(max)
Check.String.IsMatch(pattern, message?)
Check.String.IsUrl
Check.String.IsE164
Check.String.IsEmailAddress
```

**Example:**
```csharp
Rule("Email", x => x.Email, Check.String.IsEmailAddress);
```

### Guid Rules

```csharp
Check.Guid.IsEmpty
Check.Guid.IsNotEmpty
```

**Example:**
```csharp
Rule("Id", x => x.Id, Check.Guid.IsNotEmpty);
```

### Enumerable Rules

```csharp
Check.Enumerable.IsEmpty<T>()
Check.Enumerable.IsNotEmpty<T>()
Check.Enumerable.ForEach(rule)
Check.Enumerable.ForEach(validator)
```

```csharp
Rule("Tags", x => x.Tags, Check.Enumerable.ForEach(Check.String.IsNotEmpty));
```

### Inline Rules

You can also define inline rules using a lambda expression that takes the field value and returns a `Result`.

**Example:**
```csharp
Rule("FieldName", x => x.FieldName, fieldValue => fieldValue =>
    fieldValue == someCondition ? Result.Error("Error message") : Result.Ok());
```

Rules are driven by two delegates, `ValidatorRule<T>` and `LabelledValidatorRule<T>`. This stack ultimately produces a nested function chain that looks like `Func<T, Func<string, Result<Unit, ResultErrors>>>`.

## Detailed Example

```csharp
public readonly record struct TestInputId(Guid Id);

public sealed class TestInput {
    public TestInputId Id { get; init; } = new(Guid.Empty);
    public string Label { get; init; } = string.Empty;
    public int IntValue { get; init; } = -1;
    public Option<int> OptionalIntValue { get; init; } = Option<int>.Some(-1);
    public Option<string> StringOption { get; init; } = Option<string>.NoneValue;
    public string Email { get; init; } = string.Empty;
    public IEnumerable<string> Phones { get; init; } = [];
    public IEnumerable<string> AlternateEmails { get; init; } = [];
}

public sealed class TestInputIdValidator : BaseValidator<TestInputId> {
    public TestInputIdValidator() {
        Rule("Id", x => x.Id, Check.Guid.IsNotEmpty);
    }
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
        Optional(x => x.OptionalIntValue,
            Check.IsGreaterThanOrEqualTo(1));

        // optional, multiple rules
        Optional("OptionalIntValue", x => x.OptionalIntValue, [
            Check.IsGreaterThanOrEqualTo(1),
            Check.IsLessThanOrEqualTo(100)]);

        // required, single rule
        Required(x => x.StringOption,
            Check.String.IsLengthBetween(3, 100));

        // required, multiple rules
        Required("StringOption", x => x.StringOption, [
            Check.String.IsNotEmpty,
            Check.String.IsLengthBetween(3, 100) ]);

        // collection rules
        Rule("Phones", x => x.Phones, [
            Check.Enumerable.IsNotEmpty<string>(),
            Check.Enumerable.ForEach(Check.String.IsE164) ]);

        // email rule
        Rule("Email", x => x.Email,
            Check.String.IsEmailAddress);

        // collection single rule
        ForEach("AlternateEmails", x => x.AlternateEmails,
            Check.String.IsEmailAddress);
    }
}
```

## Result Handling

Validation returns a `Result<T, ResultErrors>`.
- Use `.Match(ok: ..., error: ...)` to handle both cases.
- Use `.TryGet(out var value)` to get the valid value.
- Use `.TryGetError(out var errors)` to get error details.

```csharp
var result = Validate<User>.Using<UserValidator>(user);

if (result.TryGet(out var user))
{
    // Valid!
}
else if (result.TryGetError(out var errors))
{
    // Handle errors
}
```

For more advanced usage, see the included unit tests in the repository.

## Find a bug?

There's an [issue](https://github.com/pimbrouwers/Danom/issues) for that.

## License

Licensed under [MIT](https://github.com/pimbrouwers/Danom/blob/master/LICENSE).
