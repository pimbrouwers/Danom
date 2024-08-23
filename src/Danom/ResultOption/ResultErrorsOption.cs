namespace Danom;

/// <summary>
/// Static methods for creating <see cref="IResultOption{T, TError}"/> instances with
/// <see cref="ResultErrors"/> as the error type.
/// </summary>
/// <typeparam name="T"></typeparam>
public static class ResultOption<T>
{
    public static IResultOption<T, ResultErrors> Ok(T value) =>
        ResultOption<T, ResultErrors>.Ok(value);

    public static Task<IResultOption<T, ResultErrors>> OkAsync(T value) =>
        ResultOption<T, ResultErrors>.OkAsync(value);

    public static Task<IResultOption<T, ResultErrors>> OkAsync(Task<T> valueTask) =>
        ResultOption<T, ResultErrors>.OkAsync(valueTask);

    public static IResultOption<T, ResultErrors> None() =>
        ResultOption<T, ResultErrors>.None();

    public static Task<IResultOption<T, ResultErrors>> NoneAsync() =>
        ResultOption<T, ResultErrors>.NoneAsync();

    public static IResultOption<T, ResultErrors> Error(ResultErrors errors) =>
        ResultOption<T, ResultErrors>.Error(errors);

    public static IResultOption<T, ResultErrors> Error(string message) =>
        Error([new ResultError(string.Empty, [message])]);

    public static Task<IResultOption<T, ResultErrors>> ErrorAsync(ResultErrors errors) =>
        Task.FromResult(Error(errors));

    public static Task<IResultOption<T, ResultErrors>> ErrorAsync(string message) =>
        Task.FromResult(Error(message));
}

/// <summary>
/// Extension methods for converting between <see cref="IOption{T}"/>,
/// <see cref="IResult{T, TError}"/> and <see cref="IResultOption{T, TError}"/>
/// with <see cref="ResultErrors"/> as the error type.
/// </summary>
public static class ResultOptionTConversionExtensions
{
    public static IResultOption<T, ResultErrors> ToResultOption<T>(this IOption<T> option) =>
        option.Match(ResultOption<T>.Ok, ResultOption<T>.None);

    public static Task<IResultOption<T, ResultErrors>> ToResultOptionAsync<T>(this Task<IOption<T>> optionTask) =>
        optionTask.MatchAsync(ResultOption<T>.Ok, ResultOption<T>.None);

    public static IResultOption<T, ResultErrors> AsResultOption<T>(this IResult<T, ResultErrors> result) =>
        result.Match(ResultOption<T>.Ok, ResultOption<T>.Error);

    public static Task<IResultOption<T, ResultErrors>> ToResultOptionAsync<T>(this Task<IResult<T, ResultErrors>> resultTask) =>
        resultTask.MatchAsync(ResultOption<T>.Ok, ResultOption<T>.Error);

    public static IResultOption<T, ResultErrors> ToResultOption<T>(this IResult<IOption<T>, ResultErrors> result) =>
        result.Match(
            ok: opt =>
                opt.Match(ResultOption<T>.Ok, ResultOption<T>.None),
            error: ResultOption<T>.Error);

    public static Task<IResultOption<T, ResultErrors>> ToResultOptionAsync<T>(this Task<IResult<IOption<T>, ResultErrors>> resultTask) =>
        resultTask.MatchAsync(
            ok: opt =>
                opt.Match(ResultOption<T>.Ok, ResultOption<T>.None),
            error: ResultOption<T>.Error);
}
