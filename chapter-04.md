<a id="top"></a>

# Bölüm 04: Hata Yönetimi, Loglama ve İzleme

<!-- nav:top:start -->
[← Bölüm 03](chapter-03.md#top) · [İçindekiler](README.md#top) · [Bölüm 05 →](chapter-05.md#top)
<!-- nav:top:end -->

Yazılım uygulamalarında hata yönetimi sadece .Net ve Java dillerinde olduğu gibi Exception yönetiminden ibaret değildir. Bu bölümde, farklı dillerde ve platformlarda hata yönetimi, loglama ve izleme konularına yer veriliyor. Hata yönetimi *(Error Handling)* konusundaki yanılgılardan birisi tüm hataların aynı şekilde ele alınması gerektiği düşüncesidir. Örneğin bir kullanıcının yanlış girdiği form alanı sonucu oluşan hata ile bir dizinin sınırlarının dışına çıkılması sonucu oluşan hata aynı kategoride değerlendirilmemelidir. Bu anlamda aşağıdaki tabloyu bilgi amaçlı kullanabiliriz.

| **Kategori** | **Örnek** | **Kimin Hatası?** | **Ne Yapmalı?** |
| --- | --- | --- | --- |
| **Girdi / Doğrulama** *(Validation)* | Ürün tutarı alanına `0` girilmesi, eksik JSON alanı | İstemci *(client)* | Anlamlı bir mesajla reddebiliriz *(4xx)*. Sistemin sağlığıyla ilgisi yoktur. |
| **İş Kuralı** *(Business Rule)* | Stok yetersiz, müşteri bulunamadı gibi | Kimsenin hatası değildir, beklenen bir durumdur | Alan *(domain)* diline uygun bir hata dönülebilir *(404, 409)*. |
| **Geçici Altyapı** *(Transient Infrastructure)* | Ödeme servisi zaman aşımına uğrar *(timeout)*, veritabanı bağlantısı kopar vb | Dış bağımlılık | Yeniden dene *(retry)* yine de olmazsa nazikçe başarısız ol *(503)*, **alarm çalıştır**. |
| **Programlama Hatası** *(Bug)* | `null` referans, `unwrap()` ile panic, sıfıra bölme | Geliştirici | Yakalamaya çalışmak yerine **düzeltmeliyiz**. Çalışma zamanında sadece sistemi ayakta tutmak, log bırakmak ve alarm çalıştırmak. |

İlk üç kategori **beklenen** *(expected / recoverable)* hatalardır. Yazdığımız kod bu hatalarla nasıl başa çıkacağını bilir. Dördüncü satırda ise **beklenmeyen** *(unexpected / unrecoverable)* hata söz konusudur. Onu ancak bir test, bir log satırı veya bir alarm bize haber verebilir.

> Beklenen hatalar **tip sistemi veya exception hiyerarşisi** ile modellenir. Beklenmeyen hatalar ise **gözlemlenebilirlik** *(loglama + izleme)* ile yakalanır.

Tabii dillere göre hata yönetimi felsefesi de değişir. Bir önceki modülde daha çok managed dillerdeki exception yönetimi üzerinde durulmuştu. Bu felsefeyle ilgili olarak da aşağıdaki tablodan bazı çıkarımlar yapabiliriz.

| **Model** | **Diller** | **Fikir** | **Pros** | **Cons** |
| --- | --- | --- | --- | --- |
| **Exception** | C#, Java, Python, JavaScript | Hata fırlatılır *(throw/raise)*, çağrı yığınında *(call stack)* yukarı doğru bir yakalayıcı bulana kadar ilerler. | İdeal akış *(happy path)* temiz kalır ve merkezi yakalama kolaydır. | Bir fonksiyonun hangi hataları fırlatacağı imzasından anlaşılmaz. Unutulan hata çalışma zamanında patlar. Stack unwinding maliyetlidir. |
| **Error Value** | Go, C | Fonksiyon sonuçla birlikte bir hata değeri döndürür, `val, err := f()` gibi | Açık ve öngörülebilir olması avantajdır. Fonksiyonun nasıl bir hata döneceği bellidir. | `if err != nil` tekrarı bir süre sonra fazlalaşır ve çağıran tarafta hatayı kontrol etmeyi unutmak da mümkündür. |
| **Tip Sistemi ile Hata** | Rust *(Result)*, Haskell *(Either)*, F#, Swift | Hata, dönüş tipinin bir parçasıdır: `Result<T, E>`. Derleyici hatayı ele almayı zorunlu kılar. | Hata yolları derleme zamanında görünür ve unutmak mümkün değildir. | Başlangıçta daha fazla tip tanımı ve dönüşüm gerektirir. |
| **Let It Crash** | Erlang, Elixir *(Actor Model)* | Hatalı süreç ölür ve bir **supervisor** onu temiz bir state ile yeniden başlatır. | Dağıtık *(distributed)* ve hataya dayanıklı *(resilient)* sistemlerde büyük avantaj sağlar. | Bir supervisor ağacının tasarımını gerektirir. |

Burada bahsedilen modeller birbirini dışlamazlar. Örneğin Rust dilinde de kurtarılamaz durumlar için `panic!` kullanımı vardır, Python'da da istenirse sonuç nesneleri ile hata değeri döndürülebilir. Önemli olan hangi durumda hangi aracın kullanılacağının farkında olmaktır.

Hata yönetiminde dilden bağımsız bir takım önemli ilkeler ve kurallar da vardır;

- `catch {}` veya `except: pass` gibi boş bloklar kullanmayın. Hatayı ya ele alın, ya bir üst çağrıya iletin ya da en azından loglayın. Zira aksi taktirde hatayı sessizce yutmuş oluruz *(Do Not Swallow Errors)*.
- Geçersiz bir durumu olabildiğince erken ve oluştuğu yere yakın tespit edin. Bozuk veriyle ilerlemek hataları gizler ve teşhis etmeyi zorlaştırır *(Fail Fast)*.
- Üretim ortamlarında stack trace, SQL sorgusu, dosya yolu gibi bilgiler hassas veri olarak kabul edilir ve istemcilere gönderilmez. Bular loglara yazılır ve izleme sistemlerine iletilir. Aksi taktirde güvenlik ve gizlilik riskleri oluşur. *(Sensitive Data Exposure)*
- Hataları başka bir veriye çevirmemiz gereken durumlarda orijinal hatayı kaybetmemeye özen gösterin. Bu sayede hatanın kaynağını ve bağlamını koruyabiliriz. *(Preserve Context)*
- Özellikle web tabanlı sistemlerdeki hataları göz önüne alırsak HTTP response dönüşümlerini tek bir noktadan yönetmek düşünülebilir.
- Bazen hataların farklı katmanlara sızmaması gerekir. Örneğin veritabanı sürücüsünün verdiği KeyError veya HTTP istemcisinin TimeoutException'ı iş katmanına sızmamalı, alan diline uygun bir şekilde dönüştürülmelidir. Söz gelimi NotFound gibi bir hata PaymentGatewayError gibi dönüştürülmeli *(Translate at boundaries)*

Aynı hata, geliştirme ortamında ve üretim ortamında farklı ihtiyaçlar doğurur. Geliştirme zamanında amaç hatayı **hızla anlamak ve düzeltmek**, üretimde ise **sistemi ayakta tutmak, kullanıcıyı korumak ve hatayı fark edebilmek**tir.

| **Konu** | **Geliştirme Zamanı** | **Üretim Ortamı** |
| --- | --- | --- |
| **Kim okuyor?** | Geliştirici *(insan)* | Log toplayıcı, sorgu motoru *(makine)* ve tabii operasyon nöbetçisi |
| **Log formatı** | Renkli, okunabilir, çok satırlı | Tek satır **JSON** *(structured logging)* |
| **Log seviyesi** | `debug` | `info` *(gerekirse anlık olarak artırılır)* |
| **Stack trace / backtrace** | Konsolda, tam haliyle | Sadece loglarda ve izleme sistemlerinde ama **istemciye asla!** |
| **İstemciye hata mesajı** | Detaylı | 5xx için genel mesaj ve `request_id` |
| **Hatayı nasıl buluruz?** | Debugger, breakpoint, test | Log sorguları, metrikler, alarmlar |
| **Araçlar** | IDE, `cargo clippy`, `ruff`, `mypy`, unit test vb | Loki, Prometheus, Grafana, ve farklı alarm sistemleri |

Log gerçekten çok kıymetli bir bilgidir. Farklı seviyeleri vardır ve bazen doğru şekilde ele alınmazlar. Loglar, Error, Warn, Info ve Debug olmak üzere dört farklı seviyeye sahiptir. Sık yapılan hatalardan birisi 4xx durumlarını hep Error olarak loglamaktır. Aslında bir kullanıcının yanlış ID girmesi sistemin hatası değildir. Bu yüzden her 404'ü Error olarak loglamak, gerçek hataları gürültülü bir şekilde gizler ve teşhis etmeyi zorlaştırır *(Bu konuyla ilgili alert fatigue kavramına bakılabilir)*.

Üstteki tabloda yer alan önemli kavramlardan birisi de request_id'dir. Aslında her isteğe özel olarak üretilir ve loglarda, izleme sistemlerinde ve hata mesajlarında kullanılır. Bu sayede belirli bir isteğin izini sürmek ve hataları ilişkilendirmek mümkün olur.

## Örnek Çalışma

`error-handling` klasöründe Rust ve Python dilleri ile yazılmış iki örnek servis yer almaktadır. Aynı HTTP Api'sini sunarlar ama hata yönetimleri kendi dillerine özgüdür. Bu örneklerde amaç aynı hata senaryolarının farklı felsefelerde nasıl ele alındığını görebilmektir. Servis noktalarını aşağıdaki gibi özetleyebiliriz.

| **Endpoint** | **Başarılı** | **Hata Senaryoları** |
| --- | --- | --- |
| `GET /orders/{id}` | 200 | **404** sipariş yok |
| `POST /orders` | 201 | **422** geçersiz miktar / bozuk JSON, **409** stok yetersiz |
| `POST /orders/{id}/pay` | 200 | **503** ödeme sağlayıcısı 3 denemede de başarısız *(geçici altyapı hatası, retry ile)* |
| `GET /debug/bug` | - | **500** bilerek bırakılmış bir **bug** *(Rust: panic, Python: yakalanmamış exception)* |
| `GET /metrics` | 200 | Prometheus metrikleri |

Tüm hatalar istemciye aynı formatta döner:

```json
{ "error": { "kind": "out_of_stock", "message": "'monitor' için yetersiz stok (istenen: 10, mevcut: 5)" } }
```

İstemciye dönen her yanıtta ayrıca bir `x-request-id` başlığı *(header)* bulunur. İstemci bir hata bildirdiğinde bu değer ile ilgili tüm log satırlarına ulaşabiliriz.

Klasör yapısı aşağıdaki gibidir.

```text
apps/lesson02/error-handling/
├── docker-compose.yml          # Servisler + Loki + Alloy + Prometheus + Grafana
├── rust-order-service/         # axum, thiserror, tracing, prometheus
├── python-order-service/       # FastAPI, logging, prometheus_client
├── observability/              # Loki, Alloy, Prometheus ve Grafana ayarları
└── scripts/traffic.py          # Karışık (başarılı + hatalı) trafik üretici
```

Ortamı ayağa kaldırmak için;

```bash
cd apps/lesson02/error-handling
docker compose up -d --build
docker compose ps
```

Eğer bir sıkıntı olmazsa aşağıdaki gibi servisler çalışıyor olmalıdır.

| **Arayüz** | **Adres** | **Kullanım Amacı** |
| --- | --- | --- |
| Grafana | `http://localhost:3000` *(admin / admin)* | Prometheus'taki metrikleri ve Loki'deki logları **tek bir ekranda** birleştirir. Hazır gelen *"Hata Yönetimi - Sipariş Servisleri"* dashboard'u ile hata oranı, hata türleri, yanıt süreleri ve hata logları izlenir. **Explore** menüsünden LogQL / PromQL sorguları çalıştırılır. Kendi başına veri saklamaz, sadece veri kaynaklarını sorgular ve görselleştirir. |
| Prometheus | `http://localhost:9090` | Servislerin `/metrics` uç noktalarını 5 saniyede bir **çekerek** *(pull / scrape)* sayısal metrikleri zaman serisi olarak saklar. *"Son 5 dakikada 5xx oranı ne?"*, *"Kaç panic oldu?"* gibi **ne kadar / ne sıklıkla** sorularına cevap verir. **Status -> Targets** sayfasından servislerin erişilebilir olup olmadığı görülebilir. |
| Prometheus Alarmları | `http://localhost:9090/alerts` | `alerts.yml` dosyasındaki kuralların *(`ServiceDown`, `HighServerErrorRate`, `UnexpectedCrash`)* anlık durumunu gösterir: **Inactive** -> **Pending** *(koşul sağlandı, `for` süresi bekleniyor)* -> **Firing** *(alarm aktif)*. Gerçek sistemlerde bu alarmlar Alertmanager ile Slack, e-posta gibi kanallara yönlendirilir. |
| Alloy *(log boru hattı)* | `http://localhost:12345` | Container loglarını toplayıp Loki'ye taşıyan **ajan** *(collector)*. Arayüzde log boru hattının bileşenlerini *(keşif -> etiketleme -> JSON ayrıştırma ->gönderim)* bir graf olarak ve her bileşenin sağlık durumunu görürüz. Loglar Grafana'ya ulaşmıyorsa ilk bakılacak yer burasıdır. Neden kullandığımız aşağıda açıklanmıştır. |
| Loki | `http://localhost:3100` | Logların saklandığı ve LogQL ile sorgulandığı yer. Kendi arayüzü yoktur, Grafana üzerinden kullanılır. Logların tamamını değil sadece etiketlerini *(`service`, `level` gibi)* indekslediği için **Elasticsearch** gibi çözümlere göre çok daha az kaynak tüketir. `http://localhost:3100/ready` adresi Loki'nin hazır olup olmadığını söyler. |
| Rust API | `http://localhost:8081` | Hataları `Result` ve `enum` ile **değer olarak** yöneten sipariş servisi. `/metrics` adresinden Prometheus'un okuduğu ham metrikler görülebilir. |
| Python API *(Swagger)* | `http://localhost:8082/docs` | Aynı API'yi **exception hiyerarşisi** ile sunan sipariş servisi. Swagger arayüzü üzerinden hata senaryoları tarayıcıdan denenebilir. Metrikler `http://localhost:8082/metrics/` adresindedir. |

> İlk derleme Rust bağımlılıkları nedeniyle birkaç dakika sürebilir. Ana dizindeki `docker-compose.yml` ile port çakışması olmaması için Prometheus *(9090)*, Grafana *(3000)* ve Loki *(3100)* portlarını kullanan başka bir container olmadığından emin oolmakta yarar var.

**Neden Alloy kullandık?**

Rust ve Python ile geliştirilmiş örnek servisler loglarını sadece **stdout**'a JSON olarak yazmaktalar. Loki'yi bilmezler *(12-Factor App, Logs ilkesi)*. Loki ise kendisi gidip log toplamaz, bunun yerine logların ona gönderilmesini *(push)* bekler. Bu iki uç arasında container loglarını okuyup Loki'ye taşıyacak bir aracıya ihtiyaç olduğundan Alloy devreye girer. Alloy bu boşluğu dolduran log toplayıcı ajan olarak düşünülebilir.

Bu işi yapmanın başka yolları da var.

| **Alternatif** | **Neden Tercih Etmedik?** |
| --- | --- |
| Uygulamanın logları doğrudan Loki'ye göndermesi | Her dil için ayrı bir kütüphane gerekir *(Rust ve Python için iki farklı entegrasyon)*. Uygulama altyapıya bağımlı hale gelir ve Loki değişirse kodu da değiştirmek gerekir. Loki erişilemezse loglar kaybolabilir veya uygulama yavaşlayabilir. |
| Docker'ın Loki logging driver eklentisi | Her makineye ayrıca eklenti kurmak gerekir, Docker Desktop *(Windows / macOS)* ortamlarında sorun çıkarabilir. Loki'ye ulaşılamadığında container'ları da etkileyebilir. |
| Promtail | Eskiden bu iş için kullanılan Grafana aracıydı fakat Grafana da artık Promtail yerine Alloy'u kullanmayı öneriyor. Tabii bu da değişebilir :D |

Alloy'un bu örnekte yaptığı işler *([config.alloy](apps/lesson02/error-handling/observability/alloy/config.alloy))* dosyasında tanımlanmıştır.

1. **Keşif** *(discovery)*: Docker soketi üzerinden çalışan container'ları bulur. Sadece `logging=alloy` etiketine sahip olanları alır. Böylece Loki, Grafana gibi altyapı container örneklerinin logları birbirine karışmaz.
2. **Etiketleme ***(labeling)***: docker-compose servis adını `service` etiketi olarak ekler. Grafana'da `{service="rust-order-service"}` diye sorgulayabilmemiz mümkün hale gelir.
3. **Ayrıştırma ve normalleştirme**: JSON log satırından `level` alanını okur. Rust `WARN`, Python `WARNING` yazdığı için ikisini de `warn` değerine çevirir ve böylece iki dilin logları tek bir sorguyla filtrelenebilir.
4. **Güvenilir gönderim**: Logları Loki'ye gönderir. Loki kısa süreliğine erişilemezse bekletip yeniden dener.

> Uygulama kodu tek satır bile değişmeden log altyapısını değiştirebiliyor olmamız bu ayrımın en büyük kazancıdır. Yarın Loki yerine başka bir sistem kullanmak istersek sadece Alloy ayarını değiştiririz. Ayrıca Alloy sadece log değil, metrik ve **OpenTelemetry** izlerini *(traces)* de toplayabilen tek bir ajan olarak tasarlanmıştır. Daha büyük sistemlerde Prometheus scraping ve trace toplama işleri de aynı ajan tarafından değerlendirilebilir.

Çalışmayı görmenin en iyi yolu python script'i ile trafik üretmektir.

```bash
python scripts/traffic.py --duration 300
# Daha fazla bug görmek için:
python scripts/traffic.py --duration 300 --bug-rate 0.1
```

Sonrasında `http://localhost:3000` adresinden Grafana arayüzüne giderek oluşan trafik ve hataları gözlemleyebiliriz.

![day_02_03](./images/day02_03.png)

Birde alarm simülasyonu yapabiliriz. Örnekte üç seviyeli bir alarm mekaniği simüle edilmektedir. Bu kuralları prometheus'un [alerts.yml](./apps/lesson02/error-handling/observability/prometheus/alerts.yml) dosyasında tanımladık.

| **Alarm** | **Koşul** | **Anlamı** |
| --- | --- | --- |
| `ServiceDown` | `up == 0` *(15 sn)* | Servis erişilemiyor |
| `HighServerErrorRate` | 5xx oranı > %10 *(30 sn)* | Kullanıcılar etkileniyor |
| `UnexpectedCrash` | Son 1 dk'da panic / unhandled > 0 | Kodda bug var |

Söz konusu alarm seviyeleri için aşağıdaki python script'i ile testler başlatabiliriz.

```bash
# Bug Alarmı Simülasyonu
python scripts/traffic.py --duration 120 --bug-rate 0.2

# Ödeme sağlayıcısının iyice güvenilmez hale getirilmesi (docker-compose.yml'de PAYMENT_FAILURE_RATE: "0.9")
# ve sonrasında yeniden trafik simülasyonu yapıyoruz.
docker compose up -d rust-order-service
python scripts/traffic.py --duration 120

# Python servisini durduralım
docker compose stop python-order-service
```

`http://localhost:9090/alerts` sayfasında alarmların önce **Pending**, `for` süresi dolunca **Firing** durumuna geçtiği görülür. Gerçek bir sistemde bu alarmlar **Alertmanager** veya **Grafana Alerting** üzerinden Slack, e-posta, PagerDuty gibi kanallara da yönlendirilir.

![day_02_04](./images/day02_04.png)

Python servisi durdurulduğunda da bir alert tetiklenir ve `http://localhost:9090/alerts` sayfasında bu durum gözlemlenebilir.

![day_02_05](./images/day02_05.png)

Pek tabii hatalar yapay zeka araçları ile analiz ettirilebilir. Burada bağlamın *(context)* kalitesi de oldukça önemlidir. Sadece, `HTTP 500 hatası alıyorum, düzelt` gibi yüzeysel bilgiler iletmek yerine, hatanın oluştuğu senaryo, loglar ve sistem durumu gibi detaylar da sağlanabilir. Söz gelimi elimizde Loki'den alınmış JSON bazlı log satırları varsa şöyle bir prompt pekala işe yarayacaktır.

```text
Prompt: Aşağıda rust-order-service'e ait, aynı request_id'ye sahip log satırları var.
Hatanın kök nedenini bul, ilgili kaynak dosyadaki satırı göster ve unwrap kullanmadan,
AppError::Validation dönen bir düzeltme öner. Düzeltme için bir unit test de yaz.

<Loki'den kopyalanan JSON log satırları>
```

Yapısal loglar ve `request_id` sayesindeki yapay zeka modellerine tek bir isteğin tüm hikayesini verebiliriz.

## Dikkat edilmesi gerekenler

- Yapay zeka ajanları, "hata vermesin" yönündeki isteklere sıklıkla **her şeyi `try/except Exception: pass` ile sarmalayarak** veya `unwrap_or_default()` ile hatayı sessizce yutarak cevap verebilir. Bu yaklaşım hatayı düzeltmek değil gizlemektir. Üretilen kodu mutlaka gözden geçirin ve hataların doğru şekilde ele alındığından emin olun.
- Üretim loglarını bir yapay zeka aracına yapıştırmadan önce içinde **kişisel veri, token, parola** olmadığından emin olun *([Bölüm 18 - Güvenlik](chapter-18.md#top))*.
- Modelden hata yollarını kapsayan testler üretmesini isteyin *(Negative path testing)*. Bu sayede yazılımın geçersiz verilerle, hatalı kullanıcı eylemleriyle veya beklenmeyen durumlarla karşılaştığında nasıl davrandığını test edebilirsiniz.
- Bu kurallar bir projede kalıcı hale getirilecekse, [Bölüm 14](chapter-14.md#top)'te ele alınan `custom agent / skill` veya proje talimat dosyalarına *(`copilot-instructions.md`, `CLAUDE.md` vb.)* yazılabilir. Örneğin: *"Handler'larda try/catch kullanma, AppError döndür. 4xx durumlarını WARN olarak logla."*

<!-- nav:bottom:start -->
---

| ← Önceki Bölüm | İçindekiler | Sonraki Bölüm → |
| :--- | :---: | ---: |
| [Bölüm 03: Exception Handling, Debugging ve Docker Kullanımı](chapter-03.md#top) | [İçindekiler](README.md#top) | [Bölüm 05: Bağımlılıkları Yönetmek ve Kod Kalitesini Ölçmek](chapter-05.md#top) |
<!-- nav:bottom:end -->
