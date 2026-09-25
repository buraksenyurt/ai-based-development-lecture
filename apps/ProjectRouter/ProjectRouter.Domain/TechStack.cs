namespace ProjectRouter.Domain;

public class TechStack
{
    public IReadOnlyList<string> Languages { get; }
    public IReadOnlyList<string> Platforms { get; }
    public IReadOnlyList<string> Databases { get; }

    public TechStack(IEnumerable<string> languages, IEnumerable<string>? platforms, IEnumerable<string> databases)
    {
        Languages = Guard.OrderedList(languages, Rules.Rule04, "programming language", required: true);
        Platforms = Guard.OrderedList(platforms, DomainRuleException.Validation, "platform", required: false);
        Databases = Guard.OrderedList(databases, Rules.Rule04, "database", required: true);
    }
}
