# Sözlük

Burası ders müfredatında incelenen konulara ait teknik terimlerin özetlendiği bir sözlüktür.

## İçindekiler

| # | Terim |
| --- | ------- |
| 1 | [.gitignore](#gitignore) |
| 2 | [ADR](#adr) |
| 3 | [Aggregate](#aggregate) |
| 4 | [Assert](#assert) |
| 5 | [BDD](#bdd) |
| 6 | [Bootstrap](#bootstrap) |
| 7 | [CAG *(Context Augmented Generation)*](#cag-context-augmented-generation) |
| 8 | [CI/CD](#cicd) |
| 9 | [Clean Architecture](#clean-architecture) |
| 10 | [Code Review](#code-review) |
| 11 | [Context](#context) |
| 12 | [Cross-Origin Request Blocked](#cross-origin-request-blocked) |
| 13 | [CRUD](#crud) |
| 14 | [Custom Agent](#custom-agent) |
| 15 | [Dağıtık Sistemler *(Distributed Systems)*](#dağıtık-sistemler-distributed-systems) |
| 16 | [Dependency Injection](#dependency-injection) |
| 17 | [Docker](#docker) |
| 18 | [Embedding](#embedding) |
| 19 | [Enum](#enum) |
| 20 | [Entegrasyon Testi *(Integration Testing)*](#entegrasyon-testi-integration-testing) |
| 21 | [Fine-Tuning](#fine-tuning) |
| 22 | [GitHub Copilot](#github-copilot) |
| 23 | [GitHub Repo](#github-repo) |
| 24 | [Graph RAG](#graph-rag) |
| 25 | [Guardrails](#guardrails) |
| 26 | [gRPC *(Google Remote Procedure Call)*](#grpc-google-remote-procedure-call) |
| 27 | [Hallucination](#hallucination) |
| 28 | [HTML *(Hypertext Markup Language)*](#html-hypertext-markup-language) |
| 29 | [JavaScript *(JS)*](#javascript-js) |
| 30 | [JSON *(JavaScript Object Notation)*](#json-javascript-object-notation) |
| 31 | [Knowledge Graph](#knowledge-graph) |
| 32 | [Legacy System](#legacy-system) |
| 33 | [Markdown Format](#markdown-format) |
| 34 | [MCP *(Model Context Protocol)*](#mcp-model-context-protocol) |
| 35 | [Microservices](#microservices) |
| 36 | [Playwright](#playwright) |
| 37 | [Prompt](#prompt) |
| 38 | [Prompt Injection](#prompt-injection) |
| 39 | [Pull Request](#pull-request) |
| 40 | [RAG *(Retrieval Augmented Generation)*](#rag-retrieval-augmented-generation) |
| 41 | [RDBMS *(Relational Database Management System)*](#rdbms-relational-database-management-system) |
| 42 | [Regex *(Regular Expressions)*](#regex-regular-expressions) |
| 43 | [REST *(Representational State Transfer)*](#rest-representational-state-transfer) |
| 44 | [Rich Entity](#rich-entity) |
| 45 | [Sandbox](#sandbox) |
| 46 | [Skill](#skill) |
| 47 | [SOAP *(Simple Object Access Protocol)*](#soap-simple-object-access-protocol) |
| 48 | [Spec-Oriented Programming](#spec-oriented-programming) |
| 49 | [SQL Injection](#sql-injection) |
| 50 | [Swagger](#swagger) |
| 51 | [Test Containers](#test-containers) |
| 52 | [Test Driven Development *(TDD)*](#test-driven-development-tdd) |
| 53 | [Teknik Borç *(Technical Debt)*](#teknik-borç-technical-debt) |
| 54 | [Tightly Coupled vs Loosely Coupled](#tightly-coupled-vs-loosely-coupled) |
| 55 | [UAT](#uat) |
| 56 | [Unit Test *(Birim Test)*](#unit-test-birim-test) |
| 57 | [UX *(User Experience)*](#ux-user-experience) |
| 58 | [Vektör Veritabanı](#vektör-veritabanı) |
| 59 | [XSS *(Cross-Site Scripting)*](#xss-cross-site-scripting) |
| 60 | [XML *(eXtensible Markup Language)*](#xml-extensible-markup-language) |
| 61 | [YAML *(YAML Ain't Markup Language)*](#yaml-yaml-aint-markup-language) |

## Terimler

### .gitignore

Git versiyon kontrol sisteminde hangi dosya ve klasörlerin takip edilmeyeceğini belirten yapılandırma dosyasıdır. Genellikle geçici dosyalar *(Temp files)*, bağımlılıklar *(node_modules)*, derleme çıktıları ve hassas bilgiler içeren dosyalar bu dosyaya eklenir. Her satıra bir kural yazılır ve wildcardlar (*) kullanılabilir. Genellikle proje başlangıcında oluşturulması önerilir.

### ADR

**Architecture Decision Record** ifadesinin kısaltmasıdır. Yazılım projelerinde tüm sistemi etkileyen önemli mimari kararların neden alındığını, hangi alternatiflerin değerlendirildiğini ve bu kararların olası sonuçlarını kısa ama kalıcı bir kayıt olarak belgelemek için kullanılır.

### Aggregate

**Domain-Driven Design *(DDD)*** yaklaşımında tutarlılık sınırını temsil eden yapıdır. Bir aggregate, kendi içinde ilişkili varlıkları *(entity)* ve değer türlerini *(value object)* barındırır. Dış dünya bileşenleri ile etkileşim genellikle aggregate root üzerinden yapılır.

### Assert

Testlerde, beklenen sonuç ile gerçek sonucu karşılaştırmak için kullanılan bir ifadedir. Assert, testin başarılı olup olmadığını belirler. Eğer assert başarısız olursa, test başarısız olarak kabul edilir ve genellikle bir hata mesajı üretilir. Birçok test framework'ünde testleri kolaylaştıran yardımcı metotlar bulunur, örneğin **assertEqual**, **assertTrue**, **assertFalse** vb. Bunlar framework'e göre farklılık gösterebilir ancak temel amaçları beklenen ve gerçek sonuçları karşılaştırmaktır.

### BDD

**Behavior-Driven Development *(BDD)*** ifadesinin kısaltmasıdır. Yazılımın davranışını kullanıcı senaryoları ve örnekler üzerinden tanımlamayı amaçlayan bir geliştirme yaklaşımıdır. Genellikle **Given-When-Then** kalıbı ile de ifade edilir.

### Bootstrap

Web geliştirme için kullanılan açık kaynaklı, ücretsiz bir **CSS framework**'dür. **Responsive** ve **mobil öncelikli** yaklaşımda tasarlanmış hazır bileşenler ve layout sistemleri içerir. Geliştiricilerin hızlı ve tutarlı kullanıcı arayüzleri oluşturmasını sağlar. **HTML**, **CSS** ve **JavaScript** ile çalışır.

### CAG *(Context Augmented Generation)*

Yapay zeka modellerinin, belirli bir bağlamı *(context)* kullanarak daha doğru ve ilgili yanıtlar üretmesini sağlayan bir tekniktir. Model, verilen bir sorguya yanıt üretmeden önce ilgili bağlam bilgilerini alır ve bu bilgileri kullanarak yanıtını oluşturur. Burada temel amaç kullanıcının niyetini zenginleştirerek modelin belirli bir domain çerçevesinde cevaplar üretebilmesini sağlamaktır. **RAG *(Retrieval Augmented Generation)*** ile benzer bir yaklaşım olmakla birlikte RAG, modelin harici veri kaynaklarından bilgi çekmesini sağlarken CAG daha çok mevcut bağlam bilgisini kullanarak yanıt üretmeye odaklanır. RAG ilişkili bilgileri çalışma zamanında çekmeye odaklanırken, CAG önceden yüklenmiş statik veriyi baz alarak geniş bir bağlam bilgisini belleğe alır *(long-context memory)* ve bu bağlam bilgisini kullanarak yanıt üretmeye odaklanır. *RAG* daha çok büyük ve değişen veriler için tercih edilirken, *CAG* statik ve küçük verilerle çalışmak için tercih edilir.

### CI/CD

**Continuous Integration** ve **Continuous Delivery/Deployment** kavramlarının birleşimidir. Kod değişikliklerinin otomatik olarak derlenmesi, test edilmesi ve uygun ortamlara aktarılması için kurulan geliştirme hattını ifade eder.

### Clean Architecture

Yazılım geliştirme için bir mimari desen ve prensipler setidir. Robert C. Martin tarafından tanımlanmıştır. Uygulamanın bağımsız katmanlara ayrılmasını sağlar: Domain, Application, Infrastructure ve Presentation. Bağımlılıkların içe doğru akması prensibini benimser. Test edilebilir, esnek ve sürdürülebilir kod yapıları oluşturmayı hedefler. [Burada .NET için yazılmış bir şablon da vardır](https://github.com/jasontaylordev/CleanArchitecture)

### Code Review

Yazılım geliştirme sürecinde kodun başka geliştiriciler tarafından incelendiği kalite kontrol sürecidir. Hataları erken tespit etmeyi, kod kalitesini artırma ve ekip içinde bilgi paylaşımını sağlar. Belli bir süre uygulandığında ekip üyelerinin aynı standartlarda kod yazması sağlanır. **Pull request** veya **merge request** süreçlerinde kullanılır. Kod standartlarına uygunluğu, güvenlik açıklarını ve potansiyel bug'ları yakalar.

### Context

Yapay zeka ve programlama alanında bir işlemin yürütüldüğü ortamı ve mevcut durum bilgisini ifade eder. AI modellerinde modelin anlayabileceği ve yanıt üretebileceği bilgi kümesidir. Programlamada ise bir değişkenin veya fonksiyonun erişebildiği kapsam *(scope)* anlamına gelir. Hatta **Domain-Driven Design *(DDD)*** açısından bakıldığında bir iş alanını ve o alandaki süreçleri tanımlayan bir kavramdır. Context, doğru ve etkili sonuçlar üretmek için önemlidir; zira modelin veya programın mevcut durumu ve çevresi hakkında bilgi sahibi olması gerekir.

### Cross-Origin Request Blocked

Web tarayıcılarının güvenlik politikası nedeniyle farklı bir domain'den kaynak istemeye çalışırken oluşan hata durumudur. **CORS *(Cross-Origin Resource Sharing)*** politikası tarafından engellenir. Dosya protokolü (file://) kullanılırken de ortaya çıkar. Web sunucusunda **CORS** başlıkları eklenerek veya proxy kullanılarak çözülebilir.

### CRUD

Create, Read, Update ve Delete işlemlerinin kısaltmasıdır. Bir veri kaynağı üzerinde gerçekleştirilen temel ekleme, okuma, güncelleme ve silme operasyonlarını ifade eder.

### Custom Agent

Belirli bir görev, kurum standardı veya domain bilgisi etrafında özelleştirilmiş yapay zeka ajanıdır. Genel amaçlı bir asistandan farklı olarak daha dar bir bağlamda çalışır ve çoğu zaman belirli araçlar, kurallar veya yeteneklerle *(SKILLS)* desteklenir.

### Dağıtık Sistemler *(Distributed Systems)*

Birden fazla bilgisayarın ağ üzerinden iletişim kurarak ortak bir görevi yerine getirdiği sistem mimarisidir. Yük dengeleme *(Load Balancing)*, hata toleransı *(Fault Tolerance)* ve ölçeklenebilirlik *(Scalability)* gibi çözülmesi zor problemlere ait disiplinleri gerektirir. Mikroservisler, bulut sistemleri ve blockchain gibi yapılar bu kategoriye girer. Koordinasyon ve veri tutarlılığı önemli zorlukları arasındadır. Mutlaka *CAP teoremi - Consistency, Availability, Partition Tolerance* hatırlanmalıdır.

### Dependency Injection

SOLID prensiplerinden Dependency Inversion Principle (DIP) ile yakından ilişkili olan bu kavram, bir sınıfın ihtiyaç duyduğu bağımlılıkları dışarıdan almasını sağlayan bir tasarım prensibi olarak karşımıza çıkar. Bu sayede sınıflar birbirlerine sıkı bağlı *(tight coupling)* olmaktan kurtulur ve daha esnek, test edilebilir ve bakım kolaylığı sağlayan bir yapı kurgulanır. Dependency Injection, genellikle constructor injection, setter injection veya interface injection gibi farklı yöntemlerle uygulanabilir.

### Docker

Uygulamaları ve bağımlılıklarını izole konteynerler içinde paketlemeyi ve çalıştırmayı sağlayan platformdur. Geliştirme, test ve dağıtım ortamları arasında daha tutarlı sonuçlar elde edilmesine yardımcı olur. Detaylı bilgi için [Docker'ın resmi web sitesine](https://www.docker.com/) bakabilirsiniz.

### Embedding

Metin tabanlı, görsel veya ses içerikli verilerin sayısal vektörler halinde temsil edilmesidir. Özellikle benzerlik arama, sınıflandırma ve **RAG** gibi senaryolarda anlamca yakın içeriklerin bulunmasını kolaylaştırır.

### Enum

Programlamada sınırlı ve önceden tanımlanmış sabit değer kümelerini ifade etmek için kullanılan veri tipidir. Kodun daha okunabilir ve hata yapmaya daha kapalı olmasına yardımcı olur. Farklı dillerde farklı kullanım şekilleri mevcuttur. Bazı dillerde sadece sabit değerler içerirken, bazı dillerde ek özellikler ve metodlar da içerebilir.

### Entegrasyon Testi *(Integration Testing)*

Entegrasyon testleri, birden fazla bileşenin veya modülün birlikte çalışmasını doğrulamak için yapılan testlerdir. Bu testler, bileşenlerin birbirleriyle doğru şekilde entegre olduğunu ve beklenen sonuçları ürettiğini kontrol eder. Entegrasyon testleri, birim testlerden sonra gerçekleştirilir ve sistemin genel işlevselliğini değerlendirmek için önemlidir.

### Fine-Tuning

Önceden eğitilmiş bir yapay zeka modelinin belirli bir görev veya domain için ek eğitimle özelleştirilmesi sürecidir. Bu yaklaşım modelin belli bir alandaki performansını artırabilir ancak yatırım maliyeti, veri kalitesinin korunması ve bakım yükü gibi etkileri de vardır.

### GitHub Copilot

**Microsoft** ve **OpenAI** tarafından geliştirilen, yapay zeka destekli kod tamamlama asistanıdır. Milyonlarca açık kaynak kod deposundan eğitilmiştir ve geliştiricilere gerçek zamanlı kod önerileri sunmaktadır. IDE'lere entegre olarak çalışır ve doğal dil yorumlarından kod üretebilir. Verimlilik artışı ve hızlı prototipleme sağlar.

### GitHub Repo

GitHub platformunda barındırılan git versiyon kontrol deposudur (repository). Kaynak kodları, dokümantasyonu ve proje dosyalarını saklar. İşbirlikçi geliştirme, issue takibi, pull request ve CI/CD süreçlerini destekler. Public (herkese açık) veya private (özel) olabilir.

### Graph RAG

Klasik vektör tabanlı **RAG** yaklaşımının, bilgi grafı ilişkileriyle zenginleştirilmiş türüdür. Yalnızca anlamsal yakınlığa değil, kavramlar arasındaki yapısal ilişkilere de odaklanır ve daha açıklanabilir sonuçlar üretmeyi amaçlar.

### Guardrails

Yapay zeka sistemlerinin güvenli, tutarlı ve belli politikalara uyumlu davranmasını sağlamak için kullanılan kural ve kontrol katmanlarıdır. Zararlı içerikleri süzmek, hassas verilerin sızmasını engellemek ve model çıktısını sınırlandırmak için uygulanabilir.

### gRPC *(Google Remote Procedure Call)*

Google tarafından geliştirilmiş yüksek performanslı, açık kaynaklı bir uzak prosedür çağrısı (RPC) standardıdır. Protobuf *(Protocol Buffers)* kullanarak veri serileştirmesi yapar ve HTTP/2 üzerinden iletişim kurar. Mikroservisler arasında hızlı ve verimli iletişim sağlar. REST'e göre daha düşük gecikme süresi ve daha az bant genişliği kullanır. Daha çok makineler arası iletişimde tercih edilir zira insan tarafından okunabilirliği REST'e göre daha düşüktür.

### Hallucination

Bir yapay zeka modelinin kulağa doğruymuş gibi gelen ancak gerçekte hatalı, uydurma veya doğrulanamayan bilgiler üretmesi durumudur. Özellikle bilgiye dayalı yanıtlar üreten sistemlerde dikkatle izlenmesi gereken temel risklerden biridir.

### HTML *(Hypertext Markup Language)*

Web sayfalarının yapısını oluşturmak için kullanılan işaretleme dilidir. Etiketler (tags) kullanılarak içerik organize edilir ve anlamlandırılır. Tarayıcılar HTML'i yorumlayarak görsel içeriği kullanıcıya sunar. CSS ve JavaScript ile birlikte modern web uygulamalarının temelini oluşturur.

### JavaScript *(JS)*

Web tarayıcılarında ve sunucu tarafında *(Node.js ile)* çalışabilen yüksek seviyeli, dinamik programlama dilidir. **HTML** ve **CSS** ile birlikte modern web teknolojilerinin üçüncü temel bileşenidir. Event-driven *(olay güdümlü)* ve asenkron programlamayı destekler. Çok geniş bir ekosisteme ve kütüphane desteğine sahiptir.

### JSON *(JavaScript Object Notation)*

Veri alışverişi için kullanılan hafif, metin tabanlı veri formatıdır. Anahtar-değer çiftleri *(key-value pairs)* ve dizi *(arrays)* gibi veri tipleri içerir. İnsan tarafından okunabilir ve makineler tarafından kolayca işlenebilir. API'ler, konfigürasyon dosyaları ve veri depolama için yaygın olarak kullanılır. Özellikle XML'e göre daha az yer kaplar ve daha hızlı işlenir. Bu nedenle SOAP yerine RESTful API'lerin tercih edilmesinde önemli bir rol oynamıştır.

### Knowledge Graph

Varlıkları *(entity)* ve bunlar arasındaki ilişkileri düğüm *(nodes)* ve kenarlar *(edges)* üzerinden temsil eden bilgi modelidir. Özellikle **Graph RAG**, arama ve anlamsal keşif senaryolarında bağlamı *(context)* daha ilişkisel biçimde ele almak için kullanılır.

### Legacy System

Eski teknolojiler veya artık desteklenmeyen ya da desteği bitecek olan sistemler üzerine kurulu, hala kullanımda olan yazılım ve donanım altyapısıdır. Değiştirmek maliyetli veya riskli olabilir ancak modern sistemlerle entegrasyonu zordur *(En sık başvurulan çözüm yollarından birisi API köprüleri veya adaptörler kullanmak, mesajlaşmayı kuyruk yapıları ile sağlamaktır)*. Güvenlik açıkları ve bakım zorlukları yaşanır. Yavaş yavaş modernize edilmesi veya yeniden yazılması gerekir.

### Markdown Format

Basit işaretleme sözdizimiyle düz metin formatında belge yazmayı sağlayan hafif işaretleme dilidir. **HTML**'e kolayca dönüştürülebilir ve okunması kolaydır. **README** dosyaları, dokümantasyonlar ve blog yazıları için yaygın olarak kullanılır. **GitHub**, **Stack Overflow** gibi platformlar tarafından desteklenir.

### MCP *(Model Context Protocol)*

**AI** modellerinin dış sistemler ve araçlarla etkileşim kurmasını sağlayan standart bir protokoldür. Modellerin dosya sistemlerine, veritabanlarına ve API'lere erişimini düzenler. Güvenli ve yapılandırılmış veri alışverişi sağlar. AI uygulamalarının gerçek dünya sistemleriyle entegrasyonunu kolaylaştırır.

### Microservices

Bir uygulamanın, belirli iş yeteneklerine odaklanan küçük ve bağımsız servisler olarak tasarlanması yaklaşımıdır. Her servis ayrı şekilde geliştirilebilir, dağıtılabilir ve ölçeklenebilir. Diğer yandan dağıtık sistem karmaşıklığını da beraberinde getirir.

### Playwright

Daha çok web uygulamalarının test otomasyonu için kullanılan bir araçtır. Modern web tarayıcılarını destekler. [Playwright](https://playwright.dev/) kullanıcı etkileşimlerini simüle ederek web uygulamalarının beklendiği şekilde çalışıp çalışmadığını test etmek için kullanılır. Özellikle end-to-end testlerde tercih edilir. Test runner, assertions, isolation ve paralel test çalıştırma gibi birçok özelliği destekler.

### Prompt

Yapay zeka modellerine verilen giriş metni veya talimatlardır. Modelin nasıl yanıt vereceğini ve hangi görevleri yapacağını belirler. İyi yazılmış promptlar daha kaliteli ve doğru sonuçlar üretir. Bu sebepten **Prompt Engineering** (Prompt Mühendisliği) önemli bir beceri haline gelmiştir. Promptlar, doğal dil açıklamaları, örnekler veya belirli formatlarda olabilir.

### Prompt Injection

Yapay zeka modellerine verilen promptların kötü niyetli kullanıcılar tarafından manipüle edilmesiyle ortaya çıkan bir güvenlik açığıdır. Kötü niyetli promptlar, modelin beklenmedik veya zararlı yanıtlar üretmesine neden olabilir. Bu tür saldırılar, modelin güvenliğini tehlikeye atabilir ve istenmeyen sonuçlara yol açabilir. Prompt Injection saldırılarına karşı, kullanıcı girdilerinin doğrulanması ve temizlenmesi gibi önlemler alınmalıdır.

### Pull Request

Bir sürüm kontrol sisteminde yapılan değişikliklerin ana dala *(main branch)* birleştirilmeden önce incelenmesini sağlayan iş akışıdır. Kod incelemesi, otomatik testler ve ekip içi değerlendirme süreçleri çoğunlukla **pull request** üzerinden yürütülür. Amaç, kod kalitesini artırmak, hataları erken tespit etmek ve ekip içinde bilgi paylaşımını sağlamaktır. **GitHub**, **GitLab** ve **Bitbucket** gibi platformlarda yaygın olarak kullanılır.

### RAG *(Retrieval Augmented Generation)*

Yapay zeka modellerinin bilgiye dayalı yanıtlar üretmek için harici veri kaynaklarından bilgi çekmesini sağlayan bir tekniktir. Model, verilen bir sorguya yanıt üretmeden önce ilgili bilgiyi veri tabanları, belgeler veya API'ler gibi kaynaklardan alır. Bu sayede daha doğru ve güncel yanıtlar üretebilir.

### RDBMS *(Relational Database Management System)*

Verileri tablolar halinde organize eden ve bilinen haliyle SQL *(Structured Query Language)* diliyle veri yönetimi sağlayan bir veritabanı yönetim sistemidir. Veriler arasındaki ilişkileri tanımlamak ve sorgulamak için güçlü araçlar sunar. MySQL, PostgreSQL, Oracle ve SQL Server gibi popüler RDBMS örnekleri vardır. Verilerin tutarlılığını sağlamak için ACID *(Atomicity, Consistency, Isolation, Durability)* özelliklerini destekler.

### Regex *(Regular Expressions)*

Regular Expressions (Regex), metin içinde belirli desenleri tanımlamak ve eşleştirmek için kullanılan güçlü bir araçtır. Metin arama, doğrulama ve değiştirme işlemlerinde yaygın olarak kullanılır. Örneğin, bir e-posta adresini doğrulamak veya belirli bir kelimeyi metin içinde bulmak için kullanılabilir. Farklı programlama dilleri ve araçlarda desteklenir.

### REST *(Representational State Transfer)*

Web servisleri için kullanılan bir mimari tarzdır. HTTP protokolünü kullanarak kaynaklara erişim sağlar. Kaynaklar URI'ler ile tanımlanır ve HTTP metodları (GET, POST, PUT, DELETE) ile işlemler gerçekleştirilir. JSON veya XML gibi formatlarda veri alışverişi yapılır. RESTful API'ler, basitlik, ölçeklenebilirlik ve esneklik sunar.

### Rich Entity

**Domain Driven Design (DDD)** yaklaşımında, sadece veri tutan değil aynı zamanda iş mantığını da içeren entity'lerdir. Rich Entity'ler, kendi davranışlarını ve kurallarını içerir, bu da kodun daha modüler, okunabilir ve bakımının kolay olmasını sağlar. Anemic Domain Model'in (sadece veri tutan entity'ler) aksine, Rich Entity'ler domain mantığını kapsülleyerek daha güçlü bir model oluşturur.

### Sandbox

Kodun veya komutların ana sisteme zarar vermeden denenebildiği izole çalışma ortamıdır. Güvenlik açısından riskli işlemleri sınırlı yetkilerle ve kontrollü kaynak erişimiyle çalıştırmak için tercih edilir. Özellikle yapay zeka ajanlarının dış sistemlerle etkileşim kurarken güvenliğini sağlamak için kullanılır. Örneğin, bir yapay zeka modelinin dosya sistemine erişmesi gerekiyorsa, bu erişim bir sandbox içinde sınırlandırılabilir.

### Skill

Yapay zeka ajanlarına belirli bir konuda uzmanlık ve tekrar kullanılabilir çalışma talimatları kazandıran modüler yapıdır. Genellikle açıklayıcı bir **Markdown** dosyası ve gerektiğinde örnekler, şablonlar veya yardımcı kaynaklarla birlikte kullanılır.

### SOAP *(Simple Object Access Protocol)*

**XML** tabanlı, ağ üzerinden mesaj alışverişi için kullanılan protokoldür. Kurumsal uygulamalarda web servisleri için bir standart haline gelmiştir. Güçlü tip kontrolü ve güvenlik özellikleri sunar. **REST**'e göre daha ağır ve karmaşıktır ancak daha fazla standart ve özellik içerir.

### Spec-Oriented Programming

Yazılım geliştirme sürecinde, uygulamanın mimari tasarımını ve domain modelini detaylı bir şekilde tanımlayan spesifikasyon dokümanlarına dayalı olarak geliştirme yapma yaklaşımıdır. Bu yaklaşımda, geliştiriciler öncelikle yüksek seviyede mimari ve domain tasarımını içeren dokümanlar hazırlar ve ardından bu dokümanlara göre kod üretirler. Spec-Oriented Programming, özellikle yapay zeka destekli geliştirme süreçlerinde, AI modellerinin daha iyi sonuçlar üretmesi için net ve detaylı spesifikasyonların önemini vurgular.

### SQL Injection

Kötü niyetli kullanıcıların SQL sorgularını manipüle ederek veri tabanına yetkisiz erişim sağlamasına izin veren bir güvenlik açığı türüdür. Genellikle kullanıcı tarafından sağlanan verilerin uygun şekilde doğrulanmaması veya temizlenmemesi nedeniyle ortaya çıkar. SQL Injection saldırıları, veri tabanındaki hassas bilgilerin çalınmasına, değiştirilmesine veya silinmesine yol açabilir.

### Swagger

**REST API**'lerin dokümantasyonunu üretmek, endpoint'leri keşfetmek ve test etmek için yaygın olarak kullanılan araç ve arayüz ailesidir. Günümüzde çoğunlukla **OpenAPI** tanımlarını görselleştiren kullanıcı arayüzü ile birlikte anılır. Sadece geliştirme aşamasında kullanılması önerilir, üretim ortamlarında söz konusu servislerin açık bir şekilde keşfedilmemesi gereken durumlarda dikkatli olunmalıdır, güvenlik riskleri oluşturabilir.

### Teknik Borç *(Technical Debt)*

Hızlı geliştirme için kısa vadeli çözümler tercih edildiğinde oluşan, gelecekte daha fazla efor gerektirecek kod kalitesi eksikliğidir. Zaman içinde biriken teknik borç, bakım maliyetlerini artırır ve yeni özellik eklemeyi zorlaştırır. **Refactoring** ve kod iyileştirmeleriyle azaltılır. Bilinçli bir şekilde teknik borç alınabilir ancak uzun vadede yönetilmesi gerekir.

### Test Containers

Test konteynerleri, test ortamlarını izole etmek ve yönetmek için kullanılan bir yaklaşımdır. Genellikle Docker gibi konteyner teknolojileri kullanılarak oluşturulan bu ortamlar, testlerin gerçek dünya koşullarına daha yakın bir şekilde çalışmasını sağlar. Test konteynerleri, veritabanları, mesajlaşma sistemleri veya diğer bağımlılıkları içerebilir ve testler tamamlandıktan sonra kolayca temizlenebilir. Örneğin, bir uygulamanın veritabanı entegrasyonunu test etmek için bir test konteyneri içinde geçici bir veritabanı oluşturulabilir ve böylece gerçek veritabanına zarar verme riski de olmadan ortama en yakın koşullarda testler icra edilir.

### Test Driven Development *(TDD)*

Yazılım geliştirme sürecinde testlerin önce yazıldığı, ardından kodun bu testleri geçecek şekilde geliştirildiği bir metodolojidir. TDD, kodun doğruluğunu artırır ve tasarımın daha modüler ve test edilebilir olmasını sağlar. Genellikle üç temel renkle ifade edilen bir döngü izlenir: **Red (Test Fail)** - Testler başarısız olur, **Green (Test Pass)** - Testleri geçecek kadar kod yazılır, **Blue (Refactor)** - Kod temizlenir ve optimize edilir. İlk geliştirme süresini artırabilir ancak uzun vadede bakım maliyetlerini düşürür ve kod kalitesini artırır.

### Tightly Coupled vs Loosely Coupled

Dependency Injection konusunun önemli bir parçası olan bu kavramlar, yazılım bileşenlerinin birbirlerine olan bağımlılık derecesini ifade eder. Tightly Coupled *(Sıkı Bağlı)* yapılar, bileşenlerin birbirlerine sıkı ve kolayca ayrıştırılamayacak bir şekilde bağlı olduğu durumları ifade eder. Bu sıkı bağlılık değişiklik yapmayı zorlaştırır ve test edilebilirliği azaltır. Test edilebilirliğin azalması kod bakımını zorlaştırır ve güvenilirliği düşürür. Loosely Coupled *(Gevşek Bağlı)* yapılar ise bileşenlerin çeşitli soyutlamalar ile birbirlerine daha az bağımlı olacak halde kullanılabildiği durumları ifade eder. Bu tür yapılar, değişiklik yapmayı kolaylaştırır ve test edilebilirliği artırır; doğal olarak kod bakımını kolaylaştırır ve güvenilirliği artırır. Özellikle SOLID prensiplerinden olan Dependency Inversion Principle (DIP) bu konuda önemli bir rol oynar.

### UAT

**User Acceptance Testing** ifadesinin kısaltmasıdır. Yazılımın son kullanıcı veya iş birimi tarafından kabul kriterlerine göre değerlendirildiği test aşamasını ifade eder.

### Unit Test *(Birim Test)*

Bir yazılım bileşeninin en küçük birimlerini izole ederek test etme sürecidir. Genellikle fonksiyonlar veya metodlar gibi bağımsız birimler üzerinde gerçekleştirilir. Unit testler, kodun doğru çalıştığını doğrulamak ve gelecekteki değişikliklerin mevcut işlevselliği bozmadığını garanti etmek için kullanılır.

### UX *(User Experience)*

Bir kullanıcının bir ürün veya hizmetle etkileşiminden elde ettiği genel deneyimi ifade eder. Kullanılabilirlik, erişilebilirlik, performans ve estetik gibi faktörleri içerir. İyi bir UX, kullanıcı memnuniyetini artırır ve ürünün başarısına katkıda bulunur.

### Vektör Veritabanı

**Embedding** gibi yüksek boyutlu vektör verilerini saklamak ve benzerlik araması yapmak için tasarlanmış veritabanı türüdür. Özellikle **RAG** uygulamalarında sorguya anlamsal olarak en yakın parçaları hızlıca bulmak için kullanılır.

### XSS *(Cross-Site Scripting)*

Web uygulamalarında, kötü niyetli kullanıcıların diğer kullanıcıların tarayıcılarında zararlı kod çalıştırmasına izin veren bir güvenlik açığı türüdür. Genellikle kullanıcı tarafından sağlanan verilerin uygun şekilde doğrulanmaması veya temizlenmemesi nedeniyle ortaya çıkar. XSS saldırıları, kullanıcıların oturum bilgilerini çalmak, sahte içerik göstermek veya kötü amaçlı yazılım dağıtmak gibi zararlı eylemler gerçekleştirebilir.

### XML *(eXtensible Markup Language)*

Veri depolama ve taşıma için tasarlanmış, etiket *(markup)* tabanlı işaretleme dilidir. İnsan ve makine tarafından okunabilir yapılandırılmış veriler oluşturur. Kendini tanımlayan etiketler kullanır ve hiyerarşik bir yapıya sahiptir. Web servisleri, konfigürasyon dosyaları ve veri alışverişinde kullanılır.

### YAML *(YAML Ain't Markup Language)*

Veri serileştirme ve yapılandırma için kullanılan, insan tarafından okunabilir bir veri formatıdır. JSON'a benzer şekilde veri yapılarını temsil eder ancak daha esnek ve okunabilir bir sözdizimi sunar. YAML, özellikle konfigürasyon dosyalarında ve veri alışverişinde tercih edilir. Örneğin **Docker Compose** dosyaları **YAML** formatında yazılır.

---

## Yardımcı Komutlar

Müfredat boyunca kullanılan bazı yardımcı komutlar ve araçlar aşağıda listelenecektir:

### Temel Git Komutları

Müfredat boyunca kaynak kod yönetimi için **git** kullanılmaktadır. Temel git komutları ve ne işe yaradığı aşağıda listelenmiştir:

```bash
git init # Yeni bir git deposu oluşturur
git clone <repository_url> # Var olan bir git deposunu klonlar
git add <file> # Değişiklikleri Stage'e ekler
git commit -m "commit message" # Stage'e alınmış değişiklikleri kaydeder
git push origin <branch> # Değişiklikleri uzak depoya gönderir
git pull origin <branch> # Uzak depodaki değişiklikleri yerel depoya çeker
git status # Çalışma dizininin durumunu gösterir
git log # Commit geçmişini gösterir
git branch # Mevcut dalları listeler
git checkout <branch> # Belirtilen dala geçiş yapar
git merge <branch> # Belirtilen dalı mevcut dala birleştirir
```
