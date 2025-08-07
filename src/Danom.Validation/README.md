# Danom.Validation
[![NuGet Version](https://img.shields.io/nuget/v/Danom.Validation.svg)](https://www.nuget.org/packages/Danom.Validation)
[![build](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml/badge.svg)](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml)

One of the places the `Result` type really shines is input validation. It's a natural step in most workflows to validate input data before processing it, and the `Result` type is a great way to handle this. The [Danom.Validation](https://www.nuget.org/packages/Danom.Validation/) library provides an API for defining validation rules and checking input data against those rules, returning a `Result<T, ResultErrors>` that contains either the validated data or an error message.

## Getting Started

Install the [Danom.Validation](https://www.nuget.org/packages/Danom.Validation/) NuGet package:

```
PM>  Install-Package Danom.Validation
```

Or using the dotnet CLI
```cmd
dotnet add package Danom.Validation
```

## Example

The example below demonstrates the functionality delivered from this slim API built on top of Danom.

```csharp
using Danom;
using Danom.Validation;

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

Validate<Attendee>
    .Using<AttendeeValidator>(new(
        Name: "John Doe",
        Age: 30,
        Email: Option<string>.Some("john@doe.com"),
        AlternateEmail: Option<string>.None()))
    .Match(
        ok: x => Console.WriteLine("Input is valid: {0}", x),
        error: e => Console.WriteLine("Input is invalid: {0}", e));

// or, if you don't care about the error messages
Validate<Attendee>
    .Using<AttendeeValidator>(new(
        Name: "John Doe",
        Age: 30,
        Email: Option<string>.Some("john@doe.com"),
        AlternateEmail: Option<string>.None()))
    .ToOption()
    .Match(
        some: x => Console.WriteLine("Input is valid: {0}", x),
        none: () => Console.WriteLine("Input is invalid"));
```

## Find a bug?

There's an [issue](https://github.com/pimbrouwers/Danom/issues) for that.

## License

Licensed under [MIT](https://github.com/pimbrouwers/Danom/blob/master/LICENSE).
