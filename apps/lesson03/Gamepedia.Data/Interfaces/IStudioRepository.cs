using Gamepedia.Domain;

namespace Gamepedia.Data;

public interface IStudioRepository
{
    void Save(Studio studio);
    void Delete(int studioId);
    Studio Get(int studioId);
    void AddGame(int studioId, Game game);
}
