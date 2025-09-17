namespace Danom.Examples.Todo.Infrastructure;

using System.Data;
using System.Data.SQLite;
using Leger;

public sealed class TodoConnectionFactory(string connectionString)
    : IDbConnectionFactory {
    public IDbConnection CreateConnection() =>
        new SQLiteConnection(connectionString);
}
