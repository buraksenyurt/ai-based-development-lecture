using ProjectRouter.Domain;

namespace ProjectRouter.Application.Routing;

/// <summary>
/// Scores a participant/project pair by the participant's ordered preferences.
/// The first preference that the project uses determines the partial score: 1 for the 1st choice,
/// 1/2 for the 2nd, 1/3 for the 3rd and so on (0 when nothing matches).
/// total = LanguageWeight * languageScore + DatabaseWeight * databaseScore, normalized to [0, 1].
/// </summary>
public class PreferenceMatchScorer(RoutingOptions options) : IMatchScorer
{
    public double Score(Participant participant, ProjectIdea project)
    {
        var languageScore = RankScore(participant.Languages, project.TechStack.Languages, TechnologyCatalog.LanguagesMatch);
        var databaseScore = RankScore(participant.Databases, project.TechStack.Databases, TechnologyCatalog.DatabasesMatch);

        var totalWeight = options.LanguageWeight + options.DatabaseWeight;
        if (totalWeight <= 0)
            return 0;

        return (options.LanguageWeight * languageScore + options.DatabaseWeight * databaseScore) / totalWeight;
    }

    private static double RankScore(IReadOnlyList<string> preferences, IReadOnlyList<string> offered, Func<string, string, bool> matches)
    {
        for (var rank = 0; rank < preferences.Count; rank++)
        {
            if (offered.Any(item => matches(preferences[rank], item)))
                return 1.0 / (rank + 1);
        }

        return 0;
    }
}
