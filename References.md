<a id="top"></a>

# Kaynakça

[← İçindekiler](README.md#top)

Bu sayfada ders notlarında yararlanılan, atıfta bulunulan veya konuyu derinleştirmek isteyenler için önerilen kaynaklar bölümlere göre gruplanmıştır. Kitap ve akademik makaleler **APA 7** formatında, resmi dokümanlar ve web kaynakları ise kısa açıklamalarıyla birlikte linkli liste olarak verilmiştir. Yapay zeka araçlarıyla ilgili dokümanlar sık güncellendiğinden web kaynaklarında her zaman en güncel sürüme bakılması önerilir.

- [Genel Kaynaklar](#genel-kaynaklar)
- [Kısım I: Yazılım Geliştirmenin Temelleri](#kısım-i-yazılım-geliştirmenin-temelleri)
- [Kısım II: Yapay Zeka ile Yazılım Geliştirme](#kısım-ii-yapay-zeka-ile-yazılım-geliştirme)

## Genel Kaynaklar

**Resmi Dokümanlar ve Web Kaynakları**

- [Anthropic - Prompt Engineering Overview](https://docs.claude.com/en/docs/build-with-claude/prompt-engineering/overview) - Etkili prompt yazma teknikleri ve örnekleri.
- [Anthropic - Building Effective Agents](https://www.anthropic.com/engineering/building-effective-agents) - Ajan tabanlı sistemlerde iş akışı ve ajan desenleri *(prompt chaining, routing, evaluator-optimizer vb.)*.
- [OWASP Top 10](https://owasp.org/www-project-top-ten/) - Web uygulamalarındaki en kritik güvenlik riskleri *(SQL injection, XSS vb.)*. Ana sayfadaki "Aman Dikkat" bölümüyle ilişkilidir.

## Kısım I: Yazılım Geliştirmenin Temelleri

### [Bölüm 01: Ders Hakkında, Yapay Zeka Terminolojisi ve İlk Uygulama](chapter-01.md)

**Resmi Dokümanlar ve Web Kaynakları**

- [IBM Technology - AI Periodic Table Explained: Mapping LLMs, RAG & AI Agent Frameworks](https://www.youtube.com/watch?v=ESBMgZHzfG0) - Martin Keen'in yapay zeka terimlerini periyodik tablo benzetmesiyle anlattığı video.
- [IBM Think - Behind the Scenes with Tech Trailblazers: Meet Martin Keen](https://www.ibm.com/think/insights/behind-the-scenes-with-tech-trailblazers-meet-martin-keen) - Martin Keen hakkında tanıtım yazısı.

### [Bölüm 02: CV Bank Projesi için Prototip Geliştirme](chapter-02.md)

**Kitaplar ve Makaleler**

1. Martin, R. C. (2017). *Clean architecture: A craftsman's guide to software structure and design*. Prentice Hall.
2. Evans, E. (2003). *Domain-driven design: Tackling complexity in the heart of software*. Addison-Wesley.

**Resmi Dokümanlar ve Web Kaynakları**

- [Microsoft Learn - Common Web Application Architectures](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures) - .NET özelinde katmanlı ve Clean Architecture yapıları.
- [Microsoft Learn - Create Web APIs with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/web-api/) - ASP.NET Core ile REST API geliştirme.
- [OpenAPI Specification](https://spec.openapis.org/oas/latest.html) - Swagger arayüzlerinin dayandığı API tanımlama standardı.

### [Bölüm 03: Exception Handling, Debugging ve Docker Kullanımı](chapter-03.md)

**Resmi Dokümanlar ve Web Kaynakları**

- [Microsoft Learn - Best Practices for Exceptions](https://learn.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions) - .NET'te istisna yönetimi için öneriler.
- [MDN - HTTP Response Status Codes](https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status) - HTTP durum kodlarının açıklamaları.
- [Docker - Get Docker](https://docs.docker.com/get-started/get-docker/) - Farklı işletim sistemleri için Docker kurulumu.
- [Docker CLI Cheat Sheet](https://docs.docker.com/get-started/docker_cheatsheet.pdf) - Sık kullanılan Docker komutları.
- [Docker Compose Documentation](https://docs.docker.com/compose/) - Çok konteynerli uygulamaların tanımlanması ve çalıştırılması.
- [MongoDB Manual](https://www.mongodb.com/docs/manual/) - MongoDB resmi dokümantasyonu.

### [Bölüm 04: Hata Yönetimi, Loglama ve İzleme](chapter-04.md)

**Kitaplar ve Makaleler**

1. Armstrong, J. (2003). *Making reliable distributed systems in the presence of software errors* [Doktora tezi, Royal Institute of Technology (KTH)]. *"Let it crash"* felsefesi ve supervisor yaklaşımının temel kaynağı.
2. Beyer, B., Jones, C., Petoff, J., & Murphy, N. R. (Eds.). (2016). *Site reliability engineering: How Google runs production systems*. O'Reilly Media. [Çevrimiçi sürüm](https://sre.google/sre-book/table-of-contents/)
3. Majors, C., Fong-Jones, L., & Miranda, G. (2022). *Observability engineering: Achieving production excellence*. O'Reilly Media.

**Resmi Dokümanlar ve Web Kaynakları**

- [The Rust Programming Language - Error Handling](https://doc.rust-lang.org/book/ch09-00-error-handling.html) - `Result` ve `panic!` ile hata yönetimi.
- [The Twelve-Factor App - Logs](https://12factor.net/logs) - Logların olay akışı *(event stream)* olarak ele alınması.
- [OpenTelemetry Documentation](https://opentelemetry.io/docs/) - Trace, metric ve log toplamak için açık standart.
- [Prometheus - Alerting Rules](https://prometheus.io/docs/prometheus/latest/configuration/alerting_rules/) - Alarm kurallarının tanımlanması ve *Pending/Firing* durumları.
- [Grafana Loki Documentation](https://grafana.com/docs/loki/latest/) - Log toplama ve sorgulama sistemi.
- [Grafana Alloy Documentation](https://grafana.com/docs/alloy/latest/) - Telemetri verisini toplayıp ileten OpenTelemetry uyumlu ajan.

### [Bölüm 05: Bağımlılıkları Yönetmek ve Kod Kalitesini Ölçmek](chapter-05.md)

**Kitaplar ve Makaleler**

1. Cunningham, W. (1992). The WyCash portfolio management system. *Addendum to the Proceedings on Object-Oriented Programming Systems, Languages, and Applications (OOPSLA '92)*, 29–30. `https://doi.org/10.1145/157709.157715` *(Teknik borç metaforunun ilk kullanıldığı yer)*
2. Fowler, M. (2018). *Refactoring: Improving the design of existing code* (2nd ed.). Addison-Wesley.
3. Gamma, E., Helm, R., Johnson, R., & Vlissides, J. (1994). *Design patterns: Elements of reusable object-oriented software*. Addison-Wesley.
4. Martin, R. C. (2003). *Agile software development: Principles, patterns, and practices*. Prentice Hall. *(SOLID ilkeleri)*
5. Martin, R. C. (2008). *Clean code: A handbook of agile software craftsmanship*. Prentice Hall.

**Resmi Dokümanlar ve Web Kaynakları**

- [Martin Fowler - Inversion of Control Containers and the Dependency Injection Pattern](https://martinfowler.com/articles/injection.html) - IoC ve dependency injection kavramlarının klasik açıklaması.
- [Martin Fowler - Technical Debt](https://martinfowler.com/bliki/TechnicalDebt.html) - Teknik borç kavramı üzerine kısa yazı.
- [Microsoft Learn - Dependency Injection in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection) - .NET'in yerleşik DI altyapısı.
- [SonarQube Community Build Documentation](https://docs.sonarsource.com/sonarqube-community-build/) - Statik kod analizi, kalite kapıları *(quality gates)* ve teknik borç ölçümü.

### [Bölüm 06: Yazılım Çözümlerinde Testin Önemi](chapter-06.md)

**Kitaplar ve Makaleler**

1. Beck, K. (2002). *Test-driven development: By example*. Addison-Wesley.
2. Cohn, M. (2009). *Succeeding with agile: Software development using Scrum*. Addison-Wesley. *(Test piramidi kavramı)*
3. Khorikov, V. (2020). *Unit testing principles, practices, and patterns*. Manning.

**Resmi Dokümanlar ve Web Kaynakları**

- [Ham Vocke - The Practical Test Pyramid](https://martinfowler.com/articles/practical-test-pyramid.html) - Test piramidinin pratikte uygulanması.
- [Alistair Cockburn - Hexagonal Architecture](https://alistair.cockburn.us/hexagonal-architecture/) - Ports and Adapters yaklaşımının orijinal tanımı.
- [HexagonalArchitecture_101](https://github.com/buraksenyurt/HexagonalArchitecture_101) - Bölümde referans verilen örnek proje.
- [Microsoft Learn - Unit Testing Best Practices for .NET](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices) - .NET'te birim test yazma önerileri.
- [xUnit.net](https://xunit.net/) - Örnek projede kullanılan test framework'ü.
- [Playwright](https://playwright.dev/) - Uçtan uca UI testleri için framework.

### [Bölüm 07: Yazılım Mimarileri ve Temel Seviyede Bir Örnek](chapter-07.md)

**Kitaplar ve Makaleler**

1. Bass, L., Clements, P., & Kazman, R. (2021). *Software architecture in practice* (4th ed.). Addison-Wesley.
2. Richards, M., & Ford, N. (2025). *Fundamentals of software architecture: A modern engineering approach* (2nd ed.). O'Reilly Media. *(Bölümdeki mimari karşılaştırma tablosunun dayandığı kaynak)*

**Resmi Dokümanlar ve Web Kaynakları**

- [Martin Fowler - Architecture Decision Record](https://martinfowler.com/bliki/ArchitectureDecisionRecord.html) - ADR kavramı.
- [Michael Nygard - Documenting Architecture Decisions](https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions) - ADR formatını ilk öneren yazı.
- [Azure Architecture Center - Architecture Styles](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/) - Mimari stillerin karşılaştırmalı özeti.

### [Bölüm 08: Dağıtık Sistemler Hakkında Temel Bilgiler ve Basit Bir Senaryo Üzerinden İnceleme](chapter-08.md)

**Kitaplar ve Makaleler**

1. Brewer, E. A. (2000). Towards robust distributed systems [Keynote]. *Proceedings of the 19th Annual ACM Symposium on Principles of Distributed Computing (PODC '00)*. `https://doi.org/10.1145/343477.343502`
2. Brewer, E. (2012). CAP twelve years later: How the "rules" have changed. *Computer, 45*(2), 23–29. `https://doi.org/10.1109/MC.2012.37`
3. Garcia-Molina, H., & Salem, K. (1987). Sagas. *Proceedings of the 1987 ACM SIGMOD International Conference on Management of Data*, 249–259. `https://doi.org/10.1145/38713.38742`
4. Gilbert, S., & Lynch, N. (2002). Brewer's conjecture and the feasibility of consistent, available, partition-tolerant web services. *ACM SIGACT News, 33*(2), 51–59. `https://doi.org/10.1145/564585.564601`
5. Kleppmann, M. (2017). *Designing data-intensive applications: The big ideas behind reliable, scalable, and maintainable systems*. O'Reilly Media.
6. Nygard, M. T. (2018). *Release it! Design and deploy production-ready software* (2nd ed.). Pragmatic Bookshelf. *(Circuit breaker ve diğer dayanıklılık desenleri)*

**Resmi Dokümanlar ve Web Kaynakları**

- [Wikipedia - Fallacies of Distributed Computing](https://en.wikipedia.org/wiki/Fallacies_of_distributed_computing) - L. Peter Deutsch'un dağıtık sistem yanılgıları.
- [Wikipedia - CAP Theorem](https://en.wikipedia.org/wiki/CAP_theorem) - CAP teoreminin özeti.
- [Martin Fowler - Circuit Breaker](https://martinfowler.com/bliki/CircuitBreaker.html) - Circuit breaker deseninin açıklaması.
- [Azure Architecture Center - Circuit Breaker Pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/circuit-breaker) - Desenin bulut uygulamalarında kullanımı.
- [microservices.io - Saga Pattern](https://microservices.io/patterns/data/saga.html) - Mikroservislerde saga ile dağıtık işlem yönetimi.
- [Polly](https://www.pollydocs.org/) - .NET için retry, circuit breaker ve timeout gibi dayanıklılık stratejileri sunan kütüphane.

## Kısım II: Yapay Zeka ile Yazılım Geliştirme

### [Bölüm 09: RAG (Retrieval Augmented Generation) Yaklaşımı I](chapter-09.md)

**Kitaplar ve Makaleler**

1. Lewis, P., Perez, E., Piktus, A., Petroni, F., Karpukhin, V., Goyal, N., … Kiela, D. (2020). Retrieval-augmented generation for knowledge-intensive NLP tasks. *Advances in Neural Information Processing Systems, 33*, 9459–9474. arXiv:2005.11401
2. Liu, N. F., Lin, K., Hewitt, J., Paranjape, A., Bevilacqua, M., Petroni, F., & Liang, P. (2024). Lost in the middle: How language models use long contexts. *Transactions of the Association for Computational Linguistics, 12*, 157–173. `https://doi.org/10.1162/tacl_a_00638`
3. Mikolov, T., Chen, K., Corrado, G., & Dean, J. (2013). Efficient estimation of word representations in vector space. arXiv:1301.3781 *(Kelime gömmelerinin, yani embedding fikrinin temel çalışmalarından)*

**Resmi Dokümanlar ve Web Kaynakları**

- [Anthropic - Effective Context Engineering for AI Agents](https://www.anthropic.com/engineering/effective-context-engineering-for-ai-agents) - Bağlam penceresinin verimli yönetimi.
- [Google - EmbeddingGemma Model Card](https://ai.google.dev/gemma/docs/embeddinggemma/model_card) - Bölümde kullanılan embedding modelinin teknik özellikleri.
- [Qdrant Documentation](https://qdrant.tech/documentation/) - Vektör veritabanı ve benzerlik araması.
- [LM Studio Documentation](https://lmstudio.ai/docs) - Dil modellerini yerel ortamda çalıştırma.
- [NVIDIA NeMo Guardrails](https://github.com/NVIDIA-NeMo/Guardrails) - LLM uygulamalarına programlanabilir güvenlik bariyerleri ekleme.

### [Bölüm 10: RAG (Retrieval Augmented Generation) Yaklaşımı II](chapter-10.md)

**Kitaplar ve Makaleler**

1. Singh, A., Ehtesham, A., Kumar, S., & Khoei, T. T. (2025). Agentic retrieval-augmented generation: A survey on agentic RAG. arXiv:2501.09136

**Resmi Dokümanlar ve Web Kaynakları**

- [Qdrant Python Client](https://python-client.qdrant.tech/) - Örnekte kullanılan `QdrantClient` kütüphanesinin dokümantasyonu.
- [LM Studio Documentation](https://lmstudio.ai/docs) - Yerel modelin OpenAI uyumlu API ile kullanılması.

### [Bölüm 11: RAG'ın Ötesi: Graph RAG, Knowledge Graph, CAG ve KAG](chapter-11.md)

**Kitaplar ve Makaleler**

1. Chan, B. J., Chen, C.-T., Cheng, J.-H., & Huang, H.-H. (2025). Don't do RAG: When cache-augmented generation is all you need for knowledge tasks. *Companion Proceedings of the ACM Web Conference 2025 (WWW '25)*. `https://doi.org/10.1145/3701716.3715490` (arXiv:2412.15605)
2. Edge, D., Trinh, H., Cheng, N., Bradley, J., Chao, A., Mody, A., Truitt, S., … Larson, J. (2024). From local to global: A graph RAG approach to query-focused summarization. arXiv:2404.16130
3. Hogan, A., Blomqvist, E., Cochez, M., d'Amato, C., de Melo, G., Gutierrez, C., … Zimmermann, A. (2021). Knowledge graphs. *ACM Computing Surveys, 54*(4), 1–37. `https://doi.org/10.1145/3447772`
4. Liang, L., Sun, M., Gui, Z., Zhu, Z., Jiang, Z., Zhong, L., … Zhou, J. (2024). KAG: Boosting LLMs in professional domains via knowledge augmented generation (v3). arXiv:2409.13731. `https://doi.org/10.48550/arXiv.2409.13731`
5. Tang, Y., & Yang, Y. (2024). MultiHop-RAG: Benchmarking retrieval-augmented generation for multi-hop queries. arXiv:2401.15391

> Bölümde ayrıca Bölüm 09'daki Lewis vd. (2020) ve Liu vd. (2024) çalışmalarına da atıf yapılmaktadır.

**Resmi Dokümanlar ve Web Kaynakları**

- [Microsoft GraphRAG](https://microsoft.github.io/graphrag/) - Microsoft'un Graph RAG uygulaması ve dokümantasyonu.
- [OpenSPG KAG](https://github.com/openspg/kag) - KAG yaklaşımının açık kaynak uygulaması.

### [Bölüm 12: MCP (Model Context Protocol) Kavramı ve MCP Server Yazılması](chapter-12.md)

**Kitaplar ve Makaleler**

1. Fielding, R. T. (2000). *Architectural styles and the design of network-based software architectures* [Doktora tezi, University of California, Irvine]. *(REST mimari stilinin tanımlandığı tez)*

**Resmi Dokümanlar ve Web Kaynakları**

- [Anthropic - Introducing the Model Context Protocol](https://www.anthropic.com/news/model-context-protocol) - MCP'nin duyurulduğu yazı *(Kasım 2024)*.
- [Model Context Protocol Specification](https://modelcontextprotocol.io/specification/latest) - Protokolün güncel resmi spesifikasyonu.
- [MCP Specification - Transports](https://modelcontextprotocol.io/specification/latest/basic/transports) - Stdio ve Streamable HTTP taşıma katmanları.
- [WHATWG HTML Standard - Server-Sent Events](https://html.spec.whatwg.org/multipage/server-sent-events.html) - SSE'nin standart tanımı.
- [UTCP - SSE](https://www.utcp.io/protocols/sse) ve [UTCP - Streamable HTTP](https://www.utcp.io/protocols/streamable-http) - Bölümde atıf yapılan protokol açıklamaları.
- [Burak Selim Şenyurt - Microsoft .NET Platformunda Bir MCP Server Yazmak](https://buraksenyurt.github.io/2026/03/07/microsoft-dotnet-platformunda-bir-mcp-server-yazmak/) - .NET ile MCP server geliştirme örneği.

### [Bölüm 13: MCP Server'lar ile Çalışmak](chapter-13.md)

**Resmi Dokümanlar ve Web Kaynakları**

- [FastMCP](https://gofastmcp.com/) - Python ile MCP server ve client geliştirme framework'ü.
- [MCP Python SDK](https://github.com/modelcontextprotocol/python-sdk) - Resmi Python SDK'sı.
- [VS Code - Use MCP Servers](https://code.visualstudio.com/docs/copilot/customization/mcp-servers) - `mcp.json` ile MCP server'ların VS Code'a eklenmesi.
- [Express](https://expressjs.com/) - Örnekteki Node.js API servisinin kullandığı web framework'ü.
- [Grafana Loki - HTTP API](https://grafana.com/docs/loki/latest/reference/loki-http-api/) - log-mcp-lab örneğinin logları sorguladığı API.

### [Bölüm 14: Custom Agent, Skill ve Plugin Yapıları ile Çalışmak](chapter-14.md)

**Kitaplar ve Makaleler**

1. Sumers, T. R., Yao, S., Narasimhan, K., & Griffiths, T. L. (2023). Cognitive architectures for language agents. arXiv:2309.02427 *(Ajanlarda semantic, episodic ve procedural bellek ayrımı)*
2. Tulving, E. (1972). Episodic and semantic memory. In E. Tulving & W. Donaldson (Eds.), *Organization of memory* (pp. 381–403). Academic Press.

**Resmi Dokümanlar ve Web Kaynakları**

- [Anthropic - Equipping Agents for the Real World with Agent Skills](https://www.anthropic.com/engineering/equipping-agents-for-the-real-world-with-agent-skills) - Skill kavramı ve progressive disclosure yaklaşımı.
- [Agent Skills](https://agentskills.io/home) - Açık Agent Skills standardı.
- [anthropics/skills](https://github.com/anthropics/skills) - Örnek skill'lerin yer aldığı resmi repo *(bölümde atıf yapılan `mcp-builder` dahil)*.
- [Claude Code - Subagents](https://code.claude.com/docs/en/sub-agents) - Claude Code'da özel ajan tanımları.
- [Claude Code - Skills](https://code.claude.com/docs/en/skills) - Claude Code'da skill kullanımı.
- [Claude Code - Plugins](https://code.claude.com/docs/en/plugins) - Skill, agent ve hook'ların plugin olarak paketlenmesi.
- [VS Code - Custom Agents](https://code.visualstudio.com/docs/copilot/customization/custom-agents) - GitHub Copilot için özel ajan tanımları.

### [Bölüm 15: Döngü Mühendisliği (Loop Engineering)](chapter-15.md)

**Kitaplar ve Makaleler**

1. Beck, K. (2002). *Test-driven development: By example*. Addison-Wesley. *(Döngülerde kabul kriteri olarak testlerin kullanılması)*

**Resmi Dokümanlar ve Web Kaynakları**

- [IBM Think - Loop Engineering](https://www.ibm.com/think/topics/loop-engineering) - Loop engineering kavramının tanımı.
- [Anthropic - Building Effective Agents](https://www.anthropic.com/engineering/building-effective-agents) - Evaluator-optimizer döngüsü ve ajan desenleri.
- [Claude Code - Hooks Guide](https://code.claude.com/docs/en/hooks-guide) - Ajan yaşam döngüsüne kalite kapısı gibi kontroller eklemek.
- [Claude Code - Headless Mode](https://code.claude.com/docs/en/headless) - Claude Code'un etkileşimsiz *(headless)* çalıştırılması.
- [anthropics/claude-code-action](https://github.com/anthropics/claude-code-action) - GitHub Actions üzerinden zamanlanmış veya tetiklenmiş ajan çalıştırma.
- [SonarQube Community Build Documentation](https://docs.sonarsource.com/sonarqube-community-build/) - Örnekte döngünün hedef metriklerini sağlayan kod analizi aracı.

### [Bölüm 16: Token Kullanımlarını Open Telemetry ve Aspire Dashboard ile İzlemek](chapter-16.md)

**Resmi Dokümanlar ve Web Kaynakları**

- [OpenTelemetry - Semantic Conventions for Generative AI](https://opentelemetry.io/docs/specs/semconv/gen-ai/) - LLM çağrıları ve token kullanımı için standart span ve metrik isimleri.
- [OpenTelemetry - OTLP Specification](https://opentelemetry.io/docs/specs/otlp/) - Telemetri verisinin gRPC/HTTP ile iletildiği protokol.
- [Microsoft Learn - Standalone Aspire Dashboard](https://learn.microsoft.com/dotnet/aspire/fundamentals/dashboard/standalone) - Aspire Dashboard'un bağımsız olarak Docker ile çalıştırılması.
- [VS Code - Monitor Agent Usage with OpenTelemetry](https://code.visualstudio.com/docs/copilot/guides/monitoring-agents) - VS Code Copilot için telemetri ayarları.
- [GitHub Docs - OpenTelemetry for Copilot Agents](https://docs.github.com/en/copilot/concepts/agents/opentelemetry) - Copilot CLI ve ajanları için OpenTelemetry desteği.

### [Bölüm 17: LoRA *(Low-Rank Adaptation)* ile Model Özelleştirme](chapter-17.md)

**Kitaplar ve Makaleler**

1. Dettmers, T., Pagnoni, A., Holtzman, A., & Zettlemoyer, L. (2023). QLoRA: Efficient finetuning of quantized LLMs. *Advances in Neural Information Processing Systems, 36*. arXiv:2305.14314
2. Hu, E. J., Shen, Y., Wallis, P., Allen-Zhu, Z., Li, Y., Wang, S., Wang, L., & Chen, W. (2022). LoRA: Low-rank adaptation of large language models. *International Conference on Learning Representations (ICLR 2022)*. arXiv:2106.09685

**Resmi Dokümanlar ve Web Kaynakları**

- [Unsloth - Fine-tuning LLMs Guide](https://unsloth.ai/docs/get-started/fine-tuning-llms-guide) - Örnekte kullanılan Unsloth ile LoRA eğitimi.
- [Hugging Face PEFT](https://huggingface.co/docs/peft) - Parametre verimli ince ayar *(PEFT)* yöntemleri ve LoRA uygulaması.
- [Hugging Face - GGUF](https://huggingface.co/docs/hub/gguf) - GGUF model formatı.
- [llama.cpp](https://github.com/ggml-org/llama.cpp) - GGUF modellerini çalıştırma ve dönüştürme araçları.
- [RunPod Documentation](https://docs.runpod.io/) - GPU pod'larının kiralanması ve kullanımı.
- [PyTorch - Get Started](https://pytorch.org/get-started/locally/) - CUDA sürümüne uygun PyTorch kurulumu.
- [NVIDIA CUDA Toolkit Release Notes](https://docs.nvidia.com/cuda/cuda-toolkit-release-notes/index.html) - CUDA ve sürücü uyumluluk tablosu.

### [Bölüm 18: Yapay Zeka Destekli Yazılım Geliştirmede Güvenlik](chapter-18.md)

**Kitaplar ve Makaleler**

1. Greshake, K., Abdelnabi, S., Mishra, S., Endres, C., Holz, T., & Fritz, M. (2023). Not what you've signed up for: Compromising real-world LLM-integrated applications with indirect prompt injection. *Proceedings of the 16th ACM Workshop on Artificial Intelligence and Security (AISec '23)*, 79–90. `https://doi.org/10.1145/3605764.3623985`
2. National Institute of Standards and Technology. (2023). *Artificial intelligence risk management framework (AI RMF 1.0)* (NIST AI 100-1). `https://doi.org/10.6028/NIST.AI.100-1`

**Resmi Dokümanlar ve Web Kaynakları**

- [OWASP Top 10 for LLM Applications](https://genai.owasp.org/llm-top-10/) - Prompt injection başta olmak üzere LLM uygulamalarındaki en kritik riskler.
- [MITRE ATLAS](https://atlas.mitre.org/) - Yapay zeka sistemlerine yönelik saldırı taktik ve teknikleri bilgi tabanı.
- [Invariant Labs - MCP Security Notification: Tool Poisoning Attacks](https://invariantlabs.ai/blog/mcp-security-notification-tool-poisoning-attacks) - Tool poisoning saldırısını ilk tanımlayan yazı.
- [MCP Specification - Security Best Practices](https://modelcontextprotocol.io/specification/latest/basic/security_best_practices) - MCP uygulamaları için güvenlik önerileri.
- [Burak Selim Şenyurt - AI Sandbox](https://buraksenyurt.github.io/2026/04/26/ai-sandbox/) - Bölümdeki sandbox PoC'sinin referans makalesi.

---

[← İçindekiler](README.md#top)
