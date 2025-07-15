namespace Danom.Validation
{
    using System;
    using System.Collections.Generic;

    public abstract class BaseValidator<T> : IValidator<T>
    {
        private const string DefaultField = "Value";
        private const string DefaultKey = ""; // Default key for rules without a specific field

        private readonly Dictionary<string, List<ValidatorRule<T>>> _fieldRules = new Dictionary<string, List<ValidatorRule<T>>>();

        public Result<T, ResultErrors> Validate(T value)
        {
            var isValid = true;
            var resultErrors = new ResultErrors();

            foreach (var rule in _fieldRules)
            {
                var field = rule.Key == DefaultKey ? DefaultField : rule.Key;
                var rules = rule.Value;

                foreach (var ruleFunc in rules)
                {
                    var labelledRuleFunc = ruleFunc(value);
                    if (labelledRuleFunc(field).TryGetError(out var errors))
                    {
                        if (isValid)
                        {
                            isValid = false;
                        }

                        foreach (var error in errors)
                        {
                            foreach (var errorMessage in error.Errors)
                            {
                                resultErrors.Add(rule.Key, errorMessage);
                            }
                        }
                    }
                }
            }

            return isValid
                ? Result<T>.Ok(value)
                : Result<T>.Error(resultErrors);
        }

        public void Rule(string field, ValidatorRule<T> rule, string? message = null) =>
            AddRule(field: field, rule: rule, message: message);

        public void Rule<U>(string field, Func<T, U> selector, ValidatorRule<U> rule) =>
            Rule(field: field, rule: value => rule(selector(value)));

        public void Rule<U>(string field, Func<T, U> selector, IEnumerable<ValidatorRule<U>> rules)
        {
            foreach (var rule in rules)
            {
                Rule(field: field, selector: selector, rule: rule);
            }
        }

        public void Rule(ValidatorRule<T> rule, string? message = null) =>
            AddRule(DefaultKey, rule, message);

        public void Rule<U>(Func<T, U> selector, ValidatorRule<U> rule) =>
            Rule(DefaultKey, selector, rule);

        public void Rule<U>(Func<T, U> selector, IEnumerable<ValidatorRule<U>> rules) =>
            Rule(DefaultKey, selector, rules);

        private void AddRule(string? field, ValidatorRule<T> rule, string? message)
        {
            var key = field ?? DefaultKey;

            if (!_fieldRules.TryGetValue(key, out var rules))
            {
                _fieldRules[key] = new List<ValidatorRule<T>>();
            }
            _fieldRules[key].Add(rule);
        }
    }

    public interface IValidator<T>
    {
        Result<T, ResultErrors> Validate(T value);
    }

    public delegate LabeledValidatorRule ValidatorRule<T>(T value);

    public delegate Result<Unit, ResultErrors> LabeledValidatorRule(string field);
}
