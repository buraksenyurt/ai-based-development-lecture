using Microsoft.Extensions.DependencyInjection;
using ProjectRouter.Application.Interfaces;

namespace ProjectRouter.Data.Tests;

public class DataSetupTests : SqliteTestDatabase
{
    [Fact]
    public void AddData_RegistersRepositoriesAndInitializer()
    {
        var services = new ServiceCollection();
        services.AddData("Data Source=:memory:");

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        Assert.NotNull(provider.GetRequiredService<SqliteConnectionFactory>());
        Assert.NotNull(provider.GetRequiredService<DatabaseInitializer>());
        Assert.IsType<ParticipantRepository>(scope.ServiceProvider.GetRequiredService<IParticipantRepository>());
        Assert.IsType<ProjectIdeaRepository>(scope.ServiceProvider.GetRequiredService<IProjectIdeaRepository>());
        Assert.IsType<CompetitionRepository>(scope.ServiceProvider.GetRequiredService<ICompetitionRepository>());
    }

    [Fact]
    public void Initialize_IsIdempotent()
    {
        var initializer = new DatabaseInitializer(Factory);

        var ex = Record.Exception(() => initializer.Initialize());

        Assert.Null(ex);
    }

    [Fact]
    public void ConnectionFactory_EnablesForeignKeys()
    {
        using var connection = Factory.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA foreign_keys;";

        Assert.Equal(1L, command.ExecuteScalar());
    }
}
