namespace Danom.Mvc;

using Microsoft.AspNetCore.Mvc.ModelBinding;

internal static class ModelStateDictionaryExtensions
{
    internal static void AddResultErrors(
        this ModelStateDictionary modelState,
        ResultErrors errors)
    {
        foreach (var error in errors)
        {
            foreach (var err in error.Errors)
            {
                modelState.AddModelError(error.Key, err);
            }
        }
    }
}
