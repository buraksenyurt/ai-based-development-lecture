using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectRouter.Domain;

namespace ProjectRouter.Web.Pages;

/// <summary>Helpers for the comma separated, ordered list inputs used by the forms.</summary>
public static class ListInput
{
    public static IReadOnlyList<string> Split(string? value) =>
        (value ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    public static string Join(IEnumerable<string> values) => string.Join(", ", values);

    public static void AddRuleError(this ModelStateDictionary modelState, DomainRuleException exception) =>
        modelState.AddModelError(string.Empty, exception.RuleCode == DomainRuleException.Validation
            ? exception.Message
            : $"[{exception.RuleCode}] {exception.Message}");
}
