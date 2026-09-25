using ProjectRouter.Domain;

namespace ProjectRouter.Application.Tests;

internal static class TestData
{
    private static int _counter;

    public static Participant Participant(string[] languages, string[] databases, string? name = null)
    {
        var n = Interlocked.Increment(ref _counter);
        return new Participant(
            name ?? $"Participant {n}",
            $"participant{n}@example.com",
            "University",
            "Software Engineering",
            3,
            null,
            languages,
            databases);
    }

    public static ProjectIdea Project(string[] languages, string[] databases, int min, int max, string? title = null) =>
        new(
            title ?? $"Project {Interlocked.Increment(ref _counter)}",
            "Summary",
            new TechStack(languages, ["web"], databases),
            new TeamSize(min, max),
            ProjectSize.M,
            [],
            []);
}
