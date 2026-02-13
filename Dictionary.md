# Sözlük

Burası ders müfredatında incelenen konulara ait teknik terimlerin özetlendiği bir sözlüktür.

## Terimler

### .gitignore

Git versiyon kontrol sisteminde hangi dosya ve klasörlerin takip edilmeyeceğini belirten yapılandırma dosyasıdır. Genellikle geçici dosyalar *(Temp files)*, bağımlılıklar *(node_modules)*, derleme çıktıları ve hassas bilgiler içeren dosyalar bu dosyaya eklenir. Her satıra bir kural yazılır ve wildcardlar (*) kullanılabilir. Genellikle proje başlangıcında oluşturulması önerilir.

### Bootstrap

Web geliştirme için kullanılan açık kaynaklı, ücretsiz bir **CSS framework**'dür. **Responsive** ve **mobil öncelikli** yaklaşımda tasarlanmış hazır bileşenler ve layout sistemleri içerir. Geliştiricilerin hızlı ve tutarlı kullanıcı arayüzleri oluşturmasını sağlar. **HTML**, **CSS** ve **JavaScript** ile çalışır.

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

### SOAP *(Simple Object Access Protocol)*

**XML** tabanlı, ağ üzerinden mesaj alışverişi için kullanılan protokoldür. Kurumsal uygulamalarda web servisleri için bir standart haline gelmiştir. Güçlü tip kontrolü ve güvenlik özellikleri sunar. **REST**'e göre daha ağır ve karmaşıktır ancak daha fazla standart ve özellik içerir.

### Teknik Borç *(Technical Debt)*

Hızlı geliştirme için kısa vadeli çözümler tercih edildiğinde oluşan, gelecekte daha fazla efor gerektirecek kod kalitesi eksikliğidir. Zaman içinde biriken teknik borç, bakım maliyetlerini artırır ve yeni özellik eklemeyi zorlaştırır. **Refactoring** ve kod iyileştirmeleriyle azaltılır. Bilinçli bir şekilde teknik borç alınabilir ancak uzun vadede yönetilmesi gerekir.

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
