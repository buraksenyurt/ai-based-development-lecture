using Gamepedia.Application.Services;
using Gamepedia.Data;
using Gamepedia.Domain;

namespace Gamepedia.ConsoleApp;

public class Program
{
    static void Main()
    {
        IGameRepository gameRepository = new GameTextRepository();
        var gameService = new GameService(gameRepository);

        gameService.RegisterGame(new Game
            {
                Title = "The Legend of Zelda: Breath of the Wild",
                Genre = Genre.Action,
                ReleaseYear = 2019,
                Rating = 8.7f,
                Summary = "",
                Studio = new Studio { Name = "Nintendo" }
            }
        );
    }
}
