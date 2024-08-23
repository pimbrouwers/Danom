namespace Danom;

using System.Collections;

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

public static class Result<T>
{
    public static IResult<T, ResultErrors> Ok(T value) =>
        Result<T, ResultErrors>.Ok(value);

    public static Task<IResult<T, ResultErrors>> OkAsync(T value) =>
        Result<T, ResultErrors>.OkAsync(value);

    public static Task<IResult<T, ResultErrors>> OkAsync(Task<T> valueTask) =>
        Result<T, ResultErrors>.OkAsync(valueTask);

    public static IResult<T, ResultErrors> Error(ResultErrors errors) =>
        Result<T, ResultErrors>.Error(errors);

    public static Task<IResult<T, ResultErrors>> ErrorAsync(ResultErrors errors) =>
        Task.FromResult(Error(errors));

    public static IResult<T, ResultErrors> Error(IEnumerable<string> messages) =>
        Error(new ResultErrors(messages));

    public static IResult<T, ResultErrors> Error(string message) =>
        Error([message]);


    public static Task<IResult<T, ResultErrors>> ErrorAsync(string message) =>
        Task.FromResult(Error(message));
}

public static class ResultTExtensions
{
    public static U Match<T, U>(
        this IResult<T, ResultErrors> result,
        Func<T, U> ok,
        Func<ResultErrors, U> error) =>
        result.Match(ok, error);

    public static IResult<U, ResultErrors> Bind<T, U>(
        this IResult<T, ResultErrors> result,
        Func<T, IResult<U, ResultErrors>> bind) =>
        result.Match(
            ok: ok => bind(ok),
            error: Result<U, ResultErrors>.Error);

    public static IResult<U, ResultErrors> Map<T, U>(
        this IResult<T, ResultErrors> result,
        Func<T, U> map) =>
        result.Bind(x => Result<U, ResultErrors>.Ok(map(x)));

    public static async Task<U> MatchAsync<T, U>(
        this Task<IResult<T, ResultErrors>> resultTask,
        Func<T, U> ok,
        Func<ResultErrors, U> error) =>
        (await resultTask).Match(ok, error);

    public static Task<IResult<U, ResultErrors>> BindAsync<T, U>(
        this Task<IResult<T, ResultErrors>> resultTask,
        Func<T, IResult<U, ResultErrors>> bind) =>
        resultTask.MatchAsync(x => bind(x), Result<U, ResultErrors>.Error);

    public static Task<IResult<U, ResultErrors>> MapAsync<T, U>(
        this Task<IResult<T, ResultErrors>> resultTask,
        Func<T, U> map) =>
        resultTask.BindAsync(x => Result<U, ResultErrors>.Ok(map(x)));

    public static async Task<U> MatchAsync<T, U>(
        this Task<IResult<T, ResultErrors>> resultTask,
        Func<T, Task<U>> ok,
        Func<ResultErrors, Task<U>> error) =>
        await (await resultTask).Match(ok, error);

    public static Task<IResult<U, ResultErrors>> BindAsync<T, U>(
        this Task<IResult<T, ResultErrors>> resultTask,
        Func<T, Task<IResult<U, ResultErrors>>> bind) =>
        resultTask.MatchAsync(bind, Result<U, ResultErrors>.ErrorAsync);

    public static Task<IResult<U, ResultErrors>> MapAsync<T, U>(
        this Task<IResult<T, ResultErrors>> resultTask,
        Func<T, Task<U>> map) =>
        resultTask.BindAsync(x => Result<U, ResultErrors>.OkAsync(map(x)));
}
