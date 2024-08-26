namespace Danom;

/// <summary>
/// Contains extension methods for <see cref="Option{T}"/> that allow for
/// converting between nullable types and options.
/// </summary>
public static class OptionNullableExtensions
{
    /// <summary>
    /// Converts a nullable value to an option.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Option<T> ToOption<T>(this T? x) =>
        x is not null && (!Equals(x, default(T))) ? Option<T>.Some(x) : Option<T>.None();

    /// <summary>
    /// Converts a nullable strct to an option.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Option<T> ToOption<T>(this T? x) where T : struct =>
        (x is T t) ? Option<T>.Some(t) : Option<T>.None();

    /// <summary>
    /// Converts a nullable string to an option.
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Option<string> ToOption(this string? x) =>
        x is not null && !string.IsNullOrWhiteSpace(x) ? Option<string>.Some(x) : Option<string>.None();

    /// <summary>
    /// Converts an option to a nullable value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <returns></returns>
    public static T? ToNullable<T>(this Option<T> option) =>
        option.Match(some: x => x, none: () => default!);

    /// <summary>
    /// Converts a char option to a nullable value.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public static char? ToNullable(this Option<char> option) =>
        option.Match(x => new char?(x), () => null);

    /// <summary>
    /// Converts a bool option to a nullable value.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public static bool? ToNullable(this Option<bool> option) =>
        option.Match(x => new bool?(x), () => null);

    /// <summary>
    /// Converts a byte option to a nullable value.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public static byte? ToNullable(this Option<byte> option) =>
        option.Match(x => new byte?(x), () => null);

    /// <summary>
    /// Converts a short option to a nullable value.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public static short? ToNullable(this Option<short> option) =>
        option.Match(x => new short?(x), () => null);

    /// <summary>
    /// Converts a ushort option to a nullable value.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public static int? ToNullable(this Option<int> option) =>
        option.Match(x => new int?(x), () => null);

    /// <summary>
    /// Converts a uint option to a nullable value.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>/
    public static long? ToNullable(this Option<long> option) =>
        option.Match(x => new long?(x), () => null);

    /// <summary>
    /// Converts a ulong option to a nullable value.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public static decimal? ToNullable(this Option<decimal> option) =>
        option.Match(x => new decimal?(x), () => null);

    /// <summary>
    /// Converts a double option to a nullable value.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public static double? ToNullable(this Option<double> option) =>
        option.Match(x => new double?(x), () => null);

    /// <summary>
    /// Converts a float option to a nullable value.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public static float? ToNullable(this Option<float> option) =>
        option.Match(x => new float?(x), () => null);

    /// <summary>
    /// Converts a decimal option to a nullable value.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public static Guid? ToNullable(this Option<Guid> option) =>
        option.Match(x => new Guid?(x), () => null);

    /// <summary>
    /// Converts a DateTime option to a nullable value.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public static DateTime? ToNullable(this Option<DateTime> option) =>
        option.Match(x => new DateTime?(x), () => null);

    /// <summary>
    /// Converts a DateTimeOffset option to a nullable value.
    /// </summary>
    /// <param name="option"></param>
    /// <returns></returns>
    public static DateOnly? ToNullable(this Option<DateOnly> option) =>
        option.Match(x => new DateOnly?(x), () => null);
}
