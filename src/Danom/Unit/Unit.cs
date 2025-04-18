namespace Danom;

/// <summary>
/// The unit type is a type that indicates the absence of a specific value; the
/// unit type has only a single value, which acts as a placeholder when no other
/// value exists or is needed.
/// </summary>
public readonly struct Unit
    : IEquatable<Unit>
{
    /// <summary>
    /// The single value of the unit type.
    /// </summary>
    public static readonly Unit Value;

    /// <summary>
    /// A completed task that represents the single value of the unit type.
    /// </summary>
    public static readonly Task<Unit> ValueAsync = Task.FromResult(Value);

    /// <summary>
    /// Returns true if the specified unit is equal to the current unit.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(Unit left, Unit right) =>
        left.Equals(right);

    /// <summary>
    /// Returns true if the specified unit is not equal to the current unit.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(Unit left, Unit right) =>
        !(left == right);

    /// <summary>
    /// Returns true if the specified unit is equal to the current unit.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public readonly bool Equals(Unit other) =>
        true;

    /// <summary>
    /// Returns true if the specified object is equal to the current unit.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override readonly bool Equals(object? obj) =>
        obj is Unit;

    /// <summary>
    /// Returns the hash code for the current unit.
    /// </summary>
    /// <returns></returns>
    public override readonly int GetHashCode() =>
        0;

    /// <summary>
    /// Returns true if the specified object is equal to the current unit.
    /// </summary>
    /// <returns></returns>
    public override readonly string ToString() =>
        "()";
}

/// <summary>
/// Contains extension methods for <see cref="Unit"/> that allow for converting
/// between <see cref="Unit"/> and <see cref="Action"/>.
/// </summary>
public static class UnitActionExtensions
{
    /// <summary>
    /// Converts the specified action to a function that returns
    /// <see cref="Unit"/>.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Func<TResult, Unit> ToUnitFunc<TResult>(this Action<TResult> action)
    {
        return result =>
        {
            action(result);
            return Unit.Value;
        };
    }

    /// <summary>
    /// Converts the specified action to a function that returns
    /// <see cref="Unit"/>.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Func<Unit, Unit> ToUnitFunc(this Action action)
    {
        return _ =>
        {
            action();
            return Unit.Value;
        };
    }
}
