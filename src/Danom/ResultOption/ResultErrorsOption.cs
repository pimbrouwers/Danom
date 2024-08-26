namespace Danom;

/// <summary>
/// Static methods for creating <see cref="ResultOption{T, TError}"/> instances with
/// <see cref="ResultErrors"/> as the error type.
/// </summary>
/// <typeparam name="T"></typeparam>
public static class ResultOption<T>
{
    public static ResultOption<T, ResultErrors> Ok(T value) =>
        ResultOption<T, ResultErrors>.Ok(value);

    public static Task<ResultOption<T, ResultErrors>> OkAsync(T value) =>
        ResultOption<T, ResultErrors>.OkAsync(value);

    public static Task<ResultOption<T, ResultErrors>> OkAsync(Task<T> valueTask) =>
        ResultOption<T, ResultErrors>.OkAsync(valueTask);

    public static ResultOption<T, ResultErrors> None() =>
        ResultOption<T, ResultErrors>.None();

    public static Task<ResultOption<T, ResultErrors>> NoneAsync() =>
        ResultOption<T, ResultErrors>.NoneAsync();

    public static ResultOption<T, ResultErrors> Error(ResultErrors errors) =>
        ResultOption<T, ResultErrors>.Error(errors);

    public static ResultOption<T, ResultErrors> Error(string message) =>
        Error([new ResultError(string.Empty, [message])]);

    public static Task<ResultOption<T, ResultErrors>> ErrorAsync(ResultErrors errors) =>
        Task.FromResult(Error(errors));

    public static Task<ResultOption<T, ResultErrors>> ErrorAsync(string message) =>
        Task.FromResult(Error(message));
}

/// <summary>
/// Extension methods for converting between <see cref="IOption{T}"/>,
/// <see cref="IResult{T, TError}"/> and <see cref="ResultOption{T, TError}"/>
/// with <see cref="ResultErrors"/> as the error type.
/// </summary>
public static class ResultOptionTConversionExtensions
{
    public static ResultOption<T, ResultErrors> ToResultOption<T>(this Option<T> option) =>
        option.Match(ResultOption<T>.Ok, ResultOption<T>.None);

    public static Task<ResultOption<T, ResultErrors>> ToResultOptionAsync<T>(this Task<Option<T>> optionTask) =>
        optionTask.MatchAsync(ResultOption<T>.Ok, ResultOption<T>.None);

    public static ResultOption<T, ResultErrors> AsResultOption<T>(this Result<T, ResultErrors> result) =>
        result.Match(ResultOption<T>.Ok, ResultOption<T>.Error);

    public static Task<ResultOption<T, ResultErrors>> ToResultOptionAsync<T>(this Task<Result<T, ResultErrors>> resultTask) =>
        resultTask.MatchAsync(ResultOption<T>.Ok, ResultOption<T>.Error);

    public static ResultOption<T, ResultErrors> ToResultOption<T>(this Result<Option<T>, ResultErrors> result) =>
        result.Match(
            ok: opt =>
                opt.Match(ResultOption<T>.Ok, ResultOption<T>.None),
            error: ResultOption<T>.Error);

    public static Task<ResultOption<T, ResultErrors>> ToResultOptionAsync<T>(this Task<Result<Option<T>, ResultErrors>> resultTask) =>
        resultTask.MatchAsync(
            ok: opt =>
                opt.Match(ResultOption<T>.Ok, ResultOption<T>.None),
            error: ResultOption<T>.Error);
}
