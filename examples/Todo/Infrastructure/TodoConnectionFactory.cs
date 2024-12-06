namespace Danom.Examples.Todo.Infrastructure;

using System.Data;
using Leger;
using System.Data.SQLite;

public sealed class TodoConnectionFactory(string connectionString)
    : IDbConnectionFactory
{
    public IDbConnection CreateConnection() =>
        new SQLiteConnection(connectionString);
}
