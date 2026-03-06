using Gamepedia.Domain;

namespace Gamepedia.Data;

public interface IDatabase
{
    IEnumerable<Game> GetAllGames();
    IEnumerable<Game> GetGamesByGenre(Genre genre);
    void SaveAll(IEnumerable<Game> games);
    IEnumerable<Game> GetGamesByStudio(Studio studio);
    IEnumerable<Studio> GetStudiosByGenre(string genre);
    void SaveAllStudios(IEnumerable<Studio> studios);
    void AddGameToStudio(int gameId, int studioId);
}
