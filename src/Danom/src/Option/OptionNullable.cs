namespace Danom {

#if NET6_0_OR_GREATER
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
            x != null ? Option<T>.Some(x) : Option<T>.None();
        /// <summary>
        /// Converts an option to a nullable value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public static T? ToNullable<T>(this Option<T> option) where T : class =>
            option.Match(some: x => x, none: () => default!);
    }
#endif

#if NETSTANDARD2_1
    /// <summary>
    /// Contains extension methods for <see cref="Option{T}"/> that allow for
    /// converting between nullable types and options.
    /// </summary>
    public static class OptionNullableExtensions {
        /// <summary>
        /// Converts a nullable value to an option.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Option<T> ToOption<T>(this T x) =>
            x != null ? Option<T>.Some(x) : Option<T>.None();
        /// <summary>
        /// Converts an option to a nullable value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public static T? ToNullable<T>(this Option<T> option) where T : class =>
            option.Match(some: x => x, none: () => default!);
    }
#endif

    /// <summary>
    /// Contains extension methods for <see cref="Option{T}"/> that allow for
    /// converting between nullable struct types and options.
    /// </summary>
    public static class OptionNullableStructExtensions {
        /// <summary>
        /// Converts a nullable strict to an option.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Option<T> ToOption<T>(this T? x) where T : struct =>
            (x is T t) ? Option<T>.Some(t) : Option<T>.None();

        /// <summary>
        /// Converts a char option to a nullable value.
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static T? ToNullable<T>(this Option<T> option) where T : struct =>
            option.Match(x => new T?(x), () => null);
    }

    /// <summary>
    /// Contains extension methods for <see cref="Option{T}"/> that allow for
    /// converting between nullable string types and options.
    /// </summary>
    public static class OptionNullableStringExtensions {
        /// <summary>
        /// Converts a nullable string to an option.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Option<string> ToOption(this string? x) =>
            x != null && !string.IsNullOrWhiteSpace(x) ? Option<string>.Some(x) : Option<string>.None();

    }
}
