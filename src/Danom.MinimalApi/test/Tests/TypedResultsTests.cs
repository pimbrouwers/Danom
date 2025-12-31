namespace Danom.MinimalApi.Tests.TypedResults;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Xunit;

public static class TypedOptionTests {
    [Fact]
    public static async Task Some_NoConversion_200WithJson() {
        var value = new SomeType(123);
        var option = Option.Some(new SomeType(123));

        var httpResult = DanomHttpResults.Option(option);
        Assert.IsType<OptionHttpResult<SomeType>>(httpResult);

        var response = await httpResult.ToResponse();
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal(value, response.DeserializeBody<SomeType>());
    }

    [Fact]
    public static async Task Some_Conversion_200WithJson() {
        var value = new SomeType(123);
        var option = Option.Some(new SomeType(123));

        var httpResult = DanomHttpResults.Option(option, TypedResults.NotFound);
        Assert.IsType<OptionHttpResult<SomeType, NotFound>>(httpResult);

        var response = await httpResult.ToResponse();
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal(value, response.DeserializeBody<SomeType>());
    }

    [Fact]
    public static async Task None_NoConversion_404() {
        var option = Option<SomeType>.NoneValue;

        var httpResult = DanomHttpResults.Option(option);
        Assert.IsType<OptionHttpResult<SomeType>>(httpResult);

        var response = await httpResult.ToResponse();
        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
    }

    [Fact]
    public static async Task None_WithConversion_UsesConversion() {
        var option = Option<SomeType>.NoneValue;

        var httpResult = DanomHttpResults.Option(option, TypedResults.Conflict);
        Assert.IsType<OptionHttpResult<SomeType, Conflict>>(httpResult);

        var response = await httpResult.ToResponse();
        Assert.Equal(StatusCodes.Status409Conflict, response.StatusCode);
    }
}

public static class TypedResultTests {
    [Fact]
    public static async Task Ok_NoConversion_200WithJson() {
        var value = new SomeType(123);
        var result = Result<SomeType>.Ok(new SomeType(123));

        var httpResult = DanomHttpResults.Result(result);
        Assert.IsType<ResultHttpResult<SomeType, ResultErrors>>(httpResult);

        var response = await httpResult.ToResponse();
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal(value, response.DeserializeBody<SomeType>());
    }

    [Fact]
    public static async Task Ok_Conversion_200WithJson() {
        var value = new SomeType(123);
        var result = Result<SomeType>.Ok(new SomeType(123));

        var httpResult = DanomHttpResults.Result(result, _ => throw new UnreachableException());
        Assert.IsType<ResultHttpResult<SomeType, ResultErrors>>(httpResult);

        var response = await httpResult.ToResponse();
        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        Assert.Equal(value, response.DeserializeBody<SomeType>());
    }

    [Fact]
    public static async Task Error_NoConversion_400WithJson() {
        var error = new SomeType(123);
        var result = Result<int, SomeType>.Error(error);

        var httpResult = DanomHttpResults.Result(result);
        Assert.IsType<ResultHttpResult<int, SomeType>>(httpResult);

        var response = await httpResult.ToResponse();
        Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode);
        Assert.Equal(error, response.DeserializeBody<SomeType>());
    }

    [Fact]
    public static async Task Error_Conversion_UsesConversion() {
        var error = new SomeType(123);
        var result = Result<int, SomeType>.Error(error);

        var httpResult = DanomHttpResults.Result(result, TypedResults.NotFound);
        Assert.IsType<ResultHttpResult<int, SomeType, NotFound<SomeType>>>(httpResult);

        var response = await httpResult.ToResponse();
        Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        Assert.Equal(error, response.DeserializeBody<SomeType>());
    }
}

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
sealed record SomeType(int Value);
