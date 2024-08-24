namespace Danom;

/// <summary>
/// Represents a result of an operation that can be either successful or not. It
/// is typically used in monadic error handling.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TError"></typeparam>
public interface IResult<T, TError>
{
    bool IsOk { get; }
    bool IsError { get; }
    U Match<U>(Func<T, U> ok, Func<TError, U> error);
    IResult<U, TError> Bind<U>(Func<T, IResult<U, TError>> bind);
    IResult<U, TError> Map<U>(Func<T, U> map);
    IResult<T, UError> MapError<UError>(Func<TError, UError> mapError);
    T DefaultValue(T defaultValue);
    T DefaultWith(Func<T> defaultWith);
}

/// <inheritdoc />
public readonly struct Result<T, TError>
    : IResult<T, TError>
{
    private readonly T? _ok;
    private readonly TError? _error;

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
    public bool IsOk { get; }

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
    public IResult<U, TError> Bind<U>(
        Func<T, IResult<U, TError>> bind) =>
        Match(bind, Result<U, TError>.Error);

    /// <summary>
    /// Evaluates the map delegate if Result is Ok otherwise return Error.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="map"></param>
    /// <returns></returns>
    public IResult<U, TError> Map<U>(
        Func<T, U> map) =>
        Bind(x => Result<U, TError>.Ok(map(x)));

    /// <summary>
    /// Evaluates the mapError delegate if Result is Error otherwise return Ok.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="map"></param>
    /// <returns></returns>
    public IResult<T, UError> MapError<UError>(
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
    public static IResult<T, TError> Ok(T value) =>
        new Result<T, TError>(value);

    /// <summary>
    /// Creates Result with the specified value wrapped in a completed Task.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Task<IResult<T, TError>> OkAsync(T value) =>
        Task.FromResult(Ok(value));

    /// <summary>
    /// Creates Result with the value of the awaited Task.
    /// </summary>
    /// <param name="valueTask"></param>
    /// <returns></returns>
    public static async Task<IResult<T, TError>> OkAsync(Task<T> valueTask) =>
        Ok(await valueTask);

    /// <summary>
    /// Creates a new Result with the specified error.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static IResult<T, TError> Error(TError errors) =>
        new Result<T, TError>(errors);

    /// <summary>
    /// Creates Result with the specified error wrapped in a completed Task.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static Task<IResult<T, TError>> ErrorAsync(TError errors) =>
        Task.FromResult(Error(errors));

    /// <summary>
    /// Creates Result with the value of the awaited Task.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static async Task<IResult<T, TError>> ErrorAsync(Task<TError> errors) =>
        Error(await errors);

    public static bool operator ==(Result<T, TError> left, Result<T, TError> right) =>
        left.Equals(right);

    public static bool operator !=(Result<T, TError> left, Result<T, TError> right) =>
        !(left == right);

    public override bool Equals(object? obj) =>
        _ok is not null && obj is not null && _ok.Equals(obj);

    public override int GetHashCode()
        => _ok is null ? 0 : _ok.GetHashCode();

    public override string ToString() =>
        Match(
            ok: x => $"Ok({x})",
            error: e => $"Error({e})");
}
