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

## Soru 6

Projede geliştirilen bir kod parçasını gözden geçirmeniz ve yorumlamanız isteniyor. Söz konusu kod parçası aşağıdaki gibidir.

```csharp
public void ProcessOrder(Order order)
{
    if (order != null)
    {
        if (order.Items.Count > 0)
        {
            foreach (var item in order.Items)
            {
                if (item.Price > 100)
                {
                    if (order.Customer.IsVIP)
                    {
                        item.Discount = 20;
                    }
                    else
                    {
                        item.Discount = 10;
                    }
                }
            }
        }
    }
}
```

Koda baktığınızda burnuzuza kötü kokular *(Code Smells)* gelmektedir. Sizce bu kod parçasında statik kod tarama aracına da takılacak ne gibi bir sorun vardır?

- A) Program kodunda zayıf isimlendirme standartları yer almaktadır *(Naming Convention Violation)*
- B) Kötü niyetli kullanıcılar tarafından kaynak sızıntısı oluşabilir *(Resource Leak)*
- C) Kod tekrarı söz konusudur *(Code Duplication)*
- D) Yüksek düzeyde bilişsel kod karmaşıklığı içermektedir *(High Cognitive Complexity)*

## Soru 7

Sisteme giriş yapan kullanıcılar kod tarafında aşağıdaki metot ile doğrulanmaktadır.

```csharp
public User GetUser(string username)
{
    string query = "SELECT * FROM Users WHERE Username = '" + username + "'";
    return dbContext.ExecuteQuery(query);
}
```

Program kodu CI/CD hattına alındıktan sonra çalışan statik kod tarayıcısı ise bu metotta `Blocker` seviyesinde bir bulgu tespit etmiştir. Sizce bu bulgunun sebebi aşağıdakilerden hangisidir?

- A) Sihirli sayı *(Magic Number)* kullanımı yer almaktadır
- B) SQL ifadesi doğrudan string birleştirme yöntemiyle oluşturulmuştur ve bu nedenle SQL Injection saldırılarına açıktır *(Security Vulnerability - SQL Injection)*
- C) Parametre adı isimlendirme standartlarına uyulmamaktadır *(Naming Convention Violation)*
- D) Metod içerisinde beklenmedik çalışma zamanı istisnaları ele alınmamaktadır *(Unhandled Exception)*

## Soru 8

Müşteri kayıtlarını sisteme ekleyen bir servis sınıfında aşağıdaki gibi bir metod yer almaktadır.

```csharp
public void CreateCustomer(string firstName, string lastName, string email, string phone, string addressLine1, string city, string country, string zipCode, DateTime dateOfBirth, bool isPremium)
{
    // Kayıt işlemleri...
}
```

`Clean Code` prensiplerine göre bu kodu ideal hale getirmek için aşağıdaki stratejilerden hangisini önerirsiniz?

- A) Metot içerisinde kullanılmayan parametreler varsa bunların kaldırılmasını öneririm
- B) Metodun parametre listesini daha okunabilir hale getirmek için bir `Customer` sınıfı oluşturup bu sınıfın bir örneğini parametre olarak göndermeyi öneririm
- C) Metodun parametre listesini daha okunabilir hale getirmek için `Builder` tasarım desenini kullanarak bir `CustomerBuilder` sınıfı oluşturmayı öneririm
- D) Hiçbir değişiklik yapmadan mevcut haliyle bırakmayı öneririm çünkü metodun parametre yapısı zaten yeterince açık ve anlaşılır

## Soru 9

Çocuklara matematiği sevdirmek için geliştirilen bir eğitim uygulaması üzerinde kod kontrolü yapmakla görevlendirildiniz ve kodun bir parçasında daire alanının hesaplanması ile ilgili aşağıdaki gibi yazılmış olan metotla karşılaştınız. Ne var ki bu kod parçasında sizi rahatsız eden bir şey var. Sizce bu kod parçasında ne gibi bir sorun vardır?

```csharp
public double CalculateCircleArea(double radius)
{
    return 3.14159265359 * radius * radius;
}
```

- A) Daire alanı hesaplamasında kullanılan PI değeri hatalı verilmiştir.
- B) Daire alanı hesaplamasında kullanılan formül yanlıştır.
- C) radius paramtresi için Null kontrolü yapılmamıştır ve bu nedenle NullReferenceException hatası oluşabilir.
- D) Daire alanı hesaplamasında kullanılan sayısal değer sihirli sayı *(Magic Number)* olarak kodun içerisine gömülmüştür. Bunun yerine bir sabit *(Constant)* ve hatta **Math.PI* enstrümanı kullanılarak kodun okunurluğunu artırıp bakımını kolaylaştırabiliriz.

## Soru 10

Yapay zeka dil modelleri *(LLM - Large Language Models)* çalıştığınız kurumun içeride kullandığı özel kodlama standartlarını, mimari kararları, geliştirme metodolojilerini veya iş akışlarını bilmez. Asistanın size doğru cevap verebilmesi için, sorduğunuz soruyla birlikte ilgili doküman parçalarının bağlama *(context)* dahil edilmesi gerektiğine karar verdiniz. Bu nedenle kullanıcı sorgusunu mevcut bilgi tabanından veriler getirerek zenginleştiren ve modeli bu özel bilgiyle besleyen bir metodolojide ilerlemeyi planlıyorsunuz. Aşağıdaki stratejilerden hangisini tercih edersiniz?

- A) Retreival Augmented Generation *(RAG)* yaklaşımını benimsemek ve modeli kullanıcı sorgusuyla birlikte ilgili doküman parçalarını da içeren bir bilgi getirme mekanizmasıyla beslemek
- B) Behaviror Driven Development *(BDD)* yaklaşımını benimsemek ve modeli kullanıcı hikayeleri, kabul kriterleri ve test senaryoları gibi yapılarla beslemek
- C) Prompt Engineering yaklaşımını benimsemek ve modeli kullanıcı sorgusuyla birlikte ilgili doküman parçalarını da içeren zenginleştirilmiş promptlarla beslemek
- D) Test Driven Development *(TDD)* yaklaşımını benimsemek ve modeli kullanıcı sorgusuyla birlikte ilgili doküman parçalarını da içeren test senaryolarıyla beslemek

## Soru 11

GitHub Copilot gibi bir yapay zeka asistanından uygulamada yer alan ürün yönetimi sınıfı ile ilgili olası tüm birim testleri *(Unit Test)* oluşturmasını istediniz. Asistan saniyeler içinde size yirmiden fazla test kodu üretti. Yapay zekanın bu kodu üretmesi sonrasında teknik borç yaratmamak adına bir yazılım mühendisinin izlemesi gereken en doğru yol hangisidir?

- A) Üretilen testleri doğrudan projeye ekleyip, hatalı olanları manuel olarak projenin dışına almak.
- B) Testlerin iş gereksinimlerini tamamıyla kapsayıp kapsamadığını analiz etmek ve anlamlı olup olmadıklarını satır satır gözden geçirmek *(Code Review)*.
- C) Yapay zeka kodlarının her zaman güvenlik açıkları barındırabileceğini varsayarak üretilen tüm kodları silmek.
- D) Sadece hata fırlatan testleri inceleyip, başarılı çalışan testleri gözden geçirmeden yayına almak.

## Soru 12

Komut satırında çalışan bir Copilot ajanı ile yepyeni bir .NET Solution yapısı kurduğunuz düşünün. Çözümünüz `Hexagonal Architecture` prensiplerine uygun olarak tasarlanmış olsun. Veritabanı tarafında `Postgresql` kullanıyorsunuz ve O/RM *(Object Relational Mapper)* olarak da `Entity Framework Core` tercih ediyorsunuz. Mimari olarak kayıt altına alınmasını istediğiniz bazı kararlar var bunları Claude Sonnet ile oluşturmaya karar verdiniz. `VS Code` arabirminden şu prompt'u verdiniz "Projeyi analiz et ve bu yapıya uygun `Architecture Decision Record (ADR)` dokümanlarını otomatik olarak oluştur."

Bir yapay zeka asistanının mimari karar dokümanları üretmesi ile ilgili olarak aşağıdakilerden hangisi söylenebilir?

- A) Yapay zekanın ürettiği kararlar kesinlikle uygulanmalıdır çünkü büyük dil modelleri kurumsal olarak kabul görmüş mimari standarlarla eğitilmiştir.
- B) Yapay zeka kodu analiz edemez, bu sebeple böyle bir komut her zaman hata döndürür.
- C) Asistan tarafından oluşturulan `ADR` dokümanları taslak olarak kabul edilmeli ve kararlar mutlaka yazılım mimarı/geliştirici tarafından doğrulanıp onaylanmalıdır.
- D) Sadece Python projeleri yapay zeka tarafından analiz edilebilir, .NET projelerinde böyle bir özellik yoktur.

## Soru 13

Yapay zeka destekli yazılım geliştirme süreçlerinde, geliştiricilerin yapay zeka asistanlarından gelen çıktıları dikkatlice inceleyip gerektiğinde müdahale ederek ilerlemeleri önemlidir. Bu süreçte kod güvenilirliği, teknik borç ve proje mimarisi gibi konulara dikkat etmek gerekmektedir. Bu bağlamda, Newtonsoft' un oldukça popüler olan Json kütüphanesini projenizde kullanmak istediğinizi düşünün. Projeyi `Nuget` paket yöneticisi ile sisteme ekledikten sonra şu prompt'u verdiniz: "Bu kütüphaneyi kullanarak bir JSON serileştirme ve deserileştirme işlemi gerçekleştiren örnek bir kod parçası oluştur." Ancak kodu çalıştırdığınızda yapay zeka asistanınızın aslında var olmayan, hatalı ve uydurma bilgilerle son derece mantıklı ve bir o kadarda kendinden emin bir şekilde kod ürettiğini gördünüz. Bu durum literatürde ne şekilde tanımlanır?

- A) Dil modeli token sınırını aşmıştır ve bu nedenle eksik bilgiyle kod üretmiştir.
- B) Yapay zeka halüsinasyon *(Hallucination)* sorunu yaşamış var olmayan veya hatalı bilgileri gerçekmiş gibi sunarak kod üretmiştir.
- C) Yapay zeka asistanınızın eğitim verisi güncel değildir ve bu nedenle eski bir sürümle ilgili kod üretmiştir.
- D) Yapay zeka asistanınızın API anahtarı süresi dolmuştur ve bu nedenle eksik bilgiyle kod üretmiştir.

## Soru 14

`RAG (Retrieval-Augmented Generation)` yaklaşımını benimseyen bir yapay zeka uygulamasında, sisteme yüklenen belgeler parçalama *(Chunking)* işleminden geçirilir ve bir `Embedding modeli` kullanılarak sayısal vektörlere dönüştürülüp vektör veritabanına kaydedilir. Kullanıcı bir soru sorduğunda, bu soru da vektöre çevrilir ve veritabanındaki en alakalı metin parçalarını bulmak için **"vektörel benzerlik"** hesaplanır.

Aşağıda, yüksek boyutlu vektör uzaylarında iki vektör *(A ve B)* arasındaki benzerliği ölçmek için kullanılan yaygın bir yöntemin formülü verilmiştir. Bu formülde iki vektörün iç çarpımı *(dot product)*, vektörlerin büyüklüklerinin *(magnitudes)* çarpımına bölünmektedir:

$$ \text{Similarity}(A, B) = \cos(\theta) = \frac{A \cdot B}{\|A\| \|B\|} $$

Özellikle `RAG` tabanlı sistemlerde metinlerin uzunluğundan bağımsız olarak anlamsal yönlerini *(açılarını)* karşılaştırmak için oldukça sık tercih edilen bu yöntem şıklardan hangisidir?

- A) Cosine Similarity
- B) Jaccard Similarity
- C) Euclidean Distance
- D) Manhattan Distance

## Soru 15

Veri bilimi ekibinin `Python` ve `PyTorch` kullanarak harika bir makine öğrenmesi modeli geliştirip ve eğittiğini düşünelim. Ne var ki ana sunucu altyapısı .NET ve Go tabanlı çalışıyor. Modeli `Python` bağımlılıklarıyla canlıya almak yerine dilden ve framework'ten bağımsız, optimize edilmiş bir formata dönüştürüp doğrudan C# içerisinden yüksek performansla çalıştırmak *(inferencing)* istiyorsunuz. Derin öğrenme modellerinin farklı framework'ler *(TensorFlow, PyTorch vb.)* ve programlama dilleri arasında taşınabilmesini sağlayan açık kaynaklı standart aşağıdakilerden hangisidir?

- A) ONNX *(Open Neural Network Exchange)*
- B) JSON Web Token *(JWT)*
- C) Parquet
- D) YAML

## Soru 16

Bir yapay zeka uygulamasının pyhton ile yazılmış yapılandırma dosyasında aşağıdaki değişkenler tanımlanmıştır:

```python
LM_STUDIO_BASE_URL = "http://127.0.0.1:1234/v1"
EMBEDDING_MODEL = "text-embedding-embeddinggemma-300m"
EMBEDDING_DIM = 768

QDRANT_HOST = "localhost"
QDRANT_PORT = 6333
QDRANT_COLLECTION = "documents"

CHUNK_SIZE = 500
CHUNK_OVERLAP = 50

SUPPORTED_EXTENSIONS = {".txt", ".md", ".pdf", ".docx"}
```

Tanımlı yapılandırma değerleri ve `RAG` mimarisinin çalışma prensiplerine göre aşağıdaki ifadelerden hangileri doğrudur?

- I. Büyük belgeler sisteme yüklenirken 500 birimlik *(token/karakter)* parçalara ayrılacak; ancak cümlelerin veya paragraf bağlamının ortadan bölünmemesi *(anlam kaybı yaşanmaması)* için ardışık her bir parça, bir öncekinin 50 birimlik kısmını içerecek *(overlap)* şekilde kesişecektir.
- II. `Qdrant` veritabanında `documents` adıyla oluşturulacak olan koleksiyonun *(collection)* vektör boyutu mutlaka **768** olarak ayarlanmalıdır. Aksi takdirde modelin ürettiği vektörler veritabanına kaydedilemez.
- III. Sistem `PDF` ve `DOCX` gibi zengin içerikli dosyaları desteklediği için, bu dosyalar hiçbir metin ayıklama *(text parsing/extraction)* işlemine tabi tutulmadan doğrudan `Qdrant` veritabanına ikili *(binary)* formatta kaydedilecektir.
- IV. Verilen URL ve model ismine bakıldığında, metinleri matematiksel vektörlere dönüştürme işlemi için dışarıdan bir bulut API'si *(örn. OpenAI)* değil, yerel *(localhost)* ortamda barındırılan bir dil modeli kullanılmaktadır.

- A) Yalnızca I ve II
- B) Yalnızca II ve IV
- C) I, II ve IV
- D) Hepsi

DEVAM EDECEK

## Cevap Anahtarı

| Soru No | Doğru Cevap |
|---------|-------------|
| 1       | A           |
| 2       | B           |
| 3       | D           |
| 4       | C           |
| 5       | D           |
| 6       | D           |
| 7       | B           |
| 8       | B           |
| 9       | D           |
| 10      | A           |
| 11      | B           |
| 12      | C           |
| 13      | B           |
| 14      | A           |
| 15      | A           |
| 16      | C           |
