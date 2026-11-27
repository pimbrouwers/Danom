using Danom.Operations;

namespace Danom.Examples.Todo;

public sealed record DeleteTodo(
    TodoId TodoId,
    DateTime DeletedDate);

public sealed class DeleteTodoCommand {
    public int TodoId { get; set; }
}

public sealed class DeleteTodoCommandHandler(
    IDeleteEntityAsync<DeleteTodo> deleteTodo) :
    DeleteEntityCommandHandlerAsync<DeleteTodoCommand>(deleteTodo) { }
