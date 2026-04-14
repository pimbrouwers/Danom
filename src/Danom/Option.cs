namespace Danom {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Represents the existence, or not, of a value. Provides greater safety than
    /// using nulls, and allows for more expressive code with exhaustive matching.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct Option<T> : IEquatable<Option<T>>, IComparable<Option<T>> {
        private readonly byte _state; // 0 = None, 1 = Some
        private readonly T _some;

        internal Option(T t) {
            if (t is null) {
                _state = 0;
                _some = default!;
            }
            else {
                _state = 1;
                _some = t;
            }
        }

        /// <summary>
        /// Returns true if <see cref="Option{T}"/> is Some, false otherwise.
        /// </summary>
        public bool IsSome => _state == 1 && _some != null;

        /// <summary>
        /// Returns true if <see cref="Option{T}"/> is None, false otherwise.
        /// </summary>
        public bool IsNone => !IsSome;

        /// <summary>
        /// Returns 1 if <see cref="Option{T}"/> is Some, 0 otherwise.
        /// </summary>
        public int Count => IsSome ? 1 : 0;

        /// <summary>
        /// Creates a new <see cref="Option{T}"/> with the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Option<T> Some(T value) =>
            new Option<T>(value);

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
        /// If <see cref="Option{T}"/> is Some evaluate the some delegate, otherwise none.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="some"></param>
        /// <param name="none"></param>
        /// <returns></returns>
        public U Match<U>(Func<T, U> some, Func<U> none) => _state switch {
            1 when _some != null => some(_some),
            _ => none(),
        };

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
        public Option<U> Bind<U>(Func<T, Option<U>> bind) =>
            Match(bind, Option<U>.None);

        /// <summary>
        /// Evaluates the map delegate if <see cref="Option{T}"/> is Some otherwise return None.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>
        public Option<U> Map<U>(Func<T, U> map) =>
            Bind(x => Option<U>.Some(map(x)));

        /// <summary>
        /// Applies the function contained in the input <see cref="Option{T}"/> to the value of this <see cref="Option{T}"/>.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="fn"></param>
        /// <returns></returns>
        public Option<U> Apply<U>(Option<Func<T, U>> fn) =>
            Bind(x => fn.Map(i => i.Invoke(x)));

        /// <summary>
        /// Returns the value of <see cref="Option{T}"/> if it is T otherwise return default.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T DefaultValue(T defaultValue) =>
             Match(some => some, () => defaultValue);

        /// <summary>
        /// Returns the value of <see cref="Option{T}"/> if it is T otherwise evaluate default.
        /// </summary>
        /// <param name="defaultWith"></param>
        /// <returns></returns>
        public T DefaultWith(Func<T> defaultWith) =>
            Match(some => some, () => defaultWith());

        /// <summary>
        /// Return <see cref="Option{T}"/> if it is Some, otherwise return ifNone.
        /// </summary>
        /// <param name="ifNone"></param>
        /// <returns></returns>
        public Option<T> OrElse(Option<T> ifNone) =>
            Match(Option<T>.Some, () => ifNone);

        /// <summary>
        /// Return <see cref="Option{T}"/> if it is Some, otherwise evaluate ifNoneWith.
        /// </summary>
        /// <param name="ifNoneWith"></param>
        /// <returns></returns>
        public Option<T> OrElseWith(Func<Option<T>> ifNoneWith) =>
            Match(Option<T>.Some, ifNoneWith);

        /// <summary>
        /// If <see cref="Option{T}"/> is Some, invoke the tap action and return the original Option.
        /// </summary>
        /// <param name="tap"></param>
        /// <returns></returns>
        public Option<T> Tap(Action<T> tap) =>
            Map(x => {
                tap(x);
                return x;
            });

        /// <summary>
        /// If <see cref="Option{T}"/> is Some evaluate the some delegate, otherwise do nothing.
        /// </summary>
        /// <param name="some"></param>
        public void Iter(Action<T> some) =>
            Match(some, () => { });

        /// <summary>
        /// Returns a new <see cref="Option{T}"/> containing the value if it satisfies the predicate, otherwise None.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Option<T> Filter(Func<T, bool> predicate) =>
            Bind(x => predicate(x) ? Option.Some(x) : Option<T>.None());

        /// <summary>
        /// Returns true if any value in the <see cref="Option{T}"/> satisfies the predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Exists(Func<T, bool> predicate) =>
            Map(x => predicate(x))
                .DefaultWith(() => false);

        /// <summary>
        /// Returns true if the <see cref="Option{T}"/> contains the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value) =>
            Exists(x => EqualityComparer<T>.Default.Equals(x, value));

        /// <summary>
        /// Converts the <see cref="Option{T}"/> to a List.
        /// </summary>
        /// <returns></returns>
        public List<T> ToList() =>
            Map(x => new List<T> { x })
                .DefaultWith(() => new List<T>());

        /// <summary>
        /// Converts the <see cref="Option{T}"/> to an Array.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray() =>
            Map(x => new T[] { x })
                .DefaultWith(() => Array.Empty<T>());

        /// <summary>
        /// Safely retrieve the value using procedural code.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet([MaybeNullWhen(false)] out T value) {
            value = IsSome ? _some : default!;
            return IsSome;
        }

        /// <summary>
        /// Returns true if the specified <see cref="Option{T}"/> are equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Option<T> left, Option<T> right) =>
            left.Equals(right);

        /// <summary>
        /// Returns true if the specified <see cref="Option{T}"/> are not equal.
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
        /// Returns true if the specified <see cref="Option{T}"/> are equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) =>
            obj is Option<T> o && Equals(o);

        /// <summary>
        /// Returns true if the specified <see cref="Option{T}"/> are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(Option<T> other) {
            if (_state != other._state) {
                return false;
            }

            if (_state == 0 && other._state == 0) {
                return true;
            }

            return _state switch {
                1 => _some is null ? other._some is null : _some.Equals(other._some),
                _ => false
            };
        }

        /// <summary>
        /// Returns the hash code of the <see cref="Option{T}"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => _state switch {
            1 when _some != null => _some.GetHashCode(),
            _ => 0
        };

        /// <summary>
        /// Compares the <see cref="Option{T}"/> to another <see cref="Option{T}"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Option<T> other) => _state switch {
            1 when _some is T t1 && other._state == 1 && other._some is T t2 => Comparer<T>.Default.Compare(t1, t2),
            1 when other._state == 0 => 1,
            0 when other._state == 1 => -1,
            _ => 0
        };

        /// <summary>
        /// Returns the string representation of the <see cref="Option{T}"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => _state switch {
            1 when _some is Unit => "Some()",
            1 when _some != null => $"Some({_some})",
            _ => "None"
        };

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
            IFormatProvider? provider = null) => _state switch {
                1 when _some is T t && t is IFormattable f => f.ToString(format, provider),
                1 when _some is T t => t.ToString(),
                _ => defaultValue
            } ?? string.Empty;
    }

    /// <summary>
    /// Provides static methods for creating <see cref="Option{T}"/>.
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
            NoneValue;

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
            // Provides type inference for Option.Some(value) syntax
            new Option<T>(value);

        /// <summary>
        /// Matches an enumerable of <see cref="Option{T}"/> and invokes the appropriate delegate.
        /// If all options are Some, invokes some with the values.
        /// If any option is None, invokes none.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="options"></param>
        /// <param name="none"></param>
        /// <param name="some"></param>
        /// <returns></returns>
        public static U Match<T, U>(
            IEnumerable<Option<T>> options,
            Func<U> none,
            Func<IEnumerable<T>, U> some) {
            var values = new List<T>();
            foreach (var option in options) {
                if (option.TryGet(out var v)) {
                    values.Add(v);
                }
                else {
                    return none();
                }
            }
            return some(values);
        }

        /// <summary>
        /// Matches two <see cref="Option{T}"/> and invokes the appropriate delegate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <param name="none"></param>
        /// <param name="some"></param>
        /// <returns></returns>
        public static U Match<T, T2, U>(
            Option<T> o1,
            Option<T2> o2,
            Func<T, T2, U> some,
            Func<U> none) =>
            o1.Match(v1 => o2.Match(v2 => some(v1, v2), none), none);

        /// <summary>
        /// Matches three <see cref="Option{T}"/> and invokes the appropriate delegate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <param name="o3"></param>
        /// <param name="none"></param>
        /// <param name="some"></param>
        /// <returns></returns>
        public static U Match<T, T2, T3, U>(
            Option<T> o1,
            Option<T2> o2,
            Option<T3> o3,
            Func<T, T2, T3, U> some,
            Func<U> none) =>
            Match(o1, o2, (v1, v2) => o3.Match(v3 => some(v1, v2, v3), none), none);

        /// <summary>
        /// Binds two <see cref="Option{T}"/> to a new <see cref="Option{U}"/> using the provided bind function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <param name="bind"></param>
        /// <returns></returns>
        public static Option<U> Bind<T, T2, U>(
            Option<T> o1,
            Option<T2> o2,
            Func<T, T2, Option<U>> bind) =>
            o1.Bind(v1 => o2.Bind(v2 => bind(v1, v2)));

        /// <summary>
        /// Binds three <see cref="Option{T}"/> to a new <see cref="Option{U}"/> using the provided bind function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <param name="o3"></param>
        /// <param name="bind"></param>
        /// <returns></returns>
        public static Option<U> Bind<T, T2, T3, U>(
            Option<T> o1,
            Option<T2> o2,
            Option<T3> o3,
            Func<T, T2, T3, Option<U>> bind) =>
            Bind(o1, o2, (v1, v2) => o3.Bind(v3 => bind(v1, v2, v3)));

        /// <summary>
        /// Maps two <see cref="Option{T}"/> to a new <see cref="Option{U}"/> using the provided map function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static Option<U> Map<T, T2, U>(
            Option<T> o1,
            Option<T2> o2,
            Func<T, T2, U> map) =>
            Bind(o1, o2, (v1, v2) => Some(map(v1, v2)));

        /// <summary>
        /// Maps three <see cref="Option{T}"/> to a new <see cref="Option{U}"/> using the provided map function.
        /// /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <param name="o3"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static Option<U> Map<T, T2, T3, U>(
            Option<T> o1,
            Option<T2> o2,
            Option<T3> o3,
            Func<T, T2, T3, U> map) =>
            Bind(o1, o2, o3, (v1, v2, v3) => Option<U>.Some(map(v1, v2, v3)));

        /// <summary>
        /// Flattens a nested <see cref="Option{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public static Option<T> Flatten<T>(Option<Option<T>> option) =>
            option.Bind(x => x);

        /// <summary>
        /// Flattens an enumerable of <see cref="Option{T}"/>, returning an enumerable of the values contained in the Some options.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IEnumerable<T> Flatten<T>(IEnumerable<Option<T>> options) {
            foreach (var option in options) {
                if (option.TryGet(out var v)) {
                    yield return v;
                }
            }
        }

        /// <summary>
        /// Traverses the specified items, applying the func to each item and aggregating the results.
        /// If all applications of func are some, returns a Option containing an IEnumerable of the results.
        /// If any application of func are none, returns None.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="items"></param>
        /// <param name="apply"></param>
        /// <returns></returns>
        public static Option<IEnumerable<U>> Traverse<T, U>(
            IEnumerable<T> items,
            Func<T, Option<U>> apply) =>
            items.Aggregate(
                Option<IEnumerable<U>>.Some(Enumerable.Empty<U>()),
                (state, item) => {
                    if (!state.TryGet(out var currentState)) {
                        return state;
                    }

                    if (!apply(item).TryGet(out var i)) {
                        return Option<IEnumerable<U>>.None();
                    }

                    return Option<IEnumerable<U>>.Some(currentState.Append(i));
                });


        /// <summary>
        /// Sequences the specified options, aggregating the results.
        /// If all options are some, returns a Option containing an IEnumerable of the results.
        /// If any options are none, returns None.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public static Option<IEnumerable<T>> Sequence<T>(IEnumerable<Option<T>> options) =>
            Traverse(options, x => x);

        /// <summary>
        /// Zips two <see cref="Option{T}"/> into a new <see cref="Option{T}"/> containing a tuple of the values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Option<(T, T2)> Zip<T, T2>(Option<T> o1, Option<T2> o2) =>
            o1.Bind(v1 => o2.Map(v2 => (v1, v2)));

        /// <summary>
        /// Zips three <see cref="Option{T}"/> into a new <see cref="Option{T}"/> containing a tuple of the values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <param name="o3"></param>
        /// <returns></returns>
        public static Option<(T, T2, T3)> Zip<T, T2, T3>(Option<T> o1, Option<T2> o2, Option<T3> o3) =>
            o1.Bind(v1 => o2.Bind(v2 => o3.Map(v3 => (v1, v2, v3))));
    }
}
