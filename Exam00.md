# Yazılım Mühendisliği ve Yapay Zeka Destekli Yazılım Geliştirme Final Sınavı Örnek Soruları

Bu dokümanda final sınavına hazırlık için örnek sorular yer almaktadır.

## Soru 1

Yazılım projelerinde kodun beklediğimiz şekilde çalışmasını garanti altına almak için birçok test metodolojisi kullanılır. Bunlardan birisi de birim testlerdir *(Unit Testi)*. Birim testleri, yazılımın en küçük parçalarını (örneğin fonksiyonlar veya metotlar) izole ederek test etmeye odaklanır. Bu testler, kodun belirli bir bölümünün doğru çalışıp çalışmadığını kontrol eder ve genellikle otomatikleştirilir.

Çalışmakta olduğunuz elektronik ticaret projesinde kullanıcıların sepete attıkları ürünlerin toplam tutarını hesaplayan bir fonksiyon geliştirmek istediğinizi düşünün. Bu fonksiyonun belirli kabül kriterlerini karşılaması bekleniyor. Geliştirme metodolojisini değiştiriyorsunuz ve öncelikle testin başarısız olduğu senaryoyu *(Fail)* ardından bu senaryoyu düzelten kod parçasını *(Pass)* ve son adımda ise kodun ideal hale getirilmiş versiyonunu *(Refactor)* yazarak ilerliyorsunuz. **Red - Green - Blue** olarak da bilinen bu süreç literatürde nasıl bilinir?

- A) Test Driven Development (TDD)
- B) Behavior Driven Development (BDD)
- C) Test First Development (TFD)
- D) Test Last Development (TLD)

## Soru 2

Uygulamaların geliştirme ortamlarında ihtiyaç duyduğu birçok dış bağımlılık olabilir. Veritabanları, mesajlaşma sistemleri, üçüncü taraf API'ler gibi.

Geliştirmekte olduğunuz web uygulaması, bazı fiziki dosyaları Amazon S3 üzerinden karşılamaktadır. Geliştirme sürecinde bu dosyalara erişim sağlamak için gerçek S3 ortamını kullanmak yerine, yerel bir ortamda S3'ün davranışını taklit eden bir araç kullanarak ilerlemek istediğinizi düşünün. Bu amaçla konteyner tabanlı bir çözüm kullanarak, S3'ün temel özelliklerini taklit eden bir ortam oluşturabilirsiniz. Bu senaryoda şıklardaki araçlardan hangisini kullanırsınız?

- A) Github Actions
- B) Docker
- C) Playwright
- D) SonarQube

## Soru 3

Yazılım projelerinin yaşam döngüsü boyunca, kod kalitesini ve güvenliğini sağlamak için çeşitli araçlar kullanılır. Bu araçlar, kodun belirli standartlara uygun olup olmadığını kontrol eder, potansiyel hataları ve güvenlik açıklarını tespit eder. Örneğin SonarQube statik kod analizi yaparak kodun kalitesini ölçer ve raporlar. Kodun kalitesini ölçmek için kullanılan metriklerden birisi `Code Coverage` değeridir. Bu değer, testler tarafından çalıştırılan kodun yüzdesini ifade eder. Yüksek değerler daha iyi test edilmiş bir kod tabanına işaret eder.

Code Coverage değerini artırmak için şıklarda belirtilen stratejilerden hangisini tercih edersiniz?

- A) Kodun karmaşıklığını artıran Cognitive Complexity değerini düşürmek
- B) Testleri manuel olarak çalıştırmak ve sonuçları gözlemlemek
- C) Gereksiz yorum satırlarını kaldırmak
- D) Birim test senaryolarını genişletmek ve daha fazla test eklemek

## Soru 4

Bir bayi otomasyon sisteminde yedek parça siparişleri anlık olarak çok yüksek hacimlere ulaşabileceği öngörülmektedir. Bu nedenle sistemin ölçeklenebilir *(Scalability)* olarak tasarlanması gerektiği belirlenmiş, stok yönetimi, sipariş yönetimi ve tedarikçi entegrasyonu gibi modüllerin birbirinden bağımsız dağıtılabilmesi *(Deployment)* ve yönetilebilmesi gerektiğine karar verilmiştir. `Richards & Ford` 'un da belirttiği özelliklere göre, bu gereksinimler için aşağıdaki mimarilerden hangisi en yüksek test edilebilirlik ve ölçeklenebilirlik avantajına sahiptir?

- A) Monolithic Architecture
- B) Layered Architecture
- C) Microservices Architecture
- D) Event-Driven Architecture

## Soru 5

Aşağıdaki kod parçasını dikkatlice inceleyelim.

```csharp
public class OrderService
{
    public decimal CalculateTotal(List<OrderItem> items, decimal taxRate, decimal discount)
    {
        decimal total = 0;
        decimal baseTotal = 0;

        foreach (var item in items)
        {
            total += item.Price * item.Quantity;
        }

        baseTotal = total;

        total += total * taxRate;
        total -= discount;

        return total;
    }
}
```

Statik kod tarayıcısı bu kodla ilgili bir ihlal tespit etmiştir. Sizce bu ihlal şıklardan hangisidir?

- A) Güvenlik Açığı *(Security Vulnerability)*
- B) Metodun parametre yapısı çok uzundur *(Long Parameter List)*
- C) Kodun karmaşıklığı çok yüksektir *(High Cognitive Complexity)*
- D) Programda kullanılmayan gereksiz kodlar vardır *(Dead Code/ Unused Variables)*

DEVAM EDECEK

## Cevap Anahtarı

| Soru No | Doğru Cevap |
|---------|-------------|
| 1       | A           |
| 2       | B           |
| 3       | D           |
| 4       | C           |
| 5       | D           |
