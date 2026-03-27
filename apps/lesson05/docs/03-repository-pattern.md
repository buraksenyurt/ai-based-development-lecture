# ADR-03: Repository Pattern ile Veri Erişimi

## Durum

Kabul Edildi *(Accepted)*

## Bağlam

Uygulama servislerinin doğrudan veritabanı sürücüsüne veya ORM çağrılarına bağımlı olması, servis kodunu belirli bir veri erişim teknolojisine sıkı sıkıya bağlar *(tight coupling)*. Bu durum iki önemli soruna yol açar:

1. Veri erişim katmanı değiştirilmek istendiğinde uygulama servislerinin de değişmesi gerekir.
2. Servis sınıflarını birim testlerinde izole etmek güçleşir; gerçek bir veritabanı bağlantısı olmadan test yazılamaz.

## Karar

Veri erişimi `IBookRepository` arayüzü *(interface)* arkasında soyutlanmıştır. Somut uygulama olan `BookRepository`, bu arayüzü PostgreSQL + Dapper kullanarak gerçekler.

```csharp
// Sözleşme (BookApp.Data katmanında tanımlıdır)
public interface IBookRepository
{
    Guid Save(Book book);
}

// Somut uygulama
public class BookRepository(string connectionString) : IBookRepository
{
    public Guid Save(Book book) { /* PostgreSQL + Dapper */ }
}

// Tüketici — somut uygulamayı değil, arayüzü bilir
public class BookService(IBookRepository bookRepository) { ... }
```

`BookService`, `BookRepository` sınıfına değil `IBookRepository` arayüzüne bağımlıdır. Bağımlılık dışarıdan sağlanır *(Dependency Injection)*.

## Sonuçlar

**Avantajlar:**
- `BookService` birim testlerinde `IBookRepository`, Moq ile taklit edilebilir; gerçek veritabanı bağlantısı gerekmez.
- Gelecekte farklı bir veritabanı veya ORM'e geçilmek istendiğinde yalnızca `IBookRepository`'yi gerçekleyen yeni bir sınıf yazılması yeterlidir; `BookService` değişmez.
- SOLID'in Bağımlılığı Tersine Çevirme İlkesi *(DIP)* sağlanır.

**Değiş tokuşlar:**
- Her yeni veri işlemi için hem arayüz hem de uygulama sınıfında değişiklik yapılması gerekir.
- Küçük projelerde bu soyutlama katmanı gereksiz görünebilir; ancak test edilebilirlik açısından kritik öneme sahiptir.
