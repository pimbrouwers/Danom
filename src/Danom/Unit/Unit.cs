namespace Danom;

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
