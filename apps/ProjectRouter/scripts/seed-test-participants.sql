-- Project Router: uygulama testleri için 100 anonim test katılımcısı ve bir turnuva.
--
-- ÖN KOŞUL: Önce scripts/seed-project-ideas.sql çalıştırılmalıdır; turnuva o scriptteki 25 projeyi kullanır.
--
-- Kullanım (apps/ProjectRouter altında, uygulama kapalıyken):
--   sqlite3 ProjectRouter.Web/projectrouter.db ".read scripts/seed-project-ideas.sql"
--   sqlite3 ProjectRouter.Web/projectrouter.db ".read scripts/seed-test-participants.sql"
--
-- Notlar:
-- * Veriler sabit tohumlu (seed=2026) bir üreteçle oluşturuldu; script her çalıştırmada aynı kayıtları upsert eder.
-- * Katılımcılar 1-3 dil ve 1-3 veritabanı tercihi taşır (sıra = tercih sırası, Rule 00/01).
--   Tercih havuzu yaygın dillerin yanında Java, Go ve Kotlin'i de içerir (P0021-P0025 projeleriyle eşleşir).
--   'SQL-*' / 'NoSQL-*' joker tercihleri de vardır.
-- * Katılımcıların ~%25'inin GitHub adresi yoktur (NULL).
-- * Turnuva dağıtılmamış (settled_at = NULL) olarak oluşturulur; mevcut dağıtımı varsa temizlenir.
-- * 25 projenin toplam kapasitesi 70-156 kişidir; 100 katılımcının tamamı bir projeye yerleştirilir
--   (açıkta öğrenci kalmaz). Hiçbir tercihiyle eşleşmeyen bir projeye yerleştirilen olursa uygulama bunu
--   uyarı olarak gösterir.
-- * Katılımcılar gerçek kişi izlenimi vermemesi için 'Katılımcı 1001' ... 'Katılımcı 1100' olarak adlandırılmıştır.
--   E-posta (katilimciNNNN@example.com) ve GitHub alanındaki adresler (example.com) örnek alan adı kullanır.

PRAGMA foreign_keys = ON;

BEGIN TRANSACTION;

-- Katılımcılar

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('9286ef93-cb61-54de-a42c-b620bd006e9e', 'Katılımcı 1001', 'katilimci1001@example.com', 'Yıldız Teknik Üniversitesi', 'Yazılım Mühendisliği', 4, 'https://example.com/katilimci-1001')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '9286ef93-cb61-54de-a42c-b620bd006e9e';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('9286ef93-cb61-54de-a42c-b620bd006e9e', 'language', 0, 'JavaScript'),
    ('9286ef93-cb61-54de-a42c-b620bd006e9e', 'language', 1, 'TypeScript'),
    ('9286ef93-cb61-54de-a42c-b620bd006e9e', 'database', 0, 'Redis'),
    ('9286ef93-cb61-54de-a42c-b620bd006e9e', 'database', 1, 'SQL Server'),
    ('9286ef93-cb61-54de-a42c-b620bd006e9e', 'database', 2, 'Qdrant');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('cf5e2dda-76b2-5f19-a45d-1f4302d990b2', 'Katılımcı 1002', 'katilimci1002@example.com', 'Yıldız Teknik Üniversitesi', 'Bilgisayar Mühendisliği', 3, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'cf5e2dda-76b2-5f19-a45d-1f4302d990b2';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('cf5e2dda-76b2-5f19-a45d-1f4302d990b2', 'language', 0, 'C#'),
    ('cf5e2dda-76b2-5f19-a45d-1f4302d990b2', 'database', 0, 'SQL Server'),
    ('cf5e2dda-76b2-5f19-a45d-1f4302d990b2', 'database', 1, 'SQLite'),
    ('cf5e2dda-76b2-5f19-a45d-1f4302d990b2', 'database', 2, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('2393a8dc-f861-59f0-867d-7218749051d8', 'Katılımcı 1003', 'katilimci1003@example.com', 'Ege Üniversitesi', 'Matematik Mühendisliği', 4, 'https://example.com/katilimci-1003')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '2393a8dc-f861-59f0-867d-7218749051d8';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('2393a8dc-f861-59f0-867d-7218749051d8', 'language', 0, 'C#'),
    ('2393a8dc-f861-59f0-867d-7218749051d8', 'language', 1, 'TypeScript'),
    ('2393a8dc-f861-59f0-867d-7218749051d8', 'database', 0, 'PostgreSQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('7e5b2b92-bc01-5a19-baee-d3038117c202', 'Katılımcı 1004', 'katilimci1004@example.com', 'Hacettepe Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 3, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '7e5b2b92-bc01-5a19-baee-d3038117c202';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('7e5b2b92-bc01-5a19-baee-d3038117c202', 'language', 0, 'Java'),
    ('7e5b2b92-bc01-5a19-baee-d3038117c202', 'database', 0, 'SQL-*'),
    ('7e5b2b92-bc01-5a19-baee-d3038117c202', 'database', 1, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('4f8a254e-d118-52b7-9da6-42bcda2e3c70', 'Katılımcı 1005', 'katilimci1005@example.com', 'Orta Doğu Teknik Üniversitesi', 'Endüstri Mühendisliği', 3, 'https://example.com/katilimci-1005')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '4f8a254e-d118-52b7-9da6-42bcda2e3c70';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('4f8a254e-d118-52b7-9da6-42bcda2e3c70', 'language', 0, 'JavaScript'),
    ('4f8a254e-d118-52b7-9da6-42bcda2e3c70', 'language', 1, 'C'),
    ('4f8a254e-d118-52b7-9da6-42bcda2e3c70', 'language', 2, 'TypeScript'),
    ('4f8a254e-d118-52b7-9da6-42bcda2e3c70', 'database', 0, 'Qdrant'),
    ('4f8a254e-d118-52b7-9da6-42bcda2e3c70', 'database', 1, 'NoSQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('a417251a-5d98-5a65-9bc2-316623144ade', 'Katılımcı 1006', 'katilimci1006@example.com', 'Ege Üniversitesi', 'Endüstri Mühendisliği', 2, 'https://example.com/katilimci-1006')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'a417251a-5d98-5a65-9bc2-316623144ade';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('a417251a-5d98-5a65-9bc2-316623144ade', 'language', 0, 'C'),
    ('a417251a-5d98-5a65-9bc2-316623144ade', 'language', 1, 'C#'),
    ('a417251a-5d98-5a65-9bc2-316623144ade', 'language', 2, 'Rust'),
    ('a417251a-5d98-5a65-9bc2-316623144ade', 'database', 0, 'MySQL'),
    ('a417251a-5d98-5a65-9bc2-316623144ade', 'database', 1, 'NoSQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('a01d58dd-59d3-588b-bd0d-89f505e9b5cd', 'Katılımcı 1007', 'katilimci1007@example.com', 'Orta Doğu Teknik Üniversitesi', 'Matematik Mühendisliği', 4, 'https://example.com/katilimci-1007')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'a01d58dd-59d3-588b-bd0d-89f505e9b5cd';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('a01d58dd-59d3-588b-bd0d-89f505e9b5cd', 'language', 0, 'JavaScript'),
    ('a01d58dd-59d3-588b-bd0d-89f505e9b5cd', 'language', 1, 'Java'),
    ('a01d58dd-59d3-588b-bd0d-89f505e9b5cd', 'database', 0, 'SQLite'),
    ('a01d58dd-59d3-588b-bd0d-89f505e9b5cd', 'database', 1, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('8fbcc16d-3784-57e1-8475-4064e7a782d7', 'Katılımcı 1008', 'katilimci1008@example.com', 'Dokuz Eylül Üniversitesi', 'Matematik Mühendisliği', 4, 'https://example.com/katilimci-1008')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '8fbcc16d-3784-57e1-8475-4064e7a782d7';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('8fbcc16d-3784-57e1-8475-4064e7a782d7', 'language', 0, 'C#'),
    ('8fbcc16d-3784-57e1-8475-4064e7a782d7', 'language', 1, 'C++'),
    ('8fbcc16d-3784-57e1-8475-4064e7a782d7', 'database', 0, 'Redis'),
    ('8fbcc16d-3784-57e1-8475-4064e7a782d7', 'database', 1, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('57149a7a-74f2-585c-9602-2d063c8c5ae0', 'Katılımcı 1009', 'katilimci1009@example.com', 'İstanbul Teknik Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 3, 'https://example.com/katilimci-1009')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '57149a7a-74f2-585c-9602-2d063c8c5ae0';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('57149a7a-74f2-585c-9602-2d063c8c5ae0', 'language', 0, 'C#'),
    ('57149a7a-74f2-585c-9602-2d063c8c5ae0', 'language', 1, 'C++'),
    ('57149a7a-74f2-585c-9602-2d063c8c5ae0', 'language', 2, 'C'),
    ('57149a7a-74f2-585c-9602-2d063c8c5ae0', 'database', 0, 'SQL Server'),
    ('57149a7a-74f2-585c-9602-2d063c8c5ae0', 'database', 1, 'NoSQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('0f05f23a-7cac-5c4d-b54c-af54f20f4663', 'Katılımcı 1010', 'katilimci1010@example.com', 'Yıldız Teknik Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 4, 'https://example.com/katilimci-1010')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '0f05f23a-7cac-5c4d-b54c-af54f20f4663';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('0f05f23a-7cac-5c4d-b54c-af54f20f4663', 'language', 0, 'C#'),
    ('0f05f23a-7cac-5c4d-b54c-af54f20f4663', 'language', 1, 'Java'),
    ('0f05f23a-7cac-5c4d-b54c-af54f20f4663', 'database', 0, 'NoSQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('0ea7f5a5-1db1-57e2-8cc3-638c18ed2435', 'Katılımcı 1011', 'katilimci1011@example.com', 'Yıldız Teknik Üniversitesi', 'Matematik Mühendisliği', 4, 'https://example.com/katilimci-1011')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '0ea7f5a5-1db1-57e2-8cc3-638c18ed2435';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('0ea7f5a5-1db1-57e2-8cc3-638c18ed2435', 'language', 0, 'TypeScript'),
    ('0ea7f5a5-1db1-57e2-8cc3-638c18ed2435', 'database', 0, 'NoSQL-*'),
    ('0ea7f5a5-1db1-57e2-8cc3-638c18ed2435', 'database', 1, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('b802df1e-c9fc-5538-9828-ebc6d7ab4d26', 'Katılımcı 1012', 'katilimci1012@example.com', 'Dokuz Eylül Üniversitesi', 'Yazılım Mühendisliği', 3, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'b802df1e-c9fc-5538-9828-ebc6d7ab4d26';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('b802df1e-c9fc-5538-9828-ebc6d7ab4d26', 'language', 0, 'C#'),
    ('b802df1e-c9fc-5538-9828-ebc6d7ab4d26', 'language', 1, 'Rust'),
    ('b802df1e-c9fc-5538-9828-ebc6d7ab4d26', 'database', 0, 'SQLite'),
    ('b802df1e-c9fc-5538-9828-ebc6d7ab4d26', 'database', 1, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('30403342-8ca0-52c7-a1c5-c67a738b8559', 'Katılımcı 1013', 'katilimci1013@example.com', 'Orta Doğu Teknik Üniversitesi', 'Matematik Mühendisliği', 3, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '30403342-8ca0-52c7-a1c5-c67a738b8559';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('30403342-8ca0-52c7-a1c5-c67a738b8559', 'language', 0, 'JavaScript'),
    ('30403342-8ca0-52c7-a1c5-c67a738b8559', 'language', 1, 'C#'),
    ('30403342-8ca0-52c7-a1c5-c67a738b8559', 'language', 2, 'Python'),
    ('30403342-8ca0-52c7-a1c5-c67a738b8559', 'database', 0, 'SQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('a20a4604-3b3f-57aa-a6b6-b5d5eb6decc1', 'Katılımcı 1014', 'katilimci1014@example.com', 'Hacettepe Üniversitesi', 'Bilgisayar Mühendisliği', 1, 'https://example.com/katilimci-1014')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'a20a4604-3b3f-57aa-a6b6-b5d5eb6decc1';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('a20a4604-3b3f-57aa-a6b6-b5d5eb6decc1', 'language', 0, 'Rust'),
    ('a20a4604-3b3f-57aa-a6b6-b5d5eb6decc1', 'language', 1, 'JavaScript'),
    ('a20a4604-3b3f-57aa-a6b6-b5d5eb6decc1', 'database', 0, 'SQL Server'),
    ('a20a4604-3b3f-57aa-a6b6-b5d5eb6decc1', 'database', 1, 'MongoDB'),
    ('a20a4604-3b3f-57aa-a6b6-b5d5eb6decc1', 'database', 2, 'MySQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('de2cd6bd-abd4-557f-8974-cc217289cbb1', 'Katılımcı 1015', 'katilimci1015@example.com', 'Marmara Üniversitesi', 'Endüstri Mühendisliği', 4, 'https://example.com/katilimci-1015')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'de2cd6bd-abd4-557f-8974-cc217289cbb1';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('de2cd6bd-abd4-557f-8974-cc217289cbb1', 'language', 0, 'Python'),
    ('de2cd6bd-abd4-557f-8974-cc217289cbb1', 'language', 1, 'C++'),
    ('de2cd6bd-abd4-557f-8974-cc217289cbb1', 'database', 0, 'PostgreSQL'),
    ('de2cd6bd-abd4-557f-8974-cc217289cbb1', 'database', 1, 'MySQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('499af19b-0df8-5ef0-a5dd-3d8e25e1d0f7', 'Katılımcı 1016', 'katilimci1016@example.com', 'İstanbul Teknik Üniversitesi', 'Endüstri Mühendisliği', 4, 'https://example.com/katilimci-1016')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '499af19b-0df8-5ef0-a5dd-3d8e25e1d0f7';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('499af19b-0df8-5ef0-a5dd-3d8e25e1d0f7', 'language', 0, 'JavaScript'),
    ('499af19b-0df8-5ef0-a5dd-3d8e25e1d0f7', 'language', 1, 'Rust'),
    ('499af19b-0df8-5ef0-a5dd-3d8e25e1d0f7', 'database', 0, 'MySQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('ab9cdb0d-6db6-5d5d-8302-812206a25846', 'Katılımcı 1017', 'katilimci1017@example.com', 'Boğaziçi Üniversitesi', 'Yazılım Mühendisliği', 4, 'https://example.com/katilimci-1017')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'ab9cdb0d-6db6-5d5d-8302-812206a25846';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('ab9cdb0d-6db6-5d5d-8302-812206a25846', 'language', 0, 'Python'),
    ('ab9cdb0d-6db6-5d5d-8302-812206a25846', 'database', 0, 'SQL-*'),
    ('ab9cdb0d-6db6-5d5d-8302-812206a25846', 'database', 1, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('ba3633a2-0958-52eb-88c2-007f3afa7649', 'Katılımcı 1018', 'katilimci1018@example.com', 'Dokuz Eylül Üniversitesi', 'Endüstri Mühendisliği', 4, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'ba3633a2-0958-52eb-88c2-007f3afa7649';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('ba3633a2-0958-52eb-88c2-007f3afa7649', 'language', 0, 'Python'),
    ('ba3633a2-0958-52eb-88c2-007f3afa7649', 'language', 1, 'C#'),
    ('ba3633a2-0958-52eb-88c2-007f3afa7649', 'language', 2, 'Rust'),
    ('ba3633a2-0958-52eb-88c2-007f3afa7649', 'database', 0, 'MongoDB'),
    ('ba3633a2-0958-52eb-88c2-007f3afa7649', 'database', 1, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('1ee1a35d-0151-52c6-89fd-16ef8e79dfb0', 'Katılımcı 1019', 'katilimci1019@example.com', 'Marmara Üniversitesi', 'Yazılım Mühendisliği', 1, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '1ee1a35d-0151-52c6-89fd-16ef8e79dfb0';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('1ee1a35d-0151-52c6-89fd-16ef8e79dfb0', 'language', 0, 'JavaScript'),
    ('1ee1a35d-0151-52c6-89fd-16ef8e79dfb0', 'database', 0, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('33263a1b-cc93-5ce6-9239-7849d1fae804', 'Katılımcı 1020', 'katilimci1020@example.com', 'Boğaziçi Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 3, 'https://example.com/katilimci-1020')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '33263a1b-cc93-5ce6-9239-7849d1fae804';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('33263a1b-cc93-5ce6-9239-7849d1fae804', 'language', 0, 'JavaScript'),
    ('33263a1b-cc93-5ce6-9239-7849d1fae804', 'language', 1, 'C#'),
    ('33263a1b-cc93-5ce6-9239-7849d1fae804', 'database', 0, 'MongoDB'),
    ('33263a1b-cc93-5ce6-9239-7849d1fae804', 'database', 1, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('6446948a-3ce3-536c-b35c-9f552ea27299', 'Katılımcı 1021', 'katilimci1021@example.com', 'Orta Doğu Teknik Üniversitesi', 'Matematik Mühendisliği', 2, 'https://example.com/katilimci-1021')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '6446948a-3ce3-536c-b35c-9f552ea27299';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('6446948a-3ce3-536c-b35c-9f552ea27299', 'language', 0, 'C#'),
    ('6446948a-3ce3-536c-b35c-9f552ea27299', 'language', 1, 'TypeScript'),
    ('6446948a-3ce3-536c-b35c-9f552ea27299', 'language', 2, 'Python'),
    ('6446948a-3ce3-536c-b35c-9f552ea27299', 'database', 0, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('01274473-9f9a-5cc3-89f0-b7b1acfc165b', 'Katılımcı 1022', 'katilimci1022@example.com', 'Hacettepe Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 4, 'https://example.com/katilimci-1022')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '01274473-9f9a-5cc3-89f0-b7b1acfc165b';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('01274473-9f9a-5cc3-89f0-b7b1acfc165b', 'language', 0, 'Go'),
    ('01274473-9f9a-5cc3-89f0-b7b1acfc165b', 'database', 0, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('1adb9e27-7052-53d6-86d7-9d82614c2c14', 'Katılımcı 1023', 'katilimci1023@example.com', 'Orta Doğu Teknik Üniversitesi', 'Endüstri Mühendisliği', 4, 'https://example.com/katilimci-1023')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '1adb9e27-7052-53d6-86d7-9d82614c2c14';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('1adb9e27-7052-53d6-86d7-9d82614c2c14', 'language', 0, 'Python'),
    ('1adb9e27-7052-53d6-86d7-9d82614c2c14', 'database', 0, 'PostgreSQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('9f742322-c7e8-5cc7-adbc-049d07165391', 'Katılımcı 1024', 'katilimci1024@example.com', 'Hacettepe Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 4, 'https://example.com/katilimci-1024')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '9f742322-c7e8-5cc7-adbc-049d07165391';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('9f742322-c7e8-5cc7-adbc-049d07165391', 'language', 0, 'TypeScript'),
    ('9f742322-c7e8-5cc7-adbc-049d07165391', 'language', 1, 'JavaScript'),
    ('9f742322-c7e8-5cc7-adbc-049d07165391', 'database', 0, 'SQLite'),
    ('9f742322-c7e8-5cc7-adbc-049d07165391', 'database', 1, 'PostgreSQL'),
    ('9f742322-c7e8-5cc7-adbc-049d07165391', 'database', 2, 'MySQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('20c787e8-9782-5d7e-83bd-ec5b7fe09e26', 'Katılımcı 1025', 'katilimci1025@example.com', 'Orta Doğu Teknik Üniversitesi', 'Endüstri Mühendisliği', 3, 'https://example.com/katilimci-1025')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '20c787e8-9782-5d7e-83bd-ec5b7fe09e26';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('20c787e8-9782-5d7e-83bd-ec5b7fe09e26', 'language', 0, 'C#'),
    ('20c787e8-9782-5d7e-83bd-ec5b7fe09e26', 'language', 1, 'C++'),
    ('20c787e8-9782-5d7e-83bd-ec5b7fe09e26', 'language', 2, 'Java'),
    ('20c787e8-9782-5d7e-83bd-ec5b7fe09e26', 'database', 0, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('d05c242c-3d0f-5d37-b4d5-4e49e9153f2a', 'Katılımcı 1026', 'katilimci1026@example.com', 'Hacettepe Üniversitesi', 'Matematik Mühendisliği', 4, 'https://example.com/katilimci-1026')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'd05c242c-3d0f-5d37-b4d5-4e49e9153f2a';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('d05c242c-3d0f-5d37-b4d5-4e49e9153f2a', 'language', 0, 'Python'),
    ('d05c242c-3d0f-5d37-b4d5-4e49e9153f2a', 'language', 1, 'TypeScript'),
    ('d05c242c-3d0f-5d37-b4d5-4e49e9153f2a', 'database', 0, 'PostgreSQL'),
    ('d05c242c-3d0f-5d37-b4d5-4e49e9153f2a', 'database', 1, 'MySQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('2abaf229-e076-53c1-a9b4-76dc240bde57', 'Katılımcı 1027', 'katilimci1027@example.com', 'Hacettepe Üniversitesi', 'Matematik Mühendisliği', 4, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '2abaf229-e076-53c1-a9b4-76dc240bde57';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('2abaf229-e076-53c1-a9b4-76dc240bde57', 'language', 0, 'C#'),
    ('2abaf229-e076-53c1-a9b4-76dc240bde57', 'language', 1, 'TypeScript'),
    ('2abaf229-e076-53c1-a9b4-76dc240bde57', 'database', 0, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('0dc64a9d-3b8f-5a9a-984e-18bc739d8fd3', 'Katılımcı 1028', 'katilimci1028@example.com', 'Marmara Üniversitesi', 'Yazılım Mühendisliği', 2, 'https://example.com/katilimci-1028')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '0dc64a9d-3b8f-5a9a-984e-18bc739d8fd3';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('0dc64a9d-3b8f-5a9a-984e-18bc739d8fd3', 'language', 0, 'Rust'),
    ('0dc64a9d-3b8f-5a9a-984e-18bc739d8fd3', 'database', 0, 'MongoDB'),
    ('0dc64a9d-3b8f-5a9a-984e-18bc739d8fd3', 'database', 1, 'SQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('3e9579fe-27f5-5940-bb8d-cbeadcfc7763', 'Katılımcı 1029', 'katilimci1029@example.com', 'Orta Doğu Teknik Üniversitesi', 'Matematik Mühendisliği', 3, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '3e9579fe-27f5-5940-bb8d-cbeadcfc7763';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('3e9579fe-27f5-5940-bb8d-cbeadcfc7763', 'language', 0, 'Python'),
    ('3e9579fe-27f5-5940-bb8d-cbeadcfc7763', 'database', 0, 'MongoDB'),
    ('3e9579fe-27f5-5940-bb8d-cbeadcfc7763', 'database', 1, 'SQL-*'),
    ('3e9579fe-27f5-5940-bb8d-cbeadcfc7763', 'database', 2, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('007463c9-f041-5ad4-a568-324f90201b21', 'Katılımcı 1030', 'katilimci1030@example.com', 'Hacettepe Üniversitesi', 'Bilgisayar Mühendisliği', 4, 'https://example.com/katilimci-1030')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '007463c9-f041-5ad4-a568-324f90201b21';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('007463c9-f041-5ad4-a568-324f90201b21', 'language', 0, 'Kotlin'),
    ('007463c9-f041-5ad4-a568-324f90201b21', 'database', 0, 'NoSQL-*'),
    ('007463c9-f041-5ad4-a568-324f90201b21', 'database', 1, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('d2d7c091-b785-5a93-918a-ad2b63a6ccd0', 'Katılımcı 1031', 'katilimci1031@example.com', 'Ege Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 4, 'https://example.com/katilimci-1031')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'd2d7c091-b785-5a93-918a-ad2b63a6ccd0';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('d2d7c091-b785-5a93-918a-ad2b63a6ccd0', 'language', 0, 'C'),
    ('d2d7c091-b785-5a93-918a-ad2b63a6ccd0', 'database', 0, 'PostgreSQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('b5a44ba2-8806-5144-81e3-d3625601a77e', 'Katılımcı 1032', 'katilimci1032@example.com', 'Dokuz Eylül Üniversitesi', 'Matematik Mühendisliği', 3, 'https://example.com/katilimci-1032')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'b5a44ba2-8806-5144-81e3-d3625601a77e';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('b5a44ba2-8806-5144-81e3-d3625601a77e', 'language', 0, 'Python'),
    ('b5a44ba2-8806-5144-81e3-d3625601a77e', 'database', 0, 'PostgreSQL'),
    ('b5a44ba2-8806-5144-81e3-d3625601a77e', 'database', 1, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('a01b5591-4e0b-52a3-b6f3-b78bda04c62c', 'Katılımcı 1033', 'katilimci1033@example.com', 'İstanbul Teknik Üniversitesi', 'Yazılım Mühendisliği', 2, 'https://example.com/katilimci-1033')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'a01b5591-4e0b-52a3-b6f3-b78bda04c62c';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('a01b5591-4e0b-52a3-b6f3-b78bda04c62c', 'language', 0, 'Rust'),
    ('a01b5591-4e0b-52a3-b6f3-b78bda04c62c', 'language', 1, 'C#'),
    ('a01b5591-4e0b-52a3-b6f3-b78bda04c62c', 'database', 0, 'NoSQL-*'),
    ('a01b5591-4e0b-52a3-b6f3-b78bda04c62c', 'database', 1, 'PostgreSQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('7d8c54e8-cf0c-5ca4-9d2e-6275f57880d0', 'Katılımcı 1034', 'katilimci1034@example.com', 'Orta Doğu Teknik Üniversitesi', 'Matematik Mühendisliği', 1, 'https://example.com/katilimci-1034')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '7d8c54e8-cf0c-5ca4-9d2e-6275f57880d0';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('7d8c54e8-cf0c-5ca4-9d2e-6275f57880d0', 'language', 0, 'JavaScript'),
    ('7d8c54e8-cf0c-5ca4-9d2e-6275f57880d0', 'language', 1, 'Kotlin'),
    ('7d8c54e8-cf0c-5ca4-9d2e-6275f57880d0', 'database', 0, 'MySQL'),
    ('7d8c54e8-cf0c-5ca4-9d2e-6275f57880d0', 'database', 1, 'NoSQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('1dd0f248-4e39-5ada-985b-f640fac3f9ad', 'Katılımcı 1035', 'katilimci1035@example.com', 'Ege Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 3, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '1dd0f248-4e39-5ada-985b-f640fac3f9ad';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('1dd0f248-4e39-5ada-985b-f640fac3f9ad', 'language', 0, 'Rust'),
    ('1dd0f248-4e39-5ada-985b-f640fac3f9ad', 'language', 1, 'TypeScript'),
    ('1dd0f248-4e39-5ada-985b-f640fac3f9ad', 'database', 0, 'SQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('a1cad683-58c2-5fb3-bf96-257c710d4a2d', 'Katılımcı 1036', 'katilimci1036@example.com', 'Boğaziçi Üniversitesi', 'Endüstri Mühendisliği', 2, 'https://example.com/katilimci-1036')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'a1cad683-58c2-5fb3-bf96-257c710d4a2d';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('a1cad683-58c2-5fb3-bf96-257c710d4a2d', 'language', 0, 'C#'),
    ('a1cad683-58c2-5fb3-bf96-257c710d4a2d', 'language', 1, 'Rust'),
    ('a1cad683-58c2-5fb3-bf96-257c710d4a2d', 'language', 2, 'TypeScript'),
    ('a1cad683-58c2-5fb3-bf96-257c710d4a2d', 'database', 0, 'Qdrant'),
    ('a1cad683-58c2-5fb3-bf96-257c710d4a2d', 'database', 1, 'SQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('2e7a4e33-88fa-5c90-9609-6be14efec259', 'Katılımcı 1037', 'katilimci1037@example.com', 'İstanbul Teknik Üniversitesi', 'Yazılım Mühendisliği', 3, 'https://example.com/katilimci-1037')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '2e7a4e33-88fa-5c90-9609-6be14efec259';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('2e7a4e33-88fa-5c90-9609-6be14efec259', 'language', 0, 'C#'),
    ('2e7a4e33-88fa-5c90-9609-6be14efec259', 'database', 0, 'MySQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('1a6b0adc-8e1f-50c1-8a02-a8d9dc92b9b5', 'Katılımcı 1038', 'katilimci1038@example.com', 'Hacettepe Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 2, 'https://example.com/katilimci-1038')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '1a6b0adc-8e1f-50c1-8a02-a8d9dc92b9b5';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('1a6b0adc-8e1f-50c1-8a02-a8d9dc92b9b5', 'language', 0, 'TypeScript'),
    ('1a6b0adc-8e1f-50c1-8a02-a8d9dc92b9b5', 'database', 0, 'NoSQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('80834d66-2ebd-5cdf-a8e5-ca0a46656490', 'Katılımcı 1039', 'katilimci1039@example.com', 'Yıldız Teknik Üniversitesi', 'Endüstri Mühendisliği', 3, 'https://example.com/katilimci-1039')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '80834d66-2ebd-5cdf-a8e5-ca0a46656490';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('80834d66-2ebd-5cdf-a8e5-ca0a46656490', 'language', 0, 'C#'),
    ('80834d66-2ebd-5cdf-a8e5-ca0a46656490', 'language', 1, 'TypeScript'),
    ('80834d66-2ebd-5cdf-a8e5-ca0a46656490', 'database', 0, 'MongoDB'),
    ('80834d66-2ebd-5cdf-a8e5-ca0a46656490', 'database', 1, 'PostgreSQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('326d358b-0659-5d07-b5c7-bbc052e8997a', 'Katılımcı 1040', 'katilimci1040@example.com', 'Hacettepe Üniversitesi', 'Bilgisayar Mühendisliği', 1, 'https://example.com/katilimci-1040')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '326d358b-0659-5d07-b5c7-bbc052e8997a';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('326d358b-0659-5d07-b5c7-bbc052e8997a', 'language', 0, 'C++'),
    ('326d358b-0659-5d07-b5c7-bbc052e8997a', 'language', 1, 'Rust'),
    ('326d358b-0659-5d07-b5c7-bbc052e8997a', 'language', 2, 'Python'),
    ('326d358b-0659-5d07-b5c7-bbc052e8997a', 'database', 0, 'PostgreSQL'),
    ('326d358b-0659-5d07-b5c7-bbc052e8997a', 'database', 1, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('38dab00d-9990-5ff4-afe2-0504f1078be7', 'Katılımcı 1041', 'katilimci1041@example.com', 'Dokuz Eylül Üniversitesi', 'Endüstri Mühendisliği', 3, 'https://example.com/katilimci-1041')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '38dab00d-9990-5ff4-afe2-0504f1078be7';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('38dab00d-9990-5ff4-afe2-0504f1078be7', 'language', 0, 'JavaScript'),
    ('38dab00d-9990-5ff4-afe2-0504f1078be7', 'database', 0, 'NoSQL-*'),
    ('38dab00d-9990-5ff4-afe2-0504f1078be7', 'database', 1, 'MySQL'),
    ('38dab00d-9990-5ff4-afe2-0504f1078be7', 'database', 2, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('a87018d8-8d1b-526d-8134-11e559854419', 'Katılımcı 1042', 'katilimci1042@example.com', 'Dokuz Eylül Üniversitesi', 'Matematik Mühendisliği', 4, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'a87018d8-8d1b-526d-8134-11e559854419';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('a87018d8-8d1b-526d-8134-11e559854419', 'language', 0, 'Python'),
    ('a87018d8-8d1b-526d-8134-11e559854419', 'database', 0, 'SQLite'),
    ('a87018d8-8d1b-526d-8134-11e559854419', 'database', 1, 'SQL-*'),
    ('a87018d8-8d1b-526d-8134-11e559854419', 'database', 2, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('98644c7c-beae-52d2-b844-1783316b0b6b', 'Katılımcı 1043', 'katilimci1043@example.com', 'Marmara Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 4, 'https://example.com/katilimci-1043')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '98644c7c-beae-52d2-b844-1783316b0b6b';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('98644c7c-beae-52d2-b844-1783316b0b6b', 'language', 0, 'C#'),
    ('98644c7c-beae-52d2-b844-1783316b0b6b', 'language', 1, 'JavaScript'),
    ('98644c7c-beae-52d2-b844-1783316b0b6b', 'database', 0, 'SQL Server'),
    ('98644c7c-beae-52d2-b844-1783316b0b6b', 'database', 1, 'PostgreSQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('520d8807-0e48-5a05-a244-beadb2af7161', 'Katılımcı 1044', 'katilimci1044@example.com', 'Yıldız Teknik Üniversitesi', 'Endüstri Mühendisliği', 4, 'https://example.com/katilimci-1044')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '520d8807-0e48-5a05-a244-beadb2af7161';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('520d8807-0e48-5a05-a244-beadb2af7161', 'language', 0, 'Go'),
    ('520d8807-0e48-5a05-a244-beadb2af7161', 'language', 1, 'TypeScript'),
    ('520d8807-0e48-5a05-a244-beadb2af7161', 'language', 2, 'JavaScript'),
    ('520d8807-0e48-5a05-a244-beadb2af7161', 'database', 0, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('e10e8dff-535e-5888-81ec-1c0540dda4ab', 'Katılımcı 1045', 'katilimci1045@example.com', 'Boğaziçi Üniversitesi', 'Bilgisayar Mühendisliği', 3, 'https://example.com/katilimci-1045')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'e10e8dff-535e-5888-81ec-1c0540dda4ab';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('e10e8dff-535e-5888-81ec-1c0540dda4ab', 'language', 0, 'C#'),
    ('e10e8dff-535e-5888-81ec-1c0540dda4ab', 'language', 1, 'TypeScript'),
    ('e10e8dff-535e-5888-81ec-1c0540dda4ab', 'database', 0, 'SQL-*'),
    ('e10e8dff-535e-5888-81ec-1c0540dda4ab', 'database', 1, 'MySQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('d427c697-d999-5f3f-bd50-3f7e7119f4f1', 'Katılımcı 1046', 'katilimci1046@example.com', 'Marmara Üniversitesi', 'Bilgisayar Mühendisliği', 4, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'd427c697-d999-5f3f-bd50-3f7e7119f4f1';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('d427c697-d999-5f3f-bd50-3f7e7119f4f1', 'language', 0, 'Python'),
    ('d427c697-d999-5f3f-bd50-3f7e7119f4f1', 'language', 1, 'C'),
    ('d427c697-d999-5f3f-bd50-3f7e7119f4f1', 'language', 2, 'C++'),
    ('d427c697-d999-5f3f-bd50-3f7e7119f4f1', 'database', 0, 'MongoDB'),
    ('d427c697-d999-5f3f-bd50-3f7e7119f4f1', 'database', 1, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('a9d3b5df-e456-5261-8686-8c0356eb61e6', 'Katılımcı 1047', 'katilimci1047@example.com', 'İstanbul Teknik Üniversitesi', 'Bilgisayar Mühendisliği', 2, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'a9d3b5df-e456-5261-8686-8c0356eb61e6';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('a9d3b5df-e456-5261-8686-8c0356eb61e6', 'language', 0, 'Go'),
    ('a9d3b5df-e456-5261-8686-8c0356eb61e6', 'language', 1, 'C#'),
    ('a9d3b5df-e456-5261-8686-8c0356eb61e6', 'language', 2, 'Rust'),
    ('a9d3b5df-e456-5261-8686-8c0356eb61e6', 'database', 0, 'PostgreSQL'),
    ('a9d3b5df-e456-5261-8686-8c0356eb61e6', 'database', 1, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('29088d71-995b-5394-9206-65a56f6a5444', 'Katılımcı 1048', 'katilimci1048@example.com', 'Boğaziçi Üniversitesi', 'Bilgisayar Mühendisliği', 1, 'https://example.com/katilimci-1048')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '29088d71-995b-5394-9206-65a56f6a5444';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('29088d71-995b-5394-9206-65a56f6a5444', 'language', 0, 'C#'),
    ('29088d71-995b-5394-9206-65a56f6a5444', 'language', 1, 'TypeScript'),
    ('29088d71-995b-5394-9206-65a56f6a5444', 'language', 2, 'Rust'),
    ('29088d71-995b-5394-9206-65a56f6a5444', 'database', 0, 'NoSQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('f48627b7-87ed-5855-b585-e0906479ad20', 'Katılımcı 1049', 'katilimci1049@example.com', 'Hacettepe Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 3, 'https://example.com/katilimci-1049')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'f48627b7-87ed-5855-b585-e0906479ad20';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('f48627b7-87ed-5855-b585-e0906479ad20', 'language', 0, 'JavaScript'),
    ('f48627b7-87ed-5855-b585-e0906479ad20', 'language', 1, 'Python'),
    ('f48627b7-87ed-5855-b585-e0906479ad20', 'database', 0, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('697e012a-b61a-5c2d-9326-3d853aa43a9d', 'Katılımcı 1050', 'katilimci1050@example.com', 'Hacettepe Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 4, 'https://example.com/katilimci-1050')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '697e012a-b61a-5c2d-9326-3d853aa43a9d';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('697e012a-b61a-5c2d-9326-3d853aa43a9d', 'language', 0, 'Python'),
    ('697e012a-b61a-5c2d-9326-3d853aa43a9d', 'language', 1, 'TypeScript'),
    ('697e012a-b61a-5c2d-9326-3d853aa43a9d', 'database', 0, 'SQLite'),
    ('697e012a-b61a-5c2d-9326-3d853aa43a9d', 'database', 1, 'MySQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('4ff3c957-ef07-5808-bf2a-6a0a010d11b7', 'Katılımcı 1051', 'katilimci1051@example.com', 'Orta Doğu Teknik Üniversitesi', 'Endüstri Mühendisliği', 4, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '4ff3c957-ef07-5808-bf2a-6a0a010d11b7';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('4ff3c957-ef07-5808-bf2a-6a0a010d11b7', 'language', 0, 'Kotlin'),
    ('4ff3c957-ef07-5808-bf2a-6a0a010d11b7', 'language', 1, 'TypeScript'),
    ('4ff3c957-ef07-5808-bf2a-6a0a010d11b7', 'language', 2, 'Python'),
    ('4ff3c957-ef07-5808-bf2a-6a0a010d11b7', 'database', 0, 'SQLite'),
    ('4ff3c957-ef07-5808-bf2a-6a0a010d11b7', 'database', 1, 'PostgreSQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('b02b4ee0-b5c7-5ae1-8ec0-009ffbf56575', 'Katılımcı 1052', 'katilimci1052@example.com', 'Orta Doğu Teknik Üniversitesi', 'Matematik Mühendisliği', 3, 'https://example.com/katilimci-1052')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'b02b4ee0-b5c7-5ae1-8ec0-009ffbf56575';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('b02b4ee0-b5c7-5ae1-8ec0-009ffbf56575', 'language', 0, 'C#'),
    ('b02b4ee0-b5c7-5ae1-8ec0-009ffbf56575', 'language', 1, 'Rust'),
    ('b02b4ee0-b5c7-5ae1-8ec0-009ffbf56575', 'database', 0, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('bc076adb-f4fa-5c31-b4db-9a623ec56f5b', 'Katılımcı 1053', 'katilimci1053@example.com', 'Boğaziçi Üniversitesi', 'Matematik Mühendisliği', 2, 'https://example.com/katilimci-1053')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'bc076adb-f4fa-5c31-b4db-9a623ec56f5b';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('bc076adb-f4fa-5c31-b4db-9a623ec56f5b', 'language', 0, 'C++'),
    ('bc076adb-f4fa-5c31-b4db-9a623ec56f5b', 'language', 1, 'TypeScript'),
    ('bc076adb-f4fa-5c31-b4db-9a623ec56f5b', 'language', 2, 'C#'),
    ('bc076adb-f4fa-5c31-b4db-9a623ec56f5b', 'database', 0, 'SQL-*'),
    ('bc076adb-f4fa-5c31-b4db-9a623ec56f5b', 'database', 1, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('700e8cc5-8dd6-5c6b-8d0a-e0ddc5370d6e', 'Katılımcı 1054', 'katilimci1054@example.com', 'Marmara Üniversitesi', 'Yazılım Mühendisliği', 4, 'https://example.com/katilimci-1054')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '700e8cc5-8dd6-5c6b-8d0a-e0ddc5370d6e';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('700e8cc5-8dd6-5c6b-8d0a-e0ddc5370d6e', 'language', 0, 'C#'),
    ('700e8cc5-8dd6-5c6b-8d0a-e0ddc5370d6e', 'language', 1, 'Python'),
    ('700e8cc5-8dd6-5c6b-8d0a-e0ddc5370d6e', 'database', 0, 'Redis'),
    ('700e8cc5-8dd6-5c6b-8d0a-e0ddc5370d6e', 'database', 1, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('0c65b6e1-e9f7-5110-9ea7-380c7d6da3ee', 'Katılımcı 1055', 'katilimci1055@example.com', 'Boğaziçi Üniversitesi', 'Yazılım Mühendisliği', 3, 'https://example.com/katilimci-1055')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '0c65b6e1-e9f7-5110-9ea7-380c7d6da3ee';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('0c65b6e1-e9f7-5110-9ea7-380c7d6da3ee', 'language', 0, 'Python'),
    ('0c65b6e1-e9f7-5110-9ea7-380c7d6da3ee', 'language', 1, 'TypeScript'),
    ('0c65b6e1-e9f7-5110-9ea7-380c7d6da3ee', 'database', 0, 'PostgreSQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('3a668375-4a41-5e3a-a050-69515aa925e2', 'Katılımcı 1056', 'katilimci1056@example.com', 'Dokuz Eylül Üniversitesi', 'Matematik Mühendisliği', 4, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '3a668375-4a41-5e3a-a050-69515aa925e2';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('3a668375-4a41-5e3a-a050-69515aa925e2', 'language', 0, 'JavaScript'),
    ('3a668375-4a41-5e3a-a050-69515aa925e2', 'language', 1, 'Java'),
    ('3a668375-4a41-5e3a-a050-69515aa925e2', 'database', 0, 'MySQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('096a6bb1-9832-52e6-a10d-c0e6df29170b', 'Katılımcı 1057', 'katilimci1057@example.com', 'Boğaziçi Üniversitesi', 'Matematik Mühendisliği', 1, 'https://example.com/katilimci-1057')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '096a6bb1-9832-52e6-a10d-c0e6df29170b';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('096a6bb1-9832-52e6-a10d-c0e6df29170b', 'language', 0, 'C#'),
    ('096a6bb1-9832-52e6-a10d-c0e6df29170b', 'database', 0, 'MySQL'),
    ('096a6bb1-9832-52e6-a10d-c0e6df29170b', 'database', 1, 'SQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('25141794-901b-5617-af4d-e88fb6f51fff', 'Katılımcı 1058', 'katilimci1058@example.com', 'Boğaziçi Üniversitesi', 'Matematik Mühendisliği', 2, 'https://example.com/katilimci-1058')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '25141794-901b-5617-af4d-e88fb6f51fff';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('25141794-901b-5617-af4d-e88fb6f51fff', 'language', 0, 'JavaScript'),
    ('25141794-901b-5617-af4d-e88fb6f51fff', 'language', 1, 'Python'),
    ('25141794-901b-5617-af4d-e88fb6f51fff', 'language', 2, 'Java'),
    ('25141794-901b-5617-af4d-e88fb6f51fff', 'database', 0, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('7a466552-df26-5cd1-927b-64090afd9cba', 'Katılımcı 1059', 'katilimci1059@example.com', 'Ege Üniversitesi', 'Endüstri Mühendisliği', 3, 'https://example.com/katilimci-1059')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '7a466552-df26-5cd1-927b-64090afd9cba';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('7a466552-df26-5cd1-927b-64090afd9cba', 'language', 0, 'Python'),
    ('7a466552-df26-5cd1-927b-64090afd9cba', 'language', 1, 'JavaScript'),
    ('7a466552-df26-5cd1-927b-64090afd9cba', 'database', 0, 'NoSQL-*'),
    ('7a466552-df26-5cd1-927b-64090afd9cba', 'database', 1, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('6cc0c76c-98fa-54a4-8187-58b237e71490', 'Katılımcı 1060', 'katilimci1060@example.com', 'Marmara Üniversitesi', 'Yazılım Mühendisliği', 2, 'https://example.com/katilimci-1060')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '6cc0c76c-98fa-54a4-8187-58b237e71490';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('6cc0c76c-98fa-54a4-8187-58b237e71490', 'language', 0, 'Rust'),
    ('6cc0c76c-98fa-54a4-8187-58b237e71490', 'database', 0, 'PostgreSQL'),
    ('6cc0c76c-98fa-54a4-8187-58b237e71490', 'database', 1, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('352e58ec-313d-55d2-b399-1db818fbfa26', 'Katılımcı 1061', 'katilimci1061@example.com', 'Ege Üniversitesi', 'Bilgisayar Mühendisliği', 3, 'https://example.com/katilimci-1061')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '352e58ec-313d-55d2-b399-1db818fbfa26';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('352e58ec-313d-55d2-b399-1db818fbfa26', 'language', 0, 'Java'),
    ('352e58ec-313d-55d2-b399-1db818fbfa26', 'database', 0, 'SQLite'),
    ('352e58ec-313d-55d2-b399-1db818fbfa26', 'database', 1, 'Redis');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('75981709-ef9a-5bb9-a11f-41a7cdb346e9', 'Katılımcı 1062', 'katilimci1062@example.com', 'Dokuz Eylül Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 4, 'https://example.com/katilimci-1062')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '75981709-ef9a-5bb9-a11f-41a7cdb346e9';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('75981709-ef9a-5bb9-a11f-41a7cdb346e9', 'language', 0, 'C#'),
    ('75981709-ef9a-5bb9-a11f-41a7cdb346e9', 'language', 1, 'C'),
    ('75981709-ef9a-5bb9-a11f-41a7cdb346e9', 'database', 0, 'NoSQL-*'),
    ('75981709-ef9a-5bb9-a11f-41a7cdb346e9', 'database', 1, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('2c14b2bf-6f06-5730-87bc-1b270e253aea', 'Katılımcı 1063', 'katilimci1063@example.com', 'Hacettepe Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 4, 'https://example.com/katilimci-1063')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '2c14b2bf-6f06-5730-87bc-1b270e253aea';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('2c14b2bf-6f06-5730-87bc-1b270e253aea', 'language', 0, 'JavaScript'),
    ('2c14b2bf-6f06-5730-87bc-1b270e253aea', 'database', 0, 'Redis'),
    ('2c14b2bf-6f06-5730-87bc-1b270e253aea', 'database', 1, 'SQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('0de3a470-6078-551a-9c93-a4d08973b97d', 'Katılımcı 1064', 'katilimci1064@example.com', 'İstanbul Teknik Üniversitesi', 'Bilgisayar Mühendisliği', 4, 'https://example.com/katilimci-1064')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '0de3a470-6078-551a-9c93-a4d08973b97d';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('0de3a470-6078-551a-9c93-a4d08973b97d', 'language', 0, 'Java'),
    ('0de3a470-6078-551a-9c93-a4d08973b97d', 'database', 0, 'SQL Server'),
    ('0de3a470-6078-551a-9c93-a4d08973b97d', 'database', 1, 'MySQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('e04c4716-0392-572e-8672-21a4f53a08a8', 'Katılımcı 1065', 'katilimci1065@example.com', 'İstanbul Teknik Üniversitesi', 'Yazılım Mühendisliği', 4, 'https://example.com/katilimci-1065')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'e04c4716-0392-572e-8672-21a4f53a08a8';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('e04c4716-0392-572e-8672-21a4f53a08a8', 'language', 0, 'C#'),
    ('e04c4716-0392-572e-8672-21a4f53a08a8', 'database', 0, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('8a99a4f6-8f79-5929-b780-b3eb42380a48', 'Katılımcı 1066', 'katilimci1066@example.com', 'Yıldız Teknik Üniversitesi', 'Matematik Mühendisliği', 3, 'https://example.com/katilimci-1066')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '8a99a4f6-8f79-5929-b780-b3eb42380a48';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('8a99a4f6-8f79-5929-b780-b3eb42380a48', 'language', 0, 'OCaml'),
    ('8a99a4f6-8f79-5929-b780-b3eb42380a48', 'language', 1, 'Python'),
    ('8a99a4f6-8f79-5929-b780-b3eb42380a48', 'language', 2, 'C#'),
    ('8a99a4f6-8f79-5929-b780-b3eb42380a48', 'database', 0, 'SQLite'),
    ('8a99a4f6-8f79-5929-b780-b3eb42380a48', 'database', 1, 'Redis'),
    ('8a99a4f6-8f79-5929-b780-b3eb42380a48', 'database', 2, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('0548eb9b-f864-5ef6-961e-2859929092ec', 'Katılımcı 1067', 'katilimci1067@example.com', 'Hacettepe Üniversitesi', 'Endüstri Mühendisliği', 1, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '0548eb9b-f864-5ef6-961e-2859929092ec';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('0548eb9b-f864-5ef6-961e-2859929092ec', 'language', 0, 'C#'),
    ('0548eb9b-f864-5ef6-961e-2859929092ec', 'database', 0, 'PostgreSQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('e0991099-a58a-5064-9ff1-e51aba67e094', 'Katılımcı 1068', 'katilimci1068@example.com', 'İstanbul Teknik Üniversitesi', 'Endüstri Mühendisliği', 4, 'https://example.com/katilimci-1068')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'e0991099-a58a-5064-9ff1-e51aba67e094';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('e0991099-a58a-5064-9ff1-e51aba67e094', 'language', 0, 'C#'),
    ('e0991099-a58a-5064-9ff1-e51aba67e094', 'language', 1, 'TypeScript'),
    ('e0991099-a58a-5064-9ff1-e51aba67e094', 'database', 0, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('85b79d19-f616-5288-9371-7415be7f60bd', 'Katılımcı 1069', 'katilimci1069@example.com', 'Ege Üniversitesi', 'Matematik Mühendisliği', 1, 'https://example.com/katilimci-1069')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '85b79d19-f616-5288-9371-7415be7f60bd';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('85b79d19-f616-5288-9371-7415be7f60bd', 'language', 0, 'JavaScript'),
    ('85b79d19-f616-5288-9371-7415be7f60bd', 'language', 1, 'Go'),
    ('85b79d19-f616-5288-9371-7415be7f60bd', 'database', 0, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('64ae1fa4-1233-59b5-8b16-d604156d3d3a', 'Katılımcı 1070', 'katilimci1070@example.com', 'Ege Üniversitesi', 'Bilgisayar Mühendisliği', 4, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '64ae1fa4-1233-59b5-8b16-d604156d3d3a';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('64ae1fa4-1233-59b5-8b16-d604156d3d3a', 'language', 0, 'JavaScript'),
    ('64ae1fa4-1233-59b5-8b16-d604156d3d3a', 'language', 1, 'Kotlin'),
    ('64ae1fa4-1233-59b5-8b16-d604156d3d3a', 'language', 2, 'C#'),
    ('64ae1fa4-1233-59b5-8b16-d604156d3d3a', 'database', 0, 'Redis');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('159582f5-9265-5134-b3cc-fcd3b83266f1', 'Katılımcı 1071', 'katilimci1071@example.com', 'Hacettepe Üniversitesi', 'Bilgisayar Mühendisliği', 3, 'https://example.com/katilimci-1071')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '159582f5-9265-5134-b3cc-fcd3b83266f1';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('159582f5-9265-5134-b3cc-fcd3b83266f1', 'language', 0, 'C#'),
    ('159582f5-9265-5134-b3cc-fcd3b83266f1', 'language', 1, 'C++'),
    ('159582f5-9265-5134-b3cc-fcd3b83266f1', 'language', 2, 'TypeScript'),
    ('159582f5-9265-5134-b3cc-fcd3b83266f1', 'database', 0, 'PostgreSQL'),
    ('159582f5-9265-5134-b3cc-fcd3b83266f1', 'database', 1, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('6dd42a57-7348-57e3-998e-93e125661e61', 'Katılımcı 1072', 'katilimci1072@example.com', 'Dokuz Eylül Üniversitesi', 'Matematik Mühendisliği', 4, 'https://example.com/katilimci-1072')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '6dd42a57-7348-57e3-998e-93e125661e61';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('6dd42a57-7348-57e3-998e-93e125661e61', 'language', 0, 'Go'),
    ('6dd42a57-7348-57e3-998e-93e125661e61', 'database', 0, 'PostgreSQL'),
    ('6dd42a57-7348-57e3-998e-93e125661e61', 'database', 1, 'MySQL'),
    ('6dd42a57-7348-57e3-998e-93e125661e61', 'database', 2, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('635db9ad-6ad8-5a95-a9f0-413b91ea1ca8', 'Katılımcı 1073', 'katilimci1073@example.com', 'Yıldız Teknik Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 2, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '635db9ad-6ad8-5a95-a9f0-413b91ea1ca8';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('635db9ad-6ad8-5a95-a9f0-413b91ea1ca8', 'language', 0, 'TypeScript'),
    ('635db9ad-6ad8-5a95-a9f0-413b91ea1ca8', 'language', 1, 'Python'),
    ('635db9ad-6ad8-5a95-a9f0-413b91ea1ca8', 'database', 0, 'SQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('0d0c4270-77ca-5a2a-8d03-4074a96f1f62', 'Katılımcı 1074', 'katilimci1074@example.com', 'Marmara Üniversitesi', 'Endüstri Mühendisliği', 4, 'https://example.com/katilimci-1074')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '0d0c4270-77ca-5a2a-8d03-4074a96f1f62';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('0d0c4270-77ca-5a2a-8d03-4074a96f1f62', 'language', 0, 'Python'),
    ('0d0c4270-77ca-5a2a-8d03-4074a96f1f62', 'language', 1, 'JavaScript'),
    ('0d0c4270-77ca-5a2a-8d03-4074a96f1f62', 'database', 0, 'PostgreSQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('07e39447-7978-5c27-84e7-c13043c0251d', 'Katılımcı 1075', 'katilimci1075@example.com', 'Dokuz Eylül Üniversitesi', 'Matematik Mühendisliği', 2, 'https://example.com/katilimci-1075')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '07e39447-7978-5c27-84e7-c13043c0251d';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('07e39447-7978-5c27-84e7-c13043c0251d', 'language', 0, 'TypeScript'),
    ('07e39447-7978-5c27-84e7-c13043c0251d', 'database', 0, 'SQLite'),
    ('07e39447-7978-5c27-84e7-c13043c0251d', 'database', 1, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('acfeafb4-defc-5200-b3a8-8cc3c04de556', 'Katılımcı 1076', 'katilimci1076@example.com', 'Hacettepe Üniversitesi', 'Bilgisayar Mühendisliği', 4, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'acfeafb4-defc-5200-b3a8-8cc3c04de556';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('acfeafb4-defc-5200-b3a8-8cc3c04de556', 'language', 0, 'Go'),
    ('acfeafb4-defc-5200-b3a8-8cc3c04de556', 'database', 0, 'Redis'),
    ('acfeafb4-defc-5200-b3a8-8cc3c04de556', 'database', 1, 'SQL-*'),
    ('acfeafb4-defc-5200-b3a8-8cc3c04de556', 'database', 2, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('9143da04-172e-5bfe-8786-b6bd3c429b8e', 'Katılımcı 1077', 'katilimci1077@example.com', 'Boğaziçi Üniversitesi', 'Endüstri Mühendisliği', 3, 'https://example.com/katilimci-1077')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '9143da04-172e-5bfe-8786-b6bd3c429b8e';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('9143da04-172e-5bfe-8786-b6bd3c429b8e', 'language', 0, 'C'),
    ('9143da04-172e-5bfe-8786-b6bd3c429b8e', 'database', 0, 'MySQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('69d47d9a-2fa5-56e6-96e5-034abf1c0ea9', 'Katılımcı 1078', 'katilimci1078@example.com', 'Hacettepe Üniversitesi', 'Bilgisayar Mühendisliği', 4, 'https://example.com/katilimci-1078')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '69d47d9a-2fa5-56e6-96e5-034abf1c0ea9';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('69d47d9a-2fa5-56e6-96e5-034abf1c0ea9', 'language', 0, 'TypeScript'),
    ('69d47d9a-2fa5-56e6-96e5-034abf1c0ea9', 'language', 1, 'Python'),
    ('69d47d9a-2fa5-56e6-96e5-034abf1c0ea9', 'database', 0, 'SQLite'),
    ('69d47d9a-2fa5-56e6-96e5-034abf1c0ea9', 'database', 1, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('efb27a79-14be-5929-be63-dccfb9c05b61', 'Katılımcı 1079', 'katilimci1079@example.com', 'Boğaziçi Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 4, 'https://example.com/katilimci-1079')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'efb27a79-14be-5929-be63-dccfb9c05b61';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('efb27a79-14be-5929-be63-dccfb9c05b61', 'language', 0, 'C#'),
    ('efb27a79-14be-5929-be63-dccfb9c05b61', 'database', 0, 'PostgreSQL'),
    ('efb27a79-14be-5929-be63-dccfb9c05b61', 'database', 1, 'MongoDB'),
    ('efb27a79-14be-5929-be63-dccfb9c05b61', 'database', 2, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('67fe6fb8-ceb1-5815-8d02-fb3067e73ba3', 'Katılımcı 1080', 'katilimci1080@example.com', 'Dokuz Eylül Üniversitesi', 'Bilgisayar Mühendisliği', 1, 'https://example.com/katilimci-1080')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '67fe6fb8-ceb1-5815-8d02-fb3067e73ba3';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('67fe6fb8-ceb1-5815-8d02-fb3067e73ba3', 'language', 0, 'JavaScript'),
    ('67fe6fb8-ceb1-5815-8d02-fb3067e73ba3', 'database', 0, 'NoSQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('9b9eb518-81a0-5297-9e8c-e6809c525dd8', 'Katılımcı 1081', 'katilimci1081@example.com', 'Boğaziçi Üniversitesi', 'Endüstri Mühendisliği', 4, 'https://example.com/katilimci-1081')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '9b9eb518-81a0-5297-9e8c-e6809c525dd8';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('9b9eb518-81a0-5297-9e8c-e6809c525dd8', 'language', 0, 'TypeScript'),
    ('9b9eb518-81a0-5297-9e8c-e6809c525dd8', 'database', 0, 'SQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('9e4718b3-31c1-5852-9594-355bfc71a797', 'Katılımcı 1082', 'katilimci1082@example.com', 'Ege Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 3, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '9e4718b3-31c1-5852-9594-355bfc71a797';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('9e4718b3-31c1-5852-9594-355bfc71a797', 'language', 0, 'Go'),
    ('9e4718b3-31c1-5852-9594-355bfc71a797', 'language', 1, 'Python'),
    ('9e4718b3-31c1-5852-9594-355bfc71a797', 'database', 0, 'MongoDB'),
    ('9e4718b3-31c1-5852-9594-355bfc71a797', 'database', 1, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('042b2308-4c18-5aaa-9dd1-77797344d351', 'Katılımcı 1083', 'katilimci1083@example.com', 'İstanbul Teknik Üniversitesi', 'Yazılım Mühendisliği', 2, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '042b2308-4c18-5aaa-9dd1-77797344d351';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('042b2308-4c18-5aaa-9dd1-77797344d351', 'language', 0, 'Rust'),
    ('042b2308-4c18-5aaa-9dd1-77797344d351', 'language', 1, 'C++'),
    ('042b2308-4c18-5aaa-9dd1-77797344d351', 'database', 0, 'SQL-*'),
    ('042b2308-4c18-5aaa-9dd1-77797344d351', 'database', 1, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('e3f5d261-51d2-5655-a719-b4e608356b82', 'Katılımcı 1084', 'katilimci1084@example.com', 'Ege Üniversitesi', 'Yazılım Mühendisliği', 4, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'e3f5d261-51d2-5655-a719-b4e608356b82';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('e3f5d261-51d2-5655-a719-b4e608356b82', 'language', 0, 'Python'),
    ('e3f5d261-51d2-5655-a719-b4e608356b82', 'language', 1, 'Rust'),
    ('e3f5d261-51d2-5655-a719-b4e608356b82', 'database', 0, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('8ec0fe46-8bd1-5328-8258-fcd3b5e0a2f9', 'Katılımcı 1085', 'katilimci1085@example.com', 'Dokuz Eylül Üniversitesi', 'Yazılım Mühendisliği', 2, 'https://example.com/katilimci-1085')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '8ec0fe46-8bd1-5328-8258-fcd3b5e0a2f9';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('8ec0fe46-8bd1-5328-8258-fcd3b5e0a2f9', 'language', 0, 'Rust'),
    ('8ec0fe46-8bd1-5328-8258-fcd3b5e0a2f9', 'language', 1, 'C'),
    ('8ec0fe46-8bd1-5328-8258-fcd3b5e0a2f9', 'database', 0, 'SQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('a5a0382a-fdbc-57bd-bc7e-2ce6b528df24', 'Katılımcı 1086', 'katilimci1086@example.com', 'Ege Üniversitesi', 'Endüstri Mühendisliği', 4, 'https://example.com/katilimci-1086')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'a5a0382a-fdbc-57bd-bc7e-2ce6b528df24';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('a5a0382a-fdbc-57bd-bc7e-2ce6b528df24', 'language', 0, 'Rust'),
    ('a5a0382a-fdbc-57bd-bc7e-2ce6b528df24', 'language', 1, 'C'),
    ('a5a0382a-fdbc-57bd-bc7e-2ce6b528df24', 'database', 0, 'MongoDB'),
    ('a5a0382a-fdbc-57bd-bc7e-2ce6b528df24', 'database', 1, 'SQL-*');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('36459ce5-120e-5c37-9794-7cac2524410c', 'Katılımcı 1087', 'katilimci1087@example.com', 'Orta Doğu Teknik Üniversitesi', 'Matematik Mühendisliği', 3, 'https://example.com/katilimci-1087')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '36459ce5-120e-5c37-9794-7cac2524410c';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('36459ce5-120e-5c37-9794-7cac2524410c', 'language', 0, 'Python'),
    ('36459ce5-120e-5c37-9794-7cac2524410c', 'language', 1, 'JavaScript'),
    ('36459ce5-120e-5c37-9794-7cac2524410c', 'language', 2, 'C#'),
    ('36459ce5-120e-5c37-9794-7cac2524410c', 'database', 0, 'SQL Server'),
    ('36459ce5-120e-5c37-9794-7cac2524410c', 'database', 1, 'MySQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('e169b1a3-beaa-56b6-8ee5-fe4297ffd97c', 'Katılımcı 1088', 'katilimci1088@example.com', 'Marmara Üniversitesi', 'Matematik Mühendisliği', 4, 'https://example.com/katilimci-1088')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'e169b1a3-beaa-56b6-8ee5-fe4297ffd97c';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('e169b1a3-beaa-56b6-8ee5-fe4297ffd97c', 'language', 0, 'Rust'),
    ('e169b1a3-beaa-56b6-8ee5-fe4297ffd97c', 'language', 1, 'Java'),
    ('e169b1a3-beaa-56b6-8ee5-fe4297ffd97c', 'database', 0, 'PostgreSQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('2ecb696b-e823-5e71-bd89-113c3bea633d', 'Katılımcı 1089', 'katilimci1089@example.com', 'Boğaziçi Üniversitesi', 'Endüstri Mühendisliği', 3, 'https://example.com/katilimci-1089')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '2ecb696b-e823-5e71-bd89-113c3bea633d';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('2ecb696b-e823-5e71-bd89-113c3bea633d', 'language', 0, 'TypeScript'),
    ('2ecb696b-e823-5e71-bd89-113c3bea633d', 'language', 1, 'Kotlin'),
    ('2ecb696b-e823-5e71-bd89-113c3bea633d', 'database', 0, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('a545a4af-cdc6-50d2-ba24-b7b9a416a92c', 'Katılımcı 1090', 'katilimci1090@example.com', 'Yıldız Teknik Üniversitesi', 'Endüstri Mühendisliği', 2, 'https://example.com/katilimci-1090')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'a545a4af-cdc6-50d2-ba24-b7b9a416a92c';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('a545a4af-cdc6-50d2-ba24-b7b9a416a92c', 'language', 0, 'C#'),
    ('a545a4af-cdc6-50d2-ba24-b7b9a416a92c', 'language', 1, 'Java'),
    ('a545a4af-cdc6-50d2-ba24-b7b9a416a92c', 'database', 0, 'MySQL'),
    ('a545a4af-cdc6-50d2-ba24-b7b9a416a92c', 'database', 1, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('f748f5d4-cd1a-5f9e-b505-94df90b6665b', 'Katılımcı 1091', 'katilimci1091@example.com', 'Marmara Üniversitesi', 'Matematik Mühendisliği', 4, 'https://example.com/katilimci-1091')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'f748f5d4-cd1a-5f9e-b505-94df90b6665b';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('f748f5d4-cd1a-5f9e-b505-94df90b6665b', 'language', 0, 'C#'),
    ('f748f5d4-cd1a-5f9e-b505-94df90b6665b', 'language', 1, 'Python'),
    ('f748f5d4-cd1a-5f9e-b505-94df90b6665b', 'database', 0, 'SQL Server');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('90f038e8-8d33-5a67-8e80-993d55b6a182', 'Katılımcı 1092', 'katilimci1092@example.com', 'Ege Üniversitesi', 'Yazılım Mühendisliği', 2, 'https://example.com/katilimci-1092')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '90f038e8-8d33-5a67-8e80-993d55b6a182';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('90f038e8-8d33-5a67-8e80-993d55b6a182', 'language', 0, 'JavaScript'),
    ('90f038e8-8d33-5a67-8e80-993d55b6a182', 'language', 1, 'Kotlin'),
    ('90f038e8-8d33-5a67-8e80-993d55b6a182', 'database', 0, 'PostgreSQL'),
    ('90f038e8-8d33-5a67-8e80-993d55b6a182', 'database', 1, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('6269a730-b9e9-5631-b52b-ca98bea97f73', 'Katılımcı 1093', 'katilimci1093@example.com', 'Ege Üniversitesi', 'Bilgisayar Mühendisliği', 3, 'https://example.com/katilimci-1093')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '6269a730-b9e9-5631-b52b-ca98bea97f73';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('6269a730-b9e9-5631-b52b-ca98bea97f73', 'language', 0, 'Python'),
    ('6269a730-b9e9-5631-b52b-ca98bea97f73', 'language', 1, 'C#'),
    ('6269a730-b9e9-5631-b52b-ca98bea97f73', 'database', 0, 'PostgreSQL'),
    ('6269a730-b9e9-5631-b52b-ca98bea97f73', 'database', 1, 'SQLite'),
    ('6269a730-b9e9-5631-b52b-ca98bea97f73', 'database', 2, 'Redis');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('0fa03120-ef00-5cfa-be90-ab551fc9207d', 'Katılımcı 1094', 'katilimci1094@example.com', 'Ege Üniversitesi', 'Bilgisayar Mühendisliği', 4, 'https://example.com/katilimci-1094')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '0fa03120-ef00-5cfa-be90-ab551fc9207d';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('0fa03120-ef00-5cfa-be90-ab551fc9207d', 'language', 0, 'C++'),
    ('0fa03120-ef00-5cfa-be90-ab551fc9207d', 'language', 1, 'Python'),
    ('0fa03120-ef00-5cfa-be90-ab551fc9207d', 'database', 0, 'MongoDB'),
    ('0fa03120-ef00-5cfa-be90-ab551fc9207d', 'database', 1, 'MySQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('2c0c84c2-8621-5678-8458-f26559fe43fe', 'Katılımcı 1095', 'katilimci1095@example.com', 'Orta Doğu Teknik Üniversitesi', 'Yazılım Mühendisliği', 4, 'https://example.com/katilimci-1095')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '2c0c84c2-8621-5678-8458-f26559fe43fe';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('2c0c84c2-8621-5678-8458-f26559fe43fe', 'language', 0, 'C#'),
    ('2c0c84c2-8621-5678-8458-f26559fe43fe', 'language', 1, 'JavaScript'),
    ('2c0c84c2-8621-5678-8458-f26559fe43fe', 'database', 0, 'MongoDB');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('b5848424-9192-5360-a0a9-d18bfbd235cf', 'Katılımcı 1096', 'katilimci1096@example.com', 'Orta Doğu Teknik Üniversitesi', 'Yazılım Mühendisliği', 4, 'https://example.com/katilimci-1096')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = 'b5848424-9192-5360-a0a9-d18bfbd235cf';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('b5848424-9192-5360-a0a9-d18bfbd235cf', 'language', 0, 'TypeScript'),
    ('b5848424-9192-5360-a0a9-d18bfbd235cf', 'language', 1, 'C#'),
    ('b5848424-9192-5360-a0a9-d18bfbd235cf', 'language', 2, 'Java'),
    ('b5848424-9192-5360-a0a9-d18bfbd235cf', 'database', 0, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('3c5ef98a-8cb3-555e-84f0-1df2b5a52edb', 'Katılımcı 1097', 'katilimci1097@example.com', 'İstanbul Teknik Üniversitesi', 'Yazılım Mühendisliği', 4, 'https://example.com/katilimci-1097')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '3c5ef98a-8cb3-555e-84f0-1df2b5a52edb';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('3c5ef98a-8cb3-555e-84f0-1df2b5a52edb', 'language', 0, 'Python'),
    ('3c5ef98a-8cb3-555e-84f0-1df2b5a52edb', 'language', 1, 'Go'),
    ('3c5ef98a-8cb3-555e-84f0-1df2b5a52edb', 'database', 0, 'MySQL'),
    ('3c5ef98a-8cb3-555e-84f0-1df2b5a52edb', 'database', 1, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('29d15573-be4c-5e8b-8e4d-e451d5362418', 'Katılımcı 1098', 'katilimci1098@example.com', 'Ege Üniversitesi', 'Bilgisayar Mühendisliği', 4, 'https://example.com/katilimci-1098')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '29d15573-be4c-5e8b-8e4d-e451d5362418';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('29d15573-be4c-5e8b-8e4d-e451d5362418', 'language', 0, 'Kotlin'),
    ('29d15573-be4c-5e8b-8e4d-e451d5362418', 'language', 1, 'Rust'),
    ('29d15573-be4c-5e8b-8e4d-e451d5362418', 'language', 2, 'Python'),
    ('29d15573-be4c-5e8b-8e4d-e451d5362418', 'database', 0, 'SQLite');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('2eacc77a-3ea2-569a-801c-0c4368e5f5b4', 'Katılımcı 1099', 'katilimci1099@example.com', 'Hacettepe Üniversitesi', 'Elektrik-Elektronik Mühendisliği', 4, 'https://example.com/katilimci-1099')
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '2eacc77a-3ea2-569a-801c-0c4368e5f5b4';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('2eacc77a-3ea2-569a-801c-0c4368e5f5b4', 'language', 0, 'JavaScript'),
    ('2eacc77a-3ea2-569a-801c-0c4368e5f5b4', 'database', 0, 'PostgreSQL');

INSERT INTO participants (participant_id, full_name, email, university, department, class, github_url)
VALUES ('6b255866-5d6e-551c-a702-db59db7ee7dd', 'Katılımcı 1100', 'katilimci1100@example.com', 'Hacettepe Üniversitesi', 'Endüstri Mühendisliği', 3, NULL)
ON CONFLICT (participant_id) DO UPDATE
SET full_name = excluded.full_name, email = excluded.email, university = excluded.university,
    department = excluded.department, class = excluded.class, github_url = excluded.github_url;
DELETE FROM participant_preferences WHERE participant_id = '6b255866-5d6e-551c-a702-db59db7ee7dd';
INSERT INTO participant_preferences (participant_id, kind, rank, name) VALUES
    ('6b255866-5d6e-551c-a702-db59db7ee7dd', 'language', 0, 'OCaml'),
    ('6b255866-5d6e-551c-a702-db59db7ee7dd', 'language', 1, 'Rust'),
    ('6b255866-5d6e-551c-a702-db59db7ee7dd', 'database', 0, 'PostgreSQL');

-- Turnuva: tüm proje fikirleri + 100 katılımcı, henüz dağıtılmamış
INSERT INTO competitions (competition_id, title, session, settled_at)
VALUES ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'Test Turnuvası', '2026-27', NULL)
ON CONFLICT (competition_id) DO UPDATE
SET title = excluded.title, session = excluded.session, settled_at = NULL;

DELETE FROM settlements WHERE competition_id = 'd31fa4ae-ecf6-5a38-a549-e8ae92e14cbd';
DELETE FROM competition_projects WHERE competition_id = 'd31fa4ae-ecf6-5a38-a549-e8ae92e14cbd';
DELETE FROM competition_participants WHERE competition_id = 'd31fa4ae-ecf6-5a38-a549-e8ae92e14cbd';

INSERT INTO competition_projects (competition_id, project_id) VALUES
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '4ae93a95-259b-5e0c-8107-366062bc0b46'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'eb1437e9-b2f8-5960-9cf1-a8d84459d6d4'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '81ac748c-4fa3-5585-b5db-a1ceca807281'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '8cdb40b4-56e8-51be-ad7e-201a2acd3905'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '9b247339-6cdb-5bf5-b541-23dcff541104'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '8127cff7-c15b-5cb8-8792-196f17dfe3bd'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '340a48ad-6814-5beb-ad9c-3e3db50ff25c'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'eb6a172c-c174-5fc1-882b-afbc06555cdf'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'a852ba56-8843-55b9-942b-2d55112773f6'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'f3f6530f-ce5e-5857-85c6-7bb71a6f5c5d'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '2db61a1f-bbff-52c3-9b4e-27357431781b'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '79cfb6de-1a95-534b-90b9-25f56d30c666'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'e1912a08-8fed-55e6-85b6-c1f697420d0a'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'a34615f5-1ac6-5caf-9050-232699d3385a'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '3aa269fa-ee86-52b0-8a5f-b71843162e07'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'b8d0f40d-fd92-56a6-9d0a-a9a7f9e87f26'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'cb31301b-3cba-5779-b1b6-9aa11c9ae4de'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '526a6bb7-77e3-5aa2-85d9-9336db910582'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '8aeb3a14-e793-55c3-aaed-3aa062179ed4'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'aef238f5-4b11-5954-9adf-e43fb8d4e3c4'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'd7977ab8-c372-5f57-b740-326ec3fe5548'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'f62381ce-a952-512c-8fb1-f1742f0cf517'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '47ff4585-5435-5d7e-ad33-64073f5793c1'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'db17d934-22bc-59cb-b0dc-af7c1738d6b7'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '687cf59e-f88b-5a5f-93fe-d91eb95aa2b4');

INSERT INTO competition_participants (competition_id, participant_id) VALUES
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '9286ef93-cb61-54de-a42c-b620bd006e9e'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'cf5e2dda-76b2-5f19-a45d-1f4302d990b2'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '2393a8dc-f861-59f0-867d-7218749051d8'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '7e5b2b92-bc01-5a19-baee-d3038117c202'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '4f8a254e-d118-52b7-9da6-42bcda2e3c70'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'a417251a-5d98-5a65-9bc2-316623144ade'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'a01d58dd-59d3-588b-bd0d-89f505e9b5cd'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '8fbcc16d-3784-57e1-8475-4064e7a782d7'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '57149a7a-74f2-585c-9602-2d063c8c5ae0'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '0f05f23a-7cac-5c4d-b54c-af54f20f4663'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '0ea7f5a5-1db1-57e2-8cc3-638c18ed2435'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'b802df1e-c9fc-5538-9828-ebc6d7ab4d26'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '30403342-8ca0-52c7-a1c5-c67a738b8559'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'a20a4604-3b3f-57aa-a6b6-b5d5eb6decc1'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'de2cd6bd-abd4-557f-8974-cc217289cbb1'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '499af19b-0df8-5ef0-a5dd-3d8e25e1d0f7'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'ab9cdb0d-6db6-5d5d-8302-812206a25846'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'ba3633a2-0958-52eb-88c2-007f3afa7649'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '1ee1a35d-0151-52c6-89fd-16ef8e79dfb0'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '33263a1b-cc93-5ce6-9239-7849d1fae804'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '6446948a-3ce3-536c-b35c-9f552ea27299'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '01274473-9f9a-5cc3-89f0-b7b1acfc165b'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '1adb9e27-7052-53d6-86d7-9d82614c2c14'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '9f742322-c7e8-5cc7-adbc-049d07165391'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '20c787e8-9782-5d7e-83bd-ec5b7fe09e26'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'd05c242c-3d0f-5d37-b4d5-4e49e9153f2a'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '2abaf229-e076-53c1-a9b4-76dc240bde57'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '0dc64a9d-3b8f-5a9a-984e-18bc739d8fd3'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '3e9579fe-27f5-5940-bb8d-cbeadcfc7763'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '007463c9-f041-5ad4-a568-324f90201b21'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'd2d7c091-b785-5a93-918a-ad2b63a6ccd0'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'b5a44ba2-8806-5144-81e3-d3625601a77e'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'a01b5591-4e0b-52a3-b6f3-b78bda04c62c'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '7d8c54e8-cf0c-5ca4-9d2e-6275f57880d0'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '1dd0f248-4e39-5ada-985b-f640fac3f9ad'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'a1cad683-58c2-5fb3-bf96-257c710d4a2d'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '2e7a4e33-88fa-5c90-9609-6be14efec259'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '1a6b0adc-8e1f-50c1-8a02-a8d9dc92b9b5'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '80834d66-2ebd-5cdf-a8e5-ca0a46656490'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '326d358b-0659-5d07-b5c7-bbc052e8997a'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '38dab00d-9990-5ff4-afe2-0504f1078be7'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'a87018d8-8d1b-526d-8134-11e559854419'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '98644c7c-beae-52d2-b844-1783316b0b6b'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '520d8807-0e48-5a05-a244-beadb2af7161'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'e10e8dff-535e-5888-81ec-1c0540dda4ab'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'd427c697-d999-5f3f-bd50-3f7e7119f4f1'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'a9d3b5df-e456-5261-8686-8c0356eb61e6'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '29088d71-995b-5394-9206-65a56f6a5444'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'f48627b7-87ed-5855-b585-e0906479ad20'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '697e012a-b61a-5c2d-9326-3d853aa43a9d'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '4ff3c957-ef07-5808-bf2a-6a0a010d11b7'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'b02b4ee0-b5c7-5ae1-8ec0-009ffbf56575'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'bc076adb-f4fa-5c31-b4db-9a623ec56f5b'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '700e8cc5-8dd6-5c6b-8d0a-e0ddc5370d6e'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '0c65b6e1-e9f7-5110-9ea7-380c7d6da3ee'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '3a668375-4a41-5e3a-a050-69515aa925e2'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '096a6bb1-9832-52e6-a10d-c0e6df29170b'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '25141794-901b-5617-af4d-e88fb6f51fff'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '7a466552-df26-5cd1-927b-64090afd9cba'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '6cc0c76c-98fa-54a4-8187-58b237e71490'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '352e58ec-313d-55d2-b399-1db818fbfa26'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '75981709-ef9a-5bb9-a11f-41a7cdb346e9'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '2c14b2bf-6f06-5730-87bc-1b270e253aea'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '0de3a470-6078-551a-9c93-a4d08973b97d'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'e04c4716-0392-572e-8672-21a4f53a08a8'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '8a99a4f6-8f79-5929-b780-b3eb42380a48'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '0548eb9b-f864-5ef6-961e-2859929092ec'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'e0991099-a58a-5064-9ff1-e51aba67e094'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '85b79d19-f616-5288-9371-7415be7f60bd'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '64ae1fa4-1233-59b5-8b16-d604156d3d3a'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '159582f5-9265-5134-b3cc-fcd3b83266f1'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '6dd42a57-7348-57e3-998e-93e125661e61'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '635db9ad-6ad8-5a95-a9f0-413b91ea1ca8'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '0d0c4270-77ca-5a2a-8d03-4074a96f1f62'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '07e39447-7978-5c27-84e7-c13043c0251d'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'acfeafb4-defc-5200-b3a8-8cc3c04de556'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '9143da04-172e-5bfe-8786-b6bd3c429b8e'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '69d47d9a-2fa5-56e6-96e5-034abf1c0ea9'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'efb27a79-14be-5929-be63-dccfb9c05b61'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '67fe6fb8-ceb1-5815-8d02-fb3067e73ba3'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '9b9eb518-81a0-5297-9e8c-e6809c525dd8'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '9e4718b3-31c1-5852-9594-355bfc71a797'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '042b2308-4c18-5aaa-9dd1-77797344d351'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'e3f5d261-51d2-5655-a719-b4e608356b82'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '8ec0fe46-8bd1-5328-8258-fcd3b5e0a2f9'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'a5a0382a-fdbc-57bd-bc7e-2ce6b528df24'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '36459ce5-120e-5c37-9794-7cac2524410c'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'e169b1a3-beaa-56b6-8ee5-fe4297ffd97c'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '2ecb696b-e823-5e71-bd89-113c3bea633d'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'a545a4af-cdc6-50d2-ba24-b7b9a416a92c'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'f748f5d4-cd1a-5f9e-b505-94df90b6665b'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '90f038e8-8d33-5a67-8e80-993d55b6a182'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '6269a730-b9e9-5631-b52b-ca98bea97f73'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '0fa03120-ef00-5cfa-be90-ab551fc9207d'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '2c0c84c2-8621-5678-8458-f26559fe43fe'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', 'b5848424-9192-5360-a0a9-d18bfbd235cf'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '3c5ef98a-8cb3-555e-84f0-1df2b5a52edb'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '29d15573-be4c-5e8b-8e4d-e451d5362418'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '2eacc77a-3ea2-569a-801c-0c4368e5f5b4'),
    ('d31fa4ae-ecf6-5a38-a549-e8ae92e14cbd', '6b255866-5d6e-551c-a702-db59db7ee7dd');

COMMIT;
