namespace Danom;

/// <summary>
/// Represents the existence, or not, of a value. Provides greater safety than
/// using nulls, and allows for more expressive code with exhaustive matching.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IOption<T>
{
    bool IsSome { get; }
    bool IsNone { get; }
    U Match<U>(Func<T, U> some, Func<U> none);
    IOption<U> Bind<U>(Func<T, IOption<U>> bind);
    IOption<U> Map<U>(Func<T, U> map);
    T DefaultValue(T defaultValue);
    T DefaultWith(Func<T> defaultWith);
    IOption<T> OrElse(IOption<T> ifNone);
    IOption<T> OrElseWith(Func<IOption<T>> ifNoneWith);
}

/// <inheritdoc cref="IOption{T}" />
public sealed class Option<T>
    : Maybe<T, Unit>, IOption<T>
{
    internal Option(T t) : base(t) { }

    internal Option() : base(Unit.Value) { }

    /// <summary>
    /// Returns true if the Option is Some, false otherwise.
    /// </summary>
    public bool IsSome => _isT1;

    /// <summary>
    /// Returns true if the Option is None, false otherwise.
    /// </summary>
    public bool IsNone => _isT2;

    /// <summary>
    /// Creates a new Option with the specified value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IOption<T> Some(T value) =>
        new Option<T>(value);

    /// <summary>
    /// Creates a new Option with the specified value wrapped in a completed
    /// Task.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Task<IOption<T>> SomeAsync(T value) =>
        Task.FromResult(Some(value));

    /// <summary>
    /// Creates a new Option with the value of the awaited Task.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static async Task<IOption<T>> SomeAsync(Task<T> value) =>
        Some(await value);

    /// <summary>
    /// Creates a new Option with no value.
    /// </summary>
    /// <returns></returns>
    public static IOption<T> None() =>
        new Option<T>();

    /// <summary>
    /// Creates a new Option with no value wrapped in a completed Task.
    /// </summary>
    /// <returns></returns>
    public static Task<IOption<T>> NoneAsync() =>
        Task.FromResult(None());

    /// <summary>
    /// Evaluates the some delegate if the Option is Some, otherwise evaluates
    /// the none delegate.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="some"></param>
    /// <param name="none"></param>
    /// <returns></returns>
    public U Match<U>(Func<T, U> some, Func<U> none) =>
        Match(t1: some, t2: _ => none());

    /// <summary>
    /// Evaluates the bind delegate if the Option is Some, otherwise returns
    /// None.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="bind"></param>
    /// <returns></returns>
    public IOption<U> Bind<U>(
        Func<T, IOption<U>> bind) =>
        Match(bind, Option<U>.None);

    /// <summary>
    /// Evaluates the map delegate if the Option is Some, otherwise returns
    /// None.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="map"></param>
    /// <returns></returns>
    public IOption<U> Map<U>(
        Func<T, U> map) =>
        Bind(x => Option<U>.Some(map(x)));

    /// <summary>
    /// Returns the value of the Option if it is T, otherwise returns the
    /// specified default value.
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public T DefaultValue(
         T defaultValue) =>
         Match(some => some, _ => defaultValue);

    /// <summary>
    /// Returns the value of the Option if it is T, otherwise returns the
    /// </summary>
    /// <param name="defaultWith"></param>
    /// <returns></returns>
    public T DefaultWith(
        Func<T> defaultWith) =>
        Match(some => some, _ => defaultWith());

    /// <summary>
    /// Returns the value of the Option if it is Some, otherwise returns the
    /// specified ifNone value.
    /// </summary>
    /// <param name="ifNone"></param>
    /// <returns></returns>
    public IOption<T> OrElse(
        IOption<T> ifNone) =>
        Match(Option<T>.Some, () => ifNone);

    /// <summary>
    /// Returns the value of the Option if it is Some, otherwise returns the
    /// </summary>
    /// <param name="ifNoneWith"></param>
    /// <returns></returns>
    public IOption<T> OrElseWith(
        Func<IOption<T>> ifNoneWith) =>
        Match(Option<T>.Some, ifNoneWith);

    public override string ToString() =>
        Match(
            some: x => $"Some({x})",
            none: () => "None");
}

/// <summary>
/// Extension methods to allow Option matching using
/// </summary>
public static class OptionActionExtensions
{
    /// <summary>
    /// If the Option is Some, evaluates the some delegate, otherwise evaluates
    /// the none delegate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <param name="some"></param>
    /// <param name="none"></param>
    public static void Match<T>(this IOption<T> option, Action<T> some, Action none)
    {
        if (option.ToNullable() is T t)
        {
            some(t);
        }
        else
        {
            none();
        }
    }
}
