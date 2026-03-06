using Gamepedia.Data;
using Gamepedia.Domain;

namespace Gamepedia.ConsoleApp;

/*
Test Double: Stub

Bu sınıf basit bir Test Double (test yedeği) örneğidir. Yani daha spesifik ifade etmek gerekirse bir Stub'dır.
Test mühendisliği kapsamında Stub benzeri başka kavramlar da vardır. Bunları şöyle özetleyebiliriz.

Stub    :   Önceden belirlenmiş sabit yanıtlar döner; davranışı doğrulamaz.
Mock    :   Hangi metodların kaç kez çağrıldığını doğrular (davranış doğrulama).
Fake    :   Gerçekten çalışan ama basit bir implementasyon (örn. in-memory veri tabanı).
Spy     :   Gerçek nesne gibi çalışır ama çağrıları kaydeder.

GameTextRepository Stub + Fake karışımıdır. Gerçekten bellekte veri saklar (Fake) ama gerçek bir veri tabanı kullanmaz.

Gerçek hayat senaryolarında bu tür sınıflar ya test projesine taşınır ya da Moq gibi kütüphaneler ile dinamik olarak üretilir.
*/

/// <summary>
/// Oyunları bellekte tutan ve konsola yazan stub repository.
/// Gerçek bir veri tabanı yerine geliştirme/demo amaçlı kullanılır.
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
