namespace Danom.MinimalApi.TypedResults;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

/// <summary>
/// Extensions for handling Danom results in minimal APIs using
/// <see cref="TypedResults"/>. Provides methods to convert Danom types to
/// ASP.NET Core's <see cref="Results{TResult1,TResult2}"/>.
/// </summary>
public static class TypedDanomResultExtensions
{
    private const DynamicallyAccessedMemberTypes Methods =
        DynamicallyAccessedMemberTypes.PublicMethods |
        DynamicallyAccessedMemberTypes.NonPublicMethods;

    /// <summary>
    /// Converts a <see cref="Option{T}"/> to a <see cref="Results{TResult1, TResult2}"/>.
    /// Returns a 200 OK result with the value if the <paramref name="option"/> is Some, or a <see cref="NotFound"/>
    /// if the <paramref name="option"/> is None.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="option">The option.</param>
    /// <typeparam name="T">The value type of the <paramref name="option"/>.</typeparam>
    /// <returns>The typed HTTP result.</returns>
    public static Results<Ok<T>, NotFound> Option<T>(
        this IResultExtensions _,
        Option<T> option) =>
        option.Match<Results<Ok<T>, NotFound>>(
            some: value => TypedResults.Ok(value),
            none: () => TypedResults.NotFound());

    /// <summary>
    /// Converts a <see cref="Option{T}"/> to a <see cref="Results{TResult1, TResult2}"/>.
    /// Returns a 200 OK result with the value if the <paramref name="option"/> is Some, or invokes
    /// <paramref name="noneResult"/> to create the <see cref="IResult"/> if the <paramref name="option"/> is None.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="option">The option.</param>
    /// <param name="noneResult">The factory function to create the <typeparamref name="TNotFoundResult"/>.</param>
    /// <typeparam name="T">The value type of the <paramref name="option"/>.</typeparam>
    /// <typeparam name="TNotFoundResult">The HTTP result type used when <paramref name="option"/> is empty.</typeparam>
    /// <returns>The typed HTTP result.</returns>
    public static Results<Ok<T>, TNotFoundResult> Option<T, [DynamicallyAccessedMembers(Methods)] TNotFoundResult>(
        this IResultExtensions _,
        Option<T> option,
        Func<TNotFoundResult> noneResult) where TNotFoundResult : IResult =>
        option.Match<Results<Ok<T>, TNotFoundResult>>(
            some: value => TypedResults.Ok(value),
            none: () => noneResult.Invoke());

    /// <summary>
    /// Converts a <see cref="Result{T, TError}"/> to a <see cref="Results{TResult1, TResult2}"/>.
    /// Returns a 200 OK result with the value if the <paramref name="result"/> is ok or a 400 BadRequest result
    /// if the <paramref name="result"/> is an error.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="result">The result.</param>
    /// <typeparam name="T">The value type of the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TError">The error type of the <paramref name="result"/>.</typeparam>
    /// <returns>The typed HTTP result.</returns>
    public static Results<Ok<T>, BadRequest<TError>> Result<T, TError>(
        this IResultExtensions _, Result<T, TError> result) =>
        result.Match<Results<Ok<T>, BadRequest<TError>>>(
            ok: value => TypedResults.Ok(value),
            error: error => TypedResults.BadRequest(error));

    /// <summary>
    /// Converts a <see cref="Result{T, TError}"/> to a <see cref="Results{TResult1, TResult2}"/>.
    /// Returns a 200 OK result with the value if the <paramref name="result"/> is ok or invokes
    /// <paramref name="errorResult"/> to create the <see cref="IResult"/> if the <paramref name="result"/> is an
    /// error.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="result">The result.</param>
    /// <param name="errorResult">
    /// Conversion from <typeparamref name="TError"/> to <typeparamref name="TErrorResult"/>.
    /// </param>
    /// <typeparam name="T">The value type of the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TError">The error type of the <paramref name="result"/>.</typeparam>
    /// <typeparam name="TErrorResult">The HTTP result type used when <paramref name="result"/> is an error.</typeparam>
    /// <returns>The typed HTTP result.</returns>
    public static Results<Ok<T>, TErrorResult> Result<T, TError, [DynamicallyAccessedMembers(Methods)] TErrorResult>(
        this IResultExtensions _, Result<T, TError> result, Func<TError, TErrorResult> errorResult)
        where TErrorResult : IResult =>
        result.Match<Results<Ok<T>, TErrorResult>>(
            ok: value => TypedResults.Ok(value),
            error: error => errorResult(error));
}
