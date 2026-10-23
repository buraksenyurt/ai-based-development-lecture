# Log MCP Lab - Faz 1

Bu laboratuvar, anomali tespiti ve eşik tabanlı alarm deneyleri için OpenTelemetry uyumlu sentetik log verisi üretir. Aynı veri seti iki ayrı, Docker Compose tabanlı log platformuna yüklenebilir:

- **Loki + Grafana + Alloy:** Varsayılan ve log odaklı seçenek.
- **Elasticsearch + Kibana + Filebeat:** Alan bazlı keşif ve ileri analiz alternatifi.

Platform seçimi için [karşılaştırma belgesini](docs/platform-comparison.md) kullanın. Şema ayrıntıları [log sözleşmesinde](docs/log-schema.md) bulunur.

Loki kayıtlarını bir yapay zeka ajanının analiz etmesi için .NET stdio MCP sunucusu ve istemci yapılandırması [MCP sunucusu belgesinde](docs/mcp-server.md) açıklanır. VS Code (`.vscode/mcp.json`), Claude Code ve GitHub Copilot CLI (`.mcp.json`) ile OpenCode (`opencode.json`) için makineden bağımsız yapılandırma dosyaları bu klasörde hazırdır; CLI araçlarını `apps\log-mcp-lab` klasöründen başlatmanız yeterlidir.

## Ön koşullar

- Python 3.10 veya üstü
- Docker Desktop ve Docker Compose v2

## 1. Sentetik veriyi üretin

`apps\log-mcp-lab` dizininden:

```powershell
python .\scripts\generate_logs.py
```

Komut, sabit `42` tohumu ile `data\generated\lab-logs.jsonl` dosyasına 152 olay yazar. Aynı veriyi yeniden üretmek için:

```powershell
python .\scripts\generate_logs.py --seed 42 --baseline-events 120
```

Beklenen senaryo dağılımı `data\scenario-manifest.json` içindedir.

## 2. Loki seçeneğini çalıştırın

```powershell
docker compose -f .\docker-compose.loki.yml up -d
```

- Grafana: <http://localhost:3001>
- Loki API: <http://localhost:3101>
- Alertmanager: <http://localhost:9094>

Grafana laboratuvar kolaylığı için anonim yönetici erişimiyle gelir. Bu yapı yalnızca yerel deney ortamı içindir.

Örnek LogQL sorguları:

```logql
{job="synthetic-lab"} | json | severity_text="ERROR"
```

```logql
{job="synthetic-lab"} | json | attributes_duration_ms > 500
```

`ErrorBurstDetected`, beş dakikada hizmet başına altıdan fazla hata olduğunda; `SustainedLatencyDetected`, beş dakikada hizmet başına altıdan fazla 500 ms üstü kayıt olduğunda tetiklenir. Kurallar `config\loki\rules\faz1-alerts.yaml` içindedir.

## 3. Elasticsearch seçeneğini çalıştırın

```powershell
docker compose -f .\docker-compose.elasticsearch.yml up -d
```

- Kibana: <http://localhost:5602>
- Elasticsearch API: <http://localhost:9201>

Kibana'da `lab-logs-*` veri görünümünü oluşturun ve zaman alanı olarak `timestamp` seçin. Aşağıdaki KQL filtreleri alarm deneyleri için başlangıç noktasıdır:

```text
severity_text: "ERROR"
```

```text
attributes.duration_ms > 500
```

Kibana kuralı için son beş dakikada `service_name` başına `severity_text: "ERROR"` sayısını `> 5`; gecikme alarmı için `attributes.duration_ms > 500` sayısını `> 5` olarak yapılandırın.

## Doğrulama

Loki:

```powershell
Invoke-RestMethod http://localhost:3101/ready
Invoke-RestMethod http://localhost:9094/api/v2/alerts
```

Elasticsearch:

```powershell
Invoke-RestMethod http://localhost:9201/_cluster/health
Invoke-RestMethod http://localhost:9201/lab-logs-*/_count
```

Yığınları kapatmak veri volume'lerini korur:

```powershell
docker compose -f .\docker-compose.loki.yml down
docker compose -f .\docker-compose.elasticsearch.yml down
```

Kalıcı veriyi de silmek için hedef yığına `--volumes` ekleyin.
