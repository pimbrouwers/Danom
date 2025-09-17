namespace Danom.Mvc;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// A controller that provides helper methods for working with Danom's monad types.
/// </summary>
public class DanomController
    : Controller {
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
        ResultErrors? errors = null) {
        if (errors is not null) {
            ModelState.AddResultErrors(errors);
        }

        return option.Match(
            some: x => {
                if (viewName is not null) {
                    return View(viewName, x);
                }
                else {
                    return View(x);
                }
            },
            none: () => {
                if (noneAction is not null) {
                    return noneAction();
                }

                return NotFound();
            });
    }

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
        object? model = null) {
        ModelState.AddResultErrors(errors);

        if (viewName is not null && model is not null) {
            return View(viewName, model);
        }
        else if (viewName is not null) {
            return View(viewName);
        }
        else if (model is not null) {
            return View(model);
        }
        else {
            return View();
        }
    }

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
        result.Match(
            ok: x => {
                if (viewName is not null && model is not null) {
                    return View(viewName, model);
                }
                else if (viewName is not null) {
                    return View(viewName, x);
                }
                else if (model is not null) {
                    return View(model);
                }
                else {
                    return View(x);
                }
            },
            error: errorAction);

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
        ViewResult(
            result: result,
            viewName: viewName,
            model: model,
            errorAction: e => ViewResultErrors(e, viewName, model));

}
