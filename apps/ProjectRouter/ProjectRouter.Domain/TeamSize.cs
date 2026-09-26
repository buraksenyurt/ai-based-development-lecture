namespace ProjectRouter.Domain;

/// <summary>Allowed team size range of a project (Rule 02).</summary>
public readonly record struct TeamSize
{
    public int Min { get; }
    public int Max { get; }

    public TeamSize(int min, int max)
    {
        if (min < 1)
            throw new DomainRuleException(Rules.Rule02, "Minimum team size must be at least 1.");

        if (max < min)
            throw new DomainRuleException(Rules.Rule02, "Maximum team size cannot be less than the minimum team size.");

        Min = min;
        Max = max;
    }

    public bool Allows(int memberCount) => memberCount >= Min && memberCount <= Max;

    public override string ToString() => $"{Min}-{Max}";
}
