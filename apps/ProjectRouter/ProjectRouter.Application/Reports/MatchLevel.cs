namespace ProjectRouter.Application.Reports;

/// <summary>How well a placement matches the participant's preferences. Shared by the UI badges and the exports.</summary>
public enum MatchLevel
{
    None,
    Partial,
    Strong,
}

public static class MatchLevels
{
    /// <summary>Scores at or above this value count as a strong match.</summary>
    public const double StrongThreshold = 0.75;

    public static MatchLevel Classify(double score)
    {
        if (score >= StrongThreshold)
            return MatchLevel.Strong;

        return score > 0 ? MatchLevel.Partial : MatchLevel.None;
    }
}
