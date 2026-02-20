using CvApp.Domain.Enums;

namespace CvApp.Domain.Entities;

/// <summary>
/// İletişim bilgilerini tutan Entity. Bir User'ın birden fazla Contact bilgisi olabilir (one-to-many).
/// </summary>
public class Contact
{
    public Guid ContactId { get; private set; }

    /// <summary>
    /// İletişim türü. ContactType enum değerlerinden biri olmalıdır.
    /// </summary>
    public ContactType Kind { get; private set; }

    /// <summary>
    /// Bu iletişim bilgisinin sahibi olan UserId değeridir.
    /// </summary>
    public Guid RelatedUser { get; private set; }

    /// <summary>
    /// Kind değerine göre iletişim bilgisinin içeriği (e-posta, telefon, adres, URL vb.).
    /// </summary>
    public string Value { get; private set; } = string.Empty;

    private Contact() { }

    public static Contact Create(ContactType kind, Guid relatedUserId, string value)
    {
        ValidateValue(kind, value);

        return new Contact
        {
            ContactId = Guid.NewGuid(),
            Kind = kind,
            RelatedUser = relatedUserId,
            Value = value
        };
    }

    public void Update(ContactType kind, string value)
    {
        ValidateValue(kind, value);
        Kind = kind;
        Value = value;
    }

    private static void ValidateValue(ContactType kind, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("İletişim değeri boş olamaz.", nameof(value));

        switch (kind)
        {
            //TODO: Email doğrulaması için Regex veya MailAddress sınıfı kullanılabilir.
            case ContactType.Email:
                if (!value.Contains('@') || !value.Contains('.'))
                    throw new ArgumentException("Geçerli bir e-posta adresi giriniz.", nameof(value));
                break;

            case ContactType.SocialUrl:
                if (!Uri.TryCreate(value, UriKind.Absolute, out _))
                    throw new ArgumentException("Sosyal ağ bilgisi geçerli bir URL formatında olmalıdır.", nameof(value));
                break;
        }
    }
}
