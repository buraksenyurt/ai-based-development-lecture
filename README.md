# Yapay Zeka Destekli Yazılım Geliştirme

Konya Gıda ve Tarım Üniversitesitesi Yazılım Müh. ve Pamukkale Üniversitesi Eletrik Elektronik ve Yönetim Bilişim Sistemleri bölümleri için açılmış derse ait doküman ve örnek uygulamaların yer aldığı repodur.

- [Yapay Zeka Destekli Yazılım Geliştirme](#yapay-zeka-destekli-yazılım-geliştirme)
  - [Önsöz](#önsöz)
  - [Gün 00 - Tanışma ve `Hello World` Uygulamasının Geliştirilmesi](#gün-00---tanışma-ve-hello-world-uygulamasının-geliştirilmesi)
    - [Bu çalışmadan çıkarılması gereken dersler](#bu-çalışmadan-çıkarılması-gereken-dersler)
  - [Gün 01 - CV Bank Projesi için Prototip Geliştirme](#gün-01---cv-bank-projesi-için-prototip-geliştirme)
  - [Gün 02 - Exception Handling, Debugging ve Docker Kullanımı](#gün-02---exception-handling-debugging-ve-docker-kullanımı)
    - [Dikkat Edilmesi Gereken Noktalar](#dikkat-edilmesi-gereken-noktalar)
  - [Aman Dikkat](#aman-dikkat)
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

Beklediğimiz gibi .net 10 tabanlı bir solution oluşturuldu. Klasör bazlı bir ayrım olmasa da projeler **Clean Architecture** yaklaşımında belirtildiği gibi **Domain**, **Application**, **Infrastructure** ve **Presentation** katmanlarına ayrıldı. Domain katmanında User, Contact gibi entity'ler ve Resume gibi aggregate'ler tanımlandı. **API** katmanında ise **REST API** standartlarına uygun ve Resume aggregate'ine yönelik **CRUD *(Create, Read, Update, Delete)*** operasyonlarını içeren bir **Controller** oluşturulduğu görüldü.

Ayrıca veri tabanı tarafı için MongoDb tercih edildiği ve bağlantı ayarlarının `appsettings.json` dosyasına eklendiği görüldü. Domain tasarımında **ContactType** adında bir enum tanımlanarak iletişim türlerinin sınırlı bir küme ile ifade edildiği gözlemlendi.

Solution ilk seferde derlenmedi zira eksik Nuget paketleri vardı. Ancak ajan sorunları kendisi düzelterek projeyi derlenebilir hale getirdi. Projeyi çalıştırdığımızda **Swagger** arayüzünde tanımlı endpoint'lerin beklendiği gibi göründüğü ve çalıştığı görüldü.

![Swagger Runtime](./images/day01_00.png)

Ancak;

- Daha zengin ve kaliteli bir mimari tasarım dokümanı hazırlamanın daha iyi sonuçlar vereceği anlaşılıyor. Örneğin **API** standartlarının detaylı bir şekilde tanımlanması, API katmanında daha eksiksiz ve standartlara uygun bir **Controller** oluşturulmasını sağlayabilir. Listeleme endpoint'lerinin sayfalama desteği içermesi, veri oluşturma/güncelleme/silme endpoint'lerinin **HTTP** metodlarına uygun şekilde tasarlanması gibi detaylar mimari dokümanında ne kadar iyi tanımlanırsa, üretilen kodun kalitesi ve mimari uyumu o kadar artabilir.
- **Domain** tasarımının detaylı ve iyi tanımlanmış olması, üretilen kodun kalitesini ve mimari uyumunu artırabilir. Bu da ilgili domain hakkında yetkin bilgiye sahip olmayı ve **DDD *(Domain Driven Design)*** prensiplerini iyi bilmeyi gerektirmektedir. Domain tasarımında entity'lerin, aggregate'lerin ve value object'lerin doğru şekilde tanımlanması, kodun okunabilirliğini, bakımını ve genişletilebilirliğini artırır.

## Gün 02 - Exception Handling, Debugging ve Docker Kullanımı

Bu derste **Swagger** üzerinden yaptığımız API testleri sırasında aldığımız çalışma zamanı hatalarına istinaden **.Net** gibi yönetimli ortamlarda *(Managed Environment)* istisna/hata yönetiminin nasıl ele alındığına değindik. Özellikle **Exception** mesajlarındaki **Call Stack loglarının** nasıl okunması gerektiğine baktık ki gözlerimizi acıtan **Call Stack** içeriği de aşağıdaki gibiydi ancak satır satır yorumladık.

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

> Exception yönetimi her ne kadar işi kolaylaştıran bir yol olsa da runtime maliyetlerini de düşünüerek **try...catch...finally** bloğuna ihtiyaç duymadan da bazı hatalarının yönetilebileceğini bilmemiz gerekir. Örneğin bir dosya üzerinde işlem yapan bir metot yazdığımızı düşünelim. Dosya üzerinde işlem yaparken dosyanın var olup olmadığını kontrol edebiliriz. Eğer dosya yoksa bu durumu bir istisna fırlatmak yerine, metot içerisinde yönetebiliriz. Bu sayede gereksiz yere **try...catch** bloğu kullanmayız, kodun okunurluğunu artırır, çalışma zamanını optimize ederiz. Ancak bazı hallerde istisna yönetimi kaçınılmaz olabilir. Örneğin, bir veritabanı bağlantısı kurarken, bağlantının başarısız olması gibi durumlarda istisna yönetimi kullanmak gerekebilir. Bu tür durumlarda, istisna yönetimi kullanarak hataları daha etkili bir şekilde ele alabilir ve uygulamanın çökmesini önleyebiliriz.

Ayrıca bir .net uygulamasında nasıl debug yapılır, tarayıcılarda **Developer Tools** kullanılarak ağ trafiği, request ve response bilgileri nasıl izlenir konularına değindik. Bunun yanında [HTTP statü kodlarının](https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status) ne anlama geldiğine baktık.

Projede veritabanı olarak **MongoDb** kullanmaya karar vermiştik. Sisteme mongodb ortamını kurmak yerine local ortamda bir **[docker](https://github.com/docker)** image kullandık. Daha sonradan çalıştığımız çözüme yeni servisler ekleyebileceğimiz için mongodb servisini bir [docker-compose](./docker-compose.yml) dosyası içerisine aldık. Docker compose dosyasında tanımlı olan servisleri ayağa kaldırmak oldukça basit. Bunun için terminalden aşağıdaki komutu çalıştırmak yeterli.

```bash
docker-compose up -d
```

**Windows** ve **MacOS** gibi ortamlarda **[Docker Desktop](https://docs.docker.com/get-started/get-docker/)** uygulaması ile docker imajları ve container'lar görsel bir arayüz üzerinden de yönetilebilir. **Linux** platformunda ise daha çok terminal üzerinden yönetim yapılır. Bazı temel ve ihtiyaç duyabileceğimiz komutların kullanımına ait basit örnekleri aşağıda bulabilirsiniz. Diğer yandan [Docker resmi sitesinde](https://docs.docker.com/get-started/docker_cheatsheet.pdf) faydalı bir **CLI Cheat Sheet** bulunmaktadır.

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

MongoDb erişimi sırasında aldığımız Authentication hatası nedeniyle şu prompt'u kullandık:

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

## Aman Dikkat

- Yapay zeka botları ile çalışırken şifre, gizli anahtar, kişisel veri gibi hassas bilgileri prompt'lara dahil etmekten kaçınmalısınız. Bu tür bilgilerin istemeden de olsa loglanması veya üçüncü taraflarla paylaşılması ciddi güvenlik risklerine yol açabilir. Lisanslı modeller kullanırken de mutlaka sözleşme şartlarını dikkatlice inceleyin ve gizlilik politikalarını anlayın.
- Yapay zeka araçlarının ürettiği kodların güvenlik açıkları içermediğinden emin olmak için kodu dikkatlice inceleyin ve gerekirse güvenlik tarama araçları kullanarak analiz edin. Özellikle web uygulamaları geliştirirken SQL injection, XSS gibi yaygın güvenlik açıklarına karşı dikkatli olmak gerekiyor.
- Kodun yüksek kalitede olduğunu garanti etmek için statik kod tarama araçlarından yararlanın. Örneğin, .NET projeleri için **SonarQube**, JavaScript projeleri için **ESLint** gibi araçlar ile kod kalitesini sıklıkla ölçün. Code Review ve Pull Request gibi süreçleri atlamayın, insan denetimi her zaman önemlidir.
- Kendi yapabileceğimiz çok basit bir kod parçasını yapay zeka aracına yazdırmak yerine, yapay zeka araçlarını daha karmaşık, zaman alan, aynı taskın sürekli tekrar ettiği görevler için kullanmak daha verimli olabilir. Örneğin bir mongodb docker imaj tanımını resmi sitesinden alıp projeye uygulamayı yapay zeka aracına yazdırmak yerine, ayağa kaldırdığımız bir imajın çalışması ile ilgili içinden çıkamadığım bir hatayı çözmek için yapay zeka aracından yardım almak daha verimli olabilir.
- İyi prompt'lar vermek, yapay zeka araçlarından kaliteli çıktılar almak için kritik öneme sahiptir. Prompt'larınızda açık ve net olun, gerekli detayları sağlayın ve mümkünse örnekler verin. Çıktıları mutlaka dikkatlice inceleyin, ispat arayın, doğruluğundan emin olun. Yapay zeka araçlarının ürettiği çıktıları denetleyen kodlar da geliştirebilirsiniz ;-)
- Yapay zeka araçlarının ürettiği konfigurasyon içeriklerinde şifre, gizli anahtar gibi bilgiler varsa klasik metodolojide olduğu gibi bunları daha güvenli ortamlarda *(Vault, Azure Key Vault, AWS Secrets Manager gibi)* saklamayı tercih edin. Yapay zeka araçlarının ürettiği kodlarda bu tür bilgilerin hardcoded olarak yer almamasına da ayrıca dikkat edin.

## Uygulama Önerileri

Bu repodaki birçok doküman veya içerik yeni uygulamalar yazmak için bir başlangıç noktası olabilir. Bu fikirleri hakim olduğunuz programlama dili ve geliştirme platformları ve yapay zeka araçlarıyla birleştirerek kendi projelerinizi geliştirebilirsiniz. **Vibe Coding** pratiklerinden ziyade **Agentic Engineering** yaklaşımını benimseyerek hareket etmek daha doğru olur. Yani yapay zeka araçlarını birer yardımcı olarak kullanmak ve onların ürettiği çıktıları dikkatlice inceleyip gerektiğinde müdahale ederek ilerlemek daha verimli olacaktır. Bu süreçte kod güvenilirliği, teknik borç ve proje mimarisi gibi konulara dikkat etmek önemlidir.

| Proje Fikri | Açıklama |
| --- | --- |
| Terimler Sözlüğü | Ders müfredatında geçen teknik terimlerin tanımlarını ve açıklamalarını içeren bir sözlük uygulaması. Kullanıcı terim arayabilir, yeni terimler ekleyebilir. Terimler merkezi bir veri sisteminde servis tabanlı çekilir. Düzenleme ve ekleme fonksiyonellikleri yetkiye *(Authorization)* bağlıdır. |
| | |
