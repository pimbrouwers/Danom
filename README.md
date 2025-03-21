# Danom
[![NuGet Version](https://img.shields.io/nuget/v/Danom.svg)](https://www.nuget.org/packages/Danom)
[![build](https://github.com/eastcitysoftware/danom/actions/workflows/build.yml/badge.svg)](https://github.com/eastcitysoftware/danom/actions/workflows/build.yml)

Danom is a C# library that provides (monadic) structures to facilitate durable programming patterns in C# using [Option](#option) and [Result](#result).

## Key Features

- Implementation of common monads: [Option](#option) and [Result](#result).
- Exhaustive matching to prevent null reference exceptions.
- Fluent API for chaining operations, including async support.
- Integrated with [ASP.NET Core](#aspnet-core-mvc-integration) and [Fluent Validation](#fluent-validation-integration).
- API for [parsing strings](#string-parsing) into .NET primitives and value types.

## Design Goals

- Provide a safe and expressive way to handle nullable values.
- Prevent direct use of internal value, enforcing exhaustive matching.
- Efficient implementation to minimize overhead.
- Opionated monads to encourage consistent use.

## Getting Started

Install the [Danom](https://www.nuget.org/packages/Danom/) NuGet package:

```
PM>  Install-Package Danom
```

Or using the dotnet CLI
```cmd
dotnet add package Danom
```

### Quick Start

```csharp
using Danom;

// Working with Option type
var option = Option<int>.Some(5);

option.Match(
    some: x => Console.WriteLine("Value: {0}", x),
    none: () => Console.WriteLine("No value"));

// Working with Result type
public Result<int, string> TryDivide(
    int numerator,
    int denominator) =>
    denominator == 0
        ? Result<int, string>.Error("Cannot divide by zero")
        : Result<int, string>.Ok(numerator / denominator);

TryDivide(10, 2)
    .Match(
        ok: x => Console.WriteLine("Result: {0}", x),
        error: e => Console.WriteLine("Error: {0}", e));
```

## Option

Options have an underlying type and can optionally hold a value of that type. Options are a much safer way to handle nullable values, they virtually eliminate null reference exceptions. They also provide a fantastic means of reducing primitive congestion in your code.

### Creating Options

```csharp
var option = Option<int>.Some(5);

// or, with type inference
var optionInferred = Option.Some(5);

// or, with no value
var optionNone = Option<int>.None();

// also returns none
var optionNull = Option<object>.Some(default!);
```

### Using Option

Options are commonly used when a operation might not return a value. For example, the method below tries to find a number in a list that satisfies a predicate. If the number is found, it is returned as a `Some`, otherwise, `None` is returned.

```csharp
public Option<int> TryFind(IEnumerable<int> numbers, Func<int, bool> predicate) =>
    numbers.FirstOrDefault(predicate).ToOption();
```

With this method defined we can begin performing operations against the Option result:

```csharp
IEnumerable<int> nums = [1,2,3];

// Exhaustive matching
TryFind(nums, x => x == 1)
    .Match(
        some: x => Console.WriteLine("Found: {0}", x),
        none: () => Console.WriteLine("Did not find number"));

// Mapping the value
Option<int> optionSum =
    TryFind(nums, x => x == 1)
        .Map(x => x + 1);

// Binding the option
Option<int> optionBindSum =
    TryFind(nums, x => x == 1)
        .Bind(num1 =>
            TryFind(nums, x => x == 2)
                .Map(num2 => num1 + num2));

// Handling "None"
Option<int> optionDefault =
    TryFind(nums, x => x == 4)
        .DefaultValue(99);

Option<int> optionDefaultWith =
    TryFind(nums, x => x == 4)
        .DefaultWith(() => 99); // useful if creating the value is expensive

Option<int> optionOrElse =
    TryFind(nums, x => x == 4)
        .OrElse(Option<int>.Some(99));

Option<int> optionOrElseWith =
    TryFind(nums, x => x == 4)
        .OrElseWith(() => Option<int>.Some(99)); // useful if creating the value is expensive
```

## Result

Results are used to represent a success or failure outcome. They provide a more concrete way to manage the expected errors of an operation, then throwing exceptions. Especially in recoverable or reportable scenarios.

### Creating Results

```csharp
var result = Result<int, string>.Ok(5);

// or, with an error
var resultError = Result<int, string>.Error("An error occurred");

// or, using the built-in Error type
var resultErrors = Result<int>.Ok(5);

var resultErrorsError = Result<int>.Error(new("An error occurred"));
var resultErrorsMultiError = Result<int>.Error(new(["An error occurred", "Another error occurred"]));
var resultErrorsTyped = Result<int>.Error(new("error-key", "An error occurred"));
```

### Using Results

Results are commonly used when an operation might not succeed, and you want to manage or report back the _expected_ errors. For example:

```csharp
public Result<int, string> TryDivide(int numerator, int denominator) =>
    denominator == 0
        ? Result<int, string>.Error("Cannot divide by zero")
        : Result<int, string>.Ok(numerator / denominator);
```

With this method defined we can begin performing operations against the Result result:

```csharp
// Exhaustive matching
TryDivide(10, 2)
    .Match(
        ok: x => Console.WriteLine("Result: {0}", x),
        error: e => Console.WriteLine("Error: {0}", e));

// Mapping the value
Result<int, string> resultSum =
    TryDivide(10, 2)
        .Map(x => x + 1);

// Binding the result (i.e., when a nested operation also returns a Result)
Result<int, string> resultBindSum =
    TryDivide(10, 2)
        .Bind(num1 =>
            TryDivide(20, 2)
                .Map(num2 =>
                    num1 + num2));

// Handling errors
Result<int, string> resultDefault =
    TryDivide(10, 0)
        .DefaultValue(99);

Result<int, string> resultDefaultWith =
    TryDivide(10, 0)
        .DefaultWith(() => 99); // useful if creating the value is expensive

Result<int, string> resultOrElse =
    TryDivide(10, 0)
        .OrElse(Result<int, string>.Ok(99));

Result<int, string> resultOrElseWith =
    TryDivide(10, 0)
        .OrElseWith(() =>
            Result<int, string>.Ok(99)); // useful if creating the value is expensive
```

### Result Errors

Since error messages are frequently represented as keyed string collections, the `ResultErrors` type is provided to simplify Result creation. The flexible constructor allows errors to be initialized with a single string, a collection of strings, or a key-value pair.

```csharp
var resultErrors =
    Result<int>.Ok(5);

var resultErrorsError =
    Result<int>.Error(new("An error occurred"));

var resultErrorsMultiError =
    Result<int>.Error(new(["An error occurred", "Another error occurred"]));

var resultErrorsTyped =
    Result<int>.Error(new ResultErrors("error-key", "An error occurred"));
```

## String Parsing

Most applications will at some point need to parse strings into primitives and value types. This is especially true when working with external data sources.

`Option` provides a natural mechanism to handle the case where the string cannot be parsed. The "TryParse" API is provided to simplify the process of parsing strings into .NET primitives and value types.

```csharp
using Danom;

// a common pattern
var x = int.TryParse("123", out var y) ? Option<int>.Some(y) : Option<int>.None();

// or, more simply using the TryParse API
var myInt = intOption.TryParse("123"); // -> Some(123)
var myDouble = doubleOption.TryParse("123.45"); // -> Some(123.45)
var myBool = boolOption.TryParse("true"); // -> Some(true)

// if the string cannot be parsed
var myIntNone = intOption.TryParse("danom"); // -> None
var myDoubleNone = doubleOption.TryParse("danom"); // -> None
var myBoolNone = boolOption.TryParse("danom"); // -> None

// null strings are treated as None
var myIntNull = intOption.TryParse(null); // -> None
```

The full API is below:

```csharp
public static class boolOption {
    public static Option<bool> TryParse(string? x); }

public static class byteOption {
    public static Option<byte> TryParse(string? x, IFormatProvider? provider = null); }

public static class shortOption {
    public static Option<short> TryParse(string? x, IFormatProvider? provider = null);
    public static Option<short> TryParse(string? x); }

public static class intOption {
    public static Option<int> TryParse(string? x, IFormatProvider? provider = null);
    public static Option<int> TryParse(string? x); }

public static class longOption {
    public static Option<long> TryParse(string? x, IFormatProvider? provider = null);
    public static Option<long> TryParse(string? x); }

public static class decimalOption {
    public static Option<decimal> TryParse(string? x, IFormatProvider? provider = null);
    public static Option<decimal> TryParse(string? x); }

public static class doubleOption {
    public static Option<double> TryParse(string? x, IFormatProvider? provider = null);
    public static Option<double> TryParse(string? x); }

public static class floatOption {
    public static Option<float> TryParse(string? x, IFormatProvider? provider = null);
    public static Option<float> TryParse(string? x); }

public static class GuidOption {
    public static Option<Guid> TryParse(string? x, IFormatProvider? provider = null);
    public static Option<Guid> TryParse(string? x);
    public static Option<Guid> TryParseExact(string? x, string? format); }

public static class DateTimeOffsetOption {
    public static Option<DateTimeOffset> TryParse(string? x, IFormatProvider? provider = null);
    public static Option<DateTimeOffset> TryParse(string? x);
    public static Option<DateTimeOffset> TryParseExact(string? x, string? format, IFormatProvider? provider = null, DateTimeStyles dateTimeStyles = DateTimeStyles.None); }

public static class DateTimeOption {
    public static Option<DateTime> TryParse(string? x, IFormatProvider? provider = null);
    public static Option<DateTime> TryParse(string? x);
    public static Option<DateTime> TryParseExact(string? x, string? format, IFormatProvider? provider = null, DateTimeStyles dateTimeStyles = DateTimeStyles.None); }

public static class DateOnlyOption {
    public static Option<DateOnly> TryParse(string? x, IFormatProvider? provider = null);
    public static Option<DateOnly> TryParse(string? x);
    public static Option<DateOnly> TryParseExact(string? x, string? format, IFormatProvider? provider = null, DateTimeStyles dateTimeStyles = DateTimeStyles.None); }

public static class TimeOnlyOption {
    public static Option<TimeOnly> TryParse(string? x, IFormatProvider? provider = null);
    public static Option<TimeOnly> TryParse(string? x);
    public static Option<TimeOnly> TryParseExact(string? x, string? format, IFormatProvider? provider = null, DateTimeStyles dateTimeStyles = DateTimeStyles.None); }

public static class TimeSpanOption {
    public static Option<TimeSpan> TryParse(string? x, IFormatProvider? provider = null);
    public static Option<TimeSpan> TryParse(string? x);
    public static Option<TimeSpan> TryParseExact(string? x, string? format, IFormatProvider? provider = null); }

public static class EnumOption {
    public static Option<TEnum> TryParse<TEnum>(string? x) where TEnum : struct; }
```

## Integrations

Since Danom introduces types that are most commonly found in your model and business logic layers, external integrations are not only inevitable but required to provide a seamless experience when building applications.

### Fluent Validation Integration

[Fluent Validation](https://fluentvalidation.net/) is an excellent library for building validation rules for your models. A first-class integration is available via [Danom.Validation](src/Danom.Validation/README.md) to provide a seamless way to validate your models and return a `Result` or `ResultOption` with the validation errors.

A quick example:

```csharp
using Danom;
using Danom.Validation;
using FluentValidation;

public record Person(
    string Name,
    Option<string> Email);

public class PersonValidator
    : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).Optional(x => x.EmailAddress());
    }
}

var result =
    ValidationResult<Person>
        .From<PersonValidator>(new(
            Name: "John Doe",
            Email: Option.Some("john@doe.com")));

result.Match(
    x => Console.WriteLine("Input is valid: {0}", x),
    e => Console.WriteLine("Input is invalid: {0}", e));
```

Documentation can be found [here](src/Danom.Validation/README.md).

### ASP.NET Core MVC Integration

Danom is integrated with ASP.NET Core via [Danom.Mvc](src/Danom.Mvc/README.md). This library provides a set of utilities to help integrate the core types with common tasks in ASP.NET Core MVC applications.

### ASP.NET Core Minimal API Integration

> Coming soon

## Contribute

Thank you for considering contributing to Danom, and to those who have already contributed! We appreciate (and actively resolve) PRs of all shapes and sizes.

We kindly ask that before submitting a pull request, you first submit an [issue](https://github.com/eastcitysoftware/danom/issues) or open a [discussion](https://github.com/eastcitysoftware/danom/discussions).

If functionality is added to the API, or changed, please kindly update the relevant [document](https://github.com/eastcitysoftware/danom/tree/master/docs). Unit tests must also be added and/or updated before a pull request can be successfully merged.

Only pull requests which pass all build checks and comply with the general coding guidelines can be approved.

If you have any further questions, submit an [issue](https://github.com/eastcitysoftware/danom/issues) or open a [discussion](https://github.com/eastcitysoftware/danom/discussions).


## Find a bug?

There's an [issue](https://github.com/eastcitysoftware/danom/issues) for that.

## License

Licensed under [Apache License 2.0](https://github.com/eastcitysoftware/danom/blob/master/LICENSE).
