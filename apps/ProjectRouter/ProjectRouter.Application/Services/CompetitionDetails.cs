using ProjectRouter.Domain;

namespace ProjectRouter.Application.Services;

public sealed record TeamMember(Participant Participant, double Score);

public sealed record Team(ProjectIdea Project, IReadOnlyList<TeamMember> Members);

public sealed class CompetitionDetails
{
    public required Competition Competition { get; init; }
    public required IReadOnlyList<Participant> Participants { get; init; }
    public required IReadOnlyList<ProjectIdea> Projects { get; init; }

    /// <summary>Teams of the current settlement (empty when routing has not been run yet).</summary>
    public required IReadOnlyList<Team> Teams { get; init; }

    public required IReadOnlyList<Participant> UnassignedParticipants { get; init; }
    public required IReadOnlyList<ProjectIdea> ClosedProjects { get; init; }

    public bool IsSettled => Competition.SettledAt is not null;
}

/// <summary>Export shape that follows the Competition example in docs/project-router.md.</summary>
public sealed record SettlementDocument(
    string Id,
    string Title,
    string Session,
    IReadOnlyList<IReadOnlyDictionary<string, IReadOnlyList<string>>> Settlement);
