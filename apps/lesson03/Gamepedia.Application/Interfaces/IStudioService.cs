using Gamepedia.Domain;

namespace Gamepedia.Application.Interfaces;

public interface IStudioService
{
    void RegisterStudio(Studio studio);
    void AssignGameToStudio(int gameId, int studioId);
}
