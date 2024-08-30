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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<U> MatchAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, Task<U>> some,
        Func<Task<U>> none,
        CancellationToken? cancellationToken = null)
    {
        var option = await optionTask.WaitOrCancel(cancellationToken);
        return await option.Match(some, none).WaitOrCancel(cancellationToken);
    }

    /// <summary>
    /// If the Option is Some evaluate the some delegate, otherwise none.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="some"></param>
    /// <param name="none"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<U> MatchAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, U> some,
        Func<U> none,
        CancellationToken? cancellationToken = null) =>
        (await optionTask.WaitOrCancel(cancellationToken)).Match(some, none);

    /// <summary>
    /// Evaluates the bind delegate if the Option is Some otherwise return None.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="bind"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<Option<U>> BindAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, Task<Option<U>>> bind,
        CancellationToken? cancellationToken = null) =>
        optionTask.MatchAsync(bind, Option<U>.NoneAsync, cancellationToken);

    /// <summary>
    /// Evaluates the bind delegate if the Option is Some otherwise return None.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="bind"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<Option<U>> BindAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, Option<U>> bind,
        CancellationToken? cancellationToken = null) =>
        optionTask.MatchAsync(x => bind(x), Option<U>.None, cancellationToken);

    /// <summary>
    /// Evaluates the map delegate if the Option is Some otherwise return None.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="map"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<Option<U>> MapAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, Task<U>> map,
        CancellationToken? cancellationToken = null) =>
        BindAsync(optionTask, x => Option<U>.SomeAsync(map(x)), cancellationToken);

    /// <summary>
    /// Evaluates the map delegate if the Option is Some otherwise return None.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="map"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<Option<U>> MapAsync<T, U>(
        this Task<Option<T>> optionTask,
        Func<T, U> map,
        CancellationToken? cancellationToken = null) =>
        BindAsync(optionTask, x => Option<U>.Some(map(x)), cancellationToken);

    /// <summary>
    /// Returns the value of the Option if it is T otherwise return default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<T> DefaultValueAsync<T>(
        this Task<Option<T>> optionTask,
        T defaultValue,
        CancellationToken? cancellationToken = null) =>
        optionTask.MatchAsync(some => some, () => defaultValue, cancellationToken);

    /// <summary>
    /// Returns the value of the Option if it is T otherwise return default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<T> DefaultValueAsync<T>(
        this Task<Option<T>> optionTask,
        Task<T> defaultValue,
        CancellationToken? cancellationToken = null) =>
        optionTask.MatchAsync(some => Task.FromResult(some), () => defaultValue, cancellationToken);

    /// <summary>
    /// Returns the value of the Option if it is T otherwise evaluate default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="defaultWith"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<T> DefaultWithAsync<T>(
        this Task<Option<T>> optionTask,
        Func<Task<T>> defaultWith,
        CancellationToken? cancellationToken = null) =>
        optionTask.MatchAsync(some => Task.FromResult(some), defaultWith, cancellationToken);

    /// <summary>
    /// Returns the value of the Option if it is T otherwise evaluate default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="defaultWith"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<T> DefaultWithAsync<T>(
        this Task<Option<T>> optionTask,
        Func<T> defaultWith,
        CancellationToken? cancellationToken = null) =>
        optionTask.MatchAsync(some => some, () => defaultWith(), cancellationToken);

    /// <summary>
    /// Return the Option if it is Some, otherwise return the specified Option.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="ifNone"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<Option<T>> OrElseAsync<T>(
        this Task<Option<T>> optionTask,
        Task<Option<T>> ifNone,
        CancellationToken? cancellationToken = null) =>
        optionTask.MatchAsync(_ => optionTask, () => ifNone, cancellationToken);

    /// <summary>
    /// Return the Option if it is Some, otherwise return the specified Option.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="ifNone"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<Option<T>> OrElseAsync<T>(
        this Task<Option<T>> optionTask,
        Option<T> ifNone,
        CancellationToken? cancellationToken = null) =>
        optionTask.MatchAsync(Option<T>.Some, () => ifNone, cancellationToken);

    /// <summary>
    /// Return the Option if it is Some, otherwise evaluate ifNoneWith.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="ifNoneWith"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<Option<T>> OrElseWithAsync<T>(
        this Task<Option<T>> optionTask,
        Func<Task<Option<T>>> ifNoneWith,
        CancellationToken? cancellationToken = null) =>
        optionTask.MatchAsync(_ => optionTask, ifNoneWith, cancellationToken);

    /// <summary>
    /// Return the Option if it is Some, otherwise evaluate ifNoneWith.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="optionTask"></param>
    /// <param name="ifNoneWith"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task<Option<T>> OrElseWithAsync<T>(
        this Task<Option<T>> optionTask,
        Func<Option<T>> ifNoneWith,
        CancellationToken? cancellationToken = null) =>
        optionTask.MatchAsync(Option<T>.Some, ifNoneWith, cancellationToken);
}
