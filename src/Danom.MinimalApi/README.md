# Danom.MinimalApi

[![NuGet Version](https://img.shields.io/nuget/v/Danom.MinimalApi.svg)](https://www.nuget.org/packages/Danom.MinimalApi)
[![build](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml/badge.svg)](https://github.com/pimbrouwers/Danom/actions/workflows/build.yml)

Danom.MinimalApi is a library that provides a set of utilities to help integrate the [Danom](../../README.md) library with common tasks in ASP.NET Core Minimal API applications.

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
app.MapGet("/user/{id}", (int id) =>
{
    Option<User> user = FindUser(id);
    return Results.Extensions.Option(user);
});
```

Or, with a custom result:

```csharp
app.MapGet("/user/{id}", (int id) =>
{
    Option<User> user = FindUser(id);
    return Results.Extensions.Option(user, () => Results.Conflict());
});
```

### Returning Result

- **Ok**: Returns `200 OK` with the value.
- **Error**: Returns `400 Bad Request` (or a custom result, if provided).

```csharp
app.MapPost("/user", (User user) =>
{
    Result<User, string> result = TryCreateUser(user);
    return Results.Extensions.Result(result);
});
```

Or, with a custom result:

```csharp
app.MapPost("/user", (User user) =>
{
    Result<User, string> result = TryCreateUser(user);
    return Results.Extensions.Result(result, error => Results.UnprocessableEntity());
});
```

## Typed Results

If you want to use ASP.NET Core's [typed results](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/handle-results#typed-results), Danom.MinimalApi provides additional extension methods.

### Returning Typed Option

- **Some**: Returns `TypedResults.Ok(value)` and `200 OK`.
- **None**: Returns `TypedResults.NotFound()` and `404 Not Found` (or a custom result, if provided).

```csharp
using Danom.MinimalApi.TypedResults;

app.MapGet("/user/{id}", (int id) =>
{
    Option<User> user = FindUser(id);
    // returns TypedResults.Ok(user) or TypedResults.NotFound()
    return Results.Extensions.Option(user);
});

### Returning Typed Result

- **Ok**: Returns `TypedResults.Ok(value)` and `200 OK`.
- **Error**: Returns `TypedResults.BadRequest(error)` and `400 Bad Request`

```csharp
using Danom.MinimalApi.TypedResults;

app.MapPost("/user", (User user) =>
{
    Result<User, ResultErrors> result = TryCreateUser(user);
    // returns TypedResults.Ok(user) or TypedResults.BadRequest(errors)
    return Results.Extensions.Result(result);
});
```

## Find a bug?

There's an [issue](https://github.com/pimbrouwers/Danom/issues) for that.

## License

Licensed under [MIT](https://github.com/pimbrouwers/Danom/blob/master/LICENSE).
