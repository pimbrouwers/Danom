# Danom

![Danom](https://github.com/pimbrouwers/Danom/blob/master/assets/banner.png?raw=true)
[![NuGet Version](https://img.shields.io/nuget/v/Danom.svg)](https://www.nuget.org/packages/Danom)
[![build](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml/badge.svg)](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml)
[![license](https://img.shields.io/github/license/pimbrouwers/Danom.svg)](https://github.com/pimbrouwers/Danom/blob/master/LICENSE)
![aot](https://img.shields.io/badge/aot-compatible-green.svg)
![net8.0](https://img.shields.io/badge/net-8.0-blue.svg)
![net6.0](https://img.shields.io/badge/net-6.0-blue.svg)
![netstandard2.1](https://img.shields.io/badge/netstandard-2.1-blue.svg)

Danom provides Option and Result types for C#, inspired by F#. It’s designed to be easy to use, efficient, and compatible with existing C# codebases. These types offer a type-safe way to represent nullable values and expected errors, while also supporting a fluent API (e.g., map, bind) for chaining operations and value transformations.

## Key Features

- Implementation of common monads: [Option](#option) and [Result](#result).
- Exhaustive matching to prevent null reference exceptions.
- Fluent API for chaining operations, including async support.
- Built-in error handling with [ResultErrors](#built-in-error-type).
- An API for [parsing strings](#string-parsing) into .NET primitives and value types.
- Input validation via [Danom.Validation](src/Danom.Validation/README.md).
- Integration with [ASP.NET MVC](#aspnet-core-mvc-integration) and [ASP.NET Minimal API](src/Danom.MinimalApi/README.md).

## Design Goals

- Provide a safe and expressive way to handle nullable values.
- Efficient implementation to minimize overhead.
- Enforce exhaustive matching.
- Enhance functional programming in C#.
- Opionated monads to encourage consistent use.
- Support for both synchronous and asynchronous operations.
- `netstandard2.1` compatible.

## Getting Started

Install the [Danom](https://www.nuget.org/packages/Danom/) NuGet package:

```cmd
dotnet add package Danom

OR

PM>  Install-Package Danom
```

## Input Validation

One of the places the `Result` type really shines is input validation. It's a natural step in most workflows to validate input data before processing it, and the `Result` type is a great way to handle this. The [Danom.Validation](https://www.nuget.org/packages/Danom.Validation/) library provides an API for defining validation rules and checking input data against those rules, returning a `Result<T, ResultErrors>` that contains either the validated data or an error message.

## Integrations

Since Danom introduces types that are most commonly found in your model and business logic layers, external integrations are not only inevitable but required to provide a seamless experience when building applications.

These are completely optional, but provide a great way to integrate Danom with your codebase.

### ASP.NET Core MVC & Razor Pages Integration

Danom is integrated with ASP.NET Core MVC (and Razor Pages) via [Danom.Mvc](https://nuget.org/packages/Danom.Mvc). This library provides a set of utilities to help integrate the core types with common tasks in ASP.NET Core MVC applications.

### ASP.NET Core Minimal API Integration

Danom is integrated with ASP.NET Core Minimal API via [Danom.MinimalApi](https://nuget.org/packages/Danom.MinimalApi). This library provides a set of utilities to help integrate the core types with common tasks in ASP.NET Core Minimal API applications.

## Find a bug?

There's an [issue](https://github.com/pimbrouwers/Danom/issues) for that.

## License

Licensed under [MIT](https://github.com/pimbrouwers/Danom/blob/master/LICENSE).
