# Changelog

All notable changes to this project will be documented in this file.

## [1.1.0] - 2024-12-01

### Added

- `Option<T>.TryGet(out T result)` to safely provide the internal value. A return value indicates whether or not the Option was Some(x) or None.
- `Option<T>.ToString(string defaultValue, string? format = null, IFormatProvider? provider = null)` to minimize code required to execute ToString against the inner value safely (i.e., replaces Map -> ToString -> DefaultValue chain).

## [1.0.0] - 2024-11-29

> Hello world!
