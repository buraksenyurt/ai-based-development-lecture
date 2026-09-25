using ProjectRouter.Domain;

namespace ProjectRouter.Application.Interfaces;

public interface ICompetitionRepository
{
    IReadOnlyList<Competition> GetAll();
    Competition? GetById(Guid id);

    /// <summary>Saves the competition together with its project/participant lists and settlement.</summary>
    void Save(Competition competition);

    void Delete(Guid id);
    bool IsParticipantReferenced(Guid participantId);
    bool IsProjectReferenced(Guid projectId);
}
