namespace Danom.Examples.Todo;

using Danom;

public sealed class ListTodoQueryHandler(
    Func<IEnumerable<Todo>> getAllTodos)
{
    public Option<TodoList> Handle()
    {
        var todos = getAllTodos();
        if (todos.Any())
        {
            var todoList = new TodoList(
                Incomplete: todos.Where(x => x.CompletedDate.IsNone),
                Complete: todos.Where(x => x.CompletedDate.IsSome));

            return Option.Some(todoList);
        }
        else
        {
            return Option<TodoList>.None();
        }
    }
}
