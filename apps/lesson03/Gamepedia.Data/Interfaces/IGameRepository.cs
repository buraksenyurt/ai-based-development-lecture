using Gamepedia.Domain;

namespace Gamepedia.Data;

public interface IGameRepository
{
    void Save(Game game);
    Game Get(int gameId);
    void Delete(int gameId);
    Game Update(Game game);
}
