# LoopLab - Loop Engineering Örneği

Ana README'deki **Ek 4 - Döngü Mühendisliği (Loop Engineering)** bölümünün uygulama örneğidir. **Clean Architecture** prensiplerini baz alan, kasıtlı olarak düşük test kapsamıyla başlayan deneysel bir .NET 10 sipariş servisidir. Amaç, kapsamı adım adım artıran bir ajan döngüsünü elle ve ardından otomatik olarak çalıştırıp gözlemlemektir.

## Yapı

```text
loop-engineering/
├── LoopLab.slnx
├── LOOP.md                      # Hedef, her tur kuralları, durma koşulları, yasaklar
├── PROGRESS.md                  # Döngünün kalıcı durumu (state); her tur bir kayıt
├── .claude/
│   ├── agents/test-reviewer.md  # Doğrulayıcı alt ajan (maker/checker ayrımı)
│   └── settings.json            # İzinler; ajanın src/ altını düzenlemesi yasak
├── scripts/sonar-scan.ps1       # Build + test + coverage + SonarQube taraması
├── src/
│   ├── LoopLab.Domain           # Order, OrderLine, kurallar (bağımlılık yok)
│   ├── LoopLab.Application      # İndirim, kargo, doğrulama, OrderService
│   ├── LoopLab.Infrastructure   # Bellek içi repository
│   └── LoopLab.Api              # Minimal API uç noktaları
└── tests/
    ├── LoopLab.Domain.Tests
    └── LoopLab.Application.Tests
```

| **Katman** | **Sorumluluk** | **Bağımlılık** |
| --- | --- | --- |
| **Domain** | Sipariş yaşam döngüsü ve iş kuralları | Yok |
| **Application** | Kullanım senaryoları, hesaplayıcılar, repository arayüzü | Domain |
| **Infrastructure** | Repository implementasyonu | Application |
| **Api** | HTTP uç noktaları, hata eşleme | Application, Infrastructure |

## Başlangıç Durumu

Testler bilerek az tutulmuştur; döngünün işi bunları çoğaltmaktır.

| **Sınıf** | **Mevcut Test** |
| --- | --- |
| `Order` | 3 test *(kısmi)* |
| `ShippingCostCalculator` | 2 test *(kısmi)* |
| `OrderLine`, `OrderValidator`, `OrderDiscountCalculator`, `OrderService` | Yok |

> **Bilinçli tuzak:** `OrderDiscountCalculator` hafta sonu ve Kasım kampanyaları için doğrudan `DateTime.Now` kullanır ve bir `IClock`/`TimeProvider` soyutlaması yoktur. Üretim koduna dokunmadan bu kurallar deterministik olarak test edilemez. `LOOP.md` kurallarına göre döngünün burada durup `PROGRESS.md` dosyasına **"insan kararı gerekli"** yazması beklenir. Döngünün bunu fark edip etmediğini gözlemlemek alıştırmanın önemli bir parçasıdır.

## Çalıştırma

```bash
dotnet build LoopLab.slnx
dotnet test LoopLab.slnx
dotnet run --project src/LoopLab.Api
```

## SonarQube Taraması

Yerelde çalışan SonarQube Community üzerinde `loop-lab` anahtarıyla bir proje açıp token ürettikten sonra:

```powershell
dotnet tool install --global dotnet-sonarscanner   # ilk seferde
$env:SONAR_TOKEN_LOOP_LAB = "sqp_..."
./scripts/sonar-scan.ps1
```

Elde edilen coverage değerini `PROGRESS.md` içindeki `Tur 0` kaydına yazalım.

## Döngüyü Elle Çalıştırma

Ayrı bir branch açalım *(örneğin `loop/coverage`)* ve Claude Code içinde şu prompt ile tek bir tur işletelim:

```text
LOOP.md ve PROGRESS.md dosyalarını oku ve döngünün bir sonraki turunu çalıştır. Turu bitirmeden önce test-reviewer alt ajanından onay al ve sonucu PROGRESS.md'ye kaydet. Tek tur yap ve dur.
```

Her turdan sonra şunları gözlemleyelim:

- Hangi sınıf seçildi ve kapsam ne kadar arttı?
- Reviewer gerçekten bağımsız davrandı mı?
- `PROGRESS.md` bir sonraki tur için yeterli bilgi içeriyor mu?
- Diff'i kendimiz okuduk mu? *(Anlama borcu burada başlar)*

## Sonraki Adımlar

- Döngüyü başlatacak tetikleyici *(zamanlanmış görev veya CI olayı)*.
- `Stop` hook'u ile kabul kriterini yeniden çalıştıran `scripts/check-loop-goal.sh` betiği.
