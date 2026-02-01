namespace Danom {
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a choice between four types, T1, T2, T3 and T4.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="T4"></typeparam>
    /// <typeparam name="T5"></typeparam>
    public readonly struct Choice<T1, T2, T3, T4, T5> : IEquatable<Choice<T1, T2, T3, T4, T5>> {
        private readonly byte _state; // 1 = T1, 2 = T2, 3 = T3, 4 = T4, 5 = T5
        private readonly T1 _t1;
        private readonly T2 _t2;
        private readonly T3 _t3;
        private readonly T4 _t4;
        private readonly T5 _t5;

        private Choice(T1 t1) {
            _state = 1;
            _t1 = t1;
            _t2 = default!;
            _t3 = default!;
            _t4 = default!;
            _t5 = default!;
        }

        private Choice(T2 t2) {
            _state = 2;
            _t2 = t2;
            _t1 = default!;
            _t3 = default!;
            _t4 = default!;
            _t5 = default!;
        }

        private Choice(T3 t3) {
            _state = 3;
            _t3 = t3;
            _t1 = default!;
            _t2 = default!;
            _t4 = default!;
            _t5 = default!;
        }

        private Choice(T4 t4) {
            _state = 4;
            _t4 = t4;
            _t1 = default!;
            _t2 = default!;
            _t3 = default!;
            _t5 = default!;
        }

        private Choice(T5 t5) {
            _state = 5;
            _t5 = t5;
            _t1 = default!;
            _t2 = default!;
            _t3 = default!;
            _t4 = default!;
        }

        /// <summary>
        /// Returns true if <see cref="Choice{T1, T2, T3, T4, T5}"/> is T1, false otherwise.
        /// </summary>
        public bool IsT1 => _state == 1;

        /// <summary>
        /// Returns true if <see cref="Choice{T1, T2, T3, T4, T5}"/> is T2, false otherwise.
        /// </summary>
        public bool IsT2 => _state == 2;

        /// <summary>
        /// Returns true if <see cref="Choice{T1, T2, T3, T4, T5}"/> is T3, false otherwise.
        /// </summary>
        public bool IsT3 => _state == 3;

        /// <summary>
        /// Returns true if <see cref="Choice{T1, T2, T3, T4, T5}"/> is T4, false otherwise.
        /// </summary>
        public bool IsT4 => _state == 4;

        /// <summary>
        /// Returns true if <see cref="Choice{T1, T2, T3, T4, T5}"/> is T5, false otherwise.
        /// </summary>
        public bool IsT5 => _state == 5;

        /// <summary>
        /// If <see cref="Choice{T1, T2, T3, T4, T5}"/> is T1 evaluate the t1 delegate,
        /// otherwise evaluate the t2 delegate and return its result.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        /// <returns></returns>
        public U Match<U>(Func<T1, U> t1, Func<T2, U> t2, Func<T3, U> t3, Func<T4, U> t4, Func<T5, U> t5) => _state switch {
            1 when _t1 is T1 xT1 => t1(xT1),
            2 when _t2 is T2 xT2 => t2(xT2),
            3 when _t3 is T3 xT3 => t3(xT3),
            4 when _t4 is T4 xT4 => t4(xT4),
            5 when _t5 is T5 xT5 => t5(xT5),
            _ => throw new InvalidOperationException("Choice is in an invalid state: neither T1 nor T2 is initialized."),
        };

        /// <summary>
        /// If <see cref="Choice{T1,T2}"/> is T1, evaluates the t1 delegate,
        /// otherwise evaluates the t2 delegate and ignores its result.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        public void Match(Action<T1> t1, Action<T2> t2, Action<T3> t3, Action<T4> t4, Action<T5> t5) {
            Match(
                t1: x => {
                    t1(x);
                    return Unit.Value;
                },
                t2: y => {
                    t2(y);
                    return Unit.Value;
                },
                t3: z => {
                    t3(z);
                    return Unit.Value;
                },
                t4: w => {
                    t4(w);
                    return Unit.Value;
                },
                t5: v => {
                    t5(v);
                    return Unit.Value;
                });
        }

        /// <summary>
        /// Safely retrieves the value if the Choice is in T1 state.
        /// </summary>
        /// <param name="value">The value if in T1 state, default otherwise.</param>
        /// <returns>True if the Choice is in T1 state, false otherwise.</returns>
        public bool TryGetT1([MaybeNullWhen(false)] out T1 value) {
            value = IsT1 ? _t1 : default!;
            return IsT1;
        }

        /// <summary>
        /// Safely retrieves the value if the Choice is in T2 state.
        /// </summary>
        /// <param name="value">The value if in T2 state, default otherwise.</param>
        /// <returns>True if the Choice is in T2 state, false otherwise.</returns>
        public bool TryGetT2([MaybeNullWhen(false)] out T2 value) {
            value = IsT2 ? _t2 : default!;
            return IsT2;
        }

        /// <summary>
        /// Safely retrieves the value if the Choice is in T3 state.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetT3([MaybeNullWhen(false)] out T3 value) {
            value = IsT3 ? _t3 : default!;
            return IsT3;
        }

        /// <summary>
        /// Safely retrieves the value if the Choice is in T4 state.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetT4([MaybeNullWhen(false)] out T4 value) {
            value = IsT4 ? _t4 : default!;
            return IsT4;
        }

        /// <summary>
        /// Safely retrieves the value if the Choice is in T5 state.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetT5([MaybeNullWhen(false)] out T5 value) {
            value = IsT5 ? _t5 : default!;
            return IsT5;
        }

        /// <summary>
        /// Creates a new <see cref="Choice{T1, T2, T3, T4, T5}"/> from the specified T1 value.
        /// </summary>
        /// <param name="t1"></param>
        /// <returns></returns>
        public static Choice<T1, T2, T3, T4, T5> FromT1(T1 t1) =>
            new Choice<T1, T2, T3, T4, T5>(t1);

        /// <summary>
        /// Creates a new <see cref="Choice{T1, T2, T3, T4, T5}"/> from the specified T2 value.
        /// </summary>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static Choice<T1, T2, T3, T4, T5> FromT2(T2 t2) =>
            new Choice<T1, T2, T3, T4, T5>(t2);

        /// <summary>
        /// Creates a new <see cref="Choice{T1, T2, T3, T4, T5}"/> from the specified T3 value.
        /// </summary>
        /// <param name="t3"></param>
        /// <returns></returns>
        public static Choice<T1, T2, T3, T4, T5> FromT3(T3 t3) =>
            new Choice<T1, T2, T3, T4, T5>(t3);

        /// <summary>
        /// Creates a new <see cref="Choice{T1, T2, T3, T4, T5}"/> from the specified T4 value.
        /// </summary>
        /// <param name="t4"></param>
        /// <returns></returns>
        public static Choice<T1, T2, T3, T4, T5> FromT4(T4 t4) =>
            new Choice<T1, T2, T3, T4, T5>(t4);

        /// <summary>
        /// Creates a new <see cref="Choice{T1, T2, T3, T4, T5}"/> from the specified T5 value.
        /// </summary>
        /// <param name="t5"></param>
        /// <returns></returns>
        public static Choice<T1, T2, T3, T4, T5> FromT5(T5 t5) =>
            new Choice<T1, T2, T3, T4, T5>(t5);

        /// <summary>
        /// Returns true if the specified <see cref="Choice{T1, T2, T3, T4, T5}"/>s are equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Choice<T1, T2, T3, T4, T5> left, Choice<T1, T2, T3, T4, T5> right) =>
            left.Equals(right);

        /// <summary>
        /// Returns true if the specified <see cref="Choice{T1, T2, T3, T4, T5}"/>s are not equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Choice<T1, T2, T3, T4, T5> left, Choice<T1, T2, T3, T4, T5> right) =>
            !(left == right);

        /// <summary>
        /// Returns true if the specified object is equal to the current <see cref="Choice{T1, T2, T3, T4, T5}"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) =>
            obj is Choice<T1, T2, T3, T4, T5> choice && Equals(choice);

        /// <summary>
        /// Returns true if the specified Choice is equal to the current <see cref="Choice{T1, T2, T3, T4, T5}"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Choice<T1, T2, T3, T4, T5> other) {
            if (_state != other._state) {
                return false;
            }

            if (_state == 0 && other._state == 0) {
                return true;
            }

            return _state switch {
                1 => _t1 is null ? other._t1 is null : _t1.Equals(other._t1),
                2 => _t2 is null ? other._t2 is null : _t2.Equals(other._t2),
                3 => _t3 is null ? other._t3 is null : _t3.Equals(other._t3),
                4 => _t4 is null ? other._t4 is null : _t4.Equals(other._t4),
                5 => _t5 is null ? other._t5 is null : _t5.Equals(other._t5),
                _ => false
            };
        }


        /// <summary>
        /// Returns the hash code for the <see cref="Choice{T1, T2, T3, T4, T5}"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            // Map default state (0) to a stable tag (e.g., 2) to keep hashes consistent
            var tag = _state == 0 ? (byte)2 : _state;
            return _state switch {
                1 => HashCode.Combine(tag, _t1),
                2 => HashCode.Combine(tag, _t2),
                3 => HashCode.Combine(tag, _t3),
                4 => HashCode.Combine(tag, _t4),
                5 => HashCode.Combine(tag, _t5),
                _ => HashCode.Combine(tag)
            };
        }

        /// <summary>
        /// Returns a string representation of the <see cref="Choice{T1, T2, T3, T4, T5}"/>.
        /// </summary>
        public override string ToString() => _state switch {
            1 => _t1 is null ? "T1()" : $"T1({_t1})",
            2 => _t2 is null ? "T2()" : $"T2({_t2})",
            3 => _t3 is null ? "T3()" : $"T3({_t3})",
            4 => _t4 is null ? "T4()" : $"T4({_t4})",
            5 => _t5 is null ? "T5()" : $"T5({_t5})",
            _ => "Choice()"
        };

        /// <summary>
        /// Deconstructs the Choice into its components.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <param name="t5"></param>
        public void Deconstruct(out short state, out T1 t1, out T2 t2, out T3 t3, out T4 t4, out T5 t5) {
            state = _state;
            t1 = _t1;
            t2 = _t2;
            t3 = _t3;
            t4 = _t4;
            t5 = _t5;
        }
    }
}
