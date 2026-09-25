using Microsoft.Data.Sqlite;
using ProjectRouter.Domain;

namespace ProjectRouter.Data.Tests;

/// <summary>
/// A private, shared-cache in-memory SQLite database per test class instance.
/// The keep-alive connection holds the database open while repositories open and close their own connections.
/// </summary>
public abstract class SqliteTestDatabase : IDisposable
{
    private readonly SqliteConnection _keepAlive;

    protected SqliteConnectionFactory Factory { get; }

    protected SqliteTestDatabase()
    {
        var connectionString = $"Data Source=projectrouter-tests-{Guid.NewGuid():N};Mode=Memory;Cache=Shared";
        _keepAlive = new SqliteConnection(connectionString);
        _keepAlive.Open();

        Factory = new SqliteConnectionFactory(connectionString);
        new DatabaseInitializer(Factory).Initialize();
    }

    public void Dispose()
    {
        _keepAlive.Dispose();
        GC.SuppressFinalize(this);
    }

    protected static Participant NewParticipant(string name, string[]? languages = null, string[]? databases = null, string? githubUrl = null) =>
        new(
            Guid.NewGuid(),
            new Identity(name, $"{name.Replace(' ', '.').ToLowerInvariant()}@example.com"),
            new School("ITU", "Computer Engineering", 3),
            githubUrl,
            languages ?? ["C#", "Python"],
            databases ?? ["SQLite", "NoSQL-*"]);

    protected static ProjectIdea NewProject(string title, int min = 1, int max = 3) =>
        new(
            Guid.NewGuid(),
            title,
            "Summary",
            new TechStack(["C#", "TypeScript"], ["web", "mobile"], ["PostgreSQL"]),
            new TeamSize(min, max),
            ProjectSize.L)
        {
            Similar = ["Kahoot"],
            Tags = ["education", "game"],
        };
}
