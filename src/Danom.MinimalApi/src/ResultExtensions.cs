namespace Danom.MinimalApi;

using Microsoft.AspNetCore.Http;

/// <summary>
/// Extensions for handling Danom results in minimal APIs.
/// Provides methods to convert Danom types to ASP.NET Core's `IResult`.
/// </summary>
public static class DanomResultExtensions {
    /// <summary>
    /// Converts an Option to an `IResult`. Returns a 200 OK result with the
    /// value if the option is Some, or a 404 Not Found result if the option is
    /// None.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resultExtensions"></param>
    /// <param name="option"></param>
    /// <param name="noneResult"></param>
    /// <returns></returns>
    public static IResult Option<T>(
        this IResultExtensions resultExtensions,
        Option<T> option,
        Func<IResult>? noneResult = default) {
        ArgumentNullException.ThrowIfNull(resultExtensions);
        return new OptionHttpResult<T>(option, noneResult);
    }


    /// <summary>
    /// Converts a Result to an `IResult`. Returns a 200 OK result with the
    /// value if the result is Ok, or invokes handleError if the result is an
    /// Error.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="resultExtensions"></param>
    /// <param name="result"></param>
    /// <param name="errorResult"></param>
    /// <returns></returns>
    public static IResult Result<T, TError>(
        this IResultExtensions resultExtensions,
        Result<T, TError> result,
        Func<TError, IResult>? errorResult = default) {
        ArgumentNullException.ThrowIfNull(resultExtensions);
        return new ResultHttpResult<T, TError>(result, errorResult);
    }
}
