using Microsoft.AspNetCore.Mvc;

namespace Danom.Examples.Todo;

public sealed class HomeController : Controller {
    [HttpGet]
    public IActionResult Index() =>
        View();
}
