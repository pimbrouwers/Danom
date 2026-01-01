namespace Danom.Mvc.Tests;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

internal static class ModelStateDictionaryExtensions {
    internal static void AddResultErrors(
        this ModelStateDictionary modelState,
        ResultErrors errors) {
        foreach (var error in errors) {
            foreach (var err in error.Errors) {
                modelState.AddModelError(error.Key, err);
            }
        }
    }
}

public sealed class ModelStateDictionaryExtensionsTests {
    [Fact]
    public void AddResultErrors() {
        var modelState = new ModelStateDictionary();
        var errors = new ResultErrors("Key1", "Error1");
        modelState.AddResultErrors(errors);
        Assert.Equal(1, modelState.ErrorCount);
        Assert.True(modelState.ContainsKey("Key1"));
    }

    [Fact]
    public void AddResultErrorsShouldWorkWithMultipleErrors() {
        var modelState = new ModelStateDictionary();
        var errors = new ResultErrors();
        errors.Add("Key1", "Error1");
        errors.Add("Key2", "Error2");
        modelState.AddResultErrors(errors);
        Assert.Equal(2, modelState.ErrorCount);
        Assert.True(modelState.ContainsKey("Key1"));
        Assert.True(modelState.ContainsKey("Key2"));
    }
}
