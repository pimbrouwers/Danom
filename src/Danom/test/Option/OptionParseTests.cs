namespace Danom.Tests;

using System.Globalization;
using Danom.TestHelpers;
using Xunit;

public sealed class OptionParseTests {
    private static readonly IFormatProvider _culture = CultureInfo.CreateSpecificCulture("en-US");

    [Fact]
    public void boolOptionTryParse() {
        AssertOption.IsNone(boolOption.TryParse(null));
        AssertOption.IsNone(boolOption.TryParse("danom"));
        AssertOption.IsSome(boolOption.TryParse("true"));
        AssertOption.IsSome(boolOption.TryParse("false"));
    }

    [Fact]
    public void byteOptionTryParse() {
        AssertOption.IsNone(byteOption.TryParse(null));
        AssertOption.IsNone(byteOption.TryParse("danom"));
        AssertOption.IsSome(byteOption.TryParse("0"));
        AssertOption.IsSome(byteOption.TryParse("255"));
    }

    [Fact]
    public void shortOptionTryParse() {
        AssertOption.IsNone(shortOption.TryParse(null));
        AssertOption.IsNone(shortOption.TryParse("danom"));
        AssertOption.IsSome(Option.Some("-32768").Bind(shortOption.TryParse));
        AssertOption.IsSome(shortOption.TryParse("-32768"));
        AssertOption.IsSome(shortOption.TryParse("32767"));
    }

    [Fact]
    public void intOptionTryParse() {
        AssertOption.IsNone(intOption.TryParse(null));
        AssertOption.IsNone(intOption.TryParse("danom"));
        AssertOption.IsSome(Option.Some("-2147483648").Bind(intOption.TryParse));
        AssertOption.IsSome(intOption.TryParse("-2147483648"));
        AssertOption.IsSome(intOption.TryParse("2147483647"));
        AssertOption.IsSome(intOption.TryParse("-2147483648"));
        AssertOption.IsSome(intOption.TryParse("2147483647"));
    }

    [Fact]
    public void longOptionTryParse() {
        AssertOption.IsNone(longOption.TryParse(null));
        AssertOption.IsNone(longOption.TryParse("danom"));
        AssertOption.IsSome(Option.Some("-9223372036854775808").Bind(longOption.TryParse));
        AssertOption.IsSome(longOption.TryParse("-9223372036854775808"));
        AssertOption.IsSome(longOption.TryParse("9223372036854775807"));
    }

    [Fact]
    public void decimalOptionTryParse() {
        AssertOption.IsNone(decimalOption.TryParse(null));
        AssertOption.IsNone(decimalOption.TryParse("danom"));
        AssertOption.IsSome(Option.Some("-79228162514264337593543950335").Bind(decimalOption.TryParse));
        AssertOption.IsSome(decimalOption.TryParse("-79228162514264337593543950335"));
        AssertOption.IsSome(decimalOption.TryParse("79228162514264337593543950335"));
    }

    [Fact]
    public void doubleOptionTryParse() {
        AssertOption.IsNone(doubleOption.TryParse(null));
        AssertOption.IsNone(doubleOption.TryParse("danom"));
        AssertOption.IsSome(Option.Some("-1.7976931348623157E+308").Bind(doubleOption.TryParse));
        AssertOption.IsSome(doubleOption.TryParse("-1.7976931348623157E+308"));
        AssertOption.IsSome(doubleOption.TryParse("1.7976931348623157E+308"));
    }

    [Fact]
    public void floatOptionTryParse() {
        AssertOption.IsNone(floatOption.TryParse(null));
        AssertOption.IsNone(floatOption.TryParse("danom"));
        AssertOption.IsSome(Option.Some("-3.40282347E+38").Bind(floatOption.TryParse));
        AssertOption.IsSome(floatOption.TryParse("-3.40282347E+38"));
        AssertOption.IsSome(floatOption.TryParse("3.40282347E+38"));
    }

    [Fact]
    public void GuidOptionTryParse() {
        AssertOption.IsNone(GuidOption.TryParse(null));
        AssertOption.IsNone(GuidOption.TryParse("danom"));
        AssertOption.IsSome(Option.Some("00000000-0000-0000-0000-000000000000").Bind(GuidOption.TryParse));
        AssertOption.IsSome(GuidOption.TryParse("00000000-0000-0000-0000-000000000000"));
        AssertOption.IsSome(GuidOption.TryParse("11111111-1111-1111-1111-111111111111"));
    }

    [Fact]
    public void GuidOptionTryParseExact() {
        AssertOption.IsNone(GuidOption.TryParseExact(null, null));
        AssertOption.IsNone(GuidOption.TryParseExact("danom", "N"));
        AssertOption.IsSome(GuidOption.TryParseExact("00000000-0000-0000-0000-000000000000", "D"));
        AssertOption.IsSome(GuidOption.TryParseExact("11111111111111111111111111111111", "N"));
        AssertOption.IsSome(GuidOption.TryParseExact("{00000000-0000-0000-0000-000000000000}", "B"));
        AssertOption.IsSome(GuidOption.TryParseExact("(00000000-0000-0000-0000-000000000000)", "P"));
        AssertOption.IsSome(GuidOption.TryParseExact("{0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}", "X"));
    }

    [Fact]
    public void DateTimeOptionTryParse() {
        AssertOption.IsNone(DateTimeOption.TryParse(null));
        AssertOption.IsNone(DateTimeOption.TryParse("danom"));
        AssertOption.IsSome(Option.Some("0001-01-01T00:00:00.0000000").Bind(DateTimeOption.TryParse));
        AssertOption.IsSome(DateTimeOption.TryParse("0001-01-01T00:00:00.0000000"));
        AssertOption.IsSome(DateTimeOption.TryParse("9999-12-31T23:59:59.9999999"));
    }

    [Fact]
    public void DateTimeOptionTryParseExact() {
        AssertOption.IsNone(DateTimeOption.TryParseExact(null, null, _culture));
        AssertOption.IsNone(DateTimeOption.TryParseExact("danom", null, _culture));
        AssertOption.IsSome(DateTimeOption.TryParseExact("0001-01-01T00:00:00.0000000", "O", _culture));
        AssertOption.IsSome(DateTimeOption.TryParseExact("9999-12-31T23:59:59.9999999", "O", _culture));
    }

    [Fact]
    public void DateOnlyTryParse() {
        AssertOption.IsNone(DateOnlyOption.TryParse(null));
        AssertOption.IsNone(DateOnlyOption.TryParse("danom"));
        AssertOption.IsSome(Option.Some("0001-01-01").Bind(DateOnlyOption.TryParse));
        AssertOption.IsSome(DateOnlyOption.TryParse("0001-01-01"));
        AssertOption.IsSome(DateOnlyOption.TryParse("9999-12-31"));
    }

    [Fact]
    public void DateOnlyTryParseExact() {
        AssertOption.IsNone(DateOnlyOption.TryParseExact(null, null, _culture));
        AssertOption.IsNone(DateOnlyOption.TryParseExact("danom", null, _culture));
        AssertOption.IsSome(DateOnlyOption.TryParseExact("0001-01-01", "O", _culture));
        AssertOption.IsSome(DateOnlyOption.TryParseExact("9999-12-31", "O", _culture));
    }

    [Fact]
    public void TimeOnlyOptionTryParse() {
        AssertOption.IsNone(TimeOnlyOption.TryParse(null));
        AssertOption.IsNone(TimeOnlyOption.TryParse("danom"));
        AssertOption.IsSome(Option.Some("00:00:00").Bind(TimeOnlyOption.TryParse));
        AssertOption.IsSome(TimeOnlyOption.TryParse("00:00:00"));
        AssertOption.IsSome(TimeOnlyOption.TryParse("23:59:59.9999999"));
    }

    [Fact]
    public void TimeOnlyOptionTryParseExact() {
        AssertOption.IsNone(TimeOnlyOption.TryParseExact(null, null, _culture));
        AssertOption.IsNone(TimeOnlyOption.TryParseExact("danom", null, _culture));
        AssertOption.IsSome(TimeOnlyOption.TryParseExact("23:59:59.9999999", "O", _culture));
    }

    [Fact]
    public void DateTimeOffsetOptionTryParse() {
        AssertOption.IsNone(DateTimeOffsetOption.TryParse(null));
        AssertOption.IsNone(DateTimeOffsetOption.TryParse("danom"));
        AssertOption.IsSome(Option.Some("0001-01-01T00:00:00.0000000+00:00").Bind(DateTimeOffsetOption.TryParse));
        AssertOption.IsSome(DateTimeOffsetOption.TryParse("0001-01-01T00:00:00.0000000+00:00"));
        AssertOption.IsSome(DateTimeOffsetOption.TryParse("9999-12-31T23:59:59.9999999+00:00"));
    }

    [Fact]
    public void DateTimeOffsetOptionTryParseExact() {
        AssertOption.IsNone(DateTimeOffsetOption.TryParseExact(null, null, _culture));
        AssertOption.IsNone(DateTimeOffsetOption.TryParseExact("danom", null, _culture));

        // TODO determine how to make these work consistently across operating systems
        // AssertOption.IsSome(DateTimeOffsetOption.TryParseExact("0001-01-01T00:00:00.0000000+00:00", "O", _culture));
        // AssertOption.IsSome(DateTimeOffsetOption.TryParseExact("9999-12-31T23:59:59.9999999+00:00", "O", _culture));
        // AssertOption.IsSome(DateTimeOffsetOption.TryParseExact("0001-01-01T00:00:00.0000000+00:00", "s", _culture));
        // AssertOption.IsSome(DateTimeOffsetOption.TryParseExact("9999-12-31T23:59:59.9999999+00:00", "s", _culture));
        // AssertOption.IsSome(DateTimeOffsetOption.TryParseExact("0001-01-01T00:00:00.0000000+00:00", "u", _culture));
        // AssertOption.IsSome(DateTimeOffsetOption.TryParseExact("9999-12-31T23:59:59.9999999+00:00", "u", _culture));
    }

    [Fact]
    public void TimeSpanOptionTryParse() {
        AssertOption.IsNone(TimeSpanOption.TryParse(null, _culture));
        AssertOption.IsNone(TimeSpanOption.TryParse("danom", _culture));
        AssertOption.IsSome(Option.Some("00:00:00").Bind(TimeSpanOption.TryParse));
        AssertOption.IsSome(TimeSpanOption.TryParse("00:00:00", _culture));
        AssertOption.IsSome(TimeSpanOption.TryParse("10675199.02:48:05.4775807", _culture));
    }

    [Fact]
    public void TimeSpanOptionTryParseExact() {
        AssertOption.IsNone(TimeSpanOption.TryParseExact(null, null, _culture));
        AssertOption.IsNone(TimeSpanOption.TryParseExact("danom", null, _culture));
        AssertOption.IsSome(TimeSpanOption.TryParseExact("10675199.02:48:05.4775807", "c", _culture));
        AssertOption.IsSome(TimeSpanOption.TryParseExact("3:17:14:48.153", "g", _culture));
        AssertOption.IsSome(TimeSpanOption.TryParseExact("3:17:14:48.153", "G", _culture));

        AssertOption.IsSome(TimeSpanOption.TryParseExact("10675199.02:48:05.4775807", "c", _culture));
        AssertOption.IsSome(TimeSpanOption.TryParseExact("3:17:14:48.153", "g", _culture));
        AssertOption.IsSome(TimeSpanOption.TryParseExact("3:17:14:48.153", "G", _culture));
    }

    enum Borp {
        Meep,
        Morp
    }

    [Fact]
    public void EnumOptionTryParse() {
        AssertOption.IsNone(EnumOption.TryParse<Borp>(null));
        AssertOption.IsNone(EnumOption.TryParse<Borp>("danom"));
        AssertOption.IsSome(EnumOption.TryParse<Borp>("Meep"));
        AssertOption.IsSome(EnumOption.TryParse<Borp>("Morp"));
    }
}
