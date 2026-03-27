# ADR-02: Zengin Domain Entity Kullanımı

## Durum

Kabul Edildi *(Accepted)*

## Bağlam

Domain nesneleri iki farklı yaklaşımla tasarlanabilir:

- **Anemik Model *(Anemic Domain Model)*:** Varlık yalnızca veri taşır; iş kuralları servis katmanına taşınır. Bu yaklaşım iş mantığının servis sınıflarına dağılmasına yol açar ve nesnenin geçersiz durumlarda var olabilmesine izin verir.
- **Zengin Model *(Rich Domain Model)*:** İş kuralları ve doğrulama mantığı doğrudan varlık içinde yer alır; nesnenin her zaman geçerli bir durumda olması sağlanır.

## Karar

`Book` sınıfı zengin domain entity olarak tasarlanmıştır:

- Tüm özellikler `private set` ile dışarıdan doğrudan değiştirilemez.
- Constructor'da tüm girdi doğrulamaları gerçekleştirilir; geçersiz parametrelerle `Book` nesnesi oluşturulamaz.
- Fiyat güncelleme yalnızca `UpdatePrice(decimal newPrice)` metodu üzerinden yapılabilir ve kendi doğrulamasını barındırır.

```csharp
public class Book
{
    public Guid BookId { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public decimal Price { get; private set; }
    public int PageCount { get; private set; }

    public Book(Guid bookId, string title, string author, decimal price, int pageCount)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty.", nameof(title));
        // ... diğer doğrulamalar
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("New price must be greater than zero.", nameof(newPrice));
        Price = newPrice;
    }
}
```

**Uygulanan kurallar:**

| Alan | Kural |
|------|-------|
| `Title` | Boş veya yalnızca boşluktan oluşamaz |
| `Author` | Boş veya yalnızca boşluktan oluşamaz |
| `Price` | Sıfırdan büyük olmalıdır |
| `PageCount` | Sıfırdan büyük olmalıdır |

## Sonuçlar

**Avantajlar:**
- `Book` nesnesi her zaman geçerli bir durumda oluşturulur; geçersiz `Book` sisteme sızmaz.
- İş kuralları bir yerde toplandığından bakım kolaylaşır.
- Domain testleri servis katmanından bağımsız olarak yazılabilir.
- SOLID'in Tek Sorumluluk İlkesi *(SRP)* sağlanır.

**Değiş tokuşlar:**
- `Title`, `Author` ve `PageCount` alanları constructor sonrası değiştirilemez; bu alanları güncellemek için yeni bir `Book` nesnesi oluşturulması gerekir *(bkz. `BookService.UpdateBookPrice`)*.
- Serileştirme kütüphaneleri private setter'lı sınıflarla ek yapılandırma gerektirebilir.
