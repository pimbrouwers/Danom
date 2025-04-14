namespace Danom.Validation;

using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

internal static partial class ValidationHelpers {
    internal static bool Validate<T, TValue>(
        ValidationContext<T> context,
        IValidator<TValue> validator,
        TValue instance) {
        var validationResult = validator.Validate(instance);

        if (!validationResult.IsValid) {
            var quotedDisplayName = string.Concat("'", context.DisplayName, "'");
            validationResult.Errors.ForEach(e =>
                context.AddFailure(
                    propertyName: context.DisplayName,
                    errorMessage: ReplaceMissingDisplayName(e.ErrorMessage, quotedDisplayName)));
        }

        return validationResult.IsValid;
    }

    private static string ReplaceMissingDisplayName(string errorMessage, string properDisplayName) =>
        QuotedDisplayNameRegex().Replace(errorMessage, properDisplayName);

    [GeneratedRegex(@"''")]
    private static partial Regex QuotedDisplayNameRegex();
}

/// <summary>
/// A validator for <see cref="Option{TValue}"/> that validates the value if it
/// is Some, otherwise the value is considered valid.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="validator"></param>
public sealed class OptionalValidator<T, TValue>(IValidator<TValue> validator)
    : PropertyValidator<T, Option<TValue>> {
    /// <summary>
    /// Specifies the name of the validator.
    /// </summary>
    public override string Name => "OptionValidator";

    /// <summary>
    /// Returns whether the Option is valid given the specified context.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool IsValid(ValidationContext<T> context, Option<TValue> value) =>
        value.Match(
            some: x => ValidationHelpers.Validate(context, validator, x),
            none: () => true);

    /// <summary>
    /// Returns the default error message template for the validator.
    /// </summary>
    /// <param name="errorCode"></param>
    /// <returns></returns>
    protected override string GetDefaultMessageTemplate(string errorCode) =>
        "'{PropertyName}' is optional, but invalid.";
}

/// <summary>
/// A validator for <see cref="Option{TValue}"/> that validates the value if it
/// is Some, otherwise the value is considered invalid.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="validator"></param>
public class RequiredValidator<T, TValue>(IValidator<TValue> validator) : PropertyValidator<T, Option<TValue>> {
    /// <summary>
    /// Specifies the name of the validator.
    /// </summary>
    public override string Name => "OptionNotNoneValidator";

    /// <summary>
    /// Returns whether the Option is valid given the specified context.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public override bool IsValid(ValidationContext<T> context, Option<TValue> value) =>
        value.Match(
            some: x => ValidationHelpers.Validate(context, validator, x),
            none: () => false);

    /// <summary>
    /// Returns the default error message template for the validator.
    /// </summary>
    /// <param name="errorCode"></param>
    /// <returns></returns>
    protected override string GetDefaultMessageTemplate(string errorCode) =>
        "'{PropertyName}' is required and invalid or missing.";
}

/// <summary>
/// Contains extension methods for <see cref="IRuleBuilder{T, TValue}"/>.
/// </summary>
public static class OptionValidatorExtensions {
    /// <summary>
    /// Set the validator for an Option value. The value is considered valid if
    /// it is None.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="validator"></param>
    /// <returns></returns>
    public static IRuleBuilder<T, Option<TValue>> Optional<T, TValue>(
        this IRuleBuilder<T, Option<TValue>> ruleBuilder,
        IValidator<TValue> validator) =>
        ruleBuilder.SetValidator(new OptionalValidator<T, TValue>(validator));

    /// <summary>
    /// Specifies a validator for the value if it is Some, otherwise the value
    /// is considered valid.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IRuleBuilder<T, Option<TValue>> Optional<T, TValue>(
        this IRuleBuilder<T, Option<TValue>> ruleBuilder,
        Action<IRuleBuilder<TValue, TValue>> action) {
        var inlineValidator = new InlineValidator<TValue>();
        action(inlineValidator.RuleFor(x => x));
        var optionValidator = new OptionalValidator<T, TValue>(inlineValidator);
        return ruleBuilder.SetValidator(optionValidator);
    }

    /// <summary>
    /// Indicates that an <see cref="Option{T}"/> is required to have a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilder<T, Option<TValue>> Required<T, TValue>(
        this IRuleBuilder<T, Option<TValue>> ruleBuilder) =>
            ruleBuilder.SetValidator(new RequiredValidator<T, TValue>(new InlineValidator<TValue>()));

    /// <summary>
    /// Indicates that an <see cref="Option{T}"/> is required to have a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="validator"></param>
    /// <returns></returns>
    public static IRuleBuilder<T, Option<TValue>> Required<T, TValue>(
        this IRuleBuilder<T, Option<TValue>> ruleBuilder,
        IValidator<TValue> validator) =>
            ruleBuilder.SetValidator(new RequiredValidator<T, TValue>(validator));

    /// <summary>
    /// Indicates that an <see cref="Option{T}"/> is required to have a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IRuleBuilder<T, Option<TValue>> Required<T, TValue>(
        this IRuleBuilder<T, Option<TValue>> ruleBuilder,
        Action<IRuleBuilder<TValue, TValue>> action) {
        var inlineValidator = new InlineValidator<TValue>();
        action(inlineValidator.RuleFor(x => x));
        var optionValidator = new RequiredValidator<T, TValue>(inlineValidator);
        return ruleBuilder.SetValidator(optionValidator);
    }

}
