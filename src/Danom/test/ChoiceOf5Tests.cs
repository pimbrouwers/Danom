namespace Danom.Tests;

using Xunit;

public sealed class ChoiceOf5Tests {
    [Fact]
    public void FromT1T2T3T4T5_SetArms() {
        var a = Choice<int, string, bool, double, char>.FromT1(42);
        Assert.True(a.IsT1); Assert.False(a.IsT2); Assert.False(a.IsT3); Assert.False(a.IsT4); Assert.False(a.IsT5);

        var b = Choice<int, string, bool, double, char>.FromT2("err");
        Assert.True(b.IsT2); Assert.False(b.IsT1); Assert.False(b.IsT3); Assert.False(b.IsT4); Assert.False(b.IsT5);

        var c = Choice<int, string, bool, double, char>.FromT3(true);
        Assert.True(c.IsT3); Assert.False(c.IsT1); Assert.False(c.IsT2); Assert.False(c.IsT4); Assert.False(c.IsT5);

        var d = Choice<int, string, bool, double, char>.FromT4(3.14);
        Assert.True(d.IsT4); Assert.False(d.IsT1); Assert.False(d.IsT2); Assert.False(d.IsT3); Assert.False(d.IsT5);

        var e = Choice<int, string, bool, double, char>.FromT5('Z');
        Assert.True(e.IsT5); Assert.False(e.IsT1); Assert.False(e.IsT2); Assert.False(e.IsT3); Assert.False(e.IsT4);
    }

    [Fact]
    public void Match_ReturnsExpected() {
        var a = Choice<int, string, bool, double, char>.FromT1(10);
        var ra = a.Match(x => x + 1, _ => -1, _ => -2, _ => -3, _ => -4);
        Assert.Equal(11, ra);

        var b = Choice<int, string, bool, double, char>.FromT2("boom");
        var rb = b.Match(_ => -1, s => s.Length, _ => -2, _ => -3, _ => -4);
        Assert.Equal(4, rb);

        var c = Choice<int, string, bool, double, char>.FromT3(true);
        var rc = c.Match(_ => -1, _ => -2, z => z ? 1 : 0, _ => -3, _ => -4);
        Assert.Equal(1, rc);

        var d = Choice<int, string, bool, double, char>.FromT4(2.5);
        var rd = d.Match(_ => -1, _ => -2, _ => -3, v => (int)(v * 2), _ => -4);
        Assert.Equal(5, rd);

        var e = Choice<int, string, bool, double, char>.FromT5('A');
        var re = e.Match(_ => -1, _ => -2, _ => -3, _ => -4, ch => ch == 'A' ? 1 : 0);
        Assert.Equal(1, re);
    }

    [Fact]
    public void Match_Action_InvokesCorrectArm() {
        var a = Choice<int, string, bool, double, char>.FromT1(1);
        var seen1 = 0;
        a.Match(x => seen1 = x, _ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"));
        Assert.Equal(1, seen1);

        var b = Choice<int, string, bool, double, char>.FromT2("x");
        var seen2 = "";
        b.Match(_ => Assert.Fail("wrong arm"), s => seen2 = s, _ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"));
        Assert.Equal("x", seen2);

        var c = Choice<int, string, bool, double, char>.FromT3(true);
        var seen3 = false;
        c.Match(_ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"), z => seen3 = z, _ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"));
        Assert.True(seen3);

        var d = Choice<int, string, bool, double, char>.FromT4(2.71);
        var seen4 = 0.0;
        d.Match(_ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"), v => seen4 = v, _ => Assert.Fail("wrong arm"));
        Assert.Equal(2.71, seen4);

        var e = Choice<int, string, bool, double, char>.FromT5('Q');
        var seen5 = '\0';
        e.Match(_ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"), _ => Assert.Fail("wrong arm"), ch => seen5 = ch);
        Assert.Equal('Q', seen5);
    }

    [Fact]
    public void BindT1_BindsOnlyT1() {
        var a = Choice<int, string, bool, double, char>.FromT1(10);
        var b = a.BindT1(x => Choice<int, string, bool, double, char>.FromT1(x + 1));
        Assert.True(b.IsT1);
        Assert.Equal(11, b.Match(x => x, _ => -1, _ => -1, _ => -1, _ => -1));

        var c = a.BindT1(x => Choice<int, string, bool, double, char>.FromT2($"err:{x}"));
        Assert.True(c.IsT2);
        Assert.Equal("err:10", c.Match(_ => "", s => s, _ => "", _ => "", _ => ""));

        var d = a.BindT1(x => Choice<int, string, bool, double, char>.FromT3(x > 5));
        Assert.True(d.IsT3);
        Assert.True(d.Match(_ => false, _ => false, z => z, _ => false, _ => false));

        var e = a.BindT1(x => Choice<int, string, bool, double, char>.FromT4(x * 1.5));
        Assert.True(e.IsT4);
        Assert.Equal(15.0, e.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v, _ => -1.0));

        var f = a.BindT1(x => Choice<int, string, bool, double, char>.FromT5((char)(x + 65)));
        Assert.True(f.IsT5);
        Assert.Equal('K', f.Match(_ => ' ', _ => ' ', _ => ' ', _ => ' ', ch => ch));

        var g = Choice<int, string, bool, double, char>.FromT2("boom");
        var h = g.BindT1(x => Choice<int, string, bool, double, char>.FromT1(x + 1));
        Assert.True(h.IsT2);
        Assert.Equal("boom", h.Match(_ => "", s => s, _ => "", _ => "", _ => ""));

        var i = Choice<int, string, bool, double, char>.FromT3(true);
        var j = i.BindT1(x => Choice<int, string, bool, double, char>.FromT1(x + 1));
        Assert.True(j.IsT3);
        Assert.True(j.Match(_ => false, _ => false, z => z, _ => false, _ => false));

        var k = Choice<int, string, bool, double, char>.FromT4(2.5);
        var l = k.BindT1(x => Choice<int, string, bool, double, char>.FromT1(x + 1));
        Assert.True(l.IsT4);
        Assert.Equal(2.5, l.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v, _ => -1.0));

        var m = Choice<int, string, bool, double, char>.FromT5('Z');
        var n = m.BindT1(x => Choice<int, string, bool, double, char>.FromT1(x + 1));
        Assert.True(n.IsT5);
        Assert.Equal('Z', n.Match(_ => ' ', _ => ' ', _ => ' ', _ => ' ', ch => ch));
    }

    [Fact]
    public void BindT2_BindsOnlyT2() {
        var a = Choice<int, string, bool, double, char>.FromT2("x");
        var b = a.BindT2(s => Choice<int, string, bool, double, char>.FromT2(s + "y"));
        Assert.True(b.IsT2);
        Assert.Equal("xy", b.Match(_ => "", s => s, _ => "", _ => "", _ => ""));

        var c = a.BindT2(s => Choice<int, string, bool, double, char>.FromT1(s.Length));
        Assert.True(c.IsT1);
        Assert.Equal(1, c.Match(x => x, _ => -1, _ => -1, _ => -1, _ => -1));

        var d = a.BindT2(s => Choice<int, string, bool, double, char>.FromT3(s == "x"));
        Assert.True(d.IsT3);
        Assert.True(d.Match(_ => false, _ => false, z => z, _ => false, _ => false));

        var e = a.BindT2(s => Choice<int, string, bool, double, char>.FromT4(s.Length * 1.5));
        Assert.True(e.IsT4);
        Assert.Equal(1.5, e.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v, _ => -1.0));

        var f = a.BindT2(s => Choice<int, string, bool, double, char>.FromT5(s[0]));
        Assert.True(f.IsT5);
        Assert.Equal('x', f.Match(_ => ' ', _ => ' ', _ => ' ', _ => ' ', ch => ch));

        var g = Choice<int, string, bool, double, char>.FromT1(7);
        var h = g.BindT2(s => Choice<int, string, bool, double, char>.FromT2(s + "!"));
        Assert.True(h.IsT1);
        Assert.Equal(7, h.Match(x => x, _ => -1, _ => -1, _ => -1, _ => -1));

        var i = Choice<int, string, bool, double, char>.FromT3(false);
        var j = i.BindT2(s => Choice<int, string, bool, double, char>.FromT2(s + "!"));
        Assert.True(j.IsT3);
        Assert.False(j.Match(_ => true, _ => true, z => z, _ => true, _ => true));

        var k = Choice<int, string, bool, double, char>.FromT4(3.14);
        var l = k.BindT2(s => Choice<int, string, bool, double, char>.FromT2(s + "!"));
        Assert.True(l.IsT4);
        Assert.Equal(3.14, l.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v, _ => -1.0));

        var m = Choice<int, string, bool, double, char>.FromT5('A');
        var n = m.BindT2(s => Choice<int, string, bool, double, char>.FromT2(s + "!"));
        Assert.True(n.IsT5);
        Assert.Equal('A', n.Match(_ => ' ', _ => ' ', _ => ' ', _ => ' ', ch => ch));
    }

    [Fact]
    public void BindT3_BindsOnlyT3() {
        var a = Choice<int, string, bool, double, char>.FromT3(true);
        var b = a.BindT3(z => Choice<int, string, bool, double, char>.FromT3(!z));
        Assert.True(b.IsT3);
        Assert.False(b.Match(_ => true, _ => true, z => z, _ => true, _ => true));

        var c = a.BindT3(z => Choice<int, string, bool, double, char>.FromT1(z ? 1 : 0));
        Assert.True(c.IsT1);
        Assert.Equal(1, c.Match(x => x, _ => -1, _ => -1, _ => -1, _ => -1));

        var d = a.BindT3(z => Choice<int, string, bool, double, char>.FromT2(z ? "yes" : "no"));
        Assert.True(d.IsT2);
        Assert.Equal("yes", d.Match(_ => "", s => s, _ => "", _ => "", _ => ""));

        var e = a.BindT3(z => Choice<int, string, bool, double, char>.FromT4(z ? 1.0 : 0.0));
        Assert.True(e.IsT4);
        Assert.Equal(1.0, e.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v, _ => -1.0));

        var f = a.BindT3(z => Choice<int, string, bool, double, char>.FromT5(z ? 'Y' : 'N'));
        Assert.True(f.IsT5);
        Assert.Equal('Y', f.Match(_ => ' ', _ => ' ', _ => ' ', _ => ' ', ch => ch));

        var g = Choice<int, string, bool, double, char>.FromT1(5);
        var h = g.BindT3(z => Choice<int, string, bool, double, char>.FromT3(!z));
        Assert.True(h.IsT1);
        Assert.Equal(5, h.Match(x => x, _ => -1, _ => -1, _ => -1, _ => -1));

        var i = Choice<int, string, bool, double, char>.FromT2("test");
        var j = i.BindT3(z => Choice<int, string, bool, double, char>.FromT3(!z));
        Assert.True(j.IsT2);
        Assert.Equal("test", j.Match(_ => "", s => s, _ => "", _ => "", _ => ""));

        var k = Choice<int, string, bool, double, char>.FromT4(2.71);
        var l = k.BindT3(z => Choice<int, string, bool, double, char>.FromT3(!z));
        Assert.True(l.IsT4);
        Assert.Equal(2.71, l.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v, _ => -1.0));

        var m = Choice<int, string, bool, double, char>.FromT5('Q');
        var n = m.BindT3(z => Choice<int, string, bool, double, char>.FromT3(!z));
        Assert.True(n.IsT5);
        Assert.Equal('Q', n.Match(_ => ' ', _ => ' ', _ => ' ', _ => ' ', ch => ch));
    }

    [Fact]
    public void BindT4_BindsOnlyT4() {
        var a = Choice<int, string, bool, double, char>.FromT4(3.14);
        var b = a.BindT4(v => Choice<int, string, bool, double, char>.FromT4(v * 2));
        Assert.True(b.IsT4);
        Assert.Equal(6.28, b.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v, _ => -1.0));

        var c = a.BindT4(v => Choice<int, string, bool, double, char>.FromT1((int)v));
        Assert.True(c.IsT1);
        Assert.Equal(3, c.Match(x => x, _ => -1, _ => -1, _ => -1, _ => -1));

        var d = a.BindT4(v => Choice<int, string, bool, double, char>.FromT2($"val:{v}"));
        Assert.True(d.IsT2);
        Assert.Equal("val:3.14", d.Match(_ => "", s => s, _ => "", _ => "", _ => ""));

        var e = a.BindT4(v => Choice<int, string, bool, double, char>.FromT3(v > 3.0));
        Assert.True(e.IsT3);
        Assert.True(e.Match(_ => false, _ => false, z => z, _ => false, _ => false));

        var f = a.BindT4(v => Choice<int, string, bool, double, char>.FromT5((char)(int)v));
        Assert.True(f.IsT5);
        Assert.Equal((char)3, f.Match(_ => ' ', _ => ' ', _ => ' ', _ => ' ', ch => ch));

        var g = Choice<int, string, bool, double, char>.FromT1(7);
        var h = g.BindT4(v => Choice<int, string, bool, double, char>.FromT4(v * 2));
        Assert.True(h.IsT1);
        Assert.Equal(7, h.Match(x => x, _ => -1, _ => -1, _ => -1, _ => -1));

        var i = Choice<int, string, bool, double, char>.FromT2("test");
        var j = i.BindT4(v => Choice<int, string, bool, double, char>.FromT4(v * 2));
        Assert.True(j.IsT2);
        Assert.Equal("test", j.Match(_ => "", s => s, _ => "", _ => "", _ => ""));

        var k = Choice<int, string, bool, double, char>.FromT3(true);
        var l = k.BindT4(v => Choice<int, string, bool, double, char>.FromT4(v * 2));
        Assert.True(l.IsT3);
        Assert.True(l.Match(_ => false, _ => false, z => z, _ => false, _ => false));

        var m = Choice<int, string, bool, double, char>.FromT5('B');
        var n = m.BindT4(v => Choice<int, string, bool, double, char>.FromT4(v * 2));
        Assert.True(n.IsT5);
        Assert.Equal('B', n.Match(_ => ' ', _ => ' ', _ => ' ', _ => ' ', ch => ch));
    }

    [Fact]
    public void BindT5_BindsOnlyT5() {
        var a = Choice<int, string, bool, double, char>.FromT5('X');
        var b = a.BindT5(ch => Choice<int, string, bool, double, char>.FromT5((char)(ch + 1)));
        Assert.True(b.IsT5);
        Assert.Equal('Y', b.Match(_ => ' ', _ => ' ', _ => ' ', _ => ' ', ch => ch));

        var c = a.BindT5(ch => Choice<int, string, bool, double, char>.FromT1((int)ch));
        Assert.True(c.IsT1);
        Assert.Equal(88, c.Match(x => x, _ => -1, _ => -1, _ => -1, _ => -1));

        var d = a.BindT5(ch => Choice<int, string, bool, double, char>.FromT2($"char:{ch}"));
        Assert.True(d.IsT2);
        Assert.Equal("char:X", d.Match(_ => "", s => s, _ => "", _ => "", _ => ""));

        var e = a.BindT5(ch => Choice<int, string, bool, double, char>.FromT3(ch == 'X'));
        Assert.True(e.IsT3);
        Assert.True(e.Match(_ => false, _ => false, z => z, _ => false, _ => false));

        var f = a.BindT5(ch => Choice<int, string, bool, double, char>.FromT4((double)ch));
        Assert.True(f.IsT4);
        Assert.Equal(88.0, f.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v, _ => -1.0));

        var g = Choice<int, string, bool, double, char>.FromT1(9);
        var h = g.BindT5(ch => Choice<int, string, bool, double, char>.FromT5((char)(ch + 1)));
        Assert.True(h.IsT1);
        Assert.Equal(9, h.Match(x => x, _ => -1, _ => -1, _ => -1, _ => -1));

        var i = Choice<int, string, bool, double, char>.FromT2("hello");
        var j = i.BindT5(ch => Choice<int, string, bool, double, char>.FromT5((char)(ch + 1)));
        Assert.True(j.IsT2);
        Assert.Equal("hello", j.Match(_ => "", s => s, _ => "", _ => "", _ => ""));

        var k = Choice<int, string, bool, double, char>.FromT3(false);
        var l = k.BindT5(ch => Choice<int, string, bool, double, char>.FromT5((char)(ch + 1)));
        Assert.True(l.IsT3);
        Assert.False(l.Match(_ => true, _ => true, z => z, _ => true, _ => true));

        var m = Choice<int, string, bool, double, char>.FromT4(1.5);
        var n = m.BindT5(ch => Choice<int, string, bool, double, char>.FromT5((char)(ch + 1)));
        Assert.True(n.IsT4);
        Assert.Equal(1.5, n.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v, _ => -1.0));
    }

    [Fact]
    public void MapT1_MapsOnlyT1() {
        var a = Choice<int, string, bool, double, char>.FromT1(5);
        var b = a.MapT1(x => x * 2);
        Assert.True(b.IsT1);
        Assert.Equal(10, b.Match(x => x, _ => -1, _ => -1, _ => -1, _ => -1));

        var c = Choice<int, string, bool, double, char>.FromT2("e");
        var d = c.MapT1(x => x * 2);
        Assert.True(d.IsT2);
        Assert.Equal("e", d.Match(_ => "", s => s, _ => "", _ => "", _ => ""));

        var e = Choice<int, string, bool, double, char>.FromT3(true);
        var f = e.MapT1(x => x * 2);
        Assert.True(f.IsT3);
        Assert.True(f.Match(_ => false, _ => false, z => z, _ => false, _ => false));

        var g = Choice<int, string, bool, double, char>.FromT4(2.5);
        var h = g.MapT1(x => x * 2);
        Assert.True(h.IsT4);
        Assert.Equal(2.5, h.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v, _ => -1.0));

        var i = Choice<int, string, bool, double, char>.FromT5('Z');
        var j = i.MapT1(x => x * 2);
        Assert.True(j.IsT5);
        Assert.Equal('Z', j.Match(_ => ' ', _ => ' ', _ => ' ', _ => ' ', ch => ch));
    }

    [Fact]
    public void MapT2_MapsOnlyT2() {
        var a = Choice<int, string, bool, double, char>.FromT2("ab");
        var b = a.MapT2(s => s + "c");
        Assert.True(b.IsT2);
        Assert.Equal("abc", b.Match(_ => "", s => s, _ => "", _ => "", _ => ""));

        var c = Choice<int, string, bool, double, char>.FromT1(3);
        var d = c.MapT2(s => s + "c");
        Assert.True(d.IsT1);
        Assert.Equal(3, d.Match(x => x, _ => -1, _ => -1, _ => -1, _ => -1));

        var e = Choice<int, string, bool, double, char>.FromT3(false);
        var f = e.MapT2(s => s + "c");
        Assert.True(f.IsT3);
        Assert.False(f.Match(_ => true, _ => true, z => z, _ => true, _ => true));

        var g = Choice<int, string, bool, double, char>.FromT4(1.618);
        var h = g.MapT2(s => s + "c");
        Assert.True(h.IsT4);
        Assert.Equal(1.618, h.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v, _ => -1.0));

        var i = Choice<int, string, bool, double, char>.FromT5('M');
        var j = i.MapT2(s => s + "c");
        Assert.True(j.IsT5);
        Assert.Equal('M', j.Match(_ => ' ', _ => ' ', _ => ' ', _ => ' ', ch => ch));
    }

    [Fact]
    public void MapT3_MapsOnlyT3() {
        var a = Choice<int, string, bool, double, char>.FromT3(true);
        var b = a.MapT3(z => !z);
        Assert.True(b.IsT3);
        Assert.False(b.Match(_ => true, _ => true, z => z, _ => true, _ => true));

        var c = Choice<int, string, bool, double, char>.FromT1(7);
        var d = c.MapT3(z => !z);
        Assert.True(d.IsT1);
        Assert.Equal(7, d.Match(x => x, _ => -1, _ => -1, _ => -1, _ => -1));

        var e = Choice<int, string, bool, double, char>.FromT2("test");
        var f = e.MapT3(z => !z);
        Assert.True(f.IsT2);
        Assert.Equal("test", f.Match(_ => "", s => s, _ => "", _ => "", _ => ""));

        var g = Choice<int, string, bool, double, char>.FromT4(3.14);
        var h = g.MapT3(z => !z);
        Assert.True(h.IsT4);
        Assert.Equal(3.14, h.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v, _ => -1.0));

        var i = Choice<int, string, bool, double, char>.FromT5('P');
        var j = i.MapT3(z => !z);
        Assert.True(j.IsT5);
        Assert.Equal('P', j.Match(_ => ' ', _ => ' ', _ => ' ', _ => ' ', ch => ch));
    }

    [Fact]
    public void MapT4_MapsOnlyT4() {
        var a = Choice<int, string, bool, double, char>.FromT4(2.5);
        var b = a.MapT4(v => v * 3);
        Assert.True(b.IsT4);
        Assert.Equal(7.5, b.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v, _ => -1.0));

        var c = Choice<int, string, bool, double, char>.FromT1(9);
        var d = c.MapT4(v => v * 3);
        Assert.True(d.IsT1);
        Assert.Equal(9, d.Match(x => x, _ => -1, _ => -1, _ => -1, _ => -1));

        var e = Choice<int, string, bool, double, char>.FromT2("hello");
        var f = e.MapT4(v => v * 3);
        Assert.True(f.IsT2);
        Assert.Equal("hello", f.Match(_ => "", s => s, _ => "", _ => "", _ => ""));

        var g = Choice<int, string, bool, double, char>.FromT3(false);
        var h = g.MapT4(v => v * 3);
        Assert.True(h.IsT3);
        Assert.False(h.Match(_ => true, _ => true, z => z, _ => true, _ => true));

        var i = Choice<int, string, bool, double, char>.FromT5('C');
        var j = i.MapT4(v => v * 3);
        Assert.True(j.IsT5);
        Assert.Equal('C', j.Match(_ => ' ', _ => ' ', _ => ' ', _ => ' ', ch => ch));
    }

    [Fact]
    public void MapT5_MapsOnlyT5() {
        var a = Choice<int, string, bool, double, char>.FromT5('A');
        var b = a.MapT5(ch => (char)(ch + 1));
        Assert.True(b.IsT5);
        Assert.Equal('B', b.Match(_ => ' ', _ => ' ', _ => ' ', _ => ' ', ch => ch));

        var c = Choice<int, string, bool, double, char>.FromT1(11);
        var d = c.MapT5(ch => (char)(ch + 1));
        Assert.True(d.IsT1);
        Assert.Equal(11, d.Match(x => x, _ => -1, _ => -1, _ => -1, _ => -1));

        var e = Choice<int, string, bool, double, char>.FromT2("world");
        var f = e.MapT5(ch => (char)(ch + 1));
        Assert.True(f.IsT2);
        Assert.Equal("world", f.Match(_ => "", s => s, _ => "", _ => "", _ => ""));

        var g = Choice<int, string, bool, double, char>.FromT3(true);
        var h = g.MapT5(ch => (char)(ch + 1));
        Assert.True(h.IsT3);
        Assert.True(h.Match(_ => false, _ => false, z => z, _ => false, _ => false));

        var i = Choice<int, string, bool, double, char>.FromT4(9.99);
        var j = i.MapT5(ch => (char)(ch + 1));
        Assert.True(j.IsT4);
        Assert.Equal(9.99, j.Match(_ => -1.0, _ => -1.0, _ => -1.0, v => v, _ => -1.0));
    }

    [Fact]
    public void TryGet_AllArms() {
        var a = Choice<int, string, bool, double, char>.FromT1(7);
        Assert.True(a.TryGetT1(out var v1)); Assert.Equal(7, v1);
        Assert.False(a.TryGetT2(out _)); Assert.False(a.TryGetT3(out _)); Assert.False(a.TryGetT4(out _)); Assert.False(a.TryGetT5(out _));

        var b = Choice<int, string, bool, double, char>.FromT2("e");
        Assert.True(b.TryGetT2(out var v2)); Assert.Equal("e", v2);
        Assert.False(b.TryGetT1(out _)); Assert.False(b.TryGetT3(out _)); Assert.False(b.TryGetT4(out _)); Assert.False(b.TryGetT5(out _));

        var c = Choice<int, string, bool, double, char>.FromT3(true);
        Assert.True(c.TryGetT3(out var v3)); Assert.True(v3);
        Assert.False(c.TryGetT1(out _)); Assert.False(c.TryGetT2(out _)); Assert.False(c.TryGetT4(out _)); Assert.False(c.TryGetT5(out _));

        var d = Choice<int, string, bool, double, char>.FromT4(1.618);
        Assert.True(d.TryGetT4(out var v4)); Assert.Equal(1.618, v4);
        Assert.False(d.TryGetT1(out _)); Assert.False(d.TryGetT2(out _)); Assert.False(d.TryGetT3(out _)); Assert.False(d.TryGetT5(out _));

        var e = Choice<int, string, bool, double, char>.FromT5('Z');
        Assert.True(e.TryGetT5(out var v5)); Assert.Equal('Z', v5);
        Assert.False(e.TryGetT1(out _)); Assert.False(e.TryGetT2(out _)); Assert.False(e.TryGetT3(out _)); Assert.False(e.TryGetT4(out _));
    }

    [Fact]
    public void Equals_SameArmSameValue_True() {
        var c1 = Choice<int, string, bool, double, char>.FromT1(5);
        var c2 = Choice<int, string, bool, double, char>.FromT1(5);
        Assert.True(c1.Equals(c2)); Assert.Equal(c1.GetHashCode(), c2.GetHashCode());

        var e1 = Choice<int, string, bool, double, char>.FromT2("a");
        var e2 = Choice<int, string, bool, double, char>.FromT2("a");
        Assert.True(e1.Equals(e2)); Assert.Equal(e1.GetHashCode(), e2.GetHashCode());

        var z1 = Choice<int, string, bool, double, char>.FromT3(true);
        var z2 = Choice<int, string, bool, double, char>.FromT3(true);
        Assert.True(z1.Equals(z2)); Assert.Equal(z1.GetHashCode(), z2.GetHashCode());

        var d1 = Choice<int, string, bool, double, char>.FromT4(3.14);
        var d2 = Choice<int, string, bool, double, char>.FromT4(3.14);
        Assert.True(d1.Equals(d2)); Assert.Equal(d1.GetHashCode(), d2.GetHashCode());

        var f1 = Choice<int, string, bool, double, char>.FromT5('X');
        var f2 = Choice<int, string, bool, double, char>.FromT5('X');
        Assert.True(f1.Equals(f2)); Assert.Equal(f1.GetHashCode(), f2.GetHashCode());
    }

    [Fact]
    public void Equals_SameArmDifferentValues_False() {
        var c1 = Choice<int, string, bool, double, char>.FromT1(5);
        var c2 = Choice<int, string, bool, double, char>.FromT1(6);
        Assert.False(c1.Equals(c2));

        var e1 = Choice<int, string, bool, double, char>.FromT2("a");
        var e2 = Choice<int, string, bool, double, char>.FromT2("b");
        Assert.False(e1.Equals(e2));

        var z1 = Choice<int, string, bool, double, char>.FromT3(true);
        var z2 = Choice<int, string, bool, double, char>.FromT3(false);
        Assert.False(z1.Equals(z2));

        var d1 = Choice<int, string, bool, double, char>.FromT4(3.14);
        var d2 = Choice<int, string, bool, double, char>.FromT4(2.71);
        Assert.False(d1.Equals(d2));

        var f1 = Choice<int, string, bool, double, char>.FromT5('X');
        var f2 = Choice<int, string, bool, double, char>.FromT5('Y');
        Assert.False(f1.Equals(f2));
    }

    [Fact]
    public void Equals_DifferentArm_False() {
        var a = Choice<int, string, bool, double, char>.FromT1(5);
        var b = Choice<int, string, bool, double, char>.FromT2("5");
        var c = Choice<int, string, bool, double, char>.FromT3(true);
        var d = Choice<int, string, bool, double, char>.FromT4(3.14);
        var e = Choice<int, string, bool, double, char>.FromT5('E');

        Assert.False(a.Equals(b)); Assert.False(a.Equals(c)); Assert.False(a.Equals(d)); Assert.False(a.Equals(e));
        Assert.False(b.Equals(c)); Assert.False(b.Equals(d)); Assert.False(b.Equals(e));
        Assert.False(c.Equals(d)); Assert.False(c.Equals(e));
        Assert.False(d.Equals(e));
    }

    [Fact]
    public void Equals_NullValuesOnSameArm_True() {
        var x = Choice<string, int, bool, double, char>.FromT1(null!);
        var y = Choice<string, int, bool, double, char>.FromT1(null!);
        Assert.True(x.Equals(y));

        var z2 = Choice<string, int, bool, double, char>.FromT2(default);
        var z3 = Choice<string, int, bool, double, char>.FromT3(default);
        var z4 = Choice<string, int, bool, double, char>.FromT4(default);
        var z5 = Choice<string, int, bool, double, char>.FromT5(default);
        Assert.False(x.Equals(z2)); Assert.False(x.Equals(z3)); Assert.False(x.Equals(z4)); Assert.False(x.Equals(z5));
    }

    [Fact]
    public void GetHashCode_EqualForEqualChoices() {
        var a1 = Choice<int, string, bool, double, char>.FromT1(5);
        var a2 = Choice<int, string, bool, double, char>.FromT1(5);
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());

        var b1 = Choice<int, string, bool, double, char>.FromT2("x");
        var b2 = Choice<int, string, bool, double, char>.FromT2("x");
        Assert.Equal(b1.GetHashCode(), b2.GetHashCode());

        var c1 = Choice<int, string, bool, double, char>.FromT3(true);
        var c2 = Choice<int, string, bool, double, char>.FromT3(true);
        Assert.Equal(c1.GetHashCode(), c2.GetHashCode());

        var d1 = Choice<int, string, bool, double, char>.FromT4(3.14);
        var d2 = Choice<int, string, bool, double, char>.FromT4(3.14);
        Assert.Equal(d1.GetHashCode(), d2.GetHashCode());

        var e1 = Choice<int, string, bool, double, char>.FromT5('K');
        var e2 = Choice<int, string, bool, double, char>.FromT5('K');
        Assert.Equal(e1.GetHashCode(), e2.GetHashCode());
    }

    [Fact]
    public void ToString_Formats() {
        var a = Choice<int, string, bool, double, char>.FromT1(5);
        Assert.Equal("T1(5)", a.ToString());

        var b = Choice<int, string, bool, double, char>.FromT2("x");
        Assert.Equal("T2(x)", b.ToString());

        var c = Choice<int, string, bool, double, char>.FromT3(true);
        Assert.Equal("T3(True)", c.ToString());

        var d = Choice<int, string, bool, double, char>.FromT4(2.71);
        Assert.Equal("T4(2.71)", d.ToString());

        var e = Choice<int, string, bool, double, char>.FromT5('Z');
        Assert.Equal("T5(Z)", e.ToString());

        var n = Choice<string, int, bool, double, char>.FromT1(null!);
        Assert.Equal("T1()", n.ToString());
    }

    [Fact]
    public void DefaultStructBehavior() {
        var d = default(Choice<int, string, bool, double, char>);
        Assert.False(d.IsT1); Assert.False(d.IsT2); Assert.False(d.IsT3); Assert.False(d.IsT4); Assert.False(d.IsT5);

        Assert.False(d.TryGetT1(out _)); Assert.False(d.TryGetT2(out _)); Assert.False(d.TryGetT3(out _)); Assert.False(d.TryGetT4(out _)); Assert.False(d.TryGetT5(out _));

        // If default maps to last arm or has a neutral ToString, assert accordingly:
        Assert.Equal("Choice()", d.ToString());

        Assert.Equal(d, default(Choice<int, string, bool, double, char>));
        Assert.Equal(d.GetHashCode(), default(Choice<int, string, bool, double, char>).GetHashCode());
    }

    [Fact]
    public void Deconstruct_Values() {
        var a = Choice<int, string, bool, double, char>.FromT1(9);
        a.Deconstruct(out var stateA, out var v1, out var v2a, out var v3a, out var v4a, out var v5a);
        Assert.Equal(1, stateA);
        Assert.Equal(9, v1);
        Assert.Null(v2a);
        Assert.False(v3a);
        Assert.Equal(0.0, v4a);
        Assert.Equal('\0', v5a);

        var b = Choice<int, string, bool, double, char>.FromT2("x");
        b.Deconstruct(out var stateB, out var v1b, out var v2b, out var v3b, out var v4b, out var v5b);
        Assert.Equal(2, stateB);
        Assert.Equal(0, v1b);
        Assert.Equal("x", v2b);
        Assert.False(v3b);
        Assert.Equal(0.0, v4b);
        Assert.Equal('\0', v5b);

        var c = Choice<int, string, bool, double, char>.FromT3(true);
        c.Deconstruct(out var stateC, out var v1c, out var v2c, out var v3c, out var v4c, out var v5c);
        Assert.Equal(3, stateC);
        Assert.Equal(0, v1c);
        Assert.Null(v2c);
        Assert.True(v3c);
        Assert.Equal(0.0, v4c);
        Assert.Equal('\0', v5c);

        var d = Choice<int, string, bool, double, char>.FromT4(3.14);
        d.Deconstruct(out var stateD, out var v1d, out var v2d, out var v3d, out var v4d, out var v5d);
        Assert.Equal(4, stateD);
        Assert.Equal(0, v1d);
        Assert.Null(v2d);
        Assert.False(v3d);
        Assert.Equal(3.14, v4d);
        Assert.Equal('\0', v5d);

        var e = Choice<int, string, bool, double, char>.FromT5('W');
        e.Deconstruct(out var stateE, out var v1e, out var v2e, out var v3e, out var v4e, out var v5e);
        Assert.Equal(5, stateE);
        Assert.Equal(0, v1e);
        Assert.Null(v2e);
        Assert.False(v3e);
        Assert.Equal(0.0, v4e);
        Assert.Equal('W', v5e);

        var df = default(Choice<int, string, bool, double, char>);
        df.Deconstruct(out var stateDf, out var v1df, out var v2df, out var v3df, out var v4df, out var v5df);
        Assert.Equal(0, stateDf);
        Assert.Equal(0, v1df);
        Assert.Null(v2df);
        Assert.False(v3df);
        Assert.Equal(0.0, v4df);
        Assert.Equal('\0', v5df);
    }

    [Fact]
    public void GetHashCode_DiffersAcrossArmsWithSameValueHash() {
        var t1 = Choice<int, int, int, int, int>.FromT1(42);
        var t2 = Choice<int, int, int, int, int>.FromT2(42);
        var t3 = Choice<int, int, int, int, int>.FromT3(42);
        var t4 = Choice<int, int, int, int, int>.FromT4(42);
        var t5 = Choice<int, int, int, int, int>.FromT5(42);

        Assert.NotEqual(t1.GetHashCode(), t2.GetHashCode());
        Assert.NotEqual(t1.GetHashCode(), t3.GetHashCode());
        Assert.NotEqual(t1.GetHashCode(), t4.GetHashCode());
        Assert.NotEqual(t1.GetHashCode(), t5.GetHashCode());
        Assert.NotEqual(t2.GetHashCode(), t3.GetHashCode());
        Assert.NotEqual(t3.GetHashCode(), t4.GetHashCode());
        Assert.NotEqual(t4.GetHashCode(), t5.GetHashCode());
    }

    [Fact]
    public void GetHashCode_EqualChoices_HaveEqualHashes() {
        var a1 = Choice<int, string, bool, double, char>.FromT1(1);
        var a2 = Choice<int, string, bool, double, char>.FromT1(1);
        Assert.True(a1.Equals(a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());

        a1 = Choice<int, string, bool, double, char>.FromT2("test");
        a2 = Choice<int, string, bool, double, char>.FromT2("test");
        Assert.True(a1.Equals(a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());

        a1 = Choice<int, string, bool, double, char>.FromT3(false);
        a2 = Choice<int, string, bool, double, char>.FromT3(false);
        Assert.True(a1.Equals(a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());

        a1 = Choice<int, string, bool, double, char>.FromT4(2.5);
        a2 = Choice<int, string, bool, double, char>.FromT4(2.5);
        Assert.True(a1.Equals(a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());

        a1 = Choice<int, string, bool, double, char>.FromT5('M');
        a2 = Choice<int, string, bool, double, char>.FromT5('M');
        Assert.True(a1.Equals(a2));
        Assert.Equal(a1.GetHashCode(), a2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DefaultStruct_IsStableAndDistinctFromActiveArms() {
        var d = default(Choice<int, string, bool, double, char>);
        var hDefault = d.GetHashCode();
        Assert.Equal(d, default(Choice<int, string, bool, double, char>));
        Assert.Equal(hDefault, default(Choice<int, string, bool, double, char>).GetHashCode());

        var active = Choice<int, string, bool, double, char>.FromT5('Z');
        Assert.NotEqual(hDefault, active.GetHashCode());
    }
}
