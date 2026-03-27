# ADR-05: PostgreSQL Veritabanı Seçimi

## Durum

Kabul Edildi *(Accepted)*

## Bağlam

Uygulamanın kitap verilerini kalıcı olarak depolaması gerekmektedir. Kalıcılık için kullanılacak veritabanı yönetim sistemi seçilmelidir. Başlıca adaylar şunlardır:

- **PostgreSQL:** Açık kaynak, ACID uyumlu ilişkisel veritabanı; kurumsal ölçekte üretim kullanımına uygundur.
- **SQL Server:** Microsoft'un ilişkisel veritabanı; .NET ekosistemiyle derin entegrasyona sahiptir ancak lisanslama maliyeti içerir.
- **SQLite:** Sunucusuz, dosya tabanlı ilişkisel veritabanı; geliştirme ve test ortamları için idealdir, ancak eş zamanlı yük altında yetersiz kalabilir.

## Karar

Veritabanı olarak **PostgreSQL** kullanılmasına karar verilmiştir. .NET bağlantısı için resmi sürücü olan **Npgsql** tercih edilmiştir.

```xml
<PackageReference Include="Npgsql" Version="10.0.2" />
```

`BookRepository`, her işlemde yeni bir `NpgsqlConnection` açar:

```csharp
using var connection = new NpgsqlConnection(connectionString);
connection.Open();
```

Veri modeli, PostgreSQL'in `UUID` ve `DECIMAL` türleriyle uyumlu şekilde tasarlanmıştır:

```sql
INSERT INTO books (book_id, title, author, price, page_count)
VALUES (@BookId, @Title, @Author, @Price, @PageCount)
```

## Sonuçlar

**Avantajlar:**
- Ücretsiz ve açık kaynak; ek lisans maliyeti yoktur.
- ACID uyumluluğu ve güçlü veri bütünlüğü garantisi sunar.
- `ON CONFLICT DO UPDATE` *(UPSERT)* gibi gelişmiş SQL özelliklerini destekler.
- Docker ile kolayca çalıştırılabilir; yerel geliştirme ve CI/CD ortamlarına uyumludur.
- .NET 10 ile Npgsql 10 arasında tam uyumluluk mevcuttur.

**Değiş tokuşlar:**
- Farklı bir veritabanına *(örn. SQL Server)* geçiş yapılmak istenirse bağlantı dizesi ve Npgsql'e özgü türlerin değiştirilmesi gerekir.
- PostgreSQL'e özgü `ON CONFLICT` sözdizimi diğer veritabanı yönetim sistemlerine taşınabilir değildir.
