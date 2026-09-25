using Dapper;
using ProjectRouter.Application.Interfaces;
using ProjectRouter.Domain;

namespace ProjectRouter.Data;

public class ProjectIdeaRepository(SqliteConnectionFactory connectionFactory) : IProjectIdeaRepository
{
    private const string SelectProjects = """
        SELECT project_id AS Id, title AS Title, summary AS Summary,
               team_min AS TeamMin, team_max AS TeamMax, size AS Size
        FROM projects
        """;

    private const string SelectItems = """
        SELECT project_id AS OwnerId, kind AS Kind, name AS Name
        FROM project_items
        """;

    public IReadOnlyList<ProjectIdea> GetAll()
    {
        using var connection = connectionFactory.Open();
        var rows = connection.Query<ProjectRow>($"{SelectProjects} ORDER BY title");
        var items = connection.Query<ItemRow>($"{SelectItems} ORDER BY rank");
        return Map(rows, items);
    }

    public ProjectIdea? GetById(Guid id) => GetByIds([id]).SingleOrDefault();

    public IReadOnlyList<ProjectIdea> GetByIds(IEnumerable<Guid> ids)
    {
        var keys = ids.Select(id => id.ToString()).ToList();
        if (keys.Count == 0)
            return [];

        using var connection = connectionFactory.Open();
        var rows = connection.Query<ProjectRow>($"{SelectProjects} WHERE project_id IN @Ids ORDER BY title", new { Ids = keys });
        var items = connection.Query<ItemRow>($"{SelectItems} WHERE project_id IN @Ids ORDER BY rank", new { Ids = keys });
        return Map(rows, items);
    }

    public void Save(ProjectIdea project)
    {
        using var connection = connectionFactory.Open();
        using var transaction = connection.BeginTransaction();
        var id = project.Id.ToString();

        connection.Execute("""
            INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
            VALUES (@Id, @Title, @Summary, @TeamMin, @TeamMax, @Size)
            ON CONFLICT (project_id) DO UPDATE
            SET title = excluded.title,
                summary = excluded.summary,
                team_min = excluded.team_min,
                team_max = excluded.team_max,
                size = excluded.size;
            """,
            new
            {
                Id = id,
                project.Title,
                project.Summary,
                TeamMin = project.Team.Min,
                TeamMax = project.Team.Max,
                Size = project.Size.ToString()
            },
            transaction);

        connection.Execute("DELETE FROM project_items WHERE project_id = @Id;", new { Id = id }, transaction);

        var items = ItemRow.Ranked(id, "language", project.TechStack.Languages)
            .Concat(ItemRow.Ranked(id, "platform", project.TechStack.Platforms))
            .Concat(ItemRow.Ranked(id, "database", project.TechStack.Databases))
            .Concat(ItemRow.Ranked(id, "similar", project.Similar))
            .Concat(ItemRow.Ranked(id, "tag", project.Tags));

        connection.Execute("""
            INSERT INTO project_items (project_id, kind, rank, name)
            VALUES (@OwnerId, @Kind, @Rank, @Name);
            """,
            items,
            transaction);

        transaction.Commit();
    }

    public void Delete(Guid id)
    {
        using var connection = connectionFactory.Open();
        connection.Execute("DELETE FROM projects WHERE project_id = @Id;", new { Id = id.ToString() });
    }

    private static List<ProjectIdea> Map(IEnumerable<ProjectRow> rows, IEnumerable<ItemRow> items)
    {
        var itemsByOwner = items.ToLookup(i => i.OwnerId);

        return rows.Select(row =>
        {
            var owned = itemsByOwner[row.Id].ToList();
            IEnumerable<string> Of(string kind) => owned.Where(i => i.Kind == kind).Select(i => i.Name);

            return new ProjectIdea(
                row.Title,
                row.Summary,
                new TechStack(Of("language"), Of("platform"), Of("database")),
                new TeamSize((int)row.TeamMin, (int)row.TeamMax),
                Enum.Parse<ProjectSize>(row.Size),
                Of("similar"),
                Of("tag"));
        }).ToList();
    }

    private sealed class ProjectRow
    {
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public string Summary { get; set; } = "";
        public long TeamMin { get; set; }
        public long TeamMax { get; set; }
        public string Size { get; set; } = "";
    }
}
