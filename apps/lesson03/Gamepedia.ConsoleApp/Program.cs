using Gamepedia.Application.Services;
using Gamepedia.Data;
using Gamepedia.Domain;

/*
    Composition Root
    
    Program.cs, uygulamanın "Composition Root"udur: tüm bağımlılıkların birbirine
    bağlandığı tek yerdir. Hangi IGameRepository implementasyonunun kullanılacağına burada karar verilir.

    Bu sayede:
    - GameService, GameTextRepository'yi tanımaz; yalnızca IGameRepository'yi bilir.
    - Postgres'e geçmek için yalnızca bu satırı değiştirmek yeterlidir: IGameRepository gameRepository = new Postgres.GameRepository();

    Büyük projelerde bu bağlama işi Microsoft.Extensions.DependencyInjection,
    Autofac veya benzeri bir IoC (Inversion of Control) container'ı ile yapılır.
*/

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
