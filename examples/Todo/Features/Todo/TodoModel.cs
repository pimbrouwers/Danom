namespace Danom.Examples.Todo;

using Danom;
using Danom.Validation;
using FluentValidation;

public sealed class TodoId(int value) {
    internal readonly int _value = value;
    public static explicit operator int(TodoId todoId) =>
        todoId._value;
}

public sealed class TodoIdValidator : AbstractValidator<TodoId> {
    public TodoIdValidator() {
        RuleFor(todoId => (int)todoId).GreaterThan(0);
    }
}

public sealed record Todo(
    TodoId TodoId,
    string Title,
    Option<DateOnly> DueDate,
    Option<DateTime> CompletedDate);

public sealed record TodoList(
    IEnumerable<Todo> Incomplete,
    IEnumerable<Todo> Complete);

//
// Create

public sealed record CreateTodo(
  string Title,
  Option<DateOnly> DueDate);

public sealed class CreateTodoValidator : AbstractValidator<CreateTodo> {
    public CreateTodoValidator() {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.DueDate)
            .Optional(x =>
                x.GreaterThan(DateOnly.FromDateTime(DateTime.Now.Date)));
    }
}


//
// Update

public sealed record CompleteTodo(
  TodoId TodoId,
  DateTime CompletedDate);

public sealed class CompleteTodoValidator : AbstractValidator<CompleteTodo> {
    public CompleteTodoValidator() {
        RuleFor(x => x.TodoId)
            .SetValidator(new TodoIdValidator());

        RuleFor(x => x.CompletedDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Now);
    }
}


//
// Delete

public sealed record DeleteTodo(
    TodoId TodoId,
    DateTime DeletedDate);

public sealed class DeleteTodoValidator : AbstractValidator<DeleteTodo> {
    public DeleteTodoValidator() {
        RuleFor(x => x.TodoId)
            .SetValidator(new TodoIdValidator());

        RuleFor(x => x.DeletedDate)
            .NotEmpty();
    }
}
