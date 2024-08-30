# Danom
[![NuGet Version](https://img.shields.io/nuget/v/Danom.svg)](https://www.nuget.org/packages/Danom)
[![build](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml/badge.svg)](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml)

Danom is a C# library that provides monadic structures to simplify functional programming patterns in C#, that enforces exhaustive matching by preventing direct value access (this is good).

## Key Features
- Implementation of common monads like [Option](#option), [Result](#result), and [ResultOption](#resultoption).
- Fluent API for chaining operations.
- [Error handling](#using-results) with monads.
- Support for asynchronous operations.
- Integrated with [ASP.NET Core](#aspnet-core-mvc-integration) and [Fluent Validation](#fluent-validation-integration).

## Design Goals
- **Simplicity**: Easy to use API for common monadic operations.
- **Performance**: Efficient implementation to minimize overhead.
- **Interoperability**: Seamless integration with existing C# code and libraries.
- **Durability**: Prevent direct use of internal value, enforcing exhaustive matching.

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

// Create an Option
var option = Option<int>.Some(5);

option.Match(
    some: x => Console.WriteLine("Value: {0}", x),
    none: () => Console.WriteLine("No value"));

// Create a Result
public Result<int, string> TryDivide(int numerator, int denominator) =>
    denominator == 0
        ? Result<int, string>.Error("Cannot divide by zero")
        : Result<int, string>.Ok(numerator / denominator);

TryDivide(10, 2)
    .Match(
        ok: x => Console.WriteLine("Result: {0}", x),
        error: e => Console.WriteLine("Error: {0}", e));
```

## Option

Represents when an actual value might not exist for a value or named variable. An option has an underlying type and can hold a value of that type, or it might not have a value. Options are a fantastic means of reducing primitive congestion in your code, and they are a much safer way to handle null values and virutally eliminate null reference exceptions.

### Creating Options

```csharp
var option = Option<int>.Some(5);

// or, with no value
var optionNone = Option<int>.None();

// also returns none
var optionNull = Option<object>.Some(default!);
```

### Using Option

Options are commonly used when a operation might not return a value. For example:

```csharp
public Option<int> TryFind(IEnumerable<int> numbers, Func<int, bool> predicate) =>
    numbers.FirstOrDefault(predicate).ToOption();
```

With this method defined we can begin performing operations against the Option result:

```csharp
IEnumerable<int> nums = [1,2,3];

// Exhasutive matching
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

Represents the result of an operation that can either succeed or fail. These results can be chained together allowing you to form error-tolerant pipelines. This lets you break up functionality like this into small pieces which are as composable as you need them to be. Also benefiting from the exhaustive matching.

### Creating Results

```csharp
var result = Result<int, string>.Ok(5);
// or, with an error
var resultError = Result<int, string>.Error("An error occurred");
// or, using the built-in Error type
var resultErrors = Result<int>.Ok(5);
var resultErrorsError = Result<int>.Error("An error occurred");
var resultErrorsMultiError = Result<int>.Error(["An error occurred", "Another error occurred"]);
var resultErrorsTyped = Result<int>.Error(new ResultErrors("error-key", "An error occurred"));
```

### Using Results

Results are commonly used when an operation might not succeed, and you want to manage the _expected_ errors. For example:

```csharp
public Result<int, string> TryDivide(int numerator, int denominator) =>
    denominator == 0
        ? Result<int, string>.Error("Cannot divide by zero")
        : Result<int, string>.Ok(numerator / denominator);
```

With this method defined we can begin performing operations against the Result result:

```csharp
// Exhasutive matching
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

Since error messages are frequently represented as string collections, often with keys (e.g., for validation), the `ResultErrors` type is provided to simplify Result creation. The flexible constructor allows errors to be initialized with a single string, a collection of strings, or a key-value pair.

```csharp
Result<int, ResultErrors> resultErrors = Result<int>.Ok(5);
Result<int, ResultErrors> resultErrorsError = Result<int>.Error("An error occurred");
Result<int, ResultErrors> resultErrorsMultiError = Result<int>.Error(["An error occurred", "Another error occurred"]);
Result<int, ResultErrors> resultErrorsTyped = Result<int>.Error(new ResultErrors("error-key", "An error occurred"));
```

## ResultOption

Represents a combination of the Result and Option monads. This is useful when you want to handle both the success and failure of an operation, but also want to handle the case where a value might not exist. It simplifies the inspection by eliminating the redundant nested `Match` calls.

### Creating ResultOptions

```csharp
var resultOption = ResultOption<int, string>.Ok(5);
// or, with an error
var resultOptionError = ResultOption<int, string>.Error("An error occurred");
// or, with no value
var resultOptionNone = ResultOption<int, string>.None();
```

### Using ResultOptions

ResultOptions are commonly used when an operation might not succeed, but also where a value might not exist. For example:

```csharp
public Option<int> LookupUserId(string username) => // ...

public ResultOption<int, string> GetUserId(string username)
{
    if(username == "admin")
    {
        return ResultOption<int,string>.Error("Invalid username");
    }

    return LookupUserId(username).Match(
        some: id => ResultOption<int, string>.Ok(1) :
        none: ResultOption<int, string>.None);

    // or, using the extension method
    // return LookupUserId(username).ToResultOption();
}
```

## Integrations

Since Danom introduces types that are most commonly found in your model and business logic layers, external integrations are not only inevitable but required to provide a seamless experience when build applications.

### Fluent Validation Integration

Danom is integrated with [Fluent Validation](https://fluentvalidation.net/) to provide a seamless way to validate your models and return a `Result` or `ResultOption` with the validation errors.

Documentation can be found [here](src/Danom.Validation/README.md).

### ASP.NET Core MVC Integration

Danom is integrated with [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0) to provide a set of utilities to help integrate the Danom library with common tasks in ASP.NET Core MVC applications.

Documentation can be found [here](src/Danom.Mvc/README.md).

## Contribute

Thank you for considering contributing to Danom, and to those who have already contributed! We appreciate (and actively resolve) PRs of all shapes and sizes.

We kindly ask that before submitting a pull request, you first submit an [issue](https://github.com/pimbrouwers/Danom/issues) or open a [discussion](https://github.com/pimbrouwers/Danom/discussions).

If functionality is added to the API, or changed, please kindly update the relevant [document](https://github.com/pimbrouwers/Danom/tree/master/docs). Unit tests must also be added and/or updated before a pull request can be successfully merged.

Only pull requests which pass all build checks and comply with the general coding guidelines can be approved.

If you have any further questions, submit an [issue](https://github.com/pimbrouwers/Danom/issues) or open a [discussion](https://github.com/pimbrouwers/Danom/discussions).


## Find a bug?

There's an [issue](https://github.com/pimbrouwers/Danom/issues) for that.

## License

Built with ♥ by [Pim Brouwers](https://github.com/pimbrouwers) in Toronto, ON. Licensed under [Apache License 2.0](https://github.com/pimbrouwers/Danom/blob/master/LICENSE).
