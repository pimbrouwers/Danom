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
    public async Task SomeAsyncFromTask() {
        var option = await Option<int>.SomeAsync(Task<int>.Factory.StartNew(() => 1));
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
    }

    [Fact]
    public async Task SomeAsyncFromValueTask() {
        var option = await Option<int>.SomeAsync(new ValueTask<int>(1));
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
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
    public async Task NoneAsync() {
        var option = await Option<int>.NoneAsync();
        AssertOption.IsNone(option);
        Assert.False(option.IsSome);
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
    }

    [Fact]
    public void DefaultWith_IsLazy() {
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
    }

    [Fact]
    public void OrElseWith_IsLazy() {
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
    public void EqualityOperator() {
        Assert.True(Option<int>.NoneValue == Option<int>.NoneValue);
        Assert.True(Option<int>.Some(1) == Option<int>.Some(1));
        Assert.False(Option<int>.Some(1) == Option<int>.Some(2));
        Assert.False(Option<int>.Some(1) == Option<int>.NoneValue);
    }

    [Fact]
    public void InequalityOperator() {
        Assert.False(Option<int>.NoneValue != Option<int>.NoneValue);
        Assert.False(Option<int>.Some(1) != Option<int>.Some(1));
        Assert.True(Option<int>.Some(1) != Option<int>.Some(2));
        Assert.True(Option<int>.Some(1) != Option<int>.NoneValue);
    }

    [Fact]
    public void ToStringOverloads() {
        Assert.Equal("1", Option<int>.Some(1).ToString(""));
        Assert.Equal("Here", Option<int>.NoneValue.ToString("Here"));
        Assert.Equal("12345.68", Option<double>.Some(12345.6789).ToString("", "F2"));
        Assert.Equal("1967-01-01", Option<DateTime>.Some(new DateTime(1967, 1, 1)).ToString("", "yyyy-MM-dd"));

        var culture = new CultureInfo("en-US");
        Assert.Equal("1967-01-01", Option<DateTime>.Some(new DateTime(1967, 1, 1)).ToString("", "yyyy-MM-dd", culture));
    }

    [Fact]
    public void ToString_None_IgnoresFormatAndProvider() {
        var culture = new CultureInfo("fr-FR");
        Assert.Equal("None", Option<int>.NoneValue.ToString("None", "F2", culture));
        Assert.Equal("None", Option<DateTime>.NoneValue.ToString("None", "yyyy-MM-dd", culture));
    }

    [Fact]
    public void ToString_NumberFormattingWithCulture() {
        var culture = new CultureInfo("fr-FR");
        Assert.Equal("12 345,68", Option<double>.Some(12345.6789).ToString("", "N2", culture));
    }

    [Fact]
    public void GetHashCode_ForSomeUsesValueHash() {
        var some = Option<string>.Some("abc");
        Assert.Equal("abc".GetHashCode(), some.GetHashCode());
    }
}

public sealed class OptionStaticTests {
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
    public async Task NoneAsync() {
        var option = await Option.NoneAsync();
        AssertOption.IsNone(option);
        Assert.Equal(Option<Unit>.NoneValue, option);
    }

    [Fact]
    public void Some() {
        var option = Option.Some(1);
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
        Assert.Equal("Some(1)", option.ToString());
    }

    [Fact]
    public async Task SomeAsyncFromTask() {
        var option = await Option.SomeAsync(Task<int>.Factory.StartNew(() => 1));
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
    }

    [Fact]
    public async Task SomeAsyncFromValueTask() {
        var option = await Option.SomeAsync(new ValueTask<int>(1));
        AssertOption.IsSome(option);
        Assert.False(option.IsNone);
    }

    [Fact]
    public void Match() {
        var result = Option.Match(Option.Some(21), Option.Some(21), none: () => -1,
            some: (o1, o2) => o1 + o2,
            some1: _ => -1,
            some2: _ => -1);

        Assert.Equal(42, result);
    }

    [Fact]
    public void Match_None1() {
        var result = Option.Match(Option<int>.NoneValue, Option.Some(21), none: () => -1,
            some: (o1, o2) => o1 + o2,
            some1: _ => -1,
            some2: _ => -1);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void Match_None2() {
        Assert.Equal(-1,
            Option.Match(Option.Some(21), Option<int>.NoneValue, none: () => -1,
            some: (o1, o2) => o1 + o2,
            some1: _ => -1,
            some2: _ => -1));
    }

    [Fact]
    public void Match_BothNone() {
        var result = Option.Match(Option<int>.NoneValue, Option<int>.NoneValue, none: () => -1,
            some: (o1, o2) => o1 + o2,
            some1: _ => -1,
            some2: _ => -1);

        Assert.Equal(-1, result);
    }

    [Fact]
    public void Match_Some1() {
        var result = Option.Match(Option.Some(21), Option<int>.NoneValue, none: () => -1,
            some: (o1, o2) => o1 + o2,
            some1: o1 => o1 * 2,
            some2: _ => -1);

        Assert.Equal(42, result);
    }

    [Fact]
    public void Match_Some2() {
        var result = Option.Match(Option<int>.NoneValue, Option.Some(21), none: () => -1,
            some: (o1, o2) => o1 + o2,
            some1: _ => -1,
            some2: o2 => o2 * 2);

        Assert.Equal(42, result);
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
    public void Bind2() {
        AssertOption.IsSome(3, Option.Bind(Option.Some(1), Option.Some(2), (x, y) => Option.Some(x + y)));
        AssertOption.IsNone(Option.Bind(Option<int>.NoneValue, Option.Some(2), (x, y) => Option.Some(x + y)));
        AssertOption.IsNone(Option.Bind(Option.Some(1), Option<int>.NoneValue, (x, y) => Option.Some(x + y)));
    }

    [Fact]
    public void Bind3() {
        AssertOption.IsSome(6, Option.Bind(Option.Some(1), Option.Some(2), Option.Some(3), (x, y, z) => Option.Some(x + y + z)));
        AssertOption.IsNone(Option.Bind(Option<int>.NoneValue, Option.Some(2), Option.Some(3), (x, y, z) => Option.Some(x + y + z)));
        AssertOption.IsNone(Option.Bind(Option.Some(1), Option<int>.NoneValue, Option.Some(3), (x, y, z) => Option.Some(x + y + z)));
        AssertOption.IsNone(Option.Bind(Option.Some(1), Option.Some(2), Option<int>.NoneValue, (x, y, z) => Option.Some(x + y + z)));
    }

    [Fact]
    public void Map2() {
        AssertOption.IsSome(3, Option.Map(Option.Some(1), Option.Some(2), (x, y) => x + y));
        AssertOption.IsNone(Option.Map(Option<int>.NoneValue, Option.Some(2), (x, y) => x + y));
        AssertOption.IsNone(Option.Map(Option.Some(1), Option<int>.NoneValue, (x, y) => x + y));
    }

    [Fact]
    public void Map3() {
        AssertOption.IsSome(6, Option.Map(Option.Some(1), Option.Some(2), Option.Some(3), (x, y, z) => x + y + z));
        AssertOption.IsNone(Option.Map(Option<int>.NoneValue, Option.Some(2), Option.Some(3), (x, y, z) => x + y + z));
        AssertOption.IsNone(Option.Map(Option.Some(1), Option<int>.NoneValue, Option.Some(3), (x, y, z) => x + y + z));
        AssertOption.IsNone(Option.Map(Option.Some(1), Option.Some(2), Option<int>.NoneValue, (x, y, z) => x + y + z));
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
    }

    [Fact]
    public void Traverse_EmptyCollection_ReturnsSomeWithEmptyCollection() {
        var option = Option.Traverse(
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
    }

    [Fact]
    public void Sequence_EmptyCollection_ReturnsSomeWithEmptyCollection() {
        var option = Option.Sequence(
            Array.Empty<Option<int>>());

        AssertOption.IsSome([], option);
    }
}
