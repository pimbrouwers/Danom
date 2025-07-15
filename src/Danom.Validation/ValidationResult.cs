namespace Danom.Validation
{
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
}
