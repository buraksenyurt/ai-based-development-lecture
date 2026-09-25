using ProjectRouter.Domain;

namespace ProjectRouter.Data.Tests;

public class ProjectIdeaRepositoryTests : SqliteTestDatabase
{
    private readonly ProjectIdeaRepository _repository;

    public ProjectIdeaRepositoryTests() => _repository = new ProjectIdeaRepository(Factory);

    [Fact]
    public void Save_ThenGetById_RoundTripsAllFields()
    {
        var project = NewProject("Quiz Game", min: 2, max: 5);

        _repository.Save(project);
        var loaded = _repository.GetById(project.Id);

        Assert.NotNull(loaded);
        Assert.Equal("Quiz Game", loaded.Title);
        Assert.Equal("Summary", loaded.Summary);
        Assert.Equal(new TeamSize(2, 5), loaded.Team);
        Assert.Equal(ProjectSize.L, loaded.Size);
        Assert.Equal(["C#", "TypeScript"], loaded.TechStack.Languages);
        Assert.Equal(["web", "mobile"], loaded.TechStack.Platforms);
        Assert.Equal(["PostgreSQL"], loaded.TechStack.Databases);
        Assert.Equal(["Kahoot"], loaded.Similar);
        Assert.Equal(["education", "game"], loaded.Tags);
    }

    [Fact]
    public void Save_Existing_UpdatesFieldsAndReplacesItems()
    {
        var project = NewProject("Quiz Game");
        _repository.Save(project);

        var updated = new ProjectIdea(
            project.Id,
            "Quiz Arena",
            "New summary",
            new TechStack(["Rust"], null, ["Redis"]),
            new TeamSize(3, 4),
            ProjectSize.S);
        _repository.Save(updated);

        var loaded = _repository.GetById(project.Id)!;
        Assert.Equal("Quiz Arena", loaded.Title);
        Assert.Equal("New summary", loaded.Summary);
        Assert.Equal(new TeamSize(3, 4), loaded.Team);
        Assert.Equal(ProjectSize.S, loaded.Size);
        Assert.Equal(["Rust"], loaded.TechStack.Languages);
        Assert.Empty(loaded.TechStack.Platforms);
        Assert.Equal(["Redis"], loaded.TechStack.Databases);
        Assert.Empty(loaded.Similar);
        Assert.Empty(loaded.Tags);
        Assert.Single(_repository.GetAll());
    }

    [Fact]
    public void GetById_Unknown_ReturnsNull()
    {
        Assert.Null(_repository.GetById(Guid.NewGuid()));
    }

    [Fact]
    public void GetAll_OrdersByTitle()
    {
        _repository.Save(NewProject("Chat"));
        _repository.Save(NewProject("Arena"));
        _repository.Save(NewProject("Budget"));

        Assert.Equal(["Arena", "Budget", "Chat"], _repository.GetAll().Select(p => p.Title));
    }

    [Fact]
    public void GetByIds_ReturnsOnlyRequestedProjects()
    {
        var arena = NewProject("Arena");
        var chat = NewProject("Chat");
        _repository.Save(arena);
        _repository.Save(chat);
        _repository.Save(NewProject("Budget"));

        var result = _repository.GetByIds([chat.Id, arena.Id]);

        Assert.Equal([arena.Id, chat.Id], result.Select(p => p.Id));
    }

    [Fact]
    public void GetByIds_WithNoIds_ReturnsEmpty()
    {
        _repository.Save(NewProject("Arena"));

        Assert.Empty(_repository.GetByIds([]));
    }

    [Fact]
    public void Delete_RemovesProject()
    {
        var project = NewProject("Arena");
        _repository.Save(project);

        _repository.Delete(project.Id);

        Assert.Null(_repository.GetById(project.Id));
    }
}
