namespace Danom;

using System;

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
        Match(some, _ => none());

    public IOption<U> Bind<U>(
        Func<T, IOption<U>> bind) =>
        Match(bind, Option<U>.None);

    public IOption<U> Map<U>(
        Func<T, U> map) =>
        Bind(x => Option<U>.Some(map(x)));

    public T DefaultValue(
         T defaultValue) =>
         Match(some => some, () => defaultValue);

    public T DefaultWith(
        Func<T> defaultWith) =>
        Match(some => some, () => defaultWith());

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
