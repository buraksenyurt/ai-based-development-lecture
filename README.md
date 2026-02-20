# Yapay Zeka Destekli Yazılım Geliştirme

Konya Gıda ve Tarım Üniversitesitesi Yazılım Müh. ve Pamukkale Üniversitesi Eletrik Elektronik ve Yönetim Bilişim Sistemleri bölümleri için açılmış derse ait doküman ve örnek uygulamaların yer aldığı repodur.

- [Yapay Zeka Destekli Yazılım Geliştirme](#yapay-zeka-destekli-yazılım-geliştirme)
  - [Önsöz](#önsöz)
  - [Gün 00 - Tanışma ve `Hello World` Uygulamasının Geliştirilmesi](#gün-00---tanışma-ve-hello-world-uygulamasının-geliştirilmesi)
    - [Bu çalışmadan çıkarılması gereken dersler](#bu-çalışmadan-çıkarılması-gereken-dersler)
  - [Gün 01 - CV Bank Projesi için Prototip Geliştirme](#gün-01---cv-bank-projesi-için-prototip-geliştirme)
  - [Gün 02 - Prototip Tasarımın Denetlenmesi ve İyileştirilmesi](#gün-02---prototip-tasarımın-denetlenmesi)
  - [Uygulama Önerileri](#uygulama-önerileri)
- [Terimler Sözlüğü](Dictionary.md)

## Önsöz

Yapay zeka araçları günümüzün hype konusu olsa da, bu araçların yazılım geliştirme süreçlerine entegrasyonu henüz tam olarak anlaşılmış değildir. Bu dersin amacı, yapay zeka destekli yazılım geliştirme süreçlerini anlamak ve bu süreçlerde karşılaşılabilecek zorlukları ele almaktır. Ders boyunca, yapay zeka araçlarının yazılım geliştirme süreçlerine nasıl entegre edileceği, avantajları/dezavantajları ve bu araçların kullanımı sırasında karşılaşılabilecek zorluklar üzerinde durulacaktır. Ayrıca, yapay zeka destekli yazılım geliştirme süreçlerinde güvenlik, teknik borç ve proje mimarisi gibi önemli konulara da değinilecektir. Dönem boyunca aşağıdaki soruların cevaplarını arayacağız:

- Yapay zeka araçları hangi vakalarda yazılım geliştirme süreçlerine entegre edilebilir?
- Yapay zeka araçlarının yazılım geliştirme süreçlerine entegrasyonunun avantajları ve dezavantajları nelerdir?
- Bir yapay zeka aracının ürettiği çıktıda hangi konulara dikkat etmek gerekir? Riskler nelerdir? Risklerden nasıl kaçınılır?
- Yapay zeka destekli yazılım geliştirme süreçlerinde güvenlik, teknik borç ve proje mimarisi gibi konular nasıl ele alınmalıdır?
- İdeal veya ideala yakın, hata payı düşük çıktıları elde etmek için ne gibi metodolojiler izlenebilir? Hangi teknikler kullanılır?
- Spec veya test odaklı geliştirme gibi metodolojiler yapay zeka destekli yazılım geliştirme süreçlerinde nasıl uygulanabilir?
- Yapay zeka araçlarındaki sık değişimlere adapte olmak için ne gibi stratejiler izlenebilir?
- Kaynak tüketimi yüksek yapay zeka araçlarını kullanırken maliyetleri kontrol altında tutmak için ne gibi önlemler alınabilir? Optimizasyon teknikleri nelerdir?

## Gün 00 - Tanışma ve `Hello World` Uygulamasının Geliştirilmesi

Bu ilk dersimizde **JSON** veri formatında tasarlanmış bir cv dosyasının analiz edilerek **HTML** formatında bir web sayfasına dönüştürülmesi üzerine çalışıldı. Anthropic'in **Claude Sonnet 4.5** modelini kullanarak bu dönüşümü gerçekleştirmek için çeşitli prompt'lar denedik.

İlk derste kullandığımız prompt'lar:

```text
Bu JSON içeriğini analiz et ve bir html sayfası hazırla.

Kullanıcı dostu bir arabirim sağla.
HTML 5 standartlarını kullan.
Custom CSS kullanma. Bunun yerine Bootstrap kütüphanesini kullan.
JSON içeriğini okumak için Javascript kullan.
```

Alınan CORS *(Cross-Origin Resource Sharing)* hatasına istinaden şu prompt ile devam ettik.

```text
Çalışma zamanında aşağıdaki hatayı alıyorum.

Cross-Origin Request Blocked: The Same Origin Policy disallows reading the remote resource at file:///C:/Users/burak/Development/ai-based-development-lecture/apps/lesson00/myCV.json. (Reason: CORS request not http)

Bir web sunucusu çalıştırmak gerekir mi?
```

ve bunun üstüne **Node.js**'in **http-server** paketini kullanarak basit bir web sunucusu kurmasını istedik.

```text
Sunucuyu komut satırından başlatmak yerine bir nodejs uygulaması ile başlatmak istiyorum.
```

Web sunucusunu başlatmak için aşağıdaki komut kullanılabilir.

```bash
npm run dev
```

Nihai amacımız orta ölçekte bir cv bankası uygulaması geliştirmek ve süreçte yapay zeka araçlarını kullanmak. Başlangıç aşamasında bu uygulamanın yüksek seviyede nasıl görüneceğine dair bir diyagram çizdik.

![High Level Diagram](./images/CvBankHighLevelDiagram.png)

**Ödev:** Amacımız bu diagrama göre projemizi bir adım daha ileri götürmek. Nereden başlarsınız ve Agent'a nasıl bir prompt verirsiniz?

### Bu çalışmadan çıkarılması gereken dersler

- Hayata geçirmek istediğimiz proje fikri için hakim olduğumuz programlama dili ve framework'leri tercih etmeliyiz.
- AI agent'ları ile çalışırken açık ve net prompt'lar vermeliyiz.
- Üretilen kodların herhangibir güvenlik açığı içermediğinden, teknik borç oluşturmadığından ve projenin genel mimarisine uygun olduğundan emin olmalıyız.
- Üretilen programda harici paket bağımlılıkları varsa, bu paketlerin güvenilir ve güncel olduğundan emin olmalıyız. Güvenlik açıkları içerebilecek eski paketlerden kaçınmalıyız.
- Komple bir proje yazdırmak yerine küçük parçalar halinde kod üretmenin daha verimli olabileceğini göz önüne alarak ilerlemeliyiz.

## Gün 01 - CV Bank Projesi için Prototip Geliştirme

İkinci dersimizde en temel seviyede mimari özet ve domain bilgileri içeren spec dokümanlar hazırlayarak ilerledik. [Lesson01](./apps/lesson01/docs/) klasöründen bu dokümanlara erişebilirsiniz.

YZ modeli olarak Claude Sonnet 4.6'yı kullandık. Prompt oturumuna [00-architecture-overiview](./apps/lesson01/docs/00-architecture-overiview.md) ve [01-domain-design](./apps/lesson01/docs/01-domain-design.md) dokümanlarını ekledik. Ardından aşağıdaki promptu işlettik.

```text
Bu dokümanları analiz et ve sadece backend tarafı için gerekli Solution'ı oluştur.
```

Beklediğimiz gibi .net 10 tabanlı bir solution oluşturuldu. Klasör bazlı bir ayrım olmasa da projeler Clean Architecture yaklaşımında belirtildiği gibi Domain, Application, Infrastructure ve Presentation katmanlarına ayrıldı. Domain katmanında User, Contact gibi entity'ler ve Resume gibi aggregate'ler tanımlandı. API katmanında ise REST API standartlarına uygun ve Resume aggregate'ine yönelik CRUD operasyonlarını içeren bir Controller oluşturulduğu görüldü.

Ayrıca veri tabanı tarafı için MongoDb tercih edildiği ve bağlantı ayarlarının `appsettings.json` dosyasına eklendiği görüldü. Domain tasarımında ContactType adında bir enum tanımlanarak iletişim türlerinin sınırlı bir küme ile ifade edildiği gözlemlendi.

Solution ilk seferde derlenmedi zira eksik Nuget paketleri vardı. Ancak ajan sorunları kendisi düzelterek projeyi derlenebilir hale getirdi. Projeyi çalıştırdığımızda Swagger arayüzünde tanımlı endpoint'lerin beklendiği gibi göründüğü ve çalıştığı görüldü.

![Swagger Runtime](./images/day01_00.png)

Ancak;

- Daha zengin ve kaliteli bir mimari tasarım dokümanı hazırlamanın daha iyi sonuçlar vereceği anlaşılıyor. Örneğin API standartlarının detaylı bir şekilde tanımlanması, API katmanında daha eksiksiz ve standartlara uygun bir Controller oluşturulmasını sağlayabilir. Listeleme endpoint'lerinin sayfalama desteği içermesi, veri oluşturma/güncelleme/silme endpoint'lerinin HTTP metodlarına uygun şekilde tasarlanması gibi detaylar mimari dokümanında ne kadar iyi tanımlanırsa, üretilen kodun kalitesi ve mimari uyumu o kadar artabilir.
- Domain tasarımının detaylı ve iyi tanımlanmış olması, üretilen kodun kalitesini ve mimari uyumunu artırabilir. Bu da ilgili domain hakkında yetkin bilgiye sahip olmayı ve DDD *(Domain Driven Design)* prensiplerini iyi bilmeyi gerektirmektedir. Domain tasarımında entity'lerin, aggregate'lerin ve value object'lerin doğru şekilde tanımlanması, kodun okunabilirliğini, bakımını ve genişletilebilirliğini artırır.

## Gün 02 - Prototip Tasarımın Denetlenmesi

> Önceki derste deneysel olarak yapılan geliştirmeyle ortaya çıkan kodun kalitesi ölçümlenir ve iyileştirme önerileri sunulur.

## Uygulama Önerileri

Bu repodaki birçok doküman veya içerik yeni uygulamalar yazmak için bir başlangıç noktası olabilir. Bu fikirleri hakim olduğunuz programlama dili ve geliştirme platformları ve yapay zeka araçlarıyla birleştirerek kendi projelerinizi geliştirebilirsiniz. **Vibe Coding** pratiklerinden ziyade **Agentic Engineering** yaklaşımını benimseyerek hareket etmek daha doğru olur. Yani yapay zeka araçlarını birer yardımcı olarak kullanmak ve onların ürettiği çıktıları dikkatlice inceleyip gerektiğinde müdahale ederek ilerlemek daha verimli olacaktır. Bu süreçte kod güvenilirliği, teknik borç ve proje mimarisi gibi konulara dikkat etmek önemlidir.

| Proje Fikri | Açıklama |
| --- | --- |
| Terimler Sözlüğü | Ders müfredatında geçen teknik terimlerin tanımlarını ve açıklamalarını içeren bir sözlük uygulaması. Kullanıcı terim arayabilir, yeni terimler ekleyebilir. Terimler merkezi bir veri sisteminde servis tabanlı çekilir. Düzenleme ve ekleme fonksiyonellikleri yetkiye *(Authorization)* bağlıdır. |
| | |
