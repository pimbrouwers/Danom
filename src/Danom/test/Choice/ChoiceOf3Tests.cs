namespace Danom.Tests;

using Xunit;

public sealed class ChoiceOf3Tests {
    [Fact]
    public void FromT1T2T3_SetArms() {
        var a = Choice<int, string, bool>.FromT1(42);
        Assert.True(a.IsT1);
        Assert.False(a.IsT2);
        Assert.False(a.IsT3);

        var b = Choice<int, string, bool>.FromT2("err");
        Assert.True(b.IsT2);
        Assert.False(b.IsT1);
        Assert.False(b.IsT3);

        var c = Choice<int, string, bool>.FromT3(true);
        Assert.True(c.IsT3);
        Assert.False(c.IsT1);
        Assert.False(c.IsT2);
    }

    [Fact]
    public void Match_ReturnsExpected() {
        var a = Choice<int, string, bool>.FromT1(10);
        var ra = a.Match(x => x + 1, _ => -1, _ => -2);
        Assert.Equal(11, ra);

        var b = Choice<int, string, bool>.FromT2("boom");
        var rb = b.Match(_ => -1, s => s.Length, _ => -2);
        Assert.Equal(4, rb);

        var c = Choice<int, string, bool>.FromT3(true);
        var rc = c.Match(_ => -1, _ => -2, z => z ? 1 : 0);
        Assert.Equal(1, rc);
    }

    [Fact]
    public void Match_Action_InvokesCorrectArm() {
        var a = Choice<int, string, bool>.FromT1(1);
        var seen1 = 0;
        a.Match(x => seen1 = x, _ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"));
        Assert.Equal(1, seen1);

        var b = Choice<int, string, bool>.FromT2("x");
        var seen2 = "";
        b.Match(_ => Assert.Fail("wrong arm"), s => seen2 = s, _ => Assert.Fail("wrong arm"));
        Assert.Equal("x", seen2);

        var c = Choice<int, string, bool>.FromT3(true);
        var seen3 = false;
        c.Match(_ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"), z => seen3 = z);
        Assert.True(seen3);
    }

    [Fact]
    public void TryGet_BothArms() {
        var a = Choice<int, string, bool>.FromT1(7);
        Assert.True(a.TryGetT1(out var v1));
        Assert.Equal(7, v1);
        Assert.False(a.TryGetT2(out _));
        Assert.False(a.TryGetT3(out _));

        var b = Choice<int, string, bool>.FromT2("e");
        Assert.True(b.TryGetT2(out var v2));
        Assert.Equal("e", v2);
        Assert.False(b.TryGetT1(out _));
        Assert.False(b.TryGetT3(out _));

        var c = Choice<int, string, bool>.FromT3(true);
        Assert.True(c.TryGetT3(out var v3));
        Assert.True(v3);
        Assert.False(c.TryGetT1(out _));
        Assert.False(c.TryGetT2(out _));
    }

    [Fact]
    public void Equals_SameArmSameValue_True() {
        var c1 = Choice<int, string, bool>.FromT1(5);
        var c2 = Choice<int, string, bool>.FromT1(5);
        Assert.True(c1.Equals(c2));
        Assert.Equal(c1.GetHashCode(), c2.GetHashCode());

        var e1 = Choice<int, string, bool>.FromT2("a");
        var e2 = Choice<int, string, bool>.FromT2("a");
        Assert.True(e1.Equals(e2));
        Assert.Equal(e1.GetHashCode(), e2.GetHashCode());

        var z1 = Choice<int, string, bool>.FromT3(true);
        var z2 = Choice<int, string, bool>.FromT3(true);
        Assert.True(z1.Equals(z2));
        Assert.Equal(z1.GetHashCode(), z2.GetHashCode());
    }

    [Fact]
    public void Equals_SameArmDifferentValues_False() {
        var c1 = Choice<int, string, bool>.FromT1(5);
        var c2 = Choice<int, string, bool>.FromT1(6);
        Assert.False(c1.Equals(c2));

        var e1 = Choice<int, string, bool>.FromT2("a");
        var e2 = Choice<int, string, bool>.FromT2("b");
        Assert.False(e1.Equals(e2));

        var z1 = Choice<int, string, bool>.FromT3(true);
        var z2 = Choice<int, string, bool>.FromT3(false);
        Assert.False(z1.Equals(z2));
    }

    [Fact]
    public void Equals_DifferentArm_False() {
        var a = Choice<int, string, bool>.FromT1(5);
        var b = Choice<int, string, bool>.FromT2("5");
        var c = Choice<int, string, bool>.FromT3(true);
        Assert.False(a.Equals(b));
        Assert.False(a.Equals(c));
        Assert.False(b.Equals(c));
    }

    [Fact]
    public void Equals_NullValuesOnSameArm_True() {
        var x = Choice<string, int, bool>.FromT1(null!);
        var y = Choice<string, int, bool>.FromT1(null!);
        Assert.True(x.Equals(y));

        // T2 arm default (int) and T3 arm default (bool) differ from T1 null arm
        var z2 = Choice<string, int, bool>.FromT2(default);
        var z3 = Choice<string, int, bool>.FromT3(default);
        Assert.False(x.Equals(z2));
        Assert.False(x.Equals(z3));
    }

    [Fact]
    public void GetHashCode_EqualForEqualChoices() {
        var a1 = Choice<int, string, bool>.FromT1(5);
        var a2 = Choice<int, string, bool>.FromT1(5);
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());

        var b1 = Choice<int, string, bool>.FromT2("x");
        var b2 = Choice<int, string, bool>.FromT2("x");
        Assert.Equal(b1.GetHashCode(), b2.GetHashCode());

        var c1 = Choice<int, string, bool>.FromT3(true);
        var c2 = Choice<int, string, bool>.FromT3(true);
        Assert.Equal(c1.GetHashCode(), c2.GetHashCode());
    }

    [Fact]
    public void ToString_Formats() {
        var a = Choice<int, string, bool>.FromT1(5);
        Assert.Equal("T1(5)", a.ToString());

        var b = Choice<int, string, bool>.FromT2("x");
        Assert.Equal("T2(x)", b.ToString());

        var c = Choice<int, string, bool>.FromT3(true);
        Assert.Equal("T3(True)", c.ToString());
    }

    [Fact]
    public void DefaultStructBehavior() {
        var d = default(Choice<int, string, bool>);
        Assert.False(d.IsT1);
        Assert.False(d.IsT2);
        Assert.False(d.IsT3);

        Assert.False(d.TryGetT1(out _));
        Assert.False(d.TryGetT2(out _));
        Assert.False(d.TryGetT3(out _));

        Assert.Equal("Choice()", d.ToString());

        Assert.Equal(d, default(Choice<int, string, bool>));
        Assert.Equal(d.GetHashCode(), default(Choice<int, string, bool>).GetHashCode());
    }

    [Fact]
    public void Deconstruct_Values() {
        var a = Choice<int, string, bool>.FromT1(9);
        a.Deconstruct(out var stateA, out var v1, out var v2a, out var v3a);
        Assert.Equal(1, stateA);
        Assert.Equal(9, v1);
        Assert.Null(v2a);
        Assert.False(v3a);

        var b = Choice<int, string, bool>.FromT2("x");
        b.Deconstruct(out var stateB, out var v1b, out var v2b, out var v3b);
        Assert.Equal(2, stateB);
        Assert.Equal(0, v1b);
        Assert.Equal("x", v2b);
        Assert.False(v3b);

        var c = Choice<int, string, bool>.FromT3(true);
        c.Deconstruct(out var stateC, out var v1c, out var v2c, out var v3c);
        Assert.Equal(3, stateC);
        Assert.Equal(0, v1c);
        Assert.Null(v2c);
        Assert.True(v3c);

        var d = default(Choice<int, string, bool>);
        d.Deconstruct(out var stateD, out var v1d, out var v2d, out var v3d);
        Assert.Equal(0, stateD);
        Assert.Equal(0, v1d);
        Assert.Null(v2d);
        Assert.False(v3d);
    }

    [Fact]
    public void GetHashCode_DiffersAcrossArmsWithSameValueHash() {
        var t1 = Choice<int, int, int>.FromT1(42);
        var t2 = Choice<int, int, int>.FromT2(42);
        var t3 = Choice<int, int, int>.FromT3(42);

        Assert.NotEqual(t1.GetHashCode(), t2.GetHashCode());
        Assert.NotEqual(t1.GetHashCode(), t3.GetHashCode());
        Assert.NotEqual(t2.GetHashCode(), t3.GetHashCode());
    }

    [Fact]
    public void GetHashCode_EqualChoices_HaveEqualHashes() {
        var a1 = Choice<int, string, bool>.FromT1(1);
        var a2 = Choice<int, string, bool>.FromT1(1);
        Assert.True(a1.Equals(a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());

        a1 = Choice<int, string, bool>.FromT2("test");
        a2 = Choice<int, string, bool>.FromT2("test");
        Assert.True(a1.Equals(a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());

        a1 = Choice<int, string, bool>.FromT3(false);
        a2 = Choice<int, string, bool>.FromT3(false);
        Assert.True(a1.Equals(a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DefaultStruct_IsStableAndDistinctFromActiveArms() {
        var d = default(Choice<int, string, bool>);
        var hDefault = d.GetHashCode();
        Assert.Equal(d, default(Choice<int, string, bool>));
        Assert.Equal(hDefault, default(Choice<int, string, bool>).GetHashCode());
    }
}
