namespace Danom.Tests;

using Xunit;

public sealed class ResultErrorTests {
    [Fact]
    public void CanCreateFromString() {
        Assert.Equal("Error", new ResultError("Error").ToString());
    }

    [Fact]
    public void CanCreateFromStringWithKey() {
        Assert.Equal("Key - Error", new ResultError("Key", "Error").ToString());
    }

    [Fact]
    public void CanCreateFromStrings() {
        Assert.Equal("Error1, Error2", new ResultError(["Error1", "Error2"]).ToString());
    }

    [Fact]
    public void CanCreateFromStringsWithKey() {
        Assert.Equal("Key - Error1, Error2", new ResultError("Key", ["Error1", "Error2"]).ToString());
    }

    [Fact]
    public void CanAddResultError() {
        var error = new ResultError("Error");
        error.Add(new ResultError("Error2"));
        Assert.Equal("Error, Error2", error.ToString());
    }

    [Fact]
    public void CanAddString() {
        var error = new ResultError("Error");
        error.Add("Error2");
        Assert.Equal("Error, Error2", error.ToString());
    }

    [Fact]
    public void CanAddStringWithKey() {
        var error = new ResultError("Key", "Error");
        error.Add("Key", "Error2");
        Assert.Equal("Key - Error, Error2", error.ToString());
    }

    [Fact]
    public void CanAddStrings() {
        var error = new ResultError(["Error1"]);
        error.Add(["Error2"]);
        Assert.Equal("Error1, Error2", error.ToString());
    }

    [Fact]
    public void CanAddStringsWithKey() {
        var error = new ResultError("Key", ["Error1"]);
        error.Add("Key", ["Error2"]);
        Assert.Equal("Key - Error1, Error2", error.ToString());
    }

    [Fact]
    public void AddResultErrorWithMismatchedKeyThrows() {
        var error = new ResultError("Key1", "Error1");
        var mismatchedError = new ResultError("Key2", "Error2");

        Assert.Throws<InvalidOperationException>(() => error.Add(mismatchedError));
    }

    [Fact]
    public void AddNullResultErrorThrows() {
        var error = new ResultError("Key", "Error1");

        Assert.Throws<ArgumentNullException>(() => error.Add((ResultError)null!));
    }

    [Fact]
    public void ToStringForEmptyResultError() {
        var error = new ResultError(string.Empty, []);

        Assert.Equal(string.Empty, error.ToString());
    }

    [Fact]
    public void AddDuplicateErrors() {
        var error = new ResultError("Key", "Error1");
        error.Add("Error1");

        Assert.Equal("Key - Error1, Error1", error.ToString());
    }

    [Fact]
    public void AddCaseInsensitiveKeys() {
        var resultErrors = new ResultErrors();
        resultErrors.Add("KEY", "Error1");
        resultErrors.Add("key", "Error2");

        Assert.Single(resultErrors);
        Assert.Equal("[ KEY - Error1, Error2 ]", resultErrors.ToString());
    }
}

public sealed class ResultErrorsTests {
    [Fact]
    public void CanEnumerate() {
        var resultErrors = new ResultErrors(["Error1", "Error2"]);
        Assert.Single(resultErrors);
        Assert.Equal($"[ Error1, Error2 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanCreateFromStringsWithKey() {
        var resultErrors = new ResultErrors("Key", ["Error1", "Error2"]);
        Assert.Single(resultErrors);
        Assert.Equal("[ Key - Error1, Error2 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanCreateFromParamStringsWithKey() {
        var resultErrors = new ResultErrors("Key", "Error1", "Error2", "Error3");
        Assert.Single(resultErrors);
        Assert.Equal("[ Key - Error1, Error2, Error3 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanCreateFromStringWithKey() {
        var resultErrors = new ResultErrors("Key", "Error1");
        Assert.Single(resultErrors);
        Assert.Equal("[ Key - Error1 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanCreateFromStrings() {
        var resultErrors = new ResultErrors(["Error1", "Error2"]);
        Assert.Single(resultErrors);
        Assert.Equal($"[ Error1, Error2 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanCreateFromString() {
        var resultErrors = new ResultErrors("Error");
        Assert.Single(resultErrors);
        Assert.Equal($"[ Error ]", resultErrors.ToString());
    }

    [Fact]
    public void CanAddErrorsWithKey() {
        var resultErrors = new ResultErrors();
        resultErrors.Add("Key", "Error1");
        resultErrors.Add("Key", "Error2");
        Assert.Single(resultErrors);
        Assert.Equal("[ Key - Error1, Error2 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanAddErrorsWithoutKey() {
        var resultErrors = new ResultErrors();
        resultErrors.Add("Error1");
        resultErrors.Add("Error2");
        Assert.Single(resultErrors);
        Assert.Equal("[ Error1, Error2 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanAddMultipleErrorsWithoutKey() {
        var resultErrors = new ResultErrors();
        resultErrors.Add(["Error1", "Error2"]);
        Assert.Single(resultErrors);
        Assert.Equal("[ Error1, Error2 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanAddResultError() {
        var resultErrors = new ResultErrors();
        resultErrors.Add(new ResultError("Error1"));
        Assert.Single(resultErrors);
        Assert.Equal("[ Error1 ]", resultErrors.ToString());

        Assert.Throws<ArgumentNullException>(() => resultErrors.Add((ResultError)null!));
    }

    [Fact]
    public void CanAddErrorsWithEmptyKey() {
        var resultErrors = new ResultErrors();
        resultErrors.Add(string.Empty, "Error1");
        resultErrors.Add(string.Empty, "Error2");
        Assert.Single(resultErrors);
        Assert.Equal("[ Error1, Error2 ]", resultErrors.ToString());
    }

    [Fact]
    public void MergeCombinesMultipleResultErrors() {
        var errors1 = new ResultErrors("Key1", "Error1");
        var errors2 = new ResultErrors("Key2", "Error2");
        var merged = ResultErrors.Merge(errors1, errors2);

        Assert.Equal(2, merged.Count());
        Assert.Equal("[ Key1 - Error1; Key2 - Error2 ]", merged.ToString());
    }

    [Fact]
    public void MergeWithEmptyResultErrors() {
        var errors1 = new ResultErrors("Key1", "Error1");
        var empty = ResultErrors.Empty;
        var merged = ResultErrors.Merge(errors1, empty);

        Assert.Single(merged);
        Assert.Equal("[ Key1 - Error1 ]", merged.ToString());
    }

    [Fact]
    public void MergeWithOverlappingKeys() {
        var errors1 = new ResultErrors("Key", "Error1");
        var errors2 = new ResultErrors("Key", "Error2");
        var merged = ResultErrors.Merge(errors1, errors2);

        Assert.Single(merged);
        Assert.Equal("[ Key - Error1, Error2 ]", merged.ToString());
    }

    [Fact]
    public void IndexerReturnsResultErrorForExistingKey() {
        var resultErrors = new ResultErrors("Key", "Error1");
        var error = resultErrors["Key"];

        Assert.Equal("Key - Error1", error.ToString());
    }

    [Fact]
    public void IndexerThrowsForNonExistentKey() {
        var resultErrors = new ResultErrors("Key", "Error1");

        Assert.Throws<KeyNotFoundException>(() => resultErrors["NonExistentKey"]);
    }

    [Fact]
    public void AddResultErrorsWithOverlappingKeys() {
        var resultErrors = new ResultErrors("Key", "Error1");
        var additionalErrors = new ResultErrors("Key", "Error2");

        resultErrors.Add(additionalErrors);

        Assert.Single(resultErrors);
        Assert.Equal("[ Key - Error1, Error2 ]", resultErrors.ToString());
    }

    [Fact]
    public void ToStringForEmptyResultErrors() {
        var resultErrors = ResultErrors.Empty;

        Assert.Equal("[]", resultErrors.ToString());
    }
}
