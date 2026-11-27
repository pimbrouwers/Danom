
using System.Data;
using Leger;
using Microsoft.Data.Sqlite;

namespace Danom.Examples.Todo.Infrastructure;

public sealed class TodoConnectionFactory(string connectionString)
    : IDbConnectionFactory {
    public IDbConnection CreateConnection() =>
        new SqliteConnection(connectionString);
}
