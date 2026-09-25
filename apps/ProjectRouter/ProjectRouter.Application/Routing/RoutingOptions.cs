namespace ProjectRouter.Application.Routing;

public sealed class RoutingOptions
{
    /// <summary>Weight of the programming language match in the total score.</summary>
    public double LanguageWeight { get; set; } = 0.6;

    /// <summary>Weight of the database match in the total score.</summary>
    public double DatabaseWeight { get; set; } = 0.4;
}
