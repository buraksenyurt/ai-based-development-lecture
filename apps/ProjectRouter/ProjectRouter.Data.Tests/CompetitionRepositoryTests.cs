using ProjectRouter.Domain;

namespace ProjectRouter.Data.Tests;

public class CompetitionRepositoryTests : SqliteTestDatabase
{
    private readonly CompetitionRepository _repository;
    private readonly ProjectIdea _arena = NewProject("Arena", min: 1, max: 2);
    private readonly ProjectIdea _budget = NewProject("Budget", min: 1, max: 2);
    private readonly Participant _ali = NewParticipant("Ali");
    private readonly Participant _veli = NewParticipant("Veli");

    public CompetitionRepositoryTests()
    {
        _repository = new CompetitionRepository(Factory);

        var projects = new ProjectIdeaRepository(Factory);
        projects.Save(_arena);
        projects.Save(_budget);

        var participants = new ParticipantRepository(Factory);
        participants.Save(_ali);
        participants.Save(_veli);
    }

    private Competition NewCompetition(string title = "AI Cup", string session = "2026-27") =>
        new(Guid.NewGuid(), title, session, [_arena.Id, _budget.Id], [_ali.Id, _veli.Id]);

    [Fact]
    public void Save_ThenGetById_RoundTripsListsAndSettlement()
    {
        var competition = NewCompetition();
        var settledAt = new DateTime(2026, 9, 25, 18, 30, 15, DateTimeKind.Utc);
        competition.AssignSettlement(
            new Dictionary<Guid, IReadOnlyList<Guid>> { [_arena.Id] = [_ali.Id, _veli.Id] },
            [_arena, _budget],
            settledAt);

        _repository.Save(competition);
        var loaded = _repository.GetById(competition.Id);

        Assert.NotNull(loaded);
        Assert.Equal("AI Cup", loaded.Title);
        Assert.Equal("2026-27", loaded.Session);
        Assert.Equal(competition.ProjectIds.Order(), loaded.ProjectIds.Order());
        Assert.Equal(competition.ParticipantIds.Order(), loaded.ParticipantIds.Order());
        Assert.Equal(settledAt, loaded.SettledAt);
        Assert.Equal(DateTimeKind.Utc, loaded.SettledAt!.Value.Kind);
        var (projectId, members) = Assert.Single(loaded.Settlement);
        Assert.Equal(_arena.Id, projectId);
        Assert.Equal(new[] { _ali.Id, _veli.Id }.Order(), members.Order());
    }

    [Fact]
    public void Save_WithoutSettlement_LoadsUnsettledCompetition()
    {
        var competition = NewCompetition();

        _repository.Save(competition);
        var loaded = _repository.GetById(competition.Id)!;

        Assert.Null(loaded.SettledAt);
        Assert.Empty(loaded.Settlement);
    }

    [Fact]
    public void Save_Existing_RewritesChildRows()
    {
        var competition = NewCompetition();
        competition.AssignSettlement(
            new Dictionary<Guid, IReadOnlyList<Guid>> { [_arena.Id] = [_ali.Id] },
            [_arena, _budget],
            DateTime.UtcNow);
        _repository.Save(competition);

        var edited = new Competition(competition.Id, "Renamed", "2027-28", [_budget.Id], [_veli.Id]);
        _repository.Save(edited);

        var loaded = _repository.GetById(competition.Id)!;
        Assert.Equal("Renamed", loaded.Title);
        Assert.Equal("2027-28", loaded.Session);
        Assert.Equal([_budget.Id], loaded.ProjectIds);
        Assert.Equal([_veli.Id], loaded.ParticipantIds);
        Assert.Empty(loaded.Settlement);
        Assert.Null(loaded.SettledAt);
        Assert.False(_repository.IsParticipantReferenced(_ali.Id));
        Assert.False(_repository.IsProjectReferenced(_arena.Id));
    }

    [Fact]
    public void GetById_Unknown_ReturnsNull()
    {
        Assert.Null(_repository.GetById(Guid.NewGuid()));
    }

    [Fact]
    public void GetAll_OrdersBySessionDescendingThenTitle()
    {
        _repository.Save(NewCompetition("Beta", "2025-26"));
        _repository.Save(NewCompetition("Zeta", "2026-27"));
        _repository.Save(NewCompetition("Alpha", "2026-27"));

        var all = _repository.GetAll();

        Assert.Equal(["Alpha", "Zeta", "Beta"], all.Select(c => c.Title));
    }

    [Fact]
    public void GetAll_EmptyDatabase_ReturnsEmpty()
    {
        Assert.Empty(_repository.GetAll());
    }

    [Fact]
    public void References_AreReportedForMembers()
    {
        var competition = new Competition(Guid.NewGuid(), "AI Cup", "2026-27", [_arena.Id], [_ali.Id]);
        _repository.Save(competition);

        Assert.True(_repository.IsParticipantReferenced(_ali.Id));
        Assert.False(_repository.IsParticipantReferenced(_veli.Id));
        Assert.True(_repository.IsProjectReferenced(_arena.Id));
        Assert.False(_repository.IsProjectReferenced(_budget.Id));
    }

    [Fact]
    public void Delete_RemovesCompetitionAndItsReferences()
    {
        var competition = NewCompetition();
        competition.AssignSettlement(
            new Dictionary<Guid, IReadOnlyList<Guid>> { [_arena.Id] = [_ali.Id] },
            [_arena, _budget],
            DateTime.UtcNow);
        _repository.Save(competition);

        _repository.Delete(competition.Id);

        Assert.Null(_repository.GetById(competition.Id));
        Assert.False(_repository.IsParticipantReferenced(_ali.Id));
        Assert.False(_repository.IsProjectReferenced(_arena.Id));
    }
}
