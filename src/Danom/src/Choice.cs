namespace Danom {
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents a choice between two types, T1 and T2.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public readonly struct Choice<T1, T2> : IEquatable<Choice<T1, T2>> {
        private readonly byte _state; // 1 = T1, 2 = T2
        private readonly T1 _t1;
        private readonly T2 _t2;

        private Choice(T1 t1) {
            _state = 1;
            _t1 = t1;
            _t2 = default!;
        }

        private Choice(T2 t2) {
            _state = 2;
            _t1 = default!;
            _t2 = t2;
        }

        /// <summary>
        /// Returns true if <see cref="Choice{T1, T2}"/> is T1, false otherwise.
        /// </summary>
        public bool IsT1 => _state == 1;

        /// <summary>
        /// Returns true if <see cref="Choice{T1, T2}"/> is T2, false otherwise.
        /// </summary>
        public bool IsT2 => _state == 2;

        /// <summary>
        /// If <see cref="Choice{T1, T2}"/> is T1 evaluate the t1 delegate,
        /// otherwise evaluate the t2 delegate and return its result.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        public U Match<U>(Func<T1, U> t1, Func<T2, U> t2) => _state switch {
            1 when _t1 is T1 xT1 => t1(xT1),
            2 when _t2 is T2 xT2 => t2(xT2),
            _ => throw new InvalidOperationException("Choice is in an invalid state: neither T1 nor T2 is initialized."),
        };

        /// <summary>
        /// If <see cref="Choice{T1,T2}"/> is T1, evaluates the t1 delegate,
        /// otherwise evaluates the t2 delegate and ignores its result.
        /// </summary>
        public void Match(Action<T1> t1, Action<T2> t2) {
            Match(
                t1: x => {
                    t1(x);
                    return Unit.Value;
                },
                t2: e => {
                    t2(e);
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
        /// Creates a new <see cref="Choice{T1, T2}"/> from the specified T1 value.
        /// </summary>
        /// <param name="t1"></param>
        /// <returns></returns>
        public static Choice<T1, T2> FromT1(T1 t1) =>
            new Choice<T1, T2>(t1);

        /// <summary>
        /// Creates a new <see cref="Choice{T1, T2}"/> from the specified T2 value.
        /// </summary>
        /// <param name="t2"></param>
        /// <returns></returns>
        public static Choice<T1, T2> FromT2(T2 t2) =>
            new Choice<T1, T2>(t2);

        /// <summary>
        /// Returns true if the specified <see cref="Choice{T1, T2}"/>s are equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Choice<T1, T2> left, Choice<T1, T2> right) =>
            left.Equals(right);

        /// <summary>
        /// Returns true if the specified <see cref="Choice{T1, T2}"/>s are not equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Choice<T1, T2> left, Choice<T1, T2> right) =>
            !(left == right);

        /// <summary>
        /// Returns true if the specified object is equal to the current <see cref="Choice{T1, T2}"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) =>
            obj is Choice<T1, T2> choice && Equals(choice);

        /// <summary>
        /// Returns true if the specified Choice is equal to the current <see cref="Choice{T1, T2}"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Choice<T1, T2> other) {
            if (_state != other._state) {
                return false;
            }

            if (_state == 0 && other._state == 0) {
                return true;
            }

            return _state switch {
                1 => _t1 is null ? other._t1 is null : _t1.Equals(other._t1),
                2 => _t2 is null ? other._t2 is null : _t2.Equals(other._t2),
                _ => false
            };
        }


        /// <summary>
        /// Returns the hash code for the <see cref="Choice{T1, T2}"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            // Map default state (0) to a stable tag (e.g., 2) to keep hashes consistent
            var tag = _state == 0 ? (byte)2 : _state;
            return _state switch {
                1 => HashCode.Combine(tag, _t1),
                2 => HashCode.Combine(tag, _t2),
                _ => HashCode.Combine(tag)
            };
        }

        /// <summary>
        /// Returns a string representation of the <see cref="Choice{T1, T2}"/>.
        /// </summary>
        public override string ToString() => _state switch {
            1 => _t1 is null ? "T1()" : $"T1({_t1})",
            2 => _t2 is null ? "T2()" : $"T2({_t2})",
            _ => "Choice()"
        };

        /// <summary>
        /// Deconstructs the Choice into its components.
        /// </summary>
        /// <param name="state"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        public void Deconstruct(out short state, out T1 t1, out T2 t2) {
            state = _state;
            t1 = _t1;
            t2 = _t2;
        }
    }
}
