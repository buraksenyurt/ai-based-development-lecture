using Gamepedia.Data;
using Gamepedia.Domain;

namespace Gamepedia.ConsoleApp;

// Text dosysaya yazan dummy Repository
public class GameTextRepository : IGameRepository
{
    public void Delete(int gameId)
    {
        throw new NotImplementedException();
    }
    public Game Get(int gameId)
    {
        throw new NotImplementedException();
    }
    public void Save(Game game)
    {
        Console.WriteLine($"Saving {game.Title} to text file");
    }
    public Game Update(Game game)
    {
        throw new NotImplementedException();
    }
}
public class Program
{
    static void Main()
    {
        // var gamepedia = new GamepediaApp(new Data.Postgres.GameRepository());
        var gamepedia = new GamepediaApp(new GameTextRepository());
        gamepedia.SaveGame(new Game
            {
                Title = "The Legend of Zelda: Breath of the Wild",
                Genre = Genre.Action,
                ReleaseYear = 2019,
                Rating = 8.7f,
                Summary = "",
                Studio = new Studio
                {
                    Name = "Nintendo"
                }
            }
        );
    }
}

public class GamepediaApp(IGameRepository gameRepository)
{
    private readonly IGameRepository _gameRepository = gameRepository;

    public void SaveGame(Game game)
    {
        _gameRepository.Save(game);
    }
}
