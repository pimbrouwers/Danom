namespace Danom.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides a set of validation checks that can be used in validators.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Checks if the value is equal to a specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsEqualTo<T>(T input) where T : IEquatable<T> =>
            value => value.IsEqualTo(input);

        /// <summary>
        /// Checks if the value is not equal to a specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsNotEqualTo<T>(T input) where T : IEquatable<T> =>
            value => value.IsNotEqualTo(input);

        /// <summary>
        /// Checks if the value is between two specified values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsBetween<T>(T min, T max) where T : IComparable<T> =>
            value => value.IsBetween(min, max);

        /// <summary>
        /// Checks if the value is not between two specified values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsNotBetween<T>(T min, T max) where T : IComparable<T> =>
            value => value.IsNotBetween(min, max);

        /// <summary>
        /// Checks if the value is positive.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ValidatorRule<T> IsPositive<T>() where T : IComparable<T> =>
            value => value.IsPositive();

        /// <summary>
        /// Checks if the value is negative.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ValidatorRule<T> IsNegative<T>() where T : IComparable<T> =>
            value => value.IsNegative();

        /// <summary>
        /// Checks if the value is zero.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ValidatorRule<T> IsZero<T>() where T : IComparable<T> =>
            value => value.IsZero();

        /// <summary>
        /// Checks if the value is greater than a specified threshold.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsGreaterThan<T>(T threshold) where T : IComparable<T> =>
            value => value.IsGreaterThan(threshold);

        /// <summary>
        /// Checks if the value is greater than or equal to a specified threshold.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsGreaterThanOrEqualTo<T>(T threshold) where T : IComparable<T> =>
            value => value.IsGreaterThanOrEqualTo(threshold);

        /// <summary>
        /// Checks if the value is less than a specified threshold.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsLessThan<T>(T threshold) where T : IComparable<T> =>
            value => value.IsLessThan(threshold);

        /// <summary>
        /// Checks if the value is less than or equal to a specified threshold.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsLessThanOrEqualTo<T>(T threshold) where T : IComparable<T> =>
            value => value.IsLessThanOrEqualTo(threshold);

        /// <summary>
        /// Checks if the value is required (i.e., not null or empty).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static ValidatorRule<Option<T>> Required<T>(ValidatorRule<T> func) =>
            option => option.Required(func);

        /// <summary>
        /// Checks if the value is required (i.e., not null or empty).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ValidatorRule<Option<T>> Required<T>() =>
            option => option.Required(x => field => Result.Ok());

        /// <summary>
        /// Checks if the value is optional (i.e., can be null or empty).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static ValidatorRule<Option<T>> Optional<T>(ValidatorRule<T> func) =>
            option => option.Optional(func);

        /// <summary>
        /// Checks if the value is valid according to a specified validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validator"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsValid<T>(IValidator<T> validator) =>
            value => field => validator.Validate(value).Map(_ => Unit.Value);

        /// <summary>
        /// Provides a set of validation checks for string values.
        /// </summary>
        public static class String
        {
            /// <summary>
            /// Checks if the string is empty.
            /// </summary>
            public static ValidatorRule<string> IsEmpty =>
                value => value.IsEmpty();

            /// <summary>
            /// Checks if the string is not empty.
            /// </summary>
            public static ValidatorRule<string> IsNotEmpty =>
                value => value.IsNotEmpty();

            /// <summary>
            /// Checks if the string starts with a specified prefix.
            /// </summary>
            /// <param name="prefix"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsStartingWith(string prefix) =>
                value => value.IsStartingWith(prefix);

            /// <summary>
            /// Checks if the string ends with a specified suffix.
            /// </summary>
            /// <param name="suffix"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsEndingWith(string suffix) =>
                value => value.IsEndingWith(suffix);

            /// <summary>
            /// Checks if the string contains a specified substring.
            /// </summary>
            /// <param name="substring"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsContaining(string substring) =>
                value => value.IsContaining(substring);

            /// <summary>
            /// Checks if the string has a specific length.
            /// </summary>
            /// <param name="length"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsLength(int length) =>
                value => value.IsLength(length);

            /// <summary>
            /// Checks if the string length is between two specified values.
            /// </summary>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsLengthBetween(int min, int max) =>
                value => value.IsLengthBetween(min, max);

            /// <summary>
            /// Checks if the string length is greater than a specified value.
            /// </summary>
            /// <param name="min"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsLengthGreaterThan(int min) =>
                value => value.IsLengthGreaterThan(min);

            /// <summary>
            /// Checks if the string length is greater than or equal to a specified value.
            /// </summary>
            /// <param name="min"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsLengthOrGreaterThan(int min) =>
                value => value.IsLengthOrGreaterThan(min);

            /// <summary>
            /// Checks if the string length is less than a specified value.
            /// </summary>
            /// <param name="max"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsLengthLessThan(int max) =>
                value => value.IsLengthLessThan(max);

            /// <summary>
            /// Checks if the string length is less than or equal to a specified value.
            /// </summary>
            /// <param name="max"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsLengthOrLessThan(int max) =>
                value => value.IsLengthOrLessThan(max);

            /// <summary>
            /// Checks if the string matches a specified regular expression pattern.
            /// </summary>
            /// <param name="pattern"></param>
            /// <param name="message"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsMatch(string pattern, Func<string, string>? message = null) =>
                value => value.IsMatch(pattern, message);

            /// <summary>
            /// Checks if the string is a valid email address.
            /// </summary>
            public static ValidatorRule<string> IsEmailAddress =>
                value => field =>
                    Regex.IsMatch(
                        input: value,
                        pattern: @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
                        options: RegexOptions.Compiled | RegexOptions.IgnoreCase)
                        && RuleHelper.CheckEmail(value)
                        ? Result.Ok()
                        : Result.Error($"'{field}' is not a valid email address");

            /// <summary>
            /// /// Checks if the string is a valid URL.
            /// </summary>
            public static ValidatorRule<string> IsUrl =>
                value => field =>
                    Uri.TryCreate(value, UriKind.Absolute, out var uri)
                        ? Result.Ok()
                        : Result.Error($"'{field}' is not a valid URL");

            /// <summary>
            /// /// Checks if the string is a valid E.164 phone number.
            /// </summary>
            public static ValidatorRule<string> IsE164 =>
                IsMatch(
                    pattern: @"^\+[1-9]\d{1,14}$",
                    message: field => $"'{field}' is not a valid E.164 phone number");
        }

        /// <summary>
        /// Provides a set of validation checks for GUID values.
        /// </summary>
        public static class Guid
        {
            /// <summary>
            /// Checks if the GUID is empty (i.e., 000-0000-0000-0000-0000).
            /// </summary>
            public static ValidatorRule<System.Guid> IsEmpty =>
                value => value.IsEmpty();

            /// <summary>
            /// Checks if the GUID is not empty (i.e., 000-0000-0000-0000-0000).
            /// </summary>
            public static ValidatorRule<System.Guid> IsNotEmpty =>
                value => value.IsNotEmpty();
        }

        /// <summary>
        /// Provides a set of validation checks for collection types.
        /// </summary>
        public static class Enumerable
        {
            /// <summary>
            /// Checks if the collection is empty.
            /// </summary>
            public static ValidatorRule<T> IsEmpty<T>() where T : IEnumerable<T> =>
                value => field => RuleHelper.Check(!value.Any(), $"'{field}' must be empty");

            /// <summary>
            /// Checks if the collection is not empty.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static ValidatorRule<T> IsNotEmpty<T>() where T : IEnumerable<T> =>
                value => field => RuleHelper.Check(value.Any(), $"'{field}' must be empty");
        }
    }

    internal static class EquatableRulesExtensions
    {
        internal static LabeledValidatorRule IsEqualTo<T>(this T value, T threshold) where T : IEquatable<T> =>
            field => RuleHelper.Check(value.Equals(threshold), $"'{field}' must be equal to {threshold}");

        internal static LabeledValidatorRule IsNotEqualTo<T>(this T value, T threshold) where T : IEquatable<T> =>
            field => RuleHelper.Check(!value.Equals(threshold), $"'{field}' must not equal {threshold}");
    }

    internal static class ComparableRulesExtensions
    {
        internal static LabeledValidatorRule IsBetween<T>(this T value, T min, T max) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0, $"'{field}' must be between {min} and {max}");

        internal static LabeledValidatorRule IsNotBetween<T>(this T value, T min, T max) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(min) < 0 || value.CompareTo(max) > 0, $"'{field}' must be outside the range {min} to {max}");

        internal static LabeledValidatorRule IsPositive<T>(this T value) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(default!) > 0, $"'{field}' must be positive");

        internal static LabeledValidatorRule IsNegative<T>(this T value) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(default!) < 0, $"'{field}' must be negative");

        internal static LabeledValidatorRule IsZero<T>(this T value) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(default!) == 0, $"'{field}' must be zero");

        internal static LabeledValidatorRule IsGreaterThan<T>(this T value, T threshold) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(threshold) > 0, $"'{field}' must be greater than {threshold}");

        internal static LabeledValidatorRule IsGreaterThanOrEqualTo<T>(this T value, T threshold) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(threshold) >= 0, $"'{field}' must be greater than or equal to {threshold}");

        internal static LabeledValidatorRule IsLessThan<T>(this T value, T threshold) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(threshold) < 0, $"'{field}' must be less than {threshold}");

        internal static LabeledValidatorRule IsLessThanOrEqualTo<T>(this T value, T threshold) where T : IComparable<T> =>
            field => RuleHelper.Check(value.CompareTo(threshold) <= 0, $"'{field}' must be less than or equal to {threshold}");
    }

    internal static class StringRulesExtensions
    {
        internal static LabeledValidatorRule IsEmpty(this string value) =>
            field => RuleHelper.Check(string.IsNullOrWhiteSpace(value), $"'{field}' must be empty");

        internal static LabeledValidatorRule IsNotEmpty(this string value) =>
            field => RuleHelper.Check(!string.IsNullOrWhiteSpace(value), $"'{field}' must not be empty");

        internal static LabeledValidatorRule IsStartingWith(this string value, string prefix) =>
            field => RuleHelper.Check(value.StartsWith(prefix), $"'{field}' must start with '{prefix}'");

        internal static LabeledValidatorRule IsEndingWith(this string value, string suffix) =>
            field => RuleHelper.Check(value.EndsWith(suffix), $"'{field}' must end with '{suffix}'");

        internal static LabeledValidatorRule IsContaining(this string value, string substring) =>
            field => RuleHelper.Check(value.Contains(substring), $"'{field}' must contain '{substring}'");

        internal static LabeledValidatorRule IsLength(this string value, int length) =>
            field => RuleHelper.Check(value.Length == length, $"'{field}' must be {length} characters");

        internal static LabeledValidatorRule IsLengthBetween(this string value, int min, int max) =>
            field => RuleHelper.Check(value.Length >= min && value.Length <= max, $"'{field}' must be between {min} and {max} characters");

        internal static LabeledValidatorRule IsLengthGreaterThan(this string value, int min) =>
            field => RuleHelper.Check(value.Length > min, $"'{field}' must be longer than {min} characters");

        internal static LabeledValidatorRule IsLengthOrGreaterThan(this string value, int min) =>
            field => RuleHelper.Check(value.Length >= min, $"'{field}' must be longer than or equal to {min} characters");

        internal static LabeledValidatorRule IsLengthLessThan(this string value, int max) =>
            field => RuleHelper.Check(value.Length < max, $"'{field}' must be less than {max} characters");

        internal static LabeledValidatorRule IsLengthOrLessThan(this string value, int max) =>
            field => RuleHelper.Check(value.Length <= max, $"'{field}' must be less than or equal to {max} characters");

        internal static LabeledValidatorRule IsMatch(this string value, string pattern, Func<string, string>? fieldMessage = null) =>
            field => RuleHelper.Check(
                Regex.IsMatch(value, pattern),
                fieldMessage is Func<string, string> fn
                    ? fn(field)
                    : $"'{field}' must match the pattern '{pattern}'");
    }

    internal static class GuidRulesExtensions
    {
        internal static LabeledValidatorRule IsEmpty(this Guid value) =>
            field => RuleHelper.Check(value == Guid.Empty, $"'{field}' must be empty");

        internal static LabeledValidatorRule IsNotEmpty(this Guid value) =>
            field => RuleHelper.Check(value != Guid.Empty, $"'{field}' must not be empty");
    }

    internal static class OptionRulesExtensions
    {
        internal static LabeledValidatorRule Required<T>(this Option<T> option, ValidatorRule<T> func) =>
            option.Match(
                some: value => func(value),

                none: () => field => Result.Error($"'{field}' is required"));

        internal static LabeledValidatorRule Required<T>(this Option<T> option) =>
            option.Required(x => field => Result.Ok());

        internal static LabeledValidatorRule Optional<T>(this Option<T> option, ValidatorRule<T> func) =>
            option.Match(
                some: value => func(value),

                none: () => field => Result<Unit, ResultErrors>.Ok(Unit.Value));
    }

    internal static class ValidatorRulesExtensions
    {
        internal static LabeledValidatorRule IsValid<T>(this T value, IValidator<T> validator) =>
            field => validator.Validate(value).Map(_ => Unit.Value);
    }

    internal static class RuleHelper
    {
        internal static Result<Unit, ResultErrors> Check(bool isValid, string errorMessage) =>
            isValid
                ? Result.Ok()
                : Result.Error(new[] { errorMessage });

        internal static bool CheckEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
