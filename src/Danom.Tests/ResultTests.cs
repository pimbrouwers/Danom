namespace Danom.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class ResultTests {
    [Fact]
    public void Count() {
        Assert.Equal(1, Result<int, string>.Ok(1).Count);
        Assert.Equal(0, Result<int, string>.Error("Error").Count);
    }

    [Fact]
    public void Ok() {
        var result = Result<int, string>.Ok(1);
        AssertResult.IsOk(result);
        Assert.False(result.IsError);
    }

    [Fact]
    public void Error() {
        var result = Result<int, string>.Error("Error");
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    [Fact]
    public void Match() {
        Assert.Equal(1,
            Result<int, string>.Ok(1)
                .Match(x => x, _ => -1));

        Assert.Equal(1,
            Result<int, string>.Error("Error")
                .Match(_ => -1, _ => 1));
    }

    [Fact]
    public void MatchAction() {
        var ok = false;
        var error = false;

        Result<int, string>.Ok(1)
            .Match(ok: _ => ok = true, error: _ => error = true);

        Assert.True(ok);
        Assert.False(error);

        ok = false;
        error = false;

        Result<int, string>.Error("Error")
            .Match(ok: _ => ok = true, error: _ => error = true);

        Assert.False(ok);
        Assert.True(error);
    }

    [Fact]
    public void Bind() {
        AssertResult.IsOk(2, Result<int, string>.Ok(1).Bind(x => Result<int, string>.Ok(x + 1)));
        AssertResult.IsError(Result<int, string>.Error("Error").Bind(x => Result<int, string>.Ok(x + 1)));
    }

    [Fact]
    public void Map() {
        AssertResult.IsOk(2, Result<int, string>.Ok(1).Map(x => x + 1));
        AssertResult.IsError(Result<int, string>.Error("Error").Map(x => x + 1));
    }

    [Fact]
    public void MapError() {
        var ok = Result<int, string>.Ok(1).MapError(e => e.ToUpperInvariant());
        AssertResult.IsOk(ok);

        var err = Result<int, string>.Error("bad").MapError(e => e.ToUpperInvariant());
        AssertResult.IsError(err);
        err.Match(_ => Assert.Fail("Expected error"), e => Assert.Equal("BAD", e));
    }

    [Fact]
    public void Apply() {
        var funcResult = Result<Func<int, int>, string>.Ok(x => x + 1);
        var valueResult = Result<int, string>.Ok(1);
        var applied = valueResult.Apply(funcResult);
        AssertResult.IsOk(2, applied);

        var funcError = Result<Func<int, int>, string>.Error("Func error");
        applied = valueResult.Apply(funcError);
        AssertResult.IsError(applied);

        var valueError = Result<int, string>.Error("Value error");
        applied = valueError.Apply(funcResult);
        AssertResult.IsError(applied);

        applied = valueError.Apply(funcError);
        AssertResult.IsError(applied);
    }

    [Fact]
    public void Iter() {
        var called = false;
        Result<int, string>.Ok(1).Iter(_ => called = true);
        Assert.True(called);

        called = false;
        Result<int, string>.Error("Error").Iter(_ => called = true);
        Assert.False(called);
    }

    [Fact]
    public void Tap() {
        var executed = false;
        var capturedValue = 0;

        var tapped = Result<int, string>.Ok(42).Tap(x => {
            executed = true;
            capturedValue = x;
        });

        Assert.True(executed);
        Assert.Equal(42, capturedValue);
        AssertResult.IsOk(42, tapped);

        executed = false;
        tapped = Result<int, string>.Error("Error").Tap(x => executed = true);

        Assert.False(executed);
        AssertResult.IsError(tapped);
    }

    [Fact]
    public void DefaultValue() {
        Assert.Equal(1, Result<int, string>.Error("Error").DefaultValue(1));
        Assert.Equal(2, Result<int, string>.Ok(2).DefaultValue(1));
    }

    [Fact]
    public void DefaultWith() {
        Assert.Equal(1, Result<int, string>.Error("Error").DefaultWith(() => 1));
        Assert.Equal(2, Result<int, string>.Ok(2).DefaultWith(() => 1));
    }

    [Fact]
    public void OrElse() {
        var result = Result<int, string>.Ok(1)
            .OrElse(Result<int, string>.Ok(2));
        AssertResult.IsOk(1, result);

        result = Result<int, string>.Error("Error")
            .OrElse(Result<int, string>.Ok(2));
        AssertResult.IsOk(2, result);
    }

    [Fact]
    public void OrElseWith() {
        var result = Result<int, string>.Ok(1)
            .OrElseWith(() => Result<int, string>.Ok(2));
        AssertResult.IsOk(1, result);

        result = Result<int, string>.Error("Error")
            .OrElseWith(() => Result<int, string>.Ok(2));
        AssertResult.IsOk(2, result);
    }

    [Fact]
    public void TryGet() {
        var result = Result<int, string>.Ok(1);
        if (result.TryGet(out var value)) {
            Assert.Equal(1, value);
        }
        else {
            Assert.Fail("Expected Ok result but got Error.");
        }
    }

    [Fact]
    public void TryGetError() {
        var resultE = Result<int, string>.Error("Error");
        if (resultE.TryGetError(out var errorE)) {
            Assert.Equal("Error", errorE);
        }
        else {
            Assert.Fail("Expected Error result but got Ok.");
        }
    }

    [Fact]
    public void Exists() {
        var ok = Result<int, string>.Ok(1);
        Assert.True(ok.Exists(x => x == 1));
        Assert.False(ok.Exists(x => x == 2));

        var err = Result<int, string>.Error("Error");
        Assert.False(err.Exists(x => true));
    }

    [Fact]
    public void Contains() {
        var ok = Result<int, string>.Ok(1);
        Assert.True(ok.Contains(1));
        Assert.False(ok.Contains(2));

        var err = Result<int, string>.Error("Error");
        Assert.False(err.Contains(1));
    }

    [Fact]
    public void ToList() {
        var ok = Result<int, string>.Ok(1);
        var okList = ok.ToList();
        Assert.Single(okList);
        Assert.Equal(1, okList[0]);

        var err = Result<int, string>.Error("Error");
        var errList = err.ToList();
        Assert.Empty(errList);
    }

    [Fact]
    public void ToArray() {
        var ok = Result<int, string>.Ok(1);
        var okArray = ok.ToArray();
        Assert.Single(okArray);
        Assert.Equal(1, okArray[0]);

        var err = Result<int, string>.Error("Error");
        var errArray = err.ToArray();
        Assert.Empty(errArray);
    }

    [Fact]
    public void Equality() {
        Assert.Equal(Result<int, string>.Error("Error"), Result<int, string>.Error("Error"));
        Assert.Equal(Result<int, string>.Ok(1), Result<int, string>.Ok(1));
        Assert.NotEqual(Result<int, string>.Ok(1), Result<int, string>.Ok(2));
        Assert.NotEqual(Result<int, string>.Ok(1), Result<int, string>.Error("Error"));
    }

    [Fact]
    public void EqualityOperator() {
        Assert.True(Result<int, string>.Error("Error") == Result<int, string>.Error("Error"));
        Assert.True(Result<int, string>.Ok(1) == Result<int, string>.Ok(1));
        Assert.False(Result<int, string>.Ok(1) == Result<int, string>.Ok(2));
        Assert.False(Result<int, string>.Ok(1) == Result<int, string>.Error("Error"));
    }

    [Fact]
    public void InequalityOperator() {
        Assert.False(Result<int, string>.Error("Error") != Result<int, string>.Error("Error"));
        Assert.False(Result<int, string>.Ok(1) != Result<int, string>.Ok(1));
        Assert.True(Result<int, string>.Ok(1) != Result<int, string>.Ok(2));
        Assert.True(Result<int, string>.Ok(1) != Result<int, string>.Error("Error"));
    }

    [Fact]
    public void GetHashCode_() {
        Assert.Equal(42.GetHashCode(), Result<int, string>.Ok(42).GetHashCode());
        Assert.Equal("Error".GetHashCode(), Result<int, string>.Error("Error").GetHashCode());
        Assert.Equal(0, Result<string?, int>.Ok(null).GetHashCode());
        Assert.Equal(0, Result<int, string?>.Error(null).GetHashCode());
    }

    [Fact]
    public void Equals_Object_WithDifferentType_ReturnsFalse() {
        var result = Result<int, string>.Ok(42);
        Assert.False(result.Equals("not a result"));
    }

    [Fact]
    public void Equals_WithNullValues() {
        var result1 = Result<string?, int>.Ok(null);
        var result2 = Result<string?, int>.Ok(null);
        Assert.True(result1.Equals(result2));

        var error1 = Result<int, string?>.Error(null);
        var error2 = Result<int, string?>.Error(null);
        Assert.True(error1.Equals(error2));
    }

    [Fact]
    public void ToString_OkWithNull_ReturnsOkEmpty() {
        var result = Result<string?, int>.Ok(null);
        Assert.Equal("Ok()", result.ToString());
    }

    [Fact]
    public void ToString_ErrorWithNull_ReturnsErrorEmpty() {
        var result = Result<int, string?>.Error(null);
        Assert.Equal("Error()", result.ToString());
    }

    [Fact]
    public void ToString_Ok() {
        var result = Result<int, string>.Ok(1);
        Assert.Equal("Ok(1)", result.ToString());
    }

    [Fact]
    public void ToString_Error() {
        var result = Result<int, string>.Error("Error");
        Assert.Equal("Error(Error)", result.ToString());
    }
}

public sealed class ResultTTests {
    [Fact]
    public void Ok() {
        var result = Result<int>.Ok(1);
        AssertResult.IsOk(result);
        Assert.False(result.IsError);
        Assert.Equal("Ok(1)", result.ToString());
    }

    [Fact]
    public async Task ErrorAsync() {
        var result = await Task.FromResult(Result<int>.Error("Error"));
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    [Fact]
    public void Error() {
        var result = Result<int>.Error(new ResultErrors("Error"));
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    [Fact]
    public void ErrorFromString() {
        var result = Result<int>.Error("Error");
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    [Fact]
    public void ErrorFromKeyAndValue() {
        var result = Result<int>.Error("Error", "Error message");
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    [Fact]
    public void ErrorFromKeyAndParams() {
        var result = Result<int>.Error("Error", "Error message 1", "Error message 2");
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }

    [Fact]
    public void Match() {
        Assert.Equal(1,
            Result<int>.Ok(1)
                .Match(x => x, _ => -1));

        Assert.Equal(1,
            Result<int>.Error("Error")
                .Match(_ => -1, _ => 1));
    }

    [Fact]
    public void Bind() {
        AssertResult.IsOk(2, Result<int>.Ok(1).Bind(x => Result<int>.Ok(x + 1)));
        AssertResult.IsError(Result<int>.Error("Error").Bind(x => Result<int>.Ok(x + 1)));
    }

    [Fact]
    public void Map() {
        AssertResult.IsOk(2, Result<int>.Ok(1).Map(x => x + 1));
        AssertResult.IsError(Result<int>.Error("Error").Map(x => x + 1));
    }

    [Fact]
    public void DefaultValue() {
        Assert.Equal(1, Result<int>.Error("Error").DefaultValue(1));
        Assert.Equal(2, Result<int>.Ok(2).DefaultValue(1));
    }

    [Fact]
    public void DefaultWith() {
        Assert.Equal(1, Result<int>.Error("Error").DefaultWith(() => 1));
        Assert.Equal(2, Result<int>.Ok(2).DefaultWith(() => 1));
    }

    [Fact]
    public void MapError() {
        var ok = Result<int, string>.Ok(1).MapError(e => e.ToUpperInvariant());
        AssertResult.IsOk(ok);

        var err = Result<int, string>.Error("bad").MapError(e => e.ToUpperInvariant());
        AssertResult.IsError(err);
        err.Match(_ => Assert.Fail("Expected error"), e => Assert.Equal("BAD", e));
    }

    [Fact]
    public void ToOption() {
        var some = Result<int, string>.Ok(42).ToOption();
        AssertOption.IsSome(42, some);

        var none = Result<int, string>.Error("oops").ToOption();
        AssertOption.IsNone(none);
    }

    [Fact]
    public void DefaultWith_IsLazy() {
        var called = false;
        var ok = Result<int, string>.Ok(7).DefaultWith(() => { called = true; return 99; });
        Assert.Equal(7, ok);
        Assert.False(called);

        called = false;
        var err = Result<int, string>.Error("e").DefaultWith(() => { called = true; return 99; });
        Assert.True(called);
        Assert.Equal(99, err);
    }

    [Fact]
    public void TryGet_FalseReturnsDefault() {
        var r = Result<int, string>.Error("e");
        Assert.False(r.TryGet(out var v));
        Assert.Equal(default, v);
    }

    [Fact]
    public void TryGetError_FalseReturnsDefault() {
        var r = Result<int, string>.Ok(5);
        Assert.False(r.TryGetError(out var e));
        Assert.Equal(default, e);
    }

    [Fact]
    public void Map_SelectorExceptionPropagates() {
        Assert.Throws<InvalidOperationException>(() =>
            Result<int, string>.Ok(1).Map<int>(_ => throw new InvalidOperationException()));
    }

    [Fact]
    public void Bind_SelectorExceptionPropagates() {
        Assert.Throws<InvalidOperationException>(() =>
            Result<int, string>.Ok(1).Bind<int>(_ => throw new InvalidOperationException()));
    }

    [Fact]
    public void DefaultStructBehavior() {
        var d = default(Result<int, string>);
        Assert.False(d.IsOk);
        Assert.False(d.IsError);

        // TryGet/TryGetError return defaults and appropriate flags
        Assert.False(d.TryGet(out var v));
        Assert.Equal(default, v);

        Assert.False(d.TryGetError(out var e));
        Assert.Equal(default, e);

        // ToString uses Error(e) with default error
        Assert.Throws<InvalidOperationException>(() => d.ToString());

        // HashCode/equality: default equals default
        Assert.Throws<InvalidOperationException>(() => d.Equals(default));
        Assert.Throws<InvalidOperationException>(() => d.GetHashCode());
    }

    [Fact]
    public void ErrorFromParams() {
        var result = Result<int>.Error("error1", "error2");
        AssertResult.IsError(result);
        Assert.False(result.IsOk);
    }
}

public sealed class ResultStaticTests {
    [Fact]
    public void Match_AllOk_InvokesOk() {
        var test = Result.Match(Result.Ok(1), Result.Ok(2),
            ok: (r1, r2) => {
                Assert.Equal(1, r1);
                Assert.Equal(2, r2);
                return Result.Ok(r1 + r2);
            },
            error: _ => {
                Assert.Fail("Expected Ok results but got Error.");
                return Result<int>.Error("Should not reach here");
            });

        Assert.True(test.TryGet(out var value));
        Assert.Equal(3, value);
    }

    [Fact]
    public void Match_IEnumerable() {
        Assert.Equal(6, Result.Match(
            [Result.Ok(1), Result.Ok(2), Result.Ok(3)],
            error: _ => -1,
            ok: options => options.Sum()));

        Assert.Equal(-1, Result.Match(
            [Result.Ok(1), Result<int>.Error(), Result.Ok(3)],
            error: _ => -1,
            ok: options => options.Sum()));

        Assert.Equal(-1, Result.Match(
            [Result<int>.Error(), Result<int>.Error()],
            error: _ => -1,
            ok: options => options.Sum()));

        Assert.Equal(99, Result.Match(
            Enumerable.Empty<Result<int, ResultErrors>>(),
            error: _ => -1,
            ok: _ => 99));
    }

    [Fact]
    public void Bind2() {
        AssertResult.IsOk(3, Result.Bind(Result.Ok(1), Result.Ok(2), (x, y) => Result.Ok(x + y)));
        AssertResult.IsError(Result.Bind(Result<int>.Error(), Result.Ok(2), (x, y) => Result.Ok(x + y)));
        AssertResult.IsError(Result.Bind(Result.Ok(1), Result<int>.Error(), (x, y) => Result.Ok(x + y)));
    }

    [Fact]
    public void Bind3() {
        AssertResult.IsOk(6, Result.Bind(Result.Ok(1), Result.Ok(2), Result.Ok(3), (x, y, z) => Result.Ok(x + y + z)));
        AssertResult.IsError(Result.Bind(Result<int>.Error(), Result.Ok(2), Result.Ok(3), (x, y, z) => Result.Ok(x + y + z)));
        AssertResult.IsError(Result.Bind(Result.Ok(1), Result<int>.Error(), Result.Ok(3), (x, y, z) => Result.Ok(x + y + z)));
        AssertResult.IsError(Result.Bind(Result.Ok(1), Result.Ok(2), Result<int>.Error(), (x, y, z) => Result.Ok(x + y + z)));
    }

    [Fact]
    public void Map2() {
        AssertResult.IsOk(3, Result.Map(Result.Ok(1), Result.Ok(2), (x, y) => x + y));
        AssertResult.IsError(Result.Map(Result<int>.Error(), Result.Ok(2), (x, y) => x + y));
        AssertResult.IsError(Result.Map(Result.Ok(1), Result<int>.Error(), (x, y) => x + y));
    }

    [Fact]
    public void Map3() {
        AssertResult.IsOk(6, Result.Map(Result.Ok(1), Result.Ok(2), Result.Ok(3), (x, y, z) => x + y + z));
        AssertResult.IsError(Result.Map(Result<int>.Error(), Result.Ok(2), Result.Ok(3), (x, y, z) => x + y + z));
        AssertResult.IsError(Result.Map(Result.Ok(1), Result<int>.Error(), Result.Ok(3), (x, y, z) => x + y + z));
        AssertResult.IsError(Result.Map(Result.Ok(1), Result.Ok(2), Result<int>.Error(), (x, y, z) => x + y + z));
    }

    [Fact]
    public void Traverse_AllOk_ReturnsOkWithAggregatedResults() {
        var items = new[] { 1, 2, 3 };
        var result = Result.Traverse(
            items,
            x => Result<int, string>.Ok(x * 2),
            (e1, e2) => $"{e1}; {e2}");

        AssertResult.IsOk(result);
        result.Match(
            ok => Assert.Equal(new[] { 2, 4, 6 }, ok),
            error => Assert.Fail("Expected Ok result but got Error."));
    }

    [Fact]
    public void Traverse_WithErrors_ReturnsError() {
        var items = new[] { 1, 2, 3 };
        var result = Result.Traverse(
            items,
            x => x % 2 == 0 ? Result<int, string>.Error($"Error {x}") : Result<int, string>.Ok(x),
            (e1, e2) => $"{e1}; {e2}");

        AssertResult.IsError(result);
        result.Match(
            ok => Assert.Fail("Expected Error result but got Ok."),
            error => Assert.Equal("Error 2", error));
    }

    [Fact]
    public void Traverse_MultipleErrors_AggregatesErrors() {
        var items = new[] { 1, 2, 3, 4 };
        var result = Result.Traverse(
            items,
            x => x % 2 == 0 ? Result<int, string>.Error($"Error {x}") : Result<int, string>.Ok(x),
            (e1, e2) => $"{e1}; {e2}");

        AssertResult.IsError(result);
        result.Match(
            ok => Assert.Fail("Expected Error result but got Ok."),
            error => Assert.Equal("Error 2; Error 4", error));
    }

    [Fact]
    public void Sequence_AllOk_ReturnsOkWithAggregatedResults() {
        var results = new[] {
            Result<int, string>.Ok(1),
            Result<int, string>.Ok(2),
            Result<int, string>.Ok(3)
        };

        var result = Result.Sequence(results, (e1, e2) => $"{e1}; {e2}");

        AssertResult.IsOk(result);
        result.Match(
            ok => Assert.Equal(new[] { 1, 2, 3 }, ok),
            error => Assert.Fail("Expected Ok result but got Error."));
    }

    [Fact]
    public void Sequence_WithErrors_ReturnsError() {
        var results = new[] {
            Result<int, string>.Ok(1),
            Result<int, string>.Error("Error 2"),
            Result<int, string>.Ok(3)
        };

        var result = Result.Sequence(results, (e1, e2) => $"{e1}; {e2}");

        AssertResult.IsError(result);
        result.Match(
            ok => Assert.Fail("Expected Error result but got Ok."),
            error => Assert.Equal("Error 2", error));
    }

    [Fact]
    public void Sequence_MultipleErrors_AggregatesErrors() {
        var results = new[] {
            Result<int, string>.Error("Error 1"),
            Result<int, string>.Ok(2),
            Result<int, string>.Error("Error 3")
        };

        var result = Result.Sequence(results, (e1, e2) => $"{e1}; {e2}");

        AssertResult.IsError(result);
        result.Match(
            ok => Assert.Fail("Expected Error result but got Ok."),
            error => Assert.Equal("Error 1; Error 3", error));
    }


    [Fact]
    public void OkUnit_ReturnsOkWithUnitValue() {
        var result = Result.Ok();
        AssertResult.IsOk(result);
        result.Match(
            ok: u => Assert.Equal(Unit.Value, u),
            error: _ => Assert.Fail("Expected Ok"));
    }

    [Fact]
    public void Error_FromResultErrors() {
        var errors = new ResultErrors("key", "error1", "error2");
        var result = Result.Error(errors);
        AssertResult.IsError(result);
    }

    [Fact]
    public void Error_FromKeyAndErrors() {
        var result = Result.Error("key", "error1", "error2");
        AssertResult.IsError(result);
    }

    [Fact]
    public void Error_FromKeyAndSingleError() {
        var result = Result.Error("key", "error");
        AssertResult.IsError(result);
    }

    [Fact]
    public void Error_FromSingleString() {
        var result = Result.Error("error");
        AssertResult.IsError(result);
    }

    [Fact]
    public void Error_FromStringArray() {
        var result = Result.Error("error1", "error2");
        AssertResult.IsError(result);
    }

    [Fact]
    public void Bind_SingleResult() {
        var result = Result<int, string>.Ok(42);
        var bound = Result.Bind(result, x => Result<int, string>.Ok(x + 1));
        AssertResult.IsOk(43, bound);

        var error = Result<int, string>.Error("err");
        var boundError = Result.Bind(error, x => Result<int, string>.Ok(x + 1));
        AssertResult.IsError(boundError);
    }

    [Fact]
    public void Map_SingleResult() {
        var result = Result<int, string>.Ok(42);
        var mapped = Result.Map(result, x => x + 1);
        AssertResult.IsOk(43, mapped);

        var error = Result<int, string>.Error("err");
        var mappedError = Result.Map(error, x => x + 1);
        AssertResult.IsError(mappedError);
    }

    [Fact]
    public void MapError_Static() {
        var ok = Result<int, string>.Ok(1);
        var mappedOk = Result.MapError(ok, e => e.ToUpperInvariant());
        AssertResult.IsOk(1, mappedOk);

        var err = Result<int, string>.Error("bad");
        var mappedErr = Result.MapError(err, e => e.ToUpperInvariant());
        AssertResult.IsError(mappedErr);
        mappedErr.Match(_ => Assert.Fail("Expected error"), e => Assert.Equal("BAD", e));
    }

    [Fact]
    public void Match_TwoResults_BothOk() {
        var result = Result.Match(
            Result<int, string>.Ok(21),
            Result<int, string>.Ok(21),
            ok: (v1, v2) => v1 + v2,
            error: _ => -1);
        Assert.Equal(42, result);
    }

    [Fact]
    public void Match_TwoResults_FirstError() {
        var result = Result.Match(
            Result<int, string>.Error("err1"),
            Result<int, string>.Ok(21),
            ok: (v1, v2) => v1 + v2,
            error: errors => errors.Count());
        Assert.Equal(1, result);
    }

    [Fact]
    public void Match_TwoResults_SecondError() {
        var result = Result.Match(
            Result<int, string>.Ok(21),
            Result<int, string>.Error("err2"),
            ok: (v1, v2) => v1 + v2,
            error: errors => errors.Count());
        Assert.Equal(1, result);
    }

    [Fact]
    public void Match_TwoResults_BothError() {
        var result = Result.Match(
            Result<int, string>.Error("err1"),
            Result<int, string>.Error("err2"),
            ok: (v1, v2) => v1 + v2,
            error: errors => errors.Count());
        Assert.Equal(2, result);
    }

    [Fact]
    public void Match_TwoResults_OkIsNull() {
        var result = Result.Match(
            Result<int, string>.Ok(21),
            Result<int, string>.Ok(21),
            ok: null!,
            error: _ => -1);
        Assert.Equal(-1, result);
    }

    [Fact]
    public void Match_ThreeResults_AllOk() {
        var result = Result.Match(
            Result<int, string>.Ok(14),
            Result<int, string>.Ok(14),
            Result<int, string>.Ok(14),
            ok: (v1, v2, v3) => v1 + v2 + v3,
            error: _ => -1);
        Assert.Equal(42, result);
    }

    [Fact]
    public void Match_ThreeResults_FirstError() {
        var result = Result.Match(
            Result<int, string>.Error("err1"),
            Result<int, string>.Ok(14),
            Result<int, string>.Ok(14),
            ok: (v1, v2, v3) => v1 + v2 + v3,
            error: errors => errors.Count());
        Assert.Equal(1, result);
    }

    [Fact]
    public void Match_ThreeResults_SecondError() {
        var result = Result.Match(
            Result<int, string>.Ok(14),
            Result<int, string>.Error("err2"),
            Result<int, string>.Ok(14),
            ok: (v1, v2, v3) => v1 + v2 + v3,
            error: errors => errors.Count());
        Assert.Equal(1, result);
    }

    [Fact]
    public void Match_ThreeResults_ThirdError() {
        var result = Result.Match(
            Result<int, string>.Ok(14),
            Result<int, string>.Ok(14),
            Result<int, string>.Error("err3"),
            ok: (v1, v2, v3) => v1 + v2 + v3,
            error: errors => errors.Count());
        Assert.Equal(1, result);
    }

    [Fact]
    public void Match_ThreeResults_MultipleErrors() {
        var result = Result.Match(
            Result<int, string>.Error("err1"),
            Result<int, string>.Error("err2"),
            Result<int, string>.Ok(14),
            ok: (v1, v2, v3) => v1 + v2 + v3,
            error: errors => errors.Count());
        Assert.Equal(2, result);
    }

    [Fact]
    public void Match_ThreeResults_AllErrors() {
        var result = Result.Match(
            Result<int, string>.Error("err1"),
            Result<int, string>.Error("err2"),
            Result<int, string>.Error("err3"),
            ok: (v1, v2, v3) => v1 + v2 + v3,
            error: errors => errors.Count());
        Assert.Equal(3, result);
    }

    [Fact]
    public void Match_ThreeResults_OkIsNull() {
        var result = Result.Match(
            Result<int, string>.Ok(14),
            Result<int, string>.Ok(14),
            Result<int, string>.Ok(14),
            ok: null!,
            error: _ => -1);
        Assert.Equal(-1, result);
    }

    [Fact]
    public void Traverse_WithResultErrors() {
        var items = new[] { 1, 2, 3 };
        var result = Result.Traverse(
            items,
            x => x % 2 == 0
                ? Result<int>.Error($"Error {x}")
                : Result<int>.Ok(x));

        AssertResult.IsError(result);
    }

    [Fact]
    public void Sequence_WithResultErrors() {
        var results = new[] {
        Result<int>.Ok(1),
        Result<int>.Error("Error 2"),
        Result<int>.Ok(3)
    };

        var result = Result.Sequence(results);
        AssertResult.IsError(result);
    }

    [Fact]
    public void Zip_TwoResults_BothOk() {
        var result = Result.Zip(
            Result<int, string>.Ok(42),
            Result<string, string>.Ok("Hello"));

        AssertResult.IsOk(result);
        result.Match(
            ok: tuple => {
                Assert.Equal(42, tuple.Item1);
                Assert.Equal("Hello", tuple.Item2);
            },
            error: _ => Assert.Fail("Expected Ok"));
    }

    [Fact]
    public void Zip_TwoResults_FirstError() {
        var result = Result.Zip(
            Result<int, string>.Error("err"),
            Result<string, string>.Ok("Hello"));

        AssertResult.IsError(result);
    }

    [Fact]
    public void Zip_TwoResults_SecondError() {
        var result = Result.Zip(
            Result<int, string>.Ok(42),
            Result<string, string>.Error("err"));

        AssertResult.IsError(result);
    }

    [Fact]
    public void Zip_ThreeResults_AllOk() {
        var result = Result.Zip(
            Result<int, string>.Ok(1),
            Result<int, string>.Ok(2),
            Result<int, string>.Ok(3));

        AssertResult.IsOk(result);
        result.Match(
            ok: tuple => {
                Assert.Equal(1, tuple.Item1);
                Assert.Equal(2, tuple.Item2);
                Assert.Equal(3, tuple.Item3);
            },
            error: _ => Assert.Fail("Expected Ok"));
    }

    [Fact]
    public void Zip_ThreeResults_WithError() {
        var result = Result.Zip(
            Result<int, string>.Ok(1),
            Result<int, string>.Error("err"),
            Result<int, string>.Ok(3));

        AssertResult.IsError(result);
    }
}
