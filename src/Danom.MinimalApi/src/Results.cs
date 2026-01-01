namespace Danom.MinimalApi;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;

/// <summary>
/// An <see cref="IResult"/> that represents an Option result.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="input"></param>
/// <param name="noneResult"></param>
public class OptionHttpResult<T>(Option<T> input, Func<IResult>? noneResult = default)
    : OptionHttpResult<T, IResult>(input, noneResult) {
}

/// <summary>
/// An <see cref="IResult"/> that represents an Option result.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TNotFound"></typeparam>
/// <param name="input"></param>
/// <param name="noneResult"></param>
public class OptionHttpResult<T, [DynamicallyAccessedMembers(Methods)] TNotFound>(Option<T> input, Func<TNotFound>? noneResult = default)
    : IResult, IEndpointMetadataProvider, IStatusCodeHttpResult, IValueHttpResult, IValueHttpResult<T>
    where TNotFound : IResult {
    private const DynamicallyAccessedMemberTypes Methods =
        DynamicallyAccessedMemberTypes.PublicMethods |
        DynamicallyAccessedMemberTypes.NonPublicMethods;

    /// <summary>
    /// Gets the HTTP status code that will be returned by this result.
    /// </summary>
    public int? StatusCode => input.IsSome ? StatusCodes.Status200OK : StatusCodes.Status404NotFound;

    /// <summary>
    /// Gets the value contained in the Option, or null if the Option is None.
    /// </summary>
    public object? Value => input.TryGet(out var x) ? x : default;

    T? IValueHttpResult<T>.Value => Value is T value ? value : default;

    /// <summary>
    /// Executes the result by writing to the given <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public Task ExecuteAsync(HttpContext httpContext) {
        if (input.TryGet(out var value)) {
            return Results.Ok(value).ExecuteAsync(httpContext);
        }
        else if (noneResult is not null) {
            return noneResult.Invoke().ExecuteAsync(httpContext);
        }
        else {
            return Results.NotFound().ExecuteAsync(httpContext);
        }
    }

#if NET7_0_OR_GREATER
    /// <summary>
    /// Populates the endpoint metadata for this result.
    /// </summary>
    /// <param name="method"></param>
    /// <param name="builder"></param>
    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder) {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentNullException.ThrowIfNull(builder);

        PopulateMetadataInternal(method, builder.Metadata, builder.ApplicationServices);
    }
#else
    /// <summary>
    /// Populates the endpoint metadata for this result.
    /// </summary>
    /// <param name="method"></param>
    /// <param name="metadata"></param>
    /// <param name="services"></param>
    public static void PopulateMetadata(MethodInfo method, IList<object> metadata, IServiceProvider services) {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentNullException.ThrowIfNull(metadata);
        ArgumentNullException.ThrowIfNull(services);

        PopulateMetadataInternal(method, metadata, services);
    }
#endif

    private static void PopulateMetadataInternal(MethodInfo method, IList<object> metadata, IServiceProvider services) {
        metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status200OK, typeof(T), ["application/json"]));
        metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status404NotFound, null, ["application/json"]));
    }
}

/// <summary>
/// An <see cref="IResult"/> that represents a Result result.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TError"></typeparam>
/// <param name="input"></param>
/// <param name="errorResult"></param>
public sealed class ResultHttpResult<T, TError>(Result<T, TError> input, Func<TError, IResult>? errorResult = default)
    : ResultHttpResult<T, TError, IResult>(input, errorResult) {
}

/// <summary>
/// An <see cref="IResult"/> that represents a Result result.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TError"></typeparam>
/// <typeparam name="TNotFound"></typeparam>
/// <param name="input"></param>
/// <param name="errorResult"></param>
public class ResultHttpResult<T, TError, [DynamicallyAccessedMembers(Methods)] TNotFound>(Result<T, TError> input, Func<TError, TNotFound>? errorResult = default)
    : IResult, IEndpointMetadataProvider, IStatusCodeHttpResult, IValueHttpResult, IValueHttpResult<T>
    where TNotFound : IResult {
    private const DynamicallyAccessedMemberTypes Methods =
        DynamicallyAccessedMemberTypes.PublicMethods |
        DynamicallyAccessedMemberTypes.NonPublicMethods;
    /// <summary>
    /// Gets the HTTP status code that will be returned by this result.
    /// </summary>
    public int? StatusCode => input.IsOk ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;

    /// <summary>
    /// Gets the value contained in the Result if it is Ok, or null if it is an Error.
    /// </summary>
    public object? Value => input.TryGet(out var x) ? x : default;

    T? IValueHttpResult<T>.Value => Value is T value ? value : default;

    /// <summary>
    /// Executes the result by writing to the given <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public Task ExecuteAsync(HttpContext httpContext) {
        if (input.TryGet(out var value)) {
            return Results.Ok(value).ExecuteAsync(httpContext);
        }

        if (input.TryGetError(out var error) && errorResult is not null) {
            return errorResult.Invoke(error).ExecuteAsync(httpContext);

        }

        return Results.BadRequest(error).ExecuteAsync(httpContext);
    }

#if NET7_0_OR_GREATER
    /// <summary>
    /// Populates the endpoint metadata for this result.
    /// </summary>
    /// <param name="method"></param>
    /// <param name="builder"></param>
    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder) {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentNullException.ThrowIfNull(builder);

        PopulateMetadataInternal(method, builder.Metadata, builder.ApplicationServices);
    }
#else
    /// <summary>
    /// Populates the endpoint metadata for this result.
    /// </summary>
    public static void PopulateMetadata(MethodInfo method, IList<object> metadata, IServiceProvider services) {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentNullException.ThrowIfNull(metadata);
        ArgumentNullException.ThrowIfNull(services);

        PopulateMetadataInternal(method, metadata, services);
    }
#endif

    private static void PopulateMetadataInternal(MethodInfo method, IList<object> metadata, IServiceProvider services) {
        metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status200OK, typeof(T), ["application/json"]));
        metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status400BadRequest, typeof(TError), ["application/json"]));
    }
}
