# Danom.MinimalApi

[![NuGet Version](https://img.shields.io/nuget/v/Danom.MinimalApi.svg)](https://www.nuget.org/packages/Danom.MinimalApi)
[![build](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml/badge.svg)](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml)

Danom.MinimalApi is a library that provides a set of utilities to help integrate the [Danom](../README.md) library with common tasks in ASP.NET Core Minimal API applications.

## Key Features

- Extension methods for converting `Option<T>` and `Result<T, TError>` types to `IResult` responses.
- Support for both standard and typed results in Minimal APIs.

## Design Goals

- Simplify the handling of optional and result-based data in Minimal APIs.
- Provide a clean and intuitive API for developers familiar with Danom and ASP.NET Core.

## Getting Started

Install the [Danom.MinimalApi](https://www.nuget.org/packages/Danom.MinimalApi/) NuGet package:

```
PM>  Install-Package Danom.MinimalApi
```

Or using the dotnet CLI
```cmd
dotnet add package Danom.MinimalApi
```

## Basic Usage

### Returning Option

- **Some**: Returns `200 OK` with the value.
- **None**: Return `404 Not Found` (or a custom result, if provided).

```csharp
app.MapGet("/user/{id}", (int id) => {
    var user = FindUser(id);
    return Results.Extensions.Option(user);
});
```

Or, with a custom result:

```csharp
app.MapGet("/user/{id}", (int id) => {
    var user = FindUser(id);
    return Results.Extensions.Option(user, () => Results.Conflict());
});
```

### Returning Result

- **Ok**: Returns `200 OK` with the value.
- **Error**: Returns `400 Bad Request` (or a custom `IResult`, if provided).

```csharp
app.MapPost("/user", (User user) => {
    var result = TryCreateUser(user);
    return Results.Extensions.Result(result);
});
```

Or, with a custom result:

```csharp
app.MapPost("/user", (User user) => {
    var result = TryCreateUser(user);
    return Results.Extensions.Result(result, error => Results.UnprocessableEntity());
});
```

## Typed Results

If you want to use ASP.NET Core's [typed results](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/handle-results#typed-results), Danom.MinimalApi provides additional extension methods.

### Returning Typed Option

- **Some**: Returns `OptionHttpResult<T>` and `200 OK`.
- **None**: Returns `OptionHttpResult<T>` and `404 Not Found` (or a custom `IResult`, if provided).

```csharp
using Danom;
using Danom.MinimalApi;

app.MapGet("/user/{id}", (int id) => {
    var user = FindUser(id);
    return DanomTypedResults.Option(user);
});

// or, with a custom error handler
app.MapGet("/user/{id}/custom", (int id) => {
    var user = FindUser(id);
    return DanomTypedResults.Option(user,
        noneResult: () => Results.NotFound("Custom not found!"));
});
```

### Returning Typed Result

- **Ok**: Returns `ResultHttpResult<T, TError>` and `200 OK`.
- **Error**: Returns `ResultHttpResult<T, TError>` and `400 Bad Request`

```csharp
using Danom;
using Danom.MinimalApi;

app.MapPost("/user", (User user) => {
    var result = TryCreateUser(user);
    return DanomTypedResults.Result(result);
});

// or, with a custom error handler
app.MapPost("/user/custom", (User user) => {
    var result = TryCreateUser(user);
    return DanomTypedResults.Result(result,
        errorResult: error => Results.Ok(new { Message = "There was a problem", error }));
});


// or, return a problem details response
app.MapPost("/user/problem", (User user) => {
    var result = TryCreateUser(user);
    return DanomTypedResults.ResultProblem(result);
});

```

## Contributing

I kindly ask that before submitting a pull request, you first submit an [issue](https://github.com/pimbrouwers/Danom/issues).

If functionality is added to the API, or changed, please kindly update the relevant documentation. Unit tests must also be added and/or updated before a pull request can be successfully merged.

Only pull requests which pass all build checks and comply with the general coding standard can be approved.

## Find a bug?

There's an [issue](https://github.com/pimbrouwers/Danom/issues) for that.

## License

Licensed under [MIT](https://github.com/pimbrouwers/Danom/blob/master/LICENSE).
