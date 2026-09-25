# 02 - Dapper ve SQLite

## Karar

Veri erişimi için Dapper ve `Microsoft.Data.Sqlite` kullanıldı. Şema `ProjectRouter.Data/Schema.sql` dosyasındadır ve uygulama açılırken `DatabaseInitializer` tarafından `CREATE TABLE IF NOT EXISTS` ile uygulanır.

## Detaylar

- **Id'ler** `TEXT` olarak (`Guid.ToString()`) saklanır ve okunurken `Guid.Parse` ile çevrilir. Bu sayede Dapper'a özel bir tip dönüştürücü (type handler) yazmak gerekmez.
- **Sıralı listeler** (dil/veritabanı tercihleri, proje teknolojileri) ayrı tablolarda `rank` kolonu ile tutulur. Rule 00/01'deki tercih sırası bu kolonla korunur.
- **Kayıt işlemleri** `INSERT ... ON CONFLICT DO UPDATE` ile upsert yapar. Alt listeler aynı transaction içinde silinip yeniden yazılır.
- **Foreign key** desteği SQLite'ta varsayılan olarak kapalıdır. `SqliteConnectionFactory` her bağlantıda bu desteği açar.
- **Rule 03** `settlements` tablosundaki `(competition_id, participant_id)` birincil anahtarıyla veritabanı seviyesinde de garanti edilir.
