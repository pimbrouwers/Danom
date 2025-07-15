namespace Danom.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class BaseValidator<T> : IValidator<T>
    {
        private const string DefaultKey = ""; // Default key for rules without a specific field
        // private readonly List<ValidatorRule<T>> _rules = new List<ValidatorRule<T>>();
        private readonly Dictionary<string, List<ValidatorRule<T>>> _fieldRules = new Dictionary<string, List<ValidatorRule<T>>>();

        public Result<T, ResultErrors> Validate(T value)
        {
            var isValid = true;
            var resultErrors = new ResultErrors();

            foreach (var rule in _fieldRules)
            {
                var field = rule.Key;
                var rules = rule.Value;

                foreach (var ruleFunc in rules)
                {
                    if (ruleFunc(value).TryGetError(out var errors))
                    {
                        if (isValid)
                        {
                            isValid = false;
                        }

                        foreach (var error in errors)
                        {
                            resultErrors.Add(error);
                        }
                    }
                }
            }
            // {
            //     if (rule(value).TryGetError(out var errors))
            //     {
            //         if (isValid)
            //         {
            //             isValid = false;
            //         }

            //         foreach (var error in errors)
            //         {
            //             resultErrors.Add(error);
            //         }
            //     }
            // }

            return isValid
                ? Result.Ok(value)
                : Result<T>.Error(resultErrors);
        }

        public void Rule(ValidatorRule<T> rule, string? message = null) =>
            AddRule(DefaultKey, rule);

        public void Rule(string field, ValidatorRule<T> rule, string? message = null) =>
            AddRule(field, rule);

        public void Rule<U>(string field, Func<T, U> selector, IEnumerable<ValidatorRule<U>> rules)
        {
            foreach (var rule in rules)
            {
                Rule(value =>
                {
                    var selectedValue = selector(value);
                    return rule(selectedValue);
                });
            }
        }

        private void AddRule(string? field, ValidatorRule<T> rule)
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

    public delegate Result<Unit, ResultErrors> ValidatorRule<T>(T value);
}
