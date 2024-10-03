namespace Danom.Samples.Mvc;

using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

public sealed class ResultOptionController
    : DanomController
{
    private readonly ResultOption<string, ResultErrors> _okResult = ResultOption.Ok("Success!");
    private readonly ResultOption<string, ResultErrors> _errorResult = ResultOption<string>.Error("An error occurred.");
    private readonly ResultOption<string, string> _stringErrorResult = ResultOption<string, string>.Error("An error occurred.");

    public IActionResult Ok() =>
        ViewResultOption(
            resultOption: _okResult,
            viewName: "Detail");

    public IActionResult Error() =>
        ViewResultOption(
            resultOption: _errorResult,
            viewName: "Detail");

    // Demonstrating error customization, using a string literal for error output
    public IActionResult ErrorCustom() =>
        ViewResultOption(
            resultOption: _stringErrorResult,
            errorAction: errors => View("Detail", errors),
            viewName: "Detail");
}
