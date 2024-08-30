namespace Danom.Samples.Mvc;

using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

public sealed class HomeController : DanomController
{
    public IActionResult Index() =>
        View();

    public IActionResult OptionSome() =>
        ViewOption(
            option: Option.Some("Hello, World!"),
            viewName: "Detail");

    public IActionResult OptionNone() =>
        ViewOption(
            option: Option<string>.None(),
            viewName: "Detail",
            noneAction: () => NotFound("Not found!"));

    public IActionResult ResultOk() =>
        ViewResult(
            result: Result.Ok("Success!"),
            viewName: "Detail");

    public IActionResult ResultError() =>
        ViewResult(
            result: Result<string>.Error(new ResultErrors("An error occurred.")),
            errorAction: errors =>
                ViewResultErrors(
                    errors: errors,
                    viewName: "Detail",
                    model: errors),
            viewName: "Detail");

    public IActionResult ResultErrors() =>
        ViewResult(
            result: Result<string>.Error("An error occurred."),
            viewName: "Detail");

    public IActionResult ResultOptionOk() =>
        ViewResultOption(
            resultOption: ResultOption.Ok("Hello, World!"),
            viewName: "Detail");

    public IActionResult ResultOptionError() =>
        ViewResultOption(
            resultOption: ResultOption<string>.Error(new ResultErrors("An error occurred.")),
            errorAction: errors =>
                ViewResultErrors(
                    errors: errors,
                    viewName: "Detail",
                    model: errors),
            viewName: "Detail");

    public IActionResult ResultOptionErrors() =>
        ViewResultOption(
            resultOption: ResultOption<string>.Error("An error occurred."),
            viewName: "Detail");

}
