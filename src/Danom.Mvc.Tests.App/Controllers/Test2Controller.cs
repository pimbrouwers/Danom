namespace Danom.Mvc.Tests.App;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Test inherited Danom controller
/// </summary>
public sealed class Test2Controller : DanomController {
    private readonly Option<string> _someOption = Option.Some("Hello world");
    private readonly Option<string> _noneOption = Option<string>.NoneValue;
    private readonly Result<string, ResultErrors> _okResult = Result.Ok("Success!");
    private readonly Result<string, ResultErrors> _errorResult = Result<string>.Error(["An error occurred."]);
    private readonly Result<string, string> _stringErrorResult = Result<string, string>.Error("Custom error occurred!");

    [HttpGet("/inherited/")]
    public IActionResult Index() =>
        // test that default view name works
        ViewOption(Option.Some("/"));

    [HttpGet("/inherited/option/some")]
    public IActionResult GetOptionSome() =>
        ViewOption(_someOption, viewName: "Detail");

    [HttpGet("/inherited/option/none")]
    public IActionResult GetOptionNone() =>
        ViewOption(_noneOption, viewName: "Detail");

    [HttpGet("/inherited/option/none/custom")]
    public IActionResult GetOptionNoneCustom() =>
        ViewOption(
            _noneOption,
            viewName: "Detail",
            noneAction: () => this.NotFound("Custom Not Found!"));

    [HttpGet("/inherited/result/ok")]
    public IActionResult GetResultOk() =>
        ViewResult(
            _okResult,
            viewName: "Detail");

    [HttpGet("/inherited/result/error")]
    public IActionResult GetResultError() =>
        ViewResult(
            _errorResult,
            viewName: "Detail");

    [HttpGet("/inherited/result/error/custom")]
    public IActionResult GetResultErrorCustom() =>
        ViewResult(
            _stringErrorResult,
            errorAction: this.BadRequest,
            viewName: "Detail");


}
