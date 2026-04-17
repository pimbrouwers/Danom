using System;

namespace Danom;
/// <summary>
/// Contains extension methods for functional programming.
/// </summary>
public static class FunctionalExtensions {
    /// <summary>
    /// Applies the given function to the target object and returns the result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="target"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static U Pipe<T, U>(this T target, Func<T, U> func) {
        if (func is null) {
            throw new ArgumentNullException(nameof(func));
        }

        return func(target);
    }
}
