using System.Globalization;
using Dapper;
using ProjectRouter.Application.Interfaces;
using ProjectRouter.Domain;

namespace ProjectRouter.Data;

public class CompetitionRepository(SqliteConnectionFactory connectionFactory) : ICompetitionRepository
{
    public IReadOnlyList<Competition> GetAll()
    {
        using var connection = connectionFactory.Open();
        var ids = connection.Query<string>("SELECT competition_id FROM competitions ORDER BY session DESC, title;");
        return ids.Select(id => Load(connection, id)!).ToList();
    }

    public Competition? GetById(Guid id)
    {
        using var connection = connectionFactory.Open();
        return Load(connection, id.ToString());
    }

    public void Save(Competition competition)
    {
        using var connection = connectionFactory.Open();
        using var transaction = connection.BeginTransaction();
        var id = competition.Id.ToString();

        connection.Execute("""
            INSERT INTO competitions (competition_id, title, session, settled_at)
            VALUES (@Id, @Title, @Session, @SettledAt)
            ON CONFLICT (competition_id) DO UPDATE
            SET title = excluded.title,
                session = excluded.session,
                settled_at = excluded.settled_at;
            """,
            new
            {
                Id = id,
                competition.Title,
                competition.Session,
                SettledAt = competition.SettledAt?.ToString("O", CultureInfo.InvariantCulture)
            },
            transaction);

        // Child rows are rewritten as a whole; settlements go first because they reference the other two.
        connection.Execute("DELETE FROM settlements WHERE competition_id = @Id;", new { Id = id }, transaction);
        connection.Execute("DELETE FROM competition_projects WHERE competition_id = @Id;", new { Id = id }, transaction);
        connection.Execute("DELETE FROM competition_participants WHERE competition_id = @Id;", new { Id = id }, transaction);

        connection.Execute(
            "INSERT INTO competition_projects (competition_id, project_id) VALUES (@CompetitionId, @ProjectId);",
            competition.ProjectIds.Select(p => new { CompetitionId = id, ProjectId = p.ToString() }),
            transaction);

        connection.Execute(
            "INSERT INTO competition_participants (competition_id, participant_id) VALUES (@CompetitionId, @ParticipantId);",
            competition.ParticipantIds.Select(p => new { CompetitionId = id, ParticipantId = p.ToString() }),
            transaction);

        connection.Execute(
            "INSERT INTO settlements (competition_id, project_id, participant_id) VALUES (@CompetitionId, @ProjectId, @ParticipantId);",
            competition.Settlement.SelectMany(kv => kv.Value.Select(participantId => new
            {
                CompetitionId = id,
                ProjectId = kv.Key.ToString(),
                ParticipantId = participantId.ToString()
            })),
            transaction);

        transaction.Commit();
    }

    public void Delete(Guid id)
    {
        using var connection = connectionFactory.Open();
        connection.Execute("DELETE FROM competitions WHERE competition_id = @Id;", new { Id = id.ToString() });
    }

    public bool IsParticipantReferenced(Guid participantId)
    {
        using var connection = connectionFactory.Open();
        return connection.ExecuteScalar<bool>(
            "SELECT EXISTS (SELECT 1 FROM competition_participants WHERE participant_id = @Id);",
            new { Id = participantId.ToString() });
    }

    public bool IsProjectReferenced(Guid projectId)
    {
        using var connection = connectionFactory.Open();
        return connection.ExecuteScalar<bool>(
            "SELECT EXISTS (SELECT 1 FROM competition_projects WHERE project_id = @Id);",
            new { Id = projectId.ToString() });
    }

    private static Competition? Load(System.Data.IDbConnection connection, string id)
    {
        var row = connection.QuerySingleOrDefault<CompetitionRow>("""
            SELECT competition_id AS Id, title AS Title, session AS Session, settled_at AS SettledAt
            FROM competitions
            WHERE competition_id = @Id;
            """,
            new { Id = id });

        if (row is null)
            return null;

        var projectIds = connection.Query<string>(
            "SELECT project_id FROM competition_projects WHERE competition_id = @Id;", new { Id = id });
        var participantIds = connection.Query<string>(
            "SELECT participant_id FROM competition_participants WHERE competition_id = @Id;", new { Id = id });
        var settlementRows = connection.Query<SettlementRow>(
            "SELECT project_id AS ProjectId, participant_id AS ParticipantId FROM settlements WHERE competition_id = @Id;",
            new { Id = id });

        var competition = new Competition(
            Guid.Parse(row.Id),
            row.Title,
            row.Session,
            projectIds.Select(Guid.Parse),
            participantIds.Select(Guid.Parse));

        var settlement = settlementRows
            .GroupBy(s => Guid.Parse(s.ProjectId))
            .ToDictionary(g => g.Key, g => (IReadOnlyList<Guid>)g.Select(s => Guid.Parse(s.ParticipantId)).ToList());

        var settledAt = row.SettledAt is null
            ? (DateTime?)null
            : DateTime.Parse(row.SettledAt, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);

        competition.RestoreSettlement(settlement, settledAt);
        return competition;
    }

    // Materialized by Dapper through the constructor: parameter order must match the SELECT column order.
    private sealed record CompetitionRow(string Id, string Title, string Session, string? SettledAt);

    private sealed record SettlementRow(string ProjectId, string ParticipantId);
}
