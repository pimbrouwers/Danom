using System;
using System.Globalization;

namespace Danom;
/// <summary>
/// boolOption
/// </summary>
public static class boolOption {
    /// <summary>
    /// Attempt to parse string as bool, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Option<bool> TryParse(string? x) =>
        bool.TryParse(x, out var y) ? Option.Some(y) : Option<bool>.NoneValue;
}

/// <summary>
/// byteOption
/// </summary>
public static class byteOption {
    /// <summary>
    /// Attempt to parse string as byte, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="styles"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static Option<byte> TryParse(string? x, NumberStyles? styles = default, IFormatProvider? provider = default) =>
        byte.TryParse(x, styles ?? NumberStyles.Integer, provider, out var y) ? Option.Some(y) : Option<byte>.NoneValue;
}

/// <summary>
/// shortOption
/// </summary>
public static class shortOption {
    /// <summary>
    /// Attempt to parse string as short, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="styles"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static Option<short> TryParse(string? x, NumberStyles? styles = default, IFormatProvider? provider = default) =>
        short.TryParse(x, styles ?? NumberStyles.Integer, provider, out var y) ? Option.Some(y) : Option<short>.NoneValue;
}

/// <summary>
/// intOption
/// </summary>
public static class intOption {
    /// <summary>
    /// Attempt to parse string as int, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="styles"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static Option<int> TryParse(string? x, NumberStyles? styles = default, IFormatProvider? provider = default) =>
        int.TryParse(x, styles ?? NumberStyles.Integer, provider, out var y) ? Option.Some(y) : Option<int>.NoneValue;
}

/// <summary>
/// longOption
/// </summary>
public static class longOption {
    /// <summary>
    /// Attempt to parse string as long, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="styles"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static Option<long> TryParse(string? x, NumberStyles? styles = default, IFormatProvider? provider = default) =>
        long.TryParse(x, styles ?? NumberStyles.Integer, provider, out var y) ? Option.Some(y) : Option<long>.NoneValue;
}

/// <summary>
/// decimalOption
/// </summary>
public static class decimalOption {
    /// <summary>
    /// Attempt to parse string as decimal, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="styles"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static Option<decimal> TryParse(string? x, NumberStyles? styles = default, IFormatProvider? provider = default) =>
        decimal.TryParse(x, styles ?? NumberStyles.Number, provider, out var y) ? Option.Some(y) : Option<decimal>.NoneValue;
}

/// <summary>
/// doubleOption
/// </summary>
public static class doubleOption {
    /// <summary>
    /// Attempt to parse string as double, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="styles"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static Option<double> TryParse(string? x, NumberStyles? styles = default, IFormatProvider? provider = default) =>
        // Defaulting to InvariantCulture to handle special float values like "NaN", "Infinity", and "-Infinity" consistently across cultures.
        double.TryParse(x, styles ?? NumberStyles.Float | NumberStyles.AllowThousands, provider ?? CultureInfo.InvariantCulture, out var y) ? Option.Some(y) : Option<double>.NoneValue;
}

/// <summary>
/// floatOption
/// </summary>
public static class floatOption {
    /// <summary>
    /// Attempt to parse string as float, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="styles"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static Option<float> TryParse(string? x, NumberStyles? styles = default, IFormatProvider? provider = default) =>
        // Defaulting to InvariantCulture to handle special float values like "NaN", "Infinity", and "-Infinity" consistently across cultures.
        float.TryParse(x, styles ?? NumberStyles.Float | NumberStyles.AllowThousands, provider ?? CultureInfo.InvariantCulture, out var y) ? Option.Some(y) : Option<float>.NoneValue;
}

/// <summary>
/// GuidOption
/// </summary>
public static class GuidOption {
    /// <summary>
    /// Attempt to parse string as Guid, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Option<Guid> TryParse(string? x) =>
        Guid.TryParse(x, out var y) ? Option.Some(y) : Option<Guid>.NoneValue;

    /// <summary>
    /// Attempt to parse string as Guid with exact format, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static Option<Guid> TryParseExact(string? x, string? format) =>
        Guid.TryParseExact(x, format, out var y) ? Option.Some(y) : Option<Guid>.NoneValue;
}

/// <summary>
/// DateTimeOffsetOption
/// </summary>
public static class DateTimeOffsetOption {
    /// <summary>
    /// Attempt to parse string as DateTimeOffset, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="provider"></param>
    /// <param name="dateTimeStyles"></param>
    /// <returns></returns>
    public static Option<DateTimeOffset> TryParse(string? x, IFormatProvider? provider = default, DateTimeStyles? dateTimeStyles = default) =>
        DateTimeOffset.TryParse(x, provider, dateTimeStyles ?? DateTimeStyles.None, out var y) ? Option.Some(y) : Option<DateTimeOffset>.NoneValue;

    /// <summary>
    /// Attempt to parse string as DateTimeOffset with exact format, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="format"></param>
    /// <param name="provider"></param>
    /// <param name="dateTimeStyles"></param>
    /// <returns></returns>
    public static Option<DateTimeOffset> TryParseExact(string? x, string? format, IFormatProvider? provider, DateTimeStyles dateTimeStyles = DateTimeStyles.None) =>
        DateTimeOffset.TryParseExact(x, format, provider, dateTimeStyles, out var y) ? Option.Some(y) : Option<DateTimeOffset>.NoneValue;
}

/// <summary>
/// DateTimeOption
/// </summary>
public static class DateTimeOption {
    /// <summary>
    /// Attempt to parse string as DateTime, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="provider"></param>
    /// <param name="dateTimeStyles"></param>
    /// <returns></returns>
    public static Option<DateTime> TryParse(string? x, IFormatProvider? provider = default, DateTimeStyles? dateTimeStyles = default) =>
        DateTime.TryParse(x, provider, dateTimeStyles ?? DateTimeStyles.None, out var y) ? Option.Some(y) : Option<DateTime>.NoneValue;

    /// <summary>
    /// Attempt to parse string as DateTime with exact format, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="format"></param>
    /// <param name="provider"></param>
    /// <param name="dateTimeStyles"></param>
    /// <returns></returns>
    public static Option<DateTime> TryParseExact(string? x, string? format, IFormatProvider? provider = default, DateTimeStyles dateTimeStyles = DateTimeStyles.None) =>
        DateTime.TryParseExact(x, format, provider, dateTimeStyles, out var y) ? Option.Some(y) : Option<DateTime>.NoneValue;
}

#if NET6_0_OR_GREATER
/// <summary>
/// DateOnlyOption
/// </summary>
public static class DateOnlyOption {
    /// <summary>
    /// Attempt to parse string as DateOnly, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="provider"></param>
    /// <param name="dateTimeStyles"></param>
    /// <returns></returns>
    public static Option<DateOnly> TryParse(string? x, IFormatProvider? provider = default, DateTimeStyles? dateTimeStyles = default) =>
        DateOnly.TryParse(x, provider, dateTimeStyles ?? DateTimeStyles.None, out var y) ? Option.Some(y) : Option<DateOnly>.NoneValue;

    /// <summary>
    /// Attempt to parse string as DateOnly with exact format, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="format"></param>
    /// <param name="provider"></param>
    /// <param name="dateTimeStyles"></param>
    /// <returns></returns>
    public static Option<DateOnly> TryParseExact(string? x, string? format, IFormatProvider? provider = default, DateTimeStyles dateTimeStyles = DateTimeStyles.None) =>
        DateOnly.TryParseExact(x, format, provider, dateTimeStyles, out var y) ? Option.Some(y) : Option<DateOnly>.NoneValue;
}

/// <summary>
/// TimeOnlyOption
/// </summary>
public static class TimeOnlyOption {
    /// <summary>
    /// Attempt to parse string as TimeOnly, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="provider"></param>
    /// <param name="dateTimeStyles"></param>
    /// <returns></returns>
    public static Option<TimeOnly> TryParse(string? x, IFormatProvider? provider = default, DateTimeStyles? dateTimeStyles = default) =>
        TimeOnly.TryParse(x, provider, dateTimeStyles ?? DateTimeStyles.None, out var y) ? Option.Some(y) : Option<TimeOnly>.NoneValue;

    /// <summary>
    /// Attempt to parse string as TimeOnly with exact format, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="format"></param>
    /// <param name="provider"></param>
    /// <param name="dateTimeStyles"></param>
    /// <returns></returns>
    public static Option<TimeOnly> TryParseExact(string? x, string? format, IFormatProvider? provider = default, DateTimeStyles dateTimeStyles = DateTimeStyles.None) =>
        TimeOnly.TryParseExact(x, format, provider, dateTimeStyles, out var y) ? Option.Some(y) : Option<TimeOnly>.NoneValue;
}
#endif

/// <summary>
/// TimeSpanOption
/// </summary>
public static class TimeSpanOption {
    /// <summary>
    /// Attempt to parse string as TimeSpan, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static Option<TimeSpan> TryParse(string? x, IFormatProvider? provider = default) =>
        TimeSpan.TryParse(x, provider, out var y) ? Option.Some(y) : Option<TimeSpan>.NoneValue;

    /// <summary>
    /// Attempt to parse string as TimeSpan with exact format, return None if invalid, Some if valid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="format"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static Option<TimeSpan> TryParseExact(string? x, string? format, IFormatProvider? provider = default) =>
        TimeSpan.TryParseExact(x, format, provider, out var y) ? Option.Some(y) : Option<TimeSpan>.NoneValue;
}

/// <summary>
/// EnumOption
/// </summary>
public static class EnumOption {
    /// <summary>
    /// Attempt to parse string as enum of type TEnum, return None if invalid, Some if valid.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Option<TEnum> TryParse<TEnum>(string? x) where TEnum : struct, Enum {
        if (string.IsNullOrWhiteSpace(x)) return Option<TEnum>.NoneValue;

        var s = x.Trim();
        if (Enum.TryParse<TEnum>(s, ignoreCase: true, out var value)) {
            var isNumericInput = char.IsDigit(s[0]) || s[0] == '+' || s[0] == '-';
            if (isNumericInput && !Enum.IsDefined(typeof(TEnum), value)) {
                return Option<TEnum>.NoneValue;
            }
            return Option.Some(value);
        }
        return Option<TEnum>.NoneValue;
    }
}
