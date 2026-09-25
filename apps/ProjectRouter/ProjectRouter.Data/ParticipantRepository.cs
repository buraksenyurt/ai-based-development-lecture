using Dapper;
using ProjectRouter.Application.Interfaces;
using ProjectRouter.Domain;

namespace ProjectRouter.Data;

public class ParticipantRepository(SqliteConnectionFactory connectionFactory) : IParticipantRepository
{
    private const string SelectParticipants = """
        SELECT participant_id AS Id, full_name AS FullName, email AS Email, university AS University,
               department AS Department, class AS Class, github_url AS GithubUrl
        FROM participants
        """;

    private const string SelectPreferences = """
        SELECT participant_id AS OwnerId, kind AS Kind, name AS Name
        FROM participant_preferences
        """;

    public IReadOnlyList<Participant> GetAll()
    {
        using var connection = connectionFactory.Open();
        var rows = connection.Query<ParticipantRow>($"{SelectParticipants} ORDER BY full_name");
        var preferences = connection.Query<ItemRow>($"{SelectPreferences} ORDER BY rank");
        return Map(rows, preferences);
    }

    public Participant? GetById(Guid id) => GetByIds([id]).SingleOrDefault();

    public IReadOnlyList<Participant> GetByIds(IEnumerable<Guid> ids)
    {
        var keys = ids.Select(id => id.ToString()).ToList();
        if (keys.Count == 0)
            return [];

        using var connection = connectionFactory.Open();
        var rows = connection.Query<ParticipantRow>($"{SelectParticipants} WHERE participant_id IN @Ids ORDER BY full_name", new { Ids = keys });
        var preferences = connection.Query<ItemRow>($"{SelectPreferences} WHERE participant_id IN @Ids ORDER BY rank", new { Ids = keys });
        return Map(rows, preferences);
    }

    public void Save(Participant participant)
    {
        using var connection = connectionFactory.Open();
        using var transaction = connection.BeginTransaction();
        var id = participant.Id.ToString();

        connection.Execute("""
            INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
            VALUES (@Id, @FullName, @Email, @University, @Department, @Class, @GithubUrl)
            ON CONFLICT (participant_id) DO UPDATE
            SET full_name = excluded.full_name,
                email = excluded.email,
                university = excluded.university,
                department = excluded.department,
                class = excluded.class,
                github_url = excluded.github_url;
            """,
            new
            {
                Id = id,
                participant.FullName,
                participant.Email,
                participant.University,
                participant.Department,
                participant.Class,
                participant.GithubUrl
            },
            transaction);

        connection.Execute("DELETE FROM participant_preferences WHERE participant_id = @Id;", new { Id = id }, transaction);

        var preferences = ItemRow.Ranked(id, "language", participant.Languages)
            .Concat(ItemRow.Ranked(id, "database", participant.Databases));

        connection.Execute("""
            INSERT INTO participant_preferences (participant_id, kind, rank, name)
            VALUES (@OwnerId, @Kind, @Rank, @Name);
            """,
            preferences,
            transaction);

        transaction.Commit();
    }

    public void Delete(Guid id)
    {
        using var connection = connectionFactory.Open();
        connection.Execute("DELETE FROM participants WHERE participant_id = @Id;", new { Id = id.ToString() });
    }

    private static List<Participant> Map(IEnumerable<ParticipantRow> rows, IEnumerable<ItemRow> items)
    {
        var itemsByOwner = items.ToLookup(i => i.OwnerId);

        return rows.Select(row =>
        {
            var owned = itemsByOwner[row.Id].ToList();
            return new Participant(
                Guid.Parse(row.Id),
                new Identity(row.FullName, row.Email),
                new School(row.University, row.Department, (int)row.Class),
                row.GithubUrl,
                owned.Where(i => i.Kind == "language").Select(i => i.Name),
                owned.Where(i => i.Kind == "database").Select(i => i.Name));
        }).ToList();
    }

    private sealed class ParticipantRow
    {
        public string Id { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string University { get; set; } = "";
        public string Department { get; set; } = "";
        public long Class { get; set; }
        public string? GithubUrl { get; set; }
    }
}
