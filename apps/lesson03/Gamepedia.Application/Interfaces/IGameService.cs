using Gamepedia.Domain;

namespace Gamepedia.Application.Interfaces;

public interface IGameService
{
    void RegisterGame(Game game);
    void UpdateGameRating(int gameId, float newRating);
}
