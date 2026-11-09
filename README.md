<a id="top"></a>

# Yapay Zeka Destekli Yazılım Geliştirme

Sektör Kampüste projesi kapsamında lisans bölümleri için açılan derse ait doküman ve örnek uygulamaların yer aldığı bir repodur.

Ders iki eksen üzerinde ilerler. İlk kısımda geleneksel temel yazılım geliştirme konuları *(hata yönetimi, kod kalitesi, test, mimari, dağıtık sistemler vb)* ele alınır. İkinci kısımda ise modern yapay zeka araçlarıyla bu temellere bağlı kalarak nasıl yazılım geliştirileceği incelenir.

## Kısım I: Yazılım Geliştirmenin Temelleri

| **Bölüm** | **Açıklama** | **Etiketler** |
| --- | --- | --- |
| [Bölüm 01: Ders Hakkında, Yapay Zeka Terminolojisi ve İlk Uygulama](chapter-01.md) | Dersin amacı ve dönem boyunca cevabı aranan sorular. Martin Keen'in yapay zeka periyodik tablosu üzerinden temel terimler. JSON formatındaki bir CV'nin prompt'larla HTML sayfasına dönüştürüldüğü ilk uygulama ve çıkarılan dersler. | `prompt` `ai-periodic-table` `embedding` `rag` `hello-world` |
| [Bölüm 02: CV Bank Projesi için Prototip Geliştirme](chapter-02.md) | Mimari özet ve domain dokümanlarıyla yapay zeka asistanına bağlam sağlayarak Clean Architecture yapısında bir .NET 10 REST API prototipinin geliştirilmesi. | `prototype` `clean-architecture` `rest-api` `dotnet` `swagger` `domain-docs` |
| [Bölüm 03: Exception Handling, Debugging ve Docker Kullanımı](chapter-03.md) | Yönetimli ortamlarda istisna yönetimi, call stack okuma ve debugging. MongoDB örneği üzerinden Docker ve docker compose temelleri, sık kullanılan CLI komutları. | `exception-handling` `call-stack` `debugging` `docker` `docker-compose` `mongodb` |
| [Bölüm 04: Hata Yönetimi, Loglama ve İzleme](chapter-04.md) | Hata türlerinin ayrıştırılması ve farklı dillerde hata yönetimi yaklaşımları. Grafana, Loki, Prometheus ve Alloy ile kurulan çok servisli bir örnek üzerinde loglama, metrikler, alarmlar ve hata simülasyonu. | `error-handling` `logging` `observability` `grafana` `loki` `prometheus` `alerting` |
| [Bölüm 05: Bağımlılıkları Yönetmek ve Kod Kalitesini Ölçmek](chapter-05.md) | SOLID, Clean Code ve tasarım kalıplarına genel bakış, interface ve dependency inversion ile IoC. Gamepedia örneği üzerinde SonarQube ile kod kalitesinin ölçülmesi. | `solid` `clean-code` `dependency-inversion` `ioc` `sonarqube` `technical-debt` |
| [Bölüm 06: Yazılım Çözümlerinde Testin Önemi](chapter-06.md) | Test piramidi, birim ve entegrasyon testleri, TDD'nin temel prensipleri. .NET üzerinde örnek test projeleri ve code review, pair programming gibi kaliteyi destekleyen pratikler. | `test-pyramid` `unit-test` `integration-test` `tdd` `dotnet` |
| [Bölüm 07: Yazılım Mimarileri ve Temel Seviyede Bir Örnek](chapter-07.md) | Bayi otomasyonu senaryosu üzerinden katmanlı, servis tabanlı, event-driven, serverless ve diğer mimari stillerin ölçeklenebilirlik, test edilebilirlik ve basitlik gibi ölçütlerle karşılaştırılması. | `software-architecture` `layered` `event-driven` `serverless` `architecture-tradeoffs` |
| [Bölüm 08: Dağıtık Sistemler Hakkında Temel Bilgiler ve Basit Bir Senaryo Üzerinden İnceleme](chapter-08.md) | Dağıtık sistemlerin yanılgıları, CAP teoremi, senkron ve asenkron iletişim. Sipariş, stok ve tedarikçi servisleri senaryosunda retry, circuit breaker, idempotency ve saga gibi dayanıklılık desenleri. | `distributed-systems` `cap-theorem` `async-messaging` `circuit-breaker` `saga` |

## Kısım II: Yapay Zeka ile Yazılım Geliştirme

| **Bölüm** | **Açıklama** | **Etiketler** |
| --- | --- | --- |
| [Bölüm 09: RAG (Retrieval Augmented Generation) Yaklaşımı I](chapter-09.md) | Prompt, bağlam penceresi ve bağlam yönetimi. Embedding, vektör uzayı ve benzerlik araması kavramlarıyla RAG yaklaşımının çalışma prensibi. | `context-window` `embedding` `vector-search` `rag` |
| [Bölüm 10: RAG (Retrieval Augmented Generation) Yaklaşımı II](chapter-10.md) | Qdrant ve LM Studio ile yerel bir bilgi deposunun oluşturulması ve Python tabanlı bir RAG istemcisinin geliştirilmesi. Agentic RAG ve Graph RAG'e kısa bakış. | `rag` `qdrant` `lm-studio` `python` `agentic-rag` |
| [Bölüm 11: RAG'ın Ötesi: Graph RAG, Knowledge Graph, CAG ve KAG](chapter-11.md) | Vektör RAG ile Graph RAG ve knowledge graph yaklaşımlarının karşılaştırılması. RAG, CAG *(Cache-Augmented Generation)* ve KAG *(Knowledge-Augmented Generation)* yöntemlerinden hangisinin hangi senaryoda tercih edileceği. | `graph-rag` `knowledge-graph` `cag` `kag` |
| [Bölüm 12: MCP (Model Context Protocol) Kavramı ve MCP Server Yazılması](chapter-12.md) | Klasik Web API ile MCP'nin karşılaştırılması; MCP host, client ve server rolleri. Stdio, SSE ve Streamable HTTP taşıma katmanları ve bir MCP server yazılması. | `mcp` `mcp-server` `stdio` `sse` `streamable-http` |
| [Bölüm 13: MCP Server'lar ile Çalışmak](chapter-13.md) | Tasarım dokümanlarından Node.js tabanlı bir Weather API ve onu kullanan FastMCP tabanlı Python MCP server'ının geliştirilmesi, `mcp.json` ile istemciye bağlanması. Loki logları üzerinde çalışan log-mcp-lab örneği. | `mcp` `fastmcp` `nodejs` `python` `mcp-json` `loki` |
| [Bölüm 14: Custom Agent, Skill ve Plugin Yapıları ile Çalışmak](chapter-14.md) | Custom agent tanımları, frontmatter ve talimat yapısı. Skill'lerin kurgusu ve referans dosyaları. Birden fazla skill, agent ve hook'un Claude plugin olarak paketlenmesi. | `custom-agent` `skill` `frontmatter` `claude-plugin` `hooks` |
| [Bölüm 15: Döngü Mühendisliği (Loop Engineering)](chapter-15.md) | Ajanı ölçülebilir bir hedefe götüren döngü tasarımı. LoopLab örneğinde SonarQube ve testlerle tur tur ilerleyen senaryo; hook, headless tur ve tetikleyicilerle döngünün otomatikleştirilmesi. | `loop-engineering` `agentic-workflow` `hooks` `headless` `sonarqube` |
| [Bölüm 16: Token Kullanımlarını Open Telemetry ve Aspire Dashboard ile İzlemek](chapter-16.md) | Yapay zeka çağrılarının ve token tüketiminin OpenTelemetry ile toplanıp Aspire Dashboard'da izlenmesi. VS Code Copilot ve Copilot CLI için kurulum adımları. | `opentelemetry` `aspire-dashboard` `token-usage` `copilot` `cost` |
| [Bölüm 17: LoRA *(Low-Rank Adaptation)* ile Model Özelleştirme](chapter-17.md) | Ana model ağırlıklarını dondurup düşük dereceli matrislerle ince ayar yapan LoRA tekniği. RunPod üzerinde GPU ile Unsloth eğitimi, GGUF'a dönüştürme ve LM Studio'da test. | `lora` `fine-tuning` `runpod` `unsloth` `gguf` `lm-studio` |
| [Bölüm 18: Yapay Zeka Destekli Yazılım Geliştirmede Güvenlik](chapter-18.md) | Prompt injection, dolaylı prompt injection, tool poisoning ve malware injection riskleri. Otonom DevOps ajanı senaryosu ve Docker sandbox ile güvenli kod çalıştırma PoC'si. | `prompt-injection` `tool-poisoning` `malware-injection` `sandbox` `ai-security` |

## Ek Kaynaklar

- [Ders Rehberi: Değerlendirme Kriterleri ve Proje Önerileri](course-guide.md)
- [Terimler Sözlüğü](Dictionary.md)
- [Örnek Final Sınavı](Exam00.md)
- [Kaynakça](References.md)

## Aman Dikkat

- Yapay zeka botları ile çalışırken şifre, gizli anahtar, kişisel veri gibi hassas bilgileri prompt'lara dahil etmekten kaçınmalısınız. Bu tür bilgilerin istemeden de olsa loglanması veya üçüncü taraflarla paylaşılması ciddi güvenlik risklerine yol açabilir. Lisanslı modeller kullanırken de mutlaka sözleşme şartlarını dikkatlice inceleyin ve gizlilik politikalarını anlayın.
- Yapay zeka araçlarının ürettiği kodların güvenlik açıkları içermediğinden emin olmak için kodu dikkatlice inceleyin ve gerekirse güvenlik tarama araçları kullanarak analiz edin. Özellikle web uygulamaları geliştirirken **SQL injection**, **XSS** gibi yaygın güvenlik açıklarına karşı dikkatli olmak gerekiyor.
- Birçok yapay zeka aracı vardır, **CLI**, **Context Window**, **MCP**, **RAG**, **Custom Agents**, **Skills** gibi farklı araçların ne işe yaradığını, hangi senaryolarda kullanılması gerektiğini ve birbirleriyle nasıl ilişkilendirilebileceğini iyi anlamak önemlidir. Bu araçları bilinçli bir şekilde kullanmak, güvenlik risklerini azaltmaya yardımcı olur.
- Kullanılan teknik ne olursa olsun yapay zeka araçları dil modelleri ile çalıştıklarından token tüketir. Token'lar lisanslama modellerine göre farklı maliyetlere sahip olabilirler. Senaryoya göre doğru araçları seçmek, basit bir prompt kullanırken bile token maliyetinin çok yüksek olabileceğini göz önüne almak önemlidir. Harcanan token maliyetlerini local ortamda ölçümlemek için [Bölüm 16](chapter-16.md#top)'daki teknikleri kullanabilirsiniz.
- Kodun yüksek kalitede olduğunu garanti etmek için statik kod tarama araçlarından yararlanın. Örneğin, .NET projeleri için **SonarQube**, **JavaScript** projeleri için **ESLint** gibi araçlar ile kod kalitesini sıklıkla ölçün. Code Review ve Pull Request gibi süreçleri atlamayın, insan denetimi her zaman önemlidir.
- Kendi yapabileceğimiz çok basit bir kod parçasını yapay zeka aracına yazdırmak yerine, yapay zeka araçlarını daha karmaşık, zaman alan, aynı görevin sürekli tekrar ettiği işler için kullanmak daha verimli olabilir. Örneğin bir **MongoDB Docker** imaj tanımını resmî sitesinden alıp projeye uygulamayı yapay zeka aracına yazdırmak yerine, ayağa kaldırdığımız bir imajın çalışması ile ilgili içinden çıkamadığımız bir hatayı çözmek için yapay zeka aracından yardım almak daha verimli olabilir.
- İyi **prompt**'lar vermek, yapay zeka araçlarından kaliteli çıktılar almak için kritik öneme sahiptir. Prompt'larınızda açık ve net olun, gerekli detayları sağlayın ve mümkünse örnekler verin. Çıktıları mutlaka dikkatlice inceleyin, ispat arayın, doğruluğundan emin olun. Yapay zeka araçlarının ürettiği çıktıları denetleyen kodlar da geliştirebilirsiniz ;-)
- Yapay zeka araçlarının ürettiği konfigürasyon içeriklerinde şifre, gizli anahtar gibi bilgiler varsa klasik metodolojide olduğu gibi bunları daha güvenli ortamlarda *(Vault, Azure Key Vault, AWS Secrets Manager gibi)* saklamayı tercih edin. Yapay zeka araçlarının ürettiği kodlarda bu tür bilgilerin hardcoded olarak yer almamasına da ayrıca dikkat edin.
- Özellikle **SKILL** gibi araçlar dosya sistemine erişip kod çalıştırabilirler. Bu nedenle dikkatli kullanılmaları ve Guardrails gibi güvenlik önlemleriyle desteklenmeleri önemlidir. Skill'lerin ne tür işlemler yapabileceğini, hangi kaynaklara erişebileceğini ve bu işlemlerin güvenli olup olmadığını dikkatlice değerlendirin.
