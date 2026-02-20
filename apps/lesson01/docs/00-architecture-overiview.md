# Mimari Doküman

Projenin mimari özellikleri aşağıdaki gibidir.

## Backend

Backend tarafı aşağıdaki teknolojileri kullanır.

|Konu|Teknoloji|Açıklama|
|-----|---------|-----------|
|Backend Mimarisi|Clean Architecture|Backend uygulamasında clean architecture yaklaşımı kullanılır.|
|Framework|.Net 10|Microsoft .Net 10 kullanılır|
|Programlama Dili|C#||
|Veritabanı|MongoDb|Dokükman bazlı CV'lerin tutulması için tercih edilmiştir.|
|Yaklaşım|Domain Driven Design|Detaylar [domain-design](01-domain-design.md) dokümanında yer alır.|
|Client|API Gateway|Frontend uygulamasının yürütmek istediği business logic'ler için geldiği REST Api uygulaması.|

Mutlaka uyulması gereken prensipler

- SOLID ilkeleri
- Clean Code prensipleri

## FrontEnd

Önyüz uygulaması aşağıdaki gibidir.

- Rozer Based Pages üzerine inşa edilir.
- C# ve .Net 10 kullanılır.
- Tasarım için Bootstrap kütüphanesi kullanılır.
- Backend uygulaması ile REST Api üzerinen haberleşir.
