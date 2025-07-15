namespace Danom.Validation
{
    using System;
    using System.Text.RegularExpressions;

    public static class Check
    {
        public static ValidatorRule<T> IsEqualTo<T>(T threshold) where T : IEquatable<T> =>
            value => value.IsEqualTo(threshold);
        public static ValidatorRule<T> IsNotEqualTo<T>(T threshold) where T : IEquatable<T> =>
            value => value.IsNotEqualTo(threshold);
        public static ValidatorRule<T> IsBetween<T>(T min, T max) where T : IComparable<T> =>
            value => value.IsBetween(min, max);
        public static ValidatorRule<T> IsNotBetween<T>(T min, T max) where T : IComparable<T> =>
            value => value.IsNotBetween(min, max);
        public static ValidatorRule<T> IsPositive<T>() where T : IComparable<T> =>
            value => value.IsPositive();
        public static ValidatorRule<T> IsNegative<T>() where T : IComparable<T> =>
            value => value.IsNegative();
        public static ValidatorRule<T> IsZero<T>() where T : IComparable<T> =>
            value => value.IsZero();
        public static ValidatorRule<T> IsGreaterThan<T>(T threshold) where T : IComparable<T> =>
            value => value.IsGreaterThan(threshold);
        public static ValidatorRule<T> IsGreaterThanOrEqualTo<T>(T threshold) where T : IComparable<T> =>
            value => value.IsGreaterThanOrEqualTo(threshold);
        public static ValidatorRule<T> IsLessThan<T>(T threshold) where T : IComparable<T> =>
            value => value.IsLessThan(threshold);
        public static ValidatorRule<T> IsLessThanOrEqualTo<T>(T threshold) where T : IComparable<T> =>
            value => value.IsLessThanOrEqualTo(threshold);

        public static ValidatorRule<string> IsEmpty =>
            value => value.IsEmpty();
        public static ValidatorRule<string> IsNotEmpty =>
            value => value.IsNotEmpty();
        public static ValidatorRule<string> IsStartingWith(string prefix) =>
            value => value.IsStartingWith(prefix);
        public static ValidatorRule<string> IsEndingWith(string suffix) =>
            value => value.IsEndingWith(suffix);
        public static ValidatorRule<string> IsContaining(string substring) =>
            value => value.IsContaining(substring);
        public static ValidatorRule<string> IsLength(int length) =>
            value => value.IsLength(length);
        public static ValidatorRule<string> IsLengthBetween(int min, int max) =>
            value => value.IsLengthBetween(min, max);
        public static ValidatorRule<string> IsLengthGreaterThan(int min) =>
            value => value.IsLengthGreaterThan(min);
        public static ValidatorRule<string> IsLengthOrGreaterThan(int min) =>
            value => value.IsLengthOrGreaterThan(min);
        public static ValidatorRule<string> IsLengthLessThan(int max) =>
            value => value.IsLengthLessThan(max);
        public static ValidatorRule<string> IsLengthOrLessThan(int max) =>
            value => value.IsLengthOrLessThan(max);
        public static ValidatorRule<string> IsMatch(string pattern) =>
            value => value.IsMatch(pattern);

        public static ValidatorRule<Guid> IsEmptyGuid =>
            value => value.IsEmpty();
        public static ValidatorRule<Guid> IsNotEmptyGuid =>
            value => value.IsNotEmpty();
    }

    public static class ValidatorRulesExtensions
    {
        public static LabeledValidatorRule IsValid<T>(this T value, IValidator<T> validator) =>
            field => validator.Validate(value).Map(_ => Unit.Value);
    }

    public static class EquatableRulesExtensions
    {
        public static LabeledValidatorRule IsEqualTo<T>(this T value, T threshold) where T : IEquatable<T> =>
            field => RuleHelper.Check(value.Equals(threshold), $"'{field}' must be equal to {threshold}");

        public static LabeledValidatorRule IsNotEqualTo<T>(this T value, T threshold) where T : IEquatable<T> =>
            field => RuleHelper.Check(!value.Equals(threshold), $"'{field}' must not equal {threshold}");
    }

    public static class ComparableRulesExtensions
    {
        public static LabeledValidatorRule IsBetween<T>(this T value, T min, T max) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0, $"'{field}' must be between {min} and {max}");

        public static LabeledValidatorRule IsNotBetween<T>(this T value, T min, T max) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(min) < 0 || value.CompareTo(max) > 0, $"'{field}' must be outside the range {min} to {max}");

        public static LabeledValidatorRule IsPositive<T>(this T value) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(default!) > 0, $"'{field}' must be positive");

        public static LabeledValidatorRule IsNegative<T>(this T value) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(default!) < 0, $"'{field}' must be negative");

        public static LabeledValidatorRule IsZero<T>(this T value) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(default!) == 0, $"'{field}' must be zero");

        public static LabeledValidatorRule IsGreaterThan<T>(this T value, T threshold) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(threshold) > 0, $"'{field}' must be greater than {threshold}");

        public static LabeledValidatorRule IsGreaterThanOrEqualTo<T>(this T value, T threshold) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(threshold) >= 0, $"'{field}' must be greater than or equal to {threshold}");

        public static LabeledValidatorRule IsLessThan<T>(this T value, T threshold) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(threshold) < 0, $"'{field}' must be less than {threshold}");

        public static LabeledValidatorRule IsLessThanOrEqualTo<T>(this T value, T threshold) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(threshold) <= 0, $"'{field}' must be less than or equal to {threshold}");
    }

    public static class StringRulesExtensions
    {
        public static LabeledValidatorRule IsEmpty(this string value) =>
            field => RuleHelper.Check(string.IsNullOrWhiteSpace(value), $"'{field}' must be empty");

        public static LabeledValidatorRule IsNotEmpty(this string value) =>
            field => RuleHelper.Check(!string.IsNullOrWhiteSpace(value), $"'{field}' must not be empty");

        public static LabeledValidatorRule IsStartingWith(this string value, string prefix) =>
            field => RuleHelper.Check(value.StartsWith(prefix), $"'{field}' must start with '{prefix}'");

        public static LabeledValidatorRule IsEndingWith(this string value, string suffix) =>
            field => RuleHelper.Check(value.EndsWith(suffix), $"'{field}' must end with '{suffix}'");

        public static LabeledValidatorRule IsContaining(this string value, string substring) =>
            field => RuleHelper.Check(value.Contains(substring), $"'{field}' must contain '{substring}'");

        public static LabeledValidatorRule IsLength(this string value, int length) =>
            field => RuleHelper.Check(value.Length == length, $"'{field}' must be {length} characters");

        public static LabeledValidatorRule IsLengthBetween(this string value, int min, int max) =>
            field => RuleHelper.Check(value.Length >= min && value.Length <= max, $"'{field}' must be between {min} and {max} characters");

        public static LabeledValidatorRule IsLengthGreaterThan(this string value, int min) =>
            field => RuleHelper.Check(value.Length > min, $"'{field}' must be longer than {min} characters");

        public static LabeledValidatorRule IsLengthOrGreaterThan(this string value, int min) =>
            field => RuleHelper.Check(value.Length >= min, $"'{field}' must be longer than or equal to {min} characters");

        public static LabeledValidatorRule IsLengthLessThan(this string value, int max) =>
            field => RuleHelper.Check(value.Length < max, $"'{field}' must be less than {max} characters");

        public static LabeledValidatorRule IsLengthOrLessThan(this string value, int max) =>
            field => RuleHelper.Check(value.Length <= max, $"'{field}' must be less than or equal to {max} characters");

        public static LabeledValidatorRule IsMatch(this string value, string pattern) =>
            field => RuleHelper.Check(Regex.IsMatch(value, pattern), $"'{field}' must match pattern {pattern}");
    }

    public static class GuidRulesExtensions
    {
        public static LabeledValidatorRule IsEmpty(this Guid value) =>
            field => RuleHelper.Check(value == Guid.Empty, $"'{field}' must be empty");

        public static LabeledValidatorRule IsNotEmpty(this Guid value) =>
            field => RuleHelper.Check(value != Guid.Empty, $"'{field}' must not be empty");
    }

    public static class OptionRulesExtensions
    {
        public static LabeledValidatorRule Required<T>(this Option<T> option, ValidatorRule<T> func) =>
            option.Match(
                some: value => func(value),
                none: () => field => Result.Error($"'{field}' is required"));

        public static LabeledValidatorRule Required<T>(this Option<T> option) =>
            option.Required(x => field => Result.Ok());

        public static LabeledValidatorRule Optional<T>(this Option<T> option, ValidatorRule<T> func) =>
            option.Match(
                some: value => func(value),
                none: () => field => Result<Unit, ResultErrors>.Ok(Unit.Value));
    }

    internal static class RuleHelper
    {
        public static Result<Unit, ResultErrors> Check(bool isValid, string errorMessage) =>
            isValid
                ? Result.Ok()
                : Result.Error(new[] { errorMessage });
    }
}
