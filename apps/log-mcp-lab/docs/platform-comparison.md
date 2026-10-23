# Loki ve Elasticsearch Karşılaştırması

| Ölçüt | Loki + Grafana + Alloy | Elasticsearch + Kibana + Filebeat |
|---|---|---|
| Faz 1 için kaynak tüketimi | Daha düşük | Daha yüksek; JVM belleği gerekir |
| Log arama modeli | Etiket filtresi ve LogQL | İndekslenmiş alanlar ve KQL/ES|QL |
| Yüksek kardinaliteli alanlar | Etiketlenmemelidir; gövdede aranır | Eşlenip aranabilir, indeks maliyeti artar |
| Alarm başlangıcı | Loki rule dosyaları ve Alertmanager | Kibana rule UI/API |
| Tahminleme verisini dışa aktarma | LogQL API ile | Elasticsearch sorguları ve dışa aktarma ile |
| Operasyonel karmaşıklık | Daha düşük | Daha yüksek |

## Faz 1 kararı

Varsayılan yığın **Loki + Grafana + Alloy** olmalıdır. Faz 1'in ana hedefi log temelli deneyler, hata patlaması ve gecikme eşiği alarmlarıdır; Loki bu kullanımda daha az kaynakla hızlı kurulum sağlar.

Elasticsearch alternatifi, aşağıdaki ihtiyaçlardan biri doğrulanırsa tercih edilmelidir:

- Çok sayıda olay özniteliğinde kalıcı alan bazlı arama gerekir.
- Deney sonuçlarının Elasticsearch ekosistemindeki araçlarla birleştirilmesi gerekir.
- Laboratuvara ayrılabilecek bellek ve disk kapasitesi yeterlidir.

Her iki yığın aynı JSONL sözleşmesini alır. Bu nedenle platform değişimi sentetik veri üreticisini veya deney senaryolarını değiştirmez.
