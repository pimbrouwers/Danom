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
    public override readonly bool Equals(object? obj) => obj is Unit;
    public override readonly int GetHashCode() => 0;
    public static bool operator ==(Unit left, Unit right) => left.Equals(right);
    public static bool operator !=(Unit left, Unit right) => !(left == right);
    public readonly bool Equals(Unit other) => true;
    public override readonly string ToString() => "()";
    public int CompareTo(Unit other) => 0;
}
