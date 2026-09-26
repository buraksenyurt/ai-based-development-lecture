using Moq;
using ProjectRouter.Application.Interfaces;
using ProjectRouter.Application.Services;

namespace ProjectRouter.Application.Tests;

public class ParticipantServiceQueryTests
{
    private readonly Mock<IParticipantRepository> _participants = new();
    private readonly Mock<ICompetitionRepository> _competitions = new();

    private ParticipantService CreateService() => new(_participants.Object, _competitions.Object);

    [Fact]
    public void GetAll_ReturnsRepositoryResult()
    {
        var participant = TestData.Participant(["C#"], ["SQLite"]);
        _participants.Setup(r => r.GetAll()).Returns([participant]);

        Assert.Same(participant, Assert.Single(CreateService().GetAll()));
    }

    [Fact]
    public void Get_Existing_ReturnsParticipant()
    {
        var participant = TestData.Participant(["C#"], ["SQLite"]);
        _participants.Setup(r => r.GetById(participant.Id)).Returns(participant);

        Assert.Same(participant, CreateService().Get(participant.Id));
    }

    [Fact]
    public void Save_DelegatesToRepository()
    {
        var participant = TestData.Participant(["C#"], ["SQLite"]);

        CreateService().Save(participant);

        _participants.Verify(r => r.Save(participant), Times.Once);
    }
}
