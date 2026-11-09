<a id="top"></a>

# Bölüm 14: Custom Agent, Skill ve Plugin Yapıları ile Çalışmak

<!-- nav:top:start -->
[← Bölüm 13](chapter-13.md#top) · [İçindekiler](README.md#top) · [Bölüm 15 →](chapter-15.md#top)
<!-- nav:top:end -->

Yapay zeka destekli yazılım geliştirme süreçlerinde kullandığımız birçok teknik ve araç vardır. Sadece prompt girerek ilerlemek, yüksek kapasiteleri context pencerelerine sahip modellerle çalışırken yeterli olabilir. Ancak daha karmaşık senaryolarda, yapay zeka asistanlarının yeteneklerini genişletmek ve bağlamı zenginleştirmek için farklı araçlara ve tekniklere ihtiyaç duyulur. **MCP *(Model Context Protocol)*** sunucularından yararlanmak, **RAG *(Retrieval Augmented Generation)*** tabanlı geliştirme hatları tesis etmek, **Fine-Tuning** ile domain özel eğitilmiş modelleri kullanmak bunlardan bazılarıdır. Diğer yandan kendi özelleştirdiğimiz ajanları *(Custom Agents)* ve yetenek setlerini *(Skills)* geliştirmek de mümkündür ve giderek daha çok senaryoda tercih edilmektedir.

Yapay zeka ajanları ile insan belleğinin çalışma şekli arasında da yakın ilişkiler kurmak mümkündür. Burada insan belleğinin üç farklı katmanı ele alınabilir;

- **Semantic Memory:** Türkiye'nin başkenti Ankara'dır bilgisine sahip olmak gibi genel kültür bilgileri bu katmanda yer alır. AI ajanları açısından bakıldığında bu katman **RAG** tekniğiyle desteklenebilir. Zira Türkiye, başkent, Ankara gibi kavramların birbirleriyle ilişkisi vektörel olarak ifade edilebilir ve bu sayede modelin muhakeme sürecine dahil edilebilir.
- **Episodic Memory:** Geçmişte yaşanmış olaylara dair anıların tutulduğu katmandır. Örneğin, dün akşam ne yediğimiz, geçen hafta sinemaya gidip gitmediğimiz veya geçen yaz Ankara'da olduğumuz gibi bilgiler bu katmanda saklanır. AI ajanları açısından bakıldığında bu katman bir çeşit log bilgisidir ve bugünkü modellerin sahip olduğu büyük bağlam pencereleri sayesinde bu tür bilgilerin tutulması ve muhakeme sürecine dahil edilmesi mümkün olmuştur *(Bu ders içeriğinin hazırlandığı tarih itibariyle örneğin GPT 5.4 modeli aynı session içerisindeki tek bir konuşmada 400 bin token'a kadar olan bilgiyi hatırlayabilmektedir. Bu değer Claude Sonnet 4.6 için 200 bin token civarındadır)*.
- **Procedural Memory:** Nasıl bisiklete binileceği, araba sürüleceği gibi becerilerin saklandığı katmandır. AI ajanları açısından bakıldığında bu katman **Custom Agents** veya **Skills** gibi yapılarla desteklenebilir.

## Custom Agents

Genel dil modelleri ve bunları kullanan ajanlar *(Claude Sonnet, GTP Codex vb)* çok geniş çerçevede bilgiye sahiptir. Ancak bazı senaryolarda projelerin çalıştığı domain bilgisine uyan özelleştirilmiş ajanlara da ihtiyacımız olur. Örneğin çalıştığımız kurumun kimliğini ifade edebilen CSS *(Cascading Style Sheets)* stilleri konusunda uzmanlaşmış bir ajan, genel bir asistanın verebileceği cevaplara kıyasla çok daha tutarlı ve hedefe yönelik çıktılar üretebilir. **Custom Agent**'lar, genel amaçlı yapay zeka modellerini belirli bir amaca, alana *(domain)* veya göreve odaklanacak şekilde özelleştirdiğimiz yapılandırmalardır. Genel bir asistan her konuda ortalama ve geniş kapsamlı cevaplar verirken, Custom Agent'lar tanımlanmış kesin kurallar çerçevesinde, kendisine atanmış uzmanlık rolüne *(persona)* sadık kalarak çok daha tutarlı ve hedefe yönelik çıktılar üretebilir.

Özelleştirilmiş ajanlar markdown formatında dosyalar olarak tanımlanırlar. Günümüzdeki AI destekli yazılım geliştirme araçları ve dil modelleri bu dosyalardaki standartlaştırışmış şema yapısına bakarak ajanların ne tür görevleri yerine getirebileceğini, hangi araçlara erişebileceğini, hangi kurallar çerçevesinde hareket edeceğini anlayabilirler.

Bu markdown şeması **frontmatter** ve **body** olmak üzere iki ana bölümden oluşur. Frontmatter kısmı **YAML** formatında yazılır ve ajanların kullanabileceği bazı meta bilgileri içerir. Genellikle ajanının adı *(name)*, açıklaması *(description)*, hangi araçlara erişebileceği *(tools)* gibi keşif amaçlı kullanılan ek bilgileri barındırır. Body kısmı ise ajanın kendisiyle ilgili detaylı bilgiler içerir *(Ajanın ne işe yaradığı, nasıl kullanılacağı, hangi kaynaklara sahip olduğu vb)*

Aşağıda örnek bir ajan içeriği yer almaktadır.

```text
---
name: [Ajanın adı]
description: [Ajanın ne işe yaradığına dair kısa bir açıklama]
tools: [Ajanın erişebileceği ve izin verilen araçların listesi]
  - tool1
  - tool2
  - tool3
---
# [Ajanın detaylı açıklaması ve kullanım talimatları, örnekler, standartlar, referanslar vb]
```

Özelleştirilmiş ajanlar genellikle aşağıdaki durumlarda değerlendirilir;

- **Tekrarlayan Karmaşık Görevler:** Her seferinde aynı uzun talimatları *(prompts)* yazmak yerine, kuralların bir kez tanımlanarak sürecin otomatize edilebileceği durumlarda.
- **Domain Uzmanlığı:** Sadece veritabanı optimizasyonuna, ağ güvenliğine veya belirli bir programlama dilinin en iyi pratiklerine odaklanan bir uzman görüşüne ihtiyaç duyulduğunda.
- **Standartların Korunması:** Kurum içerisinde üretilen kodların belirli standartlara *(örneğin Clean Architecture, SOLID, Design Patterns)* veya proje şablonlarına uymasını garanti etmek istediğimiz hallerde.
- **Bağlamın *(Context)* Daraltılması:** Modelin konu dışına çıkmasını engellemek, halüsinasyon *(hallucination)* riskini azaltmak ve sınırlandırılmış bir çerçevede *(Guardrails)* çalışmasını sağlamak istediğimizde.

Örnek senaryolar;

- **Code Reviewer Agent:** Projeye bir kod eklendiğinde devreye giren ve kodun kalitesini, standartlara uygunluğunu ve potansiyel hataları değerlendiren ajan.
- **Test Engineer Agent:** Uygulamanın kaynak kodunu inceleyerek *edge case*'leri tespit eden, eksik senaryoları belirleyen ve istenilen test kütüphanesine uygun birim *(unit)* testleri yazan kalite odaklı ajan.
- **Database Architect Agent:** Domain modeline ve beklenen trafik yüküne göre en uygun veritabanı şemasını *(SQL/NoSQL)* tasarlayan, indeksleme stratejileri öneren ve sorgu performanslarını değerlendiren ajan.
- **UX Developer Agent:** Kullanıcı senaryolarına bakarak uygulamaya HTML sayfaları ekleyen ve bu sayfaların kullanıcı deneyimi açısından tutarlı, erişilebilir ve estetik olmasını sağlayan ajan.

Özelleştirilmiş ajanlar bir sonraki alt başlıkta yer alan SKILL yapılarıyla da desteklenebilirler. Dolayısıyla bağlamı daraltırken hem özelleştirilmiş bir ajanı hem de onun sahip olabileceği yetenek setlerini kullanabiliriz.

## Skills

Günümüzde büyük dil modelleri ile çalışırken bağlamı zenginleştirmek ve modelin yeteneklerini genişletmek için çeşitli araçlar ve teknikler kullanılır. Bir süredir gündemde olan araçlardan birisi de **Skill**'lerdir. **Skill**'ler, yapay zeka ajanlarına yeni yetenekler ve uzmanlıklar kazandırmak için çeşitli talimatları ve kaynakları *(resources)* içeren birer modül olarak tanımlanabilir. **Anthropic** tarafından geliştirilen ve [açık kaynak olarak sunulan](https://agentskills.io/home) bu özellik tüm dil modelleri tarafından desteklenen bir standart olarak da değerlendirilmektedir.

Bir **skill** aslında en az bir **markdown** dosyası ve varsa yardımcı kaynaklardan *(çalıştırılabilir kod parçaları, şablonlar, referans dokümlanlar vb)* oluşan bir modül olarak düşünülebilir. Aşağıda örnek bir **skill** yapısı görülmektedir.

```text
mcp-builder/
├── SKILL.md
├── scripts/
    ├── connections.py
    ├── evaluation.py
    ├── requirements.txt
    └── examples.json
├── references/
    ├── evaluation.md
    ├── best_practices.md
    └── architecture.md
```

**SKILL.md** dosyası iki parçadan oluştur. **Frontmatter** ve **body** kısımları. **Frontmatter** kısmı **yaml** formatında yazılır ve ajanların kullanabileceği bazı meta bilgileri içerir. Genellikle skill'in adı *(name)* ve açıklaması *(description)* gibi temel bilgileri içerir. **Body** kısmı ise skill'in kendisiyle ilgili detaylı bilgileri içerir. Bu bölümde skill'in ne işe yaradığı, nasıl kullanılacağı, hangi kaynaklara sahip olduğu gibi bilgiler yer alır. Örnek olması açısından yine Anthropic tarafından sağlanan [bu github hesabındaki örnek modül](https://github.com/anthropics/skills/tree/main/skills/mcp-builder) incelenebilir.

```text
---
name: MCP Builder
description: A skill to help build MCP servers.
---
# Development Guidelines
## Overview
...
...

# Reference Files
## Documentation Library
...
## Example Codes
...
```

Bir **Skill** aslında **Progressive Disclosure** tekniğiyle çalışır. Yani, skill'in içeriği, ajan tarafından ihtiyaç duyuldukça açığa çıkarılır. Ajan, bir görevi yerine getirirken belirli bir yeteneğe ihtiyaç duyduğunda, ilgili skill'in içeriğini değerlendirir ve bu içeriği kullanarak ilgili görevi yerine getirir. Bu süreç üç aşamalıdır. Birinci aşamada dokümanını **frontmatter** bilgisi devreye girir. Ajan bu bilgilere göre hangi skill'in hangi görevlere uygun olduğunu değerlendirir. İkinci aşamada ise ajan, görevi yerine getirmek için gerekli olan bilgileri **body** kısmından açığa çıkarır. Üçüncü aşamada ise eğer varsa referans kaynaklara erişilir ve görevin yerine getirilmesi için bu kaynaklardan da yararlanılır.

**Skill**'ler oldukça güçlü araçlardır. Tanımları gereği dosya sistemine erişebilir, kod işletebilirler. Bu nedenle güvenlik açısından dikkatli bir şekilde tasarlanmaları gerekir. Dikkat edilmediği takdirde **Prompt Injection**, **Tool Poisoning**, **Malware Injection** gibi saldırılara maruz kalınabilir. Bu tür saldırıları önlemek için skill'lerin erişebileceği kaynakları sınırlamak, açığa çıkarılan bilgileri dikkatli bir şekilde kontrol etmek ve güvenlik duvarları *(Guardrails)* gibi ek önlemler almak önemlidir.

Buraya kadar tanımladığımız birçok kavram var. Bunlar zaman zaman birbirlerine karıştırılabilir ve hangisi ne zaman kullanılmalı sorusu açıkta kalabilir. Aşağıdaki kısım bu kavramların bize sağladığı imkanları ve birbirleriyle olan ilişkilerini özetlemektedir.

| **Kavram** | **Tanım** | **Ne sağlar?** |
| ---------- | --------- | --------- |
| **MCP (Model Context Protocol)** | Yapay zeka asistanlarının harici araçlara standart bir yolla erişmesini sağlayan bir protokol. | Bir yapay zeka ajanının hangi araçlara erişebileceğini ve bu araçları nasıl kullanabileceğini belirler. |
| **RAG (Retrieval Augmented Generation)** | Yapay zeka modellerinin bilgi deposundan çekilen parçaları muhakeme sürecine dahil ederek daha kaliteli çıktılar üretmesini sağlayan yaklaşım. | Yapay zeka ajanına bir şeyin nasıl yapılacağını öğretmez, sadece mevcut bilgileri referans almasını sağlar. |
| **Fine-Tuning** | Bir dil modelinin belirli bir domain veya görev için özel olarak eğitilmesi süreci. *(Pahalıdır, model değişirse yeniden üretim gerektirir)* | Modelin belirli bir alanda daha iyi performans göstermesini sağlar. |
| **Skill** | Yapay zeka ajanlarına yeni yetenekler ve uzmanlıklar kazandırmak için çeşitli talimatları ve kaynakları içeren modül. | Bir şeylerin nasıl, hangi sırada ve değerlendirmeye yapılması gerektiğini belirler. |
| **Custom Agent** | Belirli bir hedefi gerçekleştirmek üzere araçlar, hafıza (RAG) ve yeteneklerle (Skills) donatılmış, planlama yapabilen özelleştirilmiş yapay zeka birimi. | Orkestra şefidir. Sadece cevap üretmekle kalmaz; otonom kararlar alarak çok adımlı karmaşık görevleri uçtan uca yönetir ve aksiyon alır. |

Buna göre yukarıdaki teknikler arasında aşağıdaki ilişkiler de kurulabilir;

- **RAG:** Ajan neleri biliyor? *(Hafıza, bilgi deposu)*
- **MCP:** Ajan dış dünyayla nasıl konuşuyor? *(Araçlara erişim, iletişim protokolü)*
- **Skill:** Ajan söz konusu araçları hangi yöntemle kullanıyor. *(Uzmanlıklar, talimatnameler)*
- **Custom Agent:** Tüm bu kaynakları okuyan, araçları kullana ve görevi gerektiğinde inisiyatif alarak tamamlayabilen karar verici. *(Orkestrasyon, planlama)*


## Claude Plugin Kullanımı

Birden fazla skill veya agent'ı, hook mekanizmalarını da işin içerisine katıp paketlemek mümkündür. Bu paketler plugin olarak adlandırılırlar. Claude tarafında plugin'lerin nasıl yazılacağına dair dokümantasyona [buradan](apps/IdeasPool/docs/plugin-rehberi.md) ulaşabilirsiniz.

```bash
# Claude session'ının ilgili plugin ile başlatılması
claude --plugin-dir ./plugins/ideas
```

Bu örnekteki plugin basitçe bir hello world çalışması olarak düşünülebilir. Fikir eklenmesi, fikirlerin listelenmesi ve fikrin değerlendirilip fizibilitesinin yapılması gibi işlemleri içeren skill, agent ve hook mekanizmalarını içermektedir. Çalışma zamanına ait eğlenceli çıktılara gelirsek.

Yeni bir fikir eklenmesi ve listelenmesi; *(add ve list skill'leri çalışır)*

```text
/ideas:add "Yapay Zeka Ders Notları Wiki Sayfası. Yapay zeka dersindeki dokümanlar ve notlarad github wiki sayfalarının hazırlanması için bir çalışma yapmayı planlıyorum."
```

![Claude plugin 00](./images/claude_plugin_00.png)

Oldukça iddialı bir fikrin eklenmesi; *(add skill'i çalışır)*

```text
/ideas:add "Mars görevi. Yürüyerek mars gezegenine gitmeyi planlıyorum."
```

![Claude plugin 01](./images/claude_plugin_01.png)

Fikrin değerlendirilmesi ve fizibilitesinin yapılması süreci :D *(idea-reviewer ajanı devreye girer)*

```text
ideas:idea-reviewer #2 numaralı fikrimi değerlendirebilir misin?
```

![Claude plugin 02](./images/claude_plugin_02.png)

Tüm fikirlerin değerlendirilmesi *(idea-reviewer ajanı devreye girer)*

```text
ideas:idea-reviewer Bütün fikirleri değerlendir.
```

![Claude plugin 03](./images/claude_plugin_03.png)

<!-- nav:bottom:start -->
---

| ← Önceki Bölüm | İçindekiler | Sonraki Bölüm → |
| :--- | :---: | ---: |
| [Bölüm 13: MCP Server'lar ile Çalışmak](chapter-13.md#top) | [İçindekiler](README.md#top) | [Bölüm 15: Döngü Mühendisliği (Loop Engineering)](chapter-15.md#top) |
<!-- nav:bottom:end -->
