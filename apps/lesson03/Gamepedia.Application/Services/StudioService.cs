using Gamepedia.Application.Interfaces;
using Gamepedia.Data;
using Gamepedia.Domain;

namespace Gamepedia.Application.Services;

public class StudioService(IStudioRepository studioRepository, IGameRepository gameRepository) : IStudioService
{
    private readonly IStudioRepository _studioRepository = studioRepository;
    private readonly IGameRepository _gameRepository = gameRepository;

    /// <summary>
    /// Yeni bir stüdyoyu doğrulayarak kaydeder.
    /// </summary>
    public void RegisterStudio(Studio studio)
    {
        if (string.IsNullOrWhiteSpace(studio.Name))
            throw new ArgumentException("Stüdyo adı boş olamaz.", nameof(studio));

        _studioRepository.Save(studio);
    }

    /// <summary>
    /// Mevcut bir oyunu ilgili stüdyoyla ilişkilendirir.
    /// </summary>
    public void AssignGameToStudio(int gameId, int studioId)
    {
        var game = _gameRepository.Get(gameId);
        _studioRepository.Get(studioId); // stüdyo var mı doğrular
        _studioRepository.AddGame(studioId, game);
    }
}
