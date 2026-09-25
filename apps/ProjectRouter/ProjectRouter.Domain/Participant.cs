namespace ProjectRouter.Domain;

public class Participant
{
    public Guid Id { get; }
    public string FullName { get; }
    public string Email { get; }
    public string University { get; }
    public string Department { get; }
    public int Class { get; }
    public string? GithubUrl { get; }

    /// <summary>Preferred programming languages, ordered from first to last choice (Rule 00).</summary>
    public IReadOnlyList<string> Languages { get; }

    /// <summary>Preferred databases, ordered from first to last choice (Rule 01).</summary>
    public IReadOnlyList<string> Databases { get; }

    public Participant(
        Guid id,
        string fullName,
        string email,
        string university,
        string department,
        int @class,
        string? githubUrl,
        IEnumerable<string> languages,
        IEnumerable<string> databases)
    {
        if (@class <= 0)
            throw new DomainRuleException(DomainRuleException.Validation, "Class must be greater than zero.");

        var mail = Guard.Required(email, "Email");
        if (!mail.Contains('@'))
            throw new DomainRuleException(DomainRuleException.Validation, "Email is not valid.");

        Id = Guard.Id(id, "Participant");
        FullName = Guard.Required(fullName, "Full name");
        Email = mail;
        University = Guard.Required(university, "University");
        Department = Guard.Required(department, "Department");
        Class = @class;
        GithubUrl = string.IsNullOrWhiteSpace(githubUrl) ? null : githubUrl.Trim();
        Languages = Guard.OrderedList(languages, Rules.Rule00, "programming language", required: true);
        Databases = Guard.OrderedList(databases, Rules.Rule01, "database", required: true);
    }
}
