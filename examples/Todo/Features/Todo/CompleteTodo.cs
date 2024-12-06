namespace Danom.Examples.Todo;

using Danom;
using Danom.Validation;

public sealed record CompleteTodoCommand(
    int TodoId);

public sealed class CompleteTodoCommandHandler(
    Func<CompleteTodo, Unit> completeTodo)
{
    public Result<Unit, ResultErrors> Handle(CompleteTodoCommand command) =>
        ValidationResult<CompleteTodo>
            .From<CompleteTodoValidator>(
                new(TodoId: new(command.TodoId),
                    CompletedDate: DateTime.UtcNow))
            .Map(completeTodo);
}
