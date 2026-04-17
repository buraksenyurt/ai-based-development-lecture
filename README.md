# Yapay Zeka Destekli Yazılım Geliştirme

Konya Gıda ve Tarım Üniversitesi Yazılım Mühendisliği ve Pamukkale Üniversitesi Elektrik-Elektronik Mühendisliği ve Yönetim Bilişim Sistemleri bölümleri için açılan derse ait doküman ve örnek uygulamaların yer aldığı bir depodur.

- [Yapay Zeka Destekli Yazılım Geliştirme](#yapay-zeka-destekli-yazılım-geliştirme)
  - [Önsöz](#önsöz)
  - [Yapay Zeka Alanındaki Anahtar Terimler](#yapay-zeka-alanındaki-anahtar-terimler)
  - [Gün 00 - Tanışma ve `Hello World` Uygulamasının Geliştirilmesi](#gün-00---tanışma-ve-hello-world-uygulamasının-geliştirilmesi)
    - [Bu çalışmadan çıkarılması gereken dersler](#bu-çalışmadan-çıkarılması-gereken-dersler)
  - [Gün 01 - CV Bank Projesi için Prototip Geliştirme](#gün-01---cv-bank-projesi-için-prototip-geliştirme)
  - [Gün 02 - Exception Handling, Debugging ve Docker Kullanımı](#gün-02---exception-handling-debugging-ve-docker-kullanımı)
    - [Dikkat Edilmesi Gereken Noktalar](#dikkat-edilmesi-gereken-noktalar)
  - [Gün 03 - Bağımlılıkları Yönetmek ve Kod Kalitesini Ölçmek](#gün-03---bağımlılıkları-yönetmek-ve-kod-kalitesini-ölçmek)
  - [Gün 04 - Yazılım Çözümlerinde Testin Önemi](#gün-04---yazılım-çözümlerinde-testin-önemi)
  - [Gün 05 - Yazılım Mimarileri ve Temel Seviyede Bir Örnek](#gün-05---yazılım-mimarileri-ve-temel-seviyede-bir-örnek)
  - [Gün 06 - Dağıtık Sistemler Hakkında Temel Bilgiler ve Basit Bir Senaryo Üzerinden İnceleme](#gün-06---dağıtık-sistemler-hakkında-temel-bilgiler-ve-basit-bir-senaryo-üzerinden-i̇nceleme)
  - [Gün 07 - RAG (Retrieval Augmented Generation) Yaklaşımı I](#gün-07---rag-retrieval-augmented-generation-yaklaşımı-i)
    - [Bilgi Sağlama ve Bağlam (Context) Yönetimi](#bilgi-sağlama-ve-bağlam-context-yönetimi)
  - [Gün 08 - RAG (Retrieval Augmented Generation) Yaklaşımı II](#gün-08---rag-retrieval-augmented-generation-yaklaşımı-ii)
  - [Aman Dikkat](#aman-dikkat)
  - [Ders Geçme Prosedürü](#ders-geçme-prosedürü)
    - [Proje Değerlendirmesi](#proje-değerlendirmesi)
    - [Final Sınavı](#final-sınavı)
  - [Uygulama Önerileri](#uygulama-önerileri)
- [Terimler Sözlüğü](Dictionary.md)

## Önsöz

Yapay zeka araçları günümüzün en popüler konularından biri olsa da, bu araçların yazılım geliştirme süreçlerine entegrasyonu henüz tam olarak anlaşılmış değil. Bu dersin amacı, yapay zeka destekli yazılım geliştirme süreçlerini anlamak ve bu süreçlerde karşılaşılabilecek zorlukları ele almaktır. Ders boyunca, yapay zeka araçlarının yazılım geliştirme süreçlerine nasıl entegre edileceği, avantajları/dezavantajları ve bu araçların kullanımı sırasında karşılaşılabilecek zorluklar üzerinde durulacaktır. Ayrıca, yapay zeka destekli yazılım geliştirme süreçlerinde güvenlik, teknik borç ve proje mimarisi gibi önemli konulara da değinilecektir. Dönem boyunca aşağıdaki soruların cevaplarını arayacağız:

- Yapay zeka araçları hangi vakalarda yazılım geliştirme süreçlerine entegre edilebilir?
- Yapay zeka araçlarının yazılım geliştirme süreçlerine entegrasyonunun avantajları ve dezavantajları nelerdir?
- Bir yapay zeka aracının ürettiği çıktıda hangi konulara dikkat etmek gerekir? Riskler nelerdir? Risklerden nasıl kaçınılır?
- Yapay zeka destekli yazılım geliştirme süreçlerinde güvenlik, teknik borç ve proje mimarisi gibi konular nasıl ele alınmalıdır?
- İdeal veya ideale yakın, hata payı düşük çıktıları elde etmek için ne gibi metodolojiler izlenebilir? Hangi teknikler kullanılır?
- Spec veya test odaklı geliştirme gibi metodolojiler yapay zeka destekli yazılım geliştirme süreçlerinde nasıl uygulanabilir?
- Yapay zeka araçlarındaki sık değişimlere adapte olmak için ne gibi stratejiler izlenebilir?
- Kaynak tüketimi yüksek yapay zeka araçlarını kullanırken maliyetleri kontrol altında tutmak için ne gibi önlemler alınabilir? Optimizasyon teknikleri nelerdir?

## Yapay Zeka Alanındaki Anahtar Terimler

Yazılım geliştirme süreçlerinde yapay zeka araçlarından verimli şekilde yararlanmak için bazı temel terimlerin bilinmesi önemlidir. Bu terimler yapay zeka ile ilgili konularda başrolde yer alır. Tüm zamanların en üretken 200 mucidinden biri olarak görülen **IBM Baş Mucitlerinden *(Master Inventor)*** [Martin Keen](https://www.ibm.com/think/insights/behind-the-scenes-with-tech-trailblazers-meet-martin-keen), yapay zeka konusundaki terminolojiyi kimya derslerinden aşina olduğumuz periyodik cetvelle ilişkilendirmektedir.

![AI Periodic Table](./images/AiPeriodicTable.png)

Sütunlar beş ayrı grubu temsil etmektedir. Bunları kısaca aşağıdaki gibi özetleyebiliriz.

- **Reactive:** Değişen girdiyle çıktının da değiştiği etkileşime ait enstrümanları barındırır. Herkesin az çok aşina olduğu **prompt**'lar en temel istek gönderme biçimini tarif eder. Modelin bir başka fonksiyonu çağırabilmesi, birden fazla modelin birbirini çağırarak çalışması gibi kavramlar da bu grupta yer alır.
- **Retrieval:** Yapay zeka sistemlerinin bilgiyi nasıl aradığı, sakladığı ve hatırladığı ile ilgili kavramlar yer alır. Örneğin metinlerin sayısal temsilcileri *(embeddings)* ve bunların bir vektör uzayında temsil edilmesi ve benzerlik ölçümleri yapılarak erişilmesi gibi kavramları bu grupta düşünebiliriz.
- **Orchestration:** Tek bir birimin yapamayacağı işlerde birden fazla öğenin bir araya getirilerek işlendiği yöntemleri içerir. **RAG (Retrieval Augmented Generation)** yaklaşımı veya ihtiyaç duyulan tüm altyapıyı sunan **Framework**'ler bu grupta yer alır.
- **Validation:** Sistemin güvenliğini, doğruluğunu ve etik standartlara uygunluğunu sağlamak için kullanılan yöntemler bu grupta yer alır. Yapay zekanın hatalı veya zararlı çıktılar üretmesini engellemek için gerekli araçlar söz konusudur. Guardrail araçları ile zararlı çıktıları engellemek mümkündür. Ayrıca sistemin kırılganlığını test etmek için **Red Teaming** gibi yöntemler kullanılır.
- **Models:** Her şeyin etrafında döndüğü model ailesi bu grupta yer alır. Büyük dil modellerinin yanı sıra görüntü ve ses işleyebilen çoklu modeller ve akıl yürütme *(reasoning)* süreçleri ile gelişmiş düşünme *(thinking)* modellerini içerir.

Tablonun satırları da 4 kategoriye ayrılır. Bunları aşağıdaki gibi özetleyebiliriz.

- **Primitives:** Yapay zeka dünyasının en temel yapı taşlarını, yerinde bir benzetmeyle atomlarını temsil eder. Buradaki öğeler daha küçük parçalara bölünemezler ve aslında tablodaki diğer karmaşık yapılar tarafından kullanılır; onların temellerini oluştururlar. Örneğin **prompt**'lar yapay zeka araçlarına verilen girdilerin temel birimi olarak düşünülebilir. **Embedding**'ler ise metinlerin sayısal temsilleri olarak yapay zeka sistemlerinin bilgiyi işlemesi için temel bir yapı sağlar. Pek tabii büyük dil modelleri de burada yer alır.
- **Compositions:** Primitive'lerin bir araya getirilmesiyle oluşan daha karmaşık yapılar bu satırda yer alır. Genellikle yapım aşamasında bir modelin yanına yapılandırılmış çıktılar ve araç entegrasyonları eklenerek işlevsel bir süreç tesis edilir. Bu nedenle fonksiyon çağırma, vektör veritabanları, RAG ve Guardrails gibi unsurlar burada yer alır.
- **Emerging:** Günümüzde hızla evrilen ve yapay zeka ekosisteminin biraz da uç noktalarını temsil eden teknolojileri ifade eder. Halen gelişmekte olan bir alan gibi düşünülebilir. Örneğin yapay zekanın iş birliği yaptığı çoklu ajan sistemleri, modellerin iç mantığını anlamaya yarayan araçlar veya yanıt vermeden önce uzun süre muhakeme yapabilen düşünme modelleri burada yer alır.

Bu bileşimler yine Martin tarafından örnek senaryolarda pekiştirilmiştir. Örneğin bir şirketin kendi iç dokümanlarını baz alarak geliştirdiği bir chat-bot uygulamasında RAG yaklaşımının nasıl kullanıldığı ve bu süreçte hangi araçların devreye girdiği aşağıdaki görselde olduğu gibi özetlenebilir.

![RAG Scenario](./images/RAGScenario.png)

Müfredat boyunca yukarıda bahsettiğimiz birçok kavrama değinme fırsatı bulacağız.

## Gün 00 - Tanışma ve `Hello World` Uygulamasının Geliştirilmesi

Bu ilk dersimizde **JSON** veri formatında tasarlanmış bir CV dosyasının analiz edilerek **HTML** formatında bir web sayfasına dönüştürülmesi üzerine çalışıldı. Anthropic'in **Claude Sonnet 4.5** modelini kullanarak bu dönüşümü gerçekleştirmek için çeşitli prompt'lar denedik.

İlk derste kullandığımız prompt'lar:

```text
Bu JSON içeriğini analiz et ve bir html sayfası hazırla.

Kullanıcı dostu bir arayüz sağla.
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

ve bunun üzerine **Node.js**'in **http-server** paketini kullanarak basit bir web sunucusu kurmasını istedik.

```text
Sunucuyu komut satırından başlatmak yerine bir nodejs uygulaması ile başlatmak istiyorum.
```

Web sunucusunu başlatmak için aşağıdaki komut kullanılabilir.

```bash
npm run dev
```

Nihai amacımız, orta ölçekte bir CV bankası uygulaması geliştirmek ve bu süreçte yapay zeka araçlarını kullanmaktır. Başlangıç aşamasında bu uygulamanın yüksek seviyede nasıl görüneceğine dair bir diyagram çizdik.

![High Level Diagram](./images/CvBankHighLevelDiagram.png)

**Ödev:** Amacımız bu diyagrama göre projemizi bir adım daha ileri götürmek. Nereden başlarsınız ve Agent'a nasıl bir prompt verirsiniz?

### Bu çalışmadan çıkarılması gereken dersler

- Hayata geçirmek istediğimiz proje fikri için hakim olduğumuz programlama dili ve framework'leri tercih etmeliyiz.
- AI agent'ları ile çalışırken açık ve net prompt'lar vermeliyiz.
- Üretilen kodların herhangi bir güvenlik açığı içermediğinden, teknik borç oluşturmadığından ve projenin genel mimarisine uygun olduğundan emin olmalıyız.
- Üretilen programda harici paket bağımlılıkları varsa, bu paketlerin güvenilir ve güncel olduğundan emin olmalıyız. Güvenlik açıkları içerebilecek eski paketlerden kaçınmalıyız.
- Komple bir proje yazdırmak yerine küçük parçalar halinde kod üretmenin daha verimli olabileceğini göz önüne alarak ilerlemeliyiz.

## Gün 01 - CV Bank Projesi için Prototip Geliştirme

İkinci dersimizde en temel seviyede mimari özet ve domain bilgileri içeren temel dokümanlar hazırlayarak ilerledik. [Lesson01](./apps/lesson01/docs/) klasöründen bu dokümanlara erişebilirsiniz.

YZ modeli olarak Claude Sonnet 4.6'yı kullandık. Prompt oturumuna [00-architecture-overiview](./apps/lesson01/docs/00-architecture-overiview.md) ve [01-domain-design](./apps/lesson01/docs/01-domain-design.md) dokümanlarını ekledik. Ardından aşağıdaki prompt'u işlettik.

```text
Bu dokümanları analiz et ve sadece backend tarafı için gerekli Solution'ı oluştur.
```

Beklediğimiz gibi **.NET 10** tabanlı bir solution oluşturuldu. Klasör bazlı bir ayrım olmasa da projeler **Clean Architecture** yaklaşımında belirtildiği gibi **Domain**, **Application**, **Infrastructure** ve **Presentation** katmanlarına ayrıldı. Domain katmanında User, Contact gibi entity'ler ve Resume gibi aggregate'ler tanımlandı. **API** katmanında ise **REST API** standartlarına uygun ve Resume aggregate'ine yönelik **CRUD *(Create, Read, Update, Delete)*** operasyonlarını içeren bir **Controller** oluşturulduğu görüldü.

Ayrıca veritabanı tarafı için MongoDB tercih edildiği ve bağlantı ayarlarının `appsettings.json` dosyasına eklendiği görüldü. Domain tasarımında **ContactType** adında bir enum tanımlanarak iletişim türlerinin sınırlı bir küme ile ifade edildiği gözlemlendi.

Solution ilk seferde derlenmedi; zira eksik NuGet paketleri vardı. Ancak ajan sorunları kendisi düzelterek projeyi derlenebilir hale getirdi. Projeyi çalıştırdığımızda **Swagger** arayüzünde tanımlı endpoint'lerin beklendiği gibi göründüğü ve çalıştığı görüldü.

![Swagger Runtime](./images/day01_00.png)

Ancak;

- Daha zengin ve kaliteli bir mimari tasarım dokümanı hazırlamanın daha iyi sonuçlar vereceği anlaşılıyor. Örneğin **API** standartlarının detaylı bir şekilde tanımlanması, API katmanında daha eksiksiz ve standartlara uygun bir **Controller** oluşturulmasını sağlayabilir. Listeleme endpoint'lerinin sayfalama desteği içermesi, veri oluşturma/güncelleme/silme endpoint'lerinin **HTTP** metodlarına uygun şekilde tasarlanması gibi detaylar mimari dokümanında ne kadar iyi tanımlanırsa, üretilen kodun kalitesi ve mimari uyumu o kadar artabilir.
- **Domain** tasarımının detaylı ve iyi tanımlanmış olması, üretilen kodun kalitesini ve mimari uyumunu artırabilir. Bu da ilgili domain hakkında yetkin bilgiye sahip olmayı ve **DDD *(Domain Driven Design)*** prensiplerini iyi bilmeyi gerektirmektedir. Domain tasarımında entity'lerin, aggregate'lerin ve value object'lerin doğru şekilde tanımlanması, kodun okunabilirliğini, bakımını ve genişletilebilirliğini artırır.

## Gün 02 - Exception Handling, Debugging ve Docker Kullanımı

Bu derste **Swagger** üzerinden yaptığımız API testleri sırasında aldığımız çalışma zamanı hatalarına istinaden **.NET** gibi yönetimli ortamlarda *(Managed Environment)* istisna/hata yönetiminin nasıl ele alındığına değindik. Özellikle **Exception** mesajlarındaki **Call Stack log'larının** nasıl okunması gerektiğine baktık ki gözlerimizi acıtan **Call Stack** içeriği de aşağıdaki gibiydi; ancak satır satır yorumladık.

```text
System.TimeoutException: A timeout occurred after 30006ms selecting a server using CompositeServerSelector{ Selectors = ReadPreferenceServerSelector{ ReadPreference = { Mode : Primary } }, LatencyLimitingServerSelector{ AllowedLatencyRange = 00:00:00.0150000 }, OperationsCountServerSelector }. Client view of cluster state is { ClusterId : "1", Type : "Unknown", State : "Disconnected", Servers : [{ ServerId: "{ ClusterId : 1, EndPoint : "Unspecified/localhost:27017" }", EndPoint: "Unspecified/localhost:27017", ReasonChanged: "Heartbeat", State: "Disconnected", ServerVersion: , TopologyVersion: , Type: "Unknown", HeartbeatException: "MongoDB.Driver.MongoConnectionException: An exception occurred while opening a connection to the server.
 ---> System.Net.Sockets.SocketException (10061): No connection could be made because the target machine actively refused it. [::1]:27017
   at System.Net.Sockets.Socket.DoConnect(EndPoint endPointSnapshot, SocketAddress socketAddress)
   at System.Net.Sockets.Socket.Connect(EndPoint remoteEP)
   at MongoDB.Driver.Core.Connections.TcpStreamFactory.Connect(Socket socket, EndPoint endPoint, CancellationToken cancellationToken)
   at MongoDB.Driver.Core.Connections.TcpStreamFactory.CreateStream(EndPoint endPoint, CancellationToken cancellationToken)
   at MongoDB.Driver.Core.Connections.BinaryConnection.OpenHelper(OperationContext operationContext)
   --- End of inner exception stack trace ---
   at MongoDB.Driver.Core.Connections.BinaryConnection.OpenHelper(OperationContext operationContext)
   at MongoDB.Driver.Core.Connections.BinaryConnection.Open(OperationContext operationContext)
   at MongoDB.Driver.Core.Servers.ServerMonitor.InitializeConnection(OperationContext operationContext)
   at MongoDB.Driver.Core.Servers.ServerMonitor.Heartbeat(CancellationToken cancellationToken)", LastHeartbeatTimestamp: "2026-02-27T12:20:55.5434132Z", LastUpdateTimestamp: "2026-02-27T12:20:55.5434133Z" }] }.
   at MongoDB.Driver.Core.Clusters.Cluster.SelectServerAsync(OperationContext operationContext, IServerSelector selector)
   at MongoDB.Driver.Core.Clusters.IClusterExtensions.SelectServerAndPinIfNeededAsync(IClusterInternal cluster, OperationContext operationContext, ICoreSessionHandle session, IServerSelector selector, IReadOnlyCollection`1 deprioritizedServers)
   at MongoDB.Driver.Core.Bindings.ReadPreferenceBinding.GetReadChannelSourceAsync(OperationContext operationContext, IReadOnlyCollection`1 deprioritizedServers)
   at MongoDB.Driver.Core.Operations.RetryableReadContext.AcquireOrReplaceChannelAsync(OperationContext operationContext, IReadOnlyCollection`1 deprioritizedServers)
   at MongoDB.Driver.Core.Operations.RetryableReadContext.CreateAsync(OperationContext operationContext, IReadBinding binding, Boolean retryRequested)
   at MongoDB.Driver.Core.Operations.FindOperation`1.ExecuteAsync(OperationContext operationContext, IReadBinding binding)
   at MongoDB.Driver.OperationExecutor.ExecuteReadOperationAsync[TResult](OperationContext operationContext, IClientSessionHandle session, IReadOperation`1 operation, ReadPreference readPreference, Boolean allowChannelPinning)
   at MongoDB.Driver.MongoCollectionImpl`1.ExecuteReadOperationAsync[TResult](IClientSessionHandle session, IReadOperation`1 operation, ReadPreference explicitReadPreference, Nullable`1 timeout, CancellationToken cancellationToken)
   at MongoDB.Driver.MongoCollectionImpl`1.FindAsync[TProjection](FilterDefinition`1 filter, FindOptions`2 options, CancellationToken cancellationToken)
   at MongoDB.Driver.IAsyncCursorSourceExtensions.ToListAsync[TDocument](IAsyncCursorSource`1 source, CancellationToken cancellationToken)
   at CvApp.Infrastructure.Persistence.Repositories.ResumeRepository.GetAllAsync(CancellationToken cancellationToken) in C:\Users\burak\Development\ai-based-development-lecture\apps\lesson01\src\CvApp.Infrastructure\Persistence\Repositories\ResumeRepository.cs:line 38
   at CvApp.Application.Services.ResumeService.GetAllAsync(CancellationToken cancellationToken) in C:\Users\burak\Development\ai-based-development-lecture\apps\lesson01\src\CvApp.Application\Services\ResumeService.cs:line 26
   at CvApp.Api.Controllers.ResumesController.GetAll(CancellationToken cancellationToken) in C:\Users\burak\Development\ai-based-development-lecture\apps\lesson01\src\CvApp.Api\Controllers\ResumesController.cs:line 23
   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.TaskOfIActionResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
   at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
   at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)
```

> Exception yönetimi her ne kadar işi kolaylaştıran bir yol olsa da runtime maliyetlerini de düşünerek **try...catch...finally** bloğuna ihtiyaç duymadan da bazı hataların yönetilebileceğini bilmemiz gerekir. Örneğin bir dosya üzerinde işlem yapan bir metot yazdığımızı düşünelim. Dosya üzerinde işlem yaparken dosyanın var olup olmadığını kontrol edebiliriz. Eğer dosya yoksa bu durumu bir istisna fırlatmak yerine, metot içerisinde yönetebiliriz. Bu sayede gereksiz yere **try...catch** bloğu kullanmayız, kodun okunurluğunu artırır, çalışma zamanını optimize ederiz. Ancak bazı hallerde istisna yönetimi kaçınılmaz olabilir. Örneğin, bir veritabanı bağlantısı kurarken, bağlantının başarısız olması gibi durumlarda istisna yönetimi kullanmak gerekebilir. Bu tür durumlarda, istisna yönetimi kullanarak hataları daha etkili bir şekilde ele alabilir ve uygulamanın çökmesini önleyebiliriz.

Ayrıca bir **.NET** uygulamasında nasıl debug yapılır, tarayıcılarda **Developer Tools** kullanılarak ağ trafiği ile request ve response bilgilerinin nasıl izlendiği konularına değindik. Bunun yanında [HTTP statü kodlarının](https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status) ne anlama geldiğine baktık.

Projede veritabanı olarak **MongoDB** kullanmaya karar vermiştik. Sisteme MongoDB ortamını kurmak yerine yerel *(local)* ortamda bir **[Docker](https://github.com/docker)** image'ı kullandık. Daha sonra çalıştığımız çözüme yeni servisler ekleyebileceğimiz için MongoDB servisini bir [docker-compose](./docker-compose.yml) dosyası içine aldık. Docker Compose dosyasında tanımlı olan servisleri ayağa kaldırmak oldukça basit. Bunun için terminalden aşağıdaki komutu çalıştırmak yeterli.

```bash
docker-compose up -d
```

**Windows** ve **macOS** gibi ortamlarda **[Docker Desktop](https://docs.docker.com/get-started/get-docker/)** uygulaması ile Docker imajları ve container'lar görsel bir arayüz üzerinden de yönetilebilir. **Linux** platformunda ise daha çok terminal üzerinden yönetim yapılır. Bazı temel ve ihtiyaç duyabileceğimiz komutların kullanımına ait basit örnekleri aşağıda bulabilirsiniz. Diğer yandan [Docker resmî sitesinde](https://docs.docker.com/get-started/docker_cheatsheet.pdf) faydalı bir **CLI Cheat Sheet** bulunmaktadır.

```bash
# Bir container başlatmak için
docker pull <image_name> # Eğer image localde yoksa önce çekilir
docker run --name <container_name> <image_name>

# Çalışan container'ları listele
docker ps

# Çalışan container'ları detaylarıyla listele
docker ps -a

# Belirli bir container'ın loglarını görüntüle
docker logs <container_id>

# Belirli bir container'ın içinde bash/sh terminali başlatmak için
docker exec -it <container_id> bash/sh

# Bir container'ı durdurmak için
docker stop <container_id>

# Bir container'ı başlatmak için
docker start <container_id>

# Bir container'ı silmek için
docker rm <container_id>

# Sistemdeki imajları listelemek için
docker images

# Sistemdeki imajları detaylarıyla listelemek için
docker images -a

# Kullanılmayan imajları silmek için
docker image prune

# Bir imajı silmek için
docker rmi <image_id>

# Docker compose ile tüm servisleri durdurmak için
docker-compose down

# Docker compose ile tüm servisleri yeniden başlatmak için
docker-compose restart
```

İşte birkaç denemeye ait ekran görüntüleri:

![day02_00](./images/day02_00.png)

![day02_01](./images/day02_01.png)

![day02_02](./images/day02_02.png)

Bu derste kullandığımız prompt'lar:

```text
Prompt 1: Analyze this solution and then apply exception handling on controller level.

Prompt 2: Create a docker-compose file on root folder and add mongodb service.

Prompt 3: This docker-compose should be run on different network because we have different docker environments.
```

MongoDB erişimi sırasında aldığımız Authentication hatası nedeniyle şu prompt'u kullandık:

```text
Case: Runtime exception
When: curl -X 'GET' \
 'http://localhost:5020/api/Resumes' \
  -H 'accept: text/plain'
Exception Details:
fail: CvApp.Api.Filters.ApiExceptionFilterAttribute[0]
      Islenmeyen hata olustu: Command find failed: Command find requires authentication.
      MongoDB.Driver.MongoCommandException: Command find failed: Command find requires authentication.
```

### Dikkat Edilmesi Gereken Noktalar

Bknz: [Aman Dikkat](#aman-dikkat)

## Gün 03 - Bağımlılıkları Yönetmek ve Kod Kalitesini Ölçmek

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

## Gün 04 - Yazılım Çözümlerinde Testin Önemi

![Day 04](./images/day04_00.png)

Yazılım geliştirme süreçlerinde testin önemli bir yeri vardır. Yazılan kodun beklentiler doğrultusunda çalıştığından emin olmak, hataları erken aşamada tespit etmek ve düzeltmek, kodun bakımını kolaylaştırmak gibi birçok avantaj sunar. Kodun kalitesinin artırılması açısından da biçilmiş kaftandır. Özellikle yapay zeka araçlarını veya metodolojilerini kullanarak kod üretirken, çıktıların beklediğimiz şekilde olduğundan emin olmak için de testlere başvurabiliriz. Günümüz yapay zeka asistanları koda bakarak eksik testleri yazabilmekte, var olan testleri analiz ederek kodun hangi bölümlerinin yeterince test edilmediğini tespit edebilmektedir. Ancak bu şekilde ilerliyor olsak da mutlaka yazılan testlerin anlamlı olup olmadığını gözden geçirmeliyiz.

Pek tabii kodun belli standartlar üzerinde olmasını sağlamak, sorunlarını azaltmak için test metodolojileri tek başına bir ölçüt değildir. **Sonarqube** gibi araçlar ile teknik borcu ölçmek, **Code Review/Pull Request** süreçlerini işletmek veya **pair programming** gibi pratiklerle ilerlemek de kod kalitesini artırmak için başvurulabilecek diğer yöntemlerdir.

Bu dersimizde diyagramda görülen bazı temel kavramlara değinmeye çalıştık. Bu anlamda klasik test piramidinin başlıca katmanlarını konuştuk. [Şu klasörde yer alan örnek projede](./apps/lesson04) birim test *(Unit Test)* ve entegrasyon testlerini *(Integration Test)* basit birkaç örnekle deneyimledik. Referans olarak uçtan uca bir deneyim için **Hexagonal Architecture** yaklaşımını benimseyen şu örneği de inceleyebilirsiniz: [Hexagonal Architecture Example](https://github.com/buraksenyurt/HexagonalArchitecture_101)

Ayrıca **Test Driven Development *(TDD)*** ya da *Red-Green-Blue* yaklaşımının temel prensiplerine baktık.

**.NET** tabanlı çözümümüzde solution oluşturmak ve proje eklemek için kullanabileceğimiz komutlar:

```bash
dotnet new sln -n DeppoApp
dotnet new classlib -n DeppoApp.Domain
dotnet new classlib -n DeppoApp.Application
dotnet sln add DeppoApp.Domain/DeppoApp.Domain.csproj
dotnet sln add DeppoApp.Application/DeppoApp.Application.csproj

# test projelerini oluşturup eklemek için
dotnet new xunit -n DeppoApp.Domain.Tests
dotnet new xunit -n DeppoApp.Application.Tests
dotnet sln add DeppoApp.Domain.Tests/DeppoApp.Domain.Tests.csproj
dotnet sln add DeppoApp.Application.Tests/DeppoApp.Application.Tests.csproj

# Solution içerisindeki tüm testleri koşmak için
dotnet test
```

Eksik birim testlerin tamamlanması ve entegrasyon testleri için kullandığımız prompt'lar:

**VS Code Copilot Tarafı (Model Claude Sonnet 4.6):**

```text
`Product` sınıfı için olası tüm birim testleri `ProductTests` bileşenine ekle.

`DecreaseStock` metodundaki son değişikliğe göre testleri düzelt. Gerekli görüyorsan `Product` sınıfına yeni iş kuralları ekle.
```

**Visual Studio Copilot Tarafı(Model Claude Sonnet 4.6):**

```text
Write an integration tests for `CreateProduct(Guid, string, decimal, int)` method into new xUnit test project. Use `Moq` framework for mocking real database.
```

> todo@buraksenyurt: UI testlerinde kullanılan araçlara bir örnek yapalım. Sisteme ürün ekleme senaryosunda **Playwright** ile bir arayüz testi eklenebilir.

Araştırılabilecek diğer kavramlar:

- Behavior Driven Development (BDD)
- User Acceptance Testing (UAT)
- Test Containers

## Gün 05 - Yazılım Mimarileri ve Temel Seviyede Bir Örnek

Bu dersimizde yazılım mimarileri konusunda genel ve yüzeysel bilgiler vermeye çalıştık. Konuya bir bayi otomasyon sisteminde yedek parça sipariş formu açılmasına ait ve aşağıdaki high-level diyagramda görülen örnek senaryo ile başladık.

![day_05_00](./images/day05_00.png)

Bu senaryoda bir bayi, yedek parça siparişi vermek istediğinde, sistemdeki stok durumunu kontrol eder. Eğer stokta yeterli miktarda ürün varsa, sipariş doğrudan işlenir. Ancak stokta yeterli ürün yoksa, sistem tedarikçi firmaya otomatik olarak bir sipariş oluşturur ve bayiyi bilgilendirir. Bu süreçte stok yönetimi, sipariş yönetimi ve tedarikçi entegrasyonu gibi farklı bileşenler devreye girebilir.

Kurumsal çaptaki uygulamalar bilinen belli başlı yazılım mimarileri çerçevesinde tasarlanır. **Layered Architecture**, **Microservices Architecture**, **Event-Driven Architecture**, **Serverless Architecture** gibi farklı mimari yaklaşımlar vardır. Her mimari yaklaşımın avantajları ve dezavantajları bulunur. Örneğin, Layered Architecture basit ve anlaşılır bir yapıya sahipken, Microservices Architecture daha esnek ve ölçeklenebilir çözümler sunabilir; ancak yönetimi ve kurulumu farklı yetkinlikler gerektirebilir. Bu nedenle, projenin ihtiyaçlarına, ekibin yetkinliklerine ve diğer faktörlere bağlı olarak en uygun mimari yaklaşımı seçmek önemlidir. Karar verme noktasında **Richards & Ford'un, Fundamentals of Software Architecture** kitabı referans olarak kullanılabilir. Mimarileri dağıtık ve monolitik olmak üzere iki ana kategoriye ayıran kitaba göre, farklı özellikler nezdinde bu mimarilerin avantajları ve dezavantajları aşağıdaki tablo ile özetlenebilir.

| **Özellik** | **Layered** | **Pipeline** | **Mikro Kernel** | **Service Based** | **Event Driven** | **Space Based** | **Service Oriented** | **Microservices** |
| --------- | --------- | ---------- | -------------- | --------------- | -------------- | ------------- | ------------------ | --------------- |
| **Partition Type** | Technical | Technical | Domain + Technical | Domain | Technical | Domain + Technical | Technical | Domain |
| **Number of Quanta** | 1 | 1 | 1 | 1..n | 1..n | 1..n | 1 | 1..n |
| **Deployability** | ★ | ★★ | | ★★★★ | ★★★ | ★★★ | ★ | ★★★★ |
| **Elasticity** | ★ | ★ | | ★★ | ★★★ | ★★★★ | ★★ | ★★★★★ |
| **Evolutianry** | ★ | ★★★ | | ★★★ | ★★★★★ | ★★★ | ★ | ★★★★★ |
| **Fault Tolerance** | ★ | ★ | | ★★★★ | ★★★★★ | ★★★ | ★★★ | ★★★★ |
| **Modularity** | ★ | ★★★ | | ★★★★ | ★★★★ | ★★★ | ★★★ | ★★★★★ |
| **Overall Cost** | ★★★★★ | ★★★★★ | ★★★★★ | ★★★★ | ★★★ | ★★ | ★ | ★ |
| **Performance** | ★★ | ★★ | ★★★ | ★★★ | ★★★★★ | ★★★★★ | ★★ | ★★ |
| **Reliability** | ★★★ | ★★★ | ★★★ | ★★★★ | ★★★ | ★★★★ | ★★ | ★★★★ |
| **Scalability** | ★ | ★ | ★ | ★★★ | ★★★★★ | ★★★★★ | ★★★★ | ★★★★★ |
| **Simplicity** | ★★★★★ | ★★★★★ | ★★★★ | ★★★ | ★ | ★ | ★ | ★ |
| **Testability** | ★★ | ★★★ | ★★★ | ★★★★ | ★★ | ★ | ★ | ★★★★ |

Derste işlenen diğer konular:

- **GitHub** sayfasında **Copilot Agent** kullanılarak lesson05 altında 3-tier mimarisine uygun örnek bir proje açılması istendi. Bu çalışmada Copilot'ın ayrı bir **branch** açması, bu branch üzerinden **code review** ve **pull request** işletilmesi gibi süreçler ele alındı. Bu senaryoda kullanılan prompt ise şu şekilde: *Create a new folder which name is lesson05 and than create a simple 3-tier based .net solution structure on this folder.*
- **Copilot CLI** aracından aynı solution için verilen plan dahilinde **[Architecture Decision Record (ADR)](https://martinfowler.com/bliki/ArchitectureDecisionRecord.html)** dokümanlarının oluşturulması istendi. **ADR**'ların ne olduğu, nasıl yazılması gerektiği ve neden önemli olduklarından bahsedildi. Pek tabii ajan tarafından oluşturulan dokümanların mutlaka gözden geçirilmesi, gerektiğinde müdahale edilmesi ve kararların mimari prensiplere uygun şekilde alınması gerektiğini bir kez daha vurgulayalım. Komut satırı aracı üzerinden kullanılan prompt ise şöyle:  */plan Analyse .net solution on lesson05. Create required `Architecture Decision Records` documents according the applied structure.*

> ADR dokümanları ile ilgili şunları söyleyebiliriz. Geri dönülmesi zor olan kararlar ADR'a girer. Örneğin PostgreSQL'den başka bir veritabanına geçmek kolay değil ve ciddi bir efor gerektiriyorsa bu bir risk teşkil eder ve bu nedenle bu karar ADR'a girmelidir. Ancak bir logging framework'ünden başka bir logging framework'üne geçmek çok kolaysa bu karar ADR'a girmeyebilir. ADR'ların amacı, mimari kararların neden alındığını, hangi alternatiflerin değerlendirildiğini ve bu kararların ne gibi sonuçlara yol açabileceğini belgelemektir. Bu sayede, gelecekte benzer kararlar alınırken geçmişteki deneyimlerden yararlanılabilir ve aynı hataların tekrarlanması önlenebilir.

## Gün 06 - Dağıtık Sistemler Hakkında Temel Bilgiler ve Basit Bir Senaryo Üzerinden İnceleme

## Gün 07 - RAG (Retrieval Augmented Generation) Yaklaşımı I

Yapay zeka tabanlı süreçlerde klasik akış aşağıdaki şekilde görüldüğü gibidir. Kullanıcılar bir **prompt** hazırlar ve yapay zeka modeline gönderirler. Model, verilen prompt'a göre bir çıktı üretir ve bu çıktı kullanıcıya geri döner. Günümüz yapay zeka modellerinin çoğu ön tarafta bir arayüz sağlar. Bu, basit bir chat penceresi olabileceği gibi geliştirme ortamındaki bir eklenti de olabilir. Tüm bu araçlar, kullanıcı ve yapay zeka dil modeli arasındaki oturum *(session)* sırasında sürece farklı materyallerin eklenmesine de olanak tanır. **Context** olarak da ifade edebileceğimiz bu bölümde yardımcı belgeler prompt ile birlikte dil modeline ulaşır.

![RAG 00](./images/day07_00.png)

Ne var ki genel dil modelleri önceden eğitilmiş verilerden ya da sağladığı araç desteği ile internet aramalarından yola çıkarak muhakeme *(reasoning)* sürecine girerler. Dil modelinin belli bir çerçevede çalışmasını istediğimiz durumlarda **context** içeriğini etkili bir şekilde hazırlamak da önemlidir. Kullanıcının isteğine konuyla ilgili ne kadar parça varsa dahil edilmesi, çıktının kalitesini artırabilir ve modelin daha iyi muhakeme yapmasına olanak tanır. Dil modellerinin deterministik olduğu ifade edilse de halüsinasyon görme ve bağlamı unutma eğilimleri vardır. Dolayısıyla aynı prompt için aynı dil modeli üzerinden farklı çıktılar üretilebilir. Sonuç aynı olsa da gidiş yolu ve detayda farklılaşmalar görülebilir. Bu nedenle bazı senaryolarda **RAG *(Retrieval Augmented Generation)*** yaklaşımını benimsemek daha iyi sonuçlar verebilir. **RAG** tekniğinde dil modeline verilen prompt'a ilaveten, modelin muhakeme yaparken kullanabileceği bir bilgi deposu da sağlanır. Bu bilgi deposu, modelin daha doğru ve tutarlı çıktılar üretmesine yardımcı olur. RAG yaklaşımında modelin, bilgi deposundan ilgili parçaları çekerek muhakeme sürecine dahil etmesi beklenir. Bu sayede, modelin deterministik olmayan doğası nedeniyle ortaya çıkabilecek tutarsızlıkların önüne geçilebilir ve daha kaliteli çıktılar elde edilebilir. Aşağıdaki şekilde bu kurgu basitçe ele alınmaktadır.

![RAG 01](./images/day07_01.png)

Bu yaklaşımda modelin kullanabileceği veri setinin vektörel olarak ifade edilişi çok önemlidir. Kavramlar arası yakınlıkların ölçümü için matematiğin doğasından yararlanılır. Herhangi bir paragrafın, bir terimin, kod tabanındaki semantik bir ifadenin ilişkili olduğu diğer parçaların tespitinde embedding araçlarından yararlanılarak mesafeler ölçülür. Modelin muhakeme sürecine dahil ettiği parçaların kalitesi, modelin ürettiği çıktının kalitesini doğrudan etkiler. Bu nedenle, bilgi deposunun iyi hazırlanması ve modelin bu bilgi deposundan doğru parçaları çekebilmesi için etkili bir vektörel ifade yöntemi kullanılması önemlidir. Bu da embedding modeline ve seçilen parçalama *(chunking)* stratejisine bağlıdır.

> Mesafeler genellikle **Kosinüs Benzerliği *(Cosine Similarity)*** veya **Öklid Mesafesi *(Euclidean Distance)*** gibi matematik yöntemlerle ölçülür ve vektörler arasındaki benzerlik veya uzaklık hesaplanır. Bu sayede, modelin bilgi deposundan çektiği parçaların prompt ile ne kadar ilişkili olduğu da değerlendirilebilir. Kosinüs benzerliği, iki vektör arasındaki açıyı ölçerken, Öklid mesafesi ise iki vektör arasındaki düz çizgi mesafesini ölçer. Hangi yöntemin kullanılacağı, uygulamanın ihtiyaçlarına ve veri setinin özelliklerine bağlı olarak değişebilir. Öklid uzaklığı *(Euclidean Distance)* iki nokta arasındaki fiziksel mesafeyi *(doğrudan uzaklığı)* ölçerken ve metnin uzunluğundan *(vektörün büyüklüğünden)* olumsuz etkilenirken; Kosinüs Benzerliği vektörlerin uzaydaki açısına *(yönüne)* bakar. Bu sayede biri çok uzun diğeri çok kısa ama aynı konudan bahseden iki metni **"birbirine çok benzer"** olarak doğru bir şekilde eşleştirebilir.

Derste işlediğimiz örnek senaryoda **Python** ile bir doküman setinin parçalanıp vektörel olarak ifade edilmesi, bu parçaların bir veritabanına kaydedilmesi adımı ele alınmıştır. **Text embedding** için **LM Studio** üzerinden host edilen **text-embedding-embeddinggemma-300m** kullanılmıştır. **Vektör** veritabanı olarak da **Rust** ile yazılmış olan **Qdrant** tercih edilmiştir. Parçalama stratejisi olarak da basitçe karakter sayısına göre bölme tekniği benimsenmiştir. Daha sofistike parçalama stratejileri de tercih edilebilir. Örneğin, doğal dil işleme teknikleri kullanarak cümle veya paragraf bazında bölme yapılabilir. Kod parçalarının bölünmesinde ise semantik analiz yaparak fonksiyon, sınıf veya modül bazında bölme yapılabilir. Parçalama stratejisi, modelin bilgi deposundan çektiği parçaların kalitesini etkileyebilir. Dolayısıyla parçalama stratejisinin dikkatli bir şekilde seçilmesi ve uygulanması önemlidir.

Bir vektör veritabanı hazırlandıktan sonra kullanıcılardan gelen prompt'lar da bir işleme tabi tutulur. Yani **prompt** için de bir vektör hesaplaması yapılır ve bu vektörün bilgi deposundaki diğer vektörlerle olan mesafeleri ölçülür. En yakın olan parçalar modele gitmeden önce sistem prompt'a dahil edilir. Modelin bu parçaları muhakeme sürecine dahil ederek daha kaliteli çıktılar üretmesi beklenir. Elbette sistem prompt'larının gönderilmesi sırasında alınması gereken tedbirler de olabilir. Bilgi deposundan çekilen parçaların kalitesini ölçmek, hassas bilgilerin istemeden de olsa gönderilmesini engellemek, modelin bu parçaları nasıl kullandığını izlemek gibi önlemler alınabilir *(Guardrails)*. Bu sayede, RAG yaklaşımının avantajlarından yararlanırken ortaya çıkabilecek risklerin de önüne geçilebilir.

> Bir modelin ürettiği çıktının firmanın kurumsal politikalarına uygunluğunu denetlemek ve veri sızıntılarını önlemek için [NeMo Guardrails](https://github.com/NVIDIA-NeMo/Guardrails) gibi güvenlik katmanları ele alınmalıdır.

Metinlerin birbirleriyle ilişkileri konusu aşağıdaki şekilde ele alınabilir. Ana kelimelerimiz **tuz** ve **biber** olarak belirlenmiştir. İlk akla gelen ilişkiler yemeklerle ve doğal olarak baharatlarla ilgilidir. Ancak coğrafi olarak tuz kelimesinin geçtiği **Tuz Gölü** ya da rahmetli **Barış Manço**'nun *Domates, Biber, Patlıcan* şarkısında geçen biber kelimesi ve hatta **hayatın tuzu biberi** ifadesi gibi farklı ilişkiler de ortaya çıkabilir. Modelin sorulan soruya göre doğru ilişkileri kurabilmesine yardımcı olabilecek düzenekler açısından bakıldığında, destekleyici bilgi depolarının kalitesi önemli hale gelir.

![RAG 02](./images/day07_02.png)

**RAG** burada özetlediğimiz kadarıyla iyi bir yaklaşım olsa da handikapları da vardır. En büyük sorunlardan biri, vektörel olarak yakın bulunan parçaların her zaman anlamsal olarak doğru cevabı içermemesidir. Bu nedenle vektör aramasından dönen sonuçların modele gitmeden önce belki bir re-ranking ile tekrar sıralanması gerekebilir.

### Bilgi Sağlama ve Bağlam (Context) Yönetimi

Yapay zeka dil modelleri ile çalışırken Context, Cache-Augmented Generation, Retrieval-Augmented Generation ve Model Context Protocol gibi kavramlarla karşılaşırız. Özellikle bağlamın kalitesini artırmak, modelin daha tutarlı ve doğru çıktılar üretmesini sağlamak için bu yöntemlere sıklıkla başvurulabilir. Ancak her bir yöntemi çeşitli kriterlere göre değerlendirmek gerekir. Aşağıdaki tabloda bu yöntemlerin bazı temel özellikleri karşılaştırılmıştır.

| **Özellik** | **Context** | **Cache-Augmented Generation** | **Retrieval-Augmented Generation** | **Model Context Protocol** |
| --------- | --------- | ----------------------------- | --------------------------------- | ------------------------- |
| **Çalışma Prensibi** | Tüm veriyi doğrudan prompt içine gömerek modele sunmak. | Devasa veriyi modelin önbelleğine *(Key-Value Cache)* tek seferde yükleyip, üzerinden işlem yapmak. | Veriyi vektörlere bölüp, sadece soruyla anlamsal eşleşen parçalarını modele sunmak. | Modelin dış sistemlere standart bir protokol ile bağlanıp anlık işlem yapmasını sağlamak. |
| **Kapasite** | Sınırlı *(Modelin token limiti kadar)* | Yüksek *(Modelin önbelleği kadar. 1, 2 milyon token)* | Sınırsız *(Vektör veritabanının kapasitesi kadar)* | Yüksek *(Modelin işlem yapabileceği her türlü veri kaynağı)* |
| **Maliyet ve Hız** | Her sorguda tüm veriyi baştan işlediği için yüksek maliyetli ve yavaş olabilir. | Veriyi tek seferde yükleyip, sonrasında hızlı işlem yapabilir. Ancak ilk yükleme maliyetlidir. | Sadece ilgili parçaları işlediği için genellikle daha hızlı ve düşük token maliyetlidir. | Modelin dış sistemlerle etkileşimine bağlı olarak değişkenlik gösterebilir. Genellikle hızlıdır ancak bu hız dış sistemlerin performansına bağlıdır. |
| **Mimari Gereksinim** | LLM'in kendisi | Geniş bağlam ve önbellek yetenekleri olan bir LLM | LLM + Embedding modeli + Vektör veritabanı | LLM + MCP Server + Dış sistemler |
| **Veri Güncelliği** | O an prompt'a dahil edilen veri kadar günceldir. | Önbellek *(Cache)* yenilenene kadar statik kalır, anlık olarak değişmez. | Vektör veritabanına yeni veri eklendikçe güncellenebilir. | Tamamen eş zamanlı ve canlıdır. |
| **Kullanım Senaryoları** | Kısa bir dokümanı özetlemek, birkaç sayfalık belgeyi incelemek, büyük olmayan kod parçalarını analiz etmek. | Çok sık değişmeyen büyük kurumsal yönetmelikler veya devasa statik kod tabanları. | Sürekli büyüyen ve değişen şirket analiz dokümanları, wiki'leri, geçmiş ticket aramaları. | Canlı veritabanından stok bilgisi çekme, GitHub'a kod gönderme, dosya I/O operasyonları |

Derste ele aldığımız uygulamayı **Claude Sonnet 4.6** modeline, **VS Code** arayüzünden aşağıdaki prompt'u vererek yazdırdık.

```text
Create a new terminal application written in Python. The purpose of this app is; Analyse documents, convert them into vectors via text embedding and store them into QDrant Db.

- Get source path from terminal.
- Use local LM Studio (hosting at http://127.0.0.1:1234)
- Use `text-embedding-embeddinggemma-300m` model.
- Use Qdrant as a vector database (Hosting at docker container)
- Measure the calculation time and give a summary result to user.
```

> Vektör veritabanını ve içerdiği koleksiyonları incelemek için `http://localhost:6333/dashboard/#/collections` adresine gidilebilir. Burada koleksiyonun içeriği, her bir parçanın hangi dokümana ait olduğu, vektörlerin boyutları vb. bilgilere erişilebilir. Ayrıca görsel graph diagramları ile parçaların birbirleriyle olan ilişkileri de incelenebilir.

## Gün 08 - RAG (Retrieval Augmented Generation) Yaklaşımı II

Bir RAG *(Retrieval Augmented Generation)* uygulaması yazmak için öncelikle bir bilgi deposu oluşturulması gerekir. Bu bilgi deposu, modelin muhakeme sürecine dahil edeceği parçaların saklandığı yerdir. Parçaların vektörel olarak ifade edilmesi ve bir veritabanına kaydedilmesinden sonra, kullanıcıların sorularını iletebileceği bir istemci uygulaması geliştirilir. Bu istemci *(Client App)*, kullanıcılardan gelen prompt'ları alır, bu prompt'lar için vektör hesaplaması yapar ve bilgi deposundan ilgili parçaları çekerek modelin muhakeme sürecine dahil eder *(Context Injection)*. Son aşamada modelin ürettiği çıktılar kullanıcıya döndürülür. Bu süreçte, modelin bilgi deposundan çektiği parçaların kalitesini ölçmek, hassas bilgilerin istemeden de olsa gönderilmesini engellemek, modelin bu parçaları nasıl kullandığını izlemek gibi önlemler alınabilir *(Guardrails)*. Bu sayede, RAG yaklaşımının avantajlarından yararlanırken ortaya çıkabilecek risklerin de önüne geçilebilir.

Bu dersimizde bir önceki derste geliştirilen **python** uygulamasını baz alarak bir istemci chatbot yazmaya çalıştık. Text embedding ve dil modeli için yine local ortamda çalışan **LM Studio** ortamına yüklenen modelleri kullandık. Vektör veritabanı olarak da yine docker üzerinden ayağa kaldırdığımız **Qdrant**'ı tercih ettik. Amacımız istemci tarafındaki setup'ı tamamlayarak, kullanıcıların sorularını alıp, bu sorulara göre bilgi deposundan ilgili parçaları çekip, modelin muhakeme sürecine dahil edip çıktılar üretmesini sağlamaktı. Yerel dil modellerimiz düşük kalitede ve kaynak olarak sadece bir markdown dosyası kullandığımız için sorulara verilen cevapların kalitesini görmezden geldik.

**lesson08** klasöründe yer alan uygulamayı yazdırmak için **Claude Sonnet 4.6** modeline **VS Code Copilot** arayüzünden aşağıdaki prompt'u verdik. *(Attachment kısmına lesson07 klasöründeki main.py dosyasını ekleyerek prompt'u verdik)*

```text
Create a new client app in lesson08 folder based on this python code.

Purpose: A chatbot application which is use RAG pipeline.
```

Daha önceki çalışmalardan farklı olarak bu sefer agent'ı **plan** modunda çalıştırdık. Plan modunda agent implementasyona başlamadan önce çoktan seçmeli bazı kritik sorular sorar. Bu sorulara verilen cevaplara göre bir taslak plan oluşturur. Bu plan, agent'ın nasıl ilerleyeceği konusunda bir yol haritası sağlar. Plan modunda çalışmak, özellikle karmaşık görevlerde agent'ın daha organize ve etkili bir şekilde hareket etmesine yardımcı olabilir. Yukarıda vermiş olduğumuz prompt oldukça basit ve genel bir ifadeye sahiptir. Buna göre agent bize üç soru sordu.

İlk soruda, istemci uygulamanın arabirimi hakkında bilgi istedi. Örneğin **CLI - Command Line Interface** mi yoksa **GUI - Graphical User Interface** mi olacağı gibi.

![RAG Client runtime 00](./images/day08_00.png)

İkinci soru oldukça isabetliydi. Kullandığımız prompt içerisinde muhakeme yapacak dil modelini açıkça belirtmemiştik. Bu bilginin kodda değiştirilebilir olarak tutulması veya bir çevre değişken olarak konfigurasyondan gelmesine dair bir soruyla karşılaştık.

![RAG Client runtime 01](./images/day08_01.png)

Son soruda ise konuşmanın aynı session içerisinde korunup korunmayacağı soruldu. Yani kullanıcının önceki sorularının ve modelin önceki cevaplarının hatırlanıp hatırlanmayacağı konusu gündeme geldi. Bu da bağlam yönetimi açısından önemli bir noktadır.

![RAG Client runtime 02](./images/day08_02.png)

Buna göre yazılan **python** kodunun ilk denemesinde bir çalışma zamanı hatası aldık. Sorunu analiz etmek için **LM Studio** penceresine akan log bilgilerini de kullandık ve hatanın kaynağını tespit ettik. Görüldüğü kadarıyla **QdrantClient** nesnesinin **search** metodu paketin ilgili sürümünde değişmişti. Problemin çözümü için yine **Claude Sonnet 4.6** modeline aşağıdaki prompt'u verdik.

```text
There is a problem on runtime.

Reasoning Model: qwen/qwen3-14b
API Log:

2026-04-17 15:38:02 [INFO]
Received request to embed multiple: [
"What is the python?"
]
2026-04-17 15:38:02 [DEBUG]
[WARNING] At least one last token in strings embedded is not SEP. 'tokenizer.ggml.add_eos_token' should be set to 'true' in the GGUF header

App terminal output: [ERROR] 'QdrantClient' object has no attribute 'search'

Fix this error?
```

Yapay zeka modelleri ile çalışırken çözülmesini istediğimiz problemlerle ilgili kaliteli detaylar vermek önemlidir. Programatik ortamlarda üretilen log mesajları, exception/error nesneleri, hatanın oluştuğu andaki değişken değerleri gibi bilgiler, modelin problemi daha iyi anlamasına ve dolayısıyla daha isabetli çözümler üretmesine yardımcı olur.

İşte örneğimizi çalışma zamanında bir görüntü.

```bash
python main.py --model "qwen/qwen3-14bs"
```

![RAG runtime](./images/day08_03.png)

## Gün 09 - MCP (Model Context Protocol) Kavramı ve MCP Server Yazılması

GELECEK BÖLÜM...

![MCP High Level Diagram](./images/day09_00.png)

## Gün 10 - MCP Server'lar ile Çalışmak

## Gün 11 - Spec Driven Development (SDD) Yaklaşımı ile Geliştirme Yapmak

## Gün 12 - Proje Sunumları

## Aman Dikkat

- Yapay zeka botları ile çalışırken şifre, gizli anahtar, kişisel veri gibi hassas bilgileri prompt'lara dahil etmekten kaçınmalısınız. Bu tür bilgilerin istemeden de olsa loglanması veya üçüncü taraflarla paylaşılması ciddi güvenlik risklerine yol açabilir. Lisanslı modeller kullanırken de mutlaka sözleşme şartlarını dikkatlice inceleyin ve gizlilik politikalarını anlayın.
- Yapay zeka araçlarının ürettiği kodların güvenlik açıkları içermediğinden emin olmak için kodu dikkatlice inceleyin ve gerekirse güvenlik tarama araçları kullanarak analiz edin. Özellikle web uygulamaları geliştirirken SQL injection, XSS gibi yaygın güvenlik açıklarına karşı dikkatli olmak gerekiyor.
- Kodun yüksek kalitede olduğunu garanti etmek için statik kod tarama araçlarından yararlanın. Örneğin, .NET projeleri için **SonarQube**, **JavaScript** projeleri için **ESLint** gibi araçlar ile kod kalitesini sıklıkla ölçün. Code Review ve Pull Request gibi süreçleri atlamayın, insan denetimi her zaman önemlidir.
- Kendi yapabileceğimiz çok basit bir kod parçasını yapay zeka aracına yazdırmak yerine, yapay zeka araçlarını daha karmaşık, zaman alan, aynı görevin sürekli tekrar ettiği işler için kullanmak daha verimli olabilir. Örneğin bir **MongoDB Docker** imaj tanımını resmî sitesinden alıp projeye uygulamayı yapay zeka aracına yazdırmak yerine, ayağa kaldırdığımız bir imajın çalışması ile ilgili içinden çıkamadığımız bir hatayı çözmek için yapay zeka aracından yardım almak daha verimli olabilir.
- İyi **prompt**'lar vermek, yapay zeka araçlarından kaliteli çıktılar almak için kritik öneme sahiptir. Prompt'larınızda açık ve net olun, gerekli detayları sağlayın ve mümkünse örnekler verin. Çıktıları mutlaka dikkatlice inceleyin, ispat arayın, doğruluğundan emin olun. Yapay zeka araçlarının ürettiği çıktıları denetleyen kodlar da geliştirebilirsiniz ;-)
- Yapay zeka araçlarının ürettiği konfigürasyon içeriklerinde şifre, gizli anahtar gibi bilgiler varsa klasik metodolojide olduğu gibi bunları daha güvenli ortamlarda *(Vault, Azure Key Vault, AWS Secrets Manager gibi)* saklamayı tercih edin. Yapay zeka araçlarının ürettiği kodlarda bu tür bilgilerin hardcoded olarak yer almamasına da ayrıca dikkat edin.

## Ders Geçme Prosedürü

Bu dönem ilk kez işlenen müfredat kapsamında ders geçme kriterleri şöyle tanımlanmıştır: %40 Proje + %60 Final.

### Proje Değerlendirmesi

Proje değerlendirmesi için aşağıdaki kriterler göz önünde bulundurulacaktır:

| Kriter | Açıklama |
| ------ | -------- |
| **Takım** | En az 1 en fazla 4 kişilik takımlar oluşturulabilir. |
| **Dil Modeli** | Projede en az bir yapay zeka dil modeli aracı kullanılmalıdır. (Claude Sonnet 4.6, Gemini 3.1, Codex 5.2 vb) |
| **Teknik Değerlendirme** | Clean Code prensiplerine uygunluk, SOLID prensiplerine uygunluk, mimari uyum, kodun okunabilirliği, test edilebilirliği gibi kriterler göz önünde bulundurulacaktır. |
| **Dokümantasyon** | Proje ile ilgili mimari tasarım, kullanılan yapay zeka araçları, karşılaşılan zorluklar ve çözümler gibi konuları içeren bir README hazırlanmalıdır. |
| **Veritabanı** | Projede en az bir veritabanı kullanılmalıdır. (SQL, NoSQL, In-Memory vb) |
| **Sunum** | Dönem boyunca proje ile ilgili en az iki sunum *(10 dakikayı geçmeyecek şekilde)* yapılmalıdır |
| **Teslim Tarihi** | Dönemin son dersi |

### Final Sınavı

Final sınavında dönem boyunca işlenen konularla ilgili sorular yer alacaktır. Örnek sorulara [buradan](./Exam00.md) bakabilirsiniz.

## Uygulama Önerileri

Bu repodaki birçok doküman veya içerik, yeni uygulamalar yazmak için bir başlangıç noktası olabilir. Bu fikirleri hâkim olduğunuz programlama dili, geliştirme platformları ve yapay zeka araçlarıyla birleştirerek kendi projelerinizi geliştirebilirsiniz. **Vibe Coding** pratiklerinden ziyade **Agentic Engineering** yaklaşımını benimseyerek hareket etmek daha doğru olur. Yani yapay zeka araçlarını birer yardımcı olarak kullanmak ve onların ürettiği çıktıları dikkatlice inceleyip gerektiğinde müdahale ederek ilerlemek daha verimli olacaktır. Bu süreçte kod güvenilirliği, teknik borç ve proje mimarisi gibi konulara dikkat etmek önemlidir.

| Proje Fikri | Açıklama |
| --- | --- |
| **Terimler Sözlüğü** | Ders müfredatında geçen teknik terimlerin tanımlarını ve açıklamalarını içeren bir sözlük uygulaması. Kullanıcı terim arayabilir, yeni terimler ekleyebilir. Terimler merkezi bir veri sisteminde servis tabanlı çekilir. Düzenleme ve ekleme fonksiyonellikleri yetkiye *(Authorization)* bağlıdır. |
| **Gamepedia** | Online popüler oyunlar ansiklopedisi. Bilinen efsane oyunlarla ilgili detaylı bilgilerin yer aldığı bir web uygulamasıdır. Oyunlara ait örnek ekran görüntüleri, geliştiricileri, stüdyo bilgileri, kullanıcı puanları, aldığı ödüller vs. Ayrıca içinde bilgi yarışması da barındırır. Referans olarak **Steam** oyun platformunun web uygulaması baz alınabilir. |
| **CV Bank** | CV'lerin saklandığı, yönetildiği, analiz edildiği bir uygulama. CV'ler JSON formatında saklanır. Kullanıcılar CV'lerini yükleyebilir, düzenleyebilir, silebilir. Yüklenen CV'ler yapay zeka araçları tarafından analiz edilerek özetlenebilir, kategorize edilebilir. |
| **GeoQuiz** | Coğrafya temalı bir bilgi yarışması uygulaması. Kullanıcılar farklı zorluk seviyelerinde coğrafya sorularını cevaplayarak puan kazanır. Sorular yapay zeka araçları tarafından oluşturulabilir veya mevcut bir veri seti kullanılabilir |
