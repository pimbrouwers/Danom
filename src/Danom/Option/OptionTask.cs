namespace Danom;

/// <summary>
/// Contains Task extension methods for <see cref="Option{T}"/> that allow for
/// asynchronous operations containing <see cref="Option{T}"/>.
/// </summary>
public static class OptionTaskExtensions
{
    /// <summary>
    /// If the Option is Some evaluate the some delegate, otherwise none.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="some"></param>
    /// <param name="none"></param>
    /// <returns></returns>
    public static async Task<U> MatchAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, Task<U>> some,
        Func<Task<U>> none) =>
        await (await optionTask).Match(some, none);

    /// <summary>
    /// If the Option is Some evaluate the some delegate, otherwise none.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="some"></param>
    /// <param name="none"></param>
    /// <returns></returns>
    public static async Task<U> MatchAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, U> some,
        Func<U> none) =>
        (await optionTask).Match(some, none);

    /// <summary>
    /// Evaluates the bind delegate if the Option is Some otherwise return None.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="bind"></param>
    /// <returns></returns>
    public static Task<Option<U>> BindAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, Task<Option<U>>> bind) =>
        optionTask.MatchAsync(bind, Option<U>.NoneAsync);

    /// <summary>
    /// Evaluates the bind delegate if the Option is Some otherwise return None.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="bind"></param>
    /// <returns></returns>
    public static Task<Option<U>> BindAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, Option<U>> bind) =>
        optionTask.MatchAsync(x => bind(x), Option<U>.None);

    /// <summary>
    /// Evaluates the map delegate if the Option is Some otherwise return None.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static Task<Option<U>> MapAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, Task<U>> map) =>
        BindAsync(optionTask, x => Option<U>.SomeAsync(map(x)));

    /// <summary>
    /// Evaluates the map delegate if the Option is Some otherwise return None.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="map"></param>
    /// <returns></returns>
    public static Task<Option<U>> MapAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, U> map) =>
        BindAsync(optionTask, x => Option<U>.Some(map(x)));

    /// <summary>
    /// Returns the value of the Option if it is T otherwise return default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Task<T> DefaultValueAsync<T>(
        this Task<Option<T>> optionTask,
        T defaultValue) =>
        optionTask.MatchAsync(some => some, () => defaultValue);

    /// <summary>
    /// Returns the value of the Option if it is T otherwise return default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static Task<T> DefaultValueAsync<T>(
        this Task<Option<T>> optionTask,
        Task<T> defaultValue) =>
        optionTask.MatchAsync(some => Task.FromResult(some), () => defaultValue);

    /// <summary>
    /// Returns the value of the Option if it is T otherwise evaluate default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="defaultWith"></param>
    /// <returns></returns>
    public static Task<T> DefaultWithAsync<T>(
        this Task<Option<T>> optionTask,
        Func<Task<T>> defaultWith) =>
        optionTask.MatchAsync(some => Task.FromResult(some), defaultWith);

    /// <summary>
    /// Returns the value of the Option if it is T otherwise evaluate default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="defaultWith"></param>
    /// <returns></returns>
    public static Task<T> DefaultWithAsync<T>(
        this Task<Option<T>> optionTask,
        Func<T> defaultWith) =>
        optionTask.MatchAsync(some => some, () => defaultWith());

    /// <summary>
    /// Return the Option if it is Some, otherwise return the specified Option.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="ifNone"></param>
    /// <returns></returns>
    public static Task<Option<T>> OrElseAsync<T>(
        this Task<Option<T>> optionTask,
        Task<Option<T>> ifNone) =>
        optionTask.MatchAsync(_ => optionTask, () => ifNone);

    /// <summary>
    /// Return the Option if it is Some, otherwise return the specified Option.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="ifNone"></param>
    /// <returns></returns>
    public static Task<Option<T>> OrElseAsync<T>(
        this Task<Option<T>> optionTask,
        Option<T> ifNone) =>
        optionTask.MatchAsync(Option<T>.Some, () => ifNone);

    /// <summary>
    /// Return the Option if it is Some, otherwise evaluate ifNoneWith.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="ifNoneWith"></param>
    /// <returns></returns>
    public static Task<Option<T>> OrElseWithAsync<T>(
        this Task<Option<T>> optionTask,
        Func<Task<Option<T>>> ifNoneWith) =>
        optionTask.MatchAsync(_ => optionTask, ifNoneWith);

    /// <summary>
    /// Return the Option if it is Some, otherwise evaluate ifNoneWith.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="ifNoneWith"></param>
    /// <returns></returns>
    public static Task<Option<T>> OrElseWithAsync<T>(
        this Task<Option<T>> optionTask,
        Func<Option<T>> ifNoneWith) =>
        optionTask.MatchAsync(Option<T>.Some, ifNoneWith);
}
