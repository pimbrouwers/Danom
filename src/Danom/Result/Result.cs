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
    /// Returns true if Result is Ok, false otherwise.
    /// </summary>
    public bool IsOk { get; } = false;

    /// <summary>
    /// Returns true if Result is Error, false otherwise.
    /// </summary>
    public bool IsError => !IsOk;

    /// <summary>
    /// If Result is Ok evaluate the ok delegate, otherwise error.
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
    /// Evaluates the bind delegate if Result is Ok otherwise return Error.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="bind"></param>
    /// <returns></returns>
    public Result<U, TError> Bind<U>(
        Func<T, Result<U, TError>> bind) =>
        Match(bind, Result<U, TError>.Error);

    /// <summary>
    /// Evaluates the map delegate if Result is Ok otherwise return Error.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="map"></param>
    /// <returns></returns>
    public Result<U, TError> Map<U>(
        Func<T, U> map) =>
        Bind(x => Result<U, TError>.Ok(map(x)));

    /// <summary>
    /// Evaluates the mapError delegate if Result is Error otherwise return Ok.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="map"></param>
    /// <returns></returns>
    public Result<T, UError> MapError<UError>(
        Func<TError, UError> mapError) =>
        Match(Result<T, UError>.Ok, e => Result<T, UError>.Error(mapError(e)));

    /// <summary>
    /// Returns the value of Result if it is T, otherwise returns the
    /// specified default value.
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public T DefaultValue(
         T defaultValue) =>
         Match(ok => ok, _ => defaultValue);

    /// <summary>
    /// Returns the value of Result if it is T, otherwise returns the
    /// </summary>
    /// <param name="defaultWith"></param>
    /// <returns></returns>
    public T DefaultWith(
        Func<T> defaultWith) =>
        Match(ok => ok, _ => defaultWith());

    /// <summary>
    /// Creates a new Result with the specified value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Result<T, TError> Ok(T value) =>
        new(value);

    /// <summary>
    /// Creates Result with the specified value wrapped in a completed Task.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Task<Result<T, TError>> OkAsync(T value) =>
        Task.FromResult(Ok(value));

    /// <summary>
    /// Creates Result with the value of the awaited Task.
    /// </summary>
    /// <param name="valueTask"></param>
    /// <returns></returns>
    public static async Task<Result<T, TError>> OkAsync(Task<T> valueTask) =>
        Ok(await valueTask);

    /// <summary>
    /// Creates a new Result with the specified error.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static Result<T, TError> Error(TError errors) =>
        new(errors);

    /// <summary>
    /// Creates Result with the specified error wrapped in a completed Task.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static Task<Result<T, TError>> ErrorAsync(TError errors) =>
        Task.FromResult(Error(errors));

    /// <summary>
    /// Creates Result with the value of the awaited Task.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static async Task<Result<T, TError>> ErrorAsync(Task<TError> errors) =>
        Error(await errors);

    public static bool operator ==(Result<T, TError> left, Result<T, TError> right) =>
        left.Equals(right);

    public static bool operator !=(Result<T, TError> left, Result<T, TError> right) =>
        !(left == right);

    public override bool Equals(object? obj) =>
        obj is Result<T, TError> o && Equals(o);

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

    public override int GetHashCode() =>
        Match(
            ok: x => x is null ? 0 : x.GetHashCode(),
            error: e => e is null ? 0 : e.GetHashCode());

    public override string ToString() =>
        Match(
            ok: x => $"Ok({x})",
            error: e => $"Error({e})");
}
