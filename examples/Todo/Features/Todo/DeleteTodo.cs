namespace Danom.Examples.Todo;

using Danom;
using Danom.Validation;

public sealed record DeleteTodoCommand(
    int TodoId);

public sealed class DeleteTodoCommandHandler(
    Func<DeleteTodo, Unit> deleteTodo) {
    public Result<Unit, ResultErrors> Handle(DeleteTodoCommand command) =>
        ValidationResult<DeleteTodo>
            .From<DeleteTodoValidator>(
                new(TodoId: new(command.TodoId),
                    DeletedDate: DateTime.UtcNow))
            .Map(deleteTodo);
}
