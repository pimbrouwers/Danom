using Danom.Operations;

namespace Danom.Examples.Todo;

public sealed record TodoList(
    IEnumerable<Todo> Incomplete,
    IEnumerable<Todo> Complete);

public sealed class ListTodoQueryHandler(
    IGetEntityAsync<TodoList, Unit> getTodoList) :
    GetEntityQueryHandlerAsync<TodoList, Unit>(getTodoList) { }
