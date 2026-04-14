# Changelog

All notable changes to this project will be documented in this file.

## [Unreleased]

> Version 3.0.0 introduces a major simplification by removing the async API. This change focuses the library on its core purpose—providing functional constructs for synchronous workflows—while reducing complexity and maintenance overhead. Users relying on async methods will need to [refactor their code](migrating-from-v2-to-v3.md) to handle async workflows externally. See the migration guide for details.

## Added

- New `Option<T>` properties/methods:
    - `Count` (returns 1 if some, 0 if none)
    - `Apply<U>(Option<Func<T, U>> fn)`
    - `Tap(Action<T> tap)`
    - `Filter(Func<T, bool> predicate)`
    - `Exists(Func<T, bool> predicate)`
    - `Contains(T value)`
    - `ToList()`
    - `ToArray()`
- New `Option` static methods:
    - `Option.Match<T, T2, U>(Option<T> o1, Option<T2> o2, // ...)`
    - `Option.Match<T, T2, T3, U>(Option<T> o1, Option<T2> o2, Option<T3> o3, // ...)`
    - `Option.Match<T, U>(IEnumerable<Option<T>> options, // ...)`
    - `Option.Bind<T, T2, U>(Option<T> o1, Option<T2> o2, // ...)`
    - `Option.Bind<T, T2, T3, U>(Option<T> o1, Option<T2> o2, Option<T3> o3, // ...)`
    - `Option.Map<T, T2, U>(Option<T> o1, Option<T2> o2, // ...)`
    - `Option.Map<T, T2, T3, U>(Option<T> o1, Option<T2> o2, Option<T3> o3, // ...)`
    - `Option.Flatten<T>(Option<Option<T>> option)`
    - `Option.Flatten<T>(IEnumerable<Option<T>> options)`
    - `Option.Traverse<T, U>(IEnumerable<T> items, Func<T, Option<U>> apply)`
    - `Option.Sequence<T>(IEnumerable<Option<T>> options)`
    - `Option.Zip<T, T2>(Option<T> o1, Option<T2> o2)`
    - `Option.Zip<T, T2, T3>(Option<T> o1, Option<T2> o2, Option<T3> o3)`
- New `Result<T, TError>` properties/methods:
    - `Count` (returns 1 if ok, 0 if error)
    - `Apply<U>(Result<Func<T, U>, TError> fn)`
    - `Tap(Action<T> tap)`
    - `Filter(Func<T, bool> predicate)`
    - `Exists(Func<T, bool> predicate)`
    - `Contains(T value)`
    - `ToList()`
    - `ToArray()`
- New `Result` static methods:
    - `Result.Match<T, T2, TError, U>(Result<T, TError> r1, Result<T2, TError> r2, // ...)`
    - `Result.Match<T, T2, T3, TError, U>(Result<T, TError> r1, Result<T2, TError> r2, Result<T3, TError> r3, // ...)`
    - `Result.Bind<T, T2, TError, U>(Result<T, TError> r1, Result<T2, TError> r2, // ...)`
    - `Result.Bind<T, T2, T3, TError, U>(Result<T, TError> r1, Result<T2, TError> r2, Result<T3, TError> r3, // ...)`
    - `Result.Map<T, T2, TError, U>(Result<T, TError> r1, Result<T2, TError> r2, // ...)`
    - `Result.Map<T, T2, T3, TError, U>(Result<T, TError> r1, Result<T2, TError> r2, Result<T3, TError> r3, // ...)`
    - `Result.Traverse<T, U, TError>(IEnumerable<T> items, Func<T, Result<U, TError>> apply)`
    - `Result.Sequence<T, TError>(IEnumerable<Result<T, TError>> results, Func<TError, TError, TError> errorFunc)`
    - `Result.Sequence<T, TError>(IEnumerable<Result<T, TError>> results)`
    - `Result.Zip<T, T2, TError>(Result<T, TError> r1, Result<T2, TError> r2)`
    - `Result.Zip<T, T2, T3, TError>(Result<T, TError> r1, Result<T2, TError> r2, Result<T3, TError> r3)`
- ResultErrors.Merge method and keyed accessor (ex: `ResultErrors.Merge([r1, r2, r3])`, `resultErrors["key"]`).
- Pipe extension method, allowing for functional piping of values (ex: `value.Pipe(func)`).

## Removed

- All async methods and types (ex: `Option<T>.MapAsync`, `Result<T, TError>.BindAsync`, etc.). Users should handle async workflows externally.
- `shortOption.TryParse(string? x, IFormatProvider provider)`, was a development artifact and should be replaced by `shortOption.TryParse(string? x, NumberStyles styles, IFormatProvider? provider)`.

## [2.1.0] - 2025-12-30

## Added

- Choice type, a discriminated union similar to Option and Result types, but allowing multiple possible value types (supports up to 5 type parameters).
- Comprehensive `ValueTask` support for Option, Result, and Unit types.
- `Option.NoneValue` alias for `Option<Unit>.NoneValue`.
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
- Nullablility annotations for better type safety.

### Removed

- `ResultOption<T>` and `ResultOption<T, TError>` types. Use `Option<T>` and `Result<T, TError>` instead.
- Async wrapper methods for `Option<T>` and `Result<T, TError>` to support mixed operation chaining.

## [1.2.0] - 2024-12-06

### Added

- Option `TryParse` for: `bool`, `byte`, `short`, `int`, `long`, `decimal`, `double`, `float`, `Guid`, `DateTimeOffset`, `DateTime`, `TimeSpan`, `Enum` (ex: `intOption.TryParse`, `longOption.TryParse` etc.).

## [1.1.1] - 2024-12-06

### Fixed

- `Option<T>.NoneValue`, `Option<T>.NoneValueAsync` properties.

## [1.1.0] - 2024-12-01

### Added

- `Option<T>.TryGet(out T result)` to safely provide the internal value. A return value indicates whether or not the Option was Some(x) or None.
- `Option<T>.ToString(string defaultValue, string? format = null, IFormatProvider? provider = null)` to minimize code required to execute ToString against the inner value safely (i.e., replaces Map -> ToString -> DefaultValue chain).

## [1.0.0] - 2024-11-29

> Hello world!
