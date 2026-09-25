using ProjectRouter.Application.Exceptions;
using ProjectRouter.Application.Interfaces;
using ProjectRouter.Application.Routing;
using ProjectRouter.Domain;

namespace ProjectRouter.Application.Services;

public class CompetitionService(
    ICompetitionRepository competitions,
    IParticipantRepository participants,
    IProjectIdeaRepository projects,
    IProjectRouter router,
    IMatchScorer scorer,
    TimeProvider timeProvider)
{
    public IReadOnlyList<Competition> GetAll() => competitions.GetAll();

    public Competition Get(Guid id) => competitions.GetById(id) ?? throw new NotFoundException("Competition", id);

    /// <summary>
    /// Saves the competition. When the project or participant list of an existing competition changes,
    /// its settlement is no longer valid and is cleared.
    /// </summary>
    public void Save(Competition competition)
    {
        var existing = competitions.GetById(competition.Id);
        if (existing is not null
            && existing.ProjectIds.ToHashSet().SetEquals(competition.ProjectIds)
            && existing.ParticipantIds.ToHashSet().SetEquals(competition.ParticipantIds))
        {
            competition.RestoreSettlement(existing.Settlement, existing.SettledAt);
        }

        competitions.Save(competition);
    }

    public void Delete(Guid id) => competitions.Delete(id);

    /// <summary>Runs the routing algorithm, validates the result against the domain rules and saves it.</summary>
    public RoutingResult RunRouting(Guid competitionId)
    {
        var competition = Get(competitionId);
        var competitionParticipants = participants.GetByIds(competition.ParticipantIds);
        var competitionProjects = projects.GetByIds(competition.ProjectIds);

        var result = router.Route(competitionParticipants, competitionProjects);

        competition.AssignSettlement(result.Settlement, competitionProjects, timeProvider.GetUtcNow().UtcDateTime);
        competitions.Save(competition);

        return result;
    }

    public void ClearSettlement(Guid competitionId)
    {
        var competition = Get(competitionId);
        competition.ClearSettlement();
        competitions.Save(competition);
    }

    public CompetitionDetails GetDetails(Guid competitionId)
    {
        var competition = Get(competitionId);
        var competitionParticipants = participants.GetByIds(competition.ParticipantIds).OrderBy(p => p.FullName).ToList();
        var competitionProjects = projects.GetByIds(competition.ProjectIds).OrderBy(p => p.Title).ToList();
        var participantLookup = competitionParticipants.ToDictionary(p => p.Id);

        var teams = competitionProjects
            .Where(project => competition.Settlement.ContainsKey(project.Id))
            .Select(project => new Team(
                project,
                competition.Settlement[project.Id]
                    .Where(participantLookup.ContainsKey)
                    .Select(id => new TeamMember(participantLookup[id], scorer.Score(participantLookup[id], project)))
                    .OrderByDescending(m => m.Score)
                    .ThenBy(m => m.Participant.FullName)
                    .ToList()))
            .ToList();

        var placed = competition.Settlement.Values.SelectMany(ids => ids).ToHashSet();

        return new CompetitionDetails
        {
            Competition = competition,
            Participants = competitionParticipants,
            Projects = competitionProjects,
            Teams = teams,
            UnassignedParticipants = competition.SettledAt is null
                ? []
                : competitionParticipants.Where(p => !placed.Contains(p.Id)).ToList(),
            ClosedProjects = competition.SettledAt is null
                ? []
                : competitionProjects.Where(p => !competition.Settlement.ContainsKey(p.Id)).ToList(),
        };
    }

    public SettlementDocument Export(Guid competitionId)
    {
        var competition = Get(competitionId);
        var settlement = competition.Settlement.ToDictionary(
            kv => kv.Key.ToString(),
            kv => (IReadOnlyList<string>)kv.Value.Select(id => id.ToString()).ToList());

        return new SettlementDocument(competition.Id.ToString(), competition.Title, competition.Session, [settlement]);
    }
}
