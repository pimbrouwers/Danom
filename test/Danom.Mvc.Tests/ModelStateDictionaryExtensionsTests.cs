namespace Danom.Mvc.Tests;

using Xunit;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Danom.TestHelpers;

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

public sealed class ModelStateDictionaryExtensionsTests
{
    [Fact]
    public void AddResultErrorsShouldWork()
    {
        var modelState = new ModelStateDictionary();
        var errors = new ResultErrors("Key1", "Error1");
        modelState.AddResultErrors(errors);
        Assert.Equal(1, modelState.ErrorCount);
    }

    [Fact]
    public void AddResultErrorsShouldWorkWithMultipleErrors()
    {
        var modelState = new ModelStateDictionary();
        var errors = new ResultErrors([
            new ResultError("Key1", "Error1"),
            new ResultError("Key2", "Error2")]);
        modelState.AddResultErrors(errors);
        Assert.Equal(2, modelState.ErrorCount);
    }
}
