<a id="top"></a>

# Bölüm 11: RAG'ın Ötesi: Graph RAG, Knowledge Graph, CAG ve KAG

<!-- nav:top:start -->
[← Bölüm 10](chapter-10.md#top) · [İçindekiler](README.md#top) · [Bölüm 12 →](chapter-12.md#top)
<!-- nav:top:end -->

## Klasik RAG, Graph RAG ve Knowledge Graph'ler

Klasik RAG *(Retreival-Augmented Generation)* vektör RAG olarak da bilinir. Bu mimari yaklaşım büyük hamicli ve yapılandırılmamış metinleri anlamsak benzerliklerine göre taramak, belge kümeleri içinde konuya dair hızlı arama yapmak gibi görevler için biçilmiş kaftandır. Verinin modellemesi, sisteme veri alımı *(data ingestion)* donanımsal ve mantıksal olarak görece daha basittir. Örneğin kaynaklardaki metinler belirlenen boyutlarda *(100, 500, 1000 token gibi)* parçalara böülünür, embedding modelleri aracılığıyla çok boyutlu vektör karşılıkları çıkarılır ve bir vektör veritabanında indekslenerek kaydedilir. Kullanıcı bir soru sorduğunda, bu sorunun da vektör değeri hesaplanır ve veritabanındaki vektörler arasında matematiksel mesafe ölçümleri yapılarak en yakın metin blokları geri çağrılır *(retreival aşaması)* Bu sistemi **FAQ *(Frequently Asking Question)*** gibi sık güncellenen ya da yüzeysel doküman içi bilgi bulma senaryolarında tercih edebiliriz.

Diğer yandan klasör RAG bağlam pencereleri *(Context Window)* içerisinde izole metin bloklarını modelin kullanımına sunduğunda parçalar arası tarihsel, nedensel ve yapısal bağları kurmak zordur. Bunu şöyle örnekleyebiliriz; RAG sisteminin zengin bir blog içeriğinde dokümanları kaynak olarak kullandığını düşünelim. Yazıların birisinde `Programı Microsoft.SemanticKernel kütüphanesini kullanarak geliştirdim.` cümlesi olsun. Bir süre sonra sisteme giren başka bir devam yazısında ise `Geliştirdiğim bu yeni projede denemeler için yerele LLM'leri kullanmayı tercih ettim.` şeklinde bir ifade geçsin. Klasik RAG düzeneğinde bu iki bilginin aslında aynı projeye ait evrimsel bir bilgi olduğu mantıksal olarak cevaplanamaz.

**Graph RAG** mimarisinde ise, *yapılandırılMAmış* verinin *yapılandırılmış* bir ilişkisel ağ sistemine dönüştürülmesi söz konusudur. İşte bu noktada az önceki örnekte belirttiğimiz bağlam eksikliği ve farkında olmama durumu ortadan kalkar. Entity'ler ve aralarındaki ilişkiler açıkça ve ksin çizgilerle tanımlandığı için, **retreival** işlemi sırasında dil modeli, rastgele vektöre parçaları arasında matematiksel bir eşleştirme yapmak yerine, önceden tanımlanmış mantıksal bir ağ *(graph)* üzerinde doğrudan hedefe yönelik bir arama işlemi gereçleştirir *(graph traversal)*

Aşağıdaki tabloda bu iki yöntem arasındaki temel farklar özetlenmektedir.

| **Özellik** | **Klasik RAG** | **GraphRAG** |
| --- | --- | --- |
| **Çalışma Prensibi** | Anlamsal benzerliklere dayalı mesafe ölçümü (Kosinüs, Öklid) ve izole metin blokları | Düğümler *(nodes)* ve sınırlar *(edges)* üzerinden bağlantıları kurma |
| **Bağlamsal Kalıcılık** | Metin parçaları tek başına anlam ifade eder ve tarihsel *(historic)* ya da nedensel bağlamlar zayıftır | Node'lar arası ilişkiler uzun süreli korunabilir ve daha sürdürülebilirdir |
| **Halüsinasyon Riski** | Riskli. İlgisiz ama anlamsal olarak benzer parçaları yanlışlıla birleştirilebilir | Düşük Risk. Modelin yanıtı yalnızca yapısal ve doğrulanmış gerçeklere göre *(Faktörel Graf - Fact/knowledge Graph)* üretilir |
| **Ölçeklenebilirlik** | Daha çok dağıtık sistemlere yatkın, suncular arası parçalama *(sharding)* nispeten basit | Ağ boyutu büyüdükçe ilişkilerin bütünlüğünü korumak zorlaşır, dağıtık sistemde daha iyi bir planlama gerektirir |
| **Veri Alım İşlem Maliyeti** | Başlangıçta düşük maliyet. Sadece *embedding* sırasında CPU/GPU kaynakları harcanır | Başlangıçta yüksek maliyet. Verinin yapılandırılması, düğümlerin ve sınırların tanımlanması daha karmaşıktır |
| **Açıklanabilirlik *(Explainability*)** | Düşük. Kullanıcılara sadece hangi metin bloğunun benzerlikten dolayı getirildiği gösterilebilir. | Oldukça yüksek. Yanıtın hangi belirleyici *entity* ve ilişkilerden *(subgraphs)* türetildiği denetlenebilir. |
| **Senaryo** | Bilgi havuzu taraması, SSS *(Sıkça Sorulan Sorular)*, doküman içi veya konu odaklı arama | Çok sekmeli sorgular, kavramsal sentez, yapısal çıkarım, bütünsel veri seti anlama |

**Graph RAG** ile **Vektör RAG** arasında belirgin farklar vardır ama bunlardan belki de en önemlisi açıklanabilirlik ilkesidir. Finansal hizmetler, tıp araştırmaları, sigortacılık gibi regülasyonları sıkı takibe dayanan, görev kritik süreçler barındıran endüstrilerde bir yapay zeka modelinin belirli bir analitik sonuca nasıl, hangi verilere dayanarak ulaştığını geriye dönük kesin bir şekilde kanıtlamak gerekir. **Graph RAG**, bu tür senaryolarda daha uygun bir mimari yaklaşım olarak öne çıkar.


## RAG, CAG ve KAG

Güncel olarak tartışma konularından birisi de dil modellerini eğitilmedikleri veya bilmedikleri konularda kullanırken nasıl tutarlı sonuçlar elde edileceğiz. **RAG *(Retrieval-Augmented Generation)***, **CAG *(Cache-Augmented Generation)*** ve **KAG *(Knowledge-Augmented Generation)*** gibi yaklaşımlar bu soruna çözüm arayan yöntemler arasındadır. Bunlar hangi senaryolarda kullanabileceğimi bilmek önemlidir.

**RAG** yaklaşımında bilgi sorgu anında getirilir. Dokümanlar önceden parçalara *(chunk)* bölünür, embedding’leri çıkarılır ve bir vektör veritabanına yazılır. Kullanıcı soru sorduğunda, sorunun embedding’ine en benzer parçalar bulunur ve prompt’a eklenir. Model cevabı bu parçalara dayanarak üretir. Güçlü yanı ölçeklenmesidir. Milyonlarca doküman olabilir, veri sık değişebilir; yeni doküman eklemek için sadece indekslemek yeterlidir. Kaynak gösterme *(citation)* ve doküman bazlı yetkilendirme de doğal olarak kurulabilir. Zayıf yanı, cevap kalitesinin tamamen retrieval kalitesine bağlı olmasıdır. Yanlış parça gelirse model yanlış cevap verir. Kurumsal bilgi havuzları ve wiki üzerinden arama asistanı, büyük doküman arşivleri, sürekli güncellenen içerik *(destek talepleri, ürün katalogları, haberler gibi*), farklı kullanıcıların farklı dokümanlara erişebildiği sistemlerde göz önüne alınabilir.

**CAG** yaklaşımında fikir oldukça basittir. Bilgi kümesi modelin context penceresine sığıyorsa retrieval’a hiç gerek yoktur. Tüm bilgi önceden context’e yüklenir ve modelin bu kısım için hesapladığı vektörel değerler cache saklanır. Sonraki her soru bu hazır cache’in üzerine eklenir, böylece doküman her seferinde yeniden işlenmez. Günümüz API’lerindeki prompt caching özelliği, bu fikrin pratikteki karşılığıdır. Güçlü yanı, retrieval hatasını ortadan kaldırmasıdır. Model her zaman bilginin tamamını görür. Dolayısıyla parçalar arası ilişkileri de kurabilir. Bu yaklaşımda vektör veritabanı, embedding pipeline’ı ya da chunking stratejisi yoktur. Cache hazırlandıktan sonra gecikme *(latency)* de  düşüktür. Pek tabii bilgi context penceresine sığmalıdır. Uzun context’lerde model ortadaki bilgiyi gözden kaçırabilir *(`lost in the middle` etkisini araştıralım)*. Veri değiştiğinde cache yeniden oluşturulmalıdır. Ayrıca her istek tüm bağlamı taşıdığı için, cache kullanılsa bile maliyet hesabı yapılmalıdır.
Tek bir ürün kılavuzu ya da teknik dokümantasyon üzerine soru cevap, şirket içi politika ve yönetmelik asistanı, bir ders müfredatı ve yönergeleri üzerine öğrenci asistanı, tek bir sözleşme ya da şartname üzerinde analiz gibi senaryolar için idealdir. İlgili senaryolarda ortak nokta bilgi kümesinin sınırlı, görece statik ve sık sorgulanan bir yapıda olmasıdır.

**KAG**, Ant Group’un [OpenSPG](https://github.com/openspg/kag) ekibinden çıkan bir yaklaşım. Öözellikle uzmanlık gerektiren alanlarda RAG’in yetersiz kaldığı noktalara odaklanmakta. Burada bilgi düz metin parçaları olarak değil, yapılandırılmış bir bilgi grafiği *(knowledge graph)* olarak hazırlanır. Entity'ler *(kişi, ilaç, kanun maddesi, ürün gibi)* ve aralarındaki ilişkiler bir şemaya göre çıkarılır, grafik ile orijinal metin parçaları arasında karşılıklı indeksleme yapılır. Sorgu anında soru mantıksal adımlara ayrıştırılır. Her adım uygun olana göre bir grafik sorgusuyla bir metin aramasıyla ya da bir hesaplamayla çözülür ve sonuçlar zincirlenir. Güçlü yanı, çok adımlı akıl yürütme ve kesinlik gerektiren sorularda çok daha tutarlı olmasıdır. İlişkiler açık olduğu için cevabın nasıl elde edildiği de izlenebilir. Regülasyonların ön planda olduğu sektörlerde önemli bir avantaj sağlar ancak bedeli yüksektir. Şema *(Schema)* tasarımı alan *(domain)* uzmanlığı ister. Bilgi çıkarımı *(entity/relation extraction)* maliyetlidir ve hata yapabilir. Grafiğin güncel tutulması ise ayrı bir mühendislik yüküdür. Microsoft’un GraphRAG yaklaşımına da bu açıdan değerlendirilebilir. O da graph sistemini kullanır ama ağırlık daha çok topluluk *(community)* özetleri ve geniş kapsamlı sorulardadır. KAG ise mantıksal akıl yürütmeye ve domain bilgisinin hassasiyetine odaklanır. Tıbbi bilgi sistemleri *(ilaç etkileşimleri, tanı-tedavi ilişkileri)*, hukuk *(kanun maddeleri arası atıflar)*, finans ve risk analizi *(şirketler arası ortaklık ve sahiplik zincirleri)*, e-devlet hizmetleri *(hangi koşulda hangi belge gerekir)* ve karmaşık ürün konfigürasyonlarında tercih edilebilir.

Buradaki yaklaşımlar zaman içerisinde değişikliğe uğrayabilir veya tamamen güncelliğini yitirebilir. Yine de karar verme noktasında aşağıdaki bilgi tablosundan yararlanabiliriz.

| | **RAG** | **CAG** | **KAG** |
| --- | --- | --- | --- |
| **Bilgi ne zaman verilir?** | Sorgu anında, konu ile ilgili parçalar eklenir `[1, 2]` | Önceden ve tamamı `[2]` | Önceden yapılandırılır, sorgu anında akıl yürütülür `[3]` |
| **Veri Boyutu** | Büyük olabilir | Context'e sığmaldır `[2]` | Büyük olabilir |
| **Veri değişim sıklığı** | Sık değişebilir `[3]` | Görece statik | Orta *(Graph güncelleme maliyeti vardır)* |
| **Çok adımlı sorularda başarım** | Zayıf `[5]` | Ortalama ve üstü | Güçlü `[3]` |
| **Kurulum karmaşıklığı** | Kolay | Basit `[2]` | Zor |

1. Lewis, P., Perez, E., Piktus, A., Petroni, F., Karpukhin, V., Goyal, N., … Kiela, D. (2020). Retrieval-augmented generation for knowledge-intensive NLP tasks. Advances in Neural Information Processing Systems, 33, 9459–9474. arXiv:2005.11401
2. Chan, B. J., Chen, C.-T., Cheng, J.-H., & Huang, H.-H. (2025). Don’t do RAG: When cache-augmented generation is all you need for knowledge tasks. Companion Proceedings of the ACM Web Conference 2025 (WWW ‘25). `https://doi.org/10.1145/3701716.3715490` (arXiv:2412.15605)
3. Liang, L., Sun, M., Gui, Z., Zhu, Z., Jiang, Z., Zhong, L., … Zhou, J. (2024). KAG: Boosting LLMs in professional domains via knowledge augmented generation (v3). arXiv:2409.13731. `https://doi.org/10.48550/arXiv.2409.13731`
4. Liu, N. F., Lin, K., Hewitt, J., Paranjape, A., Bevilacqua, M., Petroni, F., & Liang, P. (2024). Lost in the middle: How language models use long contexts. Transactions of the Association for Computational Linguistics, 12, 157–173. `https://doi.org/10.1162/tacl_a_00638`
5. Tang, Y., & Yang, Y. (2024). MultiHop-RAG: Benchmarking retrieval-augmented generation for multi-hop queries. arXiv:2401.15391

<!-- nav:bottom:start -->
---

| ← Önceki Bölüm | İçindekiler | Sonraki Bölüm → |
| :--- | :---: | ---: |
| [Bölüm 10: RAG (Retrieval Augmented Generation) Yaklaşımı II](chapter-10.md#top) | [İçindekiler](README.md#top) | [Bölüm 12: MCP (Model Context Protocol) Kavramı ve MCP Server Yazılması](chapter-12.md#top) |
<!-- nav:bottom:end -->
