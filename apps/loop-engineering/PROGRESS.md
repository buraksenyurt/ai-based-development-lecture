# Döngü İlerleme Kaydı

Her tur sonunda aşağıdaki şablonla yeni bir kayıt eklenir. En yeni kayıt en alta yazılır.

```markdown
## Tur N - YYYY-AA-GG
Sınıf: <seçilen sınıf>
Kapsam: %X → %Y (modül geneli)
Eklenen testler: <test sınıfı / test sayısı>
Reviewer: ONAY | RED (<kısa gerekçe>)
PR/Commit: <referans>
Not: <engel, gözlem veya "insan kararı gerekli">
```

## Tur 0 - 2026-10-21
Sınıf: -
Kapsam: %22.4 (SonarQube, modül geneli)
Eklenen testler: Mevcut durum; OrderTests (3), ShippingCostCalculatorTests (2)
Reviewer: -
PR/Commit: ek-4-tur-0
Not: Başlangıç ölçümü insan tarafından alındı. %0 kapsamlı sınıflar: OrderDiscountCalculator, OrderService, OrderValidator.

## Tur 1 - 2026-10-21
Sınıf: OrderValidator (%0 kapsamlı üç sınıf eşitti. OrderDiscountCalculator `DateTime.Now` kullandığı, OrderService de repository fake'i gerektirdiği için tamamen deterministik olan OrderValidator seçildi)
Kapsam: %29.8 → %35.7 (modül geneli, OpenCover satır kapsamı, sonar-scan.ps1 çıktısından hesaplandı). OrderValidator: %0 → %88.2 (kapsanmayan 2 satır `const` tanımları)
Eklenen testler: OrderValidatorTests / 13 test metodu (Theory ile 15 test çalışması)
Reviewer: ONAY (küçük not: `Multiple_rule_violations_are_all_reported` yalnızca hata sayısını doğruluyor, mesaj içerikleri `Assert.Contains` ile ayrıca kontrol edilebilir)
PR/Commit: commit edilmedi (loop/coverage dalında, çalışma ağacında)
Not: sonar-scan.ps1 başarıyla çalıştı ama token'da Browse yetkisi olmadığı için Coverage metriği API'den okunamadı. Sonar "Coverage" değeri satır ve dal kapsamını birlikte hesapladığı için Tur 0'daki %22.4 ile yukarıdaki satır kapsamı doğrudan karşılaştırılamaz; güncel Sonar değeri dashboard'dan okunmalı. Gözlem: OrderDiscountCalculator hafta sonu/Kasım kurallarında `DateTime.Now` kullanıyor; bu dallar deterministik test edilemez ve ileride "insan kararı gerekli" durumuna yol açabilir.
