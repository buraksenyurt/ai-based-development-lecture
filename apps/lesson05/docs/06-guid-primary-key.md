# ADR-06: GUID Birincil Anahtar Stratejisi

## Durum

Kabul Edildi *(Accepted)*

## Bağlam

Varlıklar için birincil anahtar *(primary key)* seçimi önemli bir mimari karardır. İki temel yaklaşım değerlendirilmiştir:

- **Tamsayı *(int / long)*:** Veritabanı tarafından otomatik artırılan sıralı kimlik. Küçük, okunması kolay ve dizinleme açısından verimlidir; ancak sıralı yapısı nedeniyle tahmin edilebilir olup dağıtık sistemlerde çakışma riski taşır.
- **GUID *(Globally Unique Identifier)*:** Merkezi koordinasyon gerektirmeden üretilen, küresel düzeyde benzersiz 128-bit tanımlayıcı.

## Karar

`Book` varlığının birincil anahtarı olarak `Guid` türü kullanılmasına karar verilmiştir. `BookId` değeri uygulama tarafında üretilir ve veritabanına birincil anahtar olarak iletilir.

```csharp
public Guid BookId { get; private set; }

public Book(Guid bookId, string title, string author, decimal price, int pageCount)
{
    BookId = bookId;
    // ...
}
```

```csharp
// Çağıran tarafta
var bookId = Guid.NewGuid();
_bookService.AddBook(bookId, "Clean Code", "Robert C. Martin", 39.99m, 464);
```

Veritabanı sütun türü `UUID` olarak tanımlanmıştır:

```sql
INSERT INTO books (book_id, ...) VALUES (@BookId, ...)
```

## Sonuçlar

**Avantajlar:**
- Kimlik, veritabanına gitmeden uygulama tarafında üretilebilir; bu durum dağıtık ortamlarda ve çevrimdışı senaryolarda avantaj sağlar.
- Sıralı tamsayıların aksine GUID'ler tahmin edilemez; kayıt kimliği üçüncü taraflara ifşa edilse bile sıradaki kaydın kimliği kestirilemez.
- Birden fazla veritabanı örneğinde veya mikro servis ortamında benzersizlik garantisi mevcuttur.
- UPSERT *(bkz. ADR-07)* stratejisiyle birlikte idempotent kayıt işlemi mümkün hâle gelir.

**Değiş tokuşlar:**
- GUID'ler tamsayılara kıyasla daha fazla depolama alanı kaplar *(16 bayt)*. Rastgele GUID'ler *(v4)* B-tree dizinlerinde parçalanmaya *(fragmentation)* yol açabilir.
- Günlüklerde ve hata ayıklama süreçlerinde tamsayılara göre daha az okunabilirdir.
