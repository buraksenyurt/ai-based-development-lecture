namespace ProjectRouter.Domain;

/// <summary>
/// The main aggregate where projects and participants are matched.
/// Settlement maps a project id to the ids of the participants placed in that project.
/// </summary>
public class Competition
{
    private readonly List<Guid> _projectIds;
    private readonly List<Guid> _participantIds;
    private Dictionary<Guid, IReadOnlyList<Guid>> _settlement = [];

    public Guid Id { get; }
    public string Title { get; }
    public string Session { get; }
    public IReadOnlyList<Guid> ProjectIds => _projectIds.AsReadOnly();
    public IReadOnlyList<Guid> ParticipantIds => _participantIds.AsReadOnly();
    public IReadOnlyDictionary<Guid, IReadOnlyList<Guid>> Settlement => _settlement.AsReadOnly();
    public DateTime? SettledAt { get; private set; }

    public Competition(
        Guid id,
        string title,
        string session,
        IEnumerable<Guid> projectIds,
        IEnumerable<Guid> participantIds)
    {
        Id = Guard.Id(id, "Competition");
        Title = Guard.Required(title, "Title");
        Session = Guard.Required(session, "Session");
        _projectIds = projectIds.Distinct().ToList();
        _participantIds = participantIds.Distinct().ToList();
    }

    /// <summary>
    /// Validates and stores a settlement. Every opened project must satisfy its team size (Rule 02)
    /// and every participant may be placed in at most one project (Rule 03).
    /// Projects that are left out of the settlement (or have no members) are not opened.
    /// </summary>
    public void AssignSettlement(
        IReadOnlyDictionary<Guid, IReadOnlyList<Guid>> settlement,
        IEnumerable<ProjectIdea> projects,
        DateTime settledAt)
    {
        ArgumentNullException.ThrowIfNull(settlement);
        var projectLookup = projects.ToDictionary(p => p.Id);
        var placed = new HashSet<Guid>();
        var validated = new Dictionary<Guid, IReadOnlyList<Guid>>();

        foreach (var (projectId, members) in settlement)
        {
            if (!_projectIds.Contains(projectId) || !projectLookup.TryGetValue(projectId, out var project))
                throw new DomainRuleException(DomainRuleException.Validation, $"Project '{projectId}' is not part of this competition.");

            if (members.Count == 0)
                continue;

            if (!project.Team.Allows(members.Count))
                throw new DomainRuleException(Rules.Rule02,
                    $"Project '{project.Title}' has {members.Count} members but its team size must be between {project.Team.Min} and {project.Team.Max}.");

            foreach (var participantId in members)
            {
                if (!_participantIds.Contains(participantId))
                    throw new DomainRuleException(DomainRuleException.Validation, $"Participant '{participantId}' is not part of this competition.");

                if (!placed.Add(participantId))
                    throw new DomainRuleException(Rules.Rule03, $"Participant '{participantId}' is placed in more than one project.");
            }

            validated[projectId] = members.ToList().AsReadOnly();
        }

        _settlement = validated;
        SettledAt = settledAt;
    }

    /// <summary>Used by persistence to rehydrate a settlement that was validated before it was saved.</summary>
    public void RestoreSettlement(IReadOnlyDictionary<Guid, IReadOnlyList<Guid>> settlement, DateTime? settledAt)
    {
        _settlement = settlement.ToDictionary(kv => kv.Key, kv => kv.Value);
        SettledAt = settledAt;
    }

    public void ClearSettlement()
    {
        _settlement = [];
        SettledAt = null;
    }
}
