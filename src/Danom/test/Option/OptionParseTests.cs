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

    [Fact]
    public void EnumOptionTryParse_CaseInsensitiveAndNumeric() {
        AssertOption.IsSome(EnumOption.TryParse<Borp>("meep"));
        AssertOption.IsSome(EnumOption.TryParse<Borp>("1"));
        AssertOption.IsSome(EnumOption.TryParse<Borp>("0"));
        AssertOption.IsNone(EnumOption.TryParse<Borp>("2"));
    }

    [Fact]
    public void TryParse_EmptyStringIsNone() {
        AssertOption.IsNone(boolOption.TryParse(string.Empty));
        AssertOption.IsNone(byteOption.TryParse(string.Empty));
        AssertOption.IsNone(shortOption.TryParse(string.Empty));
        AssertOption.IsNone(intOption.TryParse(string.Empty));
        AssertOption.IsNone(longOption.TryParse(string.Empty));
        AssertOption.IsNone(decimalOption.TryParse(string.Empty));
        AssertOption.IsNone(doubleOption.TryParse(string.Empty));
        AssertOption.IsNone(floatOption.TryParse(string.Empty));
        AssertOption.IsNone(GuidOption.TryParse(string.Empty));
        AssertOption.IsNone(DateTimeOption.TryParse(string.Empty));
        AssertOption.IsNone(DateOnlyOption.TryParse(string.Empty));
        AssertOption.IsNone(TimeOnlyOption.TryParse(string.Empty));
        AssertOption.IsNone(DateTimeOffsetOption.TryParse(string.Empty));
        AssertOption.IsNone(TimeSpanOption.TryParse(string.Empty, _culture));
    }

    [Fact]
    public void boolOptionTryParse_CaseAndWhitespace() {
        AssertOption.IsSome(boolOption.TryParse(" TRUE "));
        AssertOption.IsSome(boolOption.TryParse("False"));
        AssertOption.IsNone(boolOption.TryParse("1"));
    }

    [Fact]
    public void byteOptionTryParse_Edges() {
        AssertOption.IsNone(byteOption.TryParse("-1"));
        AssertOption.IsNone(byteOption.TryParse("256"));
        AssertOption.IsSome(byteOption.TryParse(" 255 "));
    }

    [Fact]
    public void shortOptionTryParse_OverflowPlusWhitespace() {
        AssertOption.IsNone(shortOption.TryParse("32768"));
        AssertOption.IsNone(shortOption.TryParse("-32769"));
        AssertOption.IsSome(shortOption.TryParse(" +123 "));
    }

    [Fact]
    public void intOptionTryParse_OverflowPlusWhitespace() {
        AssertOption.IsNone(intOption.TryParse("2147483648"));
        AssertOption.IsNone(intOption.TryParse("-2147483649"));
        AssertOption.IsSome(intOption.TryParse(" +42 "));
    }

    [Fact]
    public void longOptionTryParse_OverflowPlusWhitespace() {
        AssertOption.IsNone(longOption.TryParse("9223372036854775808"));
        AssertOption.IsNone(longOption.TryParse("-9223372036854775809"));
        AssertOption.IsSome(longOption.TryParse(" -42 "));
    }

    [Fact]
    public void doubleOptionTryParse_SpecialValues() {
        var nan = doubleOption.TryParse("NaN");
        nan.Match(d => Assert.True(double.IsNaN(d)), () => Assert.Fail("Expected Some"));

        var pinf = doubleOption.TryParse("Infinity");
        pinf.Match(d => Assert.True(double.IsPositiveInfinity(d)), () => Assert.Fail("Expected Some"));

        var ninf = doubleOption.TryParse("-Infinity");
        ninf.Match(d => Assert.True(double.IsNegativeInfinity(d)), () => Assert.Fail("Expected Some"));
    }

    [Fact]
    public void floatOptionTryParse_SpecialValues() {
        var nan = floatOption.TryParse("NaN");
        nan.Match(f => Assert.True(float.IsNaN(f)), () => Assert.Fail("Expected Some"));

        var pinf = floatOption.TryParse("Infinity");
        pinf.Match(f => Assert.True(float.IsPositiveInfinity(f)), () => Assert.Fail("Expected Some"));

        var ninf = floatOption.TryParse("-Infinity");
        ninf.Match(f => Assert.True(float.IsNegativeInfinity(f)), () => Assert.Fail("Expected Some"));
    }

    [Fact]
    public void GuidOptionTryParse_WhitespaceAndEmpty() {
        AssertOption.IsSome(GuidOption.TryParse(" 11111111-1111-1111-1111-111111111111 "));
        AssertOption.IsNone(GuidOption.TryParse(""));
    }

    [Fact]
    public void GuidOptionTryParseExact_CaseInsensitiveAndInvalidFormat() {
        AssertOption.IsSome(GuidOption.TryParseExact("11111111111111111111111111111111", "n"));
        AssertOption.IsNone(GuidOption.TryParseExact("00000000-0000-0000-0000-000000000000", "Q"));
    }

    [Fact]
    public void DateTimeOptionTryParse_AllowsWhitespaceIso() {
        AssertOption.IsSome(DateTimeOption.TryParse(" 0001-01-01T00:00:00.0000000 "));
    }

    [Fact]
    public void DateOnlyOptionTryParseExact_CustomFormat() {
        AssertOption.IsSome(DateOnlyOption.TryParseExact("20240131", "yyyyMMdd", _culture));
    }

    [Fact]
    public void TimeOnlyOptionTryParseExact_CustomFormat() {
        AssertOption.IsSome(TimeOnlyOption.TryParseExact("235959", "HHmmss", _culture));
    }

    [Fact]
    public void DateTimeOffsetOptionTryParse_ZuluAndWhitespace() {
        AssertOption.IsSome(DateTimeOffsetOption.TryParse("2020-01-01T00:00:00Z"));
        AssertOption.IsSome(DateTimeOffsetOption.TryParse(" 2020-01-01T00:00:00+00:00 "));
    }

    [Fact]
    public void TimeSpanOptionTryParse_NegativeAndWhitespace() {
        AssertOption.IsSome(TimeSpanOption.TryParse(" -00:00:01 ", _culture));
    }

    [Fact]
    public void TimeSpanOptionTryParseExact_InvalidFormat() {
        AssertOption.IsNone(TimeSpanOption.TryParseExact("00:00:00", "X", _culture));
    }

    [Fact]
    public void byteOptionTryParse_WithStyles_Hex() {
        // Default (invariant, Integer) should not accept hex
        AssertOption.IsNone(byteOption.TryParse("FF"));
        // Hex with styles should work
        AssertOption.IsSome((byte)255, byteOption.TryParse("FF", NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void shortOptionTryParse_WithStyles_Hex() {
        AssertOption.IsNone(shortOption.TryParse("7FFF"));
        AssertOption.IsSome((short)32767, shortOption.TryParse("7FFF", NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void intOptionTryParse_WithStylesAndProvider_AllowsThousands() {
        var us = CultureInfo.CreateSpecificCulture("en-US");
        AssertOption.IsNone(intOption.TryParse("1,234")); // no thousands by default
        AssertOption.IsSome(1234, intOption.TryParse("1,234", NumberStyles.Integer | NumberStyles.AllowThousands, us));
    }

    [Fact]
    public void longOptionTryParse_WithStylesAndProvider_AllowsThousands() {
        var us = CultureInfo.CreateSpecificCulture("en-US");
        var s = "9,223,372,036,854,775,807";
        AssertOption.IsNone(longOption.TryParse(s));
        AssertOption.IsSome(9223372036854775807L, longOption.TryParse(s, NumberStyles.Integer | NumberStyles.AllowThousands, us));
    }

    [Fact]
    public void decimalOptionTryParse_WithProvider_FrCulture() {
        var fr = CultureInfo.CreateSpecificCulture("fr-FR");
        var nf = fr.NumberFormat;
        // Build "1 234,56" or "1 234,56" depending on platform (uses fr separators)
        var s = 1234.56m.ToString("N", fr);
        AssertOption.IsNone(decimalOption.TryParse(s)); // invariant expects '.' decimal and no thousands
        AssertOption.IsSome(1234.56m, decimalOption.TryParse(s, NumberStyles.Number, fr));
    }

    [Fact]
    public void doubleOptionTryParse_WithProvider_FrCulture() {
        var fr = CultureInfo.CreateSpecificCulture("fr-FR");
        var nf = fr.NumberFormat;
        // Build "1 234,5" using fr separators
        var s = 1234.5.ToString("N", fr);
        // Default invariant fails on comma decimal and thousands
        AssertOption.IsNone(doubleOption.TryParse(s));
        AssertOption.IsSome(1234.5, doubleOption.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, fr));
    }

    [Fact]
    public void floatOptionTryParse_WithProvider_FrCulture() {
        var fr = CultureInfo.CreateSpecificCulture("fr-FR");
        var s = 1234.5f.ToString("N", fr);
        AssertOption.IsNone(floatOption.TryParse(s)); // default invariant rejects fr format
        var r = floatOption.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, fr);
        r.Match(f => Assert.True(Math.Abs(f - 1234.5f) < 1e-3f), () => Assert.Fail("Expected Some"));
    }

    [Fact]
    public void DateTimeOptionTryParse_WithProvider_FrCulture() {
        var fr = CultureInfo.CreateSpecificCulture("fr-FR");
        var s = "31/12/2020 23:59:59";
        AssertOption.IsNone(DateTimeOption.TryParse(s)); // default invariant
        AssertOption.IsSome(DateTimeOption.TryParse(s, fr, DateTimeStyles.None));
    }

    [Fact]
    public void DateTimeOptionTryParseExact_WithProviderAndFormat() {
        var fr = CultureInfo.CreateSpecificCulture("fr-FR");
        var s = "31/12/2020 23:59:59";
        AssertOption.IsNone(DateTimeOption.TryParseExact(s, "O", fr)); // wrong format
        AssertOption.IsSome(DateTimeOption.TryParseExact(s, "dd/MM/yyyy HH:mm:ss", fr));
    }

    [Fact]
    public void DateTimeOffsetOptionTryParse_WithProvider_FrCulture() {
        var fr = CultureInfo.CreateSpecificCulture("fr-FR");
        var s = "31/12/2020 23:59:59 +01:00";
        AssertOption.IsNone(DateTimeOffsetOption.TryParse(s));
        AssertOption.IsSome(DateTimeOffsetOption.TryParse(s, fr, DateTimeStyles.None));
    }

    [Fact]
    public void DateTimeOffsetOptionTryParseExact_WithProviderAndFormat() {
        var fr = CultureInfo.CreateSpecificCulture("fr-FR");
        var s = "31/12/2020 23:59:59 +01:00";
        AssertOption.IsSome(DateTimeOffsetOption.TryParseExact(s, "dd/MM/yyyy HH:mm:ss zzz", fr, DateTimeStyles.None));
    }

    [Fact]
    public void DateOnlyOptionTryParse_WithProvider_FrCulture() {
        var fr = CultureInfo.CreateSpecificCulture("fr-FR");
        var s = "31/12/2020";
        AssertOption.IsNone(DateOnlyOption.TryParse(s)); // invariant expects yyyy-MM-dd
        AssertOption.IsSome(DateOnlyOption.TryParse(s, fr, DateTimeStyles.None));
    }

    [Fact]
    public void DateOnlyOptionTryParseExact_WithProviderAndFormat() {
        var fr = CultureInfo.CreateSpecificCulture("fr-FR");
        var s = "31/12/2020";
        AssertOption.IsSome(DateOnlyOption.TryParseExact(s, "dd/MM/yyyy", fr, DateTimeStyles.None));
    }

    [Fact]
    public void TimeOnlyOptionTryParseExact_WithProviderAndFormat() {
        var fr = CultureInfo.CreateSpecificCulture("fr-FR");
        var s = "23:59:59";
        AssertOption.IsSome(TimeOnlyOption.TryParseExact(s, "HH:mm:ss", fr, DateTimeStyles.None));
    }

    [Fact]
    public void TimeSpanOptionTryParse_WithProvider_FrCultureDecimalComma() {
        var fr = CultureInfo.CreateSpecificCulture("fr-FR");
        var s = "1:02:03,5"; // 1 hour, 2 minutes, 3.5 seconds (comma decimal)
        AssertOption.IsNone(TimeSpanOption.TryParse(s)); // invariant expects '.'
        AssertOption.IsSome(TimeSpanOption.TryParse(s, fr));
    }

    [Fact]
    public void TimeSpanOptionTryParseExact_WithProvider_GeneralShort() {
        var fr = CultureInfo.CreateSpecificCulture("fr-FR");
        var s = "1:02:03,5"; // matches "g" with fr decimal comma
        AssertOption.IsSome(TimeSpanOption.TryParseExact(s, "g", fr));
    }
}
