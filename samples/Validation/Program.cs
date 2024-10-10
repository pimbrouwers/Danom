using FluentValidation;
using Danom;
using Danom.Validation;

public static class Program
{
    public static void Main()
    {
        //
        // Valid case
        ValidationResult<Author>
            .From<AuthorValidator>(new("John", "Doe", Option.Some("john@doe.com")))
            .Match(
                ok: author => Console.WriteLine($"Author: {author.FirstName} {author.LastName}"),
                error: errors => Console.WriteLine($"Errors: {errors}"));

        //
        // Invalid case
        ValidationResult<Author>
            .From<AuthorValidator>(new("", "", Option.Some("")))
            .Match(
                ok: author => Console.WriteLine($"Author: {author.FirstName} {author.LastName}"),
                error: errors => Console.WriteLine($"Errors: {errors}"));
    }
}

public record Author(
    string FirstName,
    string LastName,
    Option<string> Email);

public sealed class AuthorValidator
    : AbstractValidator<Author>
{
    public AuthorValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(32);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(32);
        RuleFor(x => x.Email).Optional(x => x.NotEmpty().EmailAddress());
    }
}
