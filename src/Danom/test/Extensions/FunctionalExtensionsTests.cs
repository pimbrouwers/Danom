namespace Danom.Tests;

using Xunit;

public class FunctionalExtensionsTests {
    [Fact]
    public void Pipe_WithSimpleFunction_ReturnsExpectedResult() {
        int input = 5;
        int result = input.Pipe(x => x * 2);
        Assert.Equal(10, result);
    }

    [Fact]
    public void Pipe_WithStringTransformation_ReturnsExpectedResult() {
        string input = "hello";
        string result = input.Pipe(x => x.ToUpper());
        Assert.Equal("HELLO", result);
    }

    [Fact]
    public void Pipe_WithNullFunction_ThrowsArgumentNullException() {
        int input = 5;
        Assert.Throws<ArgumentNullException>(() => input.Pipe((Func<int, int>)null!));
    }

    [Fact]
    public void Pipe_WithComplexObject_ReturnsTransformedObject() {
        var input = new { Name = "John", Age = 30 };
        string result = input.Pipe(x => $"{x.Name} is {x.Age} years old");
        Assert.Equal("John is 30 years old", result);
    }

    [Fact]
    public void Pipe_WithChainedCalls_ReturnsExpectedResult() {
        int input = 5;
        string result = input
            .Pipe(x => x * 2)
            .Pipe(x => x.ToString())
            .Pipe(x => $"Result: {x}");
        Assert.Equal("Result: 10", result);
    }

    [Fact]
    public void Pipe_WithSideEffects_ExecutesSideEffects() {
        int input = 5;
        int sideEffect = 0;
        int result = input.Pipe(x => {
            sideEffect = x * 2;
            return sideEffect;
        });
        Assert.Equal(10, result);
        Assert.Equal(10, sideEffect);
    }
}
