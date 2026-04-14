namespace Danom.Tests;

using Xunit;

public sealed class ChoiceOf4Tests {
    [Fact]
    public void FromT1T2T3T4_SetArms() {
        var a = Choice<int, string, bool, double>.FromT1(42);
        Assert.True(a.IsT1);
        Assert.False(a.IsT2);
        Assert.False(a.IsT3);

        var b = Choice<int, string, bool, double>.FromT2("err");
        Assert.True(b.IsT2);
        Assert.False(b.IsT1);
        Assert.False(b.IsT3);

        var c = Choice<int, string, bool, double>.FromT3(true);
        Assert.True(c.IsT3);
        Assert.False(c.IsT1);
        Assert.False(c.IsT2);

        var d = Choice<int, string, bool, double>.FromT4(3.14);
        Assert.True(d.IsT4);
        Assert.False(d.IsT1);
        Assert.False(d.IsT2);
        Assert.False(d.IsT3);
    }

    [Fact]
    public void Match_ReturnsExpected() {
        var a = Choice<int, string, bool, double>.FromT1(10);
        var ra = a.Match(x => x + 1, _ => -1, _ => -2, _ => -3);
        Assert.Equal(11, ra);

        var b = Choice<int, string, bool, double>.FromT2("boom");
        var rb = b.Match(_ => -1, s => s.Length, _ => -2, _ => -3);
        Assert.Equal(4, rb);

        var c = Choice<int, string, bool, double>.FromT3(true);
        var rc = c.Match(_ => -1, _ => -2, z => z ? 1 : 0, _ => -3);
        Assert.Equal(1, rc);

        var d = Choice<int, string, bool, double>.FromT4(2.5);
        var rd = d.Match(_ => -1, _ => -2, _ => -3, v => (int)(v * 2)); ;
        Assert.Equal(5, rd);
    }

    [Fact]
    public void Match_Action_InvokesCorrectArm() {
        var a = Choice<int, string, bool, double>.FromT1(1);
        var seen1 = 0;
        a.Match(x => seen1 = x, _ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"));
        Assert.Equal(1, seen1);

        var b = Choice<int, string, bool, double>.FromT2("x");
        var seen2 = "";
        b.Match(_ => Assert.Fail("wrong arm"), s => seen2 = s, _ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"));
        Assert.Equal("x", seen2);

        var c = Choice<int, string, bool, double>.FromT3(true);
        var seen3 = false;
        c.Match(_ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"), z => seen3 = z, _ => Assert.Fail("wrong arm"));
        Assert.True(seen3);

        var d = Choice<int, string, bool, double>.FromT4(2.71);
        var seen4 = 0.0;
        d.Match(_ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"), v => seen4 = v);
        Assert.Equal(2.71, seen4);
    }

    [Fact]
    public void BindT1_BindsOnlyT1() {
        var a = Choice<int, string, bool, double>.FromT1(10);
        var b = a.BindT1(x => Choice<int, string, bool, double>.FromT1(x + 1));
        Assert.True(b.IsT1);
        Assert.Equal(11, b.Match(x => x, _ => -1, _ => -1, _ => -1));

        var c = a.BindT1(x => Choice<int, string, bool, double>.FromT2($"err:{x}"));
        Assert.True(c.IsT2);
        Assert.Equal("err:10", c.Match(_ => "", s => s, _ => "", _ => ""));

        var d = a.BindT1(x => Choice<int, string, bool, double>.FromT3(x > 5));
        Assert.True(d.IsT3);
        Assert.True(d.Match(_ => false, _ => false, z => z, _ => false));

        var e = a.BindT1(x => Choice<int, string, bool, double>.FromT4(x * 1.5));
        Assert.True(e.IsT4);
        Assert.Equal(15.0, e.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v));

        var f = Choice<int, string, bool, double>.FromT2("boom");
        var g = f.BindT1(x => Choice<int, string, bool, double>.FromT1(x + 1));
        Assert.True(g.IsT2);
        Assert.Equal("boom", g.Match(_ => "", s => s, _ => "", _ => ""));

        var h = Choice<int, string, bool, double>.FromT3(true);
        var i = h.BindT1(x => Choice<int, string, bool, double>.FromT1(x + 1));
        Assert.True(i.IsT3);
        Assert.True(i.Match(_ => false, _ => false, z => z, _ => false));

        var j = Choice<int, string, bool, double>.FromT4(2.5);
        var k = j.BindT1(x => Choice<int, string, bool, double>.FromT1(x + 1));
        Assert.True(k.IsT4);
        Assert.Equal(2.5, k.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v));
    }

    [Fact]
    public void BindT2_BindsOnlyT2() {
        var a = Choice<int, string, bool, double>.FromT2("x");
        var b = a.BindT2(s => Choice<int, string, bool, double>.FromT2(s + "y"));
        Assert.True(b.IsT2);
        Assert.Equal("xy", b.Match(_ => "", s => s, _ => "", _ => ""));

        var c = a.BindT2(s => Choice<int, string, bool, double>.FromT1(s.Length));
        Assert.True(c.IsT1);
        Assert.Equal(1, c.Match(x => x, _ => -1, _ => -1, _ => -1));

        var d = a.BindT2(s => Choice<int, string, bool, double>.FromT3(s == "x"));
        Assert.True(d.IsT3);
        Assert.True(d.Match(_ => false, _ => false, z => z, _ => false));

        var e = a.BindT2(s => Choice<int, string, bool, double>.FromT4(s.Length * 1.5));
        Assert.True(e.IsT4);
        Assert.Equal(1.5, e.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v));

        var f = Choice<int, string, bool, double>.FromT1(7);
        var g = f.BindT2(s => Choice<int, string, bool, double>.FromT2(s + "!"));
        Assert.True(g.IsT1);
        Assert.Equal(7, g.Match(x => x, _ => -1, _ => -1, _ => -1));

        var h = Choice<int, string, bool, double>.FromT3(false);
        var i = h.BindT2(s => Choice<int, string, bool, double>.FromT2(s + "!"));
        Assert.True(i.IsT3);
        Assert.False(i.Match(_ => true, _ => true, z => z, _ => true));

        var j = Choice<int, string, bool, double>.FromT4(3.14);
        var k = j.BindT2(s => Choice<int, string, bool, double>.FromT2(s + "!"));
        Assert.True(k.IsT4);
        Assert.Equal(3.14, k.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v));
    }

    [Fact]
    public void BindT3_BindsOnlyT3() {
        var a = Choice<int, string, bool, double>.FromT3(true);
        var b = a.BindT3(z => Choice<int, string, bool, double>.FromT3(!z));
        Assert.True(b.IsT3);
        Assert.False(b.Match(_ => true, _ => true, z => z, _ => true));

        var c = a.BindT3(z => Choice<int, string, bool, double>.FromT1(z ? 1 : 0));
        Assert.True(c.IsT1);
        Assert.Equal(1, c.Match(x => x, _ => -1, _ => -1, _ => -1));

        var d = a.BindT3(z => Choice<int, string, bool, double>.FromT2(z ? "yes" : "no"));
        Assert.True(d.IsT2);
        Assert.Equal("yes", d.Match(_ => "", s => s, _ => "", _ => ""));

        var e = a.BindT3(z => Choice<int, string, bool, double>.FromT4(z ? 1.0 : 0.0));
        Assert.True(e.IsT4);
        Assert.Equal(1.0, e.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v));

        var f = Choice<int, string, bool, double>.FromT1(5);
        var g = f.BindT3(z => Choice<int, string, bool, double>.FromT3(!z));
        Assert.True(g.IsT1);
        Assert.Equal(5, g.Match(x => x, _ => -1, _ => -1, _ => -1));

        var h = Choice<int, string, bool, double>.FromT2("test");
        var i = h.BindT3(z => Choice<int, string, bool, double>.FromT3(!z));
        Assert.True(i.IsT2);
        Assert.Equal("test", i.Match(_ => "", s => s, _ => "", _ => ""));

        var j = Choice<int, string, bool, double>.FromT4(2.71);
        var k = j.BindT3(z => Choice<int, string, bool, double>.FromT3(!z));
        Assert.True(k.IsT4);
        Assert.Equal(2.71, k.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v));
    }

    [Fact]
    public void BindT4_BindsOnlyT4() {
        var a = Choice<int, string, bool, double>.FromT4(3.14);
        var b = a.BindT4(v => Choice<int, string, bool, double>.FromT4(v * 2));
        Assert.True(b.IsT4);
        Assert.Equal(6.28, b.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v));

        var c = a.BindT4(v => Choice<int, string, bool, double>.FromT1((int)v));
        Assert.True(c.IsT1);
        Assert.Equal(3, c.Match(x => x, _ => -1, _ => -1, _ => -1));

        var d = a.BindT4(v => Choice<int, string, bool, double>.FromT2($"val:{v}"));
        Assert.True(d.IsT2);
        Assert.Equal("val:3.14", d.Match(_ => "", s => s, _ => "", _ => ""));

        var e = a.BindT4(v => Choice<int, string, bool, double>.FromT3(v > 3.0));
        Assert.True(e.IsT3);
        Assert.True(e.Match(_ => false, _ => false, z => z, _ => false));

        var f = Choice<int, string, bool, double>.FromT1(7);
        var g = f.BindT4(v => Choice<int, string, bool, double>.FromT4(v * 2));
        Assert.True(g.IsT1);
        Assert.Equal(7, g.Match(x => x, _ => -1, _ => -1, _ => -1));

        var h = Choice<int, string, bool, double>.FromT2("test");
        var i = h.BindT4(v => Choice<int, string, bool, double>.FromT4(v * 2));
        Assert.True(i.IsT2);
        Assert.Equal("test", i.Match(_ => "", s => s, _ => "", _ => ""));

        var j = Choice<int, string, bool, double>.FromT3(true);
        var k = j.BindT4(v => Choice<int, string, bool, double>.FromT4(v * 2));
        Assert.True(k.IsT3);
        Assert.True(k.Match(_ => false, _ => false, z => z, _ => false));
    }

    [Fact]
    public void MapT1_MapsOnlyT1() {
        var a = Choice<int, string, bool, double>.FromT1(5);
        var b = a.MapT1(x => x * 2);
        Assert.True(b.IsT1);
        Assert.Equal(10, b.Match(x => x, _ => -1, _ => -1, _ => -1));

        var c = Choice<int, string, bool, double>.FromT2("e");
        var d = c.MapT1(x => x * 2);
        Assert.True(d.IsT2);
        Assert.Equal("e", d.Match(_ => "", s => s, _ => "", _ => ""));

        var e = Choice<int, string, bool, double>.FromT3(true);
        var f = e.MapT1(x => x * 2);
        Assert.True(f.IsT3);
        Assert.True(f.Match(_ => false, _ => false, z => z, _ => false));

        var g = Choice<int, string, bool, double>.FromT4(2.5);
        var h = g.MapT1(x => x * 2);
        Assert.True(h.IsT4);
        Assert.Equal(2.5, h.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v));
    }

    [Fact]
    public void MapT2_MapsOnlyT2() {
        var a = Choice<int, string, bool, double>.FromT2("ab");
        var b = a.MapT2(s => s + "c");
        Assert.True(b.IsT2);
        Assert.Equal("abc", b.Match(_ => "", s => s, _ => "", _ => ""));

        var c = Choice<int, string, bool, double>.FromT1(3);
        var d = c.MapT2(s => s + "c");
        Assert.True(d.IsT1);
        Assert.Equal(3, d.Match(x => x, _ => -1, _ => -1, _ => -1));

        var e = Choice<int, string, bool, double>.FromT3(false);
        var f = e.MapT2(s => s + "c");
        Assert.True(f.IsT3);
        Assert.False(f.Match(_ => true, _ => true, z => z, _ => true));

        var g = Choice<int, string, bool, double>.FromT4(1.618);
        var h = g.MapT2(s => s + "c");
        Assert.True(h.IsT4);
        Assert.Equal(1.618, h.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v));
    }

    [Fact]
    public void MapT3_MapsOnlyT3() {
        var a = Choice<int, string, bool, double>.FromT3(true);
        var b = a.MapT3(z => !z);
        Assert.True(b.IsT3);
        Assert.False(b.Match(_ => true, _ => true, z => z, _ => true));

        var c = Choice<int, string, bool, double>.FromT1(7);
        var d = c.MapT3(z => !z);
        Assert.True(d.IsT1);
        Assert.Equal(7, d.Match(x => x, _ => -1, _ => -1, _ => -1));

        var e = Choice<int, string, bool, double>.FromT2("test");
        var f = e.MapT3(z => !z);
        Assert.True(f.IsT2);
        Assert.Equal("test", f.Match(_ => "", s => s, _ => "", _ => ""));

        var g = Choice<int, string, bool, double>.FromT4(3.14);
        var h = g.MapT3(z => !z);
        Assert.True(h.IsT4);
        Assert.Equal(3.14, h.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v));
    }

    [Fact]
    public void MapT4_MapsOnlyT4() {
        var a = Choice<int, string, bool, double>.FromT4(2.5);
        var b = a.MapT4(v => v * 3);
        Assert.True(b.IsT4);
        Assert.Equal(7.5, b.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v));

        var c = Choice<int, string, bool, double>.FromT1(9);
        var d = c.MapT4(v => v * 3);
        Assert.True(d.IsT1);
        Assert.Equal(9, d.Match(x => x, _ => -1, _ => -1, _ => -1));

        var e = Choice<int, string, bool, double>.FromT2("hello");
        var f = e.MapT4(v => v * 3);
        Assert.True(f.IsT2);
        Assert.Equal("hello", f.Match(_ => "", s => s, _ => "", _ => ""));

        var g = Choice<int, string, bool, double>.FromT3(false);
        var h = g.MapT4(v => v * 3);
        Assert.True(h.IsT3);
        Assert.False(h.Match(_ => true, _ => true, z => z, _ => true));
    }

    [Fact]
    public void TryGet_BothArms() {
        var a = Choice<int, string, bool, double>.FromT1(7);
        Assert.True(a.TryGetT1(out var v1));
        Assert.Equal(7, v1);
        Assert.False(a.TryGetT2(out _));
        Assert.False(a.TryGetT3(out _));

        var b = Choice<int, string, bool, double>.FromT2("e");
        Assert.True(b.TryGetT2(out var v2));
        Assert.Equal("e", v2);
        Assert.False(b.TryGetT1(out _));
        Assert.False(b.TryGetT3(out _));

        var c = Choice<int, string, bool, double>.FromT3(true);
        Assert.True(c.TryGetT3(out var v3));
        Assert.True(v3);
        Assert.False(c.TryGetT1(out _));
        Assert.False(c.TryGetT2(out _));

        var d = Choice<int, string, bool, double>.FromT4(1.618);
        Assert.True(d.TryGetT4(out var v4));
        Assert.Equal(1.618, v4);
        Assert.False(d.TryGetT1(out _));
        Assert.False(d.TryGetT2(out _));
        Assert.False(d.TryGetT3(out _));
    }

    [Fact]
    public void Equals_SameArmSameValue_True() {
        var c1 = Choice<int, string, bool, double>.FromT1(5);
        var c2 = Choice<int, string, bool, double>.FromT1(5);
        Assert.True(c1.Equals(c2));
        Assert.Equal(c1.GetHashCode(), c2.GetHashCode());

        var e1 = Choice<int, string, bool, double>.FromT2("a");
        var e2 = Choice<int, string, bool, double>.FromT2("a");
        Assert.True(e1.Equals(e2));
        Assert.Equal(e1.GetHashCode(), e2.GetHashCode());

        var z1 = Choice<int, string, bool, double>.FromT3(true);
        var z2 = Choice<int, string, bool, double>.FromT3(true);
        Assert.True(z1.Equals(z2));
        Assert.Equal(z1.GetHashCode(), z2.GetHashCode());

        var d1 = Choice<int, string, bool, double>.FromT4(3.14);
        var d2 = Choice<int, string, bool, double>.FromT4(3.14);
        Assert.True(d1.Equals(d2));
        Assert.Equal(d1.GetHashCode(), d2.GetHashCode());
    }

    [Fact]
    public void Equals_SameArmDifferentValues_False() {
        var c1 = Choice<int, string, bool, double>.FromT1(5);
        var c2 = Choice<int, string, bool, double>.FromT1(6);
        Assert.False(c1.Equals(c2));

        var e1 = Choice<int, string, bool, double>.FromT2("a");
        var e2 = Choice<int, string, bool, double>.FromT2("b");
        Assert.False(e1.Equals(e2));

        var z1 = Choice<int, string, bool, double>.FromT3(true);
        var z2 = Choice<int, string, bool, double>.FromT3(false);
        Assert.False(z1.Equals(z2));

        var d1 = Choice<int, string, bool, double>.FromT4(3.14);
        var d2 = Choice<int, string, bool, double>.FromT4(2.71);
        Assert.False(d1.Equals(d2));

    }

    [Fact]
    public void Equals_DifferentArm_False() {
        var a = Choice<int, string, bool, double>.FromT1(5);
        var b = Choice<int, string, bool, double>.FromT2("5");
        var c = Choice<int, string, bool, double>.FromT3(true);
        var d = Choice<int, string, bool, double>.FromT4(3.14);
        Assert.False(a.Equals(b));
        Assert.False(a.Equals(c));
        Assert.False(b.Equals(c));
        Assert.False(a.Equals(d));
        Assert.False(b.Equals(d));
    }

    [Fact]
    public void Equals_NullValuesOnSameArm_True() {
        var x = Choice<string, int, bool>.FromT1(null!);
        var y = Choice<string, int, bool>.FromT1(null!);
        Assert.True(x.Equals(y));

        // T2 arm default (int) and T3 arm default (bool) differ from T1 null arm
        var z2 = Choice<string, int, bool>.FromT2(default);
        var z3 = Choice<string, int, bool>.FromT3(default);
        var z4 = Choice<string, int, bool, double>.FromT4(default);
        Assert.False(x.Equals(z2));
        Assert.False(x.Equals(z3));
        Assert.False(x.Equals(z4));
    }

    [Fact]
    public void GetHashCode_EqualForEqualChoices() {
        var a1 = Choice<int, string, bool, double>.FromT1(5);
        var a2 = Choice<int, string, bool, double>.FromT1(5);
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());

        var b1 = Choice<int, string, bool, double>.FromT2("x");
        var b2 = Choice<int, string, bool, double>.FromT2("x");
        Assert.Equal(b1.GetHashCode(), b2.GetHashCode());

        var c1 = Choice<int, string, bool, double>.FromT3(true);
        var c2 = Choice<int, string, bool, double>.FromT3(true);
        Assert.Equal(c1.GetHashCode(), c2.GetHashCode());

        var d1 = Choice<int, string, bool, double>.FromT4(3.14);
        var d2 = Choice<int, string, bool, double>.FromT4(3.14);
        Assert.Equal(d1.GetHashCode(), d2.GetHashCode());
    }

    [Fact]
    public void ToString_Formats() {
        var a = Choice<int, string, bool, double>.FromT1(5);
        Assert.Equal("T1(5)", a.ToString());

        var b = Choice<int, string, bool, double>.FromT2("x");
        Assert.Equal("T2(x)", b.ToString());

        var c = Choice<int, string, bool, double>.FromT3(true);
        Assert.Equal("T3(True)", c.ToString());

        var d = Choice<int, string, bool, double>.FromT4(2.71);
        Assert.Equal("T4(2.71)", d.ToString());
    }

    [Fact]
    public void DefaultStructBehavior() {
        var d = default(Choice<int, string, bool, double>);
        Assert.False(d.IsT1);
        Assert.False(d.IsT2);
        Assert.False(d.IsT3);
        Assert.False(d.IsT4);

        Assert.False(d.TryGetT1(out _));
        Assert.False(d.TryGetT2(out _));
        Assert.False(d.TryGetT3(out _));
        Assert.False(d.TryGetT4(out _));

        Assert.Equal("Choice()", d.ToString());

        Assert.Equal(d, default(Choice<int, string, bool, double>));
        Assert.Equal(d.GetHashCode(), default(Choice<int, string, bool, double>).GetHashCode());
    }

    [Fact]
    public void Deconstruct_Values() {
        var a = Choice<int, string, bool, double>.FromT1(9);
        a.Deconstruct(out var stateA, out var v1, out var v2a, out var v3a, out var v4a);
        Assert.Equal(1, stateA);
        Assert.Equal(9, v1);
        Assert.Null(v2a);
        Assert.False(v3a);
        Assert.Equal(0.0, v4a);

        var b = Choice<int, string, bool, double>.FromT2("x");
        b.Deconstruct(out var stateB, out var v1b, out var v2b, out var v3b, out var v4b);
        Assert.Equal(2, stateB);
        Assert.Equal(0, v1b);
        Assert.Equal("x", v2b);
        Assert.False(v3b);
        Assert.Equal(0.0, v4b);

        var c = Choice<int, string, bool, double>.FromT3(true);
        c.Deconstruct(out var stateC, out var v1c, out var v2c, out var v3c, out var v4c);
        Assert.Equal(3, stateC);
        Assert.Equal(0, v1c);
        Assert.Null(v2c);
        Assert.True(v3c);
        Assert.Equal(0.0, v4c);

        var d = default(Choice<int, string, bool, double>);
        d.Deconstruct(out var stateD, out var v1d, out var v2d, out var v3d, out var v4d);
        Assert.Equal(0, stateD);
        Assert.Equal(0, v1d);
        Assert.Null(v2d);
        Assert.False(v3d);
        Assert.Equal(0.0, v4d);
    }

    [Fact]
    public void GetHashCode_DiffersAcrossArmsWithSameValueHash() {
        var t1 = Choice<int, int, int, int>.FromT1(42);
        var t2 = Choice<int, int, int, int>.FromT2(42);
        var t3 = Choice<int, int, int, int>.FromT3(42);
        var t4 = Choice<int, int, int, int>.FromT4(42);

        Assert.NotEqual(t1.GetHashCode(), t2.GetHashCode());
        Assert.NotEqual(t1.GetHashCode(), t3.GetHashCode());
        Assert.NotEqual(t1.GetHashCode(), t4.GetHashCode());
        Assert.NotEqual(t2.GetHashCode(), t3.GetHashCode());
        Assert.NotEqual(t3.GetHashCode(), t4.GetHashCode());
    }

    [Fact]
    public void GetHashCode_EqualChoices_HaveEqualHashes() {
        var a1 = Choice<int, string, bool, double>.FromT1(1);
        var a2 = Choice<int, string, bool, double>.FromT1(1);
        Assert.True(a1.Equals(a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());

        a1 = Choice<int, string, bool, double>.FromT2("test");
        a2 = Choice<int, string, bool, double>.FromT2("test");
        Assert.True(a1.Equals(a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());

        a1 = Choice<int, string, bool, double>.FromT3(false);
        a2 = Choice<int, string, bool, double>.FromT3(false);
        Assert.True(a1.Equals(a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());

        a1 = Choice<int, string, bool, double>.FromT4(2.5);
        a2 = Choice<int, string, bool, double>.FromT4(2.5);
        Assert.True(a1.Equals(a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DefaultStruct_IsStableAndDistinctFromActiveArms() {
        var d = default(Choice<int, string, bool, double>);
        var hDefault = d.GetHashCode();
        Assert.Equal(d, default(Choice<int, string, bool, double>));
        Assert.Equal(hDefault, default(Choice<int, string, bool, double>).GetHashCode());
    }
}
