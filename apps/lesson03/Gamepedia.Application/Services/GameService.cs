using Gamepedia.Application.Interfaces;
using Gamepedia.Data;
using Gamepedia.Domain;

namespace Gamepedia.Application.Services;

// GameService(IGameRepository gameRepository) ifadesi tipik bir Primary Constructor sözdizimidir.
// Primary Constructor ile parametre doğrudan alan tanımında kullanılabilir.
// Dependency Injection framework'ü (örn. Microsoft.Extensions.DependencyInjection)
// bu yapıcı metodu(Constructor) otomatik olarak çağırır ve IGameRepository implementasyonunu enjekte eder.
public class GameService(IGameRepository gameRepository) : IGameService
{
    private readonly IGameRepository _gameRepository = gameRepository;

    /// <summary>
    /// Yeni bir oyunu doğrulayarak kaydeder.
    /// </summary>
    public void RegisterGame(Game game)
    {
        // Guard Clause (Koruma Cümlesi) Paterni:
        // Geçersiz durumları metodun başında erken döndürme veya exception ile ele alır.
        // Derin if-else zinciri yerine düz, okunabilir akış sağlar.
        if (string.IsNullOrWhiteSpace(game.Title))
            throw new ArgumentException("Oyun başlığı boş olamaz.", nameof(game));

        if (game.Rating is < 0 or > 10)
            throw new ArgumentOutOfRangeException(nameof(game), "Oyun puanı 0 ile 10 arasında olmalıdır.");

        _gameRepository.Save(game);
    }

    /// <summary>
    /// Mevcut bir oyunun puanını günceller.
    /// </summary>
    public void UpdateGameRating(int gameId, float newRating)
    {
        if (newRating is < 0 or > 10)
            throw new ArgumentOutOfRangeException(nameof(newRating), "Puan 0 ile 10 arasında olmalıdır.");

        var game = _gameRepository.Get(gameId);
        game.Rating = newRating;
        _gameRepository.Update(game);
    }
}
