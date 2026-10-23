# Faz 1 Log Sözleşmesi

Her satır tek bir JSON nesnesidir (JSON Lines). Zamanlar UTC ve RFC 3339 biçimindedir. Sözleşme OpenTelemetry Logs veri modelinin alanlarını ve adlandırmasını temel alır.

## Zorunlu alanlar

| Alan | Açıklama |
|---|---|
| `timestamp` | Olayın UTC zamanı (`YYYY-MM-DDTHH:mm:ss.SSSZ`) |
| `observed_timestamp` | Toplayıcının olayı gözlediği UTC zamanı |
| `severity_number` | OpenTelemetry önem numarası |
| `severity_text` | `DEBUG`, `INFO`, `WARN`, `ERROR` veya `FATAL` |
| `body` | İnsan tarafından okunabilir olay açıklaması |
| `event_name` | Nokta ayrımlı, kararlı olay adı |
| `service_name` | Düşük kardinaliteli hizmet adı |
| `service_version` | Hizmet sürümü |
| `deployment_environment` | Ortam adı |
| `trace_id` | 32 karakterlik hex korelasyon kimliği |
| `span_id` | 16 karakterlik hex span kimliği |
| `resource` | OpenTelemetry kaynak öznitelikleri |
| `attributes` | Olay öznitelikleri |

`resource` içinde `service.name`, `service.version` ve `deployment.environment` bulunur. Kök alanlar, Loki ve Elasticsearch yükleme yapılandırmalarını sade tutmak için bu düşük kardinaliteli değerleri tekrar eder.

## İndeksleme ve etiketleme

Yalnızca `service_name`, `deployment_environment` ve `severity_text` etiket/indeks filtresi için uygundur. `trace_id`, `span_id`, hata iletisi, kullanıcı kimliği veya istek yolu gibi yüksek kardinaliteli değerler yalnızca JSON gövdesinde tutulur; Loki etiketi yapılmaz.

## Senaryo etiketleri

`attributes.scenario` alanı olayın beklenen sınıfını taşır:

- `baseline`: normal davranış
- `latency_degradation`: artan gecikme
- `error_burst`: hata patlaması
- `dependency_failure`: bağımlı hizmet arızası
- `recovery`: hata sonrası toparlanma

Bu alan deney doğrulamasına yöneliktir. Üretim alarmları yalnızca bu etikete dayanmaz.

## Veri koruma

Sentetik veri kimlik bilgisi veya kişisel veri içermez. Gerçek veriye sözleşme uygulanırken kimlikler maskelenmeli; token, parola ve oturum çerezi kesinlikle loglanmamalıdır.
