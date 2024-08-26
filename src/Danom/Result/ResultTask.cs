namespace Danom;

/// <summary>
/// Contains Task extension methods for <see cref="IResult{T, TError}"/> that allow for
/// asynchronous operations containing <see cref="IResult{T, TError}"/>.
/// </summary>
public static class ResultTaskExtensions
{
    public static async Task<U> MatchAsync<T, TError, U>(
        this Task<Result<T, TError>> resultTask,
        Func<T, Task<U>> ok,
        Func<TError, Task<U>> error) =>
        await (await resultTask).Match(ok, error);

    public static async Task<U> MatchAsync<T, TError, U>(
        this Task<Result<T, TError>> resultTask,
        Func<T, U> ok,
        Func<TError, U> error) =>
        (await resultTask).Match(ok, error);

    public static Task<Result<U, TError>> BindAsync<T, TError, U>(
        this Task<Result<T, TError>> resultTask,
        Func<T, Result<U, TError>> bind) =>
        resultTask.MatchAsync(x => bind(x), Result<U, TError>.Error);

    public static Task<Result<U, TError>> BindAsync<T, TError, U>(
        this Task<Result<T, TError>> resultTask,
        Func<T, Task<Result<U, TError>>> bind) =>
        resultTask.MatchAsync(bind, Result<U, TError>.ErrorAsync);

    public static Task<Result<U, TError>> MapAsync<T, TError, U>(
        this Task<Result<T, TError>> resultTask,
        Func<T, U> map) =>
        resultTask.BindAsync(x => Result<U, TError>.Ok(map(x)));

    public static Task<Result<U, TError>> MapAsync<T, TError, U>(
        this Task<Result<T, TError>> resultTask,
        Func<T, Task<U>> map) =>
        resultTask.BindAsync(x => Result<U, TError>.OkAsync(map(x)));

    public static Task<T> DefaultValueAsync<T, TError>(
        this Task<Result<T, TError>> resultTask,
        T defaultValue) =>
        resultTask.MatchAsync(ok => ok, _ => defaultValue);

    public static Task<T> DefaultValueAsync<T, TError>(
        this Task<Result<T, TError>> resultTask,
        Task<T> defaultValue) =>
        resultTask.MatchAsync(some => Task.FromResult(some), _ => defaultValue);

    public static Task<T> DefaultWithAsync<T, TError>(
        this Task<Result<T, TError>> resultTask,
        Func<Task<T>> defaultWith) =>
        resultTask.MatchAsync(ok => Task.FromResult(ok), _ => defaultWith());

    public static Task<T> DefaultWithAsync<T, TError>(
        this Task<Result<T, TError>> resultTask,
        Func<T> defaultWith) =>
        resultTask.MatchAsync(ok => ok, _ => defaultWith());
}
