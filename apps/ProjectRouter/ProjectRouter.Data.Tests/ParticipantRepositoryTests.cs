using ProjectRouter.Domain;

namespace ProjectRouter.Data.Tests;

public class ParticipantRepositoryTests : SqliteTestDatabase
{
    private readonly ParticipantRepository _repository;

    public ParticipantRepositoryTests() => _repository = new ParticipantRepository(Factory);

    [Fact]
    public void Save_ThenGetById_RoundTripsAllFields()
    {
        var participant = NewParticipant("Ada Lovelace", ["Go", "C#", "Rust"], ["Redis", "SQLite"], "https://github.com/ada");

        _repository.Save(participant);
        var loaded = _repository.GetById(participant.Id);

        Assert.NotNull(loaded);
        Assert.Equal(participant.Id, loaded.Id);
        Assert.Equal(participant.Identity, loaded.Identity);
        Assert.Equal(participant.School, loaded.School);
        Assert.Equal("https://github.com/ada", loaded.GithubUrl);
        Assert.Equal(["Go", "C#", "Rust"], loaded.Languages);
        Assert.Equal(["Redis", "SQLite"], loaded.Databases);
    }

    [Fact]
    public void Save_Existing_UpdatesFieldsAndReplacesPreferences()
    {
        var participant = NewParticipant("Ada Lovelace", ["Go", "C#"], ["Redis"]);
        _repository.Save(participant);

        var updated = new Participant(
            participant.Id,
            new Identity("Ada King", "ada.king@example.com"),
            new School("ODTU", "Software Engineering", 4),
            null,
            ["Python"],
            ["PostgreSQL", "MongoDB"]);
        _repository.Save(updated);

        var loaded = _repository.GetById(participant.Id)!;
        Assert.Equal("Ada King", loaded.FullName);
        Assert.Equal("ODTU", loaded.University);
        Assert.Equal(4, loaded.Class);
        Assert.Null(loaded.GithubUrl);
        Assert.Equal(["Python"], loaded.Languages);
        Assert.Equal(["PostgreSQL", "MongoDB"], loaded.Databases);
        Assert.Single(_repository.GetAll());
    }

    [Fact]
    public void GetById_Unknown_ReturnsNull()
    {
        Assert.Null(_repository.GetById(Guid.NewGuid()));
    }

    [Fact]
    public void GetAll_OrdersByFullName()
    {
        _repository.Save(NewParticipant("Zeynep"));
        _repository.Save(NewParticipant("Ali"));
        _repository.Save(NewParticipant("Mehmet"));

        var all = _repository.GetAll();

        Assert.Equal(["Ali", "Mehmet", "Zeynep"], all.Select(p => p.FullName));
    }

    [Fact]
    public void GetAll_EmptyDatabase_ReturnsEmpty()
    {
        Assert.Empty(_repository.GetAll());
    }

    [Fact]
    public void GetByIds_ReturnsOnlyRequestedParticipants()
    {
        var ali = NewParticipant("Ali");
        var veli = NewParticipant("Veli");
        _repository.Save(ali);
        _repository.Save(veli);
        _repository.Save(NewParticipant("Ayse"));

        var result = _repository.GetByIds([veli.Id, ali.Id, Guid.NewGuid()]);

        Assert.Equal([ali.Id, veli.Id], result.Select(p => p.Id));
    }

    [Fact]
    public void GetByIds_WithNoIds_ReturnsEmpty()
    {
        _repository.Save(NewParticipant("Ali"));

        Assert.Empty(_repository.GetByIds([]));
    }

    [Fact]
    public void Delete_RemovesParticipantAndPreferences()
    {
        var participant = NewParticipant("Ali");
        _repository.Save(participant);

        _repository.Delete(participant.Id);

        Assert.Null(_repository.GetById(participant.Id));
        Assert.Empty(_repository.GetAll());
    }
}
