
using Danom;
using Danom.Validation;

namespace Danom.Examples.Todo;

public sealed record CompleteTodo(
  TodoId TodoId,
  DateTime CompletedDate);

public sealed class CompleteTodoValidator : BaseValidator<CompleteTodo> {
    public CompleteTodoValidator() {
        Rule(x => x.TodoId, Check.IsValid(new TodoIdValidator()));

        Rule("Completed Date", x => x.CompletedDate,
            Check.IsGreaterThanOrEqualTo(DateTime.Now));
    }
}

public sealed record CompleteTodoCommand(
    int TodoId);

public sealed class CompleteTodoCommandHandler(
    Func<CompleteTodo, Unit> completeTodo) {
    public Result<Unit, ResultErrors> Handle(CompleteTodoCommand command) =>
        Validate<CompleteTodo>
            .Using<CompleteTodoValidator>(
                new(TodoId: new(command.TodoId),
                    CompletedDate: DateTime.UtcNow))
            .Map(completeTodo);
}
