namespace Danom.Examples.Todo;

using Danom;
using Danom.Validation;

public sealed record CreateTodoCommand(
    string Title,
    DateOnly? DueDate);

public sealed class CreateTodoCommandHandler(
    Func<CreateTodo, Unit> createTodo) {
    public Result<Unit, ResultErrors> Handle(CreateTodoCommand command) =>
        ValidationResult<CreateTodo>
            .From<CreateTodoValidator>(
                new(Title: command.Title,
                    DueDate: command.DueDate.ToOption()))
            .Map(createTodo);
}
