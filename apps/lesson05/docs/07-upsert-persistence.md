# ADR-07: Upsert Kalıcılık Stratejisi

## Durum

Kabul Edildi *(Accepted)*

## Bağlam

Veri kaydetme *(persist)* işlemi genellikle iki ayrı senaryo içerir: yeni kayıt oluşturma *(INSERT)* ve mevcut kaydı güncelleme *(UPDATE)*. Bu senaryoları ayrı metotlarla ele almak, çağıran tarafın kaydın daha önce var olup olmadığını bilmesini ya da kontrol etmesini gerektirir; bu da ek veritabanı sorgusu ve karmaşık koşullu mantık anlamına gelir.

## Karar

`IBookRepository` arayüzünde hem ekleme hem güncelleme için tek bir `Save(Book book)` metodu tanımlanmıştır. `BookRepository` bu metodu PostgreSQL'in `ON CONFLICT DO UPDATE` *(UPSERT)* özelliğiyle gerçekler:

```csharp
var sql = """
    INSERT INTO books (book_id, title, author, price, page_count)
    VALUES (@BookId, @Title, @Author, @Price, @PageCount)
    ON CONFLICT (book_id) DO UPDATE
    SET title = EXCLUDED.title,
        author = EXCLUDED.author,
        price = EXCLUDED.price,
        page_count = EXCLUDED.page_count;
    """;
```

**Çalışma biçimi:**
- `book_id` çakışması yoksa yeni satır eklenir.
- `book_id` zaten varsa `title`, `author`, `price` ve `page_count` alanları güncellenir.
- Her iki durumda da `BookId` döndürülür.

Bu tasarım, `BookService` içinde `AddBook` ve `UpdateBookPrice` işlemlerini tek bir repository çağrısıyla yönetmeyi mümkün kılar:

```csharp
public Guid AddBook(...) => bookRepository.Save(new Book(...));
public Guid UpdateBookPrice(...) { var b = new Book(...); b.UpdatePrice(newPrice); return bookRepository.Save(b); }
```

## Sonuçlar

**Avantajlar:**
- Repository arayüzü basit ve tek bir metotla yönetilebilir; `Add` ve `Update` şeklinde ikiye bölünmesi gerekmez.
- Aynı `BookId` ile tekrarlanan kayıt işlemleri idempotent sonuç üretir; yan etkisiz tekrarlar mümkündür.
- Çağıran taraf kaydın var olup olmadığını sorgulamak zorunda kalmaz; tek bir ağ gidiş-dönüşüyle işlem tamamlanır.

**Değiş tokuşlar:**
- UPSERT sözdizimi *(ON CONFLICT DO UPDATE)* PostgreSQL'e özgüdür; farklı bir veritabanına geçişte SQL yeniden yazılmalıdır.
- `Book` nesnesinin tüm alanları her kayıtta veritabanına iletilir; yalnızca değişen alanların güncellenmesi *(partial update)* desteklenmez.
- Kayıt sonrası yeni mi oluşturuldu yoksa var olan mı güncellendi bilgisi çağıran tarafa iletilmez.
