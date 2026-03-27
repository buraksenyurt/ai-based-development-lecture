# Mimari Genel Bakış

BookApp projesinde alınan mimari kararların özeti aşağıda tablolar hâlinde sunulmuştur.

## Katmanlar ve Sorumluluklar

| Proje | Katman | Sorumluluk |
|-------|--------|------------|
| `BookApp.Domain` | Domain | `Book` gibi iş nesnelerini *(entity)* ve iş kurallarını barındırır. Hiçbir dış bağımlılığı yoktur. |
| `BookApp.Data` | Data / Infrastructure | Veri erişim sözleşmesini *(IBookRepository)* ve PostgreSQL uygulamasını *(BookRepository)* içerir. |
| `BookApp.Application` | Application | `BookService` aracılığıyla iş akışlarını yönetir; Domain ve Data katmanlarını bir araya getirir. |
| `BookApp.Domain.Tests` | Test | Domain katmanına ait birim testleri barındırır. |
| `BookApp.Application.Tests` | Test | Application katmanına ait birim testleri barındırır. |

## Teknoloji Seçimleri

| Konu | Karar | Açıklama |
|------|-------|----------|
| Platform | .NET 10 | Microsoft'un en güncel LTS/preview çerçevesi |
| Programlama Dili | C# 12 | Primary constructor sözdizimi dahil |
| Veritabanı | PostgreSQL | İlişkisel, üretim kalitesinde açık kaynak veritabanı |
| Veri Erişimi | Dapper | Hafif mikro-ORM; EF Core'a tercih edilmiştir |
| Sürücü | Npgsql | PostgreSQL için resmi .NET sürücüsü |
| Birim Test Çerçevesi | xUnit | Modern, genişletilebilir test altyapısı |
| Mock Kütüphanesi | Moq | Uygulama testleri için bağımlılık taklitçisi |
| Kod Kapsamı | coverlet | Test kapsam raporu üretimi |

## Uygulanan Mimari Kararlar

| # | Karar | Referans |
|---|-------|---------|
| ADR-01 | Katmanlı Mimari | [01-layered-architecture.md](01-layered-architecture.md) |
| ADR-02 | Zengin Domain Entity | [02-rich-domain-entity.md](02-rich-domain-entity.md) |
| ADR-03 | Repository Pattern | [03-repository-pattern.md](03-repository-pattern.md) |
| ADR-04 | Dapper Mikro-ORM Seçimi | [04-dapper-orm-choice.md](04-dapper-orm-choice.md) |
| ADR-05 | PostgreSQL Veritabanı | [05-postgresql-database.md](05-postgresql-database.md) |
| ADR-06 | GUID Birincil Anahtar | [06-guid-primary-key.md](06-guid-primary-key.md) |
| ADR-07 | Upsert Kalıcılık Stratejisi | [07-upsert-persistence.md](07-upsert-persistence.md) |
| ADR-08 | Constructor Injection | [08-dependency-injection.md](08-dependency-injection.md) |
| ADR-09 | xUnit + Moq Test Stratejisi | [09-unit-testing-strategy.md](09-unit-testing-strategy.md) |

## Bağımlılık Grafiği

```
BookApp.Domain          (bağımlılık yok)
      ↑
      ├── BookApp.Data  (Domain'e bağımlı)
      │
      └── BookApp.Application  (Domain + Data'ya bağımlı)
```

Test projeleri yalnızca test ettikleri katmana bağımlıdır; `BookApp.Application.Tests`, Moq aracılığıyla `IBookRepository`'yi taklit ederek `BookApp.Data` somut uygulamasına doğrudan bağımlı olmaktan kaçınır.
