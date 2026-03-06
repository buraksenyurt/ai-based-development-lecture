using Gamepedia.Domain;

namespace Gamepedia.Data;

/*
Bu sınıf kasıtlı olarak kötü bir tasarımı temsil etmek üzere projeye dahil edilmiştir. Aşağıdaki anti-patternler sergilenmektedir:

- "Switch on Type" / Enum Dispatch Anti-Paterni: Hangi veritabanının kullanılacağına karar vermek için bir enum ve switch ifadesi
     kullanılmakta. Yeni bir veritabanı eklendiğinde enum'a yeni bir değer ve switch'e yeni bir case bloğu eklemek gerekir 
     ki bu Open/Closed prensibinin ihlalidir.

- Soyutlama Eksikliği (No Abstraction / No Polymorphism): Farklı veritabanı davranışlarını ayrı sınıflara (strateji) 
    dağıtmak yerine tek bir metodun içinde koşullu mantıkla yönetilmekte.

- Bağımlılıkların Tersine Çevrilmesi İhlali (DIP): Sınıf, somut veritabanı türlerine doğrudan bağımlı. Soyut bir arayüze bağımlı değil.
  Bırakın enjeksiyonu, türü bile runtime'da parametre olarak almak zorunda kalıyor.

- Test Edilemezlik: Veritabanı türü metoda parametre olarak verildiği için davranışı bağımsız olarak test etmek son derece güç olur.
Şöyle düşünelim, fonksiyonelliğin çalışmasını test etmek için gerçekten veritabanına gitmemiz gerek olmadan hareket etmek istersek bu bağımlılık
ayağımıza dolanabilir.

Doğru yaklaşım: IGameRepository arayüzünü implemente eden bağımsız sınıflar (Postgres.GameRepository, MongoDb.GameRepository vb.) 
oluşturmak ve bağımlılığı dışarıdan enjekte etmektir (Dependency Injection).
*/

public enum DatabaseType
{
    Postgres,
    MongoDb,
    SqlServer,
    Oracle,
}

public class BadGameRepository
{
    // public bool PostgresOrMongoDb { get; set; } = true;
    // Yarın karar değiştirdik ve verileri SQL Server'da kullanmak isteyen bir müşteri geldi?

    // Yarın yeni bir db gelirse ne yapacağız? Oracle, MySql, CosmosDb, Apache Cassandra, Redis, Neo4j, Amazon DynamoDB, Google Cloud Spanner, Microsoft Azure Cosmos DB, IBM Db2, SAP HANA, Oracle NoSQL Database, Couchbase, MarkLogic, ArangoDB, RavenDB, OrientDB, Apache HBase, Amazon Neptune, Google Cloud Bigtable etc...
    void Save(Game game, DatabaseType databaseType)
    {
        switch (databaseType)
        {
            case DatabaseType.Postgres:
                Console.WriteLine("Saving to postgres");
                break;
            case DatabaseType.MongoDb:
                Console.WriteLine("Saving to MongoDb");
                break;
            case DatabaseType.Oracle:
                Console.WriteLine("Saving to Oracle");
                break;
            default:
                Console.WriteLine("Saving to SqlServer");
                break;
        }

        //if (PostgresOrMongoDb)
        //{
        //    var postgresRepo = new Postgres.GameRepository();
        //    postgresRepo.Save(game);
        //}
        //else
        //{
        //    var mongoRepo = new MongoDb.GameRepository();
        //    mongoRepo.Save(game);
        //}
    }
}
