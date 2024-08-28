namespace Danom;

/// <summary>
/// Extension methods for converting between <see cref="Option{T}"/>,
/// <see cref="Result{T, TError}"/> and <see cref="ResultOption{T, TError}"/>.
/// </summary>
public static class ResultOptionConversionExtensions
{
    /// <summary>
    /// Converts the specified Option to a ResultOption.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="option"></param>
    /// <returns></returns>
    public static ResultOption<T, TError> ToResultOption<T, TError>(this Option<T> option) =>
        option.Match(ResultOption<T, TError>.Ok, ResultOption<T, TError>.None);

    /// <summary>
    /// Converts the specified Option to a ResultOption.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="optionTask"></param>
    /// <returns></returns>
    public static Task<ResultOption<T, TError>> ToResultOptionAsync<T, TError>(this Task<Option<T>> optionTask) =>
        optionTask.MatchAsync(ResultOption<T, TError>.Ok, ResultOption<T, TError>.None);

    /// <summary>
    /// Converts the specified Result to a ResultOption.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public static ResultOption<T, TError> ToResultOption<T, TError>(this Result<T, TError> result) =>
        result.Match(ResultOption<T, TError>.Ok, ResultOption<T, TError>.Error);

    /// <summary>
    /// Converts the specified Result to a ResultOption.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="resultTask"></param>
    /// <returns></returns>
    public static Task<ResultOption<T, TError>> ToResultOptionAsync<T, TError>(this Task<Result<T, TError>> resultTask) =>
        resultTask.MatchAsync(ResultOption<T, TError>.Ok, ResultOption<T, TError>.Error);

    /// <summary>
    /// Converts the specified ResultOption to a Result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public static ResultOption<T, TError> ToResultOption<T, TError>(this Result<Option<T>, TError> result) =>
        result.Match(
            ok: opt =>
                opt.Match(ResultOption<T, TError>.Ok, ResultOption<T, TError>.None),
            error: ResultOption<T, TError>.Error);

    /// <summary>
    /// Converts the specified ResultOption to a Result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="resultTask"></param>
    /// <returns></returns>
    public static Task<ResultOption<T, TError>> ToResultOptionAsync<T, TError>(this Task<Result<Option<T>, TError>> resultTask) =>
        resultTask.MatchAsync(
            ok: opt =>
                opt.Match(ResultOption<T, TError>.Ok, ResultOption<T, TError>.None),
            error: ResultOption<T, TError>.Error);
}


/// <summary>
/// Extension methods for converting between <see cref="Option{T}"/>,
/// <see cref="Result{T, TError}"/> and <see cref="ResultOption{T, TError}"/>
/// with <see cref="ResultErrors"/> as the error type.
/// </summary>
public static class ResultOptionTConversionExtensions
{
    /// <summary>
    /// Converts the specified Option to a ResultOption.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <returns></returns>
    public static ResultOption<T, ResultErrors> ToResultOption<T>(this Option<T> option) =>
        option.Match(ResultOption<T>.Ok, ResultOption<T>.None);

    /// <summary>
    /// Converts the specified Option to a ResultOption.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <returns></returns>
    public static Task<ResultOption<T, ResultErrors>> ToResultOptionAsync<T>(this Task<Option<T>> optionTask) =>
        optionTask.MatchAsync(ResultOption<T>.Ok, ResultOption<T>.None);

    /// <summary>
    /// Converts the specified Result to a ResultOption.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public static ResultOption<T, ResultErrors> AsResultOption<T>(this Result<T, ResultErrors> result) =>
        result.Match(ResultOption<T>.Ok, ResultOption<T>.Error);

    /// <summary>
    /// Converts the specified Result to a ResultOption.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resultTask"></param>
    /// <returns></returns>
    public static Task<ResultOption<T, ResultErrors>> ToResultOptionAsync<T>(this Task<Result<T, ResultErrors>> resultTask) =>
        resultTask.MatchAsync(ResultOption<T>.Ok, ResultOption<T>.Error);

    /// <summary>
    /// Converts the specified ResultOption to a Result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    public static ResultOption<T, ResultErrors> ToResultOption<T>(this Result<Option<T>, ResultErrors> result) =>
        result.Match(
            ok: opt =>
                opt.Match(ResultOption<T>.Ok, ResultOption<T>.None),
            error: ResultOption<T>.Error);

    /// <summary>
    /// Converts the specified ResultOption to a Result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resultTask"></param>
    /// <returns></returns>
    public static Task<ResultOption<T, ResultErrors>> ToResultOptionAsync<T>(this Task<Result<Option<T>, ResultErrors>> resultTask) =>
        resultTask.MatchAsync(
            ok: opt =>
                opt.Match(ResultOption<T>.Ok, ResultOption<T>.None),
            error: ResultOption<T>.Error);
}
