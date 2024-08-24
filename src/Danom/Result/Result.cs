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
public sealed class Result<T, TError>
    : Maybe<T, TError>, IResult<T, TError>
{
    internal Result(T t) : base(t) { }

    internal Result(TError t) : base(t) { }

    public bool IsOk => _isT1;
    public bool IsError => _isT2;

    public static IResult<T, TError> Ok(T value) =>
        new Result<T, TError>(value);

    public static Task<IResult<T, TError>> OkAsync(T value) =>
        Task.FromResult(Ok(value));

    public static async Task<IResult<T, TError>> OkAsync(Task<T> valueTask) =>
        Ok(await valueTask);

    public static IResult<T, TError> Error(TError errors) =>
        new Result<T, TError>(errors);

    public static Task<IResult<T, TError>> ErrorAsync(TError errors) =>
        Task.FromResult(Error(errors));

    public static async Task<IResult<T, TError>> ErrorAsync(Task<TError> errors) =>
        Error(await errors);

    public new U Match<U>(Func<T, U> ok, Func<TError, U> error) =>
        Match(t1: ok,  t2: error);

    public IResult<U, TError> Bind<U>(
        Func<T, IResult<U, TError>> bind) =>
        Match(bind, Result<U, TError>.Error);

    public IResult<U, TError> Map<U>(
        Func<T, U> map) =>
        Bind(x => Result<U, TError>.Ok(map(x)));

    public IResult<T, UError> MapError<UError>(
        Func<TError, UError> mapError) =>
        Match(Result<T, UError>.Ok, e => Result<T, UError>.Error(mapError(e)));

    /// <summary>
    /// Returns the value of the Result if it is T, otherwise returns the
    /// specified default value.
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public T DefaultValue(
         T defaultValue) =>
         Match(ok => ok, _ => defaultValue);

    /// <summary>
    /// Returns the value of the Result if it is T, otherwise returns the
    /// </summary>
    /// <param name="defaultWith"></param>
    /// <returns></returns>
    public T DefaultWith(
        Func<T> defaultWith) =>
        Match(ok => ok, _ => defaultWith());

    public override string ToString() =>
        Match(
            ok: x => $"Ok({x})",
            error: e => $"Error({e})");
}
