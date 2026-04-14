namespace Danom.Mvc;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// A controller that provides helper methods for working with Danom's monad types.
/// </summary>
public class DanomController : Controller {
    /// <summary>
    /// An action result that returns a 404 Not Found response if the option is
    /// None, otherwise display the specified view with the option value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <param name="viewName"></param>
    /// <param name="noneAction"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public IActionResult ViewOption<T>(
        Option<T> option,
        string? viewName = null,
        Func<IActionResult>? noneAction = null,
        ResultErrors? errors = null) =>
        DanomControllerExtensions.ViewOption(
            this,
            option: option,
            viewName: viewName,
            noneAction: noneAction,
            errors: errors);

    /// <summary>
    /// Renders the specified view with the specified model, and add the
    /// provided errors to the model state.
    /// </summary>
    /// <param name="errors"></param>
    /// <param name="viewName"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public IActionResult ViewResultErrors(
        ResultErrors errors,
        string? viewName = null,
        object? model = null) =>
        DanomControllerExtensions.ViewResultErrors(
            this,
            errors: errors,
            viewName: viewName,
            model: model);

    /// <summary>
    /// An action result that renders the specified view with the specified model
    /// if the result is Ok, otherwise execute the specified error action.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="result"></param>
    /// <param name="errorAction"></param>
    /// <param name="viewName"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public IActionResult ViewResult<T, TError>(
        Result<T, TError> result,
        Func<TError, IActionResult> errorAction,
        string? viewName = null,
        object? model = null) =>
        DanomControllerExtensions.ViewResult(
            this,
            result: result,
            errorAction: errorAction,
            viewName: viewName,
            model: model);

    /// <summary>
    /// An action result that renders the specified view with the specified model
    /// if the result is Ok, otherwise display the errors in the model state.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <param name="viewName"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public IActionResult ViewResult<T>(
        Result<T, ResultErrors> result,
        string? viewName = null,
        object? model = null) =>
        DanomControllerExtensions.ViewResult(
            this,
            result: result,
            viewName: viewName,
            model: model);
}

/// <summary>
/// Danom extension methods for <see cref="Controller" />.
/// </summary>
public static class DanomControllerExtensions {
    /// <summary>
    /// An action result that returns a 404 Not Found response if the option is
    /// None, otherwise display the specified view with the option value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controller"></param>
    /// <param name="option"></param>
    /// <param name="viewName"></param>
    /// <param name="noneAction"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static IActionResult ViewOption<T>(
        this Controller controller,
        Option<T> option,
        string? viewName = null,
        Func<IActionResult>? noneAction = null,
        ResultErrors? errors = null) {
        if (errors is not null) {
            controller.ModelState.AddResultErrors(errors);
        }

        return option.Match(
            some: x => {
                if (!string.IsNullOrWhiteSpace(viewName)) {
                    return controller.View(viewName: viewName, x);
                }
                else {
                    return controller.View(x);
                }
            },
            none: () => {
                if (noneAction is not null) {
                    return noneAction();
                }

                return controller.NotFound();
            });
    }

    /// <summary>
    /// Renders the specified view with the specified model, and add the
    /// provided errors to the model state.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="errors"></param>
    /// <param name="viewName"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static IActionResult ViewResultErrors(
        this Controller controller,
        ResultErrors errors,
        string? viewName = null,
        object? model = null) {
        controller.ModelState.AddResultErrors(errors);

        if (viewName is not null && model is not null) {
            return controller.View(viewName, model);
        }
        else if (viewName is not null) {
            return controller.View(viewName);
        }
        else if (model is not null) {
            return controller.View(model);
        }
        else {
            return controller.View();
        }
    }

    /// <summary>
    /// An action result that renders the specified view with the specified model
    /// if the result is Ok, otherwise execute the specified error action.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="controller"></param>
    /// <param name="result"></param>
    /// <param name="errorAction"></param>
    /// <param name="viewName"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static IActionResult ViewResult<T, TError>(
        this Controller controller,
        Result<T, TError> result,
        Func<TError, IActionResult> errorAction,
        string? viewName = null,
        object? model = null) =>
        result.Match(
            ok: x => {
                if (viewName is not null && model is not null) {
                    return controller.View(viewName, model);
                }
                else if (viewName is not null) {
                    return controller.View(viewName, x);
                }
                else if (model is not null) {
                    return controller.View(model);
                }
                else {
                    return controller.View(x);
                }
            },
            error: errorAction);

    /// <summary>
    /// An action result that renders the specified view with the specified model
    /// if the result is Ok, otherwise display the errors in the model state.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controller"></param>
    /// <param name="result"></param>
    /// <param name="viewName"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static IActionResult ViewResult<T>(
        this Controller controller,
        Result<T, ResultErrors> result,
        string? viewName = null,
        object? model = null) =>
        controller.ViewResult(
            result: result,
            viewName: viewName,
            model: model,
            errorAction: e => controller.ViewResultErrors(e, viewName, model));

}
