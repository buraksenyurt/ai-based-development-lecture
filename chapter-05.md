<a id="top"></a>

# Bölüm 05: Bağımlılıkları Yönetmek ve Kod Kalitesini Ölçmek

<!-- nav:top:start -->
[← Bölüm 04](chapter-04.md#top) · [İçindekiler](README.md#top) · [Bölüm 06 →](chapter-06.md#top)
<!-- nav:top:end -->

Bir yazılım projesinin kalitesi birçok kritere bağlıdır. Kod kalitesi, mimari uyum, test edilebilirlik, okunabilirlik *(readability)*, bakım kolaylığı *(maintainability)*, izlenebilirlik *(monitoring)* gibi kriterler bu faktörlerden sadece birkaçıdır. Zaman içerisinde projelerin kalitesini korumak için birçok yazılım prensibi ve tasarım kalıbı ortaya çıkmıştır. **SOLID** ilkeleri, **Clean Code** prensipleri, **Design Patterns** gibi kavramlar bu alanda önemli yer tutar. Hatta yanlış bilinen doğruları temsil etmek için **Anti-pattern** kavramları da tanımlanmıştır. Tüm bu prensipler, kalitesi yüksek, sürdürülebilir ve genişletilebilir yazılımlar geliştirmek için birer rehber niteliğindedir. Ancak bunları benimsemek ve uygulamak her zaman kolay değildir. Projeye yeni başlayan bir geliştirici için bu kavramların hepsini aynı anda uygulamak oldukça güçtür. Bu nedenle, bu derste daha çok bağımlılık yönetimi ve kod kalitesini ölçmek için kullanılan araçlar üzerinde durulmaya çalışılmıştır.

Dilden ve platformdan bağımsız olarak kod tarafında bileşenler arasındaki bağımlılıkları yönetmek için farklı teknikler kullanılabilir. Örneğin `C#` ve `Java` gibi dilleri göz önüne aldığımızda **Dependency Inversion** prensibini uygulamak için çoğunlukla arayüzlerden *(interface)* yararlanılır. Bu sayede bir bileşenin diğerine olan bağımlılığı soyutlanır *(abstraction)* ve test edilebilirlik, bakım kolaylığı *(maintainability)* gibi avantajlar kazanılır. **Inversion of Control (IoC)** konteynerleri arayüz gibi enstrümanların tanımladığı soyutlamaları ele alarak çalışır. **.NET** bir süredir dahili DI mekanizmaları ile çalışmakta ve bileşen bağımlılıklarının yönetimini oldukça kolaylaştırmaktadır. Tabii bu konuların teferruatı ders müfredatımızın kapsamı dışındadır. Bu derste daha çok bir interface türünün nasıl tanımlandığı, implementasyonu ve çok ilkel bir *dependency inversion* örneğinde ele alınışı üzerinde, örnek bir senaryo üzerinden durulmaya çalışılmıştır.

**Gamepedia** olarak tanımlanan projeye eklenen kodlar yine **docker-compose** üzerinden ayağa kaldırılmış **SonarQube** servisi ile analiz edilmiş ve kod kalitesi ile ilgili geri bildirimler alınmıştır. Tüm bunlarla ilgili olarak teknik borç *(Technical Debt)* kavramı üzerinde durulmuştur.

Kullanılan **SonarQube** komutları ise şöyle; *(token bilgisini kendi sisteminizdeki ile değiştirmeniz gerekecektir)*

```bash
dotnet sonarscanner begin /k:"ai-gamepedia" /d:sonar.host.url="http://localhost:9005"  /d:sonar.token="sqp_TOKEN_BİLGİSİ"

dotnet build

dotnet sonarscanner end /d:sonar.token="sqp_TOKEN_BİLGİSİ"
```

> Derste işlenen kodlar tekrar gözden geçirilmiş ve aralara gerekli yorumlar eklenmiştir. *Lütfen yorum satırlarını dikkatlice okuyunuz* ve bahsedilen kavramları araştırınız.

<!-- nav:bottom:start -->
---

| ← Önceki Bölüm | İçindekiler | Sonraki Bölüm → |
| :--- | :---: | ---: |
| [Bölüm 04: Hata Yönetimi, Loglama ve İzleme](chapter-04.md#top) | [İçindekiler](README.md#top) | [Bölüm 06: Yazılım Çözümlerinde Testin Önemi](chapter-06.md#top) |
<!-- nav:bottom:end -->
