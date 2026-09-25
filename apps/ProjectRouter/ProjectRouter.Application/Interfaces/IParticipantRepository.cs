using ProjectRouter.Domain;

namespace ProjectRouter.Application.Interfaces;

public interface IParticipantRepository
{
    IReadOnlyList<Participant> GetAll();
    Participant? GetById(Guid id);
    IReadOnlyList<Participant> GetByIds(IEnumerable<Guid> ids);
    void Save(Participant participant);
    void Delete(Guid id);
}
