using ProjectRouter.Domain;

namespace ProjectRouter.Application.Routing;

public interface IProjectRouter
{
    RoutingResult Route(IReadOnlyCollection<Participant> participants, IReadOnlyCollection<ProjectIdea> projects);
}
