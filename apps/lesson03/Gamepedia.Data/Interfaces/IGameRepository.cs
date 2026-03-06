using Gamepedia.Domain;

namespace Gamepedia.Data;

/*
    Repository Pattern örneği

Repository, veri erişim mantığını iş katmanından ayıran bir soyutlama katmanıdır.
Application/Domain kodunun "veri nerede saklı?" sorusunu sormamasını sağlamak için kullanılır.

Arayüz (interface) kullanmanın avantajları şöyle sıralanabilir;

- Application katmanı somut bir sınıfa değil, bu sözleşmeye bağımlıdır (DIP - Dependency Inversion Principle).
- Test sırasında gerçek veri tabanı yerine sahte (mock/stub) implementasyon kullanılabilir.
- Postgres yerine MongoDb kullanmak için uygulama koduna dokunmamaza gerek kalmaz.
*/

public interface IGameRepository
{
    void Save(Game game);
    Game Get(int gameId);
    void Delete(int gameId);
    Game Update(Game game);
}
