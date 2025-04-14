using FluentValidation;
using Danom;
using Danom.Validation;

public static class Program
{
    public static void Main()
    {
        ValidationResult();
        ValidationOption();
    }

    static void ValidationResult()
    {
        //
        // Fluent syntax

        // Valid case
        ValidationResult<Author>
            .From<AuthorValidator>(new("John", "Doe", Option.Some("john@doe.com")))
            .Match(
                ok: author => Console.WriteLine($"Author: {author.FirstName} {author.LastName}"),
                error: errors => Console.WriteLine($"Errors: {errors}"));

        // Invalid case
        ValidationResult<Author>
            .From<AuthorValidator>(new("", "", Option.Some("")))
            .Match(
                ok: author => Console.WriteLine($"Author: {author.FirstName} {author.LastName}"),
                error: errors => Console.WriteLine($"Errors: {errors}"));

        //
        // Procedural syntax

        //
        // Procedural syntax

        var authorCheck =
            ValidationResult<Author>
                .From<AuthorValidator>(
                    new("John", "Doe", Option.Some("john@doe.com")));

        if (authorCheck.TryGet(out var author))
        {
            Console.WriteLine($"Author: {author.FirstName} {author.LastName}");
        }
        else if(authorCheck.TryGetError(out var authorError))
        {
            // This will not be reached
            Console.WriteLine($"Errors: {authorError}"); // Invalid Case
        }
    }

    static void ValidationOption()
    {
        //
        // Fluent syntax

        // Valid case
        ValidationOption<Author>
            .From<AuthorValidator>(new("John", "Doe", Option.Some("john@doe.com")))
            .Match(
                some: author => Console.WriteLine($"Author: {author.FirstName} {author.LastName}"),
                none: () => Console.WriteLine("Invalid author"));

        // Invalid case
        ValidationOption<Author>
            .From<AuthorValidator>(new("", "", Option.Some("")))
            .Match(
                some: author => Console.WriteLine($"Author: {author.FirstName} {author.LastName}"),
                none: () => Console.WriteLine("Invalid author"));

        //
        // Procedural syntax

        var authorCheck =
            ValidationOption<Author>
                .From<AuthorValidator>(
                    new("John", "Doe", Option.Some("john@doe.com")));

        if (authorCheck.TryGet(out var author))
        {
            Console.WriteLine($"Author: {author.FirstName} {author.LastName}");
        }
        else
        {
            // This will not be reached
            Console.WriteLine("Invalid author"); // Invalid Case
        }
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
