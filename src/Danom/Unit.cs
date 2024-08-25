namespace Danom;

/// <summary>
/// The unit type is a type that indicates the absence of a specific value; the
/// unit type has only a single value, which acts as a placeholder when no other
/// value exists or is needed.
/// </summary>
public readonly struct Unit
    : IEquatable<Unit>, IComparable<Unit>
{
    public static readonly Unit Value;
    public static readonly Task<Unit> ValueAsync = Task.FromResult(Value);

    public static bool operator ==(Unit left, Unit right) =>
        left.Equals(right);

    public static bool operator !=(Unit left, Unit right) =>
        !(left == right);

    public readonly bool Equals(Unit other) =>
        true;

    public int CompareTo(Unit other) =>
        0;

    public override readonly string ToString() =>
        "()";

    public override readonly bool Equals(object? obj) =>
        obj is Unit;

    public override readonly int GetHashCode() =>
        0;
}

/// <summary>
/// Contains extension methods for <see cref="Unit"/> that allow for converting
/// between <see cref="Unit"/> and <see cref="Action"/>.
/// </summary>
public static class UnitActionExtensions
{
    public static Func<TResult, Unit> ToUnitFunc<TResult>(this Action<TResult> action)
    {
        return result =>
        {
            action(result);
            return Unit.Value;
        };
    }

    public static Func<Unit, Unit> ToUnitFunc(this Action action)
    {
        return _ =>
        {
            action();
            return Unit.Value;
        };
    }
}

/// <summary>
/// Contains extension methods for <see cref="Unit"/> that allow for converting
/// between <see cref="Unit"/> and <see cref="Task"/>.
/// </summary>
public static class UnitTaskExtensions
{
    public static async Task<Unit> ToUnitAsync(this Task task)
    {
        await task;
        return Unit.Value;
    }
}
