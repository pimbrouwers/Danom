namespace Danom.MinimalApi;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

/// <summary>
/// Extensions for handling Danom results in minimal APIs using
/// <see cref="TypedResults"/>. Provides methods to convert Danom types to
/// ASP.NET Core's <see cref="Results{TResult1,TResult2}"/>.
/// </summary>
public static class DanomHttpResults {
    private const DynamicallyAccessedMemberTypes Methods =
        DynamicallyAccessedMemberTypes.PublicMethods |
        DynamicallyAccessedMemberTypes.NonPublicMethods;

    /// <summary>
    /// Converts an Option to an <see cref="OptionHttpResult{T}"/>. Returns a
    /// 200 OK result with the value if the option is Some, or a 404 Not Found
    /// result if the option is None.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <param name="noneResult"></param>
    /// <returns></returns>
    public static OptionHttpResult<T> Option<T>(
        Option<T> option,
        Func<IResult>? noneResult = default) =>
        new(option, noneResult);


    /// <summary>
    /// Converts an Option to an <see cref="OptionHttpResult{T,TNotFoundResult}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TNotFoundResult"></typeparam>
    /// <param name="option"></param>
    /// <param name="noneResult"></param>
    /// <returns></returns>
    public static OptionHttpResult<T, TNotFoundResult> Option<T, [DynamicallyAccessedMembers(Methods)] TNotFoundResult>(
        Option<T> option,
        Func<TNotFoundResult> noneResult) where TNotFoundResult : IResult =>
        new(option, noneResult);

    /// <summary>
    /// Converts a Result to a <see cref="ResultHttpResult{T,TError}"/>. Returns a
    /// 200 OK result with the value if the result is Ok, or invokes handleError if
    /// the result is an Error.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="result"></param>
    /// <param name="errorResult"></param>
    /// <returns></returns>
    public static ResultHttpResult<T, TError> Result<T, TError>(
        Result<T, TError> result,
        Func<TError, IResult>? errorResult = default) =>
        new(result, errorResult);

    /// <summary>
    /// Converts a Result to a <see cref="ResultHttpResult{T,TError,TNotFoundResult}"/>. Returns a
    /// 200 OK result with the value if the result is Ok, or invokes handleError if
    /// the result is an Error.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <typeparam name="TNotFoundResult"></typeparam>
    /// <param name="result"></param>
    /// <param name="errorResult"></param>
    /// <returns></returns>
    public static ResultHttpResult<T, TError, TNotFoundResult> Result<T, TError, [DynamicallyAccessedMembers(Methods)] TNotFoundResult>(
        Result<T, TError> result,
        Func<TError, TNotFoundResult>? errorResult = default)
        where TNotFoundResult : IResult =>
        new(result, errorResult);
}
