namespace Danom;

/// <summary>
/// Contains Task extension methods for <see cref="IOption{T}"/> that allow for
/// asynchronous operations containing <see cref="IOption{T}"/>.
/// </summary>
public static class OptionTaskExtensions
{
    public static async Task<U> MatchAsync<T, U>(
        this Task<IOption<T>> optionTask,
        Func<T, Task<U>> some,
        Func<Task<U>> none) =>
        await (await optionTask).Match(some, none);

    public static async Task<U> MatchAsync<T, U>(
        this Task<IOption<T>> optionTask,
        Func<T, U> some,
        Func<U> none) =>
        (await optionTask).Match(some, none);

    public static Task<IOption<U>> BindAsync<T, U>(
        this Task<IOption<T>> optionTask,
        Func<T, Task<IOption<U>>> bind) =>
        optionTask.MatchAsync(bind, Option<U>.NoneAsync);

    public static Task<IOption<U>> BindAsync<T, U>(
        this Task<IOption<T>> optionTask,
        Func<T, IOption<U>> bind) =>
        optionTask.MatchAsync(x => bind(x), Option<U>.None);

    public static Task<IOption<U>> MapAsync<T, U>(
        this Task<IOption<T>> optionTask,
        Func<T, Task<U>> map) =>
        BindAsync(optionTask, x => Option<U>.SomeAsync(map(x)));

    public static Task<IOption<U>> MapAsync<T, U>(
        this Task<IOption<T>> optionTask,
        Func<T, U> map) =>
        BindAsync(optionTask, x => Option<U>.Some(map(x)));

    public static Task<T> DefaultValueAsync<T>(
        this Task<IOption<T>> optionTask,
        T defaultValue) =>
        optionTask.MatchAsync(some => some, () => defaultValue);

    public static Task<T> DefaultValueAsync<T>(
        this Task<IOption<T>> optionTask,
        Task<T> defaultValue) =>
        optionTask.MatchAsync(some => Task.FromResult(some), () => defaultValue);

    public static Task<T> DefaultWithAsync<T>(
        this Task<IOption<T>> optionTask,
        Func<Task<T>> defaultWith) =>
        optionTask.MatchAsync(some => Task.FromResult(some), defaultWith);

    public static Task<T> DefaultWithAsync<T>(
        this Task<IOption<T>> optionTask,
        Func<T> defaultWith) =>
        optionTask.MatchAsync(some => some, () => defaultWith());

    public static Task<IOption<T>> OrElseAsync<T>(
        this Task<IOption<T>> optionTask,
        Task<IOption<T>> ifNone) =>
        optionTask.MatchAsync(_ => optionTask, () => ifNone);

    public static Task<IOption<T>> OrElseAsync<T>(
        this Task<IOption<T>> optionTask,
        IOption<T> ifNone) =>
        optionTask.MatchAsync(Option<T>.Some, () => ifNone);

    public static Task<IOption<T>> OrElseWithAsync<T>(
        this Task<IOption<T>> optionTask,
        Func<Task<IOption<T>>> ifNoneWith) =>
        optionTask.MatchAsync(_ => optionTask, ifNoneWith);

    public static Task<IOption<T>> OrElseWithAsync<T>(
        this Task<IOption<T>> optionTask,
        Func<IOption<T>> ifNoneWith) =>
        optionTask.MatchAsync(Option<T>.Some, ifNoneWith);
}
