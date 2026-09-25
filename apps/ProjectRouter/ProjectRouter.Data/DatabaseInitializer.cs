using Dapper;

namespace ProjectRouter.Data;

/// <summary>Creates the tables on application start (idempotent, uses CREATE TABLE IF NOT EXISTS).</summary>
public class DatabaseInitializer(SqliteConnectionFactory connectionFactory)
{
    public void Initialize()
    {
        using var stream = typeof(DatabaseInitializer).Assembly.GetManifestResourceStream("ProjectRouter.Data.Schema.sql")
            ?? throw new InvalidOperationException("Schema.sql resource was not found.");
        using var reader = new StreamReader(stream);
        var script = reader.ReadToEnd();

        using var connection = connectionFactory.Open();
        connection.Execute(script);
    }
}
