using Gamepedia.Domain;

namespace Gamepedia.Data;

/*
    Interface Segregation Principle (ISP)
    
    IGameRepository ve IStudioRepository ayrı arayüzler olarak tasarlanmışlardır.
    Yalnızca oyunlarla çalışan bir servis IStudioRepository'ye bağımlı olmak zorunda değildir.

    Karşılaştırma ve sorgulama için IDatabase.cs ele alınabilir — 
    IDatabase arayüzü tüm sorumlulukları tek bir arayüze yükleyen kötü bir örnektir.
*/
public interface IStudioRepository
{
    void Save(Studio studio);
    void Delete(int studioId);
    Studio Get(int studioId);
    void AddGame(int studioId, Game game);
}
