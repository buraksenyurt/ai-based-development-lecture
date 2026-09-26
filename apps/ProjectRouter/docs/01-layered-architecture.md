# 01 - Katmanlı Mimari

## Karar

Uygulama dört katmana ayrıldı: `Domain` ← `Application` ← `Data` / `Web`.

- **Domain** kuralları constructor'larda doğrular. Geçersiz bir `Participant` veya `ProjectIdea` oluşturulamaz. Kural ihlalleri, hangi kuralın bozulduğunu `RuleCode` alanında taşıyan `DomainRuleException` ile bildirilir (ör. `Rule 00`) ve arayüzde bu kodla gösterilir.
- **Application** repository arayüzlerini tanımlar (`IParticipantRepository` vb.), böylece dağıtım motoru ve servisler veritabanından bağımsız test edilebilir.
- **Data** bu arayüzleri Dapper ile uygular.
- **Web** yalnızca form verisini domain nesnelerine çevirir ve servisleri çağırır.

## Gerekçe

Depodaki diğer örneklerle (`apps/lesson05`) aynı yapı izlendi. Dağıtım algoritması, uygulamanın en çok değişmesi beklenen parçası. `IProjectRouter` ve `IMatchScorer` arayüzleri sayesinde farklı bir algoritma (ör. Hungarian) diğer katmanlara dokunmadan eklenebilir.
