namespace Danom;

/// <summary>
/// Contains Task extension methods for <see cref="Result{T, TError}"/> that allow for
/// asynchronous operations containing <see cref="Result{T, TError}"/>.
/// </summary>
public static class ResultTaskExtensions
{
    /// <summary>
    /// If Result is Ok evaluate the ok delegate, otherwise error.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="resultTask"></param>
    /// <param name="ok"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static async Task<U> MatchAsync<T, TError, U>(
        this Task<Result<T, TError>> resultTask,
        Func<T, Task<U>> ok,
        Func<TError, Task<U>> error) =>
        await (await resultTask).Match(ok, error);

    /// <summary>
    /// If Result is Ok evaluate the ok delegate, otherwise error.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="resultTask"></param>
    /// <param name="ok"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static async Task<U> MatchAsync<T, TError, U>(
        this Task<Result<T, TError>> resultTask,
        Func<T, U> ok,
        Func<TError, U> error) =>
        (await resultTask).Match(ok, error);

    /// <summary>
    /// Evaluates the bind delegate if Result is Ok otherwise return Error.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="resultTask"></param>
    /// <param name="bind"></param>
    /// <returns></returns>
    public static Task<Result<U, TError>> BindAsync<T, TError, U>(
        this Task<Result<T, TError>> resultTask,
        Func<T, Result<U, TError>> bind) =>
        resultTask.MatchAsync(x => bind(x), Result<U, TError>.Error);

    /// <summary>
    /// Evaluates the bind delegate if Result is Ok otherwise return Error.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="resultTask"></param>
    /// <param name="bind"></param>
    /// <returns></returns>
    public static Task<Result<U, TError>> BindAsync<T, TError, U>(
        this Task<Result<T, TError>> resultTask,
        Func<T, Task<Result<U, TError>>> bind) =>
        resultTask.MatchAsync(bind, Result<U, TError>.ErrorAsync);

    /// <summary>
    /// Evaluates the map delegate if Result is Ok otherwise return Error.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="resultTask"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static Task<Result<U, TError>> MapAsync<T, TError, U>(
        this Task<Result<T, TError>> resultTask,
        Func<T, U> map) =>
        resultTask.BindAsync(x => Result<U, TError>.Ok(map(x)));

    /// <summary>
    /// Evaluates the map delegate if Result is Ok otherwise return Error.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="resultTask"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static Task<Result<U, TError>> MapAsync<T, TError, U>(
        this Task<Result<T, TError>> resultTask,
        Func<T, Task<U>> map) =>
        resultTask.BindAsync(x => Result<U, TError>.OkAsync(map(x)));

    /// <summary>
    /// Evaluates the mapError delegate if Result is Error otherwise return Ok.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="UError"></typeparam>
    /// <param name="resultTask"></param>
    /// <param name="mapError"></param>
    /// <returns></returns>
    public static Task<Result<T, UError>> MapErrorAsync<T, UError>(
        this Task<Result<T, UError>> resultTask,
        Func<UError, UError> mapError) =>
        resultTask.MatchAsync(Result<T, UError>.Ok, e => Result<T, UError>.Error(mapError(e)));

        /// <summary>
    /// Evaluates the mapError delegate if Result is Error otherwise return Ok.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="UError"></typeparam>
    /// <param name="resultTask"></param>
    /// <param name="mapError"></param>
    /// <returns></returns>
    public static Task<Result<T, UError>> MapErrorAsync<T, UError>(
        this Task<Result<T, UError>> resultTask,
        Func<UError, Task<UError>> mapError) =>
        resultTask.MatchAsync(Result<T, UError>.OkAsync, e => Result<T, UError>.ErrorAsync(mapError(e)));

    /// <summary>
    /// Returns the value of Result if it is T, otherwise returns the
    /// specified default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="resultTask"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Task<T> DefaultValueAsync<T, TError>(
        this Task<Result<T, TError>> resultTask,
        T defaultValue) =>
        resultTask.MatchAsync(ok => ok, _ => defaultValue);

    /// <summary>
    /// Returns the value of Result if it is T, otherwise returns the
    /// specified default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="resultTask"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Task<T> DefaultValueAsync<T, TError>(
        this Task<Result<T, TError>> resultTask,
        Task<T> defaultValue) =>
        resultTask.MatchAsync(some => Task.FromResult(some), _ => defaultValue);

    /// <summary>
    /// Returns the value of Result if it is T, otherwise returns the
    /// specified default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="resultTask"></param>
    /// <param name="defaultWith"></param>
    /// <returns></returns>
    public static Task<T> DefaultWithAsync<T, TError>(
        this Task<Result<T, TError>> resultTask,
        Func<Task<T>> defaultWith) =>
        resultTask.MatchAsync(ok => Task.FromResult(ok), _ => defaultWith());

    /// <summary>
    /// Returns the value of Result if it is T, otherwise returns the
    /// specified default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="resultTask"></param>
    /// <param name="defaultWith"></param>
    /// <returns></returns>
    public static Task<T> DefaultWithAsync<T, TError>(
        this Task<Result<T, TError>> resultTask,
        Func<T> defaultWith) =>
        resultTask.MatchAsync(ok => ok, _ => defaultWith());
}
