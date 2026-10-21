# Hedef
`src/LoopLab.Application/Orders` ve `src/LoopLab.Domain/Orders` klasörlerinden oluşan Orders modülünde satır kapsamını (line coverage) %70'e çıkar.

# Ölçüm
- Döngünün tek ölçütü `scripts/measure-coverage.ps1` betiğidir. Betik testleri çalıştırır ve son satırda `ORDERS_LINE_COVERAGE=<değer>` yazar.
- Her turun başında ve sonunda betiği çalıştır; `PROGRESS.md`'deki "Kapsam" satırına bu iki değeri yaz. Değeri tahmin etme, başka yöntemle hesaplama.
- "İlerleme" = `ORDERS_LINE_COVERAGE` değerinin bir önceki tura göre artması.
- SonarQube (`scripts/sonar-scan.ps1`) insan kapısıdır; döngü Sonar değerini okumaya çalışmaz.

# Her turda
- Kapsamı en düşük `TEK` sınıfı seç (`measure-coverage.ps1` çıktısındaki tabloya göre).
- Eşitlik varsa üretim koduna dokunmadan deterministik olarak test edilebilen sınıfı seç; seçim gerekçesini `PROGRESS.md`'ye yaz.
- Sadece `tests/` altına test ekle; `src/` altındaki üretim koduna dokunma.
- Testler geçmeli, mevcut hiçbir test kırılmamalı.
- Değişiklik 300 satırı geçmesin.
- Turu bitirmeden önce `test-reviewer` alt ajanından ONAY al.
- Sonucu `PROGRESS.md` dosyasına yeni bir `Tur N` kaydı olarak ekle.

# Dur
- Hedefe ulaşınca, `VEYA`
- Aynı sınıfta 2 tur üst üste ilerleme yoksa, `VEYA`
- Test edilebilmesi için üretim kodu değişmesi gerekiyorsa (bunu `PROGRESS.md`'ye "insan kararı gerekli" olarak yaz), `VEYA`
- 8 tur tamamlandıysa (kaçış kapısı; neyin engel olduğunu raporla).

# Yasaklar
- Assert içermeyen test yazma.
- Testleri `[Fact(Skip = ...)]` ile devre dışı bırakma, mevcut testleri silme.
- Coverage hesabından dosya hariç tutma (`ExcludeFromCodeCoverage`, coverlet ayarları vb.).
