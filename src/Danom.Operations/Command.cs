using System.Threading.Tasks;

namespace Danom.Operations;
/// <summary>
/// Handles a command that does not return a value.
/// </summary>
/// <typeparam name="T">The command type.</typeparam>
public interface ICommandHandler<T> {
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <returns><see cref="Unit"/> when handling succeeds.</returns>
    Unit Handle(T command);
}

/// <summary>
/// Handles a command that returns a value.
/// </summary>
/// <typeparam name="T">The command type.</typeparam>
/// <typeparam name="TOutput">The output type returned by the handler.</typeparam>
public interface ICommandHandler<T, TOutput> {
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <returns>The handler output.</returns>
    TOutput Handle(T command);
}

/// <summary>
/// Handles a command asynchronously that does not return a value.
/// </summary>
/// <typeparam name="T">The command type.</typeparam>
public interface ICommandHandlerAsync<T> {
    /// <summary>
    /// Handles the command asynchronously.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <returns>A task that resolves to <see cref="Unit"/> when handling succeeds.</returns>
    Task<Unit> HandleAsync(T command);
}

/// <summary>
/// Handles a command asynchronously and returns a value.
/// </summary>
/// <typeparam name="T">The command type.</typeparam>
/// <typeparam name="TOutput">The output type returned by the handler.</typeparam>
public interface ICommandHandlerAsync<T, TOutput> {
    /// <summary>
    /// Handles the command asynchronously.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <returns>A task that resolves to the handler output.</returns>
    Task<TOutput> HandleAsync(T command);
}
