namespace Danom.Tests;

using Xunit;
using Danom.TestHelpers;

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
}

public sealed class ResultErrorsTests
{
    [Fact]
    public void CanCreateAndEnumerate()
    {
        var resultErrors = new ResultErrors([
            new ResultError("Error1"),
            new ResultError("Error2")]);

        Assert.NotEmpty(resultErrors);
        Assert.Equal(2, resultErrors.Count());
        Assert.Equal($"[ Error1, Error2 ]", resultErrors.ToString());
    }

    [Fact]
    public void CanAdd()
    {
        var resultErrors = new ResultErrors();
        resultErrors.Add(new("Error"));
        Assert.Single(resultErrors);
        Assert.Equal($"[ Error ]", resultErrors.ToString());
    }

    [Fact]
    public void CanCreateFromString()
    {
        var resultErrors = new ResultErrors("Error");
        Assert.Single(resultErrors);
        Assert.Equal($"[ Error ]", resultErrors.ToString());
    }
}
