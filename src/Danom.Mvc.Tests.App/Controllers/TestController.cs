namespace Danom.Mvc.Tests.App;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Test Danom controller extension methods
/// </summary>
public sealed class TestController : Controller {
    private readonly Option<string> _someOption = Option.Some("Hello world");
    private readonly Option<string> _noneOption = Option<string>.NoneValue;
    private readonly Result<string, ResultErrors> _okResult = Result.Ok("Success!");
    private readonly Result<string, ResultErrors> _errorResult = Result<string>.Error(["An error occurred."]);
    private readonly Result<string, string> _stringErrorResult = Result<string, string>.Error("Custom error occurred!");

    [HttpGet("/")]
    public IActionResult Index() =>
        // test that default view name works
        this.ViewOption(Option.Some("/"));

    [HttpGet("/option/some")]
    public IActionResult GetOptionSome() =>
        this.ViewOption(_someOption, viewName: "Detail");

    [HttpGet("/option/none")]
    public IActionResult GetOptionNone() =>
        this.ViewOption(_noneOption, viewName: "Detail");

    [HttpGet("/option/none/custom")]
    public IActionResult GetOptionNoneCustom() =>
        this.ViewOption(
            _noneOption,
            viewName: "Detail",
            noneAction: () => this.NotFound("Custom Not Found!"));

    [HttpGet("/result/ok")]
    public IActionResult GetResultOk() =>
        this.ViewResult(
            _okResult,
            viewName: "Detail");

    [HttpGet("/result/error")]
    public IActionResult GetResultError() =>
        this.ViewResult(
            _errorResult,
            viewName: "Detail");

    [HttpGet("/result/error/custom")]
    public IActionResult GetResultErrorCustom() =>
        this.ViewResult(
            _stringErrorResult,
            errorAction: this.BadRequest,
            viewName: "Detail");


}
