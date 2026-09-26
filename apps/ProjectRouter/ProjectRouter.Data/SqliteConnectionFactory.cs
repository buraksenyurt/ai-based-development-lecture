using Microsoft.Data.Sqlite;

namespace ProjectRouter.Data;

public class SqliteConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(string connectionString)
    {
        // Foreign keys are disabled by default in SQLite; the schema relies on them.
        _connectionString = new SqliteConnectionStringBuilder(connectionString) { ForeignKeys = true }.ToString();
    }

    public SqliteConnection Open()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
