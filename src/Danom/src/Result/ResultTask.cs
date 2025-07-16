namespace Danom
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

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
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<U> MatchAsync<T, TError, U>(
            this Task<Result<T, TError>> resultTask,
            Func<T, Task<U>> ok,
            Func<TError, Task<U>> error,
            CancellationToken? cancellationToken = null)
        {
            var option = await resultTask.WaitOrCancel(cancellationToken);
            return await option.Match(ok, error).WaitOrCancel(cancellationToken);
        }

        /// <summary>
        /// If Result is Ok evaluate the ok delegate, otherwise error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="resultTask"></param>
        /// <param name="ok"></param>
        /// <param name="error"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<U> MatchAsync<T, TError, U>(
            this Task<Result<T, TError>> resultTask,
            Func<T, U> ok,
            Func<TError, U> error,
            CancellationToken? cancellationToken = null) =>
            (await resultTask.WaitOrCancel(cancellationToken)).Match(ok, error);

        /// <summary>
        /// Evaluates the bind delegate if Result is Ok otherwise return Error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="resultTask"></param>
        /// <param name="bind"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<Result<U, TError>> BindAsync<T, TError, U>(
            this Task<Result<T, TError>> resultTask,
            Func<T, Result<U, TError>> bind,
            CancellationToken? cancellationToken = null) =>
            resultTask.MatchAsync(x => bind(x), Result<U, TError>.Error, cancellationToken);

        /// <summary>
        /// Evaluates the bind delegate if Result is Ok otherwise return Error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="resultTask"></param>
        /// <param name="bind"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<Result<U, TError>> BindAsync<T, TError, U>(
            this Task<Result<T, TError>> resultTask,
            Func<T, Task<Result<U, TError>>> bind,
            CancellationToken? cancellationToken = null) =>
            resultTask.MatchAsync(bind, e => Task.FromResult(Result<U, TError>.Error(e)), cancellationToken);

        /// <summary>
        /// Evaluates the bind delegate if Result is Ok otherwise return Error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="result"></param>
        /// <param name="bind"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<Result<U, TError>> BindAsync<T, TError, U>(
            this Result<T, TError> result,
            Func<T, Task<Result<U, TError>>> bind,
            CancellationToken? cancellationToken = null) =>
            Task.FromResult(result).BindAsync(bind, cancellationToken);

        /// <summary>
        /// Evaluates the map delegate if Result is Ok otherwise return Error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="resultTask"></param>
        /// <param name="map"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<Result<U, TError>> MapAsync<T, TError, U>(
            this Task<Result<T, TError>> resultTask,
            Func<T, U> map,
            CancellationToken? cancellationToken = null) =>
            resultTask.BindAsync(x => Result<U, TError>.Ok(map(x)), cancellationToken);

        /// <summary>
        /// Evaluates the map delegate if Result is Ok otherwise return Error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="resultTask"></param>
        /// <param name="map"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<Result<U, TError>> MapAsync<T, TError, U>(
            this Task<Result<T, TError>> resultTask,
            Func<T, Task<U>> map,
            CancellationToken? cancellationToken = null) =>
            resultTask.BindAsync(x => Result<U, TError>.OkAsync(map(x)), cancellationToken);

        /// <summary>
        /// Evaluates the map delegate if Result is Ok otherwise return Error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="result"></param>
        /// <param name="map"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<Result<U, TError>> MapAsync<T, TError, U>(
            this Result<T, TError> result,
            Func<T, Task<U>> map,
            CancellationToken? cancellationToken = null) =>
            Task.FromResult(result).MapAsync(map, cancellationToken);

        /// <summary>
        /// Evaluates the mapError delegate if Result is Error otherwise return Ok.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="UError"></typeparam>
        /// <param name="resultTask"></param>
        /// <param name="mapError"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<Result<T, UError>> MapErrorAsync<T, UError>(
            this Task<Result<T, UError>> resultTask,
            Func<UError, UError> mapError,
            CancellationToken? cancellationToken = null) =>
            resultTask.MatchAsync(Result<T, UError>.Ok, e => Result<T, UError>.Error(mapError(e)), cancellationToken);

        /// <summary>
        /// Evaluates the mapError delegate if Result is Error otherwise return Ok.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="UError"></typeparam>
        /// <param name="resultTask"></param>
        /// <param name="mapError"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<Result<T, UError>> MapErrorAsync<T, UError>(
            this Task<Result<T, UError>> resultTask,
            Func<UError, Task<UError>> mapError,
            CancellationToken? cancellationToken = null) =>
            resultTask.MatchAsync(x => Task.FromResult(Result<T, UError>.Ok(x)), e => Result<T, UError>.ErrorAsync(mapError(e)), cancellationToken);

        /// <summary>
        /// Evaluates the mapError delegate if Result is Error otherwise return Ok.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="UError"></typeparam>
        /// <param name="result"></param>
        /// <param name="mapError"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<Result<T, UError>> MapErrorAsync<T, UError>(
            this Result<T, UError> result,
            Func<UError, Task<UError>> mapError,
            CancellationToken? cancellationToken = null) =>
            Task.FromResult(result).MapErrorAsync(mapError, cancellationToken);

        /// <summary>
        /// Returns the value of Result if it is T, otherwise returns the
        /// specified default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="resultTask"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<T> DefaultValueAsync<T, TError>(
            this Task<Result<T, TError>> resultTask,
            T defaultValue,
            CancellationToken? cancellationToken = null) =>
            resultTask.MatchAsync(ok => ok, _ => defaultValue, cancellationToken);

        /// <summary>
        /// Returns the value of Result if it is T, otherwise returns the
        /// specified default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="resultTask"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<T> DefaultValueAsync<T, TError>(
            this Task<Result<T, TError>> resultTask,
            Task<T> defaultValue,
            CancellationToken? cancellationToken = null) =>
            resultTask.MatchAsync(some => Task.FromResult(some), _ => defaultValue, cancellationToken);

        /// <summary>
        /// Returns the value of Result if it is T, otherwise returns the
        /// specified default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="result"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<T> DefaultValueAsync<T, TError>(
            this Result<T, TError> result,
            Task<T> defaultValue,
            CancellationToken? cancellationToken = null) =>
            Task.FromResult(result).DefaultValueAsync(defaultValue, cancellationToken);

        /// <summary>
        /// Returns the value of Result if it is T, otherwise returns the
        /// specified default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="resultTask"></param>
        /// <param name="defaultWith"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<T> DefaultWithAsync<T, TError>(
            this Task<Result<T, TError>> resultTask,
            Func<Task<T>> defaultWith,
            CancellationToken? cancellationToken = null) =>
            resultTask.MatchAsync(ok => Task.FromResult(ok), _ => defaultWith(), cancellationToken);

        /// <summary>
        /// Returns the value of Result if it is T, otherwise returns the
        /// specified default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="resultTask"></param>
        /// <param name="defaultWith"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<T> DefaultWithAsync<T, TError>(
            this Task<Result<T, TError>> resultTask,
            Func<T> defaultWith,
            CancellationToken? cancellationToken = null) =>
            resultTask.MatchAsync(ok => ok, _ => defaultWith(), cancellationToken);

        /// <summary>
        /// Returns the value of Result if it is T, otherwise returns the
        /// specified default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="result"></param>
        /// <param name="defaultWith"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<T> DefaultWithAsync<T, TError>(
            this Result<T, TError> result,
            Func<Task<T>> defaultWith,
            CancellationToken? cancellationToken = null) =>
            Task.FromResult(result).DefaultWithAsync(defaultWith, cancellationToken);
    }
}
