namespace ProjectRouter.Domain;

public class ProjectIdea
{
    public const int SummaryMaxLength = 250;

    public Guid Id { get; }
    public string Title { get; }
    public string Summary { get; }
    public TechStack TechStack { get; }
    public TeamSize Team { get; }
    public ProjectSize Size { get; }
    public IReadOnlyList<string> Similar { get; }
    public IReadOnlyList<string> Tags { get; }

    public ProjectIdea(
        Guid id,
        string title,
        string summary,
        TechStack techStack,
        TeamSize team,
        ProjectSize size,
        IEnumerable<string>? similar,
        IEnumerable<string>? tags)
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
        Similar = Guard.OrderedList(similar, DomainRuleException.Validation, "similar product", required: false);
        Tags = Guard.OrderedList(tags, DomainRuleException.Validation, "tag", required: false);
    }
}
