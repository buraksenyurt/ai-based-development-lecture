# 03 - Dağıtım Algoritması

## Uyum puanı (`PreferenceMatchScorer`)

Her (katılımcı, proje) çifti için 0 ile 1 arasında bir puan hesaplanır:

- **Dil puanı:** Katılımcının tercih listesinde projenin kullandığı ilk dil kaçıncı sıradaysa puan `1 / sıra` olur. 1. tercih için 1, 2. tercih için 0.5, 3. tercih için 0.33 alınır. Eşleşme yoksa puan 0'dır.
- **Veritabanı puanı:** Dil puanıyla aynı şekilde hesaplanır. `NoSQL-*` / `SQL-*` gibi joker tercihler o kategorideki tüm veritabanlarıyla eşleşir (`TechnologyCatalog`).
- **Toplam puan:** `0.6 * dil + 0.4 * veritabanı`. Ağırlıklar `appsettings.json` içindeki `Routing` bölümünden değiştirilebilir.

Karşılaştırmalarda büyük/küçük harf ve boşluklar dikkate alınmaz. Yaygın takma adlar da aynı kabul edilir (`csharp` = `C#`, `postgres` = `PostgreSQL`, `mssql` = `Sql Server`).

## Yerleştirme (`GreedyProjectRouter`)

1. **Puan matrisi:** Tüm çiftlerin uyum puanı hesaplanır.
2. **Açılacak projeler:** Projeler toplam ilgiye (puanların toplamına) göre sıralanır. Minimum takım sayılarının toplamı katılımcı sayısını aşmadığı sürece projeler açılır. Kimsenin ilgilenmediği projeler yalnızca kapasite yetmezse açılır.
3. **Minimumları doldurma:** Açık projeler en az kişi sayısına ulaşana kadar en yüksek puanlı (katılımcı, proje) çifti tekrar tekrar seçilir. *(Rule 02)*
4. **Kalanları yerleştirme:** Kalan katılımcılar, en fazla kişi sayısına ulaşmamış en uygun projeye yerleştirilir.
5. **Determinizm:** Eşitliklerde Id sırası kullanılır. Aynı girdi her zaman aynı sonucu verir.

Sonuç kaydedilmeden önce `Competition.AssignSettlement` Rule 02 ve Rule 03'ü tekrar doğrular. Yerleştirilemeyen katılımcılar, açılmayan projeler ve hiçbir tercihiyle eşleşmeyen bir projeye yerleştirilen katılımcılar uyarı olarak raporlanır.

## Bilinen sınırlar

- Greedy yaklaşım toplam memnuniyeti en üst düzeye çıkarmayı garanti etmez. Bir katılımcı, başka birinin minimumu doldurması için ikinci tercihine kayabilir.
- Proje büyüklüğü (`Size`) ve etiketler (`Tags`) şimdilik puana katılmıyor.
