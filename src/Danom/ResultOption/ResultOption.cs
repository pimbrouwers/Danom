namespace Danom;

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

public sealed class ResultOption<T, TError>
    : Choice<IOption<T>, TError>, IResultOption<T, TError>
{
    internal ResultOption(IOption<T> option) : base(option) { }

    internal ResultOption(TError tError) : base(tError) { }

    public bool IsOk => Match(option => option.IsSome, _ => false);
    public bool IsNone => Match(option => option.IsNone, _ => false);
    public bool IsError => _isT2;

    public static IResultOption<T, TError> Ok(T value) =>
        new ResultOption<T, TError>(Option.Some(value));

    public static Task<IResultOption<T, TError>> OkAsync(T value) =>
        Task.FromResult(Ok(value));

    public static async Task<IResultOption<T, TError>> OkAsync(Task<T> valueTask) =>
        Ok(await valueTask);

    public static IResultOption<T, TError> None() =>
        new ResultOption<T, TError>(Option<T>.None());

    public static Task<IResultOption<T, TError>> NoneAsync() =>
        Task.FromResult(None());

    public static IResultOption<T, TError> Error(TError error) =>
        new ResultOption<T, TError>(error);

    public static Task<IResultOption<T, TError>> ErrorAsync(TError error) =>
        Task.FromResult(Error(error));

    public static async Task<IResultOption<T, TError>> ErrorAsync(Task<TError> error) =>
        Error(await error);

    public U Match<U>(
        Func<T, U> ok,
        Func<U> none,
        Func<TError, U> error) =>
        Match(
            option =>
                option.Match(
                    some: x => ok(x),
                    none: none),
            error);

    public IResultOption<U, TError> Bind<U>(
        Func<T, IResultOption<U, TError>> bind) =>
        Match(bind, ResultOption<U, TError>.None, ResultOption<U, TError>.Error);

    public IResultOption<U, TError> Map<U>(
        Func<T, U> map) =>
        Bind(x => ResultOption<U, TError>.Ok(map(x)));

    public IResultOption<T, UError> MapError<UError>(
        Func<TError, UError> mapError) =>
        Match(ResultOption<T, UError>.Ok, ResultOption<T, UError>.None, e => ResultOption<T, UError>.Error(mapError(e)));

    public T DefaultValue(
        T defaultValue) =>
        Match(ok => ok, () => defaultValue, _ => defaultValue);

    public T DefaultWith(
        Func<T> defaultWith) =>
        Match(ok => ok, defaultWith, _ => defaultWith());

    public override string ToString() =>
        Match(
            ok: x => $"Ok({x})",
            none: () => "None",
            error: e => $"Error({e})");
}

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
