namespace Danom;

using System.Collections;

/// <summary>
/// Represents a collection of optionally keyed errors.
/// </summary>
/// <param name="Key"></param>
/// <param name="Errors"></param>
public sealed record ResultError(
    string Key,
    IEnumerable<string> Errors)
{

    /// <summary>
    /// Creates a new instance of <see cref="ResultError"/> from the specified
    /// errors.
    /// </summary>
    /// <param name="errors"></param>
    public ResultError(IEnumerable<string> errors)
        : this(string.Empty, errors) { }

    /// <summary>
    /// Creates a new instance of <see cref="ResultError"/> from the specified
    /// key and error.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="error"></param>
    public ResultError(string key, string error)
        : this(key, [error]) { }

    /// <summary>
    /// Creates a new instance of <see cref="ResultError"/> from the specified
    /// error.
    /// </summary>
    /// <param name="error"></param>
    public ResultError(string error)
        : this([error]) { }

    /// <summary>
    /// Returns a string representation of the <see cref="ResultError"/>.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var errors = string.Join(", ", Errors);
        return string.IsNullOrWhiteSpace(Key) ?
            errors :
            $"{Key} - {errors}";
    }
}

/// <summary>
/// Represents a collection of <see cref="ResultError"/> instances.
/// </summary>
public sealed class ResultErrors : IEnumerable<ResultError>
{
    private readonly List<ResultError> _errors = [];

    /// <summary>
    /// Creates a new empty instance of <see cref="ResultErrors"/>.
    /// </summary>
    public ResultErrors() { }

    /// <summary>
    /// Creates a new instance of <see cref="ResultErrors"/> from the specified
    /// errors.
    /// </summary>
    /// <param name="errors"></param>
    public ResultErrors(IEnumerable<ResultError> errors) =>
        _errors = errors.ToList();

    /// <summary>
    /// Creates a new instance of <see cref="ResultErrors"/> from the specified
    /// error strings.
    /// </summary>
    /// <param name="errors"></param>
    public ResultErrors(IEnumerable<string> errors)
        : this(errors.Select(m => new ResultError(m))) { }

    /// <summary>
    /// Creates a new instance of <see cref="ResultErrors"/> from the specified
    /// key and error.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="error"></param>
    public ResultErrors(string key, string error)
        : this([new ResultError(key, error)]) { }

    /// <summary>
    /// Creates a new instance of <see cref="ResultErrors"/> from the specified
    /// error.
    /// </summary>
    /// <param name="error"></param>
    public ResultErrors(string error)
        : this([new ResultError(error)]) { }

    /// <summary>
    /// Adds a new error to the collection.
    /// </summary>
    /// <param name="error"></param>
    public void Add(ResultError error) =>
        _errors.Add(error);

    /// <summary>
    /// Returns the enumerator for the collection.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<ResultError> GetEnumerator() =>
        _errors.GetEnumerator();

    /// <summary>
    /// Returns the enumerator for the collection.
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    /// <summary>
    /// Returns a string representation of the <see cref="ResultErrors"/>.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var errors = string.Join(Environment.NewLine, _errors);
        return string.Join(Environment.NewLine, ["[", errors, "]"]);
    }
}

// /// <summary>
// /// Static extension methods for <see cref="Result{T, TError}"/> instances with
// /// <see cref="ResultErrors"/> as the error type.
// /// </summary>
// public static class ResultTExtensions
// {
//     /// <summary>
//     /// If Result is Ok evaluate the ok delegate, otherwise error.
//     /// </summary>
//     /// <typeparam name="T"></typeparam>
//     /// <typeparam name="U"></typeparam>
//     /// <param name="result"></param>
//     /// <param name="ok"></param>
//     /// <param name="error"></param>
//     /// <returns></returns>
//     public static U Match<T, U>(
//         this Result<T, ResultErrors> result,
//         Func<T, U> ok,
//         Func<ResultErrors, U> error) =>
//         result.Match(ok, error);

//     /// <summary>
//     /// Evaluates the bind delegate if Result is Ok otherwise return Error.
//     /// </summary>
//     /// <typeparam name="T"></typeparam>
//     /// <typeparam name="U"></typeparam>
//     /// <param name="result"></param>
//     /// <param name="bind"></param>
//     /// <returns></returns>
//     public static Result<U, ResultErrors> Bind<T, U>(
//         this Result<T, ResultErrors> result,
//         Func<T, Result<U, ResultErrors>> bind) =>
//         result.Match(
//             ok: ok => bind(ok),
//             error: Result<U, ResultErrors>.Error);

//     /// <summary>
//     /// Evaluates the map delegate if Result is Ok otherwise return Error.
//     /// </summary>
//     /// <typeparam name="T"></typeparam>
//     /// <typeparam name="U"></typeparam>
//     /// <param name="result"></param>
//     /// <param name="map"></param>
//     /// <returns></returns>
//     public static Result<U, ResultErrors> Map<T, U>(
//         this Result<T, ResultErrors> result,
//         Func<T, U> map) =>
//         result.Bind(x => Result<U, ResultErrors>.Ok(map(x)));

//     /// <summary>
//     /// Evaluates the mapError delegate if Result is Error otherwise return Ok.
//     /// </summary>
//     /// <typeparam name="T"></typeparam>
//     /// <typeparam name="UError"></typeparam>
//     /// <param name="result"></param>
//     /// <param name="mapError"></param>
//     /// <returns></returns>
//     public static Result<T, UError> MapError<T, UError>(
//         this Result<T, ResultErrors> result,
//         Func<ResultErrors, UError> mapError) =>
//         result.Match(Result<T, UError>.Ok, e => Result<T, UError>.Error(mapError(e)));
// }

// /// <summary>
// /// Static extension methods for <see cref="Result{T, TError}"/> instances with
// /// <see cref="ResultErrors"/> as the error type.
// /// </summary>
// public static class ResultTTaskExtensions
// {
//     /// <summary>
//     /// If Result is Ok evaluate the ok delegate, otherwise error.
//     /// </summary>
//     /// <typeparam name="T"></typeparam>
//     /// <typeparam name="U"></typeparam>
//     /// <param name="resultTask"></param>
//     /// <param name="ok"></param>
//     /// <param name="error"></param>
//     /// <returns></returns>
//     public static async Task<U> MatchAsync<T, U>(
//         this Task<Result<T, ResultErrors>> resultTask,
//         Func<T, U> ok,
//         Func<ResultErrors, U> error) =>
//         (await resultTask).Match(ok, error);

//     /// <summary>
//     /// Evaluates the map delegate if Result is Ok otherwise return Error.
//     /// </summary>
//     /// <typeparam name="T"></typeparam>
//     /// <typeparam name="U"></typeparam>
//     /// <param name="resultTask"></param>
//     /// <param name="ok"></param>
//     /// <param name="error"></param>
//     /// <returns></returns>
//     public static async Task<U> MatchAsync<T, U>(
//         this Task<Result<T, ResultErrors>> resultTask,
//         Func<T, Task<U>> ok,
//         Func<ResultErrors, Task<U>> error) =>
//         await (await resultTask).Match(ok, error);

//     /// <summary>
//     /// Evaluates the bind delegate if Result is Ok otherwise return Error.
//     /// </summary>
//     /// <typeparam name="T"></typeparam>
//     /// <typeparam name="U"></typeparam>
//     /// <param name="resultTask"></param>
//     /// <param name="bind"></param>
//     /// <returns></returns>
//     public static Task<Result<U, ResultErrors>> BindAsync<T, U>(
//         this Task<Result<T, ResultErrors>> resultTask,
//         Func<T, Result<U, ResultErrors>> bind) =>
//         resultTask.MatchAsync(x => bind(x), Result<U, ResultErrors>.Error);

//     /// <summary>
//     /// Evaluates the bind delegate if Result is Ok otherwise return Error.
//     /// </summary>
//     /// <typeparam name="T"></typeparam>
//     /// <typeparam name="U"></typeparam>
//     /// <param name="resultTask"></param>
//     /// <param name="bind"></param>
//     /// <returns></returns>
//     public static Task<Result<U, ResultErrors>> BindAsync<T, U>(
//         this Task<Result<T, ResultErrors>> resultTask,
//         Func<T, Task<Result<U, ResultErrors>>> bind) =>
//         resultTask.MatchAsync(bind, Result<U, ResultErrors>.ErrorAsync);

//     /// <summary>
//     /// Evaluates the map delegate if Result is Ok otherwise return Error.
//     /// </summary>
//     /// <typeparam name="T"></typeparam>
//     /// <typeparam name="U"></typeparam>
//     /// <param name="resultTask"></param>
//     /// <param name="map"></param>
//     /// <returns></returns>
//     public static Task<Result<U, ResultErrors>> MapAsync<T, U>(
//         this Task<Result<T, ResultErrors>> resultTask,
//         Func<T, U> map) =>
//         resultTask.BindAsync(x => Result<U, ResultErrors>.Ok(map(x)));


//     /// <summary>
//     /// Evaluates the map delegate if Result is Ok otherwise return Error.
//     /// </summary>
//     /// <typeparam name="T"></typeparam>
//     /// <typeparam name="U"></typeparam>
//     /// <param name="resultTask"></param>
//     /// <param name="map"></param>
//     /// <returns></returns>
//     public static Task<Result<U, ResultErrors>> MapAsync<T, U>(
//         this Task<Result<T, ResultErrors>> resultTask,
//         Func<T, Task<U>> map) =>
//         resultTask.BindAsync(x => Result<U, ResultErrors>.OkAsync(map(x)));

    // /// <summary>
    // /// Evaluates the mapError delegate if Result is Error otherwise return Ok.
    // /// </summary>
    // /// <typeparam name="T"></typeparam>
    // /// <typeparam name="UError"></typeparam>
    // /// <param name="resultTask"></param>
    // /// <param name="mapError"></param>
    // /// <returns></returns>
    // public static Task<Result<T, UError>> MapErrorAsync<T, UError>(
    //     this Task<Result<T, ResultErrors>> resultTask,
    //     Func<ResultErrors, Task<UError>> mapError) =>
    //     resultTask.MatchAsync(Result<T, UError>.OkAsync, e => Result<T, UError>.ErrorAsync(mapError(e)));
// }
