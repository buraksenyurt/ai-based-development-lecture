# Döngü İlerleme Kaydı

Her tur sonunda aşağıdaki şablonla yeni bir kayıt eklenir. En yeni kayıt en alta yazılır.

```markdown
## Tur N - YYYY-AA-GG
Sınıf: <seçilen sınıf>
Kapsam: %X → %Y (ORDERS_LINE_COVERAGE, measure-coverage.ps1)
Eklenen testler: <test sınıfı / test sayısı>
Reviewer: ONAY | RED (<kısa gerekçe>)
PR/Commit: <referans>
Not: <engel, gözlem veya "insan kararı gerekli">
```

## Tur 0 - 2026-10-21
Sınıf: -
Kapsam: %25.5 (ORDERS_LINE_COVERAGE, 65/255 satır) · SonarQube Coverage: %22.4 (satır + dal, insan kapısı)
Eklenen testler: Mevcut durum; OrderTests (3), ShippingCostCalculatorTests (2)
Reviewer: -
PR/Commit: ek-4-tur-0
Not: Başlangıç ölçümü insan tarafından alındı. %0 kapsamlı sınıflar: OrderDiscountCalculator, OrderService, OrderValidator.

## Tur 1 - 2026-10-21
Sınıf: OrderValidator (%0 kapsamlı üç sınıf eşitti. OrderDiscountCalculator `DateTime.Now` kullandığı, OrderService de repository fake'i gerektirdiği için tamamen deterministik olan OrderValidator seçildi)
Kapsam: %25.5 → %35.7 (ORDERS_LINE_COVERAGE, 65/255 → 91/255 satır) [İnsan düzeltmesi: ajan başlangıcı %29.8 yazmıştı; Tur 0 raporundan yeniden hesaplandı]. OrderValidator: %0 → %88.2 (kapsanmayan 2 satır `const` tanımları)
Eklenen testler: OrderValidatorTests / 13 test metodu (Theory ile 15 test çalışması)
Reviewer: ONAY (küçük not: `Multiple_rule_violations_are_all_reported` yalnızca hata sayısını doğruluyor, mesaj içerikleri `Assert.Contains` ile ayrıca kontrol edilebilir)
PR/Commit: ec40b12 (loop/coverage)
Not: sonar-scan.ps1 başarıyla çalıştı ama token'da Browse yetkisi olmadığı için Coverage metriği API'den okunamadı. Sonar "Coverage" değeri satır ve dal kapsamını birlikte hesapladığı için Tur 0'daki %22.4 ile yukarıdaki satır kapsamı doğrudan karşılaştırılamaz; güncel Sonar değeri dashboard'dan okunmalı. Gözlem: OrderDiscountCalculator hafta sonu/Kasım kurallarında `DateTime.Now` kullanıyor; bu dallar deterministik test edilemez ve ileride "insan kararı gerekli" durumuna yol açabilir.
İnsan kararı (tur sonrası): Ölçüt Sonar'dan `scripts/measure-coverage.ps1` betiğine taşındı ve LOOP.md'ye eşitlik kuralı eklendi; ajanın Sonar'ı okuyamayıp kendi ölçümünü kullanması başlangıç değerinde ~4 puanlık hataya yol açmıştı.

## Tur 2 - 2026-10-21
Sınıf: OrderService (%0 kapsamlı üç sınıf eşitti: OrderDiscountCalculator, OrderNotFoundException, OrderService. OrderDiscountCalculator hafta sonu ve Kasım kurallarında `DateTime.Now` kullandığı için elendi. Kalan iki sınıf da üretim koduna dokunmadan deterministik test edilebiliyordu: OrderService için testte yazılan fake repository yeterli. Bunlardan etkisi büyük olan OrderService seçildi (52 satır, OrderNotFoundException'da 2 satır var))
Kapsam: %35.7 → %74.5 (ORDERS_LINE_COVERAGE, 91/255 → 190/255 satır). OrderService: %0 → %100. Dolaylı artışlar: OrderNotFoundException %0 → %100, OrderDtos %23.1 → %100, OrderDiscountCalculator %0 → %51.3, Order %45.1 → %63.4
Eklenen testler: OrderServiceTests / 8 test metodu (testlerin içinde FakeOrderRepository var), 171 satır
Reviewer: ONAY (küçük not: `Operations_on_unknown_order_throw_order_not_found_exception` tek testte 5 metodu doğruluyor, ileride `[Theory]` ile bölünebilir)
PR/Commit: commit edilmedi, insan incelemesi bekleniyor (loop/coverage, tests/LoopLab.Application.Tests/OrderServiceTests.cs)
Not: `BuildSummary`, `DateTime.Now`'a bağlı OrderDiscountCalculator'ı kullanıyor. Bu yüzden özet testi indirimin kesin değerini kontrol etmiyor. Bunun yerine `0 ≤ indirim ≤ ara toplam × %40` sınırını ve `Total = Subtotal - Discount + Shipping` bağıntısını doğruluyor. Ara toplam 120 TL olduğu için hafta sonu kuralı tetiklenmiyor. Hedef (%70) aşıldı, LOOP.md'deki "Dur" koşulu sağlandı ve döngü burada biter. Sonar değeri insan kapısında dashboard'dan okunmalı. OrderDiscountCalculator'ın zamana bağlı dalları, zaman soyutlaması olmadan (üretim kodu değişikliği) test edilemez. Bu konu ileride "insan kararı gerekli" olarak ele alınmalı.
