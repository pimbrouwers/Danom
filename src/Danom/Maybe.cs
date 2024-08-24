namespace Danom;

/// <summary>
/// Represents the exists of one value or another. Allows for more expressive
/// code with exhaustive matching.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
public abstract class Maybe<T1, T2>
{
    public static bool operator ==(Maybe<T1, T2> left, Maybe<T1, T2> right) =>
        left.Equals(right);

    public static bool operator !=(Maybe<T1, T2> left, Maybe<T1, T2> right) =>
        !(left == right);

    private readonly T1? _t1;
    private readonly T2? _t2;
    internal bool _isT1;
    internal bool _isT2;

    internal Maybe(T1 t1)
    {
        _t1 = t1;
        _isT1 = true;
    }

    internal Maybe(T2 t2)
    {
        _t2 = t2;
        _isT2 = true;
    }

    public override bool Equals(object? obj)
    {
        if (_t1 is null || obj is null)
        {
            return false;
        }

        return _t1.Equals(obj);
    }

    public override int GetHashCode()
    {
        if (_t1 is null)
        {
            return 0;
        }

        return _t1.GetHashCode();
    }

    internal U Match<U>(
      Func<T1, U> t1,
      Func<T2, U> t2)
    {
        if (_isT1 && _t1 is T1 x)
        {
            return t1(x);
        }
        else if (_isT2 && _t2 is T2 y)
        {
            return t2(y);
        }
        else
        {
            // TODO is this the right exception?
            throw new InvalidOperationException("Maybe is neither T1 nor T2.");
        }
    }
}
