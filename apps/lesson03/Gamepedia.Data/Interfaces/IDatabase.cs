using Gamepedia.Domain;

namespace Gamepedia.Data;

/*
    "God Interface" Anti-Pattern

    IDatabase, oyun ve stüdyo verilerine ait tüm sorumluluğu tek bir arayüze yüklediği için
    bazı açılardan sorunlu olduğunu ifade edelim.

    1. Interface Segregation Principle (ISP) ihlali: Yalnızca oyunlarla ilgilenen bir sınıf IDatabase'i 
    implemente etmek zorunda kalabilir, oysa  oyun stüdyosu ile ilgili metodlarını hiç kullanmayacaktır.

    2. Tek Sorumluluk Prensibi (SRP) ihlali: Veritabanı kavramı ile iş alanı kavramları (Game, Studio) iç içe geçmiş durumda.

    3. Genişletilebilirlik sorunu: Yeni bir entity (örn. Publisher) eklendiğinde bu arayüz ve tüm implemente
        eden sınıflar değişmek zorunda kalabilir. (Open/Closed Principle ihlali)

    Tercih edilen yaklaşım: IGameRepository ve IStudioRepository gibi
    odaklanmış, sorumluluk ayrımına uygun repository arayüzleri kullanmak olur. 
    Dolayısıya bu arayüz aslında dışarıda tutulabilir veya tamamen silinebilir.
*/
public interface IDatabase
{
    IEnumerable<Game> GetAllGames();
    IEnumerable<Game> GetGamesByGenre(Genre genre);
    void SaveAll(IEnumerable<Game> games);
    IEnumerable<Game> GetGamesByStudio(Studio studio);
    IEnumerable<Studio> GetStudiosByGenre(string genre);
    void SaveAllStudios(IEnumerable<Studio> studios);
    void AddGameToStudio(int gameId, int studioId);
}
