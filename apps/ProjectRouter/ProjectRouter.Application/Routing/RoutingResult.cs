namespace ProjectRouter.Application.Routing;

public sealed record Placement(Guid ParticipantId, Guid ProjectId, double Score);

public sealed class RoutingResult
{
    /// <summary>Project id → ids of the participants placed in that project. Only opened projects are included.</summary>
    public required IReadOnlyDictionary<Guid, IReadOnlyList<Guid>> Settlement { get; init; }
    public required IReadOnlyList<Placement> Placements { get; init; }
    public required IReadOnlyList<Guid> UnassignedParticipantIds { get; init; }

    /// <summary>Projects that were not opened (not enough participants to reach their minimum team size).</summary>
    public required IReadOnlyList<Guid> ClosedProjectIds { get; init; }

    public required IReadOnlyList<string> Warnings { get; init; }
}
