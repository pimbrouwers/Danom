namespace Danom;

/// <summary>
/// Contains Task extension methods for <see cref="IResultOption{T, TError}"/> that allow for
/// asynchronous operations containing <see cref="IResultOption{T, TError}"/>.
/// </summary>
public static class ResultOptionTaskExtensions
{
    public static async Task<U> MatchAsync<T, TError, U>(
        this Task<IResultOption<T, TError>> resultOptionTask,
        Func<T, Task<U>> ok,
        Func<Task<U>> none,
        Func<TError, Task<U>> error) =>
        await (await resultOptionTask).Match(ok, none, error);

    public static async Task<U> MatchAsync<T, TError, U>(
        this Task<IResultOption<T, TError>> resultOptionTask,
        Func<T, U> ok,
        Func<U> none,
        Func<TError, U> error) =>
        (await resultOptionTask).Match(ok, none, error);

    public static Task<IResultOption<U, TError>> BindAsync<T, TError, U>(
        this Task<IResultOption<T, TError>> resultOptionTask,
        Func<T, IResultOption<U, TError>> bind) =>
        resultOptionTask.MatchAsync(x => bind(x), ResultOption<U, TError>.None, ResultOption<U, TError>.Error);

    public static Task<IResultOption<U, TError>> BindAsync<T, TError, U>(
        this Task<IResultOption<T, TError>> resultOptionTask,
        Func<T, Task<IResultOption<U, TError>>> bind) =>
        resultOptionTask.MatchAsync(bind, ResultOption<U, TError>.NoneAsync, ResultOption<U, TError>.ErrorAsync);

    public static Task<IResultOption<U, TError>> MapAsync<T, TError, U>(
        this Task<IResultOption<T, TError>> resultOptionTask,
        Func<T, U> map) =>
        resultOptionTask.BindAsync(x => ResultOption<U, TError>.Ok(map(x)));

    public static Task<IResultOption<U, TError>> MapAsync<T, TError, U>(
        this Task<IResultOption<T, TError>> resultOptionTask,
        Func<T, Task<U>> map) =>
        resultOptionTask.BindAsync(x => ResultOption<U, TError>.OkAsync(map(x)));

    public static Task<T> DefaultValueAsync<T, TError>(
        this Task<IResultOption<T, TError>> resultOptionTask,
        T defaultValue) =>
        resultOptionTask.MatchAsync(ok => ok, () => defaultValue, _ => defaultValue);

    public static Task<T> DefaultValueAsync<T, TError>(
        this Task<IResultOption<T, TError>> resultTask,
        Task<T> defaultValue) =>
        resultTask.MatchAsync(some => Task.FromResult(some), () => defaultValue, _ => defaultValue);

    public static Task<T> DefaultWithAsync<T, TError>(
        this Task<IResultOption<T, TError>> resultOptionTask,
        Func<Task<T>> defaultWith) =>
        resultOptionTask.MatchAsync(ok => Task.FromResult(ok), defaultWith, _ => defaultWith());

    public static Task<T> DefaultWithAsync<T, TError>(
        this Task<IResultOption<T, TError>> resultOptionTask,
        Func<T> defaultWith) =>
        resultOptionTask.MatchAsync(ok => ok, defaultWith, _ => defaultWith());
}
