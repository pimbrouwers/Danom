namespace Danom.Validation;

using FluentValidation;
using FluentValidation.Validators;

/// <summary>
/// A validator for <see cref="Option{TValue}"/> that validates the value if it is Some.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TValue"></typeparam>
/// <param name="validator"></param>
public class OptionValidator<T, TValue>(IValidator<TValue> validator) : PropertyValidator<T, Option<TValue>>
{
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
            some: x => validator.Validate(x).IsValid,
            none: () => true);
}

/// <summary>
/// Contains extension methods for <see cref="IRuleBuilder{T, TValue}"/>.
/// </summary>
public static class OptionValidatorExtensions
{
    /// <summary>
    /// Specifies a validator for the value if it is Some, otherwise the value
    /// is considered valid.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IRuleBuilder<T, Option<TValue>> WhenSome<T, TValue>(
        this IRuleBuilder<T, Option<TValue>> ruleBuilder,
        Action<IRuleBuilder<TValue, TValue>> action)
    {
        var inlineValidator = new InlineValidator<TValue>();
        action(inlineValidator.RuleFor(x => x));
        var optionValidator = new OptionValidator<T, TValue>(inlineValidator);
        return ruleBuilder.SetValidator(optionValidator);
    }

    /// <summary>
    /// Set the validator for an Option value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="validator"></param>
    /// <returns></returns>
    public static IRuleBuilder<T, Option<TValue>> SetValidator<T, TValue>(
        this IRuleBuilder<T, Option<TValue>> ruleBuilder,
        IValidator<TValue> validator)
    {
        return ruleBuilder.SetValidator(new OptionValidator<T, TValue>(validator));
    }
}
