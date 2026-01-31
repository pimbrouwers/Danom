namespace Danom {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a result of an operation that can be either successful or not. It
    /// is typically used in monadic error handling.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    public readonly struct Result<T, TError>
        : IEquatable<Result<T, TError>> {
        private readonly byte _state; // 1 = Ok, 2 = Error
        private readonly T _ok;
        private readonly TError _error;

        private Result(T t) {
            _state = 1;
            _ok = t;
            _error = default!;
        }

        private Result(TError tError) {
            _state = 2;
            _ok = default!;
            _error = tError;
        }

        /// <summary>
        /// Returns true if <see cref="Result{T, TError}"/> is Ok, false otherwise.
        /// </summary>
        public bool IsOk => _state == 1;

        /// <summary>
        /// Returns true if <see cref="Result{T, TError}"/> is Error, false otherwise.
        /// </summary>
        public bool IsError => _state == 2;

        /// <summary>
        /// Returns 1 if <see cref="Result{T, TError}"/> is Ok, 0 otherwise.
        /// </summary>
        public int Count => IsOk ? 1 : 0;

        /// <summary>
        /// Creates a new <see cref="Result{T, TError}"/> with the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Result<T, TError> Ok(T value) =>
            new Result<T, TError>(value);

        /// <summary>
        /// Creates <see cref="Result{T, TError}"/> with the value of the awaited Task.
        /// </summary>
        /// <param name="valueTask"></param>
        /// <returns></returns>
        public static async Task<Result<T, TError>> OkAsync(Task<T> valueTask) =>
            Ok(await valueTask);

        /// <summary>
        /// Creates <see cref="Result{T, TError}"/> with the value of the awaited ValueTask.
        /// </summary>
        /// <param name="valueTask"></param>
        /// <returns></returns>
        public static async ValueTask<Result<T, TError>> OkAsync(ValueTask<T> valueTask) =>
            Ok(await valueTask);

        /// <summary>
        /// Creates a new <see cref="Result{T, TError}"/> with the specified error.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result<T, TError> Error(TError error) =>
            new Result<T, TError>(error);

        // must remove this because it creates false positive CA1849 warnings
        // public static Task<Result<T, TError>> ErrorAsync(TError error) =>
        //     Task.FromResult(Error(error));

        /// <summary>
        /// Creates <see cref="Result{T, TError}"/> with the value of the awaited Task.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static async Task<Result<T, TError>> ErrorAsync(Task<TError> error) =>
            Error(await error);

        /// <summary>
        /// Creates <see cref="Result{T, TError}"/> with the value of the awaited ValueTask.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static async ValueTask<Result<T, TError>> ErrorAsync(ValueTask<TError> error) =>
            Error(await error);

        /// <summary>
        /// If <see cref="Result{T, TError}"/> is Ok evaluate the ok delegate,
        /// otherwise error.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="ok"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public U Match<U>(Func<T, U> ok, Func<TError, U> error) => _state switch {
            1 when _ok is T t => ok(t),
            2 when _error is TError tError => error(tError),
            _ => throw new InvalidOperationException("Result error has not been initialized."),
        };

        /// <summary>
        /// If <see cref="Result{T,TError}"/> is Some, evaluates the some delegate,
        /// otherwise evaluates
        /// the none delegate.
        /// </summary>
        public void Match(Action<T> ok, Action<TError> error) {
            Match(
                ok: x => {
                    ok(x);
                    return Unit.Value;
                },
                error: e => {
                    error(e);
                    return Unit.Value;
                });
        }

        /// <summary>
        /// Evaluates the bind delegate if <see cref="Result{T, TError}"/> is Ok otherwise return Error.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="bind"></param>
        /// <returns></returns>
        public Result<U, TError> Bind<U>(
            Func<T, Result<U, TError>> bind) =>
            Match(bind, Result<U, TError>.Error);

        /// <summary>
        /// Evaluates the map delegate if <see cref="Result{T, TError}"/> is Ok otherwise return Error.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="map"></param>
        /// <returns></returns>
        public Result<U, TError> Map<U>(
            Func<T, U> map) =>
            Bind(x => Result<U, TError>.Ok(map(x)));

        /// <summary>
        /// Evaluates the mapError delegate if <see cref="Result{T, TError}"/> is Error otherwise return Ok.
        /// </summary>
        /// <typeparam name="UError"></typeparam>
        /// <param name="mapError"></param>
        /// <returns></returns>
        public Result<T, UError> MapError<UError>(
            Func<TError, UError> mapError) =>
            Match(Result<T, UError>.Ok, e => Result<T, UError>.Error(mapError(e)));

        /// <summary>
        /// Applies the function contained in the Result to the value contained in this Result if both are Ok.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="fn"></param>
        /// <returns></returns>
        public Result<U, TError> Apply<U>(
            Result<Func<T, U>, TError> fn) =>
            Bind(x => fn.Map(i => i.Invoke(x)));

        /// <summary>
        /// Executes the some action if <see cref="Result{T, TError}"/> is Ok.
        /// </summary>
        /// <param name="some"></param>
        public void Iter(
            Action<T> some) =>
            Match(some, _ => { });

        /// <summary>
        /// Returns the value of <see cref="Result{T, TError}"/> if it is T, otherwise returns the
        /// specified default value.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T DefaultValue(
             T defaultValue) =>
             Match(ok => ok, _ => defaultValue);

        /// <summary>
        /// Returns the value of <see cref="Result{T, TError}"/> if it is T, otherwise returns the
        /// </summary>
        /// <param name="defaultWith"></param>
        /// <returns></returns>
        public T DefaultWith(
            Func<T> defaultWith) =>
            Match(ok => ok, _ => defaultWith());

        /// <summary>
        /// Returns this Result if it is Ok, otherwise returns the specified ifError Result.
        /// </summary>
        /// <param name="ifError"></param>
        /// <returns></returns>
        public Result<T, TError> OrElse(
            Result<T, TError> ifError) =>
            Match(Result<T, TError>.Ok, _ => ifError);

        /// <summary>
        /// Returns this Result if it is Ok, otherwise evaluates and returns the specified ifError Result.
        /// </summary>
        /// <param name="ifErrorWith"></param>
        /// <returns></returns>
        public Result<T, TError> OrElseWith(
            Func<Result<T, TError>> ifErrorWith) =>
            Match(Result<T, TError>.Ok, _ => ifErrorWith());

        /// <summary>
        /// Safely retrieves the value if the Result is in Ok state.
        /// </summary>
        /// <param name="value">The value if in Ok state, default otherwise.</param>
        /// <returns>True if the Result is in Ok state, false otherwise.</returns>
        public bool TryGet([MaybeNullWhen(false)] out T value) {
            value = IsOk ? _ok : default!;
            return IsOk;
        }

        /// <summary>
        /// Safely retrieves the error if the Result is in Error state.
        /// </summary>
        /// <param name="error">The error if in Error state, default otherwise.</param>
        /// <returns>True if the Result is in Error state, false otherwise.</returns>
        public bool TryGetError([MaybeNullWhen(false)] out TError error) {
            error = IsError ? _error : default!;
            return IsError;
        }

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
        /// Returns true if the specified Result is equal to the current <see cref="Result{T, TError}"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Result<T, TError> left, Result<T, TError> right) =>
            left.Equals(right);

        /// <summary>
        /// Returns true if the specified Result is not equal to the current <see cref="Result{T, TError}"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Result<T, TError> left, Result<T, TError> right) =>
            !(left == right);

        /// <summary>
        /// Returns true if the specified Result is equal to the current <see cref="Result{T, TError}"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) =>
            obj is Result<T, TError> o && Equals(o);

        /// <summary>
        /// Returns true if the specified Result is equal to the current <see cref="Result{T, TError}"/>.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Result<T, TError> other) {
            if (_state != other._state) {
                return false;
            }

            if (_state == 0 && other._state == 0) {
                return true;
            }

            return _state switch {
                1 => _ok is null ? other._ok is null : _ok.Equals(other._ok),
                2 => _error is null ? other._error is null : _error.Equals(other._error),
                _ => false
            };

        }

        /// <summary>
        /// Converts the <see cref="Result{T, TError}"/> to an <see cref="Option{T}"/>.
        /// If the Result is Ok, it returns Some with the value, otherwise None.
        /// </summary>
        /// <returns></returns>
        public Option<T> ToOption() =>
            Match(
                ok: x => Option.Some(x),
                error: _ => Option<T>.NoneValue);

        /// <summary>
        /// Returns the hash code for the <see cref="Result{T, TError}"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => _state switch {
            1 => _ok is null ? 0 : _ok.GetHashCode(),
            2 => _error is null ? 0 : _error.GetHashCode(),
            _ => 0
        };

        /// <summary>
        /// Returns a string representation of the <see cref="Result{T, TError}"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => _state switch {
            1 => IsOk && _ok is T okVal ? $"Ok({okVal})" : "Ok()",
            2 => IsError && _error is TError errVal
                ? (errVal is null ? "Error()" : $"Error({errVal})")
                : "Error()",
            _ => "Error()"
        };
    }


    /// <summary>
    /// The <see cref="Result{T, ResultErrors}"/> with <see cref="ResultErrors"/>
    /// as the predefined error type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Result<T> {
        /// <summary>
        /// Creates a new <see cref="Result{T, ResultErrors}"/> with the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Result<T, ResultErrors> Ok(T value) =>
            Result<T, ResultErrors>.Ok(value);

        /// <summary>
        /// Creates <see cref="Result{T, ResultErrors}"/> with the value of the awaited Task.
        /// </summary>
        /// <param name="valueTask"></param>
        /// <returns></returns>
        public static Task<Result<T, ResultErrors>> OkAsync(Task<T> valueTask) =>
            Result<T, ResultErrors>.OkAsync(valueTask);

        /// <summary>
        /// Creates <see cref="Result{T, ResultErrors}"/> with the value of the awaited ValueTask.
        /// </summary>
        /// <param name="valueTask"></param>
        /// <returns></returns>
        public static ValueTask<Result<T, ResultErrors>> OkAsync(ValueTask<T> valueTask) =>
            Result<T, ResultErrors>.OkAsync(valueTask);

        /// <summary>
        /// Creates a new <see cref="Result{T, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static Result<T, ResultErrors> Error(ResultErrors errors) =>
            Result<T, ResultErrors>.Error(errors);

        /// <summary>
        /// Creates <see cref="Result{T, ResultErrors}"/> with the value of the awaited Task.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static Task<Result<T, ResultErrors>> ErrorAsync(Task<ResultErrors> errors) =>
            Result<T, ResultErrors>.ErrorAsync(errors);

        /// <summary>
        /// Creates <see cref="Result{T, ResultErrors}"/> with the value of the awaited ValueTask.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static ValueTask<Result<T, ResultErrors>> ErrorAsync(ValueTask<ResultErrors> errors) =>
            Result<T, ResultErrors>.ErrorAsync(errors);

        /// <summary>
        /// Create <see cref="Result{T, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static Result<T, ResultErrors> Error(string key, params string[] errors) =>
            Error(new ResultErrors(key, errors));

        /// <summary>
        /// Creates a new <see cref="Result{T, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result<T, ResultErrors> Error(string key, string error) =>
            Error(new ResultErrors(key, error));

        /// <summary>
        /// Create <see cref="Result{T, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result<T, ResultErrors> Error(string error) =>
            Error(new ResultErrors(error));

        /// <summary>
        /// Create <see cref="Result{T, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static Result<T, ResultErrors> Error(params string[] errors) =>
            Error(new ResultErrors(errors));
    }

    /// <summary>
    /// The <see cref="Result{T, ResultErrors}"/> with <see cref="ResultErrors"/>
    /// as the predefined error type.
    /// </summary>
    public static class Result {
        /// <summary>
        /// Creates a new <see cref="Result{Unit, ResultErrors}"/> with <see cref="Unit"/> value.
        /// </summary>
        /// <returns></returns>
        public static Result<Unit, ResultErrors> Ok() =>
            Result<Unit, ResultErrors>.Ok(Unit.Value);

        /// <summary>
        /// Creates a new <see cref="Result{T, ResultErrors}"/> with the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Result<T, ResultErrors> Ok<T>(T value) =>
            Result<T, ResultErrors>.Ok(value);

        /// <summary>
        /// Creates a new <see cref="Result{Unit, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static Result<Unit, ResultErrors> Error(ResultErrors errors) =>
            Result<Unit, ResultErrors>.Error(errors);

        /// <summary>
        /// Create <see cref="Result{Unit, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static Result<Unit, ResultErrors> Error(string key, params string[] errors) =>
            Error(new ResultErrors(key, errors));

        /// <summary>
        /// Creates a new <see cref="Result{Unit, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result<Unit, ResultErrors> Error(string key, string error) =>
            Error(new ResultErrors(key, error));

        /// <summary>
        /// Create <see cref="Result{Unit, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result<Unit, ResultErrors> Error(string error) =>
            Error(new ResultErrors(error));

        /// <summary>
        /// Create <see cref="Result{Unit, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static Result<Unit, ResultErrors> Error(params string[] errors) =>
            Error(new ResultErrors(errors));

        /// <summary>
        /// Matches two Results and invokes the appropriate delegate based on their states.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T2Error"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="defaultWith"></param>
        /// <param name="ok"></param>
        /// <param name="error"></param>
        /// <param name="ok1"></param>
        /// <param name="ok2"></param>
        /// <param name="error1"></param>
        /// <param name="error2"></param>
        /// <returns></returns>
        public static U Match<T, TError, T2, T2Error, U>(
            Result<T, TError> r1,
            Result<T2, T2Error> r2,
            Func<U> defaultWith,
            Func<T, T2, U>? ok = default,
            Func<TError, T2Error, U>? error = default,
            Func<T, U>? ok1 = default,
            Func<T2, U>? ok2 = default,
            Func<TError, U>? error1 = default,
            Func<T2Error, U>? error2 = default) {
            if (ok != null && r1.TryGet(out var v1) && r2.TryGet(out var v2)) {
                return ok(v1, v2);
            }

            if (error != null && r1.TryGetError(out var e1) && r2.TryGetError(out var e2)) {
                return error(e1, e2);
            }

            if (ok1 != null && r1.TryGet(out v1)) {
                return ok1(v1);
            }

            if (error1 != null && r1.TryGetError(out e1)) {
                return error1(e1);
            }

            if (ok2 != null && r2.TryGet(out v2)) {
                return ok2(v2);
            }

            if (error2 != null && r2.TryGetError(out e2)) {
                return error2(e2);
            }

            return defaultWith();
        }

        /// <summary>
        /// Matches a collection of Results and invokes the appropriate delegate based on their states.
        ///
        /// Note: this implementation is applicative and not monadic, meaning that all Results are evaluated.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="results"></param>
        /// <param name="ok"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static U Match<T, TError, U>(
            IEnumerable<Result<T, TError>> results,
            Func<IEnumerable<T>, U> ok,
            Func<IEnumerable<TError>, U> error) {
            var values = new List<T>();
            var errors = new List<TError>();
            foreach (var result in results) {
                if (result.TryGet(out var v)) {
                    values.Add(v);
                }
                else if (result.TryGetError(out var e)) {
                    errors.Add(e);
                }
            }

            if (errors.Count > 0) {
                return error(errors);
            }

            return ok(values);
        }

        /// <summary>
        /// Binds two Results together using the specified bind function if both are Ok.
        /// If either Result is Error, and no error aggregator is provider, returns the first encountered Error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="bind"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Result<U, TError> Bind<T, T2, TError, U>(
            Result<T, TError> r1,
            Result<T2, TError> r2,
            Func<T, T2, Result<U, TError>> bind,
            Func<TError, TError, TError>? error = default) {
            if (r1.TryGet(out var v1) && r2.TryGet(out var v2)) {
                return bind(v1, v2);
            }

            if (error != null && r1.TryGetError(out var e1) && r2.TryGetError(out var e2)) {
                return Result<U, TError>.Error(error(e1, e2));
            }

            if (r1.TryGetError(out e1)) {
                return Result<U, TError>.Error(e1);
            }

            if (r2.TryGetError(out e2)) {
                return Result<U, TError>.Error(e2);
            }

            // unreachable, here to satisfy the compiler
            throw new InvalidOperationException("You'll never find me!");
        }

        /// <summary>
        /// Binds three Results together using the specified bind function if all are Ok.
        /// If any Result is Error, and no error aggregator is provider, returns the first encountered Error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="r3"></param>
        /// <param name="bind"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Result<U, TError> Bind<T, T2, T3, TError, U>(
            Result<T, TError> r1,
            Result<T2, TError> r2,
            Result<T3, TError> r3,
            Func<T, T2, T3, Result<U, TError>> bind,
            Func<TError, TError, TError, TError>? error = default) {
            if (r1.TryGet(out var v1)
                && r2.TryGet(out var v2)
                && r3.TryGet(out var v3)) {
                return bind(v1, v2, v3);
            }

            if (error != null
                && r1.TryGetError(out var e1)
                && r2.TryGetError(out var e2)
                && r3.TryGetError(out var e3)) {
                return Result<U, TError>.Error(error(e1, e2, e3));
            }

            if (r1.TryGetError(out e1)) {
                return Result<U, TError>.Error(e1);
            }

            if (r2.TryGetError(out e2)) {
                return Result<U, TError>.Error(e2);
            }

            if (r3.TryGetError(out e3)) {
                return Result<U, TError>.Error(e3);
            }

            // unreachable, here to satisfy the compiler
            throw new InvalidOperationException("You'll never find me!");
        }

        /// <summary>
        /// Maps two Results together using the specified map function if both are Ok.
        /// If either Result is Error, and no error aggregator is provider, returns the first encountered Error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="map"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result<U, TError> Map<T, T2, TError, U>(
            Result<T, TError> r1,
            Result<T2, TError> r2,
            Func<T, T2, U> map,
            Func<TError, TError, TError>? error = default) {
            return Bind(r1, r2, (v1, v2) => Result<U, TError>.Ok(map(v1, v2)), error);
        }

        /// <summary>
        /// Maps three Results together using the specified map function if all are Ok.
        /// If any Result is Error, and no error aggregator is provider, returns the first encountered Error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="r1"></param>
        /// <param name="r2"></param>
        /// <param name="r3"></param>
        /// <param name="map"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result<U, TError> Map<T, T2, T3, TError, U>(
            Result<T, TError> r1,
            Result<T2, TError> r2,
            Result<T3, TError> r3,
            Func<T, T2, T3, U> map,
            Func<TError, TError, TError, TError>? error = default) {
            return Bind(r1, r2, r3, (v1, v2, v3) => Result<U, TError>.Ok(map(v1, v2, v3)), error);
        }

        /// <summary>
        /// Traverses the specified items, applying the func to each item and aggregating the results.
        /// If all applications of func are successful, returns a Result containing an IEnumerable of the results.
        /// If any application of func fails, returns a Result containing the aggregated error using errorFunc.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="items"></param>
        /// <param name="func"></param>
        /// <param name="errorFunc"></param>
        /// <returns></returns>
        public static Result<IEnumerable<U>, TError> Traverse<T, TError, U>(
            IEnumerable<T> items,
            Func<T, Result<U, TError>> func,
            Func<TError, TError, TError> errorFunc) =>
            items.Aggregate(
                Result<IEnumerable<U>, TError>.Ok(Enumerable.Empty<U>()),
                (acc, x) => {
                    var funcResult = func(x);

                    if (acc.TryGet(out var a)
                        && funcResult.TryGet(out var i)) {
                        return Result<IEnumerable<U>, TError>.Ok(a.Concat(new[] { i }));
                    }

                    if (funcResult.TryGetError(out var iErr)
                        && acc.TryGetError(out var aErr)) {
                        return Result<IEnumerable<U>, TError>.Error(errorFunc(aErr, iErr));
                    }

                    if (funcResult.TryGetError(out iErr)) {
                        return Result<IEnumerable<U>, TError>.Error(iErr);
                    }

                    return acc;
                });

        /// <summary>
        /// Traverses the specified items, applying the func to each item and aggregating the results.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Result<IEnumerable<T>, ResultErrors> Traverse<T>(
            IEnumerable<T> items,
            Func<T, Result<T, ResultErrors>> func) =>
            Traverse(items, func, (e1, e2) => ResultErrors.Merge(e1, e2));

        /// <summary>
        /// Sequences the specified results, aggregating the successful values or errors.
        /// If all results are successful, returns a Result containing an IEnumerable of the values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="results"></param>
        /// <param name="errorFunc"></param>
        /// <returns></returns>
        public static Result<IEnumerable<T>, TError> Sequence<T, TError>(
            IEnumerable<Result<T, TError>> results,
            Func<TError, TError, TError> errorFunc) =>
            Traverse(results, x => x, errorFunc);


        /// <summary>
        /// Sequences the specified results, aggregating the successful values or errors.
        /// If all results are successful, returns a Result containing an IEnumerable of the values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results"></param>
        /// <returns></returns>
        public static Result<IEnumerable<T>, ResultErrors> Sequence<T>(
            IEnumerable<Result<T, ResultErrors>> results) =>
            Sequence(results, (e1, e2) => ResultErrors.Merge(e1, e2));

    }
}
