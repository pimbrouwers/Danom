namespace Danom.Validation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an <see cref="IValidator{T}" /> as a
    /// <see cref="Result{T, ResultErrors}" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ValidationResult<T>
    {
        /// <summary>
        /// Converts an input value to a <see cref="Result{T, ResultErrors}" />
        /// using the provided validator.
        /// </summary>
        /// <typeparam name="TValidator"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Result<T, ResultErrors> From<TValidator>(T input)
            where TValidator : IValidator<T>, new()
        {
            var validator = new TValidator();
            return validator.Validate(input);
        }
    }

    /// <summary>
    /// Represents an <see cref="IValidator{T}" /> as a
    /// <see cref="Option{T}" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class ValidationOption<T>
    {
        /// <summary>
        /// Converts an input value to an <see cref="Option{T}" /> based on the
        /// result of the provided validator. If the input is valid, then Some else
        /// None.
        /// </summary>
        /// <typeparam name="TValidator"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Option<T> From<TValidator>(T input)
            where TValidator : IValidator<T>, new() =>
            ValidationResult<T>.From<TValidator>(input).ToOption();
    }

    /// <summary>
    /// Base class for validators that implements the
    /// <see cref="IValidator{T}" /> interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseValidator<T> : IValidator<T>
    {
        private readonly ValidationContext<T> _validationContext = new ValidationContext<T>();

        /// <summary>
        /// Validates the input value against the defined rules.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Result<T, ResultErrors> Validate(T value)
        {
            var isValid = true;
            var resultErrors = new ResultErrors();

            foreach (var rule in _validationContext.Rules)
            {
                foreach (var fieldRule in rule.Value)
                {
                    var labelledRuleFunc = fieldRule.ValidatorRule(value);
                    if (labelledRuleFunc(fieldRule.Field).TryGetError(out var errors))
                    {
                        if (isValid)
                        {
                            isValid = false;
                        }

                        foreach (var error in errors)
                        {
                            foreach (var errorMessage in error.Errors)
                            {
                                resultErrors.Add(rule.Key, fieldRule.Message ?? errorMessage);
                            }
                        }
                    }
                }
            }

            return isValid
                ? Result<T>.Ok(value)
                : Result<T>.Error(resultErrors);
        }

        /// <summary>
        /// Adds a validation rule for a specific field in the input value.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="field"></param>
        /// <param name="selector"></param>
        /// <param name="rule"></param>
        /// <param name="message"></param>
        public void Rule<U>(string? field, Func<T, U> selector, ValidatorRule<U> rule, string? message = null) =>
            _validationContext.AddRule(
                field: field,
                rule: value => rule(selector(value)),
                message: message);

        /// <summary>
        /// Adds a validation rule for a specific field in the input values.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="field"></param>
        /// <param name="selector"></param>
        /// <param name="rules"></param>
        /// <param name="message"></param>
        public void Rule<U>(string? field, Func<T, U> selector, IEnumerable<ValidatorRule<U>> rules, string? message = null)
        {
            foreach (var rule in rules)
            {
                Rule(field, selector, rule, message);
            }
        }

        /// <summary>
        /// Adds a validation rule for a specific field in the input value.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="selector"></param>
        /// <param name="rule"></param>
        /// <param name="message"></param>
        public void Rule<U>(Func<T, U> selector, ValidatorRule<U> rule, string? message = null) =>
            Rule(null, selector, rule, message);

        /// <summary>
        /// Adds a validation rule for a specific field in the input values.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="selector"></param>
        /// <param name="rules"></param>
        /// <param name="message"></param>
        public void Rule<U>(Func<T, U> selector, IEnumerable<ValidatorRule<U>> rules, string? message = null) =>
            Rule(null, selector, rules, message);
    }

    internal sealed class ValidationContext<T>
    {
        private const string DefaultField = "Value";
        private const string DefaultKey = "";

        public ValidationContext()
        {
             Rules = new Dictionary<string, List<FieldRule>>();
        }

        public Dictionary<string, List<FieldRule>> Rules { get; }

        public sealed class FieldRule
        {
            public FieldRule(string field, ValidatorRule<T> validatorRule, string? message = null)
            {
                Field = field;
                ValidatorRule = validatorRule;
                Message = message;
            }

            public string Field { get; }
            public ValidatorRule<T> ValidatorRule { get; }
            public string? Message { get; }
        }

        public void AddRule(string? field, ValidatorRule<T> rule, string? message)
        {
            var key = field ?? DefaultKey;
            var fieldname = field ?? DefaultField;

            if (!Rules.TryGetValue(key, out var rules))
            {
                Rules[key] = new List<FieldRule>();
            }
            Rules[key].Add(new FieldRule(fieldname, rule, message));
        }
    }
    /// <summary>
    /// Represents a validator interface for validating input values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidator<T>
    {
        /// <summary>
        /// Validates the input value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        Result<T, ResultErrors> Validate(T value);
    }

    /// <summary>
    /// Represents a rule for validating a field in the input value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate LabeledValidatorRule ValidatorRule<T>(T value);

    /// <summary>
    /// Represents a rule that validates a field with a label.
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public delegate Result<Unit, ResultErrors> LabeledValidatorRule(string field);

}
