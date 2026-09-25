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

        var scores = ScoreAll(orderedParticipants, orderedProjects);
        var openProjects = SelectProjectsToOpen(orderedParticipants, orderedProjects, scores);
        var assignment = new Assignment(openProjects, orderedParticipants, scores);

        // Step 3: reach the minimum team size of every opened project.
        assignment.Fill(p => p.Team.Min);

        // Step 4: place everyone else while capacity allows.
        assignment.Fill(p => p.Team.Max);

        var closedProjects = orderedProjects.Where(p => !assignment.IsOpen(p.Id)).ToList();

        return new RoutingResult
        {
            Settlement = assignment.Settlement(),
            Placements = assignment.Placements,
            UnassignedParticipantIds = assignment.Unassigned,
            ClosedProjectIds = closedProjects.Select(p => p.Id).ToList(),
            Warnings = BuildWarnings(assignment, closedProjects),
        };
    }

    private Dictionary<(Guid ParticipantId, Guid ProjectId), double> ScoreAll(List<Participant> participants, List<ProjectIdea> projects)
    {
        var scores = new Dictionary<(Guid ParticipantId, Guid ProjectId), double>();
        foreach (var participant in participants)
            foreach (var project in projects)
                scores[(participant.Id, project.Id)] = scorer.Score(participant, project);

        return scores;
    }

    private static List<string> BuildWarnings(Assignment assignment, List<ProjectIdea> closedProjects)
    {
        var warnings = new List<string>();

        if (assignment.Unassigned.Count > 0)
            warnings.Add($"{assignment.Unassigned.Count} participant(s) could not be placed: the opened projects have no remaining capacity.");

        foreach (var project in closedProjects)
            warnings.Add($"Project '{project.Title}' was not opened: there are not enough participants to reach its minimum team size of {project.Team.Min}.");

        var noMatchCount = assignment.Placements.Count(p => p.Score == 0);
        if (noMatchCount > 0)
            warnings.Add($"{noMatchCount} participant(s) were placed into a project that matches none of their preferences.");

        return warnings;
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

    /// <summary>Tracks who is placed where while the greedy steps run.</summary>
    private sealed class Assignment(
        List<ProjectIdea> openProjects,
        List<Participant> participants,
        Dictionary<(Guid ParticipantId, Guid ProjectId), double> scores)
    {
        private readonly Dictionary<Guid, List<Guid>> _members = openProjects.ToDictionary(p => p.Id, _ => new List<Guid>());
        private readonly List<Guid> _unassigned = participants.Select(p => p.Id).ToList();
        private readonly List<Placement> _placements = [];

        public List<Guid> Unassigned => _unassigned;
        public IReadOnlyList<Placement> Placements => _placements;

        public bool IsOpen(Guid projectId) => _members.ContainsKey(projectId);

        /// <summary>Repeatedly places the best scoring pair until no open project is below the given capacity.</summary>
        public void Fill(Func<ProjectIdea, int> capacity)
        {
            while (FindBestPair(capacity) is { } best)
            {
                _members[best.ProjectId].Add(best.ParticipantId);
                _unassigned.Remove(best.ParticipantId);
                _placements.Add(best);
            }
        }

        public Dictionary<Guid, IReadOnlyList<Guid>> Settlement() =>
            _members
                .Where(kv => kv.Value.Count > 0)
                .ToDictionary(kv => kv.Key, kv => (IReadOnlyList<Guid>)kv.Value.AsReadOnly());

        private Placement? FindBestPair(Func<ProjectIdea, int> capacity)
        {
            Placement? best = null;

            var projectsWithRoom = openProjects
                .Where(project => _members[project.Id].Count < capacity(project))
                .Select(project => project.Id);

            foreach (var projectId in projectsWithRoom)
            {
                foreach (var participantId in _unassigned)
                {
                    var score = scores[(participantId, projectId)];
                    if (best is null || score > best.Score)
                        best = new Placement(participantId, projectId, score);
                }
            }

            return best;
        }
    }
}
