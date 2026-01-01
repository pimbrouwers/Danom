namespace Danom.MinimalApi.Tests;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Xunit;

public static class DanomResultExtensionsTests {
    public static class OptionTests {
        [Fact]
        public static async Task Some_NoConversion_200WithJson() {
            var value = new SomeType(123);
            var option = Option.Some(new SomeType(123));

            var httpResult = Results.Extensions.Option(option);

            var response = await httpResult.ToResponse();
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.Equal(value, response.DeserializeBody<SomeType>());
        }

        [Fact]
        public static async Task Some_Conversion_200WithJson() {
            var value = new SomeType(123);
            var option = Option.Some(new SomeType(123));

            var httpResult = Results.Extensions.Option(option, () => throw new UnreachableException());

            var response = await httpResult.ToResponse();
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.Equal(value, response.DeserializeBody<SomeType>());
        }

        [Fact]
        public static async Task None_NoConversion_404() {
            var option = Option<Unit>.NoneValue;

            var httpResult = Results.Extensions.Option(option);

            var response = await httpResult.ToResponse();
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public static async Task None_WithConversion_UsesConversion() {
            var option = Option<Unit>.NoneValue;

            var httpResult = Results.Extensions.Option(option, TypedResults.Conflict);

            var response = await httpResult.ToResponse();
            Assert.Equal(StatusCodes.Status409Conflict, response.StatusCode);
        }
    }

    public static class ResultTests {
        [Fact]
        public static async Task Ok_NoConversion_200WithJson() {
            var value = new SomeType(123);
            var result = Result<SomeType>.Ok(new SomeType(123));

            var httpResult = Results.Extensions.Result(result);

            var response = await httpResult.ToResponse();
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.Equal(value, response.DeserializeBody<SomeType>());
        }

        [Fact]
        public static async Task Ok_Conversion_200WithJson() {
            var value = new SomeType(123);
            var result = Result<SomeType>.Ok(new SomeType(123));

            var httpResult = Results.Extensions.Result(result, IResult (_) => throw new UnreachableException());

            var response = await httpResult.ToResponse();
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
            Assert.Equal(value, response.DeserializeBody<SomeType>());
        }

        [Fact]
        public static async Task Error_NoConversion_400WithJson() {
            var error = new SomeType(123);
            var result = Result<int, SomeType>.Error(error);

            var httpResult = Results.Extensions.Result(result);

            var response = await httpResult.ToResponse();
            Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode);
            Assert.Equal(error, response.DeserializeBody<SomeType>());
        }

        [Fact]
        public static async Task Error_Conversion_UsesConversion() {
            var error = new SomeType(123);
            var result = Result<int, SomeType>.Error(error);

            var httpResult = Results.Extensions.Result(result, TypedResults.NotFound);

            var response = await httpResult.ToResponse();
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            Assert.Equal(error, response.DeserializeBody<SomeType>());
        }
    }

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
    private sealed record SomeType(int Value);
}
