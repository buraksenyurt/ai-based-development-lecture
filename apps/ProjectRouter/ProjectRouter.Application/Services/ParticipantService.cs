using ProjectRouter.Application.Exceptions;
using ProjectRouter.Application.Interfaces;
using ProjectRouter.Domain;

namespace ProjectRouter.Application.Services;

public class ParticipantService(IParticipantRepository participants, ICompetitionRepository competitions)
{
    public IReadOnlyList<Participant> GetAll() => participants.GetAll();

    public Participant Get(Guid id) => participants.GetById(id) ?? throw new NotFoundException("Participant", id);

    public void Save(Participant participant) => participants.Save(participant);

    public void Delete(Guid id)
    {
        if (competitions.IsParticipantReferenced(id))
            throw new InUseException("This participant is part of a competition. Remove them from the competition first.");

        participants.Delete(id);
    }
}
