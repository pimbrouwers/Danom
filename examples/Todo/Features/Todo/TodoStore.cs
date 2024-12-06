namespace Danom.Examples.Todo;

using System.Data;
using Danom;
using Leger;

public static class TodoReader
{
    public static Todo Map(IDataReader rd) =>
        new(TodoId: new(rd.ReadInt("todo_id")),
                Title: rd.ReadString("title"),
                DueDate: rd.ReadNullableDateOnly("due_date").ToOption(),
                CompletedDate: rd.ReadNullableDateTime("completed_date").ToOption());
}
public sealed class TodoStore(
    IDbConnectionFactory db)
{
    public IEnumerable<Todo> GetAll() =>
        db.Query("""
            SELECT    todo_id
                    , title
                    , due_date
                    , completed_date
            FROM      todo
            WHERE     deleted_date IS NULL
            """,
            TodoReader.Map);

    public Option<Todo> Get(TodoId todoId) =>
        db.QuerySingle("""
            SELECT    todo_id
                    , title
                    , due_date
                    , completed_date
            FROM      todo
            WHERE     todo_id = @todo_id
            """,
            new DbParams("todo_id", (int)todoId),
            TodoReader.Map)
            .ToOption();

    public Unit Create(CreateTodo input)
    {
        db.Execute("""
            INSERT INTO todo (title, due_date)
            VALUES (@title, @due_date);
            """,
            new DbParams()
            {
                { "title", input.Title },
                { "due_date", input.DueDate.ToNullable() }
            });

        return Unit.Value;
    }

    public Unit Complete(CompleteTodo input)
    {
        db.Execute("""
            UPDATE    todo
            SET       completed_date = @completed_date
            WHERE     todo_id = @todo_id;
            """,
            new DbParams()
            {
                { "todo_id", (int)input.TodoId },
                { "completed_date", input.CompletedDate }
            });

        return Unit.Value;
    }

    public Unit Delete(DeleteTodo input)
    {
        db.Execute("""
            UPDATE    todo
            SET       deleted_date = @deleted_date
            WHERE     todo_id = @todo_id;
            """,
            new DbParams()
            {
                { "todo_id", (int)input.TodoId },
                { "deleted_date", input.DeletedDate }
            });

        return Unit.Value;
    }
}
