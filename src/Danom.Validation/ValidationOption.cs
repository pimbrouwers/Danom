namespace Danom.Validation;

using FluentValidation;

/// <summary>
/// Represents an <see cref="IValidator{T}" /> as a
/// <see cref="Option{T}" />.
/// </summary>
/// <typeparam name="T"></typeparam>
public static class ValidationOption<T> {
    /// <summary>
    /// Converts an input value to an <see cref="Option{T}" /> based on the
    /// result of the provided validator. If the input is valid, then Some else
    /// None.
    /// </summary>
    /// <typeparam name="TValidator"></typeparam>
    /// <param name="input"></param>
    /// <returns></returns>
    public static Option<T> From<TValidator>(T input)
        where TValidator : IValidator<T>, new() {
        var validator = new TValidator();
        var validationOption = validator.Validate(input);
        if (validationOption.IsValid) {
            return Option<T>.Some(input);
        }
        else {
            return Option<T>.NoneValue;
        }
    }
}
