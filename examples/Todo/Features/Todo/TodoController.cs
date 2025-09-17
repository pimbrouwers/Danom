namespace Danom.Examples.Todo;

using Danom;
using Danom.Mvc;
using Microsoft.AspNetCore.Mvc;

public sealed class TodoController(
    TodoStore todo) : DanomController {
    [Route("/")]
    [Route("/todo")]
    public IActionResult Index() {
        var result = new ListTodoQueryHandler(todo.GetAll).Handle();
        return ViewOption(result);
    }

    [HttpGet("/todo/create")]
    public IActionResult Create() =>
        View();

    [HttpPost("/todo/create")]
    public IActionResult Create(CreateTodoCommand command) {
        var result = new CreateTodoCommandHandler(todo.Create).Handle(command);
        return result.Match(
            ok: _ => RedirectToAction(nameof(Index)),
            error: e => ViewResultErrors(e));
    }

    [HttpGet("/todo/complete/{id}")]
    public IActionResult Complete(GetTodoQuery query) {
        var result = new GetTodoQueryHandler(todo.Get).Handle(query);
        return ViewOption(result);
    }

    [HttpPost("/todo/complete/{id}")]
    public IActionResult Complete(CompleteTodoCommand command) {
        var result = new CompleteTodoCommandHandler(todo.Complete).Handle(command);
        return result.Match(
            ok: _ => RedirectToAction(nameof(Index)),
            error: e => ViewResultErrors(e));
    }

    [HttpGet("/todo/delete/{id}")]
    public IActionResult Delete(GetTodoQuery query) {
        var result = new GetTodoQueryHandler(todo.Get).Handle(query);
        return ViewOption(result);
    }

    [HttpPost("/todo/delete/{id}")]
    public IActionResult Delete(DeleteTodoCommand command) {
        var result = new DeleteTodoCommandHandler(todo.Delete).Handle(command);
        return result.Match(
            ok: _ => RedirectToAction(nameof(Index)),
            error: e => ViewResultErrors(e));
    }
}
