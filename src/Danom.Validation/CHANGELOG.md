# Changelog

All notable changes to this project will be documented in this file.

## [Unreleased]

### Changed

- `Check.String.IsUrl` now validates absolute HTTP(S) URLs only.
- `Check.IsPositive<T>`, `Check.IsNegative<T>`, and `Check.IsZero<T>` now use framework-conditional behavior:
	- generic math constraints on .NET 7+ targets where available,
	- runtime numeric-type guard fallback on lower targets.
- Refactored `Check.Enumerable.ForEach(...)` overloads to use shared internal iteration/evaluation logic.

## [2.1.0] - 2025-12-30

## Added

- Danom.Validation, rule overloads for `Option<T>` marking them `Required()` or `Optional()`.
- Collection support using `ForEach()` and multiple rules.

## [2.0.0] - 2025-09-18

### Removed

- FluentValidation dependency from `Danom.Validation` package.

## [1.2.0] - 2024-12-06

### Added

- `ValidationOption<T>` for scenarios non-message based validation scenarios.

### Updated

- Updated to Danom 1.2.0

## [1.1.1] - 2024-12-06

### Updated

- Updated to Danom 1.1.1

## [1.1.0] - 2024-12-01

### Updated

- Updated to Danom 1.1.0

## [1.0.0] - 2024-11-29

> Hello world!
