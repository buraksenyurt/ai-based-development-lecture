namespace ProjectRouter.Domain;

/// <summary>Rule codes defined in docs/project-router.md.</summary>
public static class Rules
{
    /// <summary>A participant must prefer at least one programming language, ordered first to last choice.</summary>
    public const string Rule00 = "Rule 00";

    /// <summary>Rule 00 also applies to database preferences.</summary>
    public const string Rule01 = "Rule 01";

    /// <summary>A project's team size must be within its min-max range.</summary>
    public const string Rule02 = "Rule 02";

    /// <summary>A participant can be part of only one project.</summary>
    public const string Rule03 = "Rule 03";

    /// <summary>A project must use at least one programming language and at least one database.</summary>
    public const string Rule04 = "Rule 04";
}
