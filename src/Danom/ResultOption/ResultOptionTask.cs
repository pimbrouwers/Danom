namespace Danom;

/// <summary>
/// Contains Task extension methods for <see cref="ResultOption{T, TError}"/> that allow for
/// asynchronous operations containing <see cref="ResultOption{T, TError}"/>.
/// </summary>
public static class ResultOptionTaskExtensions
{
    /// <summary>
    /// If ResultOption is Ok evaluate the ok delegate, otherwise none. If
    /// ResultOption is Error evaluate the error delegate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="resultOptionTask"></param>
    /// <param name="ok"></param>
    /// <param name="none"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static async Task<U> MatchAsync<T, TError, U>(
        this Task<ResultOption<T, TError>> resultOptionTask,
        Func<T, Task<U>> ok,
        Func<Task<U>> none,
        Func<TError, Task<U>> error) =>
        await (await resultOptionTask).Match(ok, none, error);

    /// <summary>
    /// If ResultOption is Ok evaluate the ok delegate, otherwise none. If
    /// ResultOption is Error evaluate the error delegate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="resultOptionTask"></param>
    /// <param name="ok"></param>
    /// <param name="none"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static async Task<U> MatchAsync<T, TError, U>(
        this Task<ResultOption<T, TError>> resultOptionTask,
        Func<T, U> ok,
        Func<U> none,
        Func<TError, U> error) =>
        (await resultOptionTask).Match(ok, none, error);

    /// <summary>
    /// Evaluates the bind delegate if ResultOption is Ok.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="resultOptionTask"></param>
    /// <param name="bind"></param>
    /// <returns></returns>
    public static Task<ResultOption<U, TError>> BindAsync<T, TError, U>(
        this Task<ResultOption<T, TError>> resultOptionTask,
        Func<T, ResultOption<U, TError>> bind) =>
        resultOptionTask.MatchAsync(x => bind(x), ResultOption<U, TError>.None, ResultOption<U, TError>.Error);

    /// <summary>
    /// Evaluates the bind delegate if ResultOption is Ok.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="resultOptionTask"></param>
    /// <param name="bind"></param>
    /// <returns></returns>
    public static Task<ResultOption<U, TError>> BindAsync<T, TError, U>(
        this Task<ResultOption<T, TError>> resultOptionTask,
        Func<T, Task<ResultOption<U, TError>>> bind) =>
        resultOptionTask.MatchAsync(bind, ResultOption<U, TError>.NoneAsync, ResultOption<U, TError>.ErrorAsync);

    /// <summary>
    /// Evaluates the map delegate if ResultOption is Ok.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="resultOptionTask"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static Task<ResultOption<U, TError>> MapAsync<T, TError, U>(
        this Task<ResultOption<T, TError>> resultOptionTask,
        Func<T, U> map) =>
        resultOptionTask.BindAsync(x => ResultOption<U, TError>.Ok(map(x)));

    /// <summary>
    /// Evaluates the map delegate if ResultOption is Ok.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="resultOptionTask"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static Task<ResultOption<U, TError>> MapAsync<T, TError, U>(
        this Task<ResultOption<T, TError>> resultOptionTask,
        Func<T, Task<U>> map) =>
        resultOptionTask.BindAsync(x => ResultOption<U, TError>.OkAsync(map(x)));

    /// <summary>
    /// Returns the value of ResultOption if it is Ok, otherwise return the
    /// specified default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="resultOptionTask"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Task<T> DefaultValueAsync<T, TError>(
        this Task<ResultOption<T, TError>> resultOptionTask,
        T defaultValue) =>
        resultOptionTask.MatchAsync(ok => ok, () => defaultValue, _ => defaultValue);

    /// <summary>
    /// Returns the value of ResultOption if it is Ok, otherwise evaluate the
    /// default delegate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="resultTask"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Task<T> DefaultValueAsync<T, TError>(
        this Task<ResultOption<T, TError>> resultTask,
        Task<T> defaultValue) =>
        resultTask.MatchAsync(some => Task.FromResult(some), () => defaultValue, _ => defaultValue);

    /// <summary>
    /// Returns the value of ResultOption if it is Ok, otherwise return the
    /// specified default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="resultOptionTask"></param>
    /// <param name="defaultWith"></param>
    /// <returns></returns>
    public static Task<T> DefaultWithAsync<T, TError>(
        this Task<ResultOption<T, TError>> resultOptionTask,
        Func<Task<T>> defaultWith) =>
        resultOptionTask.MatchAsync(ok => Task.FromResult(ok), defaultWith, _ => defaultWith());

    /// <summary>
    /// Returns the value of ResultOption if it is Ok, otherwise evaluate the
    /// default delegate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="resultOptionTask"></param>
    /// <param name="defaultWith"></param>
    /// <returns></returns>
    public static Task<T> DefaultWithAsync<T, TError>(
        this Task<ResultOption<T, TError>> resultOptionTask,
        Func<T> defaultWith) =>
        resultOptionTask.MatchAsync(ok => ok, defaultWith, _ => defaultWith());
}
