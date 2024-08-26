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
    public ResultError(IEnumerable<string> errors)
        : this(string.Empty, errors) { }

    public ResultError(string key, string error)
        : this(key, [error]) { }

    public ResultError(string error)
        : this([error]) { }

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

    public ResultErrors() { }

    public ResultErrors(IEnumerable<ResultError> errors) =>
        _errors = errors.ToList();

    public ResultErrors(IEnumerable<string> errors)
        : this(errors.Select(m => new ResultError(m))) { }

    public ResultErrors(string key, string error)
        : this([new ResultError(key, error)]) { }

    public ResultErrors(string error)
        : this([new ResultError(error)]) { }

    /// <summary>
    /// Adds a new error to the collection.
    /// </summary>
    /// <param name="error"></param>
    public void Add(ResultError error) =>
        _errors.Add(error);

    public IEnumerator<ResultError> GetEnumerator() =>
        _errors.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    public override string ToString()
    {
        var errors = string.Join(Environment.NewLine, _errors);
        return string.Join(Environment.NewLine, ["[", errors, "]"]);
    }
}

/// <summary>
/// The <see cref="Result{T, TError}"/> with <see cref="ResultErrors"/>
/// as the predefined error type.
///
/// Alias for <see cref="Result{T, ResultErrors}"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public static class Result<T>
{
    /// <summary>
    /// Creates a new Result with the specified value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Result<T, ResultErrors> Ok(T value) =>
        Result<T, ResultErrors>.Ok(value);

    /// <summary>
    /// Creates Result with the specified value wrapped in a completed Task.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Task<Result<T, ResultErrors>> OkAsync(T value) =>
        Result<T, ResultErrors>.OkAsync(value);

    /// <summary>
    /// Creates Result with the value of the awaited Task.
    /// </summary>
    /// <param name="valueTask"></param>
    /// <returns></returns>
    public static Task<Result<T, ResultErrors>> OkAsync(Task<T> valueTask) =>
        Result<T, ResultErrors>.OkAsync(valueTask);

    /// <summary>
    /// Creates a new Result with the specified error.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static Result<T, ResultErrors> Error(ResultErrors errors) =>
        Result<T, ResultErrors>.Error(errors);

    /// <summary>
    /// Creates Result with the specified error wrapped in a completed Task.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static Task<Result<T, ResultErrors>> ErrorAsync(ResultErrors errors) =>
        Task.FromResult(Error(errors));

    /// <summary>
    /// Creates a new Result with the specified error.
    /// </summary>
    /// <param name="messages"></param>
    /// <returns></returns>
    public static Result<T, ResultErrors> Error(IEnumerable<string> messages) =>
        Error(new ResultErrors(messages));

    /// <summary>
    /// Creates Result with the specified error wrapped in a completed Task.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Result<T, ResultErrors> Error(string message) =>
        Error([message]);

    /// <summary>
    /// Creates Result with the specified error wrapped in a completed Task.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Task<Result<T, ResultErrors>> ErrorAsync(string message) =>
        Task.FromResult(Error(message));
}

/// <summary>
/// Static extension methods for <see cref="IResult{T, TError}"/> instances with
/// <see cref="ResultErrors"/> as the error type.
/// </summary>
public static class ResultTExtensions
{
    public static U Match<T, U>(
        this Result<T, ResultErrors> result,
        Func<T, U> ok,
        Func<ResultErrors, U> error) =>
        result.Match(ok, error);

    public static Result<U, ResultErrors> Bind<T, U>(
        this Result<T, ResultErrors> result,
        Func<T, Result<U, ResultErrors>> bind) =>
        result.Match(
            ok: ok => bind(ok),
            error: Result<U, ResultErrors>.Error);

    public static Result<U, ResultErrors> Map<T, U>(
        this Result<T, ResultErrors> result,
        Func<T, U> map) =>
        result.Bind(x => Result<U, ResultErrors>.Ok(map(x)));

    public static async Task<U> MatchAsync<T, U>(
        this Task<Result<T, ResultErrors>> resultTask,
        Func<T, U> ok,
        Func<ResultErrors, U> error) =>
        (await resultTask).Match(ok, error);

    public static Task<Result<U, ResultErrors>> BindAsync<T, U>(
        this Task<Result<T, ResultErrors>> resultTask,
        Func<T, Result<U, ResultErrors>> bind) =>
        resultTask.MatchAsync(x => bind(x), Result<U, ResultErrors>.Error);

    public static Task<Result<U, ResultErrors>> MapAsync<T, U>(
        this Task<Result<T, ResultErrors>> resultTask,
        Func<T, U> map) =>
        resultTask.BindAsync(x => Result<U, ResultErrors>.Ok(map(x)));

    public static async Task<U> MatchAsync<T, U>(
        this Task<Result<T, ResultErrors>> resultTask,
        Func<T, Task<U>> ok,
        Func<ResultErrors, Task<U>> error) =>
        await (await resultTask).Match(ok, error);

    public static Task<Result<U, ResultErrors>> BindAsync<T, U>(
        this Task<Result<T, ResultErrors>> resultTask,
        Func<T, Task<Result<U, ResultErrors>>> bind) =>
        resultTask.MatchAsync(bind, Result<U, ResultErrors>.ErrorAsync);

    public static Task<Result<U, ResultErrors>> MapAsync<T, U>(
        this Task<Result<T, ResultErrors>> resultTask,
        Func<T, Task<U>> map) =>
        resultTask.BindAsync(x => Result<U, ResultErrors>.OkAsync(map(x)));
}
