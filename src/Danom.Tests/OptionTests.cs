namespace Danom.Tests;

using System.Globalization;
using Danom.TestHelpers;
using Xunit;

public sealed class OptionTests {
    [Fact]
    public void Count() {
        Assert.Equal(1, Option<int>.Some(1).Count);
        Assert.Equal(0, Option<int>.NoneValue.Count);
    }

    [Fact]
    public void Some() {
        var option = Option<int>.Some(1);
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
        Assert.Equal("Some(1)", option.ToString());
    }

    [Fact]
    public void None() {
        AssertOption.IsNone(Option<int>.NoneValue);
        var option = Option<int>.NoneValue;
        AssertOption.IsNone(option);
        Assert.False(option.IsSome);
        Assert.Equal("None", option.ToString());

        var option2 = Option<Unit>.NoneValue;
        Assert.Equal(Option<Unit>.NoneValue, option2);
    }

    [Fact]
    public void NoneValue() {
        var option = Option<int>.NoneValue;
        AssertOption.IsNone(option);
        Assert.False(option.IsSome);
    }

    [Fact]
    public void NullShouldProduceNone() {
        var option = Option<object>.Some(default!);
        AssertOption.IsNone(option);
        Assert.False(option.IsSome);
    }

    [Fact]
    public void Match() {
        Assert.Equal(1,
            Option<int>.Some(1)
                .Match(x => x, () => -1));

        Assert.Equal(1,
            Option<int>.NoneValue
                .Match(_ => -1, () => 1));
    }

    [Fact]
    public void MatchAction() {
        Option<int>.Some(1).Match(
            some: x => Assert.Equal(1, x),
            none: () => Assert.True(false));

        Option<int>.NoneValue.Match(
            some: x => Assert.True(false),
            none: () => Assert.True(true));
    }

    [Fact]
    public void Bind() {
        AssertOption.IsSome(2, Option<int>.Some(1).Bind(x => Option<int>.Some(x + 1)));
        AssertOption.IsNone(Option<int>.NoneValue.Bind(x => Option<int>.Some(x + 1)));
    }

    [Fact]
    public void Bind_SelectorNotInvokedForNone() {
        var called = false;
        var res = Option<int>.NoneValue.Bind(x => { called = true; return Option<int>.Some(x + 1); });
        Assert.False(called);
        AssertOption.IsNone(res);
    }

    [Fact]
    public void Map() {
        AssertOption.IsSome(2, Option<int>.Some(1).Map(x => x + 1));
        AssertOption.IsNone(Option<int>.NoneValue.Map(x => x + 1));
    }

    [Fact]
    public void Map_SelectorNotInvokedForNone() {
        var called = false;
        var res = Option<int>.NoneValue.Map(x => { called = true; return x + 1; });
        Assert.False(called);
        AssertOption.IsNone(res);
    }

    [Fact]
    public void Map_SelectorExceptionPropagates() {
        Assert.Throws<InvalidOperationException>(() => Option<int>.Some(1).Map<int>(_ => throw new InvalidOperationException()));
    }

    [Fact]
    public void Apply() {
        var funcOption = Option<Func<int, int>>.Some(x => x + 1);
        AssertOption.IsSome(2, Option<int>.Some(1).Apply(funcOption));
        AssertOption.IsNone(Option<int>.NoneValue.Apply(funcOption));
        AssertOption.IsNone(Option<int>.Some(1).Apply(Option<Func<int, int>>.NoneValue));
    }

    [Fact]
    public void Apply_FunctionExceptionPropagates() {
        var funcOption = Option<Func<int, int>>.Some(_ => throw new InvalidOperationException());
        Assert.Throws<InvalidOperationException>(() => Option<int>.Some(1).Apply(funcOption));
    }

    [Fact]
    public void Iter() {
        var result = 0;
        Option<int>.Some(1).Iter(
            x => result += x);
        Assert.Equal(1, result);

        result = 0;
        Option<int>.NoneValue.Iter(
            x => result += x);
        Assert.Equal(0, result);
    }

    [Fact]
    public void DefaultValue() {
        Assert.Equal(1, Option<int>.NoneValue.DefaultValue(1));
        Assert.Equal(2, Option<int>.Some(2).DefaultValue(1));
    }

    [Fact]
    public void DefaultWith() {
        Assert.Equal(1, Option<int>.NoneValue.DefaultWith(() => 1));
        Assert.Equal(2, Option<int>.Some(2).DefaultWith(() => 1));

        var called = false;
        var noneResult = Option<int>.NoneValue.DefaultWith(() => { called = true; return 5; });
        Assert.True(called);
        Assert.Equal(5, noneResult);

        called = false;
        var someResult = Option<int>.Some(3).DefaultWith(() => { called = true; return 5; });
        Assert.False(called);
        Assert.Equal(3, someResult);
    }

    [Fact]
    public void OrElse() {
        AssertOption.IsSome(1, Option<int>.NoneValue.OrElse(Option<int>.Some(1)));
        AssertOption.IsSome(2, Option<int>.Some(2).OrElse(Option<int>.Some(1)));
    }

    [Fact]
    public void OrElseWith() {
        AssertOption.IsSome(1, Option<int>.NoneValue.OrElseWith(() => Option<int>.Some(1)));
        AssertOption.IsSome(2, Option<int>.Some(2).OrElseWith(() => Option<int>.Some(1)));

        var called = false;
        var noneOrElse = Option<int>.NoneValue.OrElseWith(() => { called = true; return Option<int>.Some(9); });
        Assert.True(called);
        AssertOption.IsSome(9, noneOrElse);

        called = false;
        var someOrElse = Option<int>.Some(2).OrElseWith(() => { called = true; return Option<int>.Some(9); });
        Assert.False(called);
        AssertOption.IsSome(2, someOrElse);
    }

    [Fact]
    public void TryGet() {
        if (Option<int>.Some(1).TryGet(out var x)) {
            Assert.Equal(1, x);
        }
        else {
            Assert.Fail("Failed to TryGet a valid Option");
        }
    }

    [Fact]
    public void TryGet_None() {
        if (Option<int>.NoneValue.TryGet(out var y)) {
            Assert.Fail("Expected None but got Some.");
        }
        else {
            Assert.Equal(0, y);
        }
    }

    [Fact]
    public void TryGet_ReturnsDefaultForNone_ValueTypeAndRefType() {
        // value type
        if (Option<int>.NoneValue.TryGet(out var vi)) {
            Assert.Fail("Expected None");
        }
        else {
            Assert.Equal(default(int), vi);
        }

        // reference type
        if (Option<string>.NoneValue.TryGet(out var vs)) {
            Assert.Fail("Expected None");
        }
        else {
            Assert.Null(vs);
        }
    }

    [Fact]
    public void Exists() {
        Assert.True(Option<int>.Some(1).Exists(x => x == 1));
        Assert.False(Option<int>.Some(1).Exists(x => x == 2));
        Assert.False(Option<int>.NoneValue.Exists(x => true));
    }

    [Fact]
    public void Contains() {
        Assert.True(Option<int>.Some(1).Contains(1));
        Assert.False(Option<int>.Some(1).Contains(2));
        Assert.False(Option<int>.NoneValue.Contains(1));
    }

    [Fact]
    public void ToList() {
        var someList = Option<int>.Some(1).ToList();
        Assert.Single(someList);
        Assert.Equal(1, someList[0]);

        var noneList = Option<int>.NoneValue.ToList();
        Assert.Empty(noneList);
    }

    [Fact]
    public void ToArray() {
        var someArray = Option<int>.Some(1).ToArray();
        Assert.Single(someArray);
        Assert.Equal(1, someArray[0]);

        var noneArray = Option<int>.NoneValue.ToArray();
        Assert.Empty(noneArray);
    }

    [Fact]
    public void ToStringStandard() {
        Assert.Equal("Some(1)", Option<int>.Some(1).ToString());
        Assert.Equal("None", Option<int>.NoneValue.ToString());
        Assert.Equal("Some()", Option.Some().ToString());
    }

    [Fact]
    public void ToStringOverload() {
        Assert.Equal("1", Option<int>.Some(1).ToString(""));
        Assert.Equal("Here", Option<int>.NoneValue.ToString("Here"));
        Assert.Equal("12345.68", Option<double>.Some(12345.6789).ToString("", "F2"));
        Assert.Equal("1967-01-01", Option<DateTime>.Some(new DateTime(1967, 1, 1)).ToString("", "yyyy-MM-dd"));

        var culture = new CultureInfo("en-US");
        Assert.Equal("1967-01-01", Option<DateTime>.Some(new DateTime(1967, 1, 1)).ToString("", "yyyy-MM-dd", culture));

        culture = new CultureInfo("fr-FR");
        Assert.Equal("None", Option<int>.NoneValue.ToString("None", "F2", culture));
        Assert.Equal("None", Option<DateTime>.NoneValue.ToString("None", "yyyy-MM-dd", culture));

        culture = new CultureInfo("fr-FR");
        Assert.Equal("12 345,68", Option<double>.Some(12345.6789).ToString("", "N2", culture));
    }

    [Fact]
    public void Equality() {
        Assert.Equal(Option<int>.NoneValue, Option<int>.NoneValue);
        Assert.Equal(Option<int>.Some(1), Option<int>.Some(1));
        Assert.NotEqual(Option<int>.Some(1), Option<int>.Some(2));
        Assert.NotEqual(Option<int>.Some(1), Option<int>.NoneValue);
    }

    [Fact]
    public void GetHashCodeShouldBeZero() {
        Assert.Equal(0, Option<int>.NoneValue.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ForSomeUsesValueHash() {
        var some = Option<string>.Some("abc");
        Assert.Equal("abc".GetHashCode(), some.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ForNoneIsStable() {
        var none1 = Option<string>.NoneValue;
        var none2 = Option<string>.NoneValue;
        Assert.Equal(none1.GetHashCode(), none2.GetHashCode());
    }

    [Fact]
    public void CompareTo() {
        Assert.Equal(0, Option<int>.NoneValue.CompareTo(Option<int>.NoneValue));
        Assert.Equal(0, Option<int>.Some(1).CompareTo(Option<int>.Some(1)));
        Assert.True(Option<int>.Some(1).CompareTo(Option<int>.Some(2)) < 0);
        Assert.True(Option<int>.Some(2).CompareTo(Option<int>.Some(1)) > 0);
        Assert.True(Option<int>.NoneValue.CompareTo(Option<int>.Some(1)) < 0);
        Assert.True(Option<int>.Some(1).CompareTo(Option<int>.NoneValue) > 0);
    }

    [Fact]
    public void Operator_Equality() {
        Assert.True(Option<int>.NoneValue == Option<int>.NoneValue);
        Assert.True(Option<int>.Some(1) == Option<int>.Some(1));
        Assert.False(Option<int>.Some(1) == Option<int>.Some(2));
        Assert.False(Option<int>.Some(1) == Option<int>.NoneValue);
    }

    [Fact]
    public void Operator_Inequality() {
        Assert.False(Option<int>.NoneValue != Option<int>.NoneValue);
        Assert.False(Option<int>.Some(1) != Option<int>.Some(1));
        Assert.True(Option<int>.Some(1) != Option<int>.Some(2));
        Assert.True(Option<int>.Some(1) != Option<int>.NoneValue);
    }

    [Fact]
    public void Operator_LessThan() {
        Assert.True(Option<int>.NoneValue < Option<int>.Some(1));
        Assert.False(Option<int>.Some(1) < Option<int>.NoneValue);
        Assert.False(Option<int>.Some(1) < Option<int>.Some(1));
        Assert.True(Option<int>.Some(1) < Option<int>.Some(2));
        Assert.False(Option<int>.Some(2) < Option<int>.Some(1));
    }

    [Fact]
    public void Operator_LessThanOrEqual() {
        Assert.True(Option<int>.NoneValue <= Option<int>.Some(1));
        Assert.False(Option<int>.Some(1) <= Option<int>.NoneValue);
        Assert.True(Option<int>.Some(1) <= Option<int>.Some(1));
        Assert.True(Option<int>.Some(1) <= Option<int>.Some(2));
        Assert.False(Option<int>.Some(2) <= Option<int>.Some(1));
    }

    [Fact]
    public void Operator_GreaterThan() {
        Assert.False(Option<int>.NoneValue > Option<int>.Some(1));
        Assert.True(Option<int>.Some(1) > Option<int>.NoneValue);
        Assert.False(Option<int>.Some(1) > Option<int>.Some(1));
        Assert.False(Option<int>.Some(1) > Option<int>.Some(2));
        Assert.True(Option<int>.Some(2) > Option<int>.Some(1));
    }

    [Fact]
    public void Operator_GreaterThanOrEqual() {
        Assert.False(Option<int>.NoneValue >= Option<int>.Some(1));
        Assert.True(Option<int>.Some(1) >= Option<int>.NoneValue);
        Assert.True(Option<int>.Some(1) >= Option<int>.Some(1));
        Assert.False(Option<int>.Some(1) >= Option<int>.Some(2));
        Assert.True(Option<int>.Some(2) >= Option<int>.Some(1));
    }
}

public sealed class OptionModuleTests {
    [Fact]
    public void NoneValue() {
        AssertOption.IsNone(Option.NoneValue);
        Assert.Equal(Option<Unit>.NoneValue, Option.NoneValue);
    }

    [Fact]
    public void None() {
        AssertOption.IsNone(Option.None());
        Assert.Equal(Option<Unit>.NoneValue, Option.None());
    }

    [Fact]
    public void Some() {
        var option = Option.Some(1);
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
        Assert.Equal("Some(1)", option.ToString());
    }

    [Fact]
    public void SomeUnit() {
        var option = Option.Some();
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
        Assert.Equal("Some()", option.ToString());
    }

    [Fact]
    public void Match_IEnumerable() {
        Assert.Equal(6, Option.Match(
            [Option.Some(1), Option.Some(2), Option.Some(3)],
            none: () => -1,
            some: options => options.Sum()));

        Assert.Equal(-1, Option.Match(
            [Option.Some(1), Option<int>.NoneValue, Option.Some(3)],
            none: () => -1,
            some: options => options.Sum()));

        Assert.Equal(-1, Option.Match(
            [Option<int>.NoneValue, Option<int>.NoneValue],
            none: () => -1,
            some: options => options.Sum()));

        Assert.Equal(99, Option.Match(
            Enumerable.Empty<Option<int>>(),
            none: () => -1,
            some: _ => 99));
    }

    [Fact]
    public void Match() {
        Assert.Equal(42, Option.Match(Option.Some(21), Option.Some(21), some: (o1, o2) => o1 + o2, none: () => -1));
        Assert.Equal(-1, Option.Match(Option<int>.NoneValue, Option.Some(21), some: (o1, o2) => o1 + o2, none: () => -1));
        Assert.Equal(-1, Option.Match(Option.Some(21), Option<int>.NoneValue, some: (o1, o2) => o1 + o2, none: () => -1));
        Assert.Equal(-1, Option.Match(Option<int>.NoneValue, Option<int>.NoneValue, some: (o1, o2) => o1 + o2, none: () => -1));

        Assert.Equal(42, Option.Match(Option.Some(14), Option.Some(14), Option.Some(14), some: (o1, o2, o3) => o1 + o2 + o3, none: () => -1));
        Assert.Equal(-1, Option.Match(Option<int>.NoneValue, Option.Some(14), Option.Some(14), some: (o1, o2, o3) => o1 + o2 + o3, none: () => -1));
        Assert.Equal(-1, Option.Match(Option.Some(14), Option<int>.NoneValue, Option.Some(14), some: (o1, o2, o3) => o1 + o2 + o3, none: () => -1));
        Assert.Equal(-1, Option.Match(Option.Some(14), Option.Some(14), Option<int>.NoneValue, some: (o1, o2, o3) => o1 + o2 + o3, none: () => -1));
    }

    [Fact]
    public void Bind() {
        AssertOption.IsSome(3, Option.Bind(Option.Some(1), Option.Some(2), (x, y) => Option.Some(x + y)));
        AssertOption.IsNone(Option.Bind(Option<int>.NoneValue, Option.Some(2), (x, y) => Option.Some(x + y)));
        AssertOption.IsNone(Option.Bind(Option.Some(1), Option<int>.NoneValue, (x, y) => Option.Some(x + y)));

        AssertOption.IsSome(6, Option.Bind(Option.Some(1), Option.Some(2), Option.Some(3), (x, y, z) => Option.Some(x + y + z)));
        AssertOption.IsNone(Option.Bind(Option<int>.NoneValue, Option.Some(2), Option.Some(3), (x, y, z) => Option.Some(x + y + z)));
        AssertOption.IsNone(Option.Bind(Option.Some(1), Option<int>.NoneValue, Option.Some(3), (x, y, z) => Option.Some(x + y + z)));
        AssertOption.IsNone(Option.Bind(Option.Some(1), Option.Some(2), Option<int>.NoneValue, (x, y, z) => Option.Some(x + y + z)));
    }

    [Fact]
    public void Map() {
        AssertOption.IsSome(3, Option.Map(Option.Some(1), Option.Some(2), (x, y) => x + y));
        AssertOption.IsNone(Option.Map(Option<int>.NoneValue, Option.Some(2), (x, y) => x + y));
        AssertOption.IsNone(Option.Map(Option.Some(1), Option<int>.NoneValue, (x, y) => x + y));

        AssertOption.IsSome(6, Option.Map(Option.Some(1), Option.Some(2), Option.Some(3), (x, y, z) => x + y + z));
        AssertOption.IsNone(Option.Map(Option<int>.NoneValue, Option.Some(2), Option.Some(3), (x, y, z) => x + y + z));
        AssertOption.IsNone(Option.Map(Option.Some(1), Option<int>.NoneValue, Option.Some(3), (x, y, z) => x + y + z));
        AssertOption.IsNone(Option.Map(Option.Some(1), Option.Some(2), Option<int>.NoneValue, (x, y, z) => x + y + z));
    }

    [Fact]
    public void Flatten() {
        AssertOption.IsSome(1, Option.Flatten(Option.Some(Option.Some(1))));
        AssertOption.IsNone(Option.Flatten(Option.Some(Option<int>.NoneValue)));
    }

    [Fact]
    public void Flatten_IEnumerable() {
        Assert.Empty(Option.Flatten([Option<int>.NoneValue]));
        Assert.Equal(2, Option.Flatten([Option.Some(1), Option.Some(2)]).Count());
    }

    [Fact]
    public void Traverse() {
        var option = Option.Traverse([1, 2, 3], x => Option.Some(x * 2));

        Assert.True(option.TryGet(out var value));
        var valueList = value.ToList();
        Assert.Equal(3, valueList.Count);
        Assert.Equal(2, valueList[0]);
        Assert.Equal(4, valueList[1]);
        Assert.Equal(6, valueList[2]);

        var optionWithNone = Option.Traverse(
            [1, 2, 3],
            x => x == 2 ? Option<int>.NoneValue : Option.Some(x * 2));

        AssertOption.IsNone(optionWithNone);

        option = Option.Traverse(
            Array.Empty<int>(),
            x => Option.Some(x * 2));

        AssertOption.IsSome([], option);
    }

    [Fact]
    public void Sequence() {
        var option = Option.Sequence(
            [Option.Some(1), Option.Some(2), Option.Some(3)]);

        Assert.True(option.TryGet(out var value));
        var valueList = value.ToList();
        Assert.Equal(3, valueList.Count);
        Assert.Equal(1, valueList[0]);
        Assert.Equal(2, valueList[1]);
        Assert.Equal(3, valueList[2]);

        var optionWithNone = Option.Sequence(
            [Option.Some(1), Option<int>.NoneValue, Option.Some(3)]);

        AssertOption.IsNone(optionWithNone);

        option = Option.Sequence(
            Array.Empty<Option<int>>());

        AssertOption.IsSome([], option);
    }

    [Fact]
    public void Zip() {
        AssertOption.IsSome((1, "a"), Option.Zip(Option.Some(1), Option.Some("a")));
        AssertOption.IsNone(Option.Zip(Option<int>.NoneValue, Option.Some("a")));
        AssertOption.IsNone(Option.Zip(Option.Some(1), Option<string>.NoneValue));
        AssertOption.IsSome((1, "a", 2.0), Option.Zip(Option.Some(1), Option.Some("a"), Option.Some(2.0)));
        AssertOption.IsNone(Option.Zip(Option<int>.NoneValue, Option.Some("a"), Option.Some(2.0)));
        AssertOption.IsNone(Option.Zip(Option.Some(1), Option<string>.NoneValue, Option.Some(2.0)));
        AssertOption.IsNone(Option.Zip(Option.Some(1), Option.Some("a"), Option<double>.NoneValue));
    }
}
