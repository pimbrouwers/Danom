namespace Danom.Tests;

using Danom.TestHelpers;
using Xunit;

public sealed class ResultErrorTests
{
    [Fact]
    public void CanCreateFromString()
    {
        Assert.Equal("Error", new ResultError("Error").ToString());
    }

    [Fact]
    public void CanCreateFromStringWithKey()
    {
        Assert.Equal("Key - Error", new ResultError("Key", "Error").ToString());
    }

    [Fact]
    public void CanCreateFromStrings()
    {
        Assert.Equal("Error1, Error2", new ResultError(["Error1", "Error2"]).ToString());
    }

    [Fact]
    public void CanCreateFromStringsWithKey()
    {
        Assert.Equal("Key - Error1, Error2", new ResultError("Key", ["Error1", "Error2"]).ToString());
    }

    [Fact]
    public void CanAddResultError()
    {
        var error = new ResultError("Error");
        error.Add(new ResultError("Error2"));
        Assert.Equal("Error, Error2", error.ToString());
    }

    [Fact]
    public void CanAddString()
    {
        var error = new ResultError("Error");
        error.Add("Error2");
        Assert.Equal("Error, Error2", error.ToString());
    }

    [Fact]
    public void CanAddStringWithKey()
    {
        var error = new ResultError("Key", "Error");
        error.Add("Key", "Error2");
        Assert.Equal("Key - Error, Error2", error.ToString());
    }

    [Fact]
    public void CanAddStrings()
    {
        var error = new ResultError(["Error1"]);
        error.Add(["Error2"]);
        Assert.Equal("Error1, Error2", error.ToString());
    }

    [Fact]
    public void CanAddStringsWithKey()
    {
        var error = new ResultError("Key", ["Error1"]);
        error.Add("Key", ["Error2"]);
        Assert.Equal("Key - Error1, Error2", error.ToString());
    }

}

public sealed class ResultErrorsTests
{
    [Fact]
    public void CanEnumerate()
    {
        var resultErrors = new ResultErrors(["Error1", "Error2"]);
        Assert.Single(resultErrors);
        Assert.Equal($"[ Error1, Error2 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanCreateFromStringsWithKey()
    {
        var resultErrors = new ResultErrors("Key", ["Error1", "Error2"]);
        Assert.Single(resultErrors);
        Assert.Equal("[ Key - Error1, Error2 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanCreateFromStringWithKey()
    {
        var resultErrors = new ResultErrors("Key", "Error1");
        Assert.Single(resultErrors);
        Assert.Equal("[ Key - Error1 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanCreateFromStrings()
    {
        var resultErrors = new ResultErrors(["Error1", "Error2"]);
        Assert.Single(resultErrors);
        Assert.Equal($"[ Error1, Error2 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanCreateFromString()
    {
        var resultErrors = new ResultErrors("Error");
        Assert.Single(resultErrors);
        Assert.Equal($"[ Error ]", resultErrors.ToString());
    }

    [Fact]
    public void CanAddErrorsWithKey()
    {
        var resultErrors = new ResultErrors();
        resultErrors.Add("Key", "Error1");
        resultErrors.Add("Key", "Error2");
        Assert.Single(resultErrors);
        Assert.Equal("[ Key - Error1, Error2 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanAddErrorsWithoutKey()
    {
        var resultErrors = new ResultErrors();
        resultErrors.Add("Error1");
        resultErrors.Add("Error2");
        Assert.Single(resultErrors);
        Assert.Equal("[ Error1, Error2 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanAddErrorsWithEmptyKey()
    {
        var resultErrors = new ResultErrors();
        resultErrors.Add(string.Empty, "Error1");
        resultErrors.Add(string.Empty, "Error2");
        Assert.Single(resultErrors);
        Assert.Equal("[ Error1, Error2 ]", resultErrors.ToString());
    }
}
