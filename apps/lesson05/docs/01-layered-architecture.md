# ADR-01: Katmanlı Mimari

## Durum

Kabul Edildi *(Accepted)*

## Bağlam

Tek bir projede tüm kodu barındırmak; iş kuralları, veri erişimi ve uygulama mantığının zamanla birbirine karışmasına yol açar. Bu durum kodun test edilmesini güçleştirir, bağımlılıkları yönetilemez hâle getirir ve değişikliklerin yan etkilerini tahmin etmeyi zorlaştırır.

## Karar

Çözüm, sorumlulukları net biçimde ayrılmış üç ana katmana bölünmüştür:

| Katman | Proje | Açıklama |
|--------|-------|----------|
| Domain | `BookApp.Domain` | İş varlıklarını ve iş kurallarını barındırır. Hiçbir dış bağımlılığı yoktur. |
| Data | `BookApp.Data` | Veri erişim arayüzünü ve PostgreSQL uygulamasını barındırır. Yalnızca Domain'e bağımlıdır. |
| Application | `BookApp.Application` | İş akışı servislerini barındırır. Domain ve Data katmanlarına bağımlıdır. |

Bağımlılıklar yalnızca içe doğru akar: **Application → Data → Domain**. Domain katmanı hiçbir başka katmana bağımlı değildir.

```
BookApp.Domain       (bağımlılık yok)
      ↑
BookApp.Data         (Domain'e bağımlı)
      ↑
BookApp.Application  (Domain + Data'ya bağımlı)
```

## Sonuçlar

**Avantajlar:**
- Her katman bağımsız olarak test edilebilir.
- Domain katmanı, veritabanı veya çerçeve değişikliklerinden etkilenmez.
- Veri erişimi katmanı değiştirilmeden *(örn. EF Core'a geçiş)* uygulama mantığı sabit kalabilir.
- Takım üyeleri farklı katmanlar üzerinde paralel çalışabilir.

**Değiş tokuşlar:**
- Küçük bir proje için katman sayısı nispeten fazla görünebilir.
- Her özellik için birden fazla projede değişiklik yapmak gerekebilir.
