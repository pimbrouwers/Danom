<div align="center">

![Danom](https://github.com/pimbrouwers/Danom/blob/master/assets/banner.png?raw=true)

[![NuGet Version](https://img.shields.io/nuget/v/Danom.svg)](https://www.nuget.org/packages/Danom)
[![build](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml/badge.svg)](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml)
[![license](https://img.shields.io/github/license/pimbrouwers/Danom.svg)](https://github.com/pimbrouwers/Danom/blob/master/LICENSE)
![aot](https://img.shields.io/badge/aot-compatible-green.svg)
![net8.0](https://img.shields.io/badge/net-8.0-blue.svg)
![net6.0](https://img.shields.io/badge/net-6.0-blue.svg)
![netstandard2.1](https://img.shields.io/badge/netstandard-2.1-blue.svg)

</div>

[Danom](src/Danom/README.md) provides Option, Result and Choice types for C#, inspired by F#. It’s designed to be easy to use, efficient, and compatible with existing C# codebases. These discriminated unions offer a type-safe way to represent nullable values, expected errors and decisions, while also supporting a fluent API (e.g., map, bind) for chaining operations and value transformations.

```csharp
using Danom;

// Option: Represent nullable values safely
Option.Some(5)
    .Map(x => x * 2)
    .Match(
        some: x => Console.WriteLine($"Value: {x}"),
        none: () => Console.WriteLine("No value"));
    // ^-- Output: Value: 10

// Result: Handle success or error outcomes
Result<int, string>.Ok(10)
    .Map(x => x + 1)
    .Match(
        ok: x => Console.WriteLine($"Result: {x}"),
        error: e => Console.WriteLine($"Error: {e}"));
    // ^-- Output: Result: 11

// Choice: Represent one of multiple types
Choice<int, string, double>.FromT2("Hello")
    .Match(
        t1: x => Console.WriteLine($"Got int: {x}"),
        t2: s => Console.WriteLine($"Got string: {s}"),
        t3: d => Console.WriteLine($"Got double: {d}"));
    // ^-- Output: Got string: Hello
```

## Key Features

- Implementation of common monads: [Option](src/Danom/README.md#option), [Result](src/Danom/README.md#result) and [Choice](src/Danom/README.md#choice).
- [Unit](src/Danom/README.md#unit) type to represent the absence of a value.
- Exhaustive matching to prevent null reference exceptions.
- Fluent API for chaining operations, including async support.
- Built-in error handling with [ResultErrors](src/Danom/README.md#built-in-error-type).
- An API for [parsing strings](src/Danom/README.md#string-parsing) into .NET primitives and value types.
- Input validation via [Danom.Validation](#input-validation).
- Integrations with [ASP.NET MVC](#aspnet-core-mvc--razor-pages-integration) and [ASP.NET Minimal API](#aspnet-core-minimal-api-integration).

## Design Goals

- `netstandard2.1` compatible.
- Simplify and enhance functional programming in C#.
- Support both synchronous and asynchronous operations.
- Provide opionated monads to encourage consistent use.

## Getting Started

Install the [Danom](https://www.nuget.org/packages/Danom/) NuGet package:

```cmd
dotnet add package Danom

OR

PM>  Install-Package Danom
```

Documentation for Danom can be found [here](src/Danom/README.md).

### Looking for Validation or ASP.NET Core Integration?

- Get started with [Danom.Validation](src/Danom.Validation/README.md)
- Get started with [Danom.MinimalApi](src/Danom.MinimalApi/README.md)
- Get started with [Danom.Mvc](src/Danom.Mvc/README.md)

## Input Validation

One of the places the [Result](src/Danom/README.md#result) type really shines is [input validation](src/Danom.Validation/README.md). It's a natural step in most workflows to validate input data before processing it, and the `Result` type is a great way to handle this. The [Danom.Validation](src/Danom.Validation/README.md) library provides an API for defining validation rules and checking input data against those rules, returning a `Result<T, ResultErrors>` that contains either the validated data or an error message.

## Integrations

Since Danom introduces types that are most commonly found in your model and domain layers, external integrations are not only inevitable but required to provide a seamless experience when building applications.

These are completely optional, but provide a great way to integrate Danom with your codebase.

### ASP.NET Core MVC & Razor Pages Integration

Danom is integrated with ASP.NET Core MVC (and Razor Pages) via [Danom.Mvc](src/Danom.Mvc/README.md). This library provides extension methods and a base controller to help integrate the core types with common tasks in ASP.NET Core MVC applications.

### ASP.NET Core Minimal API Integration

Danom is integrated with ASP.NET Core Minimal API via [Danom.MinimalApi](src/Danom.MinimalApi/README.md). This library provides a set of utilities to help integrate the core types with common tasks in ASP.NET Core Minimal API applications.

## Changelog

All notable changes to this project will be documented in these files:

- [Danom CHANGELOG](src/Danom/CHANGELOG.md) - Core library changelog.
- [Danom.Validation CHANGELOG](src/Danom.Validation/CHANGELOG.md) - Validation library changelog.
- [Danom.Mvc CHANGELOG](src/Danom.Mvc/CHANGELOG.md) - MVC integration changelog.
- [Danom.MinimalApi CHANGELOG](src/Danom.MinimalApi/CHANGELOG.md) - Minimal API integration changelog.

## Contributing

I kindly ask that before submitting a pull request, you first submit an [issue](https://github.com/pimbrouwers/Danom/issues).

If functionality is added to the API, or changed, please kindly update the relevant documentation. Unit tests must also be added and/or updated before a pull request can be successfully merged.

Only pull requests which pass all build checks and comply with the general coding standard can be approved.

## Find a bug?

There's an [issue](https://github.com/pimbrouwers/Danom/issues) for that.

## License

Licensed under [MIT](https://github.com/pimbrouwers/Danom/blob/master/LICENSE).
