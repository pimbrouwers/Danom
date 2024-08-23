namespace Danom;

/// <summary>
/// Represents when an actual value might not exist for a value or named
/// variable. An option has an underlying type and can hold a value of that
/// type, or it might not have a value.
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

/// <inheritdoc />
public sealed class Option<T>
    : Choice<T, Unit>, IOption<T>
{
    internal Option(T t) : base(t) { }

    internal Option() : base(Unit.Value) { }

    public bool IsSome => _isT1;
    public bool IsNone => _isT2;

    public static IOption<T> Some(T value) =>
        new Option<T>(value);

    public static Task<IOption<T>> SomeAsync(T value) =>
        Task.FromResult(Some(value));

    public static async Task<IOption<T>> SomeAsync(Task<T> valueTask) =>
        Some(await valueTask);

    public static IOption<T> None() =>
        new Option<T>();

    public static Task<IOption<T>> NoneAsync() =>
        Task.FromResult(None());

    public U Match<U>(Func<T, U> some, Func<U> none) =>
        base.Match(some, _ => none());

    public IOption<U> Bind<U>(
        Func<T, IOption<U>> bind) =>
        Match(bind, Option<U>.None);

    public IOption<U> Map<U>(
        Func<T, U> map) =>
        Bind(x => Option<U>.Some(map(x)));

    public IOption<T> OrElse(
        IOption<T> ifNone) =>
        Match(Option<T>.Some, () => ifNone);

    public IOption<T> OrElseWith(
        Func<IOption<T>> ifNoneWith) =>
        Match(Option<T>.Some, ifNoneWith);

    public override string ToString() =>
        Match(
            some: x => $"Some({x})",
            none: () => "None");
}

public static class Option
{
    public static IOption<T> Some<T>(T value) =>
         new Option<T>(value);

    public static Task<IOption<T>> SomeAsync<T>(T value) =>
        Task.FromResult(Some(value));

    public static async Task<IOption<T>> SomeAsync<T>(Task<T> valueTask) =>
        Some(await valueTask);
}
