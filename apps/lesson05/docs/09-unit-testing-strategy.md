# ADR-09: xUnit ve Moq ile Birim Test Stratejisi

## Durum

Kabul Edildi *(Accepted)*

## Bağlam

Kodun doğruluğunu güvence altına almak ve değişiklikler sırasında regresyon riskini azaltmak için bir test stratejisi belirlenmelidir. Seçilmesi gereken bileşenler şunlardır:

- **Test çerçevesi:** Testlerin yazılıp çalıştırıldığı altyapı.
- **Mock kütüphanesi:** Bağımlılıkların taklit edilerek sınıfların izole biçimde test edilmesini sağlayan araç.

## Karar

Test altyapısı olarak **xUnit** ve bağımlılık takliti için **Moq** kullanılmasına karar verilmiştir. Testler iki ayrı projede örgütlenmiştir:

| Proje | Test Edilen Katman | Araçlar |
|-------|--------------------|---------|
| `BookApp.Domain.Tests` | `BookApp.Domain` | xUnit |
| `BookApp.Application.Tests` | `BookApp.Application` | xUnit + Moq |

**Domain testleri** — Dış bağımlılık gerektirmeden `Book` entity'sinin davranışını doğrular:

```csharp
[Fact]
public void Constructor_WithValidParameters_CreatesBook() { ... }

[Theory]
[InlineData(null)]
[InlineData("")]
[InlineData("   ")]
public void Constructor_WithEmptyTitle_ThrowsArgumentException(string? invalidTitle) { ... }
```

**Application testleri** — `IBookRepository` Moq ile taklit edilerek `BookService` izole biçimde test edilir:

```csharp
public BookServiceTests()
{
    _repositoryMock = new Mock<IBookRepository>();
    _bookService = new BookService(_repositoryMock.Object);
}

[Fact]
public void AddBook_WithValidParameters_CallsRepositorySaveOnce()
{
    _repositoryMock.Setup(r => r.Save(It.IsAny<Book>())).Returns(bookId);
    _bookService.AddBook(bookId, "Clean Code", "Robert C. Martin", 39.99m, 464);
    _repositoryMock.Verify(r => r.Save(It.IsAny<Book>()), Times.Once);
}
```

Kod kapsamı için **coverlet** toplayıcısı her iki test projesine de eklenmiştir.

## Sonuçlar

**Avantajlar:**
- xUnit, .NET ekosisteminde yaygın ve modern bir çerçevedir; paralel test çalıştırma ve Theory/Fact ayrımı gibi özellikler sunar.
- Moq sayesinde `BookService` testleri gerçek bir veritabanı bağlantısı olmadan hızla çalışır.
- `[Theory]` + `[InlineData]` kombinasyonu tek metotla çok sayıda sınır değeri test edilmesini mümkün kılar.
- Domain ve Application testlerinin ayrı projelerde tutulması, katman sınırlarını pekiştirir.
- coverlet entegrasyonu CI/CD süreçlerinde kod kapsam raporu üretimine olanak tanır.

**Değiş tokuşlar:**
- Mevcut testler yalnızca birim *(unit)* düzeyindedir; gerçek PostgreSQL veritabanıyla entegrasyonu doğrulayan entegrasyon testleri bulunmamaktadır.
- `BookService.UpdateBookPrice` metodunda `Book` yeniden oluşturulduğu için güncel veritabanı durumu doğrulanamaz; bu durum entegrasyon testi gerektiren bir boşluk bırakmaktadır.
