<a id="top"></a>

# Ders Rehberi

[← İçindekiler](README.md#top)

Dersin değerlendirme kriterleri, proje sunumları ve dönem projesi için öneriler.

## Ders Geçme Prosedürü

Bu dönem ilk kez işlenen müfredat kapsamında ders geçme kriterleri şöyle tanımlanmıştır: %40 Proje + %60 Final.

### Proje Değerlendirmesi

Proje değerlendirmesi için aşağıdaki temel kriterler göz önünde bulundurulacaktır.

| **Kriter** | **Açıklama** |
| ------ | -------- |
| **Takım** | En az 1 en fazla 4 kişilik takımlar oluşturulabilir. *(Sınıf mevcuduna göre değişir)* Takım üyelerinin projeye birbirlerine yakın ağırlıklarda katkı sağlaması beklenir. |
| **Dil Modeli** | Projede en az bir yapay zeka aracı kullanılmalıdır. Ürünün yazılmasında araç olarak, proje içerisinde bir yapay zeka entegrasyonu yaparak ya da her iki yaklaşımı bir arada ele almak mümkündür. Kullanım şekli proje dokümantasyonunda açıkça belirtilmelidir. |
| **Geliştirme Süreci** | Proje mutlaka **GitHub** veya benzeri bir versiyon kontrol sistemi üzerinde geliştirilmelidir. Gerçekçi bir tarihsel **commit** geçmişi sağlanmalıdır. Commit mesajları anlamlı ve açıklayıcı olmalıdır. En az 3 pull request yapılmış olmalıdır. |
| **Teknik Değerlendirme** | Aşağıdaki konular göz önünde bulundurulur. |
| | **Teknik Borç:** Kodun A kalite olduğunun herhangi bir ölçümleme aracı ile gösterilmesi veya raporlanması beklenir. *(örneğin SonarQube, CodeClimate)* |
| | **Güvenlik *(Security)*:** Kodun güvenlik açıkları içerip içermediği değerlendirilir. |
| | **Mimari Uyum:** Projenin seçilen ve planlanan mimariye ne kadar uyduğu göz önünde bulundurulur. |
| | **Okunabilirlik *(Readability)*:** Kodun anlaşılabilirliği ve bakım kolaylığı *(maintainability)* değerlendirilir. |
| | **Test Edilebilirlik:** Kodun birim testleri ve entegrasyon testleri ile ne kadar test edilebilir olduğuna bakılır. |
| **Dokümantasyon** | Proje ile ilgili mimari tasarım, kullanılan yapay zeka araçları, karşılaşılan zorluklar ve çözümler gibi konuları içeren bir README hazırlanmalıdır. |
| **Veritabanı** | Projede en az bir veritabanı kullanılmalıdır. *(SQL, NoSQL, In-Memory vb)* |
| **Sunum** | Proje tesliminde önce proje sunumu yapılmalıdır *(25 dakikayı geçmeyecek şekilde)* Sunumda projenin amacı, kullanılan teknolojiler, karşılaşılan zorluklar ve çözümler gibi konuların açıklanması beklenir. **Özellikle koda ne kadar hakim olunduğu sorgulanacaktır.** |
| **Teslim Tarihi** | Dönemin son dersi veya Üniversite tarafından belirlenen proje teslim tarihidir. |

### Final Sınavı

Final sınavında dönem boyunca işlenen konularla ilgili sorular yer alacaktır. Örnek sorulara [buradan](./Exam00.md) bakabilirsiniz.

## Uygulama Önerileri

Dönem başında sıfırdan özgün bir ürün fikri bulmaya çalışmak, çoğu zaman enerjinin yanlış yere harcanmasına yol açar. Bunun yerine günlük hayatta zaten kullandığınız, nasıl çalıştığını bildiğiniz popüler uygulamaların **küçük ölçekli klonlarını** *(clone)* geliştirmek daha verimli bir öğrenme yöntemidir; zira "ne yapmalıyım" sorusuyla uğraşmak yerine doğrudan "bunu nasıl doğru, güvenli ve sürdürülebilir bir şekilde yaparım" sorusuna odaklanabilirsiniz.

Aşağıdaki tabloda bu amaçla seçilmiş, farklı zorluk seviyelerinde proje önerileri yer almaktadır. **Önerilen Dil/Framework/Mimari** sütunundaki tavsiyeler bağlayıcı değildir. Hakim olduğunuz teknolojilerle de ilerleyebilirsiniz. Asıl olan seçtiğiniz yaklaşımı gerçekleştirebilmeniz.

> Projeyi seçtikten sonra unutmayın ki **Vibe Coding** pratiklerinden ziyade **Agentic Engineering** yaklaşımını benimseyerek hareket etmek daha doğru olur. Yani yapay zeka araçlarını birer yardımcı olarak kullanmak, ürettikleri çıktıları dikkatlice incelemek ve gerektiğinde müdahale ederek ilerlemek gerekir. Bu süreçte kod güvenilirliği, teknik borç ve proje mimarisi gibi konulara dikkat etmek önemlidir.

| **Kod** | **Proje Fikri** | **Açıklama** | **Zorluk** | **Önerilen Dil/Framework/Mimari** |
| --- | --- | --- | --- | --- |
| **P0001** | **Not Defteri** *(Google Keep / Evernote Clone)* | Kullanıcıların not oluşturup etiketleyebildiği, arayabildiği ve arşivleyebildiği basit bir not tutma uygulaması. CRUD operasyonları, kullanıcı bazlı yetkilendirme ve temel arama fonksiyonu üzerine odaklanır. | Kolay | .NET Minimal API veya Node.js/Express + PostgreSQL/SQLite, Layered Architecture |
| **P0002** | **Kitap Takip** *(Goodreads Clone)* | Kullanıcıların okuduğu/okuyacağı kitapları listeleyip puanladığı, yorum yazdığı bir uygulama. İleri seviyede, benzer kitap önerisi veya kitap özetleme için **RAG** tabanlı bir öneri motoru eklenebilir. | Kolay | Python *(FastAPI)* veya .NET + PostgreSQL, Clean Architecture |
| **P0003** | **Bilgi Yarışması** *(Kahoot / Duolingo Tarzı Clone)* | Kullanıcıların farklı kategori ve zorluk seviyelerinde soruları cevaplayarak puan ve rozet kazandığı bir yarışma uygulaması. Sorular statik bir veri setinden gelebileceği gibi bir dil modeline ürettirilebilir. | Kolay | .NET veya Node.js/TypeScript + RDBMS. Soru üretiminde **Prompt Engineering** |
| **P0004** | **Görev Panosu** *(Trello Clone)* | Pano, liste ve kart yapısıyla iş takibi yapılan, kartların sürükle-bırak ile taşınabildiği bir uygulama. Başka bir kullanıcının taşıdığı kartın ekranda anlık görünmesi *(gerçek zamanlı güncelleme)* önemli bir teknik zorluktur. | Orta | .NET + SignalR veya Node.js + Socket.io, Clean Architecture |
| **P0005** | **Mikroblog** *(Twitter/X Clone)* | Kullanıcıların kısa gönderi paylaşabildiği, birbirini takip edebildiği ve bir zaman tünelinde *(feed)* gönderileri görebildiği uygulama. Takipçi sayısı arttıkça feed'in nasıl üretileceği *(fan-out on write/read)* mimari bir tartışma konusudur. | Orta | Node.js/TypeScript + PostgreSQL veya .NET, Event-Driven besleme tasarımı |
| **P0006** | **Oyun Mağazası** *(Steam Clone)* | Oyun kataloğu, kullanıcı yorum/puanları, oyun kütüphanesi ve kampanya yönetimi içeren bir uygulama. [`apps/lesson03`](./apps/lesson03) altındaki **Gamepedia** örneği bu proje için doğrudan bir başlangıç noktası olabilir. | Orta | .NET Clean Architecture + MongoDB/PostgreSQL |
| **P0007** | **Bilgi Tabanı** *(Notion Clone + Anlamsal Arama)* | Sayfa/blok tabanlı not tutma, sayfalar arası iç bağlantı kurma ve içerikte anlamsal arama yapabilme özellikli bir bilgi yönetim uygulaması. **RAG** pipeline'ı bu projenin ayrılmaz bir parçasıdır. | İleri | Python *(FastAPI)* veya .NET backend + Qdrant/Vektör Veritabanı |
| **P0008** | **Mini E-Ticaret** *(Trendyol/Amazon Clone)* | Ürün kataloğu, sepet, sipariş ve stok takibi içeren bir uygulama. [Bölüm 08](chapter-08.md#top)'deki Sipariş-Stok-Tedarikçi senaryosunun genişletilmiş, gerçek bir kod tabanına dönüştürülmüş halidir. | İleri | Microservices/Event-Driven mimari, RabbitMQ/Azure Service Bus, .NET veya Node.js |
| **P0009** | **Destek Masası** *(Zendesk Clone)* | Müşteri destek taleplerinin oluşturulduğu, kategorilere göre yönlendirildiği ve durumunun takip edildiği bir uygulama. Gelen talebi analiz edip yanıt önerisi sunan veya **MCP** üzerinden talep durumunu güncelleyen bir **Custom Agent** ile zenginleştirilebilir. | İleri | Custom Agent + MCP Server entegrasyonu, .NET veya Python |
| **P0010** | **Dijital Cüzdan** *(Revolut Tarzı Clone)* | Hesap, bakiye ve para transferi işlemlerinin yönetildiği bir uygulama. Odak noktası özellik zenginliği değil; yetkilendirme, hassas veri şifreleme, işlem geçmişi/denetim kaydı *(audit log)* ve tutarlılık *(**CAP Teoremi**'nde CP tercihi)* gibi güvenlik ve doğruluk kriterleridir. | İleri | .NET + Identity/JWT, CP odaklı tutarlılık yaklaşımı |
| **P0011** | **Mini Lisp/Scheme Yorumlayıcısı** | Aritmetik işlemler, koşullu ifadeler, fonksiyon tanımları ve özyinelemeyi (recursion) destekleyen minimal bir dil yorumlayıcısı *(lexer -> parser -> evaluator)*. Fonksiyonel programlamanın temel yapı taşlarını *(closure, immutability)* uygulamalı olarak öğretir. | İleri | OCaml, Rust veya Python |
| **P0012** | **Basit Bir Unix Shell** *(Bash Mini Clone)* | Komut çalıştırma, boru hattı *(pipe operatörü)* ve yönlendirme (`>`, `<`) operatörlerini destekleyen minimal bir komut satırı yorumlayıcısı. Sistem çağrıları *(process spawn, file descriptor yönetimi)* ile doğrudan çalışıldığı için işletim sistemi kavramlarını somutlaştırır. | İleri | Rust veya C/C++ |
| **P0013** | **Tetris Clone** | Klasik blok düşürme oyununun temel mekaniklerini *(çarpışma tespiti, satır temizleme, skor sistemi)* içeren bir oyun. Oyun döngüsü *(game loop)*, durum yönetimi ve basit render mantığını öğretir. | Orta | Rust (macroquad/bevy), Python (Pygame) veya C# (MonoGame) |
| **P0014** | **2048 / Yılan (Snake) Clone** | Basit kural setine sahip, hızlıca tamamlanabilecek bir oyun. Giriş seviyesinde durum yönetimi, klavye olay yönetimi *(event handling)* ve temel oyun mantığı pratiği için idealdir. | Kolay | JavaScript + HTML5 Canvas, Python (Pygame) veya C# |
| **P0015** | **Sistem Kaynak İzleyici** *(Task Manager / htop Clone)* | Çalışan process'leri, CPU/bellek kullanımını ve ağ trafiğini gerçek zamanlı listeleyen bir masaüstü uygulaması. İşletim sistemi API'leriyle doğrudan çalışmayı gerektirir; sistem programlamaya meraklı öğrenciler için uygundur. | Orta | Python (PyQt/Tkinter + psutil), Rust (egui) veya .NET (WPF/MAUI) |
| **P0016** | **IoT Sensör Paneli** *(Google Home / SmartThings Tarzı Clone)* | Sıcaklık, nem, hareket gibi sensör verilerinin simüle edilip bir mesaj kuyruğu üzerinden *(**MQTT**)* merkezi bir panele gerçek zamanlı aktarıldığı bir sistem. Donanım/yazılım kesişimini deneyimleme fırsatı sunar; [Bölüm 08](chapter-08.md#top)'deki asenkron mesajlaşma kavramlarıyla doğrudan örtüşür. | İleri | Python (paho-mqtt) sensör simülatörü + Node.js/React dashboard |
| **P0017** | **Mini ERP** *(Fatura ve Envanter Yönetimi, Logo/QuickBooks Tarzı Clone)* | Ürün stoğu, fatura kesme, cari hesap takibi ve basit raporlama içeren bir işletme yönetim uygulaması. İş süreçlerini yazılıma dökme pratiği sağlar. | Orta | .NET + Blazor veya Python + Django, Layered/Clean Architecture |
| **P0018** | **Basit Bir Tablolama Aracı** *(Excel/Google Sheets Mini Clone)* | Hücre tabanlı veri girişi, temel formüller *(`SUM`, `AVERAGE` vb.)* ve hücreler arası bağımlılık grafiğinin *(dependency graph)* yönetildiği bir uygulama. Formül ayrıştırma *(parsing)* ve hesaplama sırası *(topological sort)* konularıyla doğrudan ilişkilidir. | İleri | TypeScript/React (web) veya .NET WPF (masaüstü) |
| **P0019** | **Masaüstü Metin Editörü** *(Notepad++/VS Code Mini Clone)* | Söz dizimi renklendirme *(syntax highlighting)*, çoklu sekme yönetimi ve temel dosya işlemlerini içeren bir masaüstü metin editörü. | Orta | Rust (Tauri), Python (PyQt) veya Electron + TypeScript |
| **P0020** | **Sinyal Görselleştirme Aracı** *(Basit Bir Osiloskop Simülatörü)* | Mikrofon veya dosya girdisinden alınan ses/sinyal verisinin dalga formunu ve frekans spektrumunu *(**FFT**)* gerçek zamanlı görselleştiren bir masaüstü uygulaması. Sinyaller ve sistemler konusuyla doğrudan kesişir. | İleri | Python *(NumPy/SciPy + PyQt/Matplotlib)* veya Rust |
| **P0021** | **Mini Redis *(Redis Clone)*** | RESP protokolüyle konuşan, GET/SET/EXPIRE ve liste/hash komutlarını destekleyen bellek içi anahtar-değer sunucusu. Eşzamanlı istemci yönetimi, TTL ve disk kalıcılığı (AOF/snapshot) öğretir. Gerçek redis-cli ile test edilebilmesi somut bir başarı ölçütü sağlar. | İleri | Go (goroutine/channel) veya Zig |
| **P0022** | **Alışkanlık Takipçisi *(Habitica / Loop Habit Tracker Clone)*** | Günlük alışkanlıkların işaretlendiği, seri (streak) ve istatistiklerin gösterildiği mobil uygulama. Çevrimdışı öncelikli (offline-first) yerel kayıt, hatırlatıcı bildirimler ve isteğe bağlı bulut senkronizasyonu içerir. | Kolay | Dart + Flutter, SQLite/Firebase |
| **P0023** | **Sohbet Sunucusu *(Discord/Slack Clone)*** | Sunucu/kanal yapısı, gerçek zamanlı mesajlaşma, çevrimiçi durumu (presence) ve okunmadı sayaçları olan sohbet uygulaması. BEAM üzerinde süreç tabanlı eşzamanlılık ve supervisor ile hata toleransı uygulatır; Actor Model'i doğrudan deneyimletir. | İleri | Elixir + Phoenix (Channels/LiveView) + PostgreSQL |
| **P0024** | **Yemek Siparişi *(Yemeksepeti/Getir Clone)*** | Restoran menüsü, sepet, sipariş durumu ve kurye konumunun haritada izlendiği native mobil uygulama ve API. Push bildirim ve sipariş yaşam döngüsü için durum makinesi (state machine) pratiği sağlar. | Orta | Kotlin (Jetpack Compose) ve/veya Swift (SwiftUI) + PostgreSQL |
| **P0025** | **Etkinlik Bileti *(Biletix/Eventbrite Clone)*** | Etkinlik listeleme, salon/koltuk seçimi, süreli koltuk rezervasyonu ve bilet satışı. Aynı koltuğun iki kez satılmaması için transaction, iyimser kilitleme (optimistic locking) ve TTL konularına odaklanır. | Orta | Java (Spring Boot) veya Ruby (Rails) + PostgreSQL + Redis |

---

[← İçindekiler](README.md#top)
