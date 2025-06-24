namespace Danom.MinimalApi;

using Microsoft.AspNetCore.Http;

/// <summary>
/// Extensions for handling Danom results in minimal APIs.
/// Provides methods to convert Danom types to ASP.NET Core's `IResult`.
/// </summary>
public static class DanomResultExtensions
{
    /// <summary>
    /// Converts an Option to an `IResult`. Returns a 200 OK result with the
    /// value if the option is Some, or a 404 Not Found result if the option is
    /// None.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="option"></param>
    /// <param name="noneResult"></param>
    /// <returns></returns>
    public static IResult Option<T>(
        this IResultExtensions _,
        Option<T> option,
        Func<IResult>? noneResult = default) =>
        option.Match(
            some: value => Results.Ok(value),
            none: () => noneResult?.Invoke() ?? Results.NotFound());

    /// <summary>
    /// Converts a Result to an `IResult`. Returns a 200 OK result with the
    /// value if the result is Ok, or invokes handleError if the result is an
    /// Error.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="_"></param>
    /// <param name="result"></param>
    /// <param name="errorResult"></param>
    /// <returns></returns>
    public static IResult Result<T, TError>(
        this IResultExtensions _,
        Result<T, TError> result,
        Func<TError, IResult>? errorResult = default) =>
        result.Match(
            ok: value => Results.Ok(value),
            error: error => errorResult?.Invoke(error) ?? Results.BadRequest(error));
}
