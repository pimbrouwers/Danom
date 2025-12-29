namespace Danom {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the existence, or not, of a value. Provides greater safety than
    /// using nulls, and allows for more expressive code with exhaustive matching.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct Option<T> : IEquatable<Option<T>>, IComparable<Option<T>> {
        private readonly T _some;

        internal Option(T t) {
            if (t is null) {
                _some = default!;
                IsSome = false;
            }
            else {
                _some = t;
                IsSome = true;
            }
        }

        /// <summary>
        /// Returns true if <see cref="Option{T}"/> is Some, false otherwise.
        /// </summary>
        public bool IsSome { get; }

        /// <summary>
        /// Returns true if <see cref="Option{T}"/> is None, false otherwise.
        /// </summary>
        public bool IsNone => !IsSome;

        /// <summary>
        /// If <see cref="Option{T}"/> is Some evaluate the some delegate, otherwise none.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="some"></param>
        /// <param name="none"></param>
        /// <returns></returns>
        public U Match<U>(Func<T, U> some, Func<U> none) =>
            IsSome && _some is T t ?
                some(t) :
                none();

        /// <summary>
        /// If <see cref="Option{T}"/> is Some evaluate the some delegate, otherwise none.
        /// </summary>
        /// <param name="some"></param>
        /// <param name="none"></param>
        public void Match(Action<T> some, Action none) =>
            Match(
                some: x => {
                    some(x);
                    return Unit.Value;
                },
                none: () => {
                    none();
                    return Unit.Value;
                });

        /// <summary>
        /// Evaluates the bind delegate if <see cref="Option{T}"/> is Some otherwise return None.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="bind"></param>
        /// <returns></returns>
        public Option<U> Bind<U>(
            Func<T, Option<U>> bind) =>
            Match(bind, Option<U>.None);

        /// <summary>
        /// Evaluates the map delegate if <see cref="Option{T}"/> is Some otherwise return None.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>
        public Option<U> Map<U>(
            Func<T, U> map) =>
            Bind(x => Option<U>.Some(map(x)));

        /// <summary>
        /// Returns the value of <see cref="Option{T}"/> if it is T otherwise return default.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T DefaultValue(
             T defaultValue) =>
             Match(some => some, () => defaultValue);

        /// <summary>
        /// Returns the value of <see cref="Option{T}"/> if it is T otherwise evaluate default.
        /// </summary>
        /// <param name="defaultWith"></param>
        /// <returns></returns>
        public T DefaultWith(
            Func<T> defaultWith) =>
            Match(some => some, () => defaultWith());

        /// <summary>
        /// Return <see cref="Option{T}"/> if it is Some, otherwise return ifNone.
        /// </summary>
        /// <param name="ifNone"></param>
        /// <returns></returns>
        public Option<T> OrElse(
            Option<T> ifNone) =>
            Match(Option<T>.Some, () => ifNone);

        /// <summary>
        /// Return <see cref="Option{T}"/> if it is Some, otherwise evaluate ifNoneWith.
        /// </summary>
        /// <param name="ifNoneWith"></param>
        /// <returns></returns>
        public Option<T> OrElseWith(
            Func<Option<T>> ifNoneWith) =>
            Match(Option<T>.Some, ifNoneWith);

        /// <summary>
        /// If <see cref="Option{T}"/> is Some evaluate the some delegate, otherwise do nothing.
        /// </summary>
        /// <param name="some"></param>
        public void Iter(
            Action<T> some) =>
            Match(some, () => { });

        /// <summary>
        /// Safely retrieve the value using procedural code.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryGet([MaybeNullWhen(false)] out T result) {
            var success = true;
            result = DefaultWith(() => {
                success = false;
                // we return this only to satisfy the compiler
                return default!;
            });
            return success;
        }

        /// <summary>
        /// Creates a new <see cref="Option{T}"/> with the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Option<T> Some(T value) =>
            new Option<T>(value);

        /// <summary>
        /// Creates <see cref="Option{T}"/> with the specified value wrapped in a completed Task.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Task<Option<T>> SomeAsync(T value) =>
            Task.FromResult(Some(value));

        /// <summary>
        /// Creates a new <see cref="Option{T}"/> with the value of the awaited Task.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<Option<T>> SomeAsync(
            Task<T> value,
            CancellationToken? cancellationToken = null) {
            var result = await value.WaitOrCancel(cancellationToken);
            return Some(result);
        }

        /// <summary>
        /// An Option of <see cref="Option{T}"/> with no value.
        /// </summary>
        /// <returns></returns>
        public static Option<T> NoneValue =>
            new Option<T>();

        /// <summary>
        /// Creates a new <see cref="Option{T}"/> with no value.
        /// </summary>
        /// <returns></returns>
        public static Option<T> None() =>
            NoneValue;

        /// <summary>
        /// Returns true if the specified <see cref="Option{T}"/>s are equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Option<T> left, Option<T> right) =>
            left.Equals(right);

        /// <summary>
        /// Returns true if the specified <see cref="Option{T}"/>s are not equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Option<T> left, Option<T> right) =>
            !(left == right);

        /// <summary>
        /// Returns true if the specified <see cref="Option{T}"/> is less than the other.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <(Option<T> left, Option<T> right) =>
            left.CompareTo(right) < 0;

        /// <summary>
        /// Returns true if the specified <see cref="Option{T}"/> is less than or equal to the other.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator <=(Option<T> left, Option<T> right) =>
            left.CompareTo(right) <= 0;

        /// <summary>
        /// Returns true if the specified <see cref="Option{T}"/> is greater than the other.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >(Option<T> left, Option<T> right) =>
            left.CompareTo(right) > 0;

        /// <summary>
        /// Returns true if the specified <see cref="Option{T}"/> is greater than or equal to the other.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator >=(Option<T> left, Option<T> right) =>
            left.CompareTo(right) >= 0;

        /// <summary>
        /// Returns true if the specified <see cref="Option{T}"/>s are equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) =>
            obj is Option<T> o && Equals(o);

        /// <summary>
        /// Returns true if the specified <see cref="Option{T}"/>s are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(Option<T> other) =>
            Match(
                some: x1 =>
                    other.Match(
                        some: x2 => x1 != null && x2 != null && x2.Equals(x1),
                        none: () => false),
                none: () =>
                    other.Match(
                        some: _ => false,
                        none: () => true)
                );

        /// <summary>
        /// Returns the hash code of the <see cref="Option{T}"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() =>
            Match(
                some: x => x is null ? 0 : x.GetHashCode(),
                none: () => 0);

        /// <summary>
        /// Compares the <see cref="Option{T}"/> to another <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Option<T> other) =>
            Match(
                some: x1 =>
                    other.Match(
                        some: x2 => Comparer<T>.Default.Compare(x1, x2),
                        none: () => 1),
                none: () =>
                    other.Match(
                        some: _ => -1,
                        none: () => 0)
                );

        /// <summary>
        /// Returns the string representation of the <see cref="Option{T}"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            Match(
                some: x => $"Some({x})",
                none: () => "None");

        /// <summary>
        /// Returns the string representation of the <see cref="Option{T}"/> or the
        /// provided default value.
        ///
        /// If format string and/or provider are provided, they are passed into
        /// the objects `ToString` method.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <param name="format"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public string ToString(
            string defaultValue,
            string? format = null,
            IFormatProvider? provider = null) =>
            Match(
                some: x =>
                x is IFormattable f ?
                    f!.ToString(format, provider) :
                    x!.ToString(),
                none: () => defaultValue) ?? string.Empty;
    }

    /// <summary>
    /// Provides static methods for creating <see cref="Option{T}"/>s.
    /// </summary>
    public static class Option {
        /// <summary>
        /// An Option of <see cref="Unit"/> with no value.
        /// </summary>
        public static Option<Unit> NoneValue =>
            Option<Unit>.NoneValue;

        /// <summary>
        /// Creates a new <see cref="Option{T}"/> with <see cref="Unit"/> value.
        /// </summary>
        /// <returns></returns>
        public static Option<Unit> None() =>
            Option.NoneValue;

        /// <summary>
        /// Creates a new <see cref="Option{T}"/> with <see cref="Unit"/> value.
        /// </summary>
        /// <returns></returns>
        public static Option<Unit> Some() =>
            Option<Unit>.Some(Unit.Value);

        /// <summary>
        /// Creates a new <see cref="Option{T}"/> with the specified value, with its
        /// type provided via method invocation. Enables `Option.Some(1)` syntax.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Option<T> Some<T>(T value) =>
            new Option<T>(value);

        /// <summary>
        /// Creates a new <see cref="Option{T}"/> with the value of the awaited Task.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<Option<T>> SomeAsync<T>(
            Task<T> value,
            CancellationToken? cancellationToken = null) {
            var result = await value.WaitOrCancel(cancellationToken);
            return Option<T>.Some(result);
        }
    }
}
