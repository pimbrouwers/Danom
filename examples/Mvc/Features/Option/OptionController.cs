namespace Danom.Examples.Mvc;

using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

public sealed class OptionController
    : DanomController {
    private readonly Option<string> _someOption = Option.Some("Hello world");
    private readonly Option<string> _noneOption = Option<string>.None();

    public IActionResult Some() =>
        ViewOption(
            option: _someOption,
            viewName: "Detail");

    // Returns the ASP.NET default `NotFound` result
    public IActionResult None() =>
        ViewOption(
            option: _noneOption,
            viewName: "Detail");

    public IActionResult NoneCustom() =>
        ViewOption(
            option: _noneOption,
            viewName: "Detail",
            noneAction: () => NotFound("Not found!"));
}
