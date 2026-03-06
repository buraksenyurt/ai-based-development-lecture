using Gamepedia.Domain;

/*
Provider Namespace İzolasyonu

Her veritabanı sağlayıcısı kendi alt namespace'inde (Gamepedia.Data.Postgres) yer alır ve böylece;

- Postgres.GameRepository ve MongoDb.GameRepository aynı "GameRepository" adını taşıyabilirler.
- Hangi implementasyonun kullanıldığı yalnızca Composition Root'ta (Client tarafındaki Program.cs) belirlenir.
- Yeni bir provider eklemek (Oracle, SQL Server gibi) mevcut kod değiştirilmeden yapılabilir (OCP - Open Close Principle).

*/

namespace Gamepedia.Data.Postgres;

public class GameRepository : IGameRepository
{
    // NotImplementedException: Bu metodlar gerçek PostgreSQL bağlantısı gerektiren stub (taslak) metodlardır.
    // Üretim kodunda Npgsql veya Entity Framework Core (PostgreSQL provider) ile doldurulur.
    public void Delete(int gameId) => throw new NotImplementedException();
    public Game Get(int gameId)    => throw new NotImplementedException();
    public void Save(Game game)    => throw new NotImplementedException();
    public Game Update(Game game)  => throw new NotImplementedException();
}
