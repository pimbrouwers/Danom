using System.Threading.Tasks;

namespace Danom.Operations;
/// <summary>
/// Handles a query that does not require input parameters.
/// </summary>
/// <typeparam name="T">The query result type.</typeparam>
public interface IQueryHandler<T> {
    /// <summary>
    /// Handles the query.
    /// </summary>
    /// <returns>The query result.</returns>
    T Handle();
}

/// <summary>
/// Handles a query that requires input parameters.
/// </summary>
/// <typeparam name="T">The query input type.</typeparam>
/// <typeparam name="TOutput">The query result type.</typeparam>
public interface IQueryHandler<T, TOutput> {
    /// <summary>
    /// Handles the query.
    /// </summary>
    /// <param name="query">The query input.</param>
    /// <returns>The query result.</returns>
    TOutput Handle(T query);
}

/// <summary>
/// Handles a parameterless query asynchronously.
/// </summary>
/// <typeparam name="T">The query result type.</typeparam>
public interface IQueryHandlerAsync<T> {
    /// <summary>
    /// Handles the query asynchronously.
    /// </summary>
    /// <returns>A task that resolves to the query result.</returns>
    Task<T> Handle();
}

/// <summary>
/// Handles a query with input parameters asynchronously.
/// </summary>
/// <typeparam name="T">The query input type.</typeparam>
/// <typeparam name="TOutput">The query result type.</typeparam>
public interface IQueryHandlerAsync<T, TOutput> {
    /// <summary>
    /// Handles the query asynchronously.
    /// </summary>
    /// <param name="query">The query input.</param>
    /// <returns>A task that resolves to the query result.</returns>
    Task<TOutput> HandleAsync(T query);
}
