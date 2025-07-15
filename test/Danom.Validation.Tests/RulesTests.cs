namespace Danom.Validation.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class EquatableRulesTests
{
    [Fact]
    public void EqualTo_ReturnsOk()
    {
        AssertResult.IsOk("test".IsEqualTo("test"));
        AssertResult.IsOk('c'.IsEqualTo('c'));

        AssertResult.IsOk(byte.MaxValue.IsEqualTo(byte.MaxValue));
        AssertResult.IsOk(short.MaxValue.IsEqualTo(short.MaxValue));
        AssertResult.IsOk(int.MaxValue.IsEqualTo(int.MaxValue));
        AssertResult.IsOk(long.MaxValue.IsEqualTo(long.MaxValue));
        AssertResult.IsOk(3.14.IsEqualTo(3.14));
        AssertResult.IsOk(3.14f.IsEqualTo(3.14f));
        AssertResult.IsOk(3.14m.IsEqualTo(3.14m));

        var guid = Guid.NewGuid();
        AssertResult.IsOk(guid.IsEqualTo(guid));
        AssertResult.IsOk(Guid.Empty.IsEqualTo(Guid.Empty));

        var datetime = new DateTime(1986, 12, 12);
        AssertResult.IsOk(datetime.IsEqualTo(datetime));

        var dateonly = new DateOnly(1986, 12, 12);
        AssertResult.IsOk(dateonly.IsEqualTo(dateonly));

        var timeonly = new TimeOnly(12, 34, 56);
        AssertResult.IsOk(timeonly.IsEqualTo(timeonly));

        var timespan = TimeSpan.FromHours(1);
        AssertResult.IsOk(timespan.IsEqualTo(timespan));

        AssertResult.IsOk(Option.Some(5).IsEqualTo(Option.Some(5)));
    }

    [Fact]
    public void EqualTo_ReturnsError()
    {
        AssertResult.IsError("test".IsEqualTo("example"));
        AssertResult.IsError('c'.IsEqualTo('d'));

        AssertResult.IsError(byte.MaxValue.IsEqualTo(byte.MinValue));
        AssertResult.IsError(short.MaxValue.IsEqualTo(short.MinValue));
        AssertResult.IsError(int.MaxValue.IsEqualTo(int.MinValue));
        AssertResult.IsError(long.MaxValue.IsEqualTo(long.MinValue));
        AssertResult.IsError(3.14.IsEqualTo(double.MinValue));
        AssertResult.IsError(3.14f.IsEqualTo(float.MinValue));
        AssertResult.IsError(3.14m.IsEqualTo(decimal.MinValue));

        var guid = Guid.NewGuid();
        AssertResult.IsError(guid.IsEqualTo(Guid.Empty));
        AssertResult.IsError(Guid.Empty.IsEqualTo(guid));

        var datetime = new DateTime(1986, 12, 12);
        AssertResult.IsError(datetime.IsEqualTo(DateTime.MinValue));

        var dateonly = new DateOnly(1986, 12, 12);
        AssertResult.IsError(dateonly.IsEqualTo(DateOnly.MinValue));

        var timeonly = new TimeOnly(12, 34, 56);
        AssertResult.IsError(timeonly.IsEqualTo(TimeOnly.MinValue));

        var timespan = TimeSpan.FromHours(1);
        AssertResult.IsError(timespan.IsEqualTo(TimeSpan.FromHours(2)));

        AssertResult.IsError(Option.Some(5).IsEqualTo(Option.Some(10)));
    }

    [Fact]
    public void EqualTo_ReturnsError_CustomFieldName()
    {
        var field = "My Number";
        AssertResult.IsErrorSingle(3.IsEqualTo(5, field), m => m.StartsWith($"'{field}'"));
    }

    [Fact]
    public void EqualTo_ReturnsError_CustomMessage()
    {
        var message = "A custom error message";
        AssertResult.IsErrorSingle(3.IsEqualTo(5, message: message), m => m == message);
    }

    [Fact]
    public void NotEqualTo_ReturnsOk()
    {
        AssertResult.IsOk(3.IsNotEqualTo(5));
        AssertResult.IsOk("test".IsNotEqualTo("example"));
    }

    [Fact]
    public void NotEqualTo_ReturnsError()
    {
        AssertResult.IsError(5.IsNotEqualTo(5));

        var field = "My Number";
        AssertResult.IsErrorSingle(5.IsNotEqualTo(5, field), m => m.StartsWith($"'{field}'"));

        var message = "A custom error message";
        AssertResult.IsErrorSingle(5.IsNotEqualTo(5, message: message), m => m == message);
    }
}

public sealed class ComparableRulesTests
{
    [Fact]
    public void Between_ReturnsOk_WhenValueIsBetweenMinAndMax()
    {
        AssertResult.IsOk(5.IsBetween(1, 10));
        AssertResult.IsOk(3.5.IsBetween(1.0, 5.0));
    }

    [Fact]
    public void Between_ReturnsError_WhenValueIsNotBetweenMinAndMax()
    {
        AssertResult.IsError(0.IsBetween(1, 10));
        AssertResult.IsError(11.IsBetween(1, 10));
        AssertResult.IsError(5.IsBetween(6, 10));

        var field = "My Number";
        AssertResult.IsErrorSingle(0.IsBetween(1, 10, field), m => m.StartsWith($"'{field}'"));

        var message = "A custom error message";
        AssertResult.IsErrorSingle(0.IsBetween(1, 10, message: message), m => m == message);
    }

    [Fact]
    public void GreaterThan_ReturnsOk_WhenValueIsGreaterThanThreshold()
    {
        AssertResult.IsOk(10.IsGreaterThan(5));
        AssertResult.IsOk(5.5.IsGreaterThan(5.0));
    }

    [Fact]
    public void GreaterThan_ReturnsError_WhenValueIsNotGreaterThanThreshold()
    {
        AssertResult.IsError(3.IsGreaterThan(5));
        AssertResult.IsError(5.IsGreaterThan(5));
    }

    [Fact]
    public void GreaterThanOrEqualTo_ReturnsOk_WhenValueIsGreaterThanOrEqualToThreshold()
    {
        AssertResult.IsOk(5.IsGreaterThanOrEqualTo(5));
        AssertResult.IsOk(6.IsGreaterThanOrEqualTo(5));
    }

    [Fact]
    public void GreaterThanOrEqualTo_ReturnsError_WhenValueIsLessThanThreshold()
    {
        AssertResult.IsError(4.IsGreaterThanOrEqualTo(5));
        AssertResult.IsError(3.9.IsGreaterThanOrEqualTo(4.0));
    }

    [Fact]
    public void LessThan_ReturnsOk_WhenValueIsLessThanThreshold()
    {
        AssertResult.IsOk(3.IsLessThan(5));
        AssertResult.IsOk(4.9.IsLessThan(5.0));
    }

    [Fact]
    public void LessThan_ReturnsError_WhenValueIsNotLessThanThreshold()
    {
        AssertResult.IsError(6.IsLessThan(5));
        AssertResult.IsError(5.IsLessThan(5));
    }

    [Fact]
    public void LessThanOrEqualTo_ReturnsOk_WhenValueIsLessThanOrEqualTo()
    {
        AssertResult.IsOk(5.IsLessThanOrEqualTo(5));
        AssertResult.IsOk(4.IsLessThanOrEqualTo(5));
    }

    [Fact]
    public void LessThanOrEqualTo_ReturnsError_WhenValueIsLessThanOrEqualTo()
    {
        AssertResult.IsError(6.IsLessThanOrEqualTo(5));
        AssertResult.IsError(5.1.IsLessThanOrEqualTo(5.0));
    }
}

public sealed class StringRulesTests
{
    [Fact]
    public void StringEmpty_ReturnsOk_WhenStringIsEmpty()
    {
        AssertResult.IsOk(string.Empty.IsEmpty());
        AssertResult.IsOk("".IsEmpty());
        AssertResult.IsOk(" ".IsEmpty());
    }

    [Fact]
    public void StringEmpty_ReturnsError_WhenStringIsNotEmpty()
    {
        AssertResult.IsError("test".IsEmpty());
    }

    [Fact]
    public void StringNotEmpty_ReturnsOk_WhenStringIsNotEmpty()
    {
        AssertResult.IsOk("test".IsNotEmpty());
    }

    [Fact]
    public void StringNotEmpty_ReturnsError_WhenStringIsEmpty()
    {
        AssertResult.IsError(string.Empty.IsNotEmpty());
        AssertResult.IsError("".IsNotEmpty());
        AssertResult.IsError(" ".IsNotEmpty());
    }
}

public sealed class GuidRulesTests
{
    [Fact]
    public void GuidEmpty_ReturnsOk_WhenGuidIsEmpty()
    {
        AssertResult.IsOk(Guid.Empty.IsEmpty());
    }

    [Fact]
    public void GuidEmpty_ReturnsError_WhenGuidIsNotEmpty()
    {
        AssertResult.IsError(Guid.NewGuid().IsEmpty());
    }

    [Fact]
    public void GuidNotEmpty_ReturnsOk_WhenGuidIsNotEmpty()
    {
        AssertResult.IsOk(Guid.NewGuid().IsNotEmpty());
    }

    [Fact]
    public void GuidNotEmpty_ReturnsError_WhenGuidIsEmpty()
    {
        AssertResult.IsError(Guid.Empty.IsNotEmpty());
    }
}

public sealed class OptionRulesTests
{
    [Fact]
    public void OptionIsSome_ReturnsOk_WhenOptionHasValue()
    {
        AssertResult.IsOk(Option.Some(5).Required());
        AssertResult.IsOk(Option.Some("test").Required());
    }

    [Fact]
    public void OptionIsSome_ReturnsError_WhenOptionIsNone()
    {
        AssertResult.IsError(Option<int>.None().Required());
        AssertResult.IsError(Option<string>.None().Required());

        var field = "My Option";
        AssertResult.IsErrorSingle(Option<int>.None().Required(field), m => m.StartsWith($"'{field}'"));

        var message = "A custom error message";
        AssertResult.IsErrorSingle(Option<int>.None().Required(message: message), m => m == message);
    }

    [Fact]
    public void OptionIsNone_ReturnsOk_WhenOptionIsNone()
    {
        AssertResult.IsOk(Option<int>.None().Optional(x => x.IsGreaterThan(0)));
        AssertResult.IsOk(Option<string>.None().Optional(x => x.IsNotEmpty()));
    }

    [Fact]
    public void OptionIsNone_ReturnsError_WhenOptionHasValue()
    {
        AssertResult.IsError(Option.Some(-1).Optional(x => x.IsGreaterThan(0)));
        AssertResult.IsError(Option.Some("").Optional(x => x.IsNotEmpty()));

        var field = "My Option";
        AssertResult.IsErrorSingle(Option.Some(-1).Optional(x => x.IsGreaterThan(0, field)), m => m.StartsWith($"'{field}'"));

        var message = "A custom error message";
        AssertResult.IsErrorSingle(Option.Some(-1).Optional(x => x.IsGreaterThan(0, message: message)), m => m == message);
    }
}
