# Sözlük

Burası ders müfredatında incelenen konulara ait teknik terimlerin özetlendiği bir sözlüktür.

## İçindekiler

| # | Terim |
|---|-------|
| 1 | [.gitignore](#gitignore) |
| 2 | [Bootstrap](#bootstrap) |
| 3 | [Clean Architecture](#clean-architecture) |
| 4 | [Code Review](#code-review) |
| 5 | [Context](#context) |
| 6 | [Cross-Origin Request Blocked](#cross-origin-request-blocked) |
| 7 | [Dağıtık Sistemler](#dağıtık-sistemler-distributed-systems) |
| 8 | [GitHub Copilot](#github-copilot) |
| 9 | [GitHub Repo](#github-repo) |
| 10 | [gRPC](#grpc-google-remote-procedure-call) |
| 11 | [HTML](#html-hypertext-markup-language) |
| 12 | [Javascript](#javascript-js) |
| 13 | [JSON](#json-javascript-object-notation) |
| 14 | [Legacy System](#legacy-system) |
| 15 | [Markdown Format](#markdown-format) |
| 16 | [MCP](#mcp-model-context-protocol) |
| 17 | [Prompt](#prompt) |
| 18 | [RAG](#rag-retrieval-augmented-generation) |
| 19 | [RDBMS](#rdbms-relational-database-management-system) |
| 20 | [Regex](#regex-regular-expressions) |
| 21 | [REST](#rest-representational-state-transfer) |
| 22 | [Rich Entity](#rich-entity) |
| 23 | [SOAP](#soap-simple-object-access-protocol) |
| 24 | [Spec-Oriented Programming](#spec-oriented-programming) |
| 25 | [Teknik Borç](#teknik-borç-technical-debt) |
| 26 | [UX](#ux-user-experience) |
| 27 | [XML](#xml-extensible-markup-language) |

## Terimler

### .gitignore

Git versiyon kontrol sisteminde hangi dosya ve klasörlerin takip edilmeyeceğini belirten yapılandırma dosyasıdır. Genellikle geçici dosyalar *(Temp files)*, bağımlılıklar *(node_modules)*, derleme çıktıları ve hassas bilgiler içeren dosyalar bu dosyaya eklenir. Her satıra bir kural yazılır ve wildcardlar (*) kullanılabilir. Genellikle proje başlangıcında oluşturulması önerilir.

### Bootstrap

Web geliştirme için kullanılan açık kaynaklı, ücretsiz bir **CSS framework**'dür. **Responsive** ve **mobil öncelikli** yaklaşımda tasarlanmış hazır bileşenler ve layout sistemleri içerir. Geliştiricilerin hızlı ve tutarlı kullanıcı arayüzleri oluşturmasını sağlar. **HTML**, **CSS** ve **JavaScript** ile çalışır.

### Clean Architecture

Yazılım geliştirme için bir mimari desen ve prensipler setidir. Robert C. Martin tarafından tanımlanmıştır. Uygulamanın bağımsız katmanlara ayrılmasını sağlar: Domain, Application, Infrastructure ve Presentation. Bağımlılıkların içe doğru akması prensibini benimser. Test edilebilir, esnek ve sürdürülebilir kod yapıları oluşturmayı hedefler. [Burada .net için yazılmış bir şablonu da vardır](https://github.com/jasontaylordev/CleanArchitecture)

### Code Review

Yazılım geliştirme sürecinde kodun başka geliştiriciler tarafından incelendiği kalite kontrol sürecidir. Hataları erken tespit etmeyi, kod kalitesini artırma ve ekip içinde bilgi paylaşımını sağlar. Belli bir süre uygulandığında ekip üyelerinin aynı standartlarda kod yazması sağlanır. **Pull request** veya **merge request** süreçlerinde kullanılır. Kod standartlarına uygunluğu, güvenlik açıklarını ve potansiyel bug'ları yakalar.

### Context

Yapay zeka ve programlama alanında bir işlemin yürütüldüğü ortamı ve mevcut durum bilgisini ifade eder. AI modellerinde, modelin anlayabileceği ve yanıt üretebileceği bilgi kümesidir. Programlamada ise bir değişkenin veya fonksiyonun erişebildiği kapsam *(scope)* anlamına gelir. Hatta domain driven design açısısından bakıldığında bir iş alanını ve o alandaki süreçleri tanımlayan bir kavramdır. Context, doğru ve etkili sonuçlar üretmek için önemlidir zira modelin veya programın mevcut durumu ve çevresi hakkında bilgi sahibi olması gerekir.

### Cross-Origin Request Blocked

Web tarayıcılarının güvenlik politikası nedeniyle farklı bir domain'den kaynak istemeye çalışırken oluşan hata durumudur. **CORS (Cross-Origin Resource Sharing)** politikası tarafından engellenir. Dosya protokolü (file://) kullanılırken de ortaya çıkar. Web sunucusuda **CORS** başlıkları ekleyerek veya proxy kullanılarak çözülebilir.

### Dağıtık Sistemler *(Distributed Systems)*

Birden fazla bilgisayarın ağ üzerinden iletişim kurarak ortak bir görevi yerine getirdiği sistem mimarisidir. Yük dengeleme *(Load Balancing)*, hata toleransı *(Fault Tolerance)* ve ölçeklenebilirlik *(Scalability)* gibi çözülmesi zor problemlere ait disiplinleri gerektirir. Mikroservisler, bulut sistemleri ve blockchain gibi yapılar bu kategoriye girer. Koordinasyon ve veri tutarlılığı önemli zorlukları arasındadır. Mutlaka *CAP teoremi - Consistency, Availability, Partition Tolerance* hatırlanmalıdır.

### GitHub Copilot

**Microsoft** ve **OpenAI** tarafından geliştirilen, yapay zeka destekli kod tamamlama asistanıdır. Milyonlarca açık kaynak kod deposundan eğitilmiştir ve geliştiricilere gerçek zamanlı kod önerileri sunmaktadır. IDE'lere entegre olarak çalışır ve doğal dil yorumlarından kod üretebilir. Verimlilik artışı ve hızlı prototipleme sağlar.

### GitHub Repo

GitHub platformunda barındırılan git versiyon kontrol deposudur (repository). Kaynak kodları, dokümantasyonu ve proje dosyalarını saklar. İşbirlikçi geliştirme, issue takibi, pull request ve CI/CD süreçlerini destekler. Public (herkese açık) veya private (özel) olabilir.

### gRPC *(Google Remote Procedure Call)*

Google tarafından geliştirilmiş yüksek performanslı, açık kaynaklı bir uzak prosedür çağrısı (RPC) standardıdır. Protobuf *(Protocol Buffers)* kullanarak veri serileştirmesi yapar ve HTTP/2 üzerinden iletişim kurar. Mikroservisler arasında hızlı ve verimli iletişim sağlar. REST'e göre daha düşük gecikme süresi ve daha az bant genişliği kullanır. Daha çok makineler arası iletişimde tercih edilir zira insan tarafından okunabilirliği REST'e göre daha düşüktür.

### HTML *(Hypertext Markup Language)*

Web sayfalarının yapısını oluşturmak için kullanılan işaretleme dilidir. Etiketler (tags) kullanılarak içerik organize edilir ve anlamlandırılır. Tarayıcılar HTML'i yorumlayarak görsel içeriği kullanıcıya sunar. CSS ve JavaScript ile birlikte modern web uygulamalarının temelini oluşturur.

### Javascript *(JS)*

Web tarayıcılarında ve sunucu tarafında *(Node.js ile)* çalışabilen yüksek seviyeli, dinamik programlama dilidir. **HTML** ve **CSS** ile birlikte modern web teknolojilerinin üçüncü temel bileşenidir. Event-driven *(olay güdümlü)* ve asenkron programlamayı destekler. Çok geniş bir ekosisteme ve kütüphane desteğine sahiptir.

### JSON *(Javascript Object Notation)*

Veri alışverişi için kullanılan hafif, metin tabanlı veri formatıdır. Anahtar-değer çiftleri *(key-value pairs)* ve dizi *(arrays)* gibi veri tipleri içerir. İnsan tarafından okunabilir ve makineler tarafından kolayca işlenebilir. API'ler, konfigürasyon dosyaları ve veri depolama için yaygın olarak kullanılır. Özellikle XML'e göre daha az yer kaplar ve daha hızlı işlenir. Bu nedenle SOAP yerine RESTful API'lerin tercih edilmesinde önemli bir rol oynamıştır.

### Legacy System

Eski teknolojiler veya artık desteklenmeyen ya da desteği bitecek olan sistemler üzerine kurulu, hala kullanımda olan yazılım ve donanım altyapısıdır. Değiştirmek maliyetli veya riskli olabilir ancak modern sistemlerle entegrasyonu zordur *(En sık başvurulan çözüm yollarından birisi API köprüleri veya adaptörler kullanmak, mesajlaşmayı kuyruk yapıları ile sağlamaktır)*. Güvenlik açıkları ve bakım zorlukları yaşanır. Yavaş yavaş modernize edilmesi veya yeniden yazılması gerekir.

### Markdown Format

Basit işaretleme sözdizimiyle düz metin formatında belge yazmayı sağlayan hafif işaretleme dilidir. **HTML**'e kolayca dönüştürülebilir ve okunması kolaydır. **README** dosyaları, dokümantasyonlar ve blog yazıları için yaygın olarak kullanılır. **GitHub**, **Stack Overflow** gibi platformlar tarafından desteklenir.

### MCP *(Model Context Protocol)*

**AI** modellerinin dış sistemler ve araçlarla etkileşim kurmasını sağlayan standart bir protokoldür. Modellerin dosya sistemlerine, veritabanlarına ve API'lere erişimini düzenler. Güvenli ve yapılandırılmış veri alışverişi sağlar. AI uygulamalarının gerçek dünya sistemleriyle entegrasyonunu kolaylaştırır.

### Prompt

Yapay zeka modellerine verilen giriş metni veya talimatlardır. Modelin nasıl yanıt vereceğini ve hangi görevleri yapacağını belirler. İyi yazılmış promptlar daha kaliteli ve doğru sonuçlar üretir. Bu sebepten **Prompt Engineering** (Prompt Mühendisliği) önemli bir beceri haline gelmiştir. Promptlar, doğal dil açıklamaları, örnekler veya belirli formatlarda olabilir.

### RAG *(Retrieval Augmented Generation)*

Yapay zeka modellerinin bilgiye dayalı yanıtlar üretmek için harici veri kaynaklarından bilgi çekmesini sağlayan bir tekniktir. Model, verilen bir sorguya yanıt üretmeden önce ilgili bilgiyi veri tabanları, belgeler veya API'ler gibi kaynaklardan alır. Bu sayede daha doğru ve güncel yanıtlar üretebilir.

### RDBMS *(Relational Database Management System)*

Verileri tablolar halinde organize eden ve bilinen haliyle SQL *(Structured Query Language)* diliyle veri yönetimi sağlayan bir veritabanı yönetim sistemidir. Veriler arasındaki ilişkileri tanımlamak ve sorgulamak için güçlü araçlar sunar. MySQL, PostgreSQL, Oracle ve SQL Server gibi popüler RDBMS örnekleri vardır. Verilerin tutarlılığını sağlamak için ACID *(Atomicity, Consistency, Isolation, Durability)* özelliklerini destekler.

### Regex *(Regular Expressions)*

Regular Expressions (Regex), metin içinde belirli desenleri tanımlamak ve eşleştirmek için kullanılan güçlü bir araçtır. Metin arama, doğrulama ve değiştirme işlemlerinde yaygın olarak kullanılır. Örneğin, bir e-posta adresini doğrulamak veya belirli bir kelimeyi metin içinde bulmak için kullanılabilir. Farklı programlama dilleri ve araçlarda desteklenir.

### REST *(Representational State Transfer)*

Web servisleri için kullanılan bir mimari tarzdır. HTTP protokolünü kullanarak kaynaklara erişim sağlar. Kaynaklar URI'ler ile tanımlanır ve HTTP metodları (GET, POST, PUT, DELETE) ile işlemler gerçekleştirilir. JSON veya XML gibi formatlarda veri alışverişi yapılır. RESTful API'ler, basitlik, ölçeklenebilirlik ve esneklik sunar.

### Rich Entity

Domain Driven Design (DDD) yaklaşımında, sadece veri tutan değil aynı zamanda iş mantığını da içeren entity'lerdir. Rich Entity'ler, kendi davranışlarını ve kurallarını içerir, bu da kodun daha modüler, okunabilir ve bakımının kolay olmasını sağlar. Anemic Domain Model'in (sadece veri tutan entity'ler) aksine, Rich Entity'ler domain mantığını kapsülleyerek daha güçlü bir model oluşturur.

### SOAP *(Simple Object Access Protocol)*

**XML** tabanlı, ağ üzerinden mesaj alışverişi için kullanılan protokoldür. Kurumsal uygulamalarda web servisleri için bir standart haline gelmiştir. Güçlü tip kontrolü ve güvenlik özellikleri sunar. **REST**'e göre daha ağır ve karmaşıktır ancak daha fazla standart ve özellik içerir.

### Spec-Oriented Programming

Yazılım geliştirme sürecinde, uygulamanın mimari tasarımını ve domain modelini detaylı bir şekilde tanımlayan spesifikasyon dokümanlarına dayalı olarak geliştirme yapma yaklaşımıdır. Bu yaklaşımda, geliştiriciler öncelikle yüksek seviyede mimari ve domain tasarımını içeren dokümanlar hazırlar ve ardından bu dokümanlara göre kod üretirler. Spec-Oriented Programming, özellikle yapay zeka destekli geliştirme süreçlerinde, AI modellerinin daha iyi sonuçlar üretmesi için net ve detaylı spesifikasyonların önemini vurgular.

### Teknik Borç *(Technical Debt)*

Hızlı geliştirme için kısa vadeli çözümler tercih edildiğinde oluşan, gelecekte daha fazla efor gerektirecek kod kalitesi eksikliğidir. Zaman içinde biriken teknik borç, bakım maliyetlerini artırır ve yeni özellik eklemeyi zorlaştırır. **Refactoring** ve kod iyileştirmeleriyle azaltılır. Bilinçli bir şekilde teknik borç alınabilir ancak uzun vadede yönetilmesi gerekir.

### UX *(User Experience)*

Bir kullanıcının bir ürün veya hizmetle etkileşiminden elde ettiği genel deneyimi ifade eder. Kullanılabilirlik, erişilebilirlik, performans ve estetik gibi faktörleri içerir. İyi bir UX, kullanıcı memnuniyetini artırır ve ürünün başarısına katkıda bulunur.

### XML *(eXtensible Markup Language)*

Veri depolama ve taşıma için tasarlanmış, etiket *(markup)* tabanlı işaretleme dilidir. İnsan ve makine tarafından okunabilir yapılandırılmış veriler oluşturur. Kendini tanımlayan etiketler kullanır ve hiyerarşik bir yapıya sahiptir. Web servisleri, konfigürasyon dosyaları ve veri alışverişinde kullanılır.

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
