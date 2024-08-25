namespace Danom;

/// <summary>
/// Represents a result of an operation that can be either successful or not. It
/// is typically used in monadic error handling in scenarios where the success
/// path may not have a value.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TError"></typeparam>
public interface IResultOption<T, TError>
{
    bool IsOk { get; }
    bool IsNone { get; }
    bool IsError { get; }
    U Match<U>(Func<T, U> ok, Func<U> none, Func<TError, U> error);
    IResultOption<U, TError> Bind<U>(Func<T, IResultOption<U, TError>> bind);
    IResultOption<U, TError> Map<U>(Func<T, U> map);
    IResultOption<T, UError> MapError<UError>(Func<TError, UError> mapError);
    T DefaultValue(T defaultValue);
    T DefaultWith(Func<T> defaultWith);
}

/// <inheritdoc />
public readonly struct ResultOption<T, TError>()
    : IResultOption<T, TError>
{
    private readonly IOption<T> _ok = Option<T>.None();
    private readonly TError? _error = default;

    private ResultOption(IOption<T> option) : this()
    {
        _ok = option;
        IsOk = option.IsSome;
    }

    private ResultOption(TError tError) : this()
    {
        _error = tError;
        IsError = true;
    }

    /// <summary>
    /// Returns true if the ResultOption is Ok, false otherwise.
    /// </summary>
    public bool IsOk { get; } = false;

    public bool IsNone => !IsOk && !IsError;

    /// <summary>
    /// Returns true if the ResultOption is Error, false otherwise.
    /// </summary>
    public bool IsError { get; } = false;

    /// <summary>
    /// If Result is Ok evaluate the ok delegate, otherwise error.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="ok"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public U Match<U>(Func<T, U> ok, Func<U> none, Func<TError, U> error) =>
        IsError && _error is TError tError ?
            error(tError) :
            _ok.Match(ok, none);

    /// <summary>
    /// Evaluates the bind delegate if ResultOption is Ok.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="bind"></param>
    /// <returns></returns>
    public IResultOption<U, TError> Bind<U>(
        Func<T, IResultOption<U, TError>> bind) =>
        Match(bind, ResultOption<U, TError>.None, ResultOption<U, TError>.Error);

    /// <summary>
    /// Evaluates the map delegate if ResultOption is Ok.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="map"></param>
    /// <returns></returns>
    public IResultOption<U, TError> Map<U>(
        Func<T, U> map) =>
        Bind(x => ResultOption<U, TError>.Ok(map(x)));

    /// <summary>
    /// Evaluates the mapError delegate if Result is Error.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="map"></param>
    /// <returns></returns>
    public IResultOption<T, UError> MapError<UError>(
        Func<TError, UError> mapError) =>
        Match(ResultOption<T, UError>.Ok, ResultOption<T, UError>.None, e =>
            ResultOption<T, UError>.Error(mapError(e)));

    /// <summary>
    /// Returns the value of ResultOption if it is T, otherwise returns the
    /// specified default value.
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public T DefaultValue(
        T defaultValue) =>
        Match(ok => ok, () => defaultValue, _ => defaultValue);

    /// <summary>
    /// Returns the value of ResultOption if it is T, otherwise returns the
    /// </summary>
    /// <param name="defaultWith"></param>
    /// <returns></returns>
    public T DefaultWith(
        Func<T> defaultWith) =>
        Match(ok => ok, defaultWith, _ => defaultWith());

    /// <summary>
    /// Creates a new ResultOption with the specified value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IResultOption<T, TError> Ok(T value) =>
        new ResultOption<T, TError>(Option<T>.Some(value));

    /// <summary>
    /// Creates ResultOption with the specified value wrapped in a completed Task.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Task<IResultOption<T, TError>> OkAsync(T value) =>
        Task.FromResult(Ok(value));

    /// <summary>
    /// Creates ResultOption with the value of the awaited Task.
    /// </summary>
    /// <param name="valueTask"></param>
    /// <returns></returns>
    public static async Task<IResultOption<T, TError>> OkAsync(Task<T> valueTask) =>
        Ok(await valueTask);

    /// <summary>
    /// Creates a new ResultOption with no value.
    /// </summary>
    /// <returns></returns>
    public static IResultOption<T, TError> None() =>
        new ResultOption<T, TError>(Option<T>.None());

    /// <summary>
    /// Creates a new ResultOption with no value wrapped in a completed Task.
    /// </summary>
    /// <returns></returns>
    public static Task<IResultOption<T, TError>> NoneAsync() =>
        Task.FromResult(None());

    /// <summary>
    /// Creates a new ResultOption with the specified error.
    /// </summary>
    /// <param name="errors"></param>
    public static IResultOption<T, TError> Error(TError error) =>
        new ResultOption<T, TError>(error);

    /// <summary>
    /// Creates ResultOption with the specified error wrapped in a completed Task.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static Task<IResultOption<T, TError>> ErrorAsync(TError error) =>
        Task.FromResult(Error(error));

    /// <summary>
    /// Creates ResultOption with the value of the awaited Task.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static async Task<IResultOption<T, TError>> ErrorAsync(Task<TError> error) =>
        Error(await error);

    public static bool operator ==(ResultOption<T, TError> left, ResultOption<T, TError> right) =>
        left.Equals(right);

    public static bool operator !=(ResultOption<T, TError> left, ResultOption<T, TError> right) =>
        !(left == right);

    public override bool Equals(object? obj) =>
        _ok is not null && obj is not null && _ok.Equals(obj);

    public override int GetHashCode()
        => _ok is null ? 0 : _ok.GetHashCode();

    public override string ToString() =>
        Match(
            ok: x => $"Ok({x})",
            none: () => "None",
            error: e => $"Error({e})");
}

/// <summary>
/// Extension methods for converting between <see cref="IOption{T}"/>,
/// <see cref="IResult{T, TError}"/> and <see cref="IResultOption{T, TError}"/>.
/// </summary>
public static class ResultOptionConversionExtensions
{
    public static IResultOption<T, TError> ToResultOption<T, TError>(this IOption<T> option) =>
        option.Match(ResultOption<T, TError>.Ok, ResultOption<T, TError>.None);

    public static Task<IResultOption<T, TError>> ToResultOptionAsync<T, TError>(this Task<IOption<T>> optionTask) =>
        optionTask.MatchAsync(ResultOption<T, TError>.Ok, ResultOption<T, TError>.None);

    public static IResultOption<T, TError> ToResultOption<T, TError>(this IResult<T, TError> result) =>
        result.Match(ResultOption<T, TError>.Ok, ResultOption<T, TError>.Error);

    public static Task<IResultOption<T, TError>> ToResultOptionAsync<T, TError>(this Task<IResult<T, TError>> resultTask) =>
        resultTask.MatchAsync(ResultOption<T, TError>.Ok, ResultOption<T, TError>.Error);

    public static IResultOption<T, TError> ToResultOption<T, TError>(this IResult<IOption<T>, TError> result) =>
        result.Match(
            ok: opt =>
                opt.Match(ResultOption<T, TError>.Ok, ResultOption<T, TError>.None),
            error: ResultOption<T, TError>.Error);

    public static Task<IResultOption<T, TError>> ToResultOptionAsync<T, TError>(this Task<IResult<IOption<T>, TError>> resultTask) =>
        resultTask.MatchAsync(
            ok: opt =>
                opt.Match(ResultOption<T, TError>.Ok, ResultOption<T, TError>.None),
            error: ResultOption<T, TError>.Error);
}
