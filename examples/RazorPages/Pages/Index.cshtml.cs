namespace Danom.Examples.RazorPages;

using Danom;
using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

public sealed class IndexModel : DanomPageModel {
    public IActionResult OnGet() {
        return Page();
    }
}
