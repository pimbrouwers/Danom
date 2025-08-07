namespace Danom.Validation.Tests;

using Xunit;

public sealed class ReadmeExampleTest
{
    [Fact]
    public void DoTheTest()
    {
        var validator = new AttendeeValidator();

        Validate<Attendee>
            .Using<AttendeeValidator>(new(
                Name: "John Doe",
                Age: 30,
                Phone: "+14055551234",
                Email: Option<string>.Some("john@doe.com"),
                AlternateEmail: Option<string>.None(),
                Interests: ["C#", "ASP.NET"]))
            .Match(
                x => Assert.Equal("John Doe", x.Name),
                e => Assert.Fail("Input is valid, but validation failed"));

        Validate<Attendee>
            .Using<AttendeeValidator>(new(
                Name: "John Doe",
                Age: 30,
                Phone: "+14055551234",
                Email: Option<string>.Some("john@doe.com"),
                AlternateEmail: Option<string>.None(),
                Interests: []))
            .ToOption()
            .Match(
                x => Assert.Equal("John Doe", x.Name),
                () => Assert.Fail("Input is valid, but validation failed"));

        Validate<Attendee>
            .Using<AttendeeValidator>(new(
                Name: "",
                Age: -1,
                Phone: "123",
                Email: Option<string>.NoneValue,
                AlternateEmail: Option<string>.Some("invalid_email"),
                Interests: ["a"]))
            .Match(
                x => Assert.Fail("Input is invalid, but validation succeeded"),
                e => Assert.True(e.Any()));
    }

    public record Attendee(
    string Name,
    int Age,
    string Phone,
    Option<string> Email,
    Option<string> AlternateEmail,
    IEnumerable<string> Interests);

    public sealed class AttendeeValidator : BaseValidator<Attendee>
    {
        public AttendeeValidator()
        {
            Rule("Name", x => x.Name,
                Check.String.IsNotEmpty);

            Rule("Age", x => x.Age,
                Check.IsGreaterThan(0));

            Rule("Phone", x => x.Phone,
                Check.String.IsE164);

            Rule("Email", x => x.Email,
                Check.Required(Check.String.IsEmailAddress));

            Rule("AlternateEmail", x => x.AlternateEmail,
                Check.Optional(Check.String.IsEmailAddress));

            Rule("Interests", x => x.Interests,
                Check.Enumerable.ForEach(Check.String.IsLengthOrGreaterThan(2)));
        }
    }
}
