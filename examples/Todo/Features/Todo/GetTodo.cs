namespace Danom.Examples.Todo;

using Danom;
using Danom.Validation;

public sealed record GetTodoQuery(
    int TodoId);

public sealed class GetTodoQueryHandler(
    Func<TodoId, Option<Todo>> getTodo)
{
    public Option<Todo> Handle(GetTodoQuery query) =>
        ValidationResult<TodoId>
            .From<TodoIdValidator>(new(query.TodoId))
            .Map(getTodo)
            .DefaultWith(Option<Todo>.None);
}
