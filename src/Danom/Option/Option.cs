namespace Danom;

/// <summary>
/// Represents the existence, or not, of a value. Provides greater safety than
/// using nulls, and allows for more expressive code with exhaustive matching.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct Option<T>
    : IEquatable<Option<T>>
{
    private readonly T? _some = default;

    private Option(T t)
    {
        if (t is not null)
        {
            _some = t;
            IsSome = true;
        }
    }

    /// <summary>
    /// Returns true if Option is Some, false otherwise.
    /// </summary>
    public bool IsSome { get; } = false;

    /// <summary>
    /// Returns true if Option is None, false otherwise.
    /// </summary>
    public bool IsNone => !IsSome;

    /// <summary>
    /// If Option is Some evaluate the some delegate, otherwise none.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="some"></param>
    /// <param name="none"></param>
    /// <returns></returns>
    public U Match<U>(Func<T, U> some, Func<U> none) =>
        IsSome && _some is T t ?
            some(t) :
            none();

    /// <summary>
    /// Evaluates the bind delegate if Option is Some otherwise return None.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="bind"></param>
    /// <returns></returns>
    public Option<U> Bind<U>(
        Func<T, Option<U>> bind) =>
        Match(bind, Option<U>.None);

    /// <summary>
    /// Evaluates the map delegate if Option is Some otherwise return None.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <param name="map"></param>
    /// <returns></returns>
    public Option<U> Map<U>(
        Func<T, U> map) =>
        Bind(x => Option<U>.Some(map(x)));

    /// <summary>
    /// Returns the value of Option if it is T otherwise return default.
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public T DefaultValue(
         T defaultValue) =>
         Match(some => some, () => defaultValue);

    /// <summary>
    /// Returns the value of Option if it is T otherwise evaluate default.
    /// </summary>
    /// <param name="defaultWith"></param>
    /// <returns></returns>
    public T DefaultWith(
        Func<T> defaultWith) =>
        Match(some => some, () => defaultWith());

    /// <summary>
    /// Return Option if it is Some, otherwise return ifNone.
    /// </summary>
    /// <param name="ifNone"></param>
    /// <returns></returns>
    public Option<T> OrElse(
        Option<T> ifNone) =>
        Match(Option<T>.Some, () => ifNone);

    /// <summary>
    /// Return Option if it is Some, otherwise evaluate ifNoneWith.
    /// </summary>
    /// <param name="ifNoneWith"></param>
    /// <returns></returns>
    public Option<T> OrElseWith(
        Func<Option<T>> ifNoneWith) =>
        Match(Option<T>.Some, ifNoneWith);


    /// <summary>
    /// Creates a new Option with the specified value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Option<T> Some(T value) =>
        new Option<T>(value);

    /// <summary>
    /// Creates Option with the specified value wrapped in a completed Task.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Task<Option<T>> SomeAsync(T value) =>
        Task.FromResult(Some(value));

    /// <summary>
    /// Creates a new Option with the value of the awaited Task.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static async Task<Option<T>> SomeAsync(Task<T> value) =>
        Some(await value);

    /// <summary>
    /// Creates a new Option with no value.
    /// </summary>
    /// <returns></returns>
    public static Option<T> None() =>
        new Option<T>();

    /// <summary>
    /// Creates a new Option with no value wrapped in a completed Task.
    /// </summary>
    /// <returns></returns>
    public static Task<Option<T>> NoneAsync() =>
        Task.FromResult(None());

    public static bool operator ==(Option<T> left, Option<T> right) =>
        left.Equals(right);

    public static bool operator !=(Option<T> left, Option<T> right) =>
        !(left == right);

    public override bool Equals(object? obj) =>
        obj is Option<T> o && Equals(o);

    public readonly bool Equals(Option<T> other) =>
        Match(
            some: x1 =>
                other.Match(
                    some: x2 => x1 is not null && x2 is not null && x2.Equals(x1),
                    none: () => false),
            none: () =>
                other.Match(
                    some: _ => false,
                    none: () => true)
            );

    public override int GetHashCode() =>
        Match(
            some: x => x is null ? 0 : x.GetHashCode(),
            none: () => 0);

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
    /// If Option is Some, evaluates the some delegate, otherwise evaluates
    /// the none delegate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <param name="some"></param>
    /// <param name="none"></param>
    public static void Match<T>(this Option<T> option, Action<T> some, Action none)
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
