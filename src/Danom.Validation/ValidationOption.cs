namespace Danom.Validation
{
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
            where TValidator : IValidator<T>, new()
        {
            var validator = new TValidator();
            return validator.Validate(input).Match(
                ok: input => Option.Some(input),
                error: _ => Option<T>.NoneValue);
        }
    }
}
