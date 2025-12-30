# Changelog

All notable changes to this project will be documented in this file.

## [2.1.0] - 2025-??-??

## Added

- Choice type, a discriminated union similar to Option and Result types, but allowing multiple possible value types (supports up to 5 type parameters).
- Comprehensive `ValueTask` support for Option, Result, and Unit types.
- `net10.0` support added to `Danom.MinimalApi` and `Danom.Mvc` packages.
- `Option.NoneValue` alias for `Option<Unit>.NoneValue`.
- Danom.Validation, rule overloads for `Option<T>` marking them `Required()` or `Optional()`, and collection support using `ForEach()`.
- TryParse overloads supporting all formatting options (ex: `intOption.TryParse(string value, NumberStyles styles, IFormatProvider provider)`).

## Fixed

- Result no longer reports `IsError = true` when unitialized.

## [2.0.0] - 2025-09-18

### Added

- **netstandard 2.1** support.
- AOT compatibility flag.
- `Result<T, TError>.ToOption()` method to convert a Result to an Option.
- `Result<T>.Error` static extensions to mirror `ResultErrors` constructor signatures.
- `Result<T, TError>.TryGet(out T ok)` and `Result<T, TError>.TryGetError(out TError error)` to safely provide the internal value. A return value indicates whether or not the Result was Ok or Error.
- `Option<T>` implementation for `IComparable<T>`.
- `ResultErrors` keyed error list constructor (ex: `new ResultErrors("key", ["value1", "value2"])`).
- `ResultError.Add` methods to support in-place error addition (ex: `resultError.Add("key", "value")`).
- Minimal API integration via `DanomResultExtensions` and `TypedDanomResultExtensions`.
- `DanomPageModel` base class for Razor Pages to provide a consistent API for handling options and results.
- Nullablility annotations for better type safety.

### Removed

- FluentValidation dependency from `Danom.Validation` package.
- `ResultOption<T>` and `ResultOption<T, TError>` types. Use `Option<T>` and `Result<T, TError>` instead.
- Async wrapper methods for `Option<T>` and `Result<T, TError>` to support mixed operation chaining.

## [1.2.0] - 2024-12-06

### Added

- Option `TryParse` for: `bool`, `byte`, `short`, `int`, `long`, `decimal`, `double`, `float`, `Guid`, `DateTimeOffset`, `DateTime`, `TimeSpan`, `Enum` (ex: `intOption.TryParse`, `longOption.TryParse` etc.).
- `ModelStateDictionary.AddResultErrors()` extension method to add `Result` errors to the `ModelStateDictionary`.
- `ValidationOption<T>` for scenarios non-message based validation scenarios.

## [1.1.1] - 2024-12-06

### Fixed

- `Option<T>.NoneValue`, `Option<T>.NoneValueAsync` properties.

## [1.1.0] - 2024-12-01

### Added

- `Option<T>.TryGet(out T result)` to safely provide the internal value. A return value indicates whether or not the Option was Some(x) or None.
- `Option<T>.ToString(string defaultValue, string? format = null, IFormatProvider? provider = null)` to minimize code required to execute ToString against the inner value safely (i.e., replaces Map -> ToString -> DefaultValue chain).

## [1.0.0] - 2024-11-29

> Hello world!
