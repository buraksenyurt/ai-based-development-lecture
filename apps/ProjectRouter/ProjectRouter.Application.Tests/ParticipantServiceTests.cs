using Moq;
using ProjectRouter.Application.Exceptions;
using ProjectRouter.Application.Interfaces;
using ProjectRouter.Application.Services;

namespace ProjectRouter.Application.Tests;

public class ParticipantServiceTests
{
    private readonly Mock<IParticipantRepository> _participants = new();
    private readonly Mock<ICompetitionRepository> _competitions = new();

    [Fact]
    public void Delete_WhenParticipantIsInACompetition_Throws()
    {
        var id = Guid.NewGuid();
        _competitions.Setup(r => r.IsParticipantReferenced(id)).Returns(true);
        var service = new ParticipantService(_participants.Object, _competitions.Object);

        Assert.Throws<InUseException>(() => service.Delete(id));
        _participants.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public void Delete_WhenParticipantIsFree_Deletes()
    {
        var id = Guid.NewGuid();
        var service = new ParticipantService(_participants.Object, _competitions.Object);

        service.Delete(id);

        _participants.Verify(r => r.Delete(id), Times.Once);
    }

    [Fact]
    public void Get_Unknown_ThrowsNotFound()
    {
        var service = new ParticipantService(_participants.Object, _competitions.Object);

        Assert.Throws<NotFoundException>(() => service.Get(Guid.NewGuid()));
    }
}
