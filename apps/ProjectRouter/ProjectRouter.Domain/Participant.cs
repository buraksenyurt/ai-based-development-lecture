namespace ProjectRouter.Domain;

public class Participant
{
    public Guid Id { get; }
    public Identity Identity { get; }
    public School School { get; }
    public string? GithubUrl { get; }

    public string FullName => Identity.FullName;
    public string Email => Identity.Email;
    public string University => School.University;
    public string Department => School.Department;
    public int Class => School.Class;

    /// <summary>Preferred programming languages, ordered from first to last choice (Rule 00).</summary>
    public IReadOnlyList<string> Languages { get; }

    /// <summary>Preferred databases, ordered from first to last choice (Rule 01).</summary>
    public IReadOnlyList<string> Databases { get; }

    /// <param name="id">A new id (<see cref="Guid.NewGuid"/>) for a new participant, or the existing id when updating or loading one.</param>
    public Participant(
        Guid id,
        Identity identity,
        School school,
        string? githubUrl,
        IEnumerable<string> languages,
        IEnumerable<string> databases)
    {
        ArgumentNullException.ThrowIfNull(identity);
        ArgumentNullException.ThrowIfNull(school);

        Id = Guard.Id(id, "Participant");
        Identity = identity;
        School = school;
        GithubUrl = string.IsNullOrWhiteSpace(githubUrl) ? null : githubUrl.Trim();
        Languages = Guard.OrderedList(languages, Rules.Rule00, "programming language", required: true);
        Databases = Guard.OrderedList(databases, Rules.Rule01, "database", required: true);
    }
}
