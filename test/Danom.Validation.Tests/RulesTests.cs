namespace Danom.Validation.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class RulesTests
{
    private static Result<Unit, ResultErrors> RunRule(LabeledValidatorRule rule) => rule("field");

    public sealed class EquatableRulesTests
    {
        [Fact]
        public void EqualTo_ReturnsOk()
        {
            AssertResult.IsOk(RunRule("test".IsEqualTo("test")));
            AssertResult.IsOk(RunRule('c'.IsEqualTo('c')));

            AssertResult.IsOk(RunRule(byte.MaxValue.IsEqualTo(byte.MaxValue)));
            AssertResult.IsOk(RunRule(short.MaxValue.IsEqualTo(short.MaxValue)));
            AssertResult.IsOk(RunRule(int.MaxValue.IsEqualTo(int.MaxValue)));
            AssertResult.IsOk(RunRule(long.MaxValue.IsEqualTo(long.MaxValue)));
            AssertResult.IsOk(RunRule(3.14.IsEqualTo(3.14)));
            AssertResult.IsOk(RunRule(3.14f.IsEqualTo(3.14f)));
            AssertResult.IsOk(RunRule(3.14m.IsEqualTo(3.14m)));

            var guid = Guid.NewGuid();
            AssertResult.IsOk(RunRule(guid.IsEqualTo(guid)));
            AssertResult.IsOk(RunRule(Guid.Empty.IsEqualTo(Guid.Empty)));

            var datetime = new DateTime(1986, 12, 12);
            AssertResult.IsOk(RunRule(datetime.IsEqualTo(datetime)));

            var dateonly = new DateOnly(1986, 12, 12);
            AssertResult.IsOk(RunRule(dateonly.IsEqualTo(dateonly)));

            var timeonly = new TimeOnly(12, 34, 56);
            AssertResult.IsOk(RunRule(timeonly.IsEqualTo(timeonly)));

            var timespan = TimeSpan.FromHours(1);
            AssertResult.IsOk(RunRule(timespan.IsEqualTo(timespan)));

            AssertResult.IsOk(RunRule(Option.Some(5).IsEqualTo(Option.Some(5))));
        }

        [Fact]
        public void EqualTo_ReturnsError()
        {
            AssertResult.IsError(RunRule("test".IsEqualTo("example")));
            AssertResult.IsError(RunRule('c'.IsEqualTo('d')));

            AssertResult.IsError(RunRule(byte.MaxValue.IsEqualTo(byte.MinValue)));
            AssertResult.IsError(RunRule(short.MaxValue.IsEqualTo(short.MinValue)));
            AssertResult.IsError(RunRule(int.MaxValue.IsEqualTo(int.MinValue)));
            AssertResult.IsError(RunRule(long.MaxValue.IsEqualTo(long.MinValue)));
            AssertResult.IsError(RunRule(3.14.IsEqualTo(double.MinValue)));
            AssertResult.IsError(RunRule(3.14f.IsEqualTo(float.MinValue)));
            AssertResult.IsError(RunRule(3.14m.IsEqualTo(decimal.MinValue)));

            var guid = Guid.NewGuid();
            AssertResult.IsError(RunRule(guid.IsEqualTo(Guid.Empty)));
            AssertResult.IsError(RunRule(Guid.Empty.IsEqualTo(guid)));

            var datetime = new DateTime(1986, 12, 12);
            AssertResult.IsError(RunRule(datetime.IsEqualTo(DateTime.MinValue)));

            var dateonly = new DateOnly(1986, 12, 12);
            AssertResult.IsError(RunRule(dateonly.IsEqualTo(DateOnly.MinValue)));

            var timeonly = new TimeOnly(12, 34, 56);
            AssertResult.IsError(RunRule(timeonly.IsEqualTo(TimeOnly.MinValue)));

            var timespan = TimeSpan.FromHours(1);
            AssertResult.IsError(RunRule(timespan.IsEqualTo(TimeSpan.FromHours(2))));

            AssertResult.IsError(RunRule(Option.Some(5).IsEqualTo(Option.Some(10))));
        }

        [Fact]
        public void NotEqualTo_ReturnsOk()
        {
            AssertResult.IsOk(RunRule(3.IsNotEqualTo(5)));
            AssertResult.IsOk(RunRule("test".IsNotEqualTo("example")));
        }

        [Fact]
        public void NotEqualTo_ReturnsError()
        {
            AssertResult.IsError(RunRule(5.IsNotEqualTo(5)));
        }
    }

    public sealed class ComparableRulesTests
    {
        [Fact]
        public void Between_ReturnsOk_WhenValueIsBetweenMinAndMax()
        {
            AssertResult.IsOk(RunRule(5.IsBetween(1, 10)));
            AssertResult.IsOk(RunRule(3.5.IsBetween(1.0, 5.0)));
        }

        [Fact]
        public void Between_ReturnsError_WhenValueIsNotBetweenMinAndMax()
        {
            AssertResult.IsError(RunRule(0.IsBetween(1, 10)));
            AssertResult.IsError(RunRule(11.IsBetween(1, 10)));
            AssertResult.IsError(RunRule(5.IsBetween(6, 10)));
        }

        [Fact]
        public void GreaterThan_ReturnsOk_WhenValueIsGreaterThanThreshold()
        {
            AssertResult.IsOk(RunRule(10.IsGreaterThan(5)));
            AssertResult.IsOk(RunRule(5.5.IsGreaterThan(5.0)));
        }

        [Fact]
        public void GreaterThan_ReturnsError_WhenValueIsNotGreaterThanThreshold()
        {
            AssertResult.IsError(RunRule(3.IsGreaterThan(5)));
            AssertResult.IsError(RunRule(5.IsGreaterThan(5)));
        }

        [Fact]
        public void GreaterThanOrEqualTo_ReturnsOk_WhenValueIsGreaterThanOrEqualToThreshold()
        {
            AssertResult.IsOk(RunRule(5.IsGreaterThanOrEqualTo(5)));
            AssertResult.IsOk(RunRule(6.IsGreaterThanOrEqualTo(5)));
        }

        [Fact]
        public void GreaterThanOrEqualTo_ReturnsError_WhenValueIsLessThanThreshold()
        {
            AssertResult.IsError(RunRule(4.IsGreaterThanOrEqualTo(5)));
            AssertResult.IsError(RunRule(3.9.IsGreaterThanOrEqualTo(4.0)));
        }

        [Fact]
        public void LessThan_ReturnsOk_WhenValueIsLessThanThreshold()
        {
            AssertResult.IsOk(RunRule(3.IsLessThan(5)));
            AssertResult.IsOk(RunRule(4.9.IsLessThan(5.0)));
        }

        [Fact]
        public void LessThan_ReturnsError_WhenValueIsNotLessThanThreshold()
        {
            AssertResult.IsError(RunRule(6.IsLessThan(5)));
            AssertResult.IsError(RunRule(5.IsLessThan(5)));
        }

        [Fact]
        public void LessThanOrEqualTo_ReturnsOk_WhenValueIsLessThanOrEqualTo()
        {
            AssertResult.IsOk(RunRule(5.IsLessThanOrEqualTo(5)));
            AssertResult.IsOk(RunRule(4.IsLessThanOrEqualTo(5)));
        }

        [Fact]
        public void LessThanOrEqualTo_ReturnsError_WhenValueIsLessThanOrEqualTo()
        {
            AssertResult.IsError(RunRule(6.IsLessThanOrEqualTo(5)));
            AssertResult.IsError(RunRule(5.1.IsLessThanOrEqualTo(5.0)));
        }
    }

    public sealed class StringRulesTests
    {
        [Fact]
        public void StringEmpty_ReturnsOk_WhenStringIsEmpty()
        {
            AssertResult.IsOk(RunRule(string.Empty.IsEmpty()));
            AssertResult.IsOk(RunRule("".IsEmpty()));
            AssertResult.IsOk(RunRule(" ".IsEmpty()));
        }

        [Fact]
        public void StringEmpty_ReturnsError_WhenStringIsNotEmpty()
        {
            AssertResult.IsError(RunRule("test".IsEmpty()));
        }

        [Fact]
        public void StringNotEmpty_ReturnsOk_WhenStringIsNotEmpty()
        {
            AssertResult.IsOk(RunRule("test".IsNotEmpty()));
        }

        [Fact]
        public void StringNotEmpty_ReturnsError_WhenStringIsEmpty()
        {
            AssertResult.IsError(RunRule(string.Empty.IsNotEmpty()));
            AssertResult.IsError(RunRule("".IsNotEmpty()));
            AssertResult.IsError(RunRule(" ".IsNotEmpty()));
        }
    }

    public sealed class GuidRulesTests
    {
        [Fact]
        public void GuidEmpty_ReturnsOk_WhenGuidIsEmpty()
        {
            AssertResult.IsOk(RunRule(Guid.Empty.IsEmpty()));
        }

        [Fact]
        public void GuidEmpty_ReturnsError_WhenGuidIsNotEmpty()
        {
            AssertResult.IsError(RunRule(Guid.NewGuid().IsEmpty()));
        }

        [Fact]
        public void GuidNotEmpty_ReturnsOk_WhenGuidIsNotEmpty()
        {
            AssertResult.IsOk(RunRule(Guid.NewGuid().IsNotEmpty()));
        }

        [Fact]
        public void GuidNotEmpty_ReturnsError_WhenGuidIsEmpty()
        {
            AssertResult.IsError(RunRule(Guid.Empty.IsNotEmpty()));
        }
    }

    public sealed class OptionRulesTests
    {
        [Fact]
        public void OptionIsSome_ReturnsOk_WhenOptionHasValue()
        {
            AssertResult.IsOk(RunRule(Option.Some(5).Required()));
            AssertResult.IsOk(RunRule(Option.Some("test").Required()));
        }

        [Fact]
        public void OptionIsSome_ReturnsError_WhenOptionIsNone()
        {
            AssertResult.IsError(RunRule(Option<int>.None().Required()));
            AssertResult.IsError(RunRule(Option<string>.None().Required()));
        }

        [Fact]
        public void OptionIsNone_ReturnsOk_WhenOptionIsNone()
        {
            AssertResult.IsOk(RunRule(Option<int>.None().Optional(x => x.IsGreaterThan(0))));
            AssertResult.IsOk(RunRule(Option<string>.None().Optional(x => x.IsNotEmpty())));
        }

        [Fact]
        public void OptionIsNone_ReturnsError_WhenOptionHasValue()
        {
            AssertResult.IsError(RunRule(Option.Some(-1).Optional(x => x.IsGreaterThan(0))));
            AssertResult.IsError(RunRule(Option.Some("").Optional(x => x.IsNotEmpty())));
        }
    }
}
