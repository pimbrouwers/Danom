namespace Danom.Tests;

using System.Globalization;
using Xunit;
using Danom.TestHelpers;

public sealed class OptionParseTests
{
    [Fact]
    public void boolOptionTryParse()
    {
        AssertOption.IsNone(boolOption.TryParse(null));
        AssertOption.IsNone(boolOption.TryParse("danom"));
        AssertOption.IsSome(boolOption.TryParse("true"));
        AssertOption.IsSome(boolOption.TryParse("false"));
    }

    [Fact]
    public void byteOptionTryParse()
    {
        AssertOption.IsNone(byteOption.TryParse(null));
        AssertOption.IsNone(byteOption.TryParse("danom"));
        AssertOption.IsSome(byteOption.TryParse("0"));
        AssertOption.IsSome(byteOption.TryParse("255"));
    }

    [Fact]
    public void shortOptionTryParse()
    {
        AssertOption.IsNone(shortOption.TryParse(null));
        AssertOption.IsNone(shortOption.TryParse("danom"));
        AssertOption.IsSome(shortOption.TryParse("-32768"));
        AssertOption.IsSome(shortOption.TryParse("32767"));
    }

    [Fact]
    public void intOptionTryParse()
    {
        AssertOption.IsNone(intOption.TryParse(null, null));
        AssertOption.IsNone(intOption.TryParse("danom", null));
        AssertOption.IsSome(intOption.TryParse("-2147483648", null));
        AssertOption.IsSome(intOption.TryParse("2147483647", null));
        AssertOption.IsSome(intOption.TryParse("-2147483648", CultureInfo.CurrentUICulture));
        AssertOption.IsSome(intOption.TryParse("2147483647", CultureInfo.CurrentUICulture));
    }

    [Fact]
    public void longOptionTryParse()
    {
        AssertOption.IsNone(longOption.TryParse(null, null));
        AssertOption.IsNone(longOption.TryParse("danom", null));
        AssertOption.IsSome(longOption.TryParse("-9223372036854775808", null));
        AssertOption.IsSome(longOption.TryParse("9223372036854775807", null));
        AssertOption.IsSome(longOption.TryParse("-9223372036854775808", CultureInfo.CurrentUICulture));
        AssertOption.IsSome(longOption.TryParse("9223372036854775807", CultureInfo.CurrentUICulture));
    }

    [Fact]
    public void decimalOptionTryParse()
    {
        AssertOption.IsNone(decimalOption.TryParse(null, null));
        AssertOption.IsNone(decimalOption.TryParse("danom", null));
        AssertOption.IsSome(decimalOption.TryParse("-79228162514264337593543950335", null));
        AssertOption.IsSome(decimalOption.TryParse("79228162514264337593543950335", null));
        AssertOption.IsSome(decimalOption.TryParse("-79228162514264337593543950335", CultureInfo.CurrentUICulture));
        AssertOption.IsSome(decimalOption.TryParse("79228162514264337593543950335", CultureInfo.CurrentUICulture));
    }

    [Fact]
    public void doubleOptionTryParse()
    {
        AssertOption.IsNone(doubleOption.TryParse(null, null));
        AssertOption.IsNone(doubleOption.TryParse("danom", null));
        AssertOption.IsSome(doubleOption.TryParse("-1.7976931348623157E+308", null));
        AssertOption.IsSome(doubleOption.TryParse("1.7976931348623157E+308", null));
        AssertOption.IsSome(doubleOption.TryParse("-1.7976931348623157E+308", CultureInfo.CurrentUICulture));
        AssertOption.IsSome(doubleOption.TryParse("1.7976931348623157E+308", CultureInfo.CurrentUICulture));
    }

    [Fact]
    public void floatOptionTryParse()
    {
        AssertOption.IsNone(floatOption.TryParse(null, null));
        AssertOption.IsNone(floatOption.TryParse("danom", null));
        AssertOption.IsSome(floatOption.TryParse("-3.40282347E+38", null));
        AssertOption.IsSome(floatOption.TryParse("3.40282347E+38", null));
        AssertOption.IsSome(floatOption.TryParse("-3.40282347E+38", CultureInfo.CurrentUICulture));
        AssertOption.IsSome(floatOption.TryParse("3.40282347E+38", CultureInfo.CurrentUICulture));
    }

    [Fact]
    public void GuidOptionTryParse()
    {
        AssertOption.IsNone(GuidOption.TryParse(null));
        AssertOption.IsNone(GuidOption.TryParse("danom"));
        AssertOption.IsSome(GuidOption.TryParse("00000000-0000-0000-0000-000000000000"));
        AssertOption.IsSome(GuidOption.TryParse("11111111-1111-1111-1111-111111111111"));
    }

    [Fact]
    public void GuidOptionTryParseExact()
    {
        AssertOption.IsNone(GuidOption.TryParseExact(null, null));
        AssertOption.IsNone(GuidOption.TryParseExact("danom", "N"));
        AssertOption.IsSome(GuidOption.TryParseExact("00000000-0000-0000-0000-000000000000", "D"));
        AssertOption.IsSome(GuidOption.TryParseExact("11111111111111111111111111111111", "N"));
        AssertOption.IsSome(GuidOption.TryParseExact("{00000000-0000-0000-0000-000000000000}", "B"));
        AssertOption.IsSome(GuidOption.TryParseExact("(00000000-0000-0000-0000-000000000000)", "P"));
        AssertOption.IsSome(GuidOption.TryParseExact("{0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}", "X"));
    }

    [Fact]
    public void DateTimeOffsetOptionTryParse()
    {
        AssertOption.IsNone(DateTimeOffsetOption.TryParse(null, null));
        AssertOption.IsNone(DateTimeOffsetOption.TryParse("danom", null));
        AssertOption.IsSome(DateTimeOffsetOption.TryParse("0001-01-01T00:00:00.0000000+00:00", null));
        AssertOption.IsSome(DateTimeOffsetOption.TryParse("9999-12-31T23:59:59.9999999+00:00", null));
    }

    [Fact]
    public void DateTimeOffsetOptionTryParseExact()
    {
        AssertOption.IsNone(DateTimeOffsetOption.TryParseExact(null, null, null));
        AssertOption.IsNone(DateTimeOffsetOption.TryParseExact("danom", null, null));
        // AssertOption.IsSome(DateTimeOffsetOption.TryParseExact("0001-01-01T00:00:00.0000000+00:00", "O", null));
        // AssertOption.IsSome(DateTimeOffsetOption.TryParseExact("9999-12-31T23:59:59.9999999+00:00", "O", null));
        // AssertOption.IsSome(DateTimeOffsetOption.TryParseExact("0001-01-01T00:00:00.0000000+00:00", "s", null));
        // AssertOption.IsSome(DateTimeOffsetOption.TryParseExact("9999-12-31T23:59:59.9999999+00:00", "s", null));
        // AssertOption.IsSome(DateTimeOffsetOption.TryParseExact("0001-01-01T00:00:00.0000000+00:00", "u", null));
        // AssertOption.IsSome(DateTimeOffsetOption.TryParseExact("9999-12-31T23:59:59.9999999+00:00", "u", null));
    }

    [Fact]
    public void DateOnlyOptionTryParse()
    {
        AssertOption.IsNone(DateOnlyOption.TryParse(null, null));
        AssertOption.IsNone(DateOnlyOption.TryParse("danom", null));
        AssertOption.IsSome(DateOnlyOption.TryParse("0001-01-01", null));
        AssertOption.IsSome(DateOnlyOption.TryParse("9999-12-31", null));
    }

    [Fact]
    public void TimeOnlyOptionTryParse()
    {
        AssertOption.IsNone(TimeOnlyOption.TryParse(null, null));
        AssertOption.IsNone(TimeOnlyOption.TryParse("danom", null));
        AssertOption.IsSome(TimeOnlyOption.TryParse("00:00:00", null));
        AssertOption.IsSome(TimeOnlyOption.TryParse("23:59:59", null));
    }

    [Fact]
    public void TimeSpanOptionTryParse()
    {
        AssertOption.IsNone(TimeSpanOption.TryParse(null, null));
        AssertOption.IsNone(TimeSpanOption.TryParse("danom", null));
        AssertOption.IsSome(TimeSpanOption.TryParse("00:00:00", null));
        AssertOption.IsSome(TimeSpanOption.TryParse("10675199.02:48:05.4775807", null));
    }

    [Fact]
    public void TimeSpanOptionTryParseExact()
    {
        AssertOption.IsNone(TimeSpanOption.TryParseExact(null, null, null));
        AssertOption.IsNone(TimeSpanOption.TryParseExact("danom", null, null));
        AssertOption.IsSome(TimeSpanOption.TryParseExact("10675199.02:48:05.4775807", "c", null));
        AssertOption.IsSome(TimeSpanOption.TryParseExact("3:17:14:48.153", "g", null));
        AssertOption.IsSome(TimeSpanOption.TryParseExact("3:17:14:48.153", "G", null));
        AssertOption.IsSome(TimeSpanOption.TryParseExact("10675199.02:48:05.4775807", "c", CultureInfo.CurrentUICulture));
        AssertOption.IsSome(TimeSpanOption.TryParseExact("3:17:14:48.153", "g", CultureInfo.CurrentUICulture));
        AssertOption.IsSome(TimeSpanOption.TryParseExact("3:17:14:48.153", "G", CultureInfo.CurrentUICulture));
    }

    enum Borp
    {
        Meep,
        Morp
    }

    [Fact]
    public void EnumOptionTryParse()
    {
        AssertOption.IsNone(EnumOption.TryParse<Borp>(null));
        AssertOption.IsNone(EnumOption.TryParse<Borp>("danom"));
        AssertOption.IsSome(EnumOption.TryParse<Borp>("Meep"));
        AssertOption.IsSome(EnumOption.TryParse<Borp>("Morp"));
    }
}
