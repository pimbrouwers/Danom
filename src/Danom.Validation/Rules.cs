namespace Danom.Validation
{
    using System;

    public static class ValidatorRules
    {
        public static Result<Unit, ResultErrors> IsValid<T>(this T value, IValidator<T> validator) =>
            validator.Validate(value).Map(_ => Unit.Value);
    }

    public static class EquatableRules
    {
        public static Result<Unit, ResultErrors> IsEqualTo<T>(this T value, T threshold, string? field = null, string? message = null)
        where T : IEquatable<T> =>
        value.Equals(threshold)
            ? Result.Ok(Unit.Value)
            : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.EqualTo(threshold, field));

        public static Result<Unit, ResultErrors> IsNotEqualTo<T>(this T value, T threshold, string? field = null, string? message = null)
            where T : IEquatable<T> =>
            !value.Equals(threshold)
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.NotEqualTo(threshold, field));
    }

    public static class ComparableRules
    {
        public static Result<Unit, ResultErrors> IsBetween<T>(this T value, T min, T max, string? field = null, string? message = null)
            where T : IComparable<T> =>
            value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.Between(min, max, field));

        public static Result<Unit, ResultErrors> IsNotBetween<T>(this T value, T min, T max, string? field = null, string? message = null)
            where T : IComparable<T> =>
            value.CompareTo(min) < 0 || value.CompareTo(max) > 0
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.NotBetween(min, max, field));

        public static Result<Unit, ResultErrors> IsPositive<T>(this T value, string? field = null, string? message = null)
            where T : IComparable<T> =>
            value.CompareTo(default!) > 0
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.Positive(field));

        public static Result<Unit, ResultErrors> IsNegative<T>(this T value, string? field = null, string? message = null)
            where T : IComparable<T> =>
            value.CompareTo(default!) < 0
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.Negative(field));

        public static Result<Unit, ResultErrors> IsZero<T>(this T value, string? field = null, string? message = null)
            where T : IComparable<T> =>
            value.CompareTo(default!) == 0
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.Zero(field));

        public static Result<Unit, ResultErrors> IsGreaterThan<T>(this T value, T threshold, string? field = null, string? message = null)
            where T : IComparable<T> =>
            value.CompareTo(threshold) > 0
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.GreaterThan(threshold, field));

        public static Result<Unit, ResultErrors> IsGreaterThanOrEqualTo<T>(this T value, T threshold, string? field = null, string? message = null)
            where T : IComparable<T> =>
            value.CompareTo(threshold) >= 0
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.GreaterThanOrEqualTo(threshold, field));

        public static Result<Unit, ResultErrors> IsLessThan<T>(this T value, T threshold, string? field = null, string? message = null)
            where T : IComparable<T> =>
            value.CompareTo(threshold) < 0
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.LessThan(threshold, field));

        public static Result<Unit, ResultErrors> IsLessThanOrEqualTo<T>(this T value, T threshold, string? field = null, string? message = null)
            where T : IComparable<T> =>
            value.CompareTo(threshold) <= 0
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.LessThanOrEqualTo(threshold, field));
    }

    public static class StringRules
    {
        public static Result<Unit, ResultErrors> IsEmpty(this string value, string? field = null, string? message = null) =>
            string.IsNullOrWhiteSpace(value)
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.StringEmpty(field));

        public static Result<Unit, ResultErrors> IsNotEmpty(this string value, string? field = null, string? message = null) =>
            string.IsNullOrWhiteSpace(value)
                ? RuleHelper.ErrorForRule(field, message ?? ValidationMessages.StringNotEmpty(field))
                : Result.Ok(Unit.Value);

        public static Result<Unit, ResultErrors> IsStartingWith(this string value, string prefix, string? field = null, string? message = null) =>
            value.StartsWith(prefix)
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.StringStartsWith(prefix, field));

        public static Result<Unit, ResultErrors> IsEndingWith(this string value, string suffix, string? field = null, string? message = null) =>
            value.EndsWith(suffix)
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.StringEndsWith(suffix, field));

        public static Result<Unit, ResultErrors> IsContaining(this string value, string substring, string? field = null, string? message = null) =>
            value.Contains(substring)
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.StringContains(substring, field));

        public static Result<Unit, ResultErrors> IsAlphanumeric(this string value, string? field = null, string? message = null) =>
            value.IsMatch("^[a-zA-Z0-9]*$", field, message ?? ValidationMessages.StringAlphanumeric(field));

        public static Result<Unit, ResultErrors> IsLength(this string value, int length, string? field = null, string? message = null) =>
            value.Length == length
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.StringEqualsLength(length, field));

        public static Result<Unit, ResultErrors> IsLengthBetween(this string value, int min, int max, string? field = null, string? message = null) =>
            value.Length >= min && value.Length <= max
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.StringBetweenLength(min, max, field));

        public static Result<Unit, ResultErrors> IsLengthGreaterThan(this string value, int min, string? field = null, string? message = null) =>
            value.Length > min
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.StringGreaterThanLength(min, field));

        public static Result<Unit, ResultErrors> IsLengthOrGreaterThan(this string value, int min, string? field = null, string? message = null) =>
            value.Length >= min
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.StringGreaterThanOrEqualToLength(min, field));

        public static Result<Unit, ResultErrors> IsLengthLessThan(this string value, int max, string? field = null, string? message = null) =>
            value.Length < max
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.StringLessThanLength(max, field));

        public static Result<Unit, ResultErrors> IsLengthOrLessThan(this string value, int max, string? field = null, string? message = null) =>
            value.Length <= max
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.StringLessThanOrEqualToLength(max, field));

        public static Result<Unit, ResultErrors> IsMatch(this string value, string pattern, string? field = null, string? message = null) =>
            System.Text.RegularExpressions.Regex.IsMatch(value, pattern)
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.StringPattern(pattern, field));
    }

    public static class GuidRules
    {
        public static Result<Unit, ResultErrors> IsEmpty(this Guid value, string? field = null, string? message = null) =>
            value == Guid.Empty
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.GuidEmpty(field));

        public static Result<Unit, ResultErrors> IsNotEmpty(this Guid value, string? field = null, string? message = null) =>
            value != Guid.Empty
                ? Result.Ok(Unit.Value)
                : RuleHelper.ErrorForRule(field, message ?? ValidationMessages.GuidNotEmpty(field));
    }

    public static class OptionRules
    {
        public static Result<Unit, ResultErrors> Required<T>(this Option<T> option, Func<T, Result<Unit, ResultErrors>> func, string? field = null, string? message = null) =>
            option.Match(
                some: value => func(value),
                none: () => RuleHelper.ErrorForRule(field, message ?? ValidationMessages.OptionIsSome(field)));

        public static Result<Unit, ResultErrors> Required<T>(this Option<T> option, string? field = null, string? message = null) =>
            option.Required(x => Result.Ok(Unit.Value), field, message);

        public static Result<Unit, ResultErrors> Optional<T>(this Option<T> option, Func<T, Result<Unit, ResultErrors>> func) =>
            option.Match(
                some: value => func(value),
                none: () => Result.Ok(Unit.Value));
    }

    internal static class ValidationMessages
    {
        // Equatable rules
        internal static string EqualTo<T>(T value, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be equal to {value}"
                : $"Value must be equal to {value}";

        internal static string NotEqualTo<T>(T value, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must not equal {value}"
                : $"Value must not equal {value}";

        // Comparable rules
        internal static string GreaterThan<T>(T min, string? field = null) where T : IComparable<T> =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be greater than {min}"
                : $"Value must be greater than {min}";

        internal static string LessThan<T>(T max, string? field = null) where T : IComparable<T> =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be less than {max}"
                : $"Value must be less than {max}";

        internal static string GreaterThanOrEqualTo<T>(T min, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be greater than or equal to {min}"
                : $"Value must be greater than or equal to {min}";

        internal static string LessThanOrEqualTo<T>(T max, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be less than or equal to {max}"
                : $"Value must be less than or equal to {max}";

        internal static string Between<T>(T min, T max, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be between {min} and {max}"
                : $"Value must be between {min} and {max}";

        internal static string NotBetween<T>(T min, T max, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be outside the range {min} to {max}"
                : $"Value must be outside the range {min} to {max}";

        internal static string Positive(string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be positive"
                : "Value must be positive";

        internal static string Negative(string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be negative"
                : "Value must be negative";

        internal static string Zero(string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be zero"
                : "Value must be zero";

        // String rules
        internal static string StringEmpty(string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be empty"
                : "Value must be empty";

        internal static string StringNotEmpty(string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must not be empty"
                : "Value must not be empty";

        internal static string StringStartsWith(string prefix, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must start with '{prefix}'"
                : $"Value must start with '{prefix}'";

        internal static string StringEndsWith(string suffix, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must end with '{suffix}'"
                : $"Value must end with '{suffix}'";

        internal static string StringContains(string substring, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must contain '{substring}'"
                : $"Value must contain '{substring}'";

        internal static string StringAlphanumeric(string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be alphanumeric"
                : "Value must be alphanumeric";

        internal static string StringEqualsLength(int length, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be {length} characters"
                : $"Value must be {length} characters";

        internal static string StringBetweenLength(int min, int max, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be between {min} and {max} characters"
                : $"Value must be between {min} and {max} characters";

        internal static string StringGreaterThanLength(int min, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be greater than {min} characters"
                : $"Value must be greater than {min} characters";

        internal static string StringGreaterThanOrEqualToLength(int min, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be greater than or equal to {min} characters"
                : $"Value must be greater than or equal to {min} characters";

        internal static string StringLessThanLength(int max, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be less than {max} characters"
                : $"Value must be less than {max} characters";

        internal static string StringLessThanOrEqualToLength(int max, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be less than or equal to {max} characters"
                : $"Value must be less than or equal to {max} characters";

        internal static string StringPattern(string pattern, string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must match pattern {pattern}"
                : $"Value must match pattern {pattern}";

        // Option rules
        internal static string OptionIsSome(string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' is required"
                : "Value is required";

        // Guid rules
        internal static string GuidEmpty(string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must be empty"
                : "Value must be empty";

        internal static string GuidNotEmpty(string? field = null) =>
            field is string f && !string.IsNullOrWhiteSpace(f)
                ? $"'{f}' must not be empty"
                : "Value must not be empty";

        // private static SeqBetweenLen() field min max = sprintf "'%s' must be between %i and %i items" field min max
        // private static SeqEmpty(string field) => $"'{field}' must be empty";
        // private static SeqEqualsLen() field len = sprintf "'%s' must be %i items" field len
        // private static SeqExists(string field) => $"'{field}' must contain the specified item";
        // private static SeqForAll(string field) => $"'{field}' must only contain items that satisfy the predicate";
        // private static SeqGreaterThanLen() field min = sprintf "'%s' must be greater than %i items" field min
        // private static SeqGreaterThanOrEqualToLen() field min = sprintf "'%s' must be greater than or equal to %i items" field min
        // private static SeqLessThanLen() field max = sprintf "'%s' must be less than %i items" field max
        // private static SeqLessThanOrEqualToLen() field max = sprintf "'%s' must be less than or equal to %i items" field max
        // private static SeqNotEmpty(string field) => $"'{field}' must not be empty";
    }

    internal static class RuleHelper
    {
        internal static Result<Unit, ResultErrors> ErrorForRule(string? field, string message)
        {
            if (field is string f && !string.IsNullOrWhiteSpace(f))
            {
                return Result.Error(new ResultErrors(f, message));
            }
            else
            {
                return Result.Error(new ResultErrors(message));
            }
        }
    }
}
