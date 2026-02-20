# Domain Design

Projede yer alan aktörlere ait domain kurgusu aşağıda özetlenmiştir.

## User *(Entity)*

Sisteme kayıt olan kullanıcıyı temsil eder.

|Field|Type|Açıklama|Örnek|Kural|
|----|----|---------|-----|-----|
|UserId|Guid|Unique/Identity alanıdır|5C98741B-64C8-49DE-9E68-3D7A2F44802B|Tekrar etmemelidir, benzersiz olmalıdır|
|Fullname|Text|Kullanıcı adı ve soyadından oluşur|Can Cey Rambo|Minimum 10 karakter maksimum 100 karakter|

## Contact *(Entity)*

İletişim bilgilerini tutan Entity nesnesidir. Bir User'ın birden fazla Contact bilgisi olabilir. Arada bire-çok *(one to many)* ilişki vardır.

|Field|Type|Açıklama|Örnek|Kural|
|----|----|---------|-----|-----|
|ContactId|Guid|Unique/Identity alanıdır|D4816D0A-CD8E-4442-98C0-65D3BA11BE70|Tekrar etmemelidir, benzersiz olmalıdır|
|Kind|[ContactType](#contacttype-enum) değerlerinden birisi olabilir|Türe göre kurallar içerir. Örneğin email geçerli formatta olmalıdır, Social Network bilgisi geçerli URL formatında olmalıdır|||
|RelatedUser|Guid|Bu iletişim bilgisinin sahibi olan UserId değeridir|5C98741B-64C8-49DE-9E68-3D7A2F44802B||
|Value|Text|Kind değerine göre iletişim bilgisinin içeriğidir|noone@nowhere.org, +41 111 111 11 11 vb||

## Resume *(Aggregate)*

CV'nin detaylarını taşır. User, Contact, Skills vb bilgileri içerir.

|Field|Type|Açıklama|Örnek|Kural|
|----|----|---------|-----|-----|
|ResumeId|Guid|Unique/Identity alanıdır|82FB0397-862D-46A0-B562-D180179B0565||
|User|User|Bağlı olduğu UserId bilgisidir|5C98741B-64C8-49DE-9E68-3D7A2F44802B||
|Contact|Contact Array|Bu Resume sahibi olan User nesnesine bağlı Contact bilgilerini içerir|||

## ContactType *(Enum)*

Bir iletişim bilgisinin türünü ifade eder. Sadece aşağıdaki değerlerden birisi olabilir.

- Email
- Phone
- PostalAddress
- SocialUrl
