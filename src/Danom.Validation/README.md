# Danom.Validation
[![NuGet Version](https://img.shields.io/nuget/v/Danom.Validation.svg)](https://www.nuget.org/packages/Danom.Validation)
[![build](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml/badge.svg)](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml)

One of the places the Results type really shines is input validation. It's a natural step in most workflows to validate input data before processing it, and the Result type is a great way to handle this. The (Danom.Validation)[https://www.nuget.org/packages/Danom.Validation/] library provides a set of utilities to help with this and integrates with the wonderful (FluentValidation)[https://github.com/FluentValidation/FluentValidation] library.

## Getting Started

Install the [Danom.Validation](https://www.nuget.org/packages/Danom.Validation/) NuGet package:

```
PM>  Install-Package Danom.Validation
```

Or using the dotnet CLI
```cmd
dotnet add package Danom.Validation
```

### Quick Start

```csharp
using Danom;
using Danom.Validation;
using FluentValidation;

public record Attendee(
    string Name,
    int Age,
    Option<string> Email,
    Option<string> AlternateEmail);

public class AttendeeValidator
    : AbstractValidator<Attendee>
{
    public AttendeeValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Age).GreaterThan(0);
        RuleFor(x => x.Email).Required(x => x.EmailAddress());
        RuleFor(x => x.AlternateEmail).Optional(x => x.EmailAddress());
    }
}
var input =
    new Attendee(
        Name: "John Doe",
        Age: 30,
        Email: Option<string>.Some("john@doe.com"),
        AlternateEmail: Option<string>.None());

var result =
    ValidationResult<Attendee>
        .From<AttendeeValidator>(input);

result.Match(
    x => Console.WriteLine("Input is valid: {0}", x),
    e => Console.WriteLine("Input is invalid: {0}", e));
```
