using ProjectRouter.Domain;

namespace ProjectRouter.Application.Routing;

public interface IMatchScorer
{
    /// <summary>Returns how well a project fits a participant's preferences, between 0 and 1.</summary>
    double Score(Participant participant, ProjectIdea project);
}
