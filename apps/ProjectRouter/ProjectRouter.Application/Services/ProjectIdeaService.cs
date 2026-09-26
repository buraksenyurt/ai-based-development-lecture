using ProjectRouter.Application.Exceptions;
using ProjectRouter.Application.Interfaces;
using ProjectRouter.Domain;

namespace ProjectRouter.Application.Services;

public class ProjectIdeaService(IProjectIdeaRepository projects, ICompetitionRepository competitions)
{
    public IReadOnlyList<ProjectIdea> GetAll() => projects.GetAll();

    public ProjectIdea Get(Guid id) => projects.GetById(id) ?? throw new NotFoundException("Project", id);

    public void Save(ProjectIdea project) => projects.Save(project);

    public void Delete(Guid id)
    {
        if (competitions.IsProjectReferenced(id))
            throw new InUseException("This project is part of a competition. Remove it from the competition first.");

        projects.Delete(id);
    }
}
