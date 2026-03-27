# ADR-08: Constructor Injection ile Bağımlılık Yönetimi

## Durum

Kabul Edildi *(Accepted)*

## Bağlam

Sınıfların ihtiyaç duydukları bağımlılıkları *(dependency)* nasıl edindikleri, kodun test edilebilirliğini ve esnekliğini doğrudan etkiler. Üç temel yaklaşım değerlendirilmiştir:

- **`new` anahtar sözcüğüyle doğrudan örnekleme:** Sınıf bağımlılığını kendisi oluşturur; sıkı bağlılık *(tight coupling)* yaratır ve test edilebilirliği ortadan kaldırır.
- **Özellik enjeksiyonu *(Property Injection)*:** Bağımlılık public bir özellik aracılığıyla atanır; ancak sınıfın eksik bağımlılıkla kullanılabilmesine kapı açar.
- **Constructor Injection:** Bağımlılıklar constructor aracılığıyla zorunlu olarak alınır; sınıf, bağımlılıkları olmadan örneklenemez.

## Karar

Tüm zorunlu bağımlılıklar **constructor injection** yöntemiyle alınmaktadır. C# 12'nin **primary constructor** sözdizimi kullanılarak bu enjeksiyonlar öz biçimde ifade edilmiştir:

```csharp
// BookRepository — bağlantı dizesini constructor aracılığıyla alır
public class BookRepository(string connectionString) : IBookRepository
{
    public Guid Save(Book book)
    {
        using var connection = new NpgsqlConnection(connectionString);
        // ...
    }
}

// BookService — IBookRepository soyutlamasını constructor aracılığıyla alır
public class BookService(IBookRepository bookRepository)
{
    public Guid AddBook(...) => bookRepository.Save(new Book(...));
}
```

`BookService`, somut `BookRepository`'ye değil `IBookRepository` arayüzüne bağımlıdır. Bu sayede birim testlerinde gerçek uygulama yerine taklit *(mock)* nesnesi kullanılabilir.

## Sonuçlar

**Avantajlar:**
- Bağımlılıklar açıkça bildirilir; sınıfın neye ihtiyaç duyduğu constructor imzasından doğrudan anlaşılır.
- Sınıf, gerekli bağımlılıklar sağlanmadan örneklenemez; eksik bağımlılık sorunu derleme veya nesne oluşturma aşamasında yakalanır.
- Birim testlerinde `IBookRepository` Moq ile kolayca taklit edilebilir; gerçek veritabanı bağlantısı gerekmez.
- SOLID'in Bağımlılığı Tersine Çevirme İlkesi *(DIP)* sağlanır.
- Primary constructor sözdizimi boilerplate kodu azaltır ve okunabilirliği artırır.

**Değiş tokuşlar:**
- Bağımlılık grafiği derinleştikçe constructor parametre sayısı artabilir; bu durum sınıfın çok fazla sorumluluğu üstlendiğine işaret edebilir *(SRP ihlali uyarısı)*.
- Çalışma zamanı *(runtime)* bağımlılık çözümlemesi için bir IoC konteyneri *(örn. `Microsoft.Extensions.DependencyInjection`)* gerekebilir; mevcut durumda bu konfigürasyon uygulama katmanının dışında yapılmaktadır.
