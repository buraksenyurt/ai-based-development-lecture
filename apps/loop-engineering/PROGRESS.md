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

## Tur 0 - Başlangıç
Sınıf: -
Kapsam: %?? (SonarQube ilk tarama sonucu buraya yazılacak)
Eklenen testler: Mevcut durum; OrderTests (3), ShippingCostCalculatorTests (2)
Reviewer: -
PR/Commit: -
Not: Başlangıç ölçümü insan tarafından alınır.
