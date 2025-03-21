namespace Danom.Mvc;

using Microsoft.AspNetCore.Mvc.RazorPages;

/// <summary>
/// A page model that provides helper methods for working with Danom's monad
/// types.
/// </summary>
public class DanomPageModel : PageModel
{
    /// <summary>
    /// Returns a page result with the specified errors added to the model
    /// state.
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public PageResult Page(ResultErrors errors)
    {
        ModelState.AddResultErrors(errors);
        return Page();
    }
}
