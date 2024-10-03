# Danom.Mvc
[![NuGet Version](https://img.shields.io/nuget/v/Danom.Mvc.svg)](https://www.nuget.org/packages/Danom.Mvc)
[![build](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml/badge.svg)](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml)

Danom.Mvc is a library that provides a set of rendering utilities to help integrate the [Danom](../../README.md) library with common tasks in ASP.NET Core MVC applications.

## Getting Started

Install the [Danom.Mvc](https://www.nuget.org/packages/Danom.Mvc/) NuGet package:

```
PM>  Install-Package Danom.Mvc
```

Or using the dotnet CLI
```cmd
dotnet add package Danom.Mvc
```

## Working With `Option`

```csharp
using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

public sealed class OptionController
    : DanomController
{
    private readonly Option<string> _someOption = Option.Some("Hello world");
    private readonly Option<string> _noneOption = Option<string>.None();

    public IActionResult OptionSome() =>
        ViewOption(
            option: _someOption,
            viewName: "Detail");

    // Returns the ASP.NET default `NotFound` result
    public IActionResult OptionNone() =>
        ViewOption(
            option: _noneOption,
            viewName: "Detail");

    public IActionResult OptionNoneCustom() =>
        ViewOption(
            option: _noneOption,
            viewName: "Detail",
            noneAction: () => NotFound("Not found!"));
}
```

## Working with `Result`

```csharp
using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

public sealed class ResultController
    : DanomController
{
    private readonly Result<string, ResultErrors> _okResult = Result.Ok("Success!");
    private readonly Result<string, ResultErrors> _errorResult = Result<string>.Error(["An error occurred."]);
    private readonly Result<string, string> _stringErrorResult = Result<string, string>.Error("An error occurred.");

    public IActionResult ResultOk() =>
        ViewResult(
            result: _okResult,
            viewName: "Detail");

    public IActionResult ResultErrors() =>
        ViewResult(
            result: _errorResult,
            viewName: "Detail");

    // Demonstrating error customization, using a string literal for error output
    public IActionResult ResultError() =>
        ViewResult(
            result: _stringErrorResult,
            errorAction: errors => View("Detail", errors),
            viewName: "Detail");

}
```

## Working with `ResultOption`

```csharp
using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

public sealed class ResultOptionController
    : DanomController
{
    private readonly ResultOption<string, ResultErrors> _okResult = ResultOption.Ok("Success!");
    private readonly ResultOption<string, ResultErrors> _errorResult = ResultOption<string>.Error("An error occurred.");
    private readonly ResultOption<string, string> _stringErrorResult = ResultOption<string, string>.Error("An error occurred.");

    public IActionResult ResultOk() =>
        ViewResultOption(
            resultOption: _okResult,
            viewName: "Detail");

    public IActionResult ResultErrors() =>
        ViewResultOption(
            resultOption: _errorResult,
            viewName: "Detail");

    // Demonstrating error customization, using a string literal for error output
    public IActionResult ResultError() =>
        ViewResultOption(
            resultOption: _stringErrorResult,
            errorAction: errors => View("Detail", errors),
            viewName: "Detail");
}
```
