using Danom;
using Danom.Validation;

public static class Program {
    public static void Main() {
        ValidationResult();
        ValidationOption();
    }

    static void ValidationResult() {
        //
        // Fluent syntax

        // Valid case
        Validate<Author>
            .Using<AuthorValidator>(new("John", "Doe", Option.Some("john@doe.com")))
            .Match(
                ok: author => Console.WriteLine($"Author: {author.FirstName} {author.LastName}"),
                error: errors => Console.WriteLine($"Errors: {errors}"));

        // Invalid case
        Validate<Author>
            .Using<AuthorValidator>(new("", "", Option.Some("")))
            .Match(
                ok: author => Console.WriteLine($"Author: {author.FirstName} {author.LastName}"),
                error: errors => Console.WriteLine($"Errors: {errors}"));

        //
        // Procedural syntax

        //
        // Procedural syntax

        var authorCheck =
            Validate<Author>
                .Using<AuthorValidator>(
                    new("John", "Doe", Option.Some("john@doe.com")));

        if (authorCheck.TryGet(out var author)) {
            Console.WriteLine($"Author: {author.FirstName} {author.LastName}");
        }
        else if (authorCheck.TryGetError(out var authorError)) {
            // This will not be reached
            Console.WriteLine($"Errors: {authorError}"); // Invalid Case
        }
    }

    static void ValidationOption() {
        //
        // Fluent syntax

        // Valid case
        Validate<Author>
            .Using<AuthorValidator>(new("John", "Doe", Option.Some("john@doe.com")))
            .ToOption()
            .Match(
                some: author => Console.WriteLine($"Author: {author.FirstName} {author.LastName}"),
                none: () => Console.WriteLine("Invalid author"));

        // Invalid case
        Validate<Author>
            .Using<AuthorValidator>(new("", "", Option.Some("")))
            .ToOption()
            .Match(
                some: author => Console.WriteLine($"Author: {author.FirstName} {author.LastName}"),
                none: () => Console.WriteLine("Invalid author"));

        //
        // Procedural syntax

        var authorCheck =
            Validate<Author>
                .Using<AuthorValidator>(new("John", "Doe", Option.Some("john@doe.com")))
                .ToOption();

        if (authorCheck.TryGet(out var author)) {
            Console.WriteLine($"Author: {author.FirstName} {author.LastName}");
        }
        else {
            // This will not be reached
            Console.WriteLine("Invalid author"); // Invalid Case
        }
    }
}

public record Author(
    string FirstName,
    string LastName,
    Option<string> Email);

public sealed class AuthorValidator : BaseValidator<Author> {
    public AuthorValidator() {
        Rule("First Name", x => x.FirstName, [
            Check.String.IsNotEmpty,
            Check.String.IsLengthBetween(1, 32) ]);

        Rule("Last Name", x => x.LastName, [
            Check.String.IsNotEmpty,
            Check.String.IsLengthBetween(1, 32) ]);

        Rule("Email", x => x.Email, Check.Optional([
            Check.String.IsNotEmpty,
            Check.String.IsLengthBetween(5, 128)]));
    }
}
