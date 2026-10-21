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
