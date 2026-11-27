using Danom.Operations;

namespace Danom.Examples.Todo;

public sealed class TodoId {
    internal readonly int _value;

    private TodoId(int value) => _value = value;

    public static explicit operator int(TodoId todoId) =>
        todoId._value;

    public static Option<TodoId> TryParse(int value) =>
        value > 0
            ? Option.Some(new TodoId(value))
            : Option<TodoId>.None();
}

public sealed record Todo(
    TodoId TodoId,
    string Title,
    Option<DateOnly> DueDate,
    Option<DateTime> CompletedDate);

public sealed class GetTodoQuery {
    public int TodoId { get; set; }
}

public sealed class GetTodoQueryHandler(
    IGetEntityAsync<Todo, TodoId> getTodo) :
    GetEntityQueryHandlerAsync<Todo, TodoId>(getTodo) { }
