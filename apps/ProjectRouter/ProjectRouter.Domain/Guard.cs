namespace ProjectRouter.Domain;

internal static class Guard
{
    /// <summary>
    /// Trims entries, drops blanks and rejects duplicates while keeping the given order
    /// (the order is the preference order: first item = first choice).
    /// </summary>
    public static IReadOnlyList<string> OrderedList(IEnumerable<string>? values, string ruleCode, string fieldName, bool required)
    {
        var result = new List<string>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var raw in values ?? [])
        {
            var value = raw?.Trim();
            if (string.IsNullOrEmpty(value))
                continue;

            if (!seen.Add(value))
                throw new DomainRuleException(ruleCode, $"The {fieldName} list contains a duplicate entry: '{value}'.");

            result.Add(value);
        }

        if (required && result.Count == 0)
            throw new DomainRuleException(ruleCode, $"At least one {fieldName} must be specified.");

        return result.AsReadOnly();
    }

    public static string Required(string? value, string fieldName, int? maxLength = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainRuleException(DomainRuleException.Validation, $"{fieldName} cannot be empty.");

        var trimmed = value.Trim();
        if (maxLength is not null && trimmed.Length > maxLength)
            throw new DomainRuleException(DomainRuleException.Validation, $"{fieldName} cannot be longer than {maxLength} characters.");

        return trimmed;
    }
}
