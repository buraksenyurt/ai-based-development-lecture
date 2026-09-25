namespace ProjectRouter.Domain;

public class ProjectIdea
{
    public const int SummaryMaxLength = 250;

    private readonly IReadOnlyList<string> _similar = [];
    private readonly IReadOnlyList<string> _tags = [];

    public Guid Id { get; }
    public string Title { get; }
    public string Summary { get; }
    public TechStack TechStack { get; }
    public TeamSize Team { get; }
    public ProjectSize Size { get; }

    /// <summary>Optional list of similar products (e.g. "Kahoot").</summary>
    public IReadOnlyList<string> Similar
    {
        get => _similar;
        init => _similar = Guard.OrderedList(value, DomainRuleException.Validation, "similar product", required: false);
    }

    /// <summary>Optional list of tags.</summary>
    public IReadOnlyList<string> Tags
    {
        get => _tags;
        init => _tags = Guard.OrderedList(value, DomainRuleException.Validation, "tag", required: false);
    }

    /// <param name="id">A new id (<see cref="Guid.NewGuid"/>) for a new project, or the existing id when updating or loading one.</param>
    public ProjectIdea(
        Guid id,
        string title,
        string summary,
        TechStack techStack,
        TeamSize team,
        ProjectSize size)
    {
        ArgumentNullException.ThrowIfNull(techStack);

        if (team == default)
            throw new DomainRuleException(Rules.Rule02, "Team size must be specified.");

        Id = Guard.Id(id, "Project");
        Title = Guard.Required(title, "Title");
        Summary = Guard.Required(summary, "Summary", SummaryMaxLength);
        TechStack = techStack;
        Team = team;
        Size = size;
    }
}
