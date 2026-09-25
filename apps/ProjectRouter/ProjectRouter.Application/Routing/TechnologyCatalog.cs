namespace ProjectRouter.Application.Routing;

/// <summary>
/// Normalizes technology names so that "C#", "csharp" or "Postgres", "PostgreSQL" are treated as the same thing,
/// and knows the category (SQL / NoSQL) of common databases for wildcard preferences such as "NoSQL-*".
/// </summary>
public static class TechnologyCatalog
{
    private const string WildcardSuffix = "-*";

    private static readonly Dictionary<string, string> Aliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["csharp"] = "c#",
        ["c-sharp"] = "c#",
        ["js"] = "javascript",
        ["ts"] = "typescript",
        ["golang"] = "go",
        ["py"] = "python",
        ["pyhton"] = "python",
        ["postgres"] = "postgresql",
        ["mssql"] = "sqlserver",
        ["mongo"] = "mongodb",
        ["rust"] = "rust",
        ["rs"] = "rust",
        ["c++"] = "c++",
        ["cpp"] = "c++"
    };

    private static readonly Dictionary<string, string> DatabaseCategories = new(StringComparer.OrdinalIgnoreCase)
    {
        ["sqlserver"] = "sql",
        ["postgresql"] = "sql",
        ["mysql"] = "sql",
        ["mariadb"] = "sql",
        ["sqlite"] = "sql",
        ["oracle"] = "sql",
        ["mongodb"] = "nosql",
        ["redis"] = "nosql",
        ["cassandra"] = "nosql",
        ["couchdb"] = "nosql",
        ["couchbase"] = "nosql",
        ["dynamodb"] = "nosql",
        ["cosmosdb"] = "nosql",
        ["ravendb"] = "nosql",
        ["neo4j"] = "nosql",
        ["elasticsearch"] = "nosql",
        ["firebase"] = "nosql",
    };

    /// <summary>Lower-cases, removes whitespace and resolves known aliases.</summary>
    public static string Normalize(string value)
    {
        var normalized = string.Concat(value.Where(c => !char.IsWhiteSpace(c))).ToLowerInvariant();
        return Aliases.TryGetValue(normalized, out var canonical) ? canonical : normalized;
    }

    public static bool LanguagesMatch(string left, string right) => Normalize(left) == Normalize(right);

    /// <summary>
    /// Two databases match when they are the same product, or when one side is a category wildcard
    /// (e.g. "NoSQL-*") and the other side belongs to that category (or is the same wildcard).
    /// </summary>
    public static bool DatabasesMatch(string left, string right)
    {
        var a = Normalize(left);
        var b = Normalize(right);

        if (a == b)
            return true;

        return MatchesWildcard(a, b) || MatchesWildcard(b, a);
    }

    private static bool MatchesWildcard(string maybeWildcard, string database)
    {
        if (!maybeWildcard.EndsWith(WildcardSuffix, StringComparison.Ordinal))
            return false;

        var category = maybeWildcard[..^WildcardSuffix.Length];
        return DatabaseCategories.TryGetValue(database, out var databaseCategory) && databaseCategory == category;
    }
}
