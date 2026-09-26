-- Project Router: README "Uygulama Önerileri" bölümündeki P0001-P0020 proje fikirleri
-- ve README'ye henüz eklenmemiş ek öneriler P0021-P0025 (Go, Zig, Dart/Flutter, Elixir, Kotlin/Swift, Java/Ruby).
--
-- Kullanım (uygulama bir kez çalıştırılıp şema oluşturulduktan sonra, apps/ProjectRouter altında):
--   sqlite3 projectrouter.db ".read scripts/seed-project-ideas.sql"
--
-- Notlar:
-- * Script tekrar çalıştırılabilir: projeler sabit id'lerle upsert edilir, teknoloji/etiket listeleri yeniden yazılır.
--   Bir turnuvada kullanılan projeler de güncellenebilir (turnuva bağları korunur).
-- * Id'ler küçük harfli Guid'dir; uygulama Guid.ToString() ile aynı biçimi kullanır.
-- * Zorluk -> büyüklük ve takım aralığı (README'de takım büyüklüğü yok, varsayılan seçildi):
--     Kolay -> S, 2-5 kişi | Orta -> M, 3-6 kişi | İleri -> L, 3-7 kişi
--   25 projenin toplam kapasitesi 70-156 kişidir. Dağıtımda açıkta öğrenci kalmaması için katılımcı sayısı
--   en fazla 156 olmalıdır; daha kalabalık bir sınıfta proje eklenmeli veya üst sınırlar artırılmalıdır.
-- * Rule 04 her projede en az bir veritabanı istediği için:
--     README'de veritabanı belirtilmemiş sunucu tarafı projelerde 'SQL-*' (herhangi bir SQL veritabanı),
--     veritabanı gerektirmeyen masaüstü/CLI/oyun projelerinde yerel kayıt (skor, ayar vb.) için 'SQLite' kullanıldı.
-- * '.NET' önerileri dil olarak 'C#', 'Node.js' önerileri 'JavaScript' olarak girildi.
-- * P0021 (Mini Redis) veritabanı olarak 'Redis' taşır: projenin uyumluluk hedefi Redis'tir (redis-cli ile test edilir).
-- * Etiketlerin ilki README'deki proje kodudur (P0001 ...).

PRAGMA foreign_keys = ON;

BEGIN TRANSACTION;

-- P0001 - Not Defteri (Kolay)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('4ae93a95-259b-5e0c-8107-366062bc0b46', 'P0001 - Not Defteri',
        'Not oluşturma, etiketleme, arama ve arşivleme sunan basit bir not uygulaması. Odak: CRUD operasyonları, kullanıcı bazlı yetkilendirme ve temel arama.',
        2, 5, 'S')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = '4ae93a95-259b-5e0c-8107-366062bc0b46';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('4ae93a95-259b-5e0c-8107-366062bc0b46', 'language', 0, 'C#'),
    ('4ae93a95-259b-5e0c-8107-366062bc0b46', 'language', 1, 'JavaScript'),
    ('4ae93a95-259b-5e0c-8107-366062bc0b46', 'platform', 0, 'web'),
    ('4ae93a95-259b-5e0c-8107-366062bc0b46', 'platform', 1, 'api'),
    ('4ae93a95-259b-5e0c-8107-366062bc0b46', 'database', 0, 'PostgreSQL'),
    ('4ae93a95-259b-5e0c-8107-366062bc0b46', 'database', 1, 'SQLite'),
    ('4ae93a95-259b-5e0c-8107-366062bc0b46', 'similar', 0, 'Google Keep'),
    ('4ae93a95-259b-5e0c-8107-366062bc0b46', 'similar', 1, 'Evernote'),
    ('4ae93a95-259b-5e0c-8107-366062bc0b46', 'tag', 0, 'P0001'),
    ('4ae93a95-259b-5e0c-8107-366062bc0b46', 'tag', 1, 'crud'),
    ('4ae93a95-259b-5e0c-8107-366062bc0b46', 'tag', 2, 'arama'),
    ('4ae93a95-259b-5e0c-8107-366062bc0b46', 'tag', 3, 'layered-architecture');

-- P0002 - Kitap Takip (Kolay)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('eb1437e9-b2f8-5960-9cf1-a8d84459d6d4', 'P0002 - Kitap Takip',
        'Okunan/okunacak kitapların listelenip puanlandığı ve yorumlandığı uygulama. İleri seviyede RAG tabanlı kitap önerisi veya özetleme motoru eklenebilir.',
        2, 5, 'S')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = 'eb1437e9-b2f8-5960-9cf1-a8d84459d6d4';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('eb1437e9-b2f8-5960-9cf1-a8d84459d6d4', 'language', 0, 'Python'),
    ('eb1437e9-b2f8-5960-9cf1-a8d84459d6d4', 'language', 1, 'C#'),
    ('eb1437e9-b2f8-5960-9cf1-a8d84459d6d4', 'platform', 0, 'web'),
    ('eb1437e9-b2f8-5960-9cf1-a8d84459d6d4', 'platform', 1, 'api'),
    ('eb1437e9-b2f8-5960-9cf1-a8d84459d6d4', 'database', 0, 'PostgreSQL'),
    ('eb1437e9-b2f8-5960-9cf1-a8d84459d6d4', 'similar', 0, 'Goodreads'),
    ('eb1437e9-b2f8-5960-9cf1-a8d84459d6d4', 'tag', 0, 'P0002'),
    ('eb1437e9-b2f8-5960-9cf1-a8d84459d6d4', 'tag', 1, 'rag'),
    ('eb1437e9-b2f8-5960-9cf1-a8d84459d6d4', 'tag', 2, 'clean-architecture');

-- P0003 - Bilgi Yarışması (Kolay)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('81ac748c-4fa3-5585-b5db-a1ceca807281', 'P0003 - Bilgi Yarışması',
        'Farklı kategori ve zorluklarda soru cevaplayarak puan ve rozet kazanılan yarışma uygulaması. Sorular statik bir veri setinden gelebilir veya dil modeline ürettirilebilir.',
        2, 5, 'S')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = '81ac748c-4fa3-5585-b5db-a1ceca807281';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('81ac748c-4fa3-5585-b5db-a1ceca807281', 'language', 0, 'C#'),
    ('81ac748c-4fa3-5585-b5db-a1ceca807281', 'language', 1, 'TypeScript'),
    ('81ac748c-4fa3-5585-b5db-a1ceca807281', 'platform', 0, 'web'),
    ('81ac748c-4fa3-5585-b5db-a1ceca807281', 'database', 0, 'SQL-*'),
    ('81ac748c-4fa3-5585-b5db-a1ceca807281', 'similar', 0, 'Kahoot'),
    ('81ac748c-4fa3-5585-b5db-a1ceca807281', 'similar', 1, 'Duolingo'),
    ('81ac748c-4fa3-5585-b5db-a1ceca807281', 'tag', 0, 'P0003'),
    ('81ac748c-4fa3-5585-b5db-a1ceca807281', 'tag', 1, 'prompt-engineering'),
    ('81ac748c-4fa3-5585-b5db-a1ceca807281', 'tag', 2, 'oyunlastirma');

-- P0004 - Görev Panosu (Orta)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('8cdb40b4-56e8-51be-ad7e-201a2acd3905', 'P0004 - Görev Panosu',
        'Pano, liste ve kart yapısıyla iş takibi; kartlar sürükle-bırak ile taşınır. Başka kullanıcının taşıdığı kartın anlık görünmesi (gerçek zamanlı güncelleme) temel teknik zorluktur.',
        3, 6, 'M')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = '8cdb40b4-56e8-51be-ad7e-201a2acd3905';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('8cdb40b4-56e8-51be-ad7e-201a2acd3905', 'language', 0, 'C#'),
    ('8cdb40b4-56e8-51be-ad7e-201a2acd3905', 'language', 1, 'JavaScript'),
    ('8cdb40b4-56e8-51be-ad7e-201a2acd3905', 'platform', 0, 'web'),
    ('8cdb40b4-56e8-51be-ad7e-201a2acd3905', 'database', 0, 'SQL-*'),
    ('8cdb40b4-56e8-51be-ad7e-201a2acd3905', 'similar', 0, 'Trello'),
    ('8cdb40b4-56e8-51be-ad7e-201a2acd3905', 'tag', 0, 'P0004'),
    ('8cdb40b4-56e8-51be-ad7e-201a2acd3905', 'tag', 1, 'real-time'),
    ('8cdb40b4-56e8-51be-ad7e-201a2acd3905', 'tag', 2, 'signalr'),
    ('8cdb40b4-56e8-51be-ad7e-201a2acd3905', 'tag', 3, 'socket.io'),
    ('8cdb40b4-56e8-51be-ad7e-201a2acd3905', 'tag', 4, 'clean-architecture');

-- P0005 - Mikroblog (Orta)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('9b247339-6cdb-5bf5-b541-23dcff541104', 'P0005 - Mikroblog',
        'Kısa gönderi paylaşımı, takip ve zaman tüneli (feed) sunan uygulama. Takipçi sayısı arttıkça feed''in nasıl üretileceği (fan-out on write/read) mimari tartışma konusudur.',
        3, 6, 'M')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = '9b247339-6cdb-5bf5-b541-23dcff541104';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('9b247339-6cdb-5bf5-b541-23dcff541104', 'language', 0, 'TypeScript'),
    ('9b247339-6cdb-5bf5-b541-23dcff541104', 'language', 1, 'C#'),
    ('9b247339-6cdb-5bf5-b541-23dcff541104', 'platform', 0, 'web'),
    ('9b247339-6cdb-5bf5-b541-23dcff541104', 'platform', 1, 'api'),
    ('9b247339-6cdb-5bf5-b541-23dcff541104', 'database', 0, 'PostgreSQL'),
    ('9b247339-6cdb-5bf5-b541-23dcff541104', 'similar', 0, 'Twitter'),
    ('9b247339-6cdb-5bf5-b541-23dcff541104', 'similar', 1, 'X'),
    ('9b247339-6cdb-5bf5-b541-23dcff541104', 'tag', 0, 'P0005'),
    ('9b247339-6cdb-5bf5-b541-23dcff541104', 'tag', 1, 'event-driven'),
    ('9b247339-6cdb-5bf5-b541-23dcff541104', 'tag', 2, 'feed');

-- P0006 - Oyun Mağazası (Orta)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('8127cff7-c15b-5cb8-8792-196f17dfe3bd', 'P0006 - Oyun Mağazası',
        'Oyun kataloğu, kullanıcı yorum/puanları, oyun kütüphanesi ve kampanya yönetimi. apps/lesson03 altındaki Gamepedia örneği doğrudan başlangıç noktası olabilir.',
        3, 6, 'M')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = '8127cff7-c15b-5cb8-8792-196f17dfe3bd';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('8127cff7-c15b-5cb8-8792-196f17dfe3bd', 'language', 0, 'C#'),
    ('8127cff7-c15b-5cb8-8792-196f17dfe3bd', 'platform', 0, 'web'),
    ('8127cff7-c15b-5cb8-8792-196f17dfe3bd', 'platform', 1, 'api'),
    ('8127cff7-c15b-5cb8-8792-196f17dfe3bd', 'database', 0, 'MongoDB'),
    ('8127cff7-c15b-5cb8-8792-196f17dfe3bd', 'database', 1, 'PostgreSQL'),
    ('8127cff7-c15b-5cb8-8792-196f17dfe3bd', 'similar', 0, 'Steam'),
    ('8127cff7-c15b-5cb8-8792-196f17dfe3bd', 'tag', 0, 'P0006'),
    ('8127cff7-c15b-5cb8-8792-196f17dfe3bd', 'tag', 1, 'clean-architecture'),
    ('8127cff7-c15b-5cb8-8792-196f17dfe3bd', 'tag', 2, 'gamepedia');

-- P0007 - Bilgi Tabanı (İleri)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('340a48ad-6814-5beb-ad9c-3e3db50ff25c', 'P0007 - Bilgi Tabanı',
        'Sayfa/blok tabanlı not tutma, sayfalar arası iç bağlantı ve içerikte anlamsal arama sunan bilgi yönetim uygulaması. RAG pipeline''ı projenin ayrılmaz parçasıdır.',
        3, 7, 'L')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = '340a48ad-6814-5beb-ad9c-3e3db50ff25c';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('340a48ad-6814-5beb-ad9c-3e3db50ff25c', 'language', 0, 'Python'),
    ('340a48ad-6814-5beb-ad9c-3e3db50ff25c', 'language', 1, 'C#'),
    ('340a48ad-6814-5beb-ad9c-3e3db50ff25c', 'platform', 0, 'web'),
    ('340a48ad-6814-5beb-ad9c-3e3db50ff25c', 'platform', 1, 'api'),
    ('340a48ad-6814-5beb-ad9c-3e3db50ff25c', 'database', 0, 'Qdrant'),
    ('340a48ad-6814-5beb-ad9c-3e3db50ff25c', 'similar', 0, 'Notion'),
    ('340a48ad-6814-5beb-ad9c-3e3db50ff25c', 'tag', 0, 'P0007'),
    ('340a48ad-6814-5beb-ad9c-3e3db50ff25c', 'tag', 1, 'rag'),
    ('340a48ad-6814-5beb-ad9c-3e3db50ff25c', 'tag', 2, 'vektor-veritabani'),
    ('340a48ad-6814-5beb-ad9c-3e3db50ff25c', 'tag', 3, 'anlamsal-arama');

-- P0008 - Mini E-Ticaret (İleri)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('eb6a172c-c174-5fc1-882b-afbc06555cdf', 'P0008 - Mini E-Ticaret',
        'Ürün kataloğu, sepet, sipariş ve stok takibi. Gün 06''daki Sipariş-Stok-Tedarikçi senaryosunun genişletilip gerçek bir kod tabanına dönüştürülmüş hali.',
        3, 7, 'L')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = 'eb6a172c-c174-5fc1-882b-afbc06555cdf';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('eb6a172c-c174-5fc1-882b-afbc06555cdf', 'language', 0, 'C#'),
    ('eb6a172c-c174-5fc1-882b-afbc06555cdf', 'language', 1, 'JavaScript'),
    ('eb6a172c-c174-5fc1-882b-afbc06555cdf', 'platform', 0, 'web'),
    ('eb6a172c-c174-5fc1-882b-afbc06555cdf', 'platform', 1, 'api'),
    ('eb6a172c-c174-5fc1-882b-afbc06555cdf', 'database', 0, 'SQL-*'),
    ('eb6a172c-c174-5fc1-882b-afbc06555cdf', 'similar', 0, 'Trendyol'),
    ('eb6a172c-c174-5fc1-882b-afbc06555cdf', 'similar', 1, 'Amazon'),
    ('eb6a172c-c174-5fc1-882b-afbc06555cdf', 'tag', 0, 'P0008'),
    ('eb6a172c-c174-5fc1-882b-afbc06555cdf', 'tag', 1, 'microservices'),
    ('eb6a172c-c174-5fc1-882b-afbc06555cdf', 'tag', 2, 'event-driven'),
    ('eb6a172c-c174-5fc1-882b-afbc06555cdf', 'tag', 3, 'rabbitmq'),
    ('eb6a172c-c174-5fc1-882b-afbc06555cdf', 'tag', 4, 'azure-service-bus');

-- P0009 - Destek Masası (İleri)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('a852ba56-8843-55b9-942b-2d55112773f6', 'P0009 - Destek Masası',
        'Destek taleplerinin oluşturulup kategorilere yönlendirildiği ve takip edildiği uygulama. Talebi analiz edip yanıt öneren veya MCP ile durum güncelleyen bir Custom Agent eklenebilir.',
        3, 7, 'L')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = 'a852ba56-8843-55b9-942b-2d55112773f6';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('a852ba56-8843-55b9-942b-2d55112773f6', 'language', 0, 'C#'),
    ('a852ba56-8843-55b9-942b-2d55112773f6', 'language', 1, 'Python'),
    ('a852ba56-8843-55b9-942b-2d55112773f6', 'platform', 0, 'web'),
    ('a852ba56-8843-55b9-942b-2d55112773f6', 'platform', 1, 'api'),
    ('a852ba56-8843-55b9-942b-2d55112773f6', 'database', 0, 'SQL-*'),
    ('a852ba56-8843-55b9-942b-2d55112773f6', 'similar', 0, 'Zendesk'),
    ('a852ba56-8843-55b9-942b-2d55112773f6', 'tag', 0, 'P0009'),
    ('a852ba56-8843-55b9-942b-2d55112773f6', 'tag', 1, 'custom-agent'),
    ('a852ba56-8843-55b9-942b-2d55112773f6', 'tag', 2, 'mcp');

-- P0010 - Dijital Cüzdan (İleri)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('f3f6530f-ce5e-5857-85c6-7bb71a6f5c5d', 'P0010 - Dijital Cüzdan',
        'Hesap, bakiye ve para transferi yönetimi. Odak; yetkilendirme, hassas veri şifreleme, işlem geçmişi/denetim kaydı (audit log) ve tutarlılıktır (CAP teoreminde CP tercihi).',
        3, 7, 'L')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = 'f3f6530f-ce5e-5857-85c6-7bb71a6f5c5d';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('f3f6530f-ce5e-5857-85c6-7bb71a6f5c5d', 'language', 0, 'C#'),
    ('f3f6530f-ce5e-5857-85c6-7bb71a6f5c5d', 'platform', 0, 'web'),
    ('f3f6530f-ce5e-5857-85c6-7bb71a6f5c5d', 'platform', 1, 'api'),
    ('f3f6530f-ce5e-5857-85c6-7bb71a6f5c5d', 'database', 0, 'SQL-*'),
    ('f3f6530f-ce5e-5857-85c6-7bb71a6f5c5d', 'similar', 0, 'Revolut'),
    ('f3f6530f-ce5e-5857-85c6-7bb71a6f5c5d', 'tag', 0, 'P0010'),
    ('f3f6530f-ce5e-5857-85c6-7bb71a6f5c5d', 'tag', 1, 'guvenlik'),
    ('f3f6530f-ce5e-5857-85c6-7bb71a6f5c5d', 'tag', 2, 'jwt'),
    ('f3f6530f-ce5e-5857-85c6-7bb71a6f5c5d', 'tag', 3, 'audit-log'),
    ('f3f6530f-ce5e-5857-85c6-7bb71a6f5c5d', 'tag', 4, 'cap-teoremi');

-- P0011 - Mini Lisp/Scheme Yorumlayıcısı (İleri)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('2db61a1f-bbff-52c3-9b4e-27357431781b', 'P0011 - Mini Lisp/Scheme Yorumlayıcısı',
        'Aritmetik, koşullu ifadeler, fonksiyon tanımları ve özyinelemeyi destekleyen minimal yorumlayıcı (lexer -> parser -> evaluator). Closure ve immutability gibi kavramları uygulatır.',
        3, 7, 'L')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = '2db61a1f-bbff-52c3-9b4e-27357431781b';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('2db61a1f-bbff-52c3-9b4e-27357431781b', 'language', 0, 'OCaml'),
    ('2db61a1f-bbff-52c3-9b4e-27357431781b', 'language', 1, 'Rust'),
    ('2db61a1f-bbff-52c3-9b4e-27357431781b', 'language', 2, 'Python'),
    ('2db61a1f-bbff-52c3-9b4e-27357431781b', 'platform', 0, 'cli'),
    ('2db61a1f-bbff-52c3-9b4e-27357431781b', 'database', 0, 'SQLite'),
    ('2db61a1f-bbff-52c3-9b4e-27357431781b', 'similar', 0, 'Lisp'),
    ('2db61a1f-bbff-52c3-9b4e-27357431781b', 'similar', 1, 'Scheme'),
    ('2db61a1f-bbff-52c3-9b4e-27357431781b', 'tag', 0, 'P0011'),
    ('2db61a1f-bbff-52c3-9b4e-27357431781b', 'tag', 1, 'yorumlayici'),
    ('2db61a1f-bbff-52c3-9b4e-27357431781b', 'tag', 2, 'fonksiyonel-programlama');

-- P0012 - Basit Bir Unix Shell (İleri)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('79cfb6de-1a95-534b-90b9-25f56d30c666', 'P0012 - Basit Bir Unix Shell',
        'Komut çalıştırma, pipe ve yönlendirme (>, <) operatörlerini destekleyen minimal komut satırı yorumlayıcısı. Process spawn ve file descriptor yönetimiyle işletim sistemi kavramlarını somutlaştırır.',
        3, 7, 'L')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = '79cfb6de-1a95-534b-90b9-25f56d30c666';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('79cfb6de-1a95-534b-90b9-25f56d30c666', 'language', 0, 'Rust'),
    ('79cfb6de-1a95-534b-90b9-25f56d30c666', 'language', 1, 'C'),
    ('79cfb6de-1a95-534b-90b9-25f56d30c666', 'language', 2, 'C++'),
    ('79cfb6de-1a95-534b-90b9-25f56d30c666', 'platform', 0, 'cli'),
    ('79cfb6de-1a95-534b-90b9-25f56d30c666', 'database', 0, 'SQLite'),
    ('79cfb6de-1a95-534b-90b9-25f56d30c666', 'similar', 0, 'Bash'),
    ('79cfb6de-1a95-534b-90b9-25f56d30c666', 'tag', 0, 'P0012'),
    ('79cfb6de-1a95-534b-90b9-25f56d30c666', 'tag', 1, 'isletim-sistemleri'),
    ('79cfb6de-1a95-534b-90b9-25f56d30c666', 'tag', 2, 'sistem-cagrilari');

-- P0013 - Tetris Clone (Orta)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('e1912a08-8fed-55e6-85b6-c1f697420d0a', 'P0013 - Tetris Clone',
        'Çarpışma tespiti, satır temizleme ve skor sistemi içeren klasik blok düşürme oyunu. Oyun döngüsü (game loop), durum yönetimi ve basit render mantığını öğretir.',
        3, 6, 'M')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = 'e1912a08-8fed-55e6-85b6-c1f697420d0a';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('e1912a08-8fed-55e6-85b6-c1f697420d0a', 'language', 0, 'Rust'),
    ('e1912a08-8fed-55e6-85b6-c1f697420d0a', 'language', 1, 'Python'),
    ('e1912a08-8fed-55e6-85b6-c1f697420d0a', 'language', 2, 'C#'),
    ('e1912a08-8fed-55e6-85b6-c1f697420d0a', 'platform', 0, 'desktop'),
    ('e1912a08-8fed-55e6-85b6-c1f697420d0a', 'database', 0, 'SQLite'),
    ('e1912a08-8fed-55e6-85b6-c1f697420d0a', 'similar', 0, 'Tetris'),
    ('e1912a08-8fed-55e6-85b6-c1f697420d0a', 'tag', 0, 'P0013'),
    ('e1912a08-8fed-55e6-85b6-c1f697420d0a', 'tag', 1, 'oyun'),
    ('e1912a08-8fed-55e6-85b6-c1f697420d0a', 'tag', 2, 'game-loop');

-- P0014 - 2048 / Yılan (Snake) Clone (Kolay)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('a34615f5-1ac6-5caf-9050-232699d3385a', 'P0014 - 2048 / Yılan (Snake) Clone',
        'Basit kural setine sahip, hızlı tamamlanabilecek bir oyun. Durum yönetimi, klavye olay yönetimi (event handling) ve temel oyun mantığı pratiği için idealdir.',
        2, 5, 'S')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = 'a34615f5-1ac6-5caf-9050-232699d3385a';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('a34615f5-1ac6-5caf-9050-232699d3385a', 'language', 0, 'JavaScript'),
    ('a34615f5-1ac6-5caf-9050-232699d3385a', 'language', 1, 'Python'),
    ('a34615f5-1ac6-5caf-9050-232699d3385a', 'language', 2, 'C#'),
    ('a34615f5-1ac6-5caf-9050-232699d3385a', 'platform', 0, 'web'),
    ('a34615f5-1ac6-5caf-9050-232699d3385a', 'platform', 1, 'desktop'),
    ('a34615f5-1ac6-5caf-9050-232699d3385a', 'database', 0, 'SQLite'),
    ('a34615f5-1ac6-5caf-9050-232699d3385a', 'similar', 0, '2048'),
    ('a34615f5-1ac6-5caf-9050-232699d3385a', 'similar', 1, 'Snake'),
    ('a34615f5-1ac6-5caf-9050-232699d3385a', 'tag', 0, 'P0014'),
    ('a34615f5-1ac6-5caf-9050-232699d3385a', 'tag', 1, 'oyun'),
    ('a34615f5-1ac6-5caf-9050-232699d3385a', 'tag', 2, 'event-handling');

-- P0015 - Sistem Kaynak İzleyici (Orta)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('3aa269fa-ee86-52b0-8a5f-b71843162e07', 'P0015 - Sistem Kaynak İzleyici',
        'Çalışan process''leri, CPU/bellek kullanımını ve ağ trafiğini gerçek zamanlı listeleyen masaüstü uygulaması. İşletim sistemi API''leriyle doğrudan çalışmayı gerektirir.',
        3, 6, 'M')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = '3aa269fa-ee86-52b0-8a5f-b71843162e07';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('3aa269fa-ee86-52b0-8a5f-b71843162e07', 'language', 0, 'Python'),
    ('3aa269fa-ee86-52b0-8a5f-b71843162e07', 'language', 1, 'Rust'),
    ('3aa269fa-ee86-52b0-8a5f-b71843162e07', 'language', 2, 'C#'),
    ('3aa269fa-ee86-52b0-8a5f-b71843162e07', 'platform', 0, 'desktop'),
    ('3aa269fa-ee86-52b0-8a5f-b71843162e07', 'database', 0, 'SQLite'),
    ('3aa269fa-ee86-52b0-8a5f-b71843162e07', 'similar', 0, 'Task Manager'),
    ('3aa269fa-ee86-52b0-8a5f-b71843162e07', 'similar', 1, 'htop'),
    ('3aa269fa-ee86-52b0-8a5f-b71843162e07', 'tag', 0, 'P0015'),
    ('3aa269fa-ee86-52b0-8a5f-b71843162e07', 'tag', 1, 'sistem-programlama'),
    ('3aa269fa-ee86-52b0-8a5f-b71843162e07', 'tag', 2, 'real-time');

-- P0016 - IoT Sensör Paneli (İleri)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('b8d0f40d-fd92-56a6-9d0a-a9a7f9e87f26', 'P0016 - IoT Sensör Paneli',
        'Simüle edilen sıcaklık, nem, hareket gibi sensör verilerinin MQTT ile merkezi bir panele gerçek zamanlı aktarıldığı sistem. Gün 06''daki asenkron mesajlaşma kavramlarıyla örtüşür.',
        3, 7, 'L')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = 'b8d0f40d-fd92-56a6-9d0a-a9a7f9e87f26';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('b8d0f40d-fd92-56a6-9d0a-a9a7f9e87f26', 'language', 0, 'Python'),
    ('b8d0f40d-fd92-56a6-9d0a-a9a7f9e87f26', 'language', 1, 'JavaScript'),
    ('b8d0f40d-fd92-56a6-9d0a-a9a7f9e87f26', 'platform', 0, 'web'),
    ('b8d0f40d-fd92-56a6-9d0a-a9a7f9e87f26', 'platform', 1, 'iot'),
    ('b8d0f40d-fd92-56a6-9d0a-a9a7f9e87f26', 'database', 0, 'SQL-*'),
    ('b8d0f40d-fd92-56a6-9d0a-a9a7f9e87f26', 'similar', 0, 'Google Home'),
    ('b8d0f40d-fd92-56a6-9d0a-a9a7f9e87f26', 'similar', 1, 'SmartThings'),
    ('b8d0f40d-fd92-56a6-9d0a-a9a7f9e87f26', 'tag', 0, 'P0016'),
    ('b8d0f40d-fd92-56a6-9d0a-a9a7f9e87f26', 'tag', 1, 'mqtt'),
    ('b8d0f40d-fd92-56a6-9d0a-a9a7f9e87f26', 'tag', 2, 'real-time'),
    ('b8d0f40d-fd92-56a6-9d0a-a9a7f9e87f26', 'tag', 3, 'asenkron-mesajlasma');

-- P0017 - Mini ERP (Orta)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('cb31301b-3cba-5779-b1b6-9aa11c9ae4de', 'P0017 - Mini ERP',
        'Ürün stoğu, fatura kesme, cari hesap takibi ve basit raporlama içeren işletme yönetim uygulaması. İş süreçlerini yazılıma dökme pratiği sağlar.',
        3, 6, 'M')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = 'cb31301b-3cba-5779-b1b6-9aa11c9ae4de';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('cb31301b-3cba-5779-b1b6-9aa11c9ae4de', 'language', 0, 'C#'),
    ('cb31301b-3cba-5779-b1b6-9aa11c9ae4de', 'language', 1, 'Python'),
    ('cb31301b-3cba-5779-b1b6-9aa11c9ae4de', 'platform', 0, 'web'),
    ('cb31301b-3cba-5779-b1b6-9aa11c9ae4de', 'database', 0, 'SQL-*'),
    ('cb31301b-3cba-5779-b1b6-9aa11c9ae4de', 'similar', 0, 'Logo'),
    ('cb31301b-3cba-5779-b1b6-9aa11c9ae4de', 'similar', 1, 'QuickBooks'),
    ('cb31301b-3cba-5779-b1b6-9aa11c9ae4de', 'tag', 0, 'P0017'),
    ('cb31301b-3cba-5779-b1b6-9aa11c9ae4de', 'tag', 1, 'blazor'),
    ('cb31301b-3cba-5779-b1b6-9aa11c9ae4de', 'tag', 2, 'django'),
    ('cb31301b-3cba-5779-b1b6-9aa11c9ae4de', 'tag', 3, 'clean-architecture');

-- P0018 - Basit Bir Tablolama Aracı (İleri)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('526a6bb7-77e3-5aa2-85d9-9336db910582', 'P0018 - Basit Bir Tablolama Aracı',
        'Hücre tabanlı veri girişi, temel formüller (SUM, AVERAGE vb.) ve hücreler arası bağımlılık grafiğinin yönetildiği uygulama. Formül ayrıştırma ve topological sort ile ilişkilidir.',
        3, 7, 'L')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = '526a6bb7-77e3-5aa2-85d9-9336db910582';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('526a6bb7-77e3-5aa2-85d9-9336db910582', 'language', 0, 'TypeScript'),
    ('526a6bb7-77e3-5aa2-85d9-9336db910582', 'language', 1, 'C#'),
    ('526a6bb7-77e3-5aa2-85d9-9336db910582', 'platform', 0, 'web'),
    ('526a6bb7-77e3-5aa2-85d9-9336db910582', 'platform', 1, 'desktop'),
    ('526a6bb7-77e3-5aa2-85d9-9336db910582', 'database', 0, 'SQLite'),
    ('526a6bb7-77e3-5aa2-85d9-9336db910582', 'similar', 0, 'Excel'),
    ('526a6bb7-77e3-5aa2-85d9-9336db910582', 'similar', 1, 'Google Sheets'),
    ('526a6bb7-77e3-5aa2-85d9-9336db910582', 'tag', 0, 'P0018'),
    ('526a6bb7-77e3-5aa2-85d9-9336db910582', 'tag', 1, 'parsing'),
    ('526a6bb7-77e3-5aa2-85d9-9336db910582', 'tag', 2, 'topological-sort');

-- P0019 - Masaüstü Metin Editörü (Orta)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('8aeb3a14-e793-55c3-aaed-3aa062179ed4', 'P0019 - Masaüstü Metin Editörü',
        'Söz dizimi renklendirme (syntax highlighting), çoklu sekme yönetimi ve temel dosya işlemlerini içeren masaüstü metin editörü.',
        3, 6, 'M')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = '8aeb3a14-e793-55c3-aaed-3aa062179ed4';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('8aeb3a14-e793-55c3-aaed-3aa062179ed4', 'language', 0, 'Rust'),
    ('8aeb3a14-e793-55c3-aaed-3aa062179ed4', 'language', 1, 'Python'),
    ('8aeb3a14-e793-55c3-aaed-3aa062179ed4', 'language', 2, 'TypeScript'),
    ('8aeb3a14-e793-55c3-aaed-3aa062179ed4', 'platform', 0, 'desktop'),
    ('8aeb3a14-e793-55c3-aaed-3aa062179ed4', 'database', 0, 'SQLite'),
    ('8aeb3a14-e793-55c3-aaed-3aa062179ed4', 'similar', 0, 'Notepad++'),
    ('8aeb3a14-e793-55c3-aaed-3aa062179ed4', 'similar', 1, 'VS Code'),
    ('8aeb3a14-e793-55c3-aaed-3aa062179ed4', 'tag', 0, 'P0019'),
    ('8aeb3a14-e793-55c3-aaed-3aa062179ed4', 'tag', 1, 'tauri'),
    ('8aeb3a14-e793-55c3-aaed-3aa062179ed4', 'tag', 2, 'electron'),
    ('8aeb3a14-e793-55c3-aaed-3aa062179ed4', 'tag', 3, 'syntax-highlighting');

-- P0020 - Sinyal Görselleştirme Aracı (İleri)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('aef238f5-4b11-5954-9adf-e43fb8d4e3c4', 'P0020 - Sinyal Görselleştirme Aracı',
        'Mikrofon veya dosyadan alınan sinyalin dalga formunu ve frekans spektrumunu (FFT) gerçek zamanlı görselleştiren masaüstü uygulaması. Sinyaller ve sistemler konusuyla kesişir.',
        3, 7, 'L')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = 'aef238f5-4b11-5954-9adf-e43fb8d4e3c4';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('aef238f5-4b11-5954-9adf-e43fb8d4e3c4', 'language', 0, 'Python'),
    ('aef238f5-4b11-5954-9adf-e43fb8d4e3c4', 'language', 1, 'Rust'),
    ('aef238f5-4b11-5954-9adf-e43fb8d4e3c4', 'platform', 0, 'desktop'),
    ('aef238f5-4b11-5954-9adf-e43fb8d4e3c4', 'database', 0, 'SQLite'),
    ('aef238f5-4b11-5954-9adf-e43fb8d4e3c4', 'similar', 0, 'Osiloskop'),
    ('aef238f5-4b11-5954-9adf-e43fb8d4e3c4', 'tag', 0, 'P0020'),
    ('aef238f5-4b11-5954-9adf-e43fb8d4e3c4', 'tag', 1, 'fft'),
    ('aef238f5-4b11-5954-9adf-e43fb8d4e3c4', 'tag', 2, 'sinyal-isleme');

-- P0021 - Mini Redis (İleri)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('d7977ab8-c372-5f57-b740-326ec3fe5548', 'P0021 - Mini Redis',
        'RESP protokolüyle konuşan, GET/SET/EXPIRE ve liste/hash komutlarını destekleyen bellek içi anahtar-değer sunucusu. Eşzamanlı istemci yönetimi, TTL ve disk kalıcılığı (AOF/snapshot) öğretir.',
        3, 7, 'L')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = 'd7977ab8-c372-5f57-b740-326ec3fe5548';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('d7977ab8-c372-5f57-b740-326ec3fe5548', 'language', 0, 'Go'),
    ('d7977ab8-c372-5f57-b740-326ec3fe5548', 'language', 1, 'Zig'),
    ('d7977ab8-c372-5f57-b740-326ec3fe5548', 'platform', 0, 'server'),
    ('d7977ab8-c372-5f57-b740-326ec3fe5548', 'platform', 1, 'cli'),
    ('d7977ab8-c372-5f57-b740-326ec3fe5548', 'database', 0, 'Redis'),
    ('d7977ab8-c372-5f57-b740-326ec3fe5548', 'similar', 0, 'Redis'),
    ('d7977ab8-c372-5f57-b740-326ec3fe5548', 'tag', 0, 'P0021'),
    ('d7977ab8-c372-5f57-b740-326ec3fe5548', 'tag', 1, 'tcp'),
    ('d7977ab8-c372-5f57-b740-326ec3fe5548', 'tag', 2, 'eszamanlilik'),
    ('d7977ab8-c372-5f57-b740-326ec3fe5548', 'tag', 3, 'protokol-tasarimi');

-- P0022 - Alışkanlık Takipçisi (Kolay)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('f62381ce-a952-512c-8fb1-f1742f0cf517', 'P0022 - Alışkanlık Takipçisi',
        'Günlük alışkanlıkların işaretlendiği, seri (streak) ve istatistiklerin gösterildiği mobil uygulama. Çevrimdışı öncelikli (offline-first) yerel kayıt, hatırlatıcı bildirimler ve isteğe bağlı bulut senkronizasyonu içerir.',
        2, 5, 'S')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = 'f62381ce-a952-512c-8fb1-f1742f0cf517';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('f62381ce-a952-512c-8fb1-f1742f0cf517', 'language', 0, 'Dart'),
    ('f62381ce-a952-512c-8fb1-f1742f0cf517', 'platform', 0, 'mobile'),
    ('f62381ce-a952-512c-8fb1-f1742f0cf517', 'database', 0, 'SQLite'),
    ('f62381ce-a952-512c-8fb1-f1742f0cf517', 'database', 1, 'Firebase'),
    ('f62381ce-a952-512c-8fb1-f1742f0cf517', 'similar', 0, 'Habitica'),
    ('f62381ce-a952-512c-8fb1-f1742f0cf517', 'similar', 1, 'Loop Habit Tracker'),
    ('f62381ce-a952-512c-8fb1-f1742f0cf517', 'tag', 0, 'P0022'),
    ('f62381ce-a952-512c-8fb1-f1742f0cf517', 'tag', 1, 'flutter'),
    ('f62381ce-a952-512c-8fb1-f1742f0cf517', 'tag', 2, 'offline-first'),
    ('f62381ce-a952-512c-8fb1-f1742f0cf517', 'tag', 3, 'state-management');

-- P0023 - Sohbet Sunucusu (İleri)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('47ff4585-5435-5d7e-ad33-64073f5793c1', 'P0023 - Sohbet Sunucusu',
        'Sunucu/kanal yapısı, gerçek zamanlı mesajlaşma, çevrimiçi durumu (presence) ve okunmadı sayaçları olan sohbet uygulaması. BEAM üzerinde süreç tabanlı eşzamanlılık ve supervisor ile hata toleransı uygulatır.',
        3, 7, 'L')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = '47ff4585-5435-5d7e-ad33-64073f5793c1';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('47ff4585-5435-5d7e-ad33-64073f5793c1', 'language', 0, 'Elixir'),
    ('47ff4585-5435-5d7e-ad33-64073f5793c1', 'platform', 0, 'web'),
    ('47ff4585-5435-5d7e-ad33-64073f5793c1', 'database', 0, 'PostgreSQL'),
    ('47ff4585-5435-5d7e-ad33-64073f5793c1', 'similar', 0, 'Discord'),
    ('47ff4585-5435-5d7e-ad33-64073f5793c1', 'similar', 1, 'Slack'),
    ('47ff4585-5435-5d7e-ad33-64073f5793c1', 'tag', 0, 'P0023'),
    ('47ff4585-5435-5d7e-ad33-64073f5793c1', 'tag', 1, 'phoenix'),
    ('47ff4585-5435-5d7e-ad33-64073f5793c1', 'tag', 2, 'websocket'),
    ('47ff4585-5435-5d7e-ad33-64073f5793c1', 'tag', 3, 'actor-model'),
    ('47ff4585-5435-5d7e-ad33-64073f5793c1', 'tag', 4, 'fault-tolerance');

-- P0024 - Yemek Siparişi (Orta)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('db17d934-22bc-59cb-b0dc-af7c1738d6b7', 'P0024 - Yemek Siparişi',
        'Restoran menüsü, sepet, sipariş durumu ve kurye konumunun haritada izlendiği native mobil uygulama ve API. Push bildirim ve sipariş yaşam döngüsü için durum makinesi (state machine) pratiği sağlar.',
        3, 6, 'M')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = 'db17d934-22bc-59cb-b0dc-af7c1738d6b7';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('db17d934-22bc-59cb-b0dc-af7c1738d6b7', 'language', 0, 'Kotlin'),
    ('db17d934-22bc-59cb-b0dc-af7c1738d6b7', 'language', 1, 'Swift'),
    ('db17d934-22bc-59cb-b0dc-af7c1738d6b7', 'platform', 0, 'mobile'),
    ('db17d934-22bc-59cb-b0dc-af7c1738d6b7', 'platform', 1, 'api'),
    ('db17d934-22bc-59cb-b0dc-af7c1738d6b7', 'database', 0, 'PostgreSQL'),
    ('db17d934-22bc-59cb-b0dc-af7c1738d6b7', 'similar', 0, 'Yemeksepeti'),
    ('db17d934-22bc-59cb-b0dc-af7c1738d6b7', 'similar', 1, 'Getir'),
    ('db17d934-22bc-59cb-b0dc-af7c1738d6b7', 'tag', 0, 'P0024'),
    ('db17d934-22bc-59cb-b0dc-af7c1738d6b7', 'tag', 1, 'android'),
    ('db17d934-22bc-59cb-b0dc-af7c1738d6b7', 'tag', 2, 'ios'),
    ('db17d934-22bc-59cb-b0dc-af7c1738d6b7', 'tag', 3, 'jetpack-compose'),
    ('db17d934-22bc-59cb-b0dc-af7c1738d6b7', 'tag', 4, 'swiftui'),
    ('db17d934-22bc-59cb-b0dc-af7c1738d6b7', 'tag', 5, 'state-machine');

-- P0025 - Etkinlik Bileti (Orta)
INSERT INTO projects (project_id, title, summary, team_min, team_max, size)
VALUES ('687cf59e-f88b-5a5f-93fe-d91eb95aa2b4', 'P0025 - Etkinlik Bileti',
        'Etkinlik listeleme, salon/koltuk seçimi, süreli koltuk rezervasyonu ve bilet satışı. Aynı koltuğun iki kez satılmaması için transaction, iyimser kilitleme (optimistic locking) ve TTL konularına odaklanır.',
        3, 6, 'M')
ON CONFLICT (project_id) DO UPDATE
SET title = excluded.title, summary = excluded.summary, team_min = excluded.team_min,
    team_max = excluded.team_max, size = excluded.size;
DELETE FROM project_items WHERE project_id = '687cf59e-f88b-5a5f-93fe-d91eb95aa2b4';
INSERT INTO project_items (project_id, kind, rank, name) VALUES
    ('687cf59e-f88b-5a5f-93fe-d91eb95aa2b4', 'language', 0, 'Java'),
    ('687cf59e-f88b-5a5f-93fe-d91eb95aa2b4', 'language', 1, 'Ruby'),
    ('687cf59e-f88b-5a5f-93fe-d91eb95aa2b4', 'platform', 0, 'web'),
    ('687cf59e-f88b-5a5f-93fe-d91eb95aa2b4', 'platform', 1, 'api'),
    ('687cf59e-f88b-5a5f-93fe-d91eb95aa2b4', 'database', 0, 'PostgreSQL'),
    ('687cf59e-f88b-5a5f-93fe-d91eb95aa2b4', 'database', 1, 'Redis'),
    ('687cf59e-f88b-5a5f-93fe-d91eb95aa2b4', 'similar', 0, 'Biletix'),
    ('687cf59e-f88b-5a5f-93fe-d91eb95aa2b4', 'similar', 1, 'Eventbrite'),
    ('687cf59e-f88b-5a5f-93fe-d91eb95aa2b4', 'tag', 0, 'P0025'),
    ('687cf59e-f88b-5a5f-93fe-d91eb95aa2b4', 'tag', 1, 'spring-boot'),
    ('687cf59e-f88b-5a5f-93fe-d91eb95aa2b4', 'tag', 2, 'rails'),
    ('687cf59e-f88b-5a5f-93fe-d91eb95aa2b4', 'tag', 3, 'eszamanlilik'),
    ('687cf59e-f88b-5a5f-93fe-d91eb95aa2b4', 'tag', 4, 'transaction');

COMMIT;
