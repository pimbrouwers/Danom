namespace Danom.Validation.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class RulesTests
{
    private static Result<Unit, ResultErrors> RunRule<T>(T input, ValidatorRule<T> rule) =>
        rule(input)("field").TryGetError(out var errors)
            ? Result.Error(errors)
            : Result.Ok();

    public sealed class EquatableRulesTests
    {
        [Fact]
        public void EqualTo_ReturnsOk()
        {
            Assert.True(RunRule('c', Check.IsEqualTo('c')).IsOk);
            Assert.True(RunRule('c', Check.IsNotEqualTo('d')).IsOk);

            Assert.True(RunRule("test", Check.IsEqualTo("test")).IsOk);
            Assert.True(RunRule("test", Check.IsNotEqualTo("test")).IsError);
            Assert.True(RunRule("test", Check.IsNotEqualTo("best")).IsOk);
            Assert.True(RunRule("test", Check.IsEqualTo("best")).IsError);

            Assert.True(RunRule(byte.MaxValue, Check.IsEqualTo(byte.MaxValue)).IsOk);
            Assert.True(RunRule(short.MaxValue, Check.IsEqualTo(short.MaxValue)).IsOk);
            Assert.True(RunRule(int.MaxValue, Check.IsEqualTo(int.MaxValue)).IsOk);
            Assert.True(RunRule(long.MaxValue, Check.IsEqualTo(long.MaxValue)).IsOk);
            Assert.True(RunRule(3.14, Check.IsEqualTo(3.14)).IsOk);
            Assert.True(RunRule(3.14f, Check.IsEqualTo(3.14f)).IsOk);
            Assert.True(RunRule(3.14m, Check.IsEqualTo(3.14m)).IsOk);

            var guid = Guid.NewGuid();
            Assert.True(RunRule(guid, Check.IsEqualTo(guid)).IsOk);
            Assert.True(RunRule(Guid.Empty, Check.IsEqualTo(Guid.Empty)).IsOk);

            var datetime = new DateTime(1986, 12, 12);
            Assert.True(RunRule(datetime, Check.IsEqualTo(datetime)).IsOk);

            var dateonly = new DateOnly(1986, 12, 12);
            Assert.True(RunRule(dateonly, Check.IsEqualTo(dateonly)).IsOk);

            var timeonly = new TimeOnly(12, 34, 56);
            Assert.True(RunRule(timeonly, Check.IsEqualTo(timeonly)).IsOk);

            var timespan = TimeSpan.FromHours(1);
            Assert.True(RunRule(timespan, Check.IsEqualTo(timespan)).IsOk);

            Assert.True(RunRule(Option.Some(5), Check.IsEqualTo(Option.Some(5))).IsOk);
        }

        [Fact]
        public void EqualTo_ReturnsError()
        {
            Assert.True(RunRule("test", Check.IsEqualTo("example")).IsError);
            Assert.True(RunRule('c', Check.IsEqualTo('d')).IsError);

            Assert.True(RunRule(byte.MaxValue, Check.IsEqualTo(byte.MinValue)).IsError);
            Assert.True(RunRule(short.MaxValue, Check.IsEqualTo(short.MinValue)).IsError);
            Assert.True(RunRule(int.MaxValue, Check.IsEqualTo(int.MinValue)).IsError);
            Assert.True(RunRule(long.MaxValue, Check.IsEqualTo(long.MinValue)).IsError);
            Assert.True(RunRule(3.14, Check.IsEqualTo(double.MinValue)).IsError);
            Assert.True(RunRule(3.14f, Check.IsEqualTo(float.MinValue)).IsError);
            Assert.True(RunRule(3.14m, Check.IsEqualTo(decimal.MinValue)).IsError);

            var guid = Guid.NewGuid();
            Assert.True(RunRule(guid, Check.IsEqualTo(Guid.Empty)).IsError);
            Assert.True(RunRule(Guid.Empty, Check.IsEqualTo(guid)).IsError);

            var datetime = new DateTime(1986, 12, 12);
            Assert.True(RunRule(datetime, Check.IsEqualTo(DateTime.MinValue)).IsError);

            var dateonly = new DateOnly(1986, 12, 12);
            Assert.True(RunRule(dateonly, Check.IsEqualTo(DateOnly.MinValue)).IsError);

            var timeonly = new TimeOnly(12, 34, 56);
            Assert.True(RunRule(timeonly, Check.IsEqualTo(TimeOnly.MinValue)).IsError);

            var timespan = TimeSpan.FromHours(1);
            Assert.True(RunRule(timespan, Check.IsEqualTo(TimeSpan.FromHours(2))).IsError);

            Assert.True(RunRule(Option.Some(5), Check.IsEqualTo(Option.Some(10))).IsError);
        }

        [Fact]
        public void NotEqualTo_ReturnsOk()
        {
            Assert.True(RunRule(3, Check.IsNotEqualTo(5)).IsOk);
            Assert.True(RunRule("test", Check.IsNotEqualTo("example")).IsOk);
        }

        [Fact]
        public void NotEqualTo_ReturnsError()
        {
            Assert.True(RunRule(5, Check.IsNotEqualTo(5)).IsError);
        }
    }

    public sealed class ComparableRulesTests
    {
        [Fact]
        public void Between_ReturnsOk_WhenValueIsBetweenMinAndMax()
        {
            Assert.True(RunRule(5, Check.IsBetween(1, 10)).IsOk);
            Assert.True(RunRule(3.5, Check.IsBetween(1.0, 5.0)).IsOk);
        }

        [Fact]
        public void Between_ReturnsError_WhenValueIsNotBetweenMinAndMax()
        {
            Assert.True(RunRule(0, Check.IsBetween(1, 10)).IsError);
            Assert.True(RunRule(11, Check.IsBetween(1, 10)).IsError);
            Assert.True(RunRule(5, Check.IsBetween(6, 10)).IsError);
        }

        [Fact]
        public void GreaterThan_ReturnsOk_WhenValueIsGreaterThanThreshold()
        {
            Assert.True(RunRule(10, Check.IsGreaterThan(5)).IsOk);
            Assert.True(RunRule(5.5, Check.IsGreaterThan(5.0)).IsOk);
        }

        [Fact]
        public void GreaterThan_ReturnsError_WhenValueIsNotGreaterThanThreshold()
        {
            Assert.True(RunRule(3, Check.IsGreaterThan(5)).IsError);
            Assert.True(RunRule(5, Check.IsGreaterThan(5)).IsError);
        }

        [Fact]
        public void GreaterThanOrEqualTo_ReturnsOk_WhenValueIsGreaterThanOrEqualToThreshold()
        {
            Assert.True(RunRule(5, Check.IsGreaterThanOrEqualTo(5)).IsOk);
            Assert.True(RunRule(6, Check.IsGreaterThanOrEqualTo(5)).IsOk);
        }

        [Fact]
        public void GreaterThanOrEqualTo_ReturnsError_WhenValueIsLessThanThreshold()
        {
            Assert.True(RunRule(4, Check.IsGreaterThanOrEqualTo(5)).IsError);
            Assert.True(RunRule(3.9, Check.IsGreaterThanOrEqualTo(4.0)).IsError);
        }

        [Fact]
        public void LessThan_ReturnsOk_WhenValueIsLessThanThreshold()
        {
            Assert.True(RunRule(3, Check.IsLessThan(5)).IsOk);
            Assert.True(RunRule(4.9, Check.IsLessThan(5.0)).IsOk);
        }

        [Fact]
        public void LessThan_ReturnsError_WhenValueIsNotLessThanThreshold()
        {
            Assert.True(RunRule(6, Check.IsLessThan(5)).IsError);
            Assert.True(RunRule(5, Check.IsLessThan(5)).IsError);
        }

        [Fact]
        public void LessThanOrEqualTo_ReturnsOk_WhenValueIsLessThanOrEqualTo()
        {
            Assert.True(RunRule(5, Check.IsLessThanOrEqualTo(5)).IsOk);
            Assert.True(RunRule(4, Check.IsLessThanOrEqualTo(5)).IsOk);
        }

        [Fact]
        public void LessThanOrEqualTo_ReturnsError_WhenValueIsLessThanOrEqualTo()
        {
            Assert.True(RunRule(6, Check.IsLessThanOrEqualTo(5)).IsError);
            Assert.True(RunRule(5.1, Check.IsLessThanOrEqualTo(5.0)).IsError);
        }
    }

    public sealed class StringRulesTests
    {
        [Fact]
        public void StringEmpty_ReturnsOk_WhenStringIsEmpty()
        {
            Assert.True(RunRule(string.Empty, Check.String.IsEmpty).IsOk);
            Assert.True(RunRule("", Check.String.IsEmpty).IsOk);
            Assert.True(RunRule(" ", Check.String.IsEmpty).IsOk);
        }

        [Fact]
        public void StringEmpty_ReturnsError_WhenStringIsNotEmpty()
        {
            Assert.True(RunRule("test", Check.String.IsEmpty).IsError);
        }

        [Fact]
        public void StringNotEmpty_ReturnsOk_WhenStringIsNotEmpty()
        {
            Assert.True(RunRule("test", Check.String.IsNotEmpty).IsOk);
        }

        [Fact]
        public void StringNotEmpty_ReturnsError_WhenStringIsEmpty()
        {
            Assert.True(RunRule(string.Empty, Check.String.IsNotEmpty).IsError);
            Assert.True(RunRule("", Check.String.IsNotEmpty).IsError);
            Assert.True(RunRule(" ", Check.String.IsNotEmpty).IsError);
        }

        [Fact]
        public void TypedString_ReturnsOk()
        {
            Assert.True(RunRule("jim@bob.com", Check.String.IsEmailAddress).IsOk);
            Assert.True(RunRule("http://example.com", Check.String.IsUrl).IsOk);
            Assert.True(RunRule("https://example.com", Check.String.IsUrl).IsOk);
            Assert.True(RunRule("ftp://example.com", Check.String.IsUrl).IsOk);
            Assert.True(RunRule("+1234567890", Check.String.IsE164).IsOk);
        }

        [Fact]
        public void TypedString_ReturnsError()
        {
            Assert.True(RunRule("invalid-email", Check.String.IsEmailAddress).IsError);
            Assert.True(RunRule("invalid-url", Check.String.IsUrl).IsError);
            Assert.True(RunRule("1", Check.String.IsE164).IsError);
        }
    }

    public sealed class GuidRulesTests
    {
        [Fact]
        public void GuidEmpty_ReturnsOk_WhenGuidIsEmpty()
        {
            Assert.True(RunRule(Guid.Empty, Check.Guid.IsEmpty).IsOk);
        }

        [Fact]
        public void GuidEmpty_ReturnsError_WhenGuidIsNotEmpty()
        {
            Assert.True(RunRule(Guid.NewGuid(), Check.Guid.IsEmpty).IsError);
        }

        [Fact]
        public void GuidNotEmpty_ReturnsOk_WhenGuidIsNotEmpty()
        {
            Assert.True(RunRule(Guid.NewGuid(), Check.Guid.IsNotEmpty).IsOk);
        }

        [Fact]
        public void GuidNotEmpty_ReturnsError_WhenGuidIsEmpty()
        {
            Assert.True(RunRule(Guid.Empty, Check.Guid.IsNotEmpty).IsError);
        }
    }

    public sealed class OptionRulesTests
    {
        [Fact]
        public void OptionIsSome_ReturnsOk_WhenOptionHasValue()
        {
            Assert.True(RunRule(Option.Some(5), Check.Required<int>()).IsOk);
            Assert.True(RunRule(Option.Some("test"), Check.Required<string>()).IsOk);
        }

        [Fact]
        public void OptionIsSome_ReturnsError_WhenOptionIsNone()
        {
            Assert.True(RunRule(Option<int>.None(), Check.Required<int>()).IsError);
            Assert.True(RunRule(Option<string>.None(), Check.Required<string>()).IsError);
        }

        [Fact]
        public void OptionIsNone_ReturnsOk_WhenOptionIsNone()
        {
            Assert.True(RunRule(Option<int>.None(), Check.Optional(Check.IsGreaterThan(0))).IsOk);
            Assert.True(RunRule(Option<string>.None(), Check.Optional(Check.String.IsNotEmpty)).IsOk);
        }

        [Fact]
        public void OptionIsNone_ReturnsError_WhenOptionHasValue()
        {
            Assert.True(RunRule(Option.Some(-1), Check.Optional(Check.IsGreaterThan(0))).IsError);
            Assert.True(RunRule(Option.Some(""), Check.Optional(Check.String.IsNotEmpty)).IsError);
        }
    }
}
