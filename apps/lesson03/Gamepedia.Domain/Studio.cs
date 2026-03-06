namespace Gamepedia.Domain;

/*
Aggregate Root Adayı

Studio sınıfı kendisine ait oyunları (Games) barındırmakta. DDD-Domain Driven Design terminolojisinden olaya bakarsak
bu ilişki "Aggregate" olarak modellenebilir: Studio, Aggregate Root ile içerdiği Game, Aggregate rolünü üstlenir.
Aggregate Root üzerinden geçilmeden çocuk nesnelere doğrudan erişilmemesi sağlanabilir ve bu da tutarlılığı (consistency) garanti eder.
*/
public class Studio
{
    public int StudioId { get; }
    public required string Name { get; set; }

    // IEnumerable kullanmak okuma amaçlı erişimi temsil eder.
    // List<T> kullanılsaydı dışarıdan Add/Remove çağrılabilirdi ve kapsülleme bozulurdu.
    // Koleksiyona eleman eklemek için AddGame gibi bir domain metodu tanımlanmalıdır.
    public IEnumerable<Game> Games { get; set; } = [];

    // Daha birçok özellik eklenebilir, örneğin: Kuruluş Yılı, Kurucular, 
}
