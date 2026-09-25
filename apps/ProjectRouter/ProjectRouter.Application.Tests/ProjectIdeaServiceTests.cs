using Moq;
using ProjectRouter.Application.Exceptions;
using ProjectRouter.Application.Interfaces;
using ProjectRouter.Application.Services;

namespace ProjectRouter.Application.Tests;

public class ProjectIdeaServiceTests
{
    private readonly Mock<IProjectIdeaRepository> _projects = new();
    private readonly Mock<ICompetitionRepository> _competitions = new();

    private ProjectIdeaService CreateService() => new(_projects.Object, _competitions.Object);

    [Fact]
    public void GetAll_ReturnsRepositoryResult()
    {
        var project = TestData.Project(["C#"], ["SQLite"], 1, 3);
        _projects.Setup(r => r.GetAll()).Returns([project]);

        var result = CreateService().GetAll();

        Assert.Same(project, Assert.Single(result));
    }

    [Fact]
    public void Get_Existing_ReturnsProject()
    {
        var project = TestData.Project(["C#"], ["SQLite"], 1, 3);
        _projects.Setup(r => r.GetById(project.Id)).Returns(project);

        Assert.Same(project, CreateService().Get(project.Id));
    }

    [Fact]
    public void Get_Unknown_ThrowsNotFound()
    {
        var id = Guid.NewGuid();

        var ex = Assert.Throws<NotFoundException>(() => CreateService().Get(id));

        Assert.Contains(id.ToString(), ex.Message);
    }

    [Fact]
    public void Save_DelegatesToRepository()
    {
        var project = TestData.Project(["C#"], ["SQLite"], 1, 3);

        CreateService().Save(project);

        _projects.Verify(r => r.Save(project), Times.Once);
    }

    [Fact]
    public void Delete_WhenProjectIsInACompetition_Throws()
    {
        var id = Guid.NewGuid();
        _competitions.Setup(r => r.IsProjectReferenced(id)).Returns(true);

        Assert.Throws<InUseException>(() => CreateService().Delete(id));
        _projects.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void Delete_WhenProjectIsFree_Deletes()
    {
        var id = Guid.NewGuid();

        CreateService().Delete(id);

        _projects.Verify(r => r.Delete(id), Times.Once);
    }
}
