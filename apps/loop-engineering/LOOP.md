# Hedef
`src/LoopLab.Application/Orders` ve `src/LoopLab.Domain/Orders` klasörlerinden oluşan Orders modülünde satır kapsamını (line coverage) %70'e çıkar.

# Ölçüm
- Kapsam değeri `scripts/sonar-scan.ps1` çalıştırıldıktan sonra SonarQube'daki **Coverage** metriğinden okunur.
- Hızlı yerel kontrol için: `dotnet test --collect:"XPlat Code Coverage"`.

# Her turda
- Kapsamı en düşük `TEK` sınıfı seç.
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
