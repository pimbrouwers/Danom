namespace Danom;

public static class OptionNullableExtensions
{
    public static IOption<T> ToOption<T>(this T? x) =>
        x is not null && (!Equals(x, default(T))) ? Option<T>.Some(x) : Option<T>.None();

    public static IOption<T> ToOption<T>(this T? x) where T : struct =>
        (x is T t) ? Option<T>.Some(t) : Option<T>.None();

    public static IOption<string> ToOption(this string? x) =>
        x is not null && !string.IsNullOrWhiteSpace(x) ? Option<string>.Some(x) : Option<string>.None();

    public static T? ToNullable<T>(this IOption<T> option) =>
        option.Match(some: x => x, none: () => default!);

    public static char? ToNullable(this IOption<char> option) =>
        option.Match(x => new char?(x), () => null);

    public static bool? ToNullable(this IOption<bool> option) =>
        option.Match(x => new bool?(x), () => null);

    public static byte? ToNullable(this IOption<byte> option) =>
        option.Match(x => new byte?(x), () => null);

    public static short? ToNullable(this IOption<short> option) =>
        option.Match(x => new short?(x), () => null);

    public static int? ToNullable(this IOption<int> option) =>
        option.Match(x => new int?(x), () => null);

    public static long? ToNullable(this IOption<long> option) =>
        option.Match(x => new long?(x), () => null);

    public static decimal? ToNullable(this IOption<decimal> option) =>
        option.Match(x => new decimal?(x), () => null);

    public static double? ToNullable(this IOption<double> option) =>
        option.Match(x => new double?(x), () => null);

    public static float? ToNullable(this IOption<float> option) =>
        option.Match(x => new float?(x), () => null);

    public static Guid? ToNullable(this IOption<Guid> option) =>
        option.Match(x => new Guid?(x), () => null);

    public static DateTime? ToNullable(this IOption<DateTime> option) =>
        option.Match(x => new DateTime?(x), () => null);

    public static DateOnly? ToNullable(this IOption<DateOnly> option) =>
        option.Match(x => new DateOnly?(x), () => null);
}
