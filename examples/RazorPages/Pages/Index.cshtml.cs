namespace Danom.Examples.RazorPages;

using Danom;
using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

public sealed class IndexModel : DanomPageModel
{
    public IActionResult OnGet()
    {
        var resultWithErrors = Result<string>.Error(new("An error occurred."));
        if(resultWithErrors.TryGetError(out var e))
        {
            return Page(e);
        }

        return Page();
    }
}
