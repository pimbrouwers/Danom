namespace Danom.Validation;

using FluentValidation;

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
        var validationResult = validator.Validate(input);
        if (validationResult.IsValid)
        {
            return Result<T, ResultErrors>.Ok(input);
        }
        else
        {
            return Result<T, ResultErrors>.Error(
                new ResultErrors(
                    validationResult.Errors
                        .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                        .Select(x => new ResultError(x.Key, x.AsEnumerable()))));
        }
    }
}
