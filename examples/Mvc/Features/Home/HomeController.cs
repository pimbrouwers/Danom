namespace Danom.Examples.Mvc;

using Microsoft.AspNetCore.Mvc;

public sealed class HomeController : Controller {
    public IActionResult Index() =>
        View();
}
