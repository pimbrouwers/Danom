namespace Danom
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a result of an operation that can be either successful or not. It
    /// is typically used in monadic error handling.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    public readonly struct Result<T, TError>
        : IEquatable<Result<T, TError>>
    {
        private readonly T _ok;
        private readonly TError _error;

        private Result(T t)
        {
            _ok = t;
            IsOk = true;
            _error = default!;
        }

        private Result(TError tError)
        {
            _ok = default!;
            IsOk = false;
            _error = tError;
        }

        /// <summary>
        /// Returns true if <see cref="Result{T, TError}"/> is Ok, false otherwise.
        /// </summary>
        public bool IsOk { get; }

        /// <summary>
        /// Returns true if <see cref="Result{T, TError}"/> is Error, false otherwise.
        /// </summary>
        public bool IsError => !IsOk;

        /// <summary>
        /// If <see cref="Result{T, TError}"/> is Ok evaluate the ok delegate,
        /// otherwise error.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="ok"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public U Match<U>(Func<T, U> ok, Func<TError, U> error) =>
            IsOk && _ok is T t ?
                ok(t) :
                IsError && _error is TError tError ?
                    error(tError) :
                    throw new InvalidOperationException("Result error has not been initialized.");

        /// <summary>
        /// If <see cref="Result{T,TError}"/> is Some, evaluates the some delegate,
        /// otherwise evaluates
        /// the none delegate.
        /// </summary>
        public void Match(Action<T> ok, Action<TError> error)
        {
            Match(
                ok: x =>
                {
                    ok(x);
                    return Unit.Value;
                },
                error: e =>
                {
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
        /// Safely retrieves the value if the Result is in Ok state.
        /// </summary>
        /// <param name="value">The value if in Ok state, default otherwise.</param>
        /// <returns>True if the Result is in Ok state, false otherwise.</returns>
        public bool TryGet(out T value)
        {
            value = IsOk ? _ok : default!;
            return IsOk;
        }

        /// <summary>
        /// Safely retrieves the error if the Result is in Error state.
        /// </summary>
        /// <param name="error">The error if in Error state, default otherwise.</param>
        /// <returns>True if the Result is in Error state, false otherwise.</returns>
        public bool TryGetError(out TError error)
        {
            error = IsError ? _error : default!;
            return IsError;
        }

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
        /// Creates a new <see cref="Result{T, TError}"/> with the specified error.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static Result<T, TError> Error(TError errors) =>
            new Result<T, TError>(errors);

        /// <summary>
        /// Creates <see cref="Result{T, TError}"/> with the value of the awaited Task.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static async Task<Result<T, TError>> ErrorAsync(Task<TError> errors) =>
            Error(await errors);

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
        public bool Equals(Result<T, TError> other) =>
            Match(
                ok: x1 =>
                    other.Match(
                        ok: x2 => x1 != null && x2 != null && x2.Equals(x1),
                        error: _ => false),
                error: e1 =>
                    other.Match(
                        ok: _ => false,
                        error: e2 => e2 != null && e2.Equals(e1)));

        /// <summary>
        /// Returns the hash code for the <see cref="Result{T, TError}"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() =>
            Match(
                ok: x => x is null ? 0 : x.GetHashCode(),
                error: e => e is null ? 0 : e.GetHashCode());

        /// <summary>
        /// Returns a string representation of the <see cref="Result{T, TError}"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString() =>
            Match(
                ok: x => $"Ok({x})",
                error: e => $"Error({e})");
    }


    /// <summary>
    /// The <see cref="Result{T, ResultErrors}"/> with <see cref="ResultErrors"/>
    /// as the predefined error type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Result<T>
    {
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
        /// Creates a new <see cref="Result{T, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static Result<T, ResultErrors> Error(ResultErrors errors) =>
            Result<T, ResultErrors>.Error(errors);

        /// <summary>
        /// Creates a new <see cref="Result{T, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result<T, ResultErrors> Error(string error) =>
            Result<T, ResultErrors>.Error(new ResultErrors(error));

        /// <summary>
        /// Creates a new <see cref="Result{T, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result<T, ResultErrors> Error(string key, string error) =>
            Result<T, ResultErrors>.Error(new ResultErrors(key, error));

        /// <summary>
        /// Creates a new <see cref="Result{T, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static Result<T, ResultErrors> Error(string key, params string[] errors) =>
            Result<T, ResultErrors>.Error(new ResultErrors(key, errors));
    }

    /// <summary>
    /// The <see cref="Result{T, ResultErrors}"/> with <see cref="ResultErrors"/>
    /// as the predefined error type.
    /// </summary>
    public static class Result
    {
        /// <summary>
        /// Creates a new <see cref="Result{Unit, ResultErrors}"/> with <see cref="Unit"/> value.
        /// </summary>
        /// <returns></returns>
        public static Result<Unit, ResultErrors> Ok() =>
            Result<Unit, ResultErrors>.Ok(Unit.Value);

        /// <summary>
        /// Creates a new <see cref="Result{Unit, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static Result<Unit, ResultErrors> Error(ResultErrors errors) =>
            Result<Unit, ResultErrors>.Error(errors);

        /// <summary>
        /// Creates a new <see cref="Result{Unit, ResultErrors}"/> with the specified error.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Result<Unit, ResultErrors> Error(string error) =>
            Result<Unit, ResultErrors>.Error(new ResultErrors(error));

        /// <summary>
        /// Creates a new <see cref="Result{T, ResultErrors}"/> with the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Result<T, ResultErrors> Ok<T>(T value) =>
            Result<T, ResultErrors>.Ok(value);

        /// <summary>
        /// Creates <see cref="Result{T, ResultErrors}"/> with the value of the awaited Task.
        /// </summary>
        /// <param name="valueTask"></param>
        /// <returns></returns>
        public static Task<Result<T, ResultErrors>> OkAsync<T>(Task<T> valueTask) =>
            Result<T, ResultErrors>.OkAsync(valueTask);
    }
}
