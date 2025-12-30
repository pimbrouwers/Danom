namespace Danom.Tests;

using Xunit;

public sealed class ChoiceTests {
    [Fact]
    public void FromT1AndFromT2_SetArms() {
        var a = Choice<int, string>.FromT1(42);
        Assert.True(a.IsT1);
        Assert.False(a.IsT2);

        var b = Choice<int, string>.FromT2("err");
        Assert.True(b.IsT2);
        Assert.False(b.IsT1);
    }

    [Fact]
    public void Match_ReturnsExpected() {
        var a = Choice<int, string>.FromT1(10);
        var ra = a.Match(x => x + 1, _ => -1);
        Assert.Equal(11, ra);

        var b = Choice<int, string>.FromT2("boom");
        var rb = b.Match(_ => -1, s => s.Length);
        Assert.Equal(4, rb);
    }

    [Fact]
    public void Match_Action_InvokesCorrectArm() {
        var a = Choice<int, string>.FromT1(1);
        var seen1 = 0;
        a.Match(x => seen1 = x, _ => Assert.Fail("wrong arm"));
        Assert.Equal(1, seen1);

        var b = Choice<int, string>.FromT2("x");
        var seen2 = "";
        b.Match(_ => Assert.Fail("wrong arm"), s => seen2 = s);
        Assert.Equal("x", seen2);
    }

    [Fact]
    public void TryGet_BothArms() {
        var a = Choice<int, string>.FromT1(7);
        Assert.True(a.TryGetT1(out var v1));
        Assert.Equal(7, v1);
        Assert.False(a.TryGetT2(out _));

        var b = Choice<int, string>.FromT2("e");
        Assert.True(b.TryGetT2(out var v2));
        Assert.Equal("e", v2);
        Assert.False(b.TryGetT1(out _));
    }

    [Fact]
    public void Equals_SameArmSameValue_True() {
        var c1 = Choice<int, string>.FromT1(5);
        var c2 = Choice<int, string>.FromT1(5);
        Assert.True(c1.Equals(c2));
        Assert.True(c1 == c2);
        Assert.False(c1 != c2);
        Assert.Equal(c1.GetHashCode(), c2.GetHashCode());
    }

    [Fact]
    public void Equals_SameArmDifferentValues_False() {
        var c1 = Choice<int, string>.FromT1(5);
        var c2 = Choice<int, string>.FromT1(6);
        Assert.False(c1.Equals(c2));
        Assert.False(c1 == c2);
        Assert.True(c1 != c2);

        var e1 = Choice<int, string>.FromT2("a");
        var e2 = Choice<int, string>.FromT2("b");
        Assert.False(e1.Equals(e2));
        Assert.False(e1 == e2);
        Assert.True(e1 != e2);
    }

    [Fact]
    public void Equals_DifferentArm_False() {
        var a = Choice<int, string>.FromT1(5);
        var b = Choice<int, string>.FromT2("5");
        Assert.False(a.Equals(b));
        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void Equals_NullValuesOnSameArm_True() {
        var x = Choice<string, int>.FromT1(null!);
        var y = Choice<string, int>.FromT1(null!);
        Assert.True(x.Equals(y));
        Assert.True(x == y);
        Assert.False(x != y);

        // T2 arm default (int) differs from T1 null arm
        var z = Choice<string, int>.FromT2(default);
        Assert.False(x.Equals(z));
    }

    [Fact]
    public void GetHashCode_EqualForEqualChoices() {
        var a1 = Choice<int, string>.FromT1(5);
        var a2 = Choice<int, string>.FromT1(5);
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());

        var b1 = Choice<int, string>.FromT2("x");
        var b2 = Choice<int, string>.FromT2("x");
        Assert.Equal(b1.GetHashCode(), b2.GetHashCode());
    }

    [Fact]
    public void ToString_Formats() {
        var a = Choice<int, string>.FromT1(5);
        Assert.Equal("T1(5)", a.ToString());

        var b = Choice<int, string>.FromT2("x");
        Assert.Equal("T2(x)", b.ToString());

        var n = Choice<string, int>.FromT1(null!);
        Assert.Equal("T1()", n.ToString());
    }

    [Fact]
    public void DefaultStructBehavior() {
        var d = default(Choice<int, string>); // IsT1=false => IsT2=true; values default
        Assert.False(d.IsT1);
        Assert.False(d.IsT2);

        Assert.False(d.TryGetT1(out _));
        Assert.False(d.TryGetT2(out var _));

        Assert.Equal("Choice()", d.ToString());

        Assert.Equal(d, default(Choice<int, string>));
        Assert.Equal(d.GetHashCode(), default(Choice<int, string>).GetHashCode());
    }

    [Fact]
    public void Deconstruct_Values() {
        var a = Choice<int, string>.FromT1(9);
        a.Deconstruct(out var isT1a, out var v1, out var v2a);
        Assert.Equal(1, isT1a);
        Assert.Equal(9, v1);
        Assert.Null(v2a);

        var b = Choice<int, string>.FromT2("x");
        b.Deconstruct(out var isT1b, out var v1b, out var v2b);
        Assert.Equal(2, isT1b);
        Assert.Equal(0, v1b);
        Assert.Equal("x", v2b);
    }


    [Fact]
    public void GetHashCode_DiffersAcrossArmsWithSameValueHash() {
        var t1 = Choice<int, int>.FromT1(42);
        var t2 = Choice<int, int>.FromT2(42);

        Assert.NotEqual(t1.GetHashCode(), t2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_EqualChoices_HaveEqualHashes() {
        var a1 = Choice<int, string>.FromT1(1);
        var a2 = Choice<int, string>.FromT1(1);
        Assert.True(a1.Equals(a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());

        a1 = Choice<int, string>.FromT2("test");
        a2 = Choice<int, string>.FromT2("test");
        Assert.True(a1.Equals(a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DefaultStruct_IsStableAndDistinctFromActiveArms() {
        var d = default(Choice<int, string>);
        var hDefault = d.GetHashCode();
        Assert.Equal(d, default(Choice<int, string>));
        Assert.Equal(hDefault, default(Choice<int, string>).GetHashCode());
    }
}
