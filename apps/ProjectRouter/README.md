# Project Router

Derse katılan öğrencileri dönem projelerine tercihlerine göre otomatik dağıtan Razor Pages uygulaması. Gereksinimler, modeller ve kurallar için [docs/project-router.md](../../docs/project-router.md) dokümanına bakın.

## Çalıştırma

```bash
cd apps/ProjectRouter
dotnet test ProjectRouter.slnx
dotnet run --project ProjectRouter.Web
```

Veritabanı (`projectrouter.db`) bağlantı dizesindeki göreli yol nedeniyle uygulamanın çalışma dizininde otomatik oluşturulur (yukarıdaki komutlarla `apps/ProjectRouter` altında).

## Proje yapısı

| Proje | Sorumluluk |
| ----- | ---------- |
| `ProjectRouter.Domain` | `Participant`, `ProjectIdea`, `Competition` ve kural doğrulamaları (Rule 00–04). Dış bağımlılığı yoktur. |
| `ProjectRouter.Application` | Servisler, repository arayüzleri ve dağıtım motoru (`Routing/`). |
| `ProjectRouter.Data` | Dapper + SQLite repository'leri ve şema (`Schema.sql`). |
| `ProjectRouter.Web` | Razor Pages arayüzü: Katılımcılar, Projeler, Turnuvalar. |
| `*.Tests` | xUnit + Moq testleri. |

## Tasarım kararları

- [01 - Katmanlı mimari](docs/01-layered-architecture.md)
- [02 - Dapper ve SQLite](docs/02-dapper-sqlite.md)
- [03 - Dağıtım algoritması](docs/03-routing-algorithm.md)
