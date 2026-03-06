using Gamepedia.Data;
using Gamepedia.Domain;

namespace Gamepedia.ConsoleApp;

/// <summary>
/// Oyunları bellekte tutan ve konsola yazan mock repository.
/// Gerçek bir veri tabanı yerine test/demo amaçlı kullanılır.
/// </summary>
public class GameTextRepository : IGameRepository
{
    private readonly Dictionary<int, Game> _store = [];
    private int _nextId = 1;

    public void Save(Game game)
    {
        var id = _nextId++;
        _store[id] = game;
        Console.WriteLine($"[SAVED]   #{id} - {game.Title} ({game.ReleaseYear}) | Rating: {game.Rating} | Studio: {game.Studio.Name}");
    }

    public Game Get(int gameId)
    {
        if (!_store.TryGetValue(gameId, out var game))
            throw new KeyNotFoundException($"GameId={gameId} bulunamadı.");

        Console.WriteLine($"[FETCHED] #{gameId} - {game.Title}");
        return game;
    }

    public Game Update(Game game)
    {
        if (!_store.ContainsKey(game.GameId))
            throw new KeyNotFoundException($"GameId={game.GameId} bulunamadı.");

        _store[game.GameId] = game;
        Console.WriteLine($"[UPDATED] #{game.GameId} - {game.Title} | Yeni Rating: {game.Rating}");
        return game;
    }

    public void Delete(int gameId)
    {
        if (!_store.Remove(gameId))
            throw new KeyNotFoundException($"GameId={gameId} bulunamadı.");

        Console.WriteLine($"[DELETED] #{gameId}");
    }
}
