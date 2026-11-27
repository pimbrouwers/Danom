
using Danom;
using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Danom.Examples.Todo;

public sealed class TodoController(
    TodoStore todo) : DanomController {
    [HttpGet]
    public IActionResult Index() {
        var result = new ListTodoQueryHandler(todo.GetAll).Handle();
        return ViewOption(result);
    }

    [HttpGet]
    public IActionResult Create() =>
        View();

    [HttpPost]
    public IActionResult Create(CreateTodoCommand command) {
        var result = new CreateTodoCommandHandler(todo.Create).Handle(command);
        return result.Match(
            ok: _ => RedirectToAction(nameof(Index)),
            error: e => ViewResultErrors(e));
    }

    [HttpGet]
    public IActionResult Complete(GetTodoQuery query) {
        var result = new GetTodoQueryHandler(todo.Get).Handle(query);
        return ViewOption(result);
    }

    [HttpPost]
    public IActionResult Complete(CompleteTodoCommand command) {
        var result = new CompleteTodoCommandHandler(todo.Complete).Handle(command);
        return result.Match(
            ok: _ => RedirectToAction(nameof(Index)),
            error: e => ViewResultErrors(e));
    }

    [HttpGet]
    public IActionResult Delete(GetTodoQuery query) {
        var result = new GetTodoQueryHandler(todo.Get).Handle(query);
        return ViewOption(result);
    }

    [HttpPost]
    public IActionResult Delete(DeleteTodoCommand command) {
        var result = new DeleteTodoCommandHandler(todo.Delete).Handle(command);
        return result.Match(
            ok: _ => RedirectToAction(nameof(Index)),
            error: e => ViewResultErrors(e));
    }
}
