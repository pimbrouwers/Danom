using Danom;
using Danom.MinimalApi;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var someOption = Option.Some("Hello world");
var noneOption = Option<string>.NoneValue;

app.MapGet("/option/some", () =>
    Results.Extensions.Option(someOption));

app.MapGet("/option/none", () =>
    Results.Extensions.Option(noneOption,
        noneResult: () => Results.NotFound("Custom Not Found!")));

var okResult = Result.Ok("Success!");
var errorResult = Result<string>.Error(["An error occurred."]);
var stringErrorResult = Result<string, string>.Error("An error occurred.");

app.MapGet("/result/ok", () =>
    Results.Extensions.Result(okResult));

app.MapGet("/result/error", () =>
    Results.Extensions.Result(errorResult));

app.MapGet("/result/error/custom", () =>
    Results.Extensions.Result(stringErrorResult,
        errorResult: errors => Results.Ok(new { Message = "There was a problem", errors })));

app.MapGet("/", () =>
    new[]
    {
        "/option/some",
        "/option/none",
        "/result/ok",
        "/result/error",
        "/result/error/custom"
    });

app.Run();
