using ProjectRouter.Domain;

namespace ProjectRouter.Application.Routing;

/// <summary>
/// Places participants into projects in four steps:
/// 1. Score every (participant, project) pair.
/// 2. Decide which projects to open: projects with the highest total demand first, as long as the sum of
///    minimum team sizes still fits the number of participants. Projects nobody is interested in are only
///    opened when extra capacity is needed.
/// 3. Fill every opened project up to its minimum team size (Rule 02) by repeatedly taking the best scoring pair.
/// 4. Place the remaining participants into their best project that is still below its maximum team size.
/// Ties are broken by id, so the same input always produces the same settlement.
/// Every participant ends up in at most one project (Rule 03).
/// </summary>
public class GreedyProjectRouter(IMatchScorer scorer) : IProjectRouter
{
    public RoutingResult Route(IReadOnlyCollection<Participant> participants, IReadOnlyCollection<ProjectIdea> projects)
    {
        var orderedParticipants = participants.OrderBy(p => p.Id).ToList();
        var orderedProjects = projects.OrderBy(p => p.Id).ToList();

        var scores = new Dictionary<(Guid ParticipantId, Guid ProjectId), double>();
        foreach (var participant in orderedParticipants)
            foreach (var project in orderedProjects)
                scores[(participant.Id, project.Id)] = scorer.Score(participant, project);

        var openProjects = SelectProjectsToOpen(orderedParticipants, orderedProjects, scores);
        var members = openProjects.ToDictionary(p => p.Id, _ => new List<Guid>());
        var unassigned = orderedParticipants.Select(p => p.Id).ToList();
        var placements = new List<Placement>();

        // Step 3: reach the minimum team size of every opened project.
        AssignBestPairs(openProjects, p => p.Team.Min);

        // Step 4: place everyone else while capacity allows.
        AssignBestPairs(openProjects, p => p.Team.Max);

        var warnings = new List<string>();
        var closedProjects = orderedProjects.Where(p => !members.ContainsKey(p.Id)).ToList();

        if (unassigned.Count > 0)
            warnings.Add($"{unassigned.Count} participant(s) could not be placed: the opened projects have no remaining capacity.");

        foreach (var project in closedProjects)
            warnings.Add($"Project '{project.Title}' was not opened: there are not enough participants to reach its minimum team size of {project.Team.Min}.");

        var noMatchCount = placements.Count(p => p.Score == 0);
        if (noMatchCount > 0)
            warnings.Add($"{noMatchCount} participant(s) were placed into a project that matches none of their preferences.");

        return new RoutingResult
        {
            Settlement = members
                .Where(kv => kv.Value.Count > 0)
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyList<Guid>)kv.Value.AsReadOnly()),
            Placements = placements,
            UnassignedParticipantIds = unassigned,
            ClosedProjectIds = closedProjects.Select(p => p.Id).ToList(),
            Warnings = warnings,
        };

        void AssignBestPairs(IReadOnlyList<ProjectIdea> candidates, Func<ProjectIdea, int> capacity)
        {
            while (unassigned.Count > 0)
            {
                Placement? best = null;

                foreach (var project in candidates)
                {
                    if (members[project.Id].Count >= capacity(project))
                        continue;

                    foreach (var participantId in unassigned)
                    {
                        var score = scores[(participantId, project.Id)];
                        if (best is null || score > best.Score)
                            best = new Placement(participantId, project.Id, score);
                    }
                }

                if (best is null)
                    return;

                members[best.ProjectId].Add(best.ParticipantId);
                unassigned.Remove(best.ParticipantId);
                placements.Add(best);
            }
        }
    }

    private static List<ProjectIdea> SelectProjectsToOpen(
        List<Participant> participants,
        List<ProjectIdea> projects,
        Dictionary<(Guid ParticipantId, Guid ProjectId), double> scores)
    {
        var byDemand = projects
            .Select(project => (Project: project, Demand: participants.Sum(p => scores[(p.Id, project.Id)])))
            .OrderByDescending(x => x.Demand)
            .ThenBy(x => x.Project.Id)
            .ToList();

        var open = new List<ProjectIdea>();
        var reservedMin = 0;

        // First pass: projects that at least one participant is interested in.
        foreach (var (project, demand) in byDemand)
        {
            if (demand > 0 && reservedMin + project.Team.Min <= participants.Count)
            {
                open.Add(project);
                reservedMin += project.Team.Min;
            }
        }

        // Second pass: open the remaining projects only while capacity is still missing.
        foreach (var (project, _) in byDemand)
        {
            if (open.Sum(p => p.Team.Max) >= participants.Count)
                break;

            if (!open.Contains(project) && reservedMin + project.Team.Min <= participants.Count)
            {
                open.Add(project);
                reservedMin += project.Team.Min;
            }
        }

        return open;
    }
}
