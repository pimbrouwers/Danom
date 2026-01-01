using Danom;
using Danom.MinimalApi;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var someOption = Option.Some("Hello world");
var noneOption = Option<string>.NoneValue;
var okResult = Result.Ok("Success!");
var errorResult = Result<string>.Error(["An error occurred."]);
var stringErrorResult = Result<string, string>.Error("An error occurred.");

// Result Extensions
app.MapGet("/option/some", () =>
    Results.Extensions.Option(someOption));

app.MapGet("/option/none", () =>
    Results.Extensions.Option(noneOption));

app.MapGet("/option/none/custom", () =>
    Results.Extensions.Option(noneOption,
        noneResult: () => Results.NotFound("Custom Not Found!")));

app.MapGet("/result/ok", () =>
    Results.Extensions.Result(okResult));

app.MapGet("/result/error", () =>
    Results.Extensions.Result(errorResult));

app.MapGet("/result/error/custom", () =>
    Results.Extensions.Result(stringErrorResult,
        errorResult: error => Results.Ok(new { Message = "There was a problem", error })));

app.MapGet("/result/error/problem", () =>
    Results.Extensions.ResultProblem(errorResult));

app.MapGet("/result/error/custom/problem", () =>
    Results.Extensions.ResultProblem(stringErrorResult));

// Typed Results
app.MapGet("/typed/option/some", () =>
    DanomTypedResults.Option(someOption));

app.MapGet("/typed/option/none", () =>
    DanomTypedResults.Option(noneOption));

app.MapGet("/typed/option/none/custom", () =>
    DanomTypedResults.Option(noneOption,
        noneResult: () => Results.NotFound("Custom Not Found!")));

app.MapGet("/typed/result/ok", () =>
    DanomTypedResults.Result(okResult));

app.MapGet("/typed/result/error", () =>
    DanomTypedResults.Result(errorResult));

app.MapGet("/typed/result/error/custom", () =>
    DanomTypedResults.Result(stringErrorResult,
        errorResult: error => Results.Ok(new { Message = "There was a problem", error })));

app.MapGet("/typed/result/error/problem", () =>
    DanomTypedResults.ResultProblem(errorResult));

app.MapGet("/typed/result/error/custom/problem", () =>
    DanomTypedResults.ResultProblem(stringErrorResult));

app.Run();

public partial class Program { }
