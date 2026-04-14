namespace Danom.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class UnitTests {
    [Fact]
    public void Equality() {
        Assert.Equal(Unit.Value, Unit.Value);
    }

    [Fact]
    public void ToStringShouldProduceBrackets() {
        Assert.Equal("()", Unit.Value.ToString());
    }

    [Fact]
    public void GetHashCodeShouldBeZero() {
        Assert.Equal(0, Unit.Value.GetHashCode());
    }

    [Fact]
    public void EqualityOperator() {
#pragma warning disable CS1718 // Comparison made to same variable
        Assert.True(Unit.Value == Unit.Value);
#pragma warning restore CS1718 // Comparison made to same variable
    }

    [Fact]
    public void InequalityOperator() {
#pragma warning disable CS1718 // Comparison made to same variable
        Assert.False(Unit.Value != Unit.Value);
#pragma warning restore CS1718 // Comparison made to same variable
    }

    [Fact]
    public void OptionAndResultShortcuts() {
        AssertOption.IsSome(Option.Some());
        AssertOption.IsNone(Option.None());

        AssertResult.IsOk(Result.Ok());
        AssertResult.IsError(Result.Error("Error"));
    }

    [Fact]
    public async Task ValueAsyncReturnsValue() {
        var u = await Unit.ValueAsync;
        Assert.Equal(Unit.Value, u);
    }

    [Fact]
    public void EqualsObject() {
        Assert.True(Unit.Value.Equals((object)Unit.Value));
        Assert.False(Unit.Value.Equals(null));
        Assert.False(Unit.Value.Equals(42));
    }

    [Fact]
    public void DefaultUnitEqualsValue() {
        var d = default(Unit);
        Assert.True(d == Unit.Value);
        Assert.True(d.Equals(Unit.Value));
    }

    [Fact]
    public void HashSetDeduplicatesUnit() {
        var set = new HashSet<Unit> { Unit.Value, default };
        Assert.Single(set);
        Assert.Contains(Unit.Value, set);
    }

    [Fact]
    public void DictionaryKeyEquality() {
        var dict = new Dictionary<Unit, int> { [Unit.Value] = 1 };
        dict[default] = 2; // should overwrite same key
        Assert.Single(dict);
        Assert.Equal(2, dict[Unit.Value]);
    }

    [Fact]
    public void ToUnitFunc_Action() {
        var called = false;
        Action a = () => called = true;
        var f = a.ToUnitFunc();
        var r = f(Unit.Value);
        Assert.True(called);
        Assert.Equal(Unit.Value, r);
    }

    [Fact]
    public void ToUnitFunc_ActionOfT() {
        int received = 0;
        Action<int> a = x => received = x;
        var f = a.ToUnitFunc();
        var r = f(7);
        Assert.Equal(7, received);
        Assert.Equal(Unit.Value, r);
    }

    [Fact]
    public void EqualityOperatorsWorkAcrossInstances() {
        var u1 = Unit.Value;
        var u2 = default(Unit);
        Assert.True(u1 == u2);
        Assert.False(u1 != u2);
    }
}
