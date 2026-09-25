namespace ProjectRouter.Domain;

/// <summary>
/// Thrown when a domain rule is violated. RuleCode refers to the rules in docs/project-router.md
/// (e.g. "Rule 00") or to a general validation ("Validation").
/// </summary>
public class DomainRuleException(string ruleCode, string message) : Exception(message)
{
    public const string Validation = "Validation";

    public string RuleCode { get; } = ruleCode;
}
