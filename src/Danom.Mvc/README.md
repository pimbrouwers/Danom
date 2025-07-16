# Danom.Mvc

[![NuGet Version](https://img.shields.io/nuget/v/Danom.Mvc.svg)](https://www.nuget.org/packages/Danom.Mvc)
[![build](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml/badge.svg)](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml)

Danom.Mvc is a library that provides a set of utilities to help integrate the [Danom](../../README.md) library with common tasks in ASP.NET Core [MVC](#mvc) and [Razor Pages](#razor-pages) applications.

## Getting Started

Install the [Danom.Mvc](https://www.nuget.org/packages/Danom.Mvc/) NuGet package:

```
PM>  Install-Package Danom.Mvc
```

Or using the dotnet CLI
```cmd
dotnet add package Danom.Mvc
```

## MVC

The `DanomController` class extends the base controller class to provide a set of methods to help work with `Result` and `Option` types in ASP.NET Core MVC applications.


### Option

The `ViewOption` method  is used to render a view based on the presence of an `Option` value.

If the `Option` is `Some`, the view is rendered with the value. If the `Option` is `None`, the `noneAction` is invoked. By default, the `noneAction` returns a `NotFound` result.

A custom view name can be provided to render a view with a different name than the action.

Some examples demonstrating the use of `ViewOption` are shown below:

```csharp
using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

public sealed class OptionController
    : DanomController
{
    public IActionResult OptionSome() =>
        ViewOption(
            option: Option.Some("Hello world"),
            viewName: "Detail");

    // Returns the ASP.NET default `NotFound` result
    public IActionResult OptionNone() =>
        ViewOption(
            option: Option<string>.NoneValue,
            viewName: "Detail");

    public IActionResult OptionNoneCustom() =>
        ViewOption(
            option: Option<string>.NoneValue,
            viewName: "Detail",
            noneAction: () => NotFound("Not found!"));
}
```

### Result

The `ViewResult` method is used to render a view based on the presence of a `Result` value.

By default the `ViewResult` method will render the view with the value if the `Result` is `Ok`. If the `Result` is `Error`, the `errorAction` is invoked.

A custom view name can be provided to render a view with a different name than the action.

```csharp
using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

public sealed class ResultController
    : DanomController
{
    public IActionResult ResultOk() =>
        ViewResult(
            result: Result<string, string>.Ok("Success!"),
            viewName: "Detail");

    public IActionResult ResultError() =>
        ViewResult(
            result: Result<string, string>.Error("An error occurred."),
            errorAction: errors => View("Detail", errors),
            viewName: "Detail");

}
```

Built into Danom is the `ResultErrors` type, which is particularly well suited for reporting model errors in ASP.NET Core MVC applications. The `ViewResultErrors` method, provided by the `DanomController` class, is a proxy for the `View` method that will inject the `ResultErrors` value into the model state.

When using `ResultErrors` as the error type, the `ViewResultErrors` will default the `errorAction` to inject the `ResultErrors` value into the model state.

```csharp
using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

public sealed class ResultController
    : DanomController
{
    public IActionResult ResultOk() =>
        ViewResult(
            // notice the lack of second type parameter, which is inferred to be ResultErrors
            result: Result<string>.Ok("Success!"),
            viewName: "Detail");

    public IActionResult ResultError() =>
        ViewResult(
            result: Result<string>.Error("An error occurred."),
            viewName: "Detail");


    public IActionResult ResultErrorView() =>
        // can be used directly
        ViewResultErrors(
            errors: new("An error occurred."));
}
```

### View

While not explicitly part of the `Danom.Mvc` library, there are some patterns that make rendering the `Option` type easier in Razor views. Two methods from the base library are especially valuable: `TryGet` and `ToString`.

The `TryGet` method is used to extract the value from an `Option` type. If the `Option` is `Some`, the value is assigned to the `out` parameter and the method returns `true`. If the `Option` is `None`, the method returns `false`.

The custom `ToString` method is used to convert the `Option` value to a string. If the `Option` is `Some`, the value is converted to a string. If the `Option` is `None`, the method returns the default value. The method optionally accepts a second parameter for the format string.

Consider the following type:

```csharp
public record Person(
    string Name,
    Option<DateOnly> Birthdate,
    Option<string> Email);
```

The `TryGet` and `ToString` methods can be used in a Razor view to help render the optional properties.

```cshtml
@model Person

<h1>@Model.Name</h1>
<h2>Email: <i>@Model.Email.ToString("-")</i></h2>

@if (Model.Birthdate.TryGet(out var birthdate))
{
    var now = DateTime.Now;
    var a = (now.Year * 100 + now.Month) * 100 + now.Day;
    var b = (birthdate.Year * 100 + birthdate.Month) * 100 + birthdate.Day;
    var age = (a - b) / 10000;

    <p>You are born on @birthdate, and are @age years old.</p>
}
else
{
    <p>You are an ageless wonder!</p>
}
```

## Razor Pages

The `DanomPageModel` class extends the base page model class to provide method(s) to help work with `Result` and `Option` types in ASP.NET Core Razor Pages applications.

```csharp
using Danom.Mvc;

public sealed class IndexModel
    : DanomPageModel
{
    public void OnGet()
    {
        var resultWithErrors = Result<string, string>.Error("An error occurred.");
        if(resultWithErrors.TryGetError(out var e))
        {
            return Page(e);
        }
    }
}
```

## Find a bug?

There's an [issue](https://github.com/pimbrouwers/Danom/issues) for that.

## License

Licensed under [MIT](https://github.com/pimbrouwers/Danom/blob/master/LICENSE).
