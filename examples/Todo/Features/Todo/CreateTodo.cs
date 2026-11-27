
using Danom;
using Danom.Validation;

namespace Danom.Examples.Todo;

public sealed record CreateTodo(
  string Title,
  Option<DateOnly> DueDate);

public sealed class CreateTodoValidator : BaseValidator<CreateTodo> {
    public CreateTodoValidator() {
        Rule("Title", x => x.Title, [
            Check.String.IsNotEmpty,
            Check.String.IsLengthOrLessThan(256)]);

        Optional("Due Date", x => x.DueDate,
            Check.IsGreaterThan(DateOnly.FromDateTime(DateTime.Now.Date)));
    }
}

public sealed class CreateTodoCommand {
    public required string Title { get; set; }
    public DateOnly? DueDate { get; set; }
}

public sealed class CreateTodoCommandHandler(
    Func<CreateTodo, Unit> createTodo) {
    public Result<Unit, ResultErrors> Handle(CreateTodoCommand command) =>
        Validate<CreateTodo>
            .Using<CreateTodoValidator>(
                new(Title: command.Title,
                    DueDate: command.DueDate.ToOption()))
            .Map(createTodo);
}
