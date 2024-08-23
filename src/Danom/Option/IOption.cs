namespace Danom;

/// <summary>
/// Contains operations for working with options.
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
