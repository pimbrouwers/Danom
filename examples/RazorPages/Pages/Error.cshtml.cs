namespace Danom.Examples.RazorPages;

using Danom;
using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

public sealed class ErrorModel : DanomPageModel {
    public IActionResult OnGet() {
        var resultWithErrors = Result<int>.Error("Error Key", "An error occurred.");
        if (resultWithErrors.TryGetError(out var e)) {
            return Page(e);
        }

        return Page();
    }
}
