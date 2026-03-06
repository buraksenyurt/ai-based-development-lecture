using Gamepedia.Domain;

namespace Gamepedia.Data;

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
    void Save(Game game,DatabaseType databaseType)
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
