namespace Danom;

/// <summary>
/// Represents a result of an operation that can be either successful or not. It
/// is typically used in monadic error handling.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TError"></typeparam>
public readonly struct Result<T, TError>
    : IEquatable<Result<T, TError>>
{
    private readonly T? _ok = default;
    private readonly TError? _error = default;

    private Result(T t)
    {
        _ok = t;
        IsOk = true;
    }

    private Result(TError tError)
    {
        _error = tError;
    }

    /// <summary>
    /// Returns true if <see cref="Result{T, TError}"/> is Ok, false otherwise.
    /// </summary>
    public bool IsOk { get; } = false;

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
    /// Creates a new <see cref="Result{T, TError}"/> with the specified value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Result<T, TError> Ok(T value) =>
        new(value);

    /// <summary>
    /// Creates <see cref="Result{T, TError}"/> with the specified value wrapped in a completed Task.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Task<Result<T, TError>> OkAsync(T value) =>
        Task.FromResult(Ok(value));

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
        new(errors);

    /// <summary>
    /// Creates <see cref="Result{T, TError}"/> with the specified error wrapped in a completed Task.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static Task<Result<T, TError>> ErrorAsync(TError errors) =>
        Task.FromResult(Error(errors));

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
    public readonly bool Equals(Result<T, TError> other) =>
        Match(
            ok: x1 =>
                other.Match(
                    ok: x2 => x1 is not null &&  x2 is not null && x2.Equals(x1),
                    error: _ => false),
            error: e1 =>
                other.Match(
                    ok: _ => false,
                    error: e2 => e2 is not null && e2.Equals(e1)));

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
    /// Creates <see cref="Result{T, ResultErrors}"/> with the specified value wrapped in a completed Task.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Task<Result<T, ResultErrors>> OkAsync(T value) =>
        Result<T, ResultErrors>.OkAsync(value);

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
    /// Creates <see cref="Result{T, ResultErrors}"/> with the specified error wrapped in a completed Task.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static Task<Result<T, ResultErrors>> ErrorAsync(ResultErrors errors) =>
        Task.FromResult(Error(errors));

    /// <summary>
    /// Creates a new <see cref="Result{T, ResultErrors}"/> with the specified error.
    /// </summary>
    /// <param name="messages"></param>
    /// <returns></returns>
    public static Result<T, ResultErrors> Error(IEnumerable<string> messages) =>
        Error(new ResultErrors(messages));

    /// <summary>
    /// Creates <see cref="Result{T, ResultErrors}"/> with the specified error wrapped in a completed Task.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Result<T, ResultErrors> Error(string message) =>
        Error([message]);

    /// <summary>
    /// Creates <see cref="Result{T, ResultErrors}"/> with the specified error wrapped in a completed Task.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Task<Result<T, ResultErrors>> ErrorAsync(string message) =>
        Task.FromResult(Error(message));
}
