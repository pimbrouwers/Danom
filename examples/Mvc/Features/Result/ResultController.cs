namespace Danom.Examples.Mvc;

using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

public sealed class ResultController
    : DanomController {
    private readonly Result<string, ResultErrors> _okResult = Result.Ok("Success!");
    private readonly Result<string, ResultErrors> _errorResult = Result<string>.Error(["An error occurred."]);
    private readonly Result<string, string> _stringErrorResult = Result<string, string>.Error("An error occurred.");

    public IActionResult Ok() =>
        ViewResult(
            result: _okResult,
            viewName: "Detail");

    public IActionResult Error() =>
        ViewResult(
            result: _errorResult,
            viewName: "Detail");

    // Demonstrating error customization, using a string literal for error output
    public IActionResult ErrorCustom() =>
        ViewResult(
            result: _stringErrorResult,
            errorAction: errors => View("Detail", errors),
            viewName: "Detail");

}
