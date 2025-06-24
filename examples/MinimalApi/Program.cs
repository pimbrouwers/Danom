using Danom;
using Danom.MinimalApi;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var _someOption = Option.Some("Hello world");
var _noneOption = Option<string>.NoneValue;

app.MapGet("/option/some", () =>
    Results.Extensions.Option(_someOption));

app.MapGet("/option/none", () =>
    Results.Extensions.Option(_noneOption,
        noneResult: () => Results.NotFound("Custom Not Found!")));

app.MapGet("/", () =>
    new { urls = "/option/some, /option/none" });

app.Run();
