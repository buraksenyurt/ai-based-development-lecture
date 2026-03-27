# ADR-04: Dapper Mikro-ORM Seçimi

## Durum

Kabul Edildi *(Accepted)*

## Bağlam

.NET ekosisteminde veri erişimi için iki yaygın yaklaşım mevcuttur:

- **Entity Framework Core *(EF Core)*:** Tam özellikli ORM; nesne-ilişki eşlemesini, değişiklik takibini *(change tracking)*, migration yönetimini ve LINQ sorgularını otomatik olarak sağlar. Ancak daha fazla soyutlama katmanı ekler ve ürettiği SQL üzerinde kontrol sınırlıdır.
- **Dapper:** Hafif mikro-ORM; SQL sorgularını doğrudan çalıştırır ve sonuçları C# nesnelerine eşler. ADO.NET üzerine ince bir katman ekler; üretilen SQL tamamen geliştiricinin kontrolündedir.

## Karar

Veri erişim katmanında **Dapper** kullanılmasına karar verilmiştir.

```xml
<PackageReference Include="Dapper" Version="2.1.72" />
<PackageReference Include="Npgsql" Version="10.0.2" />
```

`BookRepository.Save()` metodu, Dapper'ın `Execute` metodunu kullanarak parametreli SQL'i doğrudan çalıştırır:

```csharp
connection.Execute(sql, new
{
    book.BookId,
    book.Title,
    book.Author,
    book.Price,
    book.PageCount
});
```

## Sonuçlar

**Avantajlar:**
- SQL sorguları geliştiricinin tam kontrolündedir; beklenmedik veya verimsiz sorgu üretimi yaşanmaz.
- Dapper, EF Core'a kıyasla belirgin biçimde daha hızlıdır; ek yük *(overhead)* minimaldır.
- Öğrenmesi ve anlaşılması kolaydır; SQL bilen herkes kodu okuyabilir.
- Bağımlılık boyutu küçüktür.

**Değiş tokuşlar:**
- Tablo oluşturma, migration yönetimi ve şema değişiklikleri için ayrı bir araç *(örn. Flyway, Liquibase veya elle yazılmış script)* gerekir.
- Karmaşık nesne grafikleri için ilişki yönetimi ve nesne eşleme manuel olarak yapılmalıdır.
- EF Core'un sunduğu değişiklik takibi *(change tracking)* ve LINQ desteği mevcut değildir.
