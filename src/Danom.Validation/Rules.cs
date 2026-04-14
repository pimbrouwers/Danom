namespace Danom.Validation {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using System.Text.RegularExpressions;
#if NET7_0_OR_GREATER && !NETSTANDARD2_1
    using System.Numerics;
#endif

    /// <summary>
    /// Provides a set of validation checks that can be used in validators.
    /// </summary>
    public static class Check {
        /// <summary>
        /// Checks if the value is equal to a specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsEqualTo<T>(T threshold) =>
            value => field => RuleHelper.Check(
                EqualityComparer<T>.Default.Equals(value, threshold),
                $"'{field}' must be equal to {threshold}");

        /// <summary>
        /// Checks if the value is not equal to a specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsNotEqualTo<T>(T input) =>
            value => field => RuleHelper.Check(
                !EqualityComparer<T>.Default.Equals(value, input),
                $"'{field}' must not equal {input}");

        /// <summary>
        /// Checks if the value is between two specified values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsBetween<T>(T min, T max) where T : IComparable<T> =>
            value => field => RuleHelper.Check(
                Comparer<T>.Default.Compare(value, min) >= 0
                && Comparer<T>.Default.Compare(value, max) <= 0,
                $"'{field}' must be between {min} and {max}");

        /// <summary>
        /// Checks if the value is not between two specified values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsNotBetween<T>(T min, T max) where T : IComparable<T> =>
            value => field => RuleHelper.Check(
                Comparer<T>.Default.Compare(value, min) < 0
                || Comparer<T>.Default.Compare(value, max) > 0,
                $"'{field}' must be outside the range {min} to {max}");

        /// <summary>
        /// Checks if the value is positive.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
#if NET7_0_OR_GREATER && !NETSTANDARD2_1
        public static ValidatorRule<T> IsPositive<T>() where T : INumber<T> =>
            value => field => RuleHelper.Check(value > T.Zero, $"'{field}' must be positive");
#else
        public static ValidatorRule<T> IsPositive<T>() where T : IComparable<T> {
            EnsureNumericComparableType<T>(nameof(IsPositive));
            return value => field => RuleHelper.Check(Comparer<T>.Default.Compare(value, default!) > 0, $"'{field}' must be positive");
        }
#endif

        /// <summary>
        /// Checks if the value is negative.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
#if NET7_0_OR_GREATER && !NETSTANDARD2_1
        public static ValidatorRule<T> IsNegative<T>() where T : INumber<T> =>
            value => field => RuleHelper.Check(value < T.Zero, $"'{field}' must be negative");
#else
        public static ValidatorRule<T> IsNegative<T>() where T : IComparable<T> {
            EnsureNumericComparableType<T>(nameof(IsNegative));
            return value => field => RuleHelper.Check(Comparer<T>.Default.Compare(value, default!) < 0, $"'{field}' must be negative");
        }
#endif

        /// <summary>
        /// Checks if the value is zero.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
#if NET7_0_OR_GREATER && !NETSTANDARD2_1
        public static ValidatorRule<T> IsZero<T>() where T : INumber<T> =>
            value => field => RuleHelper.Check(value == T.Zero, $"'{field}' must be zero");
#else
        public static ValidatorRule<T> IsZero<T>() where T : IComparable<T> {
            EnsureNumericComparableType<T>(nameof(IsZero));
            return value => field => RuleHelper.Check(Comparer<T>.Default.Compare(value, default!) == 0, $"'{field}' must be zero");
        }

        private static void EnsureNumericComparableType<T>(string ruleName) {
            var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            if (!IsNumericType(type)) {
                throw new NotSupportedException(
                    $"{ruleName} only supports numeric types. Type '{type.Name}' is not supported.");
            }
        }

        private static bool IsNumericType(Type type) =>
            type == typeof(byte)
            || type == typeof(sbyte)
            || type == typeof(short)
            || type == typeof(ushort)
            || type == typeof(int)
            || type == typeof(uint)
            || type == typeof(long)
            || type == typeof(ulong)
            || type == typeof(float)
            || type == typeof(double)
            || type == typeof(decimal);
#endif

        /// <summary>
        /// Checks if the value is greater than a specified threshold.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsGreaterThan<T>(T threshold) where T : IComparable<T> =>
            value => field => RuleHelper.Check(
                Comparer<T>.Default.Compare(value, threshold) > 0,
                $"'{field}' must be greater than {threshold}");

        /// <summary>
        /// Checks if the value is greater than or equal to a specified threshold.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsGreaterThanOrEqualTo<T>(T threshold) where T : IComparable<T> =>
            value => field => RuleHelper.Check(
                Comparer<T>.Default.Compare(value, threshold) >= 0,
                $"'{field}' must be greater than or equal to {threshold}");

        /// <summary>
        /// Checks if the value is less than a specified threshold.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsLessThan<T>(T threshold) where T : IComparable<T> =>
            value => field => RuleHelper.Check(
                Comparer<T>.Default.Compare(value, threshold) < 0,
                $"'{field}' must be less than {threshold}");

        /// <summary>
        /// Checks if the value is less than or equal to a specified threshold.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsLessThanOrEqualTo<T>(T threshold) where T : IComparable<T> =>
            value => field => RuleHelper.Check(
                Comparer<T>.Default.Compare(value, threshold) <= 0,
                $"'{field}' must be less than or equal to {threshold}");

        /// <summary>
        /// Checks if the value is required and satisfies the specified rule.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static ValidatorRule<Option<T>> Required<T>(ValidatorRule<T> func) =>
            optionValue => optionValue.Match(
                some: value => func(value),
                none: () => field => Result.Error($"'{field}' is required"));

        /// <summary>
        /// Checks if the value is required and satisfies all the specified rules.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcs"></param>
        /// <returns></returns>
        public static ValidatorRule<Option<T>> Required<T>(params ValidatorRule<T>[] funcs) =>
            Required<T>(value => field => EvaluateRules(value, field, funcs));

        /// <summary>
        /// Checks if the Option value is some.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ValidatorRule<Option<T>> Required<T>() =>
            Required<T>(x => field => Result.Ok());

        /// <summary>
        /// Checks if the Option value is some, and satisfies the specified rule.
        /// Or, if the Option is none, it is considered valid.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static ValidatorRule<Option<T>> Optional<T>(ValidatorRule<T> func) =>
            optionValue => optionValue.Match(
                some: value => func(value),
                none: () => field => Result<Unit, ResultErrors>.Ok(Unit.Value));

        /// <summary>
        /// Checks if the Option value is some, and satisfies all the specified rules.
        /// Or, if the Option is none, it is considered valid.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcs"></param>
        /// <returns></returns>
        public static ValidatorRule<Option<T>> Optional<T>(params ValidatorRule<T>[] funcs) =>
            Optional<T>(value => field => EvaluateRules(value, field, funcs));

        private static Result<Unit, ResultErrors> EvaluateRules<T>(
            T value,
            string field,
            IEnumerable<ValidatorRule<T>> funcs) {
            var resultErrors = new ResultErrors();
            var isValid = true;

            foreach (var func in funcs) {
                var rule = func(value);
                var result = rule(field);

                if (result.TryGetError(out var errors)) {
                    if (isValid) {
                        isValid = false;
                    }

                    resultErrors.Add(errors);
                }
            }

            return isValid
                ? Result.Ok()
                : Result.Error(resultErrors);
        }

        /// <summary>
        /// Checks if the value is valid according to a specified validator.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validator"></param>
        /// <returns></returns>
        public static ValidatorRule<T> IsValid<T>(IValidator<T> validator) =>
            value => field => validator.Validate(value)
                .Match<Result<Unit, ResultErrors>>(
                    ok: _ => Result.Ok(),
                    error: errors => Result.Error(PrefixNestedErrors(field, errors)));

        private static ResultErrors PrefixNestedErrors(string field, ResultErrors errors) {
            var prefixedErrors = new ResultErrors();

            foreach (var error in errors) {
                var nestedFieldPath = CombineFieldPath(field, error.Key);

                foreach (var message in error.Errors) {
                    prefixedErrors.Add(nestedFieldPath, message);
                }
            }

            return prefixedErrors;
        }

        private static string CombineFieldPath(string parentField, string nestedKey) {
            if (string.IsNullOrWhiteSpace(parentField)) {
                return nestedKey;
            }

            if (string.IsNullOrWhiteSpace(nestedKey)) {
                return parentField;
            }

            return $"{parentField}.{nestedKey}";
        }

        /// <summary>
        /// Provides a set of validation checks for string values.
        /// </summary>
        public static class String {
            private static readonly Regex E164Regex = new Regex(
                pattern: @"^\+[1-9]\d{1,14}$",
                options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

            private static readonly Regex EmailAddressRegex = new Regex(
                pattern: @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
                options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

            /// <summary>
            /// Checks if the string is empty.
            /// </summary>
            public static ValidatorRule<string> IsEmpty =>
                value => field => RuleHelper.Check(string.IsNullOrWhiteSpace(value), $"'{field}' must be empty");

            /// <summary>
            /// Checks if the string is not empty.
            /// </summary>
            public static ValidatorRule<string> IsNotEmpty =>
                value => field => RuleHelper.Check(!string.IsNullOrWhiteSpace(value), $"'{field}' must not be empty");

            /// <summary>
            /// Checks if the string starts with a specified prefix.
            /// </summary>
            /// <param name="prefix"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsStartingWith(string prefix) =>
                value => field => RuleHelper.Check(
                    !string.IsNullOrEmpty(value) && value.StartsWith(prefix, StringComparison.Ordinal),
                    $"'{field}' must start with '{prefix}'");

            /// <summary>
            /// Checks if the string ends with a specified suffix.
            /// </summary>
            /// <param name="suffix"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsEndingWith(string suffix) =>
                value => field => RuleHelper.Check(
                    !string.IsNullOrEmpty(value) && value.EndsWith(suffix, StringComparison.Ordinal),
                    $"'{field}' must end with '{suffix}'");

            /// <summary>
            /// Checks if the string contains a specified substring.
            /// </summary>
            /// <param name="substring"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsContaining(string substring) =>
                value => field => RuleHelper.Check(
                    !string.IsNullOrEmpty(value) && value.Contains(substring, StringComparison.Ordinal),
                    $"'{field}' must contain '{substring}'");

            /// <summary>
            /// Checks if the string has a specific length.
            /// </summary>
            /// <param name="length"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsLength(int length) =>
                value => field => RuleHelper.Check(
                    value != null && value.Length == length,
                    $"'{field}' must be {length} characters");

            /// <summary>
            /// Checks if the string length is between two specified values.
            /// </summary>
            /// <param name="min"></param>
            /// <param name="max"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsLengthBetween(int min, int max) =>
                value => field => RuleHelper.Check(
                    value != null && value.Length >= min && value.Length <= max,
                    $"'{field}' must be between {min} and {max} characters");

            /// <summary>
            /// Checks if the string length is greater than a specified value.
            /// </summary>
            /// <param name="min"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsLengthGreaterThan(int min) =>
                value => field => RuleHelper.Check(
                    value != null && value.Length > min,
                    $"'{field}' must be longer than {min} characters");

            /// <summary>
            /// Checks if the string length is greater than or equal to a specified value.
            /// </summary>
            /// <param name="min"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsLengthOrGreaterThan(int min) =>
                value => field => RuleHelper.Check(
                    value != null && value.Length >= min,
                    $"'{field}' must be longer than or equal to {min} characters");

            /// <summary>
            /// Checks if the string length is less than a specified value.
            /// </summary>
            /// <param name="max"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsLengthLessThan(int max) =>
                value => field => RuleHelper.Check(
                    value != null && value.Length < max,
                    $"'{field}' must be less than {max} characters");

            /// <summary>
            /// Checks if the string length is less than or equal to a specified value.
            /// </summary>
            /// <param name="max"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsLengthOrLessThan(int max) =>
                value => field => RuleHelper.Check(
                    value != null && value.Length <= max,
                    $"'{field}' must be less than or equal to {max} characters");

            /// <summary>
            /// Checks if the string matches a specified regular expression pattern.
            /// </summary>
            /// <param name="pattern"></param>
            /// <param name="message"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsMatch(string pattern, Func<string, string>? message = null) =>
                value => field => RuleHelper.Check(
                    !string.IsNullOrEmpty(value) && Regex.IsMatch(value, pattern),
                    message is Func<string, string> fn
                        ? fn(field)
                        : $"'{field}' must match the pattern '{pattern}'");

            /// <summary>
            /// Checks if the string matches a specified regular expression.
            /// </summary>
            /// <param name="regex"></param>
            /// <param name="message"></param>
            /// <returns></returns>
            public static ValidatorRule<string> IsMatch(Regex regex, Func<string, string>? message = null) =>
                value => field => RuleHelper.Check(
                    !string.IsNullOrEmpty(value) && regex.IsMatch(value),
                    message is Func<string, string> fn
                        ? fn(field)
                        : $"'{field}' must match the pattern '{regex}'");

            /// <summary>
            /// /// Checks if the string is a valid URL.
            /// </summary>
            public static ValidatorRule<string> IsUrl =>
                value => field =>
                    !string.IsNullOrWhiteSpace(value)
                        && Uri.TryCreate(value, UriKind.Absolute, out var uri)
                        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
                        ? Result.Ok()
                        : Result.Error($"'{field}' is not a valid URL");

            /// <summary>
            /// /// Checks if the string is a valid E.164 phone number.
            /// </summary>
            public static ValidatorRule<string> IsE164 =>
                IsMatch(E164Regex, field => $"'{field}' is not a valid E.164 phone number");

            /// <summary>
            /// Checks if the string is a valid email address.
            /// </summary>
            public static ValidatorRule<string> IsEmailAddress =>
                IsMatch(EmailAddressRegex, field => $"'{field}' is not a valid email address");
        }

        /// <summary>
        /// Provides a set of validation checks for GUID values.
        /// </summary>
        public static class Guid {
            /// <summary>
            /// Checks if the GUID is empty (i.e., 000-0000-0000-0000-0000).
            /// </summary>
            public static ValidatorRule<System.Guid> IsEmpty =>
                value => field => RuleHelper.Check(value == System.Guid.Empty, $"'{field}' must be empty");

            /// <summary>
            /// Checks if the GUID is not empty (i.e., 000-0000-0000-0000-0000).
            /// </summary>
            public static ValidatorRule<System.Guid> IsNotEmpty =>
                value => field => RuleHelper.Check(value != System.Guid.Empty, $"'{field}' must not be empty");
        }

        /// <summary>
        /// Provides a set of validation checks for collection types.
        /// </summary>
        public static class Enumerable {
            /// <summary>
            /// Checks if the collection is empty.
            /// </summary>
            public static ValidatorRule<IEnumerable<T>> IsEmpty<T>() =>
                value => field => RuleHelper.Check(value is null || !value.Any(), $"'{field}' must be empty");

            /// <summary>
            /// Checks if the collection is not empty.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static ValidatorRule<IEnumerable<T>> IsNotEmpty<T>() =>
                value => field => RuleHelper.Check(value != null && value.Any(), $"'{field}' must not be empty");

            /// <summary>
            /// Checks if each element in the collection satisfies a specified
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="rule"></param>
            /// <returns></returns>
            public static ValidatorRule<IEnumerable<T>> ForEach<T>(ValidatorRule<T> rule) =>
                values => field => EvaluateEach(
                    values,
                    field,
                    (item, indexedField) => {
                        var itemRule = rule(item);
                        return itemRule(indexedField);
                    });

            /// <summary>
            /// Checks if each element in the collection satisfies all specified
            /// rules.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="rules"></param>
            /// <returns></returns>
            public static ValidatorRule<IEnumerable<T>> ForEach<T>(params ValidatorRule<T>[] rules) =>
                values => field => EvaluateEach(
                    values,
                    field,
                    (item, indexedField) => EvaluateRules(item, indexedField, rules));

            /// <summary>
            /// Checks if each element in the collection satisfies a specified
            /// validator.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="validator"></param>
            /// <returns></returns>
            public static ValidatorRule<IEnumerable<T>> ForEach<T>(IValidator<T> validator) =>
                values => field => EvaluateEach(
                    values,
                    field,
                    (item, indexedField) => validator.Validate(item)
                        .Match<Result<Unit, ResultErrors>>(
                            ok: _ => Result.Ok(),
                            error: errors => Result.Error(PrefixNestedErrors(indexedField, errors))));

            private static Result<Unit, ResultErrors> EvaluateEach<T>(
                IEnumerable<T>? values,
                string field,
                Func<T, string, Result<Unit, ResultErrors>> evaluator) {
                var resultErrors = new ResultErrors();
                var isValid = true;
                var index = 0;

                if (values is null) {
                    return Result.Ok();
                }

                foreach (var item in values) {
                    var indexedField = BuildIndexedField(field, index);
                    var result = evaluator(item, indexedField);

                    if (result.TryGetError(out var errors)) {
                        if (isValid) {
                            isValid = false;
                        }

                        resultErrors.Add(errors);
                    }

                    index++;
                }

                return isValid
                    ? Result.Ok()
                    : Result.Error(resultErrors);
            }

            private static string BuildIndexedField(string field, int index) =>
                string.IsNullOrWhiteSpace(field)
                    ? $"[{index}]"
                    : $"{field}[{index}]";
        }
    }

    internal static class RuleHelper {
        internal static Result<Unit, ResultErrors> Check(bool isValid, string errorMessage) =>
            isValid
                ? Result.Ok()
                : Result.Error(new[] { errorMessage });
    }
}
