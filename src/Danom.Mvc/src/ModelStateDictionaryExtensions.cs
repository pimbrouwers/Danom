namespace Danom.Mvc;

using Microsoft.AspNetCore.Mvc.ModelBinding;

/// <summary>
/// Danom extension methods for <see cref="ModelStateDictionary" />.
/// </summary>
public static class ModelStateDictionaryExtensions
{
    /// <summary>
    /// Adds <see cref="ResultErrors" /> to the <see cref="ModelStateDictionary" />.
    /// </summary>
    /// <param name="modelState"></param>
    /// <param name="errors"></param>
    public static void AddResultErrors(
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
