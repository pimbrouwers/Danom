# Changelog

All notable changes to this project will be documented in this file.

## [Unreleased]

### Added

- `Result<T, TError>.TryGet(out var ok)` and `Result<T, TError>.TryGetError(out var error)` to safely provide the internal value. A return value indicates whether or not the Result was Ok or Error.
- `DanomPageModel` base class for Razor Pages to provide a consistent API for handling options and results.
- `Option<T>` implementation for `IComparable<T>`.

### Removed

- `ResultOption<T>` and `ResultOption<T, TError>` types. Use `Option<T>` and `Result<T, TError>` instead.
- Async helpers for `Option<T>` and `Result<T, TError>` to allow for async operations.

## [1.2.0] - 2024-12-06

### Added

- Option `TryParse` for: `bool`, `byte`, `short`, `int`, `long`, `decimal`, `double`, `float`, `Guid`, `DateTimeOffset`, `DateTime`, `DateOnly`, `TimeOnly`, `TimeSpan`, `Enum` (ex: `intOption.TryParse`).
- `ModelStateDictionary.AddResultErrors()` extension method to add `Result` errors to the `ModelStateDictionary`.
- `ValidationOption<T>` for scenarios non-message based validation scenarios.

## [1.1.1] - 2024-12-06

### Fixed

- Option<T>.NoneValue, Option<T>.NoneValueAsync properties.

## [1.1.0] - 2024-12-01

### Added

- `Option<T>.TryGet(out T result)` to safely provide the internal value. A return value indicates whether or not the Option was Some(x) or None.
- `Option<T>.ToString(string defaultValue, string? format = null, IFormatProvider? provider = null)` to minimize code required to execute ToString against the inner value safely (i.e., replaces Map -> ToString -> DefaultValue chain).

## [1.0.0] - 2024-11-29

> Hello world!
