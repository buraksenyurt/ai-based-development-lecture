using ProjectRouter.Domain;

namespace ProjectRouter.Application.Interfaces;

public interface IProjectIdeaRepository
{
    IReadOnlyList<ProjectIdea> GetAll();
    ProjectIdea? GetById(Guid id);
    IReadOnlyList<ProjectIdea> GetByIds(IEnumerable<Guid> ids);
    void Save(ProjectIdea project);
    void Delete(Guid id);
}
